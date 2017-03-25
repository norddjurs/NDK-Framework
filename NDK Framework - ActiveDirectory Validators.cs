using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Text;

namespace NDK.Framework {

	#region ActiveDirectoryUserValidator class.
	/// <summary>
	/// This class can be used to validate a user, according to the configured values.
	/// </summary>
	public class ActiveDirectoryUserValidator {
		private IFramework framework;

		private Boolean configFailOnGroupNotFound = true;

		private List<String> configWhiteGroupOneStrings = null;
		private List<String> configWhiteGroupAllStrings = null;
		private List<String> configBlackGroupOneStrings = null;
		private List<String> configBlackGroupAllStrings = null;

		private List<GroupPrincipal> configWhiteGroupOneGroups = null;
		private List<GroupPrincipal> configWhiteGroupAllGroups = null;
		private List<GroupPrincipal> configBlackGroupOneGroups = null;
		private List<GroupPrincipal> configBlackGroupAllGroups = null;

		#region Constructors.
		/// <summary>
		/// Connect to the Active Directory as the current user.
		/// </summary>
		/// <param name="framework">The framework.</param>
		/// <param name="keySufflix">The sufflix added to the configuration keys.</param>
		public ActiveDirectoryUserValidator(IFramework framework, String keySufflix = null) {
			this.framework = framework;

			if (keySufflix == null) {
				keySufflix = String.Empty;
			}

			this.configFailOnGroupNotFound = this.framework.GetLocalValue("FailOnGroupNotFound", true);

			this.configWhiteGroupOneStrings = this.framework.GetLocalValues("WhiteGroupsOne" + keySufflix);
			this.configWhiteGroupAllStrings = this.framework.GetLocalValues("WhiteGroupsAll" + keySufflix);
			this.configBlackGroupOneStrings = this.framework.GetLocalValues("BlackGroupsOne" + keySufflix);
			this.configBlackGroupAllStrings = this.framework.GetLocalValues("BlackGroupsAll" + keySufflix);

			this.configWhiteGroupOneGroups = new List<GroupPrincipal>();
			this.configWhiteGroupAllGroups = new List<GroupPrincipal>();
			this.configBlackGroupOneGroups = new List<GroupPrincipal>();
			this.configBlackGroupAllGroups = new List<GroupPrincipal>();

			// Get the 'white groups one' groups.
			foreach (String configWhiteGroupOneString in configWhiteGroupOneStrings) {
				GroupPrincipal configWhiteGroupOneGroup = this.framework.GetGroup(configWhiteGroupOneString);
				if (configWhiteGroupOneGroup != null) {
					// The group was found.
					this.configWhiteGroupOneGroups.Add(configWhiteGroupOneGroup);

					// Log.
					this.framework.LogDebug("Found group '{0}' (WhiteGroupOne).", configWhiteGroupOneGroup.Name);
				} else {
					// Log that the group was not found.
					this.framework.LogError("Unable to find group '{0}' (WhiteGroupOne).", configWhiteGroupOneString);
				}
			}

			// Get the 'white groups all' groups.
			foreach (String configWhiteGroupAllString in configWhiteGroupAllStrings) {
				GroupPrincipal configWhiteGroupAllGroup = this.framework.GetGroup(configWhiteGroupAllString);
				if (configWhiteGroupAllGroup != null) {
					// The group was found.
					this.configWhiteGroupAllGroups.Add(configWhiteGroupAllGroup);

					// Log.
					this.framework.LogDebug("Found group '{0}' (WhiteGroupAll).", configWhiteGroupAllGroup.Name);
				} else {
					// Log that the group was not found.
					this.framework.LogError("Unable to find group '{0}' (WhiteGroupOne).", configWhiteGroupAllString);
				}
			}

			// Get the 'black groups one' groups.
			foreach (String configBlackGroupOneString in configBlackGroupOneStrings) {
				GroupPrincipal configBlackGroupOneGroup = this.framework.GetGroup(configBlackGroupOneString);
				if (configBlackGroupOneGroup != null) {
					// The group was found.
					this.configBlackGroupOneGroups.Add(configBlackGroupOneGroup);

					// Log.
					this.framework.LogDebug("Found group '{0}' (BlackGroupOne).", configBlackGroupOneGroup.Name);
				} else {
					// Log that the group was not found.
					this.framework.LogError("Unable to find group '{0}' (BlackGroupOne).", configBlackGroupOneString);
				}
			}

			// Get the 'black groups all' groups.
			foreach (String configBlackGroupAllString in configBlackGroupAllStrings) {
				GroupPrincipal configBlackGroupAllGroup = this.framework.GetGroup(configBlackGroupAllString);
				if (configBlackGroupAllGroup != null) {
					// The group was found.
					this.configBlackGroupAllGroups.Add(configBlackGroupAllGroup);

					// Log.
					this.framework.LogDebug("Found group '{0}' (BlackGroupAll).", configBlackGroupAllGroup.Name);
				} else {
					// Log that the group was not found.
					this.framework.LogError("Unable to find group '{0}' (BlackGroupAll).", configBlackGroupAllString);
				}
			}

			// Fail if not all groups was found.
			if ((this.configFailOnGroupNotFound == true) &&
				(configWhiteGroupOneGroups.Count != configWhiteGroupOneStrings.Count) &&
				(configWhiteGroupAllGroups.Count != configWhiteGroupAllStrings.Count) &&
				(configBlackGroupOneGroups.Count != configBlackGroupOneStrings.Count) &&
				(configBlackGroupAllGroups.Count != configBlackGroupAllStrings.Count)) {
				throw new Exception("One or more groups was not found in the Active Directory. See the DEBUG log for details.");
			}
		} // ActiveDirectoryUserValidator
		#endregion

		#region Properties.
		public GroupPrincipal[] WhiteGroupsOne {
			get {
				return this.configWhiteGroupOneGroups.ToArray();
			}
		} // WhiteGroupsOne

		public GroupPrincipal[] WhiteGroupsAll {
			get {
				return this.configWhiteGroupAllGroups.ToArray();
			}
		} // WhiteGroupsAll

		public GroupPrincipal[] BlackGroupsOne {
			get {
				return this.configBlackGroupOneGroups.ToArray();
			}
		} // BlackGroupsOne

		public GroupPrincipal[] BlackGroupsAll {
			get {
				return this.configBlackGroupAllGroups.ToArray();
			}
		} // BlackGroupsAll
		#endregion

		#region Public methods.
		/// <summary>
		/// Validates if the user is member of or not member of the configured groups.
		/// 
		/// The user must be member of one of the 'WhiteGroupsOne' groups.
		/// The user must be member of all of the 'WhiteGroupsAll' groups.
		/// The user may not be member of one of the 'BlackGroupsOne' groups.
		/// The user may not be member of all of the 'BlackGroupsAll' groups.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns>True if the user is member of or not member of the configured groups.</returns>
		public Boolean ValidateUser(AdUser user) {
			Int32 validWhiteGroupOneCount = 0;
			Int32 validWhiteGroupAllCount = 0;
			Int32 validBlackGroupOneCount = 0;
			Int32 validBlackGroupAllCount = 0;

			Boolean validWhiteGroupOne = false;
			Boolean validWhiteGroupAll = false;
			Boolean validBlackGroupOne = false;
			Boolean validBlackGroupAll = false;

			// Validate that the user is member of one of the 'white groups one' groups.
			foreach (GroupPrincipal configWhiteGroupOneGroup in this.configWhiteGroupOneGroups) {
				if (this.framework.IsUserMemberOfGroup(user, configWhiteGroupOneGroup, true) == true) {
					validWhiteGroupOneCount++;

					// Log.
					this.framework.LogDebug("The user is member of group '{0}' (WhiteGroupOne).", configWhiteGroupOneGroup.Name);
				} else {
					// Log.
					this.framework.LogDebug("The user is not member of group '{0}' (WhiteGroupOne).", configWhiteGroupOneGroup.Name);
				}
			}
			validWhiteGroupOne = ((configWhiteGroupOneGroups.Count == 0) || (validWhiteGroupOneCount > 0));

			// Validate that the user is member of all of the 'white groups all' groups.
			foreach (GroupPrincipal configWhiteGroupAllGroup in this.configWhiteGroupAllGroups) {
				if (this.framework.IsUserMemberOfGroup(user, configWhiteGroupAllGroup, true) == true) {
					validWhiteGroupAllCount++;

					// Log.
					this.framework.LogDebug("The user is member of group '{0}' (WhiteGroupAll).", configWhiteGroupAllGroup.Name);
				} else {
					// Log.
					this.framework.LogDebug("The user is not member of group '{0}' (WhiteGroupAll).", configWhiteGroupAllGroup.Name);
				}
			}
			validWhiteGroupAll = ((configWhiteGroupAllGroups.Count == 0) || (validWhiteGroupAllCount == configWhiteGroupAllGroups.Count));

			// Validate that the user is not member of one of the 'black groups one' groups.
			foreach (GroupPrincipal configBlackGroupOneGroup in this.configBlackGroupOneGroups) {
				if (this.framework.IsUserMemberOfGroup(user, configBlackGroupOneGroup, true) == true) {
					validBlackGroupOneCount++;

					// Log.
					this.framework.LogDebug("The user is member of group '{0}' (BlackGroupOne).", configBlackGroupOneGroup.Name);
				} else {
					// Log.
					this.framework.LogDebug("The user is not member of group '{0}' (BlackGroupOne).", configBlackGroupOneGroup.Name);
				}
			}
			validBlackGroupOne = ((configBlackGroupOneGroups.Count == 0) || (validBlackGroupOneCount == 0));

			// Validate that the user is not member of all of the 'black groups all' groups.
			foreach (GroupPrincipal configBlackGroupAllGroup in this.configBlackGroupAllGroups) {
				if (this.framework.IsUserMemberOfGroup(user, configBlackGroupAllGroup, true) == true) {
					validBlackGroupAllCount++;

					// Log.
					this.framework.LogDebug("The user is member of group '{0}' (BlackGroupAll).", configBlackGroupAllGroup.Name);
				} else {
					// Log.
					this.framework.LogDebug("The user is not member of group '{0}' (BlackGroupAll).", configBlackGroupAllGroup.Name);
				}
			}
			validBlackGroupAll = ((configBlackGroupAllGroups.Count == 0) || (validBlackGroupAllCount != configBlackGroupAllGroups.Count));

			// Return the result.
			if ((validWhiteGroupOne == true) && (validWhiteGroupAll == true) && (validBlackGroupOne == true) && (validBlackGroupAll == true)) {
				return true;
			} else {
				return false;
			}
		} // ValidateUser
		#endregion

	} // ActiveDirectoryUserValidator
	#endregion

} // NDK.Framework