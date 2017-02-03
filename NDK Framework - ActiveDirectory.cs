using System;
using System.Collections;
using System.DirectoryServices.AccountManagement;
using System.Text;

namespace NDK.Framework {

	[DirectoryRdnPrefix("CN")]
	[DirectoryObjectClass("person")]
	public class Person : UserPrincipal {
		private PersonSearchFilter searchFilter = null;

		public Person(PrincipalContext context) : base(context) {
		} // Person

		public Person(PrincipalContext context, String samAccountName, String password, Boolean enabled) : base(context, samAccountName, password, enabled) {
		} // Person

		public new PersonSearchFilter AdvancedSearchFilter {
			get {
				if (searchFilter == null) {
					searchFilter = new PersonSearchFilter(this);
				}
				return searchFilter;
			}
		} // AdvancedSearchFilter

		#region Implement directory object class "inetOrgPerson".
		[DirectoryProperty("mobile")]
		public String MobilePhone {
			get {
				if (ExtensionGet("mobile").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("mobile")[0];
				}
			}
			set {
				ExtensionSet("mobile", value);
			}
		} // MobilePhone

		// Create the other home phone property.    
		[DirectoryProperty("otherHomePhone")]
		public String[] HomePhoneOther {
			get {
				Int32 len = ExtensionGet("otherHomePhone").Length;
				String[] otherHomePhone = new String[len];
				Object[] otherHomePhoneRaw = ExtensionGet("otherHomePhone");
				for (Int32 i = 0; i < len; i++) {
					otherHomePhone[i] = (String)otherHomePhoneRaw[i];
				}
				return otherHomePhone;
			}
			set {
				ExtensionSet("otherHomePhone", value);
			}
		} // HomePhoneOther

		// Create the logoncount property.    
		[DirectoryProperty("LogonCount")]
		public Nullable<Int32> LogonCount {
			get {
				if (ExtensionGet("LogonCount").Length != 1) {
					return null;
				} else {
					return ((Nullable<Int32>)ExtensionGet("LogonCount")[0]);
				}
			}
		} // LogonCount
		#endregion

		#region Implement directory object class "person" (ExtensionAttribute1 to 15).
		[DirectoryProperty("extensionAttribute1")]
		public String ExtensionAttribute1 {
			get {
				if (ExtensionGet("extensionAttribute1").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute1")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute1", value);
			}
		} // ExtensionAttribute1

		[DirectoryProperty("extensionAttribute2")]
		public String ExtensionAttribute2 {
			get {
				if (ExtensionGet("extensionAttribute2").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute2")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute2", value);
			}
		} // ExtensionAttribute2

		[DirectoryProperty("extensionAttribute3")]
		public String ExtensionAttribute3 {
			get {
				if (ExtensionGet("extensionAttribute3").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute3")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute3", value);
			}
		} // ExtensionAttribute3

		[DirectoryProperty("extensionAttribute4")]
		public String ExtensionAttribute4 {
			get {
				if (ExtensionGet("extensionAttribute4").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute4")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute4", value);
			}
		} // ExtensionAttribute4

		[DirectoryProperty("extensionAttribute5")]
		public String ExtensionAttribute5 {
			get {
				if (ExtensionGet("extensionAttribute5").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute5")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute5", value);
			}
		} // ExtensionAttribute5

		[DirectoryProperty("extensionAttribute6")]
		public String ExtensionAttribute6 {
			get {
				if (ExtensionGet("extensionAttribute6").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute6")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute6", value);
			}
		} // ExtensionAttribute6

		[DirectoryProperty("extensionAttribute7")]
		public String ExtensionAttribute7 {
			get {
				if (ExtensionGet("extensionAttribute7").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute7")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute7", value);
			}
		} // ExtensionAttribute7

		[DirectoryProperty("extensionAttribute8")]
		public String ExtensionAttribute8 {
			get {
				if (ExtensionGet("extensionAttribute8").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute8")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute8", value);
			}
		} // ExtensionAttribute8

		[DirectoryProperty("extensionAttribute9")]
		public String ExtensionAttribute9 {
			get {
				if (ExtensionGet("extensionAttribute9").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute9")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute9", value);
			}
		} // ExtensionAttribute9

		[DirectoryProperty("extensionAttribute1")]
		public String ExtensionAttribute10 {
			get {
				if (ExtensionGet("extensionAttribute10").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute10")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute10", value);
			}
		} // ExtensionAttribute10

		[DirectoryProperty("extensionAttribute11")]
		public String ExtensionAttribute11 {
			get {
				if (ExtensionGet("extensionAttribute11").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute11")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute11", value);
			}
		} // ExtensionAttribute11

		[DirectoryProperty("extensionAttribute12")]
		public String ExtensionAttribute12 {
			get {
				if (ExtensionGet("extensionAttribute12").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute12")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute12", value);
			}
		} // ExtensionAttribute12

		[DirectoryProperty("extensionAttribute13")]
		public String ExtensionAttribute13 {
			get {
				if (ExtensionGet("extensionAttribute13").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute13")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute13", value);
			}
		} // ExtensionAttribute13

		[DirectoryProperty("extensionAttribute14")]
		public String ExtensionAttribute14 {
			get {
				if (ExtensionGet("extensionAttribute14").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute14")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute14", value);
			}
		} // ExtensionAttribute14

		[DirectoryProperty("extensionAttribute15")]
		public String ExtensionAttribute15 {
			get {
				if (ExtensionGet("extensionAttribute15").Length != 1) {
					return null;
				} else {
					return (String)ExtensionGet("extensionAttribute15")[0];
				}
			}
			set {
				ExtensionSet("extensionAttribute15", value);
			}
		} // ExtensionAttribute15
		#endregion

		#region Implement the overloaded methods (FindByIdentity, ToString).
		public static new Person FindByIdentity(PrincipalContext context, String identityValue) {
			return (Person)FindByIdentityWithType(context, typeof(Person), identityValue);
		} // FindByIdentity

		// Implement the overloaded search method FindByIdentity. 
		public static new Person FindByIdentity(PrincipalContext context, IdentityType identityType, String identityValue) {
			return (Person)FindByIdentityWithType(context, typeof(Person), identityType, identityValue);
		} // FindByIdentity

		public override String ToString() {
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("DistinguishedName:{0}", DistinguishedName);
			sb.AppendLine();
			sb.AppendFormat("DisplayName:{0}", DisplayName);
			sb.AppendLine();
			sb.AppendFormat("EmailAddress:{0}", EmailAddress);
			sb.AppendLine();
			sb.AppendFormat("UserPrincipalName:{0}", UserPrincipalName);
			sb.AppendLine();
			sb.AppendFormat("Name:{0}", Name);
			sb.AppendLine();
			sb.AppendFormat("GivenName:{0}", GivenName);
			sb.AppendLine();
			sb.AppendFormat("SamAccountName:{0}", SamAccountName);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute1:{0}", ExtensionAttribute1);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute2:{0}", ExtensionAttribute2);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute3:{0}", ExtensionAttribute3);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute4:{0}", ExtensionAttribute4);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute5:{0}", ExtensionAttribute5);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute6:{0}", ExtensionAttribute6);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute7:{0}", ExtensionAttribute7);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute8:{0}", ExtensionAttribute8);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute9:{0}", ExtensionAttribute9);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute10:{0}", ExtensionAttribute10);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute11:{0}", ExtensionAttribute11);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute12:{0}", ExtensionAttribute12);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute13:{0}", ExtensionAttribute13);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute14:{0}", ExtensionAttribute14);
			sb.AppendLine();
			sb.AppendFormat("ExtensionAttribute15:{0}", ExtensionAttribute15);
			sb.AppendLine();

			return sb.ToString();
		} // ToString
		#endregion

	} // Person


	public class PersonSearchFilter : AdvancedFilters {

		public PersonSearchFilter(Principal principal) : base(principal) {
		} // PersonSearchFilter

		public void LogonCount(Int32 value, MatchType matchType) {
			this.AdvancedFilterSet("LogonCount", value, typeof(Int32), matchType);
		} // LogonCount

	} // PersonSearchFilter

} // NDK.Framework