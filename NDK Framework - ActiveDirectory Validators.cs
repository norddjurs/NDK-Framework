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

		private List<AdGroup> configWhiteGroupOneGroups = null;
		private List<AdGroup> configWhiteGroupAllGroups = null;
		private List<AdGroup> configBlackGroupOneGroups = null;
		private List<AdGroup> configBlackGroupAllGroups = null;

		private List<String> configWhiteGroupOneMembers = null;
		private List<String> configWhiteGroupAllMembers = null;
		private List<String> configBlackGroupOneMembers = null;
		private List<String> configBlackGroupAllMembers = null;

		#region Constructors.
		/// <summary>
		/// Connect to the Active Directory as the current user.
		/// </summary>
		/// <param name="framework">The framework.</param>
		public ActiveDirectoryUserValidator(IFramework framework) : this(framework, null, null) {
		} // ActiveDirectoryUserValidator

		/// <summary>
		/// Connect to the Active Directory as the current user.
		/// </summary>
		/// <param name="framework">The framework.</param>
		/// <param name="keyPrefix">The prefix inserted first to the configuration keys.</param>
		/// <param name="keySufflix">The sufflix added to the configuration keys.</param>
		public ActiveDirectoryUserValidator(IFramework framework, String keyPrefix, String keySufflix = null) {
			this.framework = framework;

			if (keyPrefix == null) {
				keyPrefix = String.Empty;
			}

			if (keySufflix == null) {
				keySufflix = String.Empty;
			}

			this.configFailOnGroupNotFound = this.framework.GetLocalValue(keyPrefix + "FailOnGroupNotFound" + keySufflix, true);

			this.configWhiteGroupOneStrings = this.framework.GetLocalValues(keyPrefix + "WhiteGroupsOne" + keySufflix);
			this.configWhiteGroupAllStrings = this.framework.GetLocalValues(keyPrefix + "WhiteGroupsAll" + keySufflix);
			this.configBlackGroupOneStrings = this.framework.GetLocalValues(keyPrefix + "BlackGroupsOne" + keySufflix);
			this.configBlackGroupAllStrings = this.framework.GetLocalValues(keyPrefix + "BlackGroupsAll" + keySufflix);

			this.configWhiteGroupOneGroups = new List<AdGroup>();
			this.configWhiteGroupAllGroups = new List<AdGroup>();
			this.configBlackGroupOneGroups = new List<AdGroup>();
			this.configBlackGroupAllGroups = new List<AdGroup>();

			this.configWhiteGroupOneMembers = new List<String>();
			this.configWhiteGroupAllMembers = new List<String>();
			this.configBlackGroupOneMembers = new List<String>();
			this.configBlackGroupAllMembers = new List<String>();

			// Get the 'white groups one' groups.
			foreach (String configWhiteGroupOneString in configWhiteGroupOneStrings) {
				AdGroup configWhiteGroupOneGroup = this.framework.GetGroup(configWhiteGroupOneString);
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
				AdGroup configWhiteGroupAllGroup = this.framework.GetGroup(configWhiteGroupAllString);
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
				AdGroup configBlackGroupOneGroup = this.framework.GetGroup(configBlackGroupOneString);
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
				AdGroup configBlackGroupAllGroup = this.framework.GetGroup(configBlackGroupAllString);
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

			// To speed up many validations, the members are found and cached.
			foreach (AdGroup group in this.configWhiteGroupOneGroups) {
				foreach (AdUser user in group.GetMembers(true)) {
					this.configWhiteGroupOneMembers.Add(user.SamAccountName);
				}
			}
			foreach (AdGroup group in this.configWhiteGroupAllGroups) {
				foreach (AdUser user in group.GetMembers(true)) {
					this.configWhiteGroupAllMembers.Add(user.SamAccountName);
				}
			}
			foreach (AdGroup group in this.configBlackGroupOneGroups) {
				foreach (AdUser user in group.GetMembers(true)) {
					this.configBlackGroupOneMembers.Add(user.SamAccountName);
				}
			}
			foreach (AdGroup group in this.configBlackGroupAllGroups) {
				foreach (AdUser user in group.GetMembers(true)) {
					this.configBlackGroupAllMembers.Add(user.SamAccountName);
				}
			}
		} // ActiveDirectoryUserValidator
		#endregion

		#region Properties.
		public Boolean FailOnGroupNotFound {
			get {
				return this.configFailOnGroupNotFound;
			}
		} // FailOnGroupNotFound

		public AdGroup[] WhiteGroupsOne {
			get {
				return this.configWhiteGroupOneGroups.ToArray();
			}
		} // WhiteGroupsOne

		public AdGroup[] WhiteGroupsAll {
			get {
				return this.configWhiteGroupAllGroups.ToArray();
			}
		} // WhiteGroupsAll

		public AdGroup[] BlackGroupsOne {
			get {
				return this.configBlackGroupOneGroups.ToArray();
			}
		} // BlackGroupsOne

		public AdGroup[] BlackGroupsAll {
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
			// Only validate users that we know is a member of one of the groups.
			if (this.configWhiteGroupOneMembers.Contains(user.SamAccountName) == true) {
				foreach (AdGroup configWhiteGroupOneGroup in this.configWhiteGroupOneGroups) {
					if (this.framework.IsUserMemberOfGroup(user, configWhiteGroupOneGroup, true) == true) {
						validWhiteGroupOneCount++;

						// Log.
						this.framework.LogDebug("The user is member of group '{0}' (WhiteGroupOne).", configWhiteGroupOneGroup.Name);
					} else {
						// Log.
						this.framework.LogDebug("The user is not member of group '{0}' (WhiteGroupOne).", configWhiteGroupOneGroup.Name);
					}
				}
			}
			validWhiteGroupOne = ((configWhiteGroupOneGroups.Count == 0) || (validWhiteGroupOneCount > 0));

			// Validate that the user is member of all of the 'white groups all' groups.
			// Only validate users that we know is a member of one of the groups.
			if (this.configWhiteGroupAllMembers.Contains(user.SamAccountName) == true) {
				foreach (AdGroup configWhiteGroupAllGroup in this.configWhiteGroupAllGroups) {
					if (this.framework.IsUserMemberOfGroup(user, configWhiteGroupAllGroup, true) == true) {
						validWhiteGroupAllCount++;

						// Log.
						this.framework.LogDebug("The user is member of group '{0}' (WhiteGroupAll).", configWhiteGroupAllGroup.Name);
					} else {
						// Log.
						this.framework.LogDebug("The user is not member of group '{0}' (WhiteGroupAll).", configWhiteGroupAllGroup.Name);
					}
				}
			}
			validWhiteGroupAll = ((configWhiteGroupAllGroups.Count == 0) || (validWhiteGroupAllCount == configWhiteGroupAllGroups.Count));

			// Validate that the user is not member of one of the 'black groups one' groups.
			// Only validate users that we know is a member of one of the groups.
			if (this.configBlackGroupOneMembers.Contains(user.SamAccountName) == true) {
				foreach (AdGroup configBlackGroupOneGroup in this.configBlackGroupOneGroups) {
					if (this.framework.IsUserMemberOfGroup(user, configBlackGroupOneGroup, true) == true) {
						validBlackGroupOneCount++;

						// Log.
						this.framework.LogDebug("The user is member of group '{0}' (BlackGroupOne).", configBlackGroupOneGroup.Name);
					} else {
						// Log.
						this.framework.LogDebug("The user is not member of group '{0}' (BlackGroupOne).", configBlackGroupOneGroup.Name);
					}
				}
			}
			validBlackGroupOne = ((configBlackGroupOneGroups.Count == 0) || (validBlackGroupOneCount == 0));

			// Validate that the user is not member of all of the 'black groups all' groups.
			// Only validate users that we know is a member of one of the groups.
			if (this.configBlackGroupAllMembers.Contains(user.SamAccountName) == true) {
				foreach (AdGroup configBlackGroupAllGroup in this.configBlackGroupAllGroups) {
					if (this.framework.IsUserMemberOfGroup(user, configBlackGroupAllGroup, true) == true) {
						validBlackGroupAllCount++;

						// Log.
						this.framework.LogDebug("The user is member of group '{0}' (BlackGroupAll).", configBlackGroupAllGroup.Name);
					} else {
						// Log.
						this.framework.LogDebug("The user is not member of group '{0}' (BlackGroupAll).", configBlackGroupAllGroup.Name);
					}
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