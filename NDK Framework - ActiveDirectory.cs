using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
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

	#region ActiveDirectory class.
	public class ActiveDirectory {
		private IConfiguration config = null;
		private ILogger logger = null;
		private PrincipalContext context = null;

		/// <summary>
		/// Connect to the Active Directory as the current user.
		/// </summary>
		public ActiveDirectory(IConfiguration config, ILogger logger) {
			this.config = config;
			this.logger = logger;

			// Connect to the Active Directory.
			this.context = new PrincipalContext(ContextType.Domain);
		} // ActiveDirectory

		/// <summary>
		/// Gets the current user.
		/// </summary>
		/// <returns>The current user.</returns>
		public Person GetCurrentUser() {
			return this.GetUser(Environment.UserName);
		} // GetCurrentUser

		/// <summary>
		/// Gets the user identified by the user id.
		/// The user id can be person number (CPR), Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The user id to find.</param>
		/// <returns>The matching user or null.</returns>
		public Person GetUser(String userId) {
			Person user = null;

			// Search cpr number.
			// TODO: Add check for numeric userId.
			if (userId.Trim().Replace("-", String.Empty).Length == 10) {
				try {
					// Get which attribute stores the cpr number.
					String userCprAttribute = this.config.GetSystemValue("ActiveDirectoryCprAttribute", "EmployeeId");

					// Initialize the query.
					Person userQueryFilter = new Person(this.context);
					switch (userCprAttribute.ToLower()) {
						case "extensionattribute1":
							userQueryFilter.ExtensionAttribute1 = userId;
							break;
						case "extensionattribute2":
							userQueryFilter.ExtensionAttribute2 = userId;
							break;
						case "extensionattribute3":
							userQueryFilter.ExtensionAttribute3 = userId;
							break;
						case "extensionattribute4":
							userQueryFilter.ExtensionAttribute4 = userId;
							break;
						case "extensionattribute5":
							userQueryFilter.ExtensionAttribute5 = userId;
							break;
						case "extensionattribute6":
							userQueryFilter.ExtensionAttribute6 = userId;
							break;
						case "extensionattribute7":
							userQueryFilter.ExtensionAttribute7 = userId;
							break;
						case "extensionattribute8":
							userQueryFilter.ExtensionAttribute8 = userId;
							break;
						case "extensionattribute9":
							userQueryFilter.ExtensionAttribute9 = userId;
							break;
						case "extensionattribute10":
							userQueryFilter.ExtensionAttribute10 = userId;
							break;
						case "extensionattribute11":
							userQueryFilter.ExtensionAttribute11 = userId;
							break;
						case "extensionattribute12":
							userQueryFilter.ExtensionAttribute12 = userId;
							break;
						case "extensionattribute13":
							userQueryFilter.ExtensionAttribute13 = userId;
							break;
						case "extensionattribute14":
							userQueryFilter.ExtensionAttribute14 = userId;
							break;
						case "extensionattribute15":
							userQueryFilter.ExtensionAttribute15 = userId;
							break;
						case "employeeid":
						default:
							userQueryFilter.EmployeeId = userId;
							break;
					}
					PrincipalSearcher searcher = new PrincipalSearcher();
					searcher.QueryFilter = userQueryFilter;
					user = (Person)searcher.FindOne();
				} catch { }
			}

			// Search guid.
			Guid userGuid = Guid.Empty;
			if ((user == null) && (Guid.TryParse(userId, out userGuid) == true)) {
				try {
					user = Person.FindByIdentity(this.context, IdentityType.Guid, userId);
				} catch { }
			}

			// Search distinguished name.
			if (user == null) {
				try {
					user = Person.FindByIdentity(this.context, IdentityType.DistinguishedName, userId);
				} catch { }
			}

			// Search sam account name.
			if (user == null) {
				try {
					user = Person.FindByIdentity(this.context, IdentityType.SamAccountName, userId);
				} catch { }
			}

			// Search user principal name.
			if (user == null) {
				try {
					user = Person.FindByIdentity(this.context, IdentityType.UserPrincipalName, userId);
				} catch { }
			}

			// Search security identifier.
			if (user == null) {
				try {
					user = Person.FindByIdentity(this.context, IdentityType.Sid, userId);
				} catch { }
			}

			// Return the user or null.
			return user;
		} // GetUser

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <returns>All users.</returns>
		public List<Person> GetAllUsers(UserQuery userFilter = UserQuery.ALL) {
			return this.GetAllUsers(userFilter, 0);
		} // GetAllUsers

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <param name="advancedUserFilterDays">Days added/substracted when using advanced user filters.</param>
		/// <returns>All users.</returns>
		public List<Person> GetAllUsers(UserQuery userFilter, Int32 advancedUserFilterDays = 0) {
			List<Person> users = new List<Person>();

			// Create local datetime.
			DateTime localDateTime = DateTime.SpecifyKind(DateTime.Now.AddDays(advancedUserFilterDays), DateTimeKind.Local);

			// Create the query filter.
			Person userQueryFilter = new Person(this.context);
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
			foreach (Person searchResult in searchResults) {
				// Perform post filtering.
				switch (userFilter) {
					// Simple queries.
					case UserQuery.PASSWORD_CHANGE_ENABLED:
						if (searchResult.UserCannotChangePassword == false) {
							users.Add((Person)searchResult);
						}
						break;
					case UserQuery.PASSWORD_CHANGE_DISABLED:
						if (searchResult.UserCannotChangePassword == true) {
							users.Add(searchResult);
						}
						break;

					// Advanced queries.

					// Default query.
					case UserQuery.ALL:
					default:
						users.Add(searchResult);
						break;
				}
			}

			// Return the found users.
			return users;
		} // GetAllUsers

		/// <summary>
		/// Gets the group identified by the group id.
		/// The group id can be Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The group id to find.</param>
		/// <returns>The matching group or null.</returns>
		public GroupPrincipal GetGroup(String groupId) {
			GroupPrincipal group = null;

			// Search guid.
			Guid groupGuid = Guid.Empty;
			if ((group == null) && (Guid.TryParse(groupId, out groupGuid) == true)) {
				try {
					group = GroupPrincipal.FindByIdentity(this.context, IdentityType.Guid, groupId);
				} catch { }
			}

			// Search distinguished name.
			if (group == null) {
				try {
					group = GroupPrincipal.FindByIdentity(this.context, IdentityType.DistinguishedName, groupId);
				} catch { }
			}

			// Search sam account name.
			if (group == null) {
				try {
					group = GroupPrincipal.FindByIdentity(this.context, IdentityType.SamAccountName, groupId);
				} catch { }
			}

			// Search user principal name.
			if (group == null) {
				try {
					group = GroupPrincipal.FindByIdentity(this.context, IdentityType.UserPrincipalName, groupId);
				} catch { }
			}

			// Search security identifier.
			if (group == null) {
				try {
					group = GroupPrincipal.FindByIdentity(this.context, IdentityType.Sid, groupId);
				} catch { }
			}

			// Return the group or null.
			return group;
		} // GetGroup

		/// <summary>
		/// Gets all groups.
		/// </summary>
		/// <returns>All groups.</returns>
		public List<GroupPrincipal> GetAllGroups() {
			List<GroupPrincipal> groups = new List<GroupPrincipal>();

			// Create the query filter.
			GroupPrincipal groupQueryFilter = new GroupPrincipal(this.context);
			groupQueryFilter.Name = "*";

			// Create the searcher, and set the PageSize on the underlying DirectorySearcher to get all entries.
			PrincipalSearcher searcher = new PrincipalSearcher();
			searcher.QueryFilter = groupQueryFilter;
			((DirectorySearcher)searcher.GetUnderlyingSearcher()).PageSize = Int32.MaxValue;

			// Execute the search.
			PrincipalSearchResult<Principal> searchResults = searcher.FindAll();
			foreach (Principal searchResult in searchResults) {
				groups.Add((GroupPrincipal)searchResult);
			}

			// Return the found groups.
			return groups;
		} // GetAllGroups

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
		public Boolean IsUserMemberOfGroup(Person user, GroupPrincipal group, Boolean recursive = true) {
			// Get the direct member groups.
			List<GroupPrincipal> testetGroups = new List<GroupPrincipal>();
			Stack<GroupPrincipal> groups = new Stack<GroupPrincipal>();
			foreach (GroupPrincipal memberGroup in user.GetGroups()) {
				groups.Push(memberGroup);
			}

			// Process.
			while (groups.Count > 0) {
				// Get the group.
				GroupPrincipal memberGroup = groups.Pop();

				// Test the group.
				if (group.Equals(memberGroup) == true) {
					return true;
				}

				// Remember that the group is processed.
				testetGroups.Add(memberGroup);

				// Get child groups.
				foreach (GroupPrincipal childGroup in memberGroup.GetGroups()) {
					if (testetGroups.Contains(childGroup) == false) {
						groups.Push(childGroup);
					}
				}
			}

			// Return false.
			return false;
		} // IsUserMemberOfGroup

	} // ActiveDirectory
	#endregion

	#region Person class.
	[DirectoryRdnPrefix("CN")]
	[DirectoryObjectClass("user")]  // person
	public class Person : UserPrincipal {
		private PersonSearchFilter searchFilter = null;

		public Person(PrincipalContext context) : base(context) {
		} // Person

		public Person(PrincipalContext context, String samAccountName, String password, Boolean enabled) : base(context, samAccountName, password, enabled) {
		} // Person

// TODO: This custom filter don't work - should be removed or fixed.
//		public new PersonSearchFilter AdvancedSearchFilter {
//			get {
//				if (searchFilter == null) {
//					searchFilter = new PersonSearchFilter(this);
//				}
//				return searchFilter;
//			}
//		} // AdvancedSearchFilter

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
	#endregion

	#region PersonSearchFilter class.
	// TODO: This custom filter don't work - should be removed or fixed.
	public class PersonSearchFilter : AdvancedFilters {

		public PersonSearchFilter(Principal principal) : base(principal) {
		} // PersonSearchFilter

		public void LogonCount(Int32 value, MatchType matchType) {
			this.AdvancedFilterSet("LogonCount", value, typeof(Int32), matchType);
		} // LogonCount

	} // PersonSearchFilter
	#endregion

} // NDK.Framework