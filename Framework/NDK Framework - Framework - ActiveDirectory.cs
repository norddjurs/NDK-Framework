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