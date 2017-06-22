using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines Active Directory access.
	/// </summary>
	public partial interface IFramework {

		#region Active Directory user methods.
		/// <summary>
		/// Gets the current user.
		/// </summary>
		/// <returns>The current user.</returns>
		AdUser GetCurrentUser();

		/// <summary>
		/// Gets the user identified by the user id.
		/// The user id can be person number (CPR), Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The user id to find.</param>
		/// <returns>The matching user or null.</returns>
		AdUser GetUser(String userId);

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <returns>All users.</returns>
		List<AdUser> GetAllUsers(UserQuery userFilter = UserQuery.ALL);

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <param name="advancedUserFilterDays">Days added/substracted when using advanced user filters.</param>
		/// <returns>All users.</returns>
		List<AdUser> GetAllUsers(UserQuery userFilter, Int32 advancedUserFilterDays = 0);

		/// <summary>
		/// Gets all users that are member of the group.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns></returns>
		List<AdUser> GetAllUsers(AdGroup group, Boolean recursive = true);

		/// <summary>
		/// Gets the CPR number from the user.
		/// This uses the "ActiveDirectoryCprAttribute" system configuration to determane which field that stores the value.
		/// </summary>
		/// <param name="user">The user.</param>
		String GetUserCprNumber(AdUser user);

		/// <summary>
		/// Gets the MiFare identifier from the user.
		/// This uses the "ActiveDirectoryMiFareAttribute" system configuration to determane which field that stores the value.
		/// </summary>
		/// <param name="user">The user.</param>
		String GetUserMiFareId(AdUser user);

		/// <summary>
		/// Gets the first other mobile number from the user.
		/// </summary>
		/// <param name="user">The user.</param>
		String GetUserOtherMobile(AdUser user);

		/// <summary>
		/// Sets the CPR number in the user.
		/// This uses the "ActiveDirectoryCprAttribute" system configuration to determane which field that stores the value.
		/// This method does not commit the change.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="cprNumber">The CPR number.</param>
		void SetUserCprNumber(AdUser user, String cprNumber);

		/// <summary>
		/// Sets the MiFare identifier in the user.
		/// This uses the "ActiveDirectoryMiFareAttribute" system configuration to determane which field that stores the value.
		/// This method does not commit the change.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="miFareId">The MiFare identifier.</param>
		void SetUserMiFareId(AdUser user, String miFareId);

		/// <summary>
		/// Sets the first other mobile number in the user.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="cprNumber">The CPR number.</param>
		void SetUserOtherMobile(AdUser user, String otherMobile);
		#endregion

		#region Active Directory group methods.
		/// <summary>
		/// Gets the group identified by the group id.
		/// The group id can be Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The group id to find.</param>
		/// <returns>The matching group or null.</returns>
		AdGroup GetGroup(String groupId);

		/// <summary>
		/// Gets all groups.
		/// </summary>
		/// <returns>All groups.</returns>
		List<AdGroup> GetAllGroups();
		#endregion

		#region Active Directory membership methods.
		/// <summary>
		/// Gets if the current user is member of the group.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns>True if the current user is member of the group.</returns>
		Boolean IsUserMemberOfGroup(GroupPrincipal group, Boolean recursive = true);

		/// <summary>
		/// Gets if the user is member of the group.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns>True if the user is member of the group.</returns>
		Boolean IsUserMemberOfGroup(AdUser user, GroupPrincipal group, Boolean recursive = true);

		/// <summary>
		/// Gets if the user is member of one or all the groups.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <param name="all">True if the user must be member of all the groups.</param>
		/// <param name="groups">The groups.</param>
		/// <returns>True if the user is member of one or all the groups as specified.</returns>
		Boolean IsUserMemberOfGroups(AdUser user, Boolean recursive, Boolean all, params GroupPrincipal[] groups);
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework