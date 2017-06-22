using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Globalization;
using System.Security;
using System.Text;

namespace NDK.Framework {

	#region UserQuery enum.
	/// <summary>
	/// Filter used by the GetAllUsers methods.
	/// </summary>
	public enum UserQuery {
		// Simple queries.
		/// <summary>
		/// Get all enabled users.
		/// </summary>
		ENABLED = 1,

		/// <summary>
		/// Get all disabled users.
		/// </summary>
		DISABLED = 2,

		/// <summary>
		/// Get all users, where the password newer expires.
		/// </summary>
		PASSWORD_NEWER_EXPIRES = 3,

		/// <summary>
		/// Get all users, where the password expire at some time.
		/// </summary>
		PASSWORD_EXPIRES = 4,

		/// <summary>
		/// Get all users, where a password is not required.
		/// </summary>
		PASSWORD_NOT_REQUIRED = 5,

		/// <summary>
		/// Get all users, where a password is required.
		/// </summary>
		PASSWORD_REQUIRED = 6,

		/// <summary>
		/// Get all users, who can change their own password.
		/// This query takes long time to perform, because all users are queried and then filtered.
		/// </summary>
		PASSWORD_CHANGE_ENABLED = 7,

		/// <summary>
		/// Get all users, who can not change their own password.
		/// This query takes long time to perform, because all users are queried and then filtered.
		/// </summary>
		PASSWORD_CHANGE_DISABLED = 8,

		// Advanced queries.
		/// <summary>
		/// Get all users, where the account is not expired.
		/// Use the GetAllUsers method with the advancedUserFilterDays parameter, to specify a time before or after now.
		/// </summary>
		ACCOUNT_NOT_EXPIRED = 101,

		/// <summary>
		/// Get all users, where the account is expired.
		/// </summary>
		ACCOUNT_EXPIRED = 102,

		/// <summary>
		/// Gets all users, where the account is not locked out.
		/// </summary>
		ACCOUNT_NOT_LOCKED_OUT = 103,

		/// <summary>
		/// Gets all users, where the account is locked out.
		/// Use the GetAllUsers method with the advancedUserFilterDays parameter, to specify a time before or after now.
		/// </summary>
		ACCOUNT_LOCKED_OUT = 104,

		// Default query.
		/// <summary>
		/// Get all users.
		/// </summary>
		ALL = 0
	} // UserQuery
	#endregion

	#region AdUtility class.
	public static class AdUtility {

		/// <summary>
		/// Gets the domain names in the current forest.
		/// </summary>
		public static String[] GetDomainNames() {
			try {
				// Get the domains in the current forest.
				List<String> domainNames = new List<String>();
				Forest forest = Forest.GetCurrentForest();
				DomainCollection domains = forest.Domains;
				foreach (Domain domain in domains) {
					domainNames.Add(domain.Name);
				}

				// Return the found domain names.
				return domainNames.ToArray();
			} catch {
				return new String[0];
			}
		} // GetDomainNames

		/// <summary>
		/// Gets a random domain name in the current forest.
		/// </summary>
		public static String GetDomainName() {
			try {
				Random random = new Random();
				String[] domainNames = AdUtility.GetDomainNames();
				return domainNames[random.Next(0, domainNames.Length - 1)];
			} catch {
				return String.Empty;
			}
		} // GetDomainName

		public static PrincipalContext GetContext() {
			// Convert the domain name to the distinguished name.
			String domainName = AdUtility.GetDomainName();
			String distinguishedName = "DC=" + domainName.Replace(".", ",DC=");    // "DC=intern,DC=norddjurs,DC=dk"

			// Connect to the Active Directory.
			// The distinguished name is given as the container (root conainer) to avoid
			// this exception later: "The specified directory service attribute or value does not exist"
			return new PrincipalContext(ContextType.Domain, domainName, distinguishedName);
		} // GetContext

		public static PrincipalContext GetContext(String bindDistinguishedName) {
			// Connect to the Active Directory.
			// The distinguished name is given as the container (root conainer) to avoid
			// this exception later: "The specified directory service attribute or value does not exist"
			return new PrincipalContext(ContextType.Domain, AdUtility.GetDomainName(), bindDistinguishedName);
		} // GetContext

	} // AdUlitity
	#endregion

	#region AdUser class.
	[DirectoryRdnPrefix("CN")]
	[DirectoryObjectClass("user")]      // person
	public class AdUser : UserPrincipal {

		#region Constructors.
		public AdUser(PrincipalContext context) : base(context) {
		} // AdUser

		public AdUser(PrincipalContext context, String samAccountName, String password, Boolean enabled) : base(context, samAccountName, password, enabled) {
		} // AdUser
		#endregion

		#region Public properties.
		/* The following properties are inherited from UserPrincipal class.

			SamAccountName
			Sid
			Guid
			DistinguishedName
			UserPrincipalName
			LastLogon
			LastPasswordSet
			LastBadPasswordAttempt
			PermittedLogonTimes
			AccountExpirationDate
			AccountLockoutTime
			BadLogonCount
			Enabled
			SmartcardLogonRequired
			PasswordNotRequired
			PasswordNeverExpires
			UserCannotChangePassword
			AllowReversiblePasswordEncryption
			GivenName
			MiddleName
			Surname
			DisplayName
			Name
			Description
			EmailAddress
			VoiceTelephoneNumber
			EmployeeId
			ScriptPath
			HomeDirectory
			HomeDrive

			Manager
			StreetAddress
			st
			l
			postalCode
			co

				Certificates
				AdvancedSearchFilter
				Context
				ContextRaw
				ContextType
				Current
				DelegationPermitted
				PermittedWorkstations
				StructuralObjectClass

		*/

		[DirectoryProperty("whenCreated")]
		public DateTime? Created {
			get {
				if (base.ExtensionGet("whenCreated").Length != 1) {
					return null;
				} else {
					return (DateTime)base.ExtensionGet("whenCreated")[0];
				}
			}
		} // Created

		[DirectoryProperty("whenChanged")]
		public DateTime? Modified {
			get {
				if (base.ExtensionGet("whenChanged").Length != 1) {
					return null;
				} else {
					return (DateTime)base.ExtensionGet("whenChanged")[0];
				}
			}
		} // Modified

		public Boolean Hidden {
			get {
				return this.SamAccountName.EndsWith("$") == true;
			}
		} // Hidden

		[DirectoryProperty("LogonCount")]
		public Nullable<Int32> LogonCount {
			get {
				if (base.ExtensionGet("LogonCount").Length != 1) {
					return null;
				} else {
					return ((Nullable<Int32>)base.ExtensionGet("LogonCount")[0]);
				}
			}
		} // LogonCount

		[DirectoryProperty("title")]
		public String Title {
			get {
				if (base.ExtensionGet("title").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("title")[0];
				}
			}
			set {
				base.ExtensionSet("title", value);
			}
		} // Title

		[DirectoryProperty("physicalDeliveryOfficeName")]
		public String PhysicalDeliveryOfficeName {
			get {
				if (base.ExtensionGet("physicalDeliveryOfficeName").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("physicalDeliveryOfficeName")[0];
				}
			}
			set {
				base.ExtensionSet("physicalDeliveryOfficeName", value);
			}
		} // PhysicalDeliveryOfficeName

		[DirectoryProperty("department")]
		public String Department {
			get {
				if (base.ExtensionGet("department").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("department")[0];
				}
			}
			set {
				base.ExtensionSet("department", value);
			}
		} // Department

		[DirectoryProperty("company")]
		public String Company {
			get {
				if (base.ExtensionGet("company").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("company")[0];
				}
			}
			set {
				base.ExtensionSet("company", value);
			}
		} // Company

		[DirectoryProperty("info")]
		public String Info {
			get {
				if (base.ExtensionGet("info").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("info")[0];
				}
			}
			set {
				base.ExtensionSet("info", value);
			}
		} // Info

		public String SmtpProxyAddress {
			get {
				String[] addresses = this.SmtpProxyAddresses;
				if (addresses.Length > 0) {
					return addresses[0];
				} else {
					return String.Empty;
				}
			}
			set {
				String[] addresses = this.SmtpProxyAddresses;
				if (addresses.Length > 0) {
					addresses[0] = value;
				} else if (value.IsNullOrWhiteSpace() == false){
					addresses = new String[] { value };
				}
				this.SmtpProxyAddresses = addresses;
			}
		} // SmtpProxyAddress


		[DirectoryProperty("proxyAddresses")]
		public String[] SmtpProxyAddresses {
			get {
				try {
					//	Get all SMTP addresses.
					List<String> addresses = new List<String>();
					foreach (String address in (String[])base.ExtensionGet("proxyAddresses")) {
						if (address.Trim().StartsWith("SMTP:") == true) {
							// Always insert the primary address as the first item.
							addresses.Insert(0, address.Trim().Substring(5));
						} else if (address.Trim().ToLower().StartsWith("smtp:") == true) {
							// Append non primary addresses.
							addresses.Add(address.Trim().Substring(5));
						}
					}

					// Return the result.
					return addresses.ToArray();
				} catch {
					return new String[0];
				}
			}
			set {
				// Replace all SMTP addresses with the values.
				// Get all non SMTP addresses.
				List<String> addresses = new List<String>();
				foreach (String address in (String[])base.ExtensionGet("proxyAddresses")) {
					if (address.Trim().ToLower().StartsWith("smtp:") == false) {
						addresses.Add(address.Trim());
					}
				}

				// Add the new SMTP addresses.
				Boolean firstSmtpAddress = true;
				foreach (String address in value) {
					if (firstSmtpAddress == true) {
						//	Add the first address as the primary address.
						addresses.Add("SMTP:" + address.ToLower());
						firstSmtpAddress = false;
					} else {
						//	Add non first addresses as secondary addresses.
						addresses.Add("smtp:" + address.ToLower());
					}
				}

				// Save the addresses.
				base.ExtensionSet("proxyAddresses", addresses.ToArray());

				// Save the mail address.
				if (value.Length > 0) {
					base.EmailAddress = value[0];
				}
			}
		} // SmtpProxyAddresses

		[DirectoryProperty("wwwHomePage")]
		public String WWWHomePage {
			get {
				if (base.ExtensionGet("wwwHomePage").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("wwwHomePage")[0];
				}
			}
			set {
				base.ExtensionSet("wwwHomePage", value);
			}
		} // WWWHomePage

		[DirectoryProperty("url")]
		public String[] Url {
			get {
				if (base.ExtensionGet("url").Length < 1) {
					return null;
				} else {
					return (String[])base.ExtensionGet("url");
				}
			}
			set {
				base.ExtensionSet("url", value);
			}
		} // Url

		[DirectoryProperty("telephoneNumber")]
		public String TelephoneNumber {
			get {
				if (base.ExtensionGet("telephoneNumber").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("telephoneNumber")[0];
				}
			}
			set {
				base.ExtensionSet("telephoneNumber", value);
			}
		} // TelephoneNumber

		[DirectoryProperty("otherTelephone")]
		public String[] OtherTelephone {
			get {
				if (base.ExtensionGet("otherTelephone").Length < 1) {
					return null;
				} else {
					return (String[])base.ExtensionGet("otherTelephone");
				}
			}
			set {
				base.ExtensionSet("otherTelephone", value);
			}
		} // OtherTelephone

		[DirectoryProperty("mobile")]
		public String Mobile {
			get {
				if (base.ExtensionGet("mobile").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("mobile")[0];
				}
			}
			set {
				base.ExtensionSet("mobile", value);
			}
		} // Mobile

		[DirectoryProperty("otherMobile")]
		public String[] OtherMobile {
			get {
				if (base.ExtensionGet("otherMobile").Length < 1) {
					return null;
				} else {
					return (String[])base.ExtensionGet("otherMobile");
				}
			}
			set {
				base.ExtensionSet("otherMobile", value);
			}
		} // OtherMobile

		[DirectoryProperty("facsimileTelephoneNumber")]
		public String FacsimileTelephoneNumber {
			get {
				if (base.ExtensionGet("facsimileTelephoneNumber").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("facsimileTelephoneNumber")[0];
				}
			}
			set {
				base.ExtensionSet("facsimileTelephoneNumber", value);
			}
		} // FacsimileTelephoneNumber

		[DirectoryProperty("otherFacsimileTelephoneNumber")]
		public String[] OtherFacsimileTelephoneNumber {
			get {
				if (base.ExtensionGet("otherFacsimileTelephoneNumber").Length < 1) {
					return null;
				} else {
					return (String[])base.ExtensionGet("otherFacsimileTelephoneNumber");
				}
			}
			set {
				base.ExtensionSet("otherFacsimileTelephoneNumber", value);
			}
		} // OtherFacsimileTelephoneNumber

		[DirectoryProperty("pager")]
		public String Pager {
			get {
				if (base.ExtensionGet("pager").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("pager")[0];
				}
			}
			set {
				base.ExtensionSet("pager", value);
			}
		} // Pager

		[DirectoryProperty("otherPager")]
		public String[] OtherPager {
			get {
				if (base.ExtensionGet("otherPager").Length < 1) {
					return null;
				} else {
					return (String[])base.ExtensionGet("otherPager");
				}
			}
			set {
				base.ExtensionSet("otherPager", value);
			}
		} // OtherPager

		[DirectoryProperty("homePhone")]
		public String HomePhone {
			get {
				if (base.ExtensionGet("homePhone").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("homePhone")[0];
				}
			}
			set {
				base.ExtensionSet("homePhone", value);
			}
		} // HomePhone


		[DirectoryProperty("otherHomePhone")]
		public String[] HomePhoneOther {
			get {
				if (base.ExtensionGet("otherHomePhone").Length < 1) {
					return null;
				} else {
					return (String[])base.ExtensionGet("otherHomePhone");
				}
			}
			set {
				base.ExtensionSet("otherHomePhone", value);
			}
		} // HomePhoneOther

		[DirectoryProperty("ipPhone")]
		public String IPPhone {
			get {
				if (base.ExtensionGet("ipPhone").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("ipPhone")[0];
				}
			}
			set {
				base.ExtensionSet("ipPhone", value);
			}
		} // IPPhone


		[DirectoryProperty("otherIpPhone")]
		public String[] OtherIpPhone {
			get {
				if (base.ExtensionGet("otherIpPhone").Length < 1) {
					return null;
				} else {
					return (String[])base.ExtensionGet("otherIpPhone");
				}
			}
			set {
				base.ExtensionSet("otherIpPhone", value);
			}
		} // OtherIpPhone

		[DirectoryProperty("manager")]
		public String Manager {
			get {
				if (base.ExtensionGet("manager").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("manager")[0];
				}
			}
			set {
				base.ExtensionSet("manager", value);
			}
		} // Manager

		[DirectoryProperty("streetAddress")]
		public String Street {
			get {
				if (base.ExtensionGet("streetAddress").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("streetAddress")[0];
				}
			}
			set {
				base.ExtensionSet("streetAddress", value);
			}
		} // Street

		[DirectoryProperty("st")]
		public String State {
			get {
				if (base.ExtensionGet("st").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("st")[0];
				}
			}
			set {
				base.ExtensionSet("st", value);
			}
		} // State

		[DirectoryProperty("l")]
		public String City {
			get {
				if (base.ExtensionGet("l").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("l")[0];
				}
			}
			set {
				base.ExtensionSet("l", value);
			}
		} // City

		[DirectoryProperty("postalCode")]
		public String PostalCode {
			get {
				if (base.ExtensionGet("postalCode").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("postalCode")[0];
				}
			}
			set {
				base.ExtensionSet("postalCode", value);
			}
		} // PostalCode

		[DirectoryProperty("c")]
		public String Country {
			get {
				if (base.ExtensionGet("c").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("c")[0];
				}
			}
			set {
				try {
					RegionInfo region = new RegionInfo(value);
					base.ExtensionSet("c", region.TwoLetterISORegionName);
				} catch {
					base.ExtensionSet("c", value);
				}
			}
		} // Country

		[DirectoryProperty("extensionAttribute1")]
		public String ExtensionAttribute1 {
			get {
				if (base.ExtensionGet("extensionAttribute1").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute1")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute1", value);
			}
		} // ExtensionAttribute1

		[DirectoryProperty("extensionAttribute2")]
		public String ExtensionAttribute2 {
			get {
				if (base.ExtensionGet("extensionAttribute2").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute2")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute2", value);
			}
		} // ExtensionAttribute2

		[DirectoryProperty("extensionAttribute3")]
		public String ExtensionAttribute3 {
			get {
				if (base.ExtensionGet("extensionAttribute3").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute3")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute3", value);
			}
		} // ExtensionAttribute3

		[DirectoryProperty("extensionAttribute4")]
		public String ExtensionAttribute4 {
			get {
				if (base.ExtensionGet("extensionAttribute4").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute4")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute4", value);
			}
		} // ExtensionAttribute4

		[DirectoryProperty("extensionAttribute5")]
		public String ExtensionAttribute5 {
			get {
				if (base.ExtensionGet("extensionAttribute5").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute5")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute5", value);
			}
		} // ExtensionAttribute5

		[DirectoryProperty("extensionAttribute6")]
		public String ExtensionAttribute6 {
			get {
				if (base.ExtensionGet("extensionAttribute6").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute6")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute6", value);
			}
		} // ExtensionAttribute6

		[DirectoryProperty("extensionAttribute7")]
		public String ExtensionAttribute7 {
			get {
				if (base.ExtensionGet("extensionAttribute7").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute7")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute7", value);
			}
		} // ExtensionAttribute7

		[DirectoryProperty("extensionAttribute8")]
		public String ExtensionAttribute8 {
			get {
				if (base.ExtensionGet("extensionAttribute8").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute8")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute8", value);
			}
		} // ExtensionAttribute8

		[DirectoryProperty("extensionAttribute9")]
		public String ExtensionAttribute9 {
			get {
				if (base.ExtensionGet("extensionAttribute9").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute9")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute9", value);
			}
		} // ExtensionAttribute9

		[DirectoryProperty("extensionAttribute1")]
		public String ExtensionAttribute10 {
			get {
				if (base.ExtensionGet("extensionAttribute10").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute10")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute10", value);
			}
		} // ExtensionAttribute10

		[DirectoryProperty("extensionAttribute11")]
		public String ExtensionAttribute11 {
			get {
				if (base.ExtensionGet("extensionAttribute11").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute11")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute11", value);
			}
		} // ExtensionAttribute11

		[DirectoryProperty("extensionAttribute12")]
		public String ExtensionAttribute12 {
			get {
				if (base.ExtensionGet("extensionAttribute12").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute12")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute12", value);
			}
		} // ExtensionAttribute12

		[DirectoryProperty("extensionAttribute13")]
		public String ExtensionAttribute13 {
			get {
				if (base.ExtensionGet("extensionAttribute13").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute13")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute13", value);
			}
		} // ExtensionAttribute13

		[DirectoryProperty("extensionAttribute14")]
		public String ExtensionAttribute14 {
			get {
				if (base.ExtensionGet("extensionAttribute14").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute14")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute14", value);
			}
		} // ExtensionAttribute14

		[DirectoryProperty("extensionAttribute15")]
		public String ExtensionAttribute15 {
			get {
				if (base.ExtensionGet("extensionAttribute15").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("extensionAttribute15")[0];
				}
			}
			set {
				base.ExtensionSet("extensionAttribute15", value);
			}
		} // ExtensionAttribute15

		[DirectoryProperty("profilePath")]
		public String ProfilePath {
			get {
				if (base.ExtensionGet("profilePath").Length != 1) {
					return null;
				} else {
					return (String)base.ExtensionGet("profilePath")[0];
				}
			}
			set {
				base.ExtensionSet("profilePath", value);
			}
		} // ProfilePath

		[DirectoryProperty("jpegPhoto")]
		public Image Photo {
			get {
				if (base.ExtensionGet("jpegPhoto").Length != 1) {
					return null;
				} else {
					return ByteArrayToImage((Byte[])base.ExtensionGet("jpegPhoto")[0]);
				}
			}
			set {
				base.ExtensionSet("jpegPhoto", ImageToByteArray(value));
			}
		} // Photo

		[DirectoryProperty("thumbnailPhoto")]
		public Image PhotoThumbnail {
			get {
				if (base.ExtensionGet("thumbnailPhoto").Length != 1) {
					return null;
				} else {
					return ByteArrayToImage((Byte[])base.ExtensionGet("thumbnailPhoto")[0]);
				}
			}
			set {
				// The ThumbnailPhoto attribute is limited to 100 KB.
				// This limit is defined in the RangeUpper value of the attribute.
				// More: http://blogs.technet.com/b/exchange/archive/2010/06/01/3410006.aspx

				// Resize the picture to 96 * 96 pixels.
				if ((value.Width > 96) || (value.Height > 96)) {
					value = ResizeImageFixedSize(value, 96, 96, Color.Transparent);
				}

				base.ExtensionSet("thumbnailPhoto", ImageToByteArray(value));
			}
		} // PhotoThumbnail
		#endregion

		#region Public methods.
		public Boolean IsAccountExpired() {
			return ((this.AccountExpirationDate != null) && (this.AccountExpirationDate.Value.CompareTo(DateTime.Now) < 0));
		} // IsAccountExpired

		/// <summary>
		/// Inserts the info text at the beginning of the info, followed by a new line.
		/// The info text is preceeded with the current date and succeeded with the current username.
		/// Null and empty info text, is ignored.
		/// 
		/// You should invoke the save method after this.
		/// </summary>
		/// <param name="infoText">The info text.</param>
		public void InsertInfo(String infoText) {
			if ((infoText != null) && (infoText.Trim().Length > 0)) {
				if ((this.Info == null) || (this.Info.Trim().Length == 0)) {
					// First and only line.
					this.Info = String.Format("{0:dd.MM.yy}: {1} ({2})", DateTime.Now, infoText.Trim(), Environment.UserName);
				} else {
					// Insert before existing text.
					this.Info = String.Format("{0:dd.MM.yy}: {1} ({3}){4}{2}", DateTime.Now, infoText.Trim(), this.Info.Trim(), Environment.UserName, Environment.NewLine);
				}
			}
		} // InsertInfo

		/// <summary>
		/// Gets all the authorization groups of which this user is a member.
		/// This function only returns groups that are security groups, distribution groups are not returned.
		/// 
		/// This method searches all groups recursively and returns the groups in which this user is a member.
		/// </summary>
		/// <returns>The groups of which this user is a member.</returns>
		public new List<AdGroup> GetAuthorizationGroups() {
			List<AdGroup> groups = new List<AdGroup>();
			foreach (Principal principal in base.GetAuthorizationGroups()) {
				AdGroup group = AdGroup.FindByIdentity(base.Context, principal.DistinguishedName);
				if (group != null) {
					groups.Add(group);
				}
			}
			return groups;
		} //GetAuthorizationGroups

		/// <summary>
		/// Gets all the groups of which this user is a member.
		/// 
		/// This method returns only the groups of which this user is directly a member, no recursive searches are performed.
		/// </summary>
		/// <returns>The groups of which this user is a member.</returns>
		//////////[Obsolete("This is not supported in this class.", true)]
		[SecurityCriticalAttribute]
		public new List<AdGroup> GetGroups() {
			//////////throw new NotImplementedException("This is not supported in this class!");
			List<AdGroup> groups = new List<AdGroup>();
			foreach (Principal principal in base.GetGroups()) {
				AdGroup group = AdGroup.FindByIdentity(base.Context, principal.DistinguishedName);
				if (group != null) {
					groups.Add(group);
				}
			}
			return groups;
		} // GetGroups

		/// <summary>
		/// Gets all the groups of which this user is a member.
		/// </summary>
		/// <param name="recursive">Perform a recursive search.</param>
		/// <returns>The groups of which this user is a member.</returns>
		[SecurityCriticalAttribute]
		public List<AdGroup> GetGroups(Boolean recursive) {
			if (recursive == false) {
				List<AdGroup> groups = new List<AdGroup>();
				foreach (Principal principal in base.GetGroups()) {
					AdGroup group = AdGroup.FindByIdentity(base.Context, principal.DistinguishedName);
					if (group != null) {
						groups.Add(group);
					}
				}
				return groups;
			} else {
				// Get the direct member groups.
				List<AdGroup> groups = new List<AdGroup>();
				Stack<AdGroup> stackedGroups = new Stack<AdGroup>();
				foreach (AdGroup memberGroup in this.GetGroups(this.Context)) {
					stackedGroups.Push(memberGroup);
				}

				// Process.
				while (stackedGroups.Count > 0) {
					// Get the group.
					AdGroup memberGroup = stackedGroups.Pop();

					// Add the group.
					if (groups.Contains(memberGroup) == false) {
						groups.Add(memberGroup);

						// Get child groups.
						foreach (AdGroup childGroup in memberGroup.GetGroups(this.Context)) {
							if (groups.Contains(childGroup) == false) {
								stackedGroups.Push(childGroup);
							}
						}
					}
				}

				// Return false.
				return groups;
			}
		} // GetGroups

		/// <summary>
		/// Gets all the groups of which this user is a member of and that exist in the store provided by the specified context parameter.
		/// 
		/// This method returns only the groups of which this user is directly a member, no recursive searches are performed.
		/// </summary>
		/// <param name="contextToQuery">The principal context that specifies the server or domain against which operations are performed.</param>
		/// <returns>The groups of which this user is a member.</returns>
		[SecurityCriticalAttribute]
		public new List<AdGroup> GetGroups(PrincipalContext contextToQuery) {
			List<AdGroup> groups = new List<AdGroup>();
			foreach (Principal principal in base.GetGroups(contextToQuery)) {
				AdGroup group = AdGroup.FindByIdentity(base.Context, principal.DistinguishedName);
				if (group != null) {
					groups.Add(group);
				}
			}
			return groups;
		} // GetGroups

		/// <summary>
		/// Gets whether this user is a member of the specified group.
		/// 
		/// This method searches only the groups of which this user is directly a member, no recursive searches are performed.
		/// </summary>
		/// <param name="group">The group for which membership is determined.</param>
		/// <returns>True if this user is a member of the specified group.</returns>
		[SecurityCriticalAttribute]
		public new Boolean IsMemberOf(GroupPrincipal group) {
			return base.IsMemberOf(group);
		} // IsMemberOf

		/// <summary>
		/// Gets whether this user is a member of the specified group.
		/// </summary>
		/// <param name="group">The group for which membership is determined.</param>
		/// <param name="recursive">Perform a recursive search.</param>
		/// <returns>True if this user is a member of the specified group.</returns>
		[SecurityCriticalAttribute]
		public Boolean IsMemberOf(GroupPrincipal group, Boolean recursive) {
			if (recursive == false) {
				return base.IsMemberOf(group);
			} else {
				// Get the direct member groups.
				List<GroupPrincipal> groups = new List<GroupPrincipal>();
				Stack<GroupPrincipal> stackedGroups = new Stack<GroupPrincipal>();
				foreach (GroupPrincipal memberGroup in this.GetGroups(this.Context)) {
					stackedGroups.Push(memberGroup);
				}

				// Process.
				while (stackedGroups.Count > 0) {
					// Get the group.
					GroupPrincipal memberGroup = stackedGroups.Pop();

					// Test the group.
					if (group.Equals(memberGroup) == true) {
						return true;
					}

					// Remember that the group is processed.
					groups.Add(memberGroup);

					// Get child groups.
					foreach (GroupPrincipal childGroup in memberGroup.GetGroups(this.Context)) {
						if (groups.Contains(childGroup) == false) {
							stackedGroups.Push(childGroup);
						}
					}
				}

				// Return false.
				return false;
			}
		} // IsMemberOf

		/// <summary>
		/// Gets whether this user is a member of the group specified by identity type and value.
		/// </summary>
		/// <param name="context">The principal context that specifies the server or domain against which operations are performed.</param>
		/// <param name="identityType">A identity type value that specifies the format of the identity value.</param>
		/// <param name="identityValue">The identity of the group.</param>
		/// <returns>True if this user is a member of the specified group.</returns>
		[SecurityCriticalAttribute]
		public new Boolean IsMemberOf(PrincipalContext context, IdentityType identityType, String identityValue) {
			return base.IsMemberOf(context, identityType, identityValue);
		} // IsMemberOf

		public override String ToString() {
			StringBuilder result = new StringBuilder();

			result.AppendFormat("DistinguishedName:{0}", DistinguishedName);
			result.AppendLine();
			result.AppendFormat("DisplayName:{0}", DisplayName);
			result.AppendLine();
			result.AppendFormat("EmailAddress:{0}", EmailAddress);
			result.AppendLine();
			result.AppendFormat("UserPrincipalName:{0}", UserPrincipalName);
			result.AppendLine();
			result.AppendFormat("Name:{0}", Name);
			result.AppendLine();
			result.AppendFormat("GivenName:{0}", GivenName);
			result.AppendLine();
			result.AppendFormat("SamAccountName:{0}", SamAccountName);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute1:{0}", ExtensionAttribute1);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute2:{0}", ExtensionAttribute2);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute3:{0}", ExtensionAttribute3);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute4:{0}", ExtensionAttribute4);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute5:{0}", ExtensionAttribute5);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute6:{0}", ExtensionAttribute6);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute7:{0}", ExtensionAttribute7);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute8:{0}", ExtensionAttribute8);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute9:{0}", ExtensionAttribute9);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute10:{0}", ExtensionAttribute10);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute11:{0}", ExtensionAttribute11);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute12:{0}", ExtensionAttribute12);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute13:{0}", ExtensionAttribute13);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute14:{0}", ExtensionAttribute14);
			result.AppendLine();
			result.AppendFormat("ExtensionAttribute15:{0}", ExtensionAttribute15);
			result.AppendLine();

			return result.ToString();
		} // ToString
		#endregion

		#region Public static methods.
		/// <summary>
		/// Gets the user identified by the identity type and identity value.
		/// </summary>
		/// <param name="context">The principal context that specifies the server or domain against which operations are performed.</param>
		/// <param name="identityType">A identity type value that specifies the format of the identity value.</param>
		/// <param name="identityValue">The identity of the user.</param>
		/// <returns>A user object that matches the specified identity value, or null if no matches are found.</returns>
		public static new AdUser FindByIdentity(PrincipalContext context, IdentityType identityType, String identityValue) {
			return (AdUser)FindByIdentityWithType(context, typeof(AdUser), identityType, identityValue);
		} // FindByIdentity

		/// <summary>
		/// Gets the user identified by the identity value.
		/// The identity value can be Name, Globally Unique Identifier (GUID), Distinguished Name (DN),
		/// Security Account Manager (SAM) name, User Principal Name (UPN) or 
		/// Security Identifier (SID) in Security Descriptor Definition Language (SDDL) format.
		/// </summary>
		/// <param name="context">The principal context that specifies the server or domain against which operations are performed.</param>
		/// <param name="identityValue">The identity of the user.</param>
		/// <returns>A user object that matches the specified identity value, or null if no matches are found.</returns>
		public static new AdUser FindByIdentity(PrincipalContext context, String identityValue) {
			return (AdUser)FindByIdentityWithType(context, typeof(AdUser), identityValue);
		} // FindByIdentity

		/// <summary>
		/// Gets the user identified by the identity value.
		/// The identity value can be Person Number (CPR), Name, Globally Unique Identifier (GUID), Distinguished Name (DN),
		/// Security Account Manager (SAM) name, User Principal Name (UPN) or 
		/// Security Identifier (SID) in Security Descriptor Definition Language (SDDL) format.
		/// </summary>
		/// <param name="context">The principal context that specifies the server or domain against which operations are performed.</param>
		/// <param name="identityValue">The identity of the user.</param>
		/// <returns>A user object that matches the specified identity value, or null if no matches are found.</returns>
		public static AdUser FindByIdentity(String identityValue, String userCprAttribute = "EmployeeId") {
			return AdUser.FindByIdentity(AdUtility.GetContext(), identityValue, userCprAttribute);
		} // FindByIdentity

		/// <summary>
		/// Gets the user identified by the identity value.
		/// The identity value can be Person Number (CPR), Name, Globally Unique Identifier (GUID), Distinguished Name (DN),
		/// Security Account Manager (SAM) name, User Principal Name (UPN) or 
		/// Security Identifier (SID) in Security Descriptor Definition Language (SDDL) format.
		/// </summary>
		/// <param name="context">The principal context that specifies the server or domain against which operations are performed.</param>
		/// <param name="identityValue">The identity of the user.</param>
		/// <param name="userCprAttribute">The attribute holding the person number (EmployeeId).</param>
		/// <returns>A user object that matches the specified identity value, or null if no matches are found.</returns>
		public static AdUser FindByIdentity(PrincipalContext context, String identityValue, String userCprAttribute = "EmployeeId") {
			try {
				AdUser user = null;

				// Search for the Person Number (CPR) identity type.
				// TODO: Add check for numeric identityValue.
				if (identityValue.Trim().Replace("-", String.Empty).Length == 10) {
					try {
						// Initialize the query.
						AdUser userQueryFilter = new AdUser(context);
						switch (userCprAttribute.ToLower()) {
							case "extensionattribute1":
								userQueryFilter.ExtensionAttribute1 = identityValue;
								break;
							case "extensionattribute2":
								userQueryFilter.ExtensionAttribute2 = identityValue;
								break;
							case "extensionattribute3":
								userQueryFilter.ExtensionAttribute3 = identityValue;
								break;
							case "extensionattribute4":
								userQueryFilter.ExtensionAttribute4 = identityValue;
								break;
							case "extensionattribute5":
								userQueryFilter.ExtensionAttribute5 = identityValue;
								break;
							case "extensionattribute6":
								userQueryFilter.ExtensionAttribute6 = identityValue;
								break;
							case "extensionattribute7":
								userQueryFilter.ExtensionAttribute7 = identityValue;
								break;
							case "extensionattribute8":
								userQueryFilter.ExtensionAttribute8 = identityValue;
								break;
							case "extensionattribute9":
								userQueryFilter.ExtensionAttribute9 = identityValue;
								break;
							case "extensionattribute10":
								userQueryFilter.ExtensionAttribute10 = identityValue;
								break;
							case "extensionattribute11":
								userQueryFilter.ExtensionAttribute11 = identityValue;
								break;
							case "extensionattribute12":
								userQueryFilter.ExtensionAttribute12 = identityValue;
								break;
							case "extensionattribute13":
								userQueryFilter.ExtensionAttribute13 = identityValue;
								break;
							case "extensionattribute14":
								userQueryFilter.ExtensionAttribute14 = identityValue;
								break;
							case "extensionattribute15":
								userQueryFilter.ExtensionAttribute15 = identityValue;
								break;
							case "employeeid":
							default:
								userQueryFilter.EmployeeId = identityValue;
								break;
						}
						PrincipalSearcher searcher = new PrincipalSearcher();
						searcher.QueryFilter = userQueryFilter;
						user = (AdUser)searcher.FindOne();
					} catch { }
				}

				// Search for other identity types.
				if (user == null) {
					try {
						user = (AdUser)FindByIdentityWithType(context, typeof(AdUser), identityValue);
					} catch { }
				}

				// Return the user or null.
				if ((user != null) && (user.StructuralObjectClass == "user")) {
					return user;
				} else {
					return null;
				}
			} catch {
				return null;
			}
		} // FindByIdentity

		/// <summary>
		/// Gets the current user.
		/// </summary>
		/// <returns>The current user.</returns>
		public static AdUser GetCurrentUser() {
			return AdUser.FindByIdentity(Environment.UserName);
		} // GetCurrentUser

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <returns>All users.</returns>
		public static List<AdUser> GetAllUsers(UserQuery userFilter = UserQuery.ALL) {
			return AdUser.GetAllUsers(userFilter, 0);
		} // GetAllUsers

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <param name="advancedUserFilterDays">Days added/substracted when using advanced user filters.</param>
		/// <returns>All users.</returns>
		public static List<AdUser> GetAllUsers(UserQuery userFilter, Int32 advancedUserFilterDays = 0) {
			List<AdUser> users = new List<AdUser>();

			// Create local datetime.
			DateTime localDateTime = DateTime.SpecifyKind(DateTime.Now.AddDays(advancedUserFilterDays), DateTimeKind.Local);

			// Create the query filter.
			AdUser userQueryFilter = new AdUser(AdUtility.GetContext());
			switch (userFilter) {
				// Simple queries.
				case UserQuery.ENABLED:
					userQueryFilter.Enabled = true;
					break;
				case UserQuery.DISABLED:
					userQueryFilter.Enabled = false;
					break;
				case UserQuery.PASSWORD_NEWER_EXPIRES:
					userQueryFilter.PasswordNeverExpires = true;
					break;
				case UserQuery.PASSWORD_EXPIRES:
					userQueryFilter.PasswordNeverExpires = false;
					break;
				case UserQuery.PASSWORD_NOT_REQUIRED:
					userQueryFilter.PasswordNotRequired = true;
					break;
				case UserQuery.PASSWORD_REQUIRED:
					userQueryFilter.PasswordNotRequired = false;
					break;
				case UserQuery.PASSWORD_CHANGE_ENABLED:
				case UserQuery.PASSWORD_CHANGE_DISABLED:
					// This gives this exception:
					// The property 'AuthenticablePrincipal.UserCannotChangePassword' can not be used in a query against this store.
					// Get all users, and "filter" later.
					userQueryFilter.Name = "*";
					break;

				// Advanced queries.
				case UserQuery.ACCOUNT_NOT_EXPIRED:
					userQueryFilter.AdvancedSearchFilter.AccountExpirationDate(localDateTime, MatchType.GreaterThan);
					break;
				case UserQuery.ACCOUNT_EXPIRED:
					userQueryFilter.AdvancedSearchFilter.AccountExpirationDate(localDateTime, MatchType.LessThanOrEquals);
					break;
				case UserQuery.ACCOUNT_NOT_LOCKED_OUT:
					userQueryFilter.AdvancedSearchFilter.AccountLockoutTime(localDateTime, MatchType.LessThanOrEquals);
					break;
				case UserQuery.ACCOUNT_LOCKED_OUT:
					userQueryFilter.AdvancedSearchFilter.AccountLockoutTime(DateTime.Now.AddDays(-1), MatchType.GreaterThan);
					break;

				// Default query.
				case UserQuery.ALL:
				default:
					userQueryFilter.Name = "*";
					break;
			}

			// Create the searcher, and set the PageSize on the underlying DirectorySearcher to get all entries.
			PrincipalSearcher searcher = new PrincipalSearcher();
			searcher.QueryFilter = userQueryFilter;
			((DirectorySearcher)searcher.GetUnderlyingSearcher()).PageSize = Int32.MaxValue;

			// Execute the search.
			PrincipalSearchResult<Principal> searchResults = searcher.FindAll();
			foreach (AdUser searchResult in searchResults) {
				// Perform post filtering.
				switch (userFilter) {
					// Simple queries.
					case UserQuery.PASSWORD_CHANGE_ENABLED:
						if (searchResult.UserCannotChangePassword == false) {
							if (searchResult.StructuralObjectClass.ToLower() == "user") {
								users.Add((AdUser)searchResult);
							}
						}
						break;
					case UserQuery.PASSWORD_CHANGE_DISABLED:
						if (searchResult.UserCannotChangePassword == true) {
							if (searchResult.StructuralObjectClass.ToLower() == "user") {
								users.Add(searchResult);
							}
						}
						break;

					// Advanced queries.

					// Default query.
					case UserQuery.ALL:
					default:
						if (searchResult.StructuralObjectClass.ToLower() == "user") {
							users.Add(searchResult);
						}
						break;
				}
			}

			// Return the found users.
			return users;
		} // GetAllUsers
		#endregion

		#region Private methods.
		private Byte[] ImageToByteArray(Image value) {
			using (MemoryStream memoryStream = new MemoryStream()) {
				value.Save(memoryStream, ImageFormat.Png);
				return memoryStream.ToArray();
			}
		} // ImageToByteArray

		private Image ByteArrayToImage(Byte[] value) {
			using (MemoryStream memoryStream = new MemoryStream(value)) {
				Image returnImage = Image.FromStream(memoryStream);
				return returnImage;
			}
		} // ByteArrayToImage

		private Image ResizeImageFixedSize(Image imgPhoto, Int32 Width, Int32 Height, Color paddingColor) {
			Int32 sourceWidth = imgPhoto.Width;
			Int32 sourceHeight = imgPhoto.Height;
			Int32 sourceX = 0;
			Int32 sourceY = 0;
			Int32 destX = 0;
			Int32 destY = 0;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width / (float)sourceWidth);
			nPercentH = ((float)Height / (float)sourceHeight);
			if (nPercentH < nPercentW) {
				nPercent = nPercentH;
				destX = System.Convert.ToInt16((Width - (sourceWidth * nPercent)) / 2);
			} else {
				nPercent = nPercentW;
				destY = System.Convert.ToInt16((Height - (sourceHeight * nPercent)) / 2);
			}

			Int32 destWidth = (Int32)(sourceWidth * nPercent);
			Int32 destHeight = (Int32)(sourceHeight * nPercent);
			Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.Clear(paddingColor);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
			grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
			grPhoto.Dispose();
			return bmPhoto;
		} // ResizeImageFixedSize
		#endregion

	} // AdUser
	#endregion

	#region AdGroup class.
	[DirectoryRdnPrefix("CN")]
	[DirectoryObjectClass("group")]
	public class AdGroup : GroupPrincipal {

		#region Constructors.
		public AdGroup(PrincipalContext context) : base(context) {
		} // AdGroup

		public AdGroup(PrincipalContext context, String samAccountName) : base(context, samAccountName) {
		} // AdGroup
		#endregion

		#region Public properties.

		// public PrincipalCollection Members { get; }
		// Members

		#endregion

		#region Public methods.
		/// <summary>
		/// Gets all the groups of which this group is a member.
		/// 
		/// This method returns only the groups of which this group is directly a member, no recursive searches are performed.
		/// </summary>
		/// <returns>The groups of which this group is a member.</returns>
		[SecurityCriticalAttribute]
		public new List<AdGroup> GetGroups() {
			List<AdGroup> groups = new List<AdGroup>();
			foreach (Principal principal in base.GetGroups()) {
				AdGroup group = AdGroup.FindByIdentity(base.Context, principal.DistinguishedName);
				if (group != null) {
					groups.Add(group);
				}
			}
			return groups;
		} // GetGroups

		/// <summary>
		/// Gets all the groups of which this group is a member.
		/// </summary>
		/// <param name="recursive">Perform a recursive search.</param>
		/// <returns>The groups of which this group is a member.</returns>
		[SecurityCriticalAttribute]
		public List<AdGroup> GetGroups(Boolean recursive) {
			if (recursive == false) {
				List<AdGroup> groups = new List<AdGroup>();
				foreach (Principal principal in base.GetGroups()) {
					AdGroup group = AdGroup.FindByIdentity(base.Context, principal.DistinguishedName);
					if (group != null) {
						groups.Add(group);
					}
				}
				return groups;
			} else {
				// Get the direct member groups.
				List<AdGroup> groups = new List<AdGroup>();
				Stack<AdGroup> stackedGroups = new Stack<AdGroup>();
				foreach (AdGroup memberGroup in this.GetGroups(this.Context)) {
					stackedGroups.Push(memberGroup);
				}

				// Process.
				while (stackedGroups.Count > 0) {
					// Get the group.
					AdGroup memberGroup = stackedGroups.Pop();

					// Add the group.
					if (groups.Contains(memberGroup) == false) {
						groups.Add(memberGroup);

						// Get child groups.
						foreach (AdGroup childGroup in memberGroup.GetGroups(this.Context)) {
							if (groups.Contains(childGroup) == false) {
								stackedGroups.Push(childGroup);
							}
						}
					}
				}

				// Return false.
				return groups;
			}
		} // GetGroups

		/// <summary>
		/// Gets all the groups of which this group is a member of and that exist in the store provided by the specified context parameter.
		/// 
		/// This method returns only the groups of which this group is directly a member, no recursive searches are performed.
		/// </summary>
		/// <param name="contextToQuery">The principal context that specifies the server or domain against which operations are performed.</param>
		/// <returns>The groups of which this group is a member.</returns>
		[SecurityCriticalAttribute]
		public new List<AdGroup> GetGroups(PrincipalContext contextToQuery) {
			List<AdGroup> groups = new List<AdGroup>();
			foreach (Principal principal in base.GetGroups(contextToQuery)) {
				AdGroup group = AdGroup.FindByIdentity(base.Context, principal.DistinguishedName);
				if (group != null) {
					groups.Add(group);
				}
			}
			return groups;
		} // GetGroups

		/// <summary>
		/// Gets all the users that is a member of this group.
		/// 
		/// This method returns only the users that is a directly member of this group, no recursive searches are performed.
		/// </summary>
		/// <returns>The users that is a member of this group.</returns>
		public new List<AdUser> GetMembers() {
			List<AdUser> users = new List<AdUser>();
			foreach (Principal principal in base.GetMembers()) {
				AdUser user = AdUser.FindByIdentity(base.Context, principal.DistinguishedName);
				if (user != null) {
					users.Add(user);
				}
			}
			return users;
		} // GetMembers

		/// <summary>
		/// Gets all the users that is a member of this group.
		/// </summary>
		/// <param name="recursive">Perform a recursive search.</param>
		/// <returns>The users that is a member of this group.</returns>
		public new List<AdUser> GetMembers(Boolean recursive) {
			List<AdUser> users = new List<AdUser>();
			foreach (Principal principal in base.GetMembers(recursive)) {
				AdUser user = AdUser.FindByIdentity(base.Context, principal.DistinguishedName);
				if (user != null) {
					users.Add(user);
				}
			}
			return users;
		} // GetMembers

		/// <summary>
		/// Gets whether this group is a member of the specified group.
		/// 
		/// This method searches only the groups of which this user is directly a member, no recursive searches are performed.
		/// </summary>
		/// <param name="group">The group for which membership is determined.</param>
		/// <returns>True if this group is a member of the specified group.</returns>
		[SecurityCriticalAttribute]
		public new Boolean IsMemberOf(GroupPrincipal group) {
			return base.IsMemberOf(group);
		} // IsMemberOf

		/// <summary>
		/// Gets whether this group is a member of the specified group.
		/// </summary>
		/// <param name="group">The group for which membership is determined.</param>
		/// <param name="recursive">Perform a recursive search.</param>
		/// <returns>True if this group is a member of the specified group.</returns>
		[SecurityCriticalAttribute]
		public Boolean IsMemberOf(GroupPrincipal group, Boolean recursive) {
			if (recursive == false) {
				return base.IsMemberOf(group);
			} else {
				// Get the direct member groups.
				List<GroupPrincipal> groups = new List<GroupPrincipal>();
				Stack<GroupPrincipal> stackedGroups = new Stack<GroupPrincipal>();
				foreach (GroupPrincipal memberGroup in this.GetGroups(this.Context)) {
					stackedGroups.Push(memberGroup);
				}

				// Process.
				while (stackedGroups.Count > 0) {
					// Get the group.
					GroupPrincipal memberGroup = stackedGroups.Pop();

					// Test the group.
					if (group.Equals(memberGroup) == true) {
						return true;
					}

					// Remember that the group is processed.
					groups.Add(memberGroup);

					// Get child groups.
					foreach (GroupPrincipal childGroup in memberGroup.GetGroups(this.Context)) {
						if (groups.Contains(childGroup) == false) {
							stackedGroups.Push(childGroup);
						}
					}
				}

				// Return false.
				return false;
			}
		} // IsMemberOf

		/// <summary>
		/// Gets whether this group is a member of the group specified by identity type and value.
		/// </summary>
		/// <param name="context">The principal context that specifies the server or domain against which operations are performed.</param>
		/// <param name="identityType">A identity type value that specifies the format of the identity value.</param>
		/// <param name="identityValue">The identity of the group.</param>
		/// <returns>True if this group is a member of the specified group.</returns>
		[SecurityCriticalAttribute]
		public new Boolean IsMemberOf(PrincipalContext context, IdentityType identityType, String identityValue) {
			return base.IsMemberOf(context, identityType, identityValue);
		} // IsMemberOf

		public override String ToString() {
			StringBuilder result = new StringBuilder();

			result.AppendFormat("DistinguishedName:{0}", DistinguishedName);
			result.AppendLine();
			result.AppendFormat("DisplayName:{0}", DisplayName);
			result.AppendLine();
			result.AppendFormat("UserPrincipalName:{0}", UserPrincipalName);
			result.AppendLine();
			result.AppendFormat("Name:{0}", Name);
			result.AppendLine();

			return result.ToString();
		} // ToString
		#endregion

		#region Public static methods.
		/// <summary>
		/// Gets the group identified by the identity type and identity value.
		/// </summary>
		/// <param name="context">The principal context that specifies the server or domain against which operations are performed.</param>
		/// <param name="identityType">A identity type value that specifies the format of the identity value.</param>
		/// <param name="identityValue">The identity of the group.</param>
		/// <returns>A group object that matches the specified identity value, or null if no matches are found.</returns>
		public static new AdGroup FindByIdentity(PrincipalContext context, IdentityType identityType, String identityValue) {
			return (AdGroup)FindByIdentityWithType(context, typeof(AdGroup), identityType, identityValue);
		} // FindByIdentity

		/// <summary>
		/// Gets the group identified by the identity type and identity value.
		/// The identity value can be Name, Globally Unique Identifier (GUID), Distinguished Name (DN),
		/// Security Account Manager (SAM) name, User Principal Name (UPN) or 
		/// Security Identifier (SID) in Security Descriptor Definition Language (SDDL) format.
		/// </summary>
		/// <param name="context">The principal context that specifies the server or domain against which operations are performed.</param>
		/// <param name="identityType">A identity type value that specifies the format of the identity value.</param>
		/// <param name="identityValue">The identity of the group.</param>
		/// <returns>A group object that matches the specified identity value, or null if no matches are found.</returns>
		public static new AdGroup FindByIdentity(PrincipalContext context, String identityValue) {
			return (AdGroup)FindByIdentityWithType(context, typeof(AdGroup), identityValue);
		} // FindByIdentity

		/// <summary>
		/// Gets the group identified by the identity value.
		/// The identity value can be Name, Globally Unique Identifier (GUID), Distinguished Name (DN),
		/// Security Account Manager (SAM) name, User Principal Name (UPN) or 
		/// Security Identifier (SID) in Security Descriptor Definition Language (SDDL) format.
		/// </summary>
		/// <param name="identityValue">The identity of the group.</param>
		/// <returns>A group object that matches the specified identity value, or null if no matches are found.</returns>
		public static AdGroup FindByIdentity(String identityValue) {
			return (AdGroup)FindByIdentityWithType(AdUtility.GetContext(), typeof(AdGroup), identityValue);
		} // FindByIdentity

		/// <summary>
		/// Gets all groups.
		/// </summary>
		/// <returns>All groups.</returns>
		public static List<AdGroup> GetAllGroups() {
			List<AdGroup> groups = new List<AdGroup>();

			// Create the query filter.
			AdGroup groupQueryFilter = new AdGroup(AdUtility.GetContext());
			groupQueryFilter.Name = "*";

			// Create the searcher, and set the PageSize on the underlying DirectorySearcher to get all entries.
			PrincipalSearcher searcher = new PrincipalSearcher();
			searcher.QueryFilter = groupQueryFilter;
			((DirectorySearcher)searcher.GetUnderlyingSearcher()).PageSize = Int32.MaxValue;

			// Execute the search.
			PrincipalSearchResult<Principal> searchResults = searcher.FindAll();
			foreach (Principal searchResult in searchResults) {
				groups.Add((AdGroup)searchResult);
			}

			// Return the found groups.
			return groups;
		} // GetAllGroups
		#endregion

	} // AdGroup
	#endregion

	#region AdOrganizationalUnit class.
	public class AdOrganizationalUnit {

		#region Constructors.
		public AdOrganizationalUnit() {
		} // AdOrganizationalUnit
		#endregion

		#region Properties.
		#endregion

		#region Methods.
		#endregion

		#region Static methods.
		#endregion

	} // AdOrganizationalUnit
	#endregion

} // NDK.Framework