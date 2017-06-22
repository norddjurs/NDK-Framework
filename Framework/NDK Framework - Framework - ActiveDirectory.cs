using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace NDK.Framework {

	#region Framework
	/// <summary>
	/// This partial part of the class, implements Active Directory access.
	/// </summary>
	public abstract partial class Framework : IFramework {
		private PrincipalContext context = null;

		#region Private Active Directory initialization
		private void ActiveDirectoryInitialize() {
			// Convert the domain name to the distinguished name.
			String domainName = this.DomainName;    // Environment.UserDomainName
			String distinguishedName = "DC=" + domainName.Replace(".", ",DC=");    // "DC=intern,DC=norddjurs,DC=dk"
			distinguishedName = this.GetSystemValue("ActiveDirectoryBindDN", distinguishedName);

			// Connect to the Active Directory.
			// The distinguished name is given as the container (root conainer) to avoid
			// this exception later: "The specified directory service attribute or value does not exist"
			this.context = new PrincipalContext(ContextType.Domain, domainName, distinguishedName);
		} // ActiveDirectoryInitialize
		#endregion

		#region Public Active Directory properties.
		/// <summary>
		/// Gets the domain names in the current forest.
		/// </summary>
		public String[] DomainNames {
			get {
				return AdUtility.GetDomainNames();
			}
		} // DomainNames

		/// <summary>
		/// Gets a random domain name in the current forest.
		/// </summary>
		public String DomainName {
			get {
				return AdUtility.GetDomainName();
			}
		} // DomainName
		#endregion

		#region Public Active Directory user methods.
		/// <summary>
		/// Gets the current user.
		/// </summary>
		/// <returns>The current user.</returns>
		public AdUser GetCurrentUser() {
			return AdUser.GetCurrentUser();
		} // GetCurrentUser

		/// <summary>
		/// Gets the user identified by the identity value.
		/// The identity value can be Person Number (CPR), Name, Globally Unique Identifier (GUID), Distinguished Name (DN),
		/// Security Account Manager (SAM) name, User Principal Name (UPN) or 
		/// Security Identifier (SID) in Security Descriptor Definition Language (SDDL) format.
		/// </summary>
		/// <param name="userId">The user id to find.</param>
		/// <returns>The matching user or null.</returns>
		public AdUser GetUser(String userId) {
			return AdUser.FindByIdentity(userId);
		} // GetUser

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <returns>All users.</returns>
		public List<AdUser> GetAllUsers(UserQuery userFilter = UserQuery.ALL) {
			return AdUser.GetAllUsers(userFilter, 0);
		} // GetAllUsers

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <param name="advancedUserFilterDays">Days added/substracted when using advanced user filters.</param>
		/// <returns>All users.</returns>
		public List<AdUser> GetAllUsers(UserQuery userFilter, Int32 advancedUserFilterDays = 0) {
			return AdUser.GetAllUsers(userFilter, advancedUserFilterDays);
		} // GetAllUsers

		/// <summary>
		/// Gets all users that are member of the group.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns></returns>
		public List<AdUser> GetAllUsers(AdGroup group, Boolean recursive = true) {
			return group.GetMembers(recursive);
		} // GetAllUsers

		/// <summary>
		/// Gets the CPR number from the user.
		/// This uses the "ActiveDirectoryCprAttribute" system configuration to determane which field that stores the value.
		/// </summary>
		/// <param name="user">The user.</param>
		public String GetUserCprNumber(AdUser user) {
			try {
				String userCprAttribute = this.GetSystemValue("ActiveDirectoryCprAttribute", "EmployeeId");

				switch (userCprAttribute.ToLower()) {
					case "extensionattribute1":
						return user.ExtensionAttribute1.FormatStringCpr();
					case "extensionattribute2":
						return user.ExtensionAttribute2.FormatStringCpr();
					case "extensionattribute3":
						return user.ExtensionAttribute3.FormatStringCpr();
					case "extensionattribute4":
						return user.ExtensionAttribute4.FormatStringCpr();
					case "extensionattribute5":
						return user.ExtensionAttribute5.FormatStringCpr();
					case "extensionattribute6":
						return user.ExtensionAttribute6.FormatStringCpr();
					case "extensionattribute7":
						return user.ExtensionAttribute7.FormatStringCpr();
					case "extensionattribute8":
						return user.ExtensionAttribute8.FormatStringCpr();
					case "extensionattribute9":
						return user.ExtensionAttribute9.FormatStringCpr();
					case "extensionattribute10":
						return user.ExtensionAttribute10.FormatStringCpr();
					case "extensionattribute11":
						return user.ExtensionAttribute11.FormatStringCpr();
					case "extensionattribute12":
						return user.ExtensionAttribute12.FormatStringCpr();
					case "extensionattribute13":
						return user.ExtensionAttribute13.FormatStringCpr();
					case "extensionattribute14":
						return user.ExtensionAttribute14.FormatStringCpr();
					case "extensionattribute15":
						return user.ExtensionAttribute15.FormatStringCpr();
					case "employeeid":
					default:
						return user.EmployeeId.FormatStringCpr();
				}
			} catch {
				return String.Empty;
			}
		} // GetUserCprNumber

		/// <summary>
		/// Gets the MiFare identifier from the user.
		/// This uses the "ActiveDirectoryMiFareAttribute" system configuration to determane which field that stores the value.
		/// </summary>
		/// <param name="user">The user.</param>
		public String GetUserMiFareId(AdUser user) {
			try {
				String userMiFareAttribute = this.GetSystemValue("ActiveDirectoryMiFareAttribute", String.Empty);

				switch (userMiFareAttribute.ToLower()) {
					case "extensionattribute1":
						return user.ExtensionAttribute1;
					case "extensionattribute2":
						return user.ExtensionAttribute2;
					case "extensionattribute3":
						return user.ExtensionAttribute3;
					case "extensionattribute4":
						return user.ExtensionAttribute4;
					case "extensionattribute5":
						return user.ExtensionAttribute5;
					case "extensionattribute6":
						return user.ExtensionAttribute6;
					case "extensionattribute7":
						return user.ExtensionAttribute7;
					case "extensionattribute8":
						return user.ExtensionAttribute8;
					case "extensionattribute9":
						return user.ExtensionAttribute9;
					case "extensionattribute10":
						return user.ExtensionAttribute10;
					case "extensionattribute11":
						return user.ExtensionAttribute11;
					case "extensionattribute12":
						return user.ExtensionAttribute12;
					case "extensionattribute13":
						return user.ExtensionAttribute13;
					case "extensionattribute14":
						return user.ExtensionAttribute14;
					case "extensionattribute15":
						return user.ExtensionAttribute15;
					default:
						return String.Empty;
				}
			} catch {
				return String.Empty;
			}
		} // GetUserMiFareId

		/// <summary>
		/// Gets the first other mobile number from the user.
		/// </summary>
		/// <param name="user">The user.</param>
		public String GetUserOtherMobile(AdUser user) {
			try {
				return user.OtherMobile[0].FormatStringPhone();
			} catch {
				return String.Empty;
			}
		} // GetUserOtherMobile

		/// <summary>
		/// Sets the CPR number in the user.
		/// This uses the "ActiveDirectoryCprAttribute" system configuration to determane which field that stores the value.
		/// This method does not commit the change.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="cprNumber">The CPR number.</param>
		public void SetUserCprNumber(AdUser user, String cprNumber) {
			String userCprAttribute = this.GetSystemValue("ActiveDirectoryCprAttribute", "EmployeeId");
			cprNumber = cprNumber.FormatStringCpr();

			switch (userCprAttribute.ToLower()) {
				case "extensionattribute1":
					user.ExtensionAttribute1 = cprNumber;
					break;
				case "extensionattribute2":
					user.ExtensionAttribute2 = cprNumber;
					break;
				case "extensionattribute3":
					user.ExtensionAttribute3 = cprNumber;
					break;
				case "extensionattribute4":
					user.ExtensionAttribute4 = cprNumber;
					break;
				case "extensionattribute5":
					user.ExtensionAttribute5 = cprNumber;
					break;
				case "extensionattribute6":
					user.ExtensionAttribute6 = cprNumber;
					break;
				case "extensionattribute7":
					user.ExtensionAttribute7 = cprNumber;
					break;
				case "extensionattribute8":
					user.ExtensionAttribute8 = cprNumber;
					break;
				case "extensionattribute9":
					user.ExtensionAttribute9 = cprNumber;
					break;
				case "extensionattribute10":
					user.ExtensionAttribute10 = cprNumber;
					break;
				case "extensionattribute11":
					user.ExtensionAttribute11 = cprNumber;
					break;
				case "extensionattribute12":
					user.ExtensionAttribute12 = cprNumber;
					break;
				case "extensionattribute13":
					user.ExtensionAttribute13 = cprNumber;
					break;
				case "extensionattribute14":
					user.ExtensionAttribute14 = cprNumber;
					break;
				case "extensionattribute15":
					user.ExtensionAttribute15 = cprNumber;
					break;
				case "employeeid":
				default:
					user.EmployeeId = cprNumber;
					break;
			}
		} // SetUserCprNumber

		/// <summary>
		/// Sets the MiFare identifier in the user.
		/// This uses the "ActiveDirectoryMiFareAttribute" system configuration to determane which field that stores the value.
		/// This method does not commit the change.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="miFareId">The MiFare identifier.</param>
		public void SetUserMiFareId(AdUser user, String miFareId) {
			String userMiFareAttribute = this.GetSystemValue("ActiveDirectoryMiFareAttribute", String.Empty);

			switch (userMiFareAttribute.ToLower()) {
				case "extensionattribute1":
					user.ExtensionAttribute1 = miFareId;
					break;
				case "extensionattribute2":
					user.ExtensionAttribute2 = miFareId;
					break;
				case "extensionattribute3":
					user.ExtensionAttribute3 = miFareId;
					break;
				case "extensionattribute4":
					user.ExtensionAttribute4 = miFareId;
					break;
				case "extensionattribute5":
					user.ExtensionAttribute5 = miFareId;
					break;
				case "extensionattribute6":
					user.ExtensionAttribute6 = miFareId;
					break;
				case "extensionattribute7":
					user.ExtensionAttribute7 = miFareId;
					break;
				case "extensionattribute8":
					user.ExtensionAttribute8 = miFareId;
					break;
				case "extensionattribute9":
					user.ExtensionAttribute9 = miFareId;
					break;
				case "extensionattribute10":
					user.ExtensionAttribute10 = miFareId;
					break;
				case "extensionattribute11":
					user.ExtensionAttribute11 = miFareId;
					break;
				case "extensionattribute12":
					user.ExtensionAttribute12 = miFareId;
					break;
				case "extensionattribute13":
					user.ExtensionAttribute13 = miFareId;
					break;
				case "extensionattribute14":
					user.ExtensionAttribute14 = miFareId;
					break;
				case "extensionattribute15":
					user.ExtensionAttribute15 = miFareId;
					break;
			}
		} // SetUserMiFareId

		/// <summary>
		/// Sets the first other mobile number in the user.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="cprNumber">The CPR number.</param>
		public void SetUserOtherMobile(AdUser user, String otherMobile) {
			if ((user.OtherMobile != null) && (user.OtherMobile.Length > 0)) {
				String[] values = user.OtherMobile;
				values[0] = otherMobile.FormatStringPhone();
				user.OtherMobile = values;
			} else {
				user.OtherMobile = new String[] { otherMobile.FormatStringPhone() };
			}
		} // SetUserOtherMobile
		#endregion

		#region Public Active Directory group methods.
		/// <summary>
		/// Gets the group identified by the identity value.
		/// The identity value can be Name, Globally Unique Identifier (GUID), Distinguished Name (DN),
		/// Security Account Manager (SAM) name, User Principal Name (UPN) or 
		/// Security Identifier (SID) in Security Descriptor Definition Language (SDDL) format.
		/// </summary>
		/// <param name="groupId">The identity of the group.</param>
		/// <returns>The matching group or null.</returns>
		public AdGroup GetGroup(String groupId) {
			return AdGroup.FindByIdentity(groupId);
		} // GetGroup

		/// <summary>
		/// Gets all groups.
		/// </summary>
		/// <returns>All groups.</returns>
		public List<AdGroup> GetAllGroups() {
			return AdGroup.GetAllGroups();
		} // GetAllGroups
		#endregion

		#region Public Active Directory membership methods.
		/// <summary>
		/// Gets if the current user is member of the group.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns>True if the current user is member of the group.</returns>
		public Boolean IsUserMemberOfGroup(GroupPrincipal group, Boolean recursive = true) {
			return this.IsUserMemberOfGroup(this.GetCurrentUser(), group, recursive);
		} // IsUserMemberOfGroup

		/// <summary>
		/// Gets if the user is member of the group.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns>True if the user is member of the group.</returns>
		public Boolean IsUserMemberOfGroup(AdUser user, GroupPrincipal group, Boolean recursive = true) {
			return user.IsMemberOf(group, recursive);
		} // IsUserMemberOfGroup

		/// <summary>
		/// Gets if the user is member of one or all the groups.
		/// This method return false if the groups array is empty.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <param name="all">True if the user must be member of all the groups.</param>
		/// <param name="groups">The groups.</param>
		/// <returns>True if the user is member of one or all the groups as specified.</returns>
		public Boolean IsUserMemberOfGroups(AdUser user, Boolean recursive, Boolean all, params GroupPrincipal[] groups) {
			Int32 isMemberCount = 0;
			foreach (GroupPrincipal group in groups) {
				Boolean isMember = this.IsUserMemberOfGroup(user, group, recursive);

				// Count the number of member groups.
				if (isMember == true) {
					isMemberCount++;
				}

				// Return false if the user is not member of the group, when the user must be member of all the groups.
				if ((all == true) && (isMember == false)) {
					return false;
				}

				// Return true if the user is member of the group, when the user must be member of one of the groups.
				if ((all == false) && (isMember == true)) {
					return true;
				}
			}

			// Return true if the user is member of all the groups, when the user must be member of all the groups.
			if (all == true) {
				return groups.Length == isMemberCount;
			} else {
				// Return false.
				return false;
			}
		} // IsUserMemberOfGroups
		#endregion

	} // Framework
	#endregion

} // NDK.Framework