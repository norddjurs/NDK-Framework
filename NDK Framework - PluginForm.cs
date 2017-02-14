using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace NDK.Framework {

	#region PluginForm class.
	/// <summary>
	/// Extend this class and implement abstract methods to create a Windows Form.
	/// 
	/// Because the WinForms designer in VisualStudio need to create an instance of this class, it can not be abstract.
	/// 
	/// Note that the NDK Framework implementations of IConfig and ILogger are awailable, but it is recommended
	/// to use the configuration and logging methods inherited from this class, because they automatically
	/// use the guid etc.
	/// </summary>
	public class PluginForm : Form, IFramework {

		#region Override methods.
		/// <summary>
		/// Initialize the framework and the form.
		/// </summary>
		protected override void OnLoad(EventArgs e) {
			// Load the plugins.
			PluginList<IPlugin> plugins = new PluginList<IPlugin>();

			// Initialize the framework.
			this.framework = new FrameworkBase();
			this.framework.Initialize(this.GetGuid(), plugins);

			// Initialize the form.
			base.Text = this.GetName();

			// Invoke base method.
			base.OnLoad(e);
		} // OnLoad
		#endregion
		

		// Implement IFramework.
		// All those methods simply invoke the same method on the framework object.
		// The methods are implemented here to make it easier for the developper to use the framework.
		private IFramework framework = null;

		#region Properties.
		/// <summary>
		/// The initialized configuration class.
		/// </summary>
		public IConfiguration Config {
			get {
				return this.framework.Config;
			}
		} // Config

		/// <summary>
		/// Gets the initialized logger class.
		/// </summary>
		public ILogger Logger {
			get {
				return this.framework.Logger;
			}
		} // Logger

		/// <summary>
		/// Gets the initialized arguments.
		/// </summary>
		public String[] Arguments {
			get {
				return this.framework.Arguments;
			}
		} // Arguments

		/// <summary>
		/// Gets the initialized plugins.
		/// </summary>
		public PluginList<IPlugin> Plugins {
			get {
				return this.framework.Plugins;
			}
		} // Plugins

		/// <summary>
		/// Gets the initialized active directory.
		/// </summary>
		public ActiveDirectory ActiveDirectory {
			get {
				return this.framework.ActiveDirectory;
			}
		} // ActiveDirectory

		/// <summary>
		/// Gets the initialized guid used when referencing resources.
		/// </summary>
		public Guid Guid {
			get {
				return this.framework.Guid;
			}
		} // Guid

		/// <summary>
		/// Gets if the object is initialized.
		/// </summary>
		public Boolean IsInitialized {
			get {
				return this.framework.IsInitialized;
			}
		} // IsInitialized

		// The Tag property is inherited from the Form class.
		/// <summary>
		/// Gets or sets the tagged object.
		/// </summary>
		//public new Object Tag {
		//	get {
		//		return this.framework.Tag;
		//	}
		//	set {
		//		this.framework.Tag = value;
		//	}
		//} // Tag
		#endregion

		#region Initialize methods.
		/// <summary>
		/// Initialize the framework, with defaults.
		/// </summary>
		/// <param name="guid">The guid used when referencing resources.</param>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="arguments">The arguments.</param>
		public void Initialize(Guid guid, PluginList<IPlugin> plugins, params String[] arguments) {
			this.framework.Initialize(guid, plugins, arguments);
		} // Initialize

		/// <summary>
		/// Initialize the framework.
		/// </summary>
		/// <param name="guid">The guid used when referencing resources.</param>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="config">The configuration.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="arguments">The arguments.</param>
		public void Initialize(Guid guid, PluginList<IPlugin> plugins, IConfiguration config, ILogger logger, params String[] arguments) {
			this.framework.Initialize(guid, plugins, config, logger, arguments);
		} // Initialize
		#endregion

		#region Configuration methods.
		/// <summary>
		/// Gets the configuration keys associated with the initialized guid.
		/// </summary>
		/// <returns>All the keys associated with the initialized guid.</returns>
		public String[] GetConfigKeys() {
			return this.framework.GetConfigKeys();
		} // GetConfigKeys

		/// <summary>
		/// Gets the configuration value associated with the initialized guid and the key.
		/// If more then one value is associated with the initialized guid and the key, the first value is returned.
		/// If no value is associated with the initialized guid and the key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public String GetConfigValue(String key, String defaultValue = null) {
			return this.framework.GetConfigValue(key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// True values are "true", "yes" and "1" in any case.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public Boolean GetValue(String key, Boolean defaultValue) {
			return this.framework.GetValue(key, defaultValue);
		} // GetValue

		/// <summary>
		/// Gets the configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public Int32 GetValue(String key, Int32 defaultValue) {
			return this.framework.GetValue(key, defaultValue);
		} // GetValue

		/// <summary>
		/// Gets the configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public DateTime GetValue(String key, DateTime defaultValue) {
			return this.framework.GetValue(key, defaultValue);
		} // GetValue

		/// <summary>
		/// Gets the configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public Guid GetValue(String key, Guid defaultValue) {
			return this.framework.GetValue(key, defaultValue);
		} // GetValue

		/// <summary>
		/// Gets the configuration values associated with the initialized guid and the key.
		/// If no value is associated with the initialized guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetConfigValues(String key) {
			return this.framework.GetConfigValues(key);
		} // GetConfigValues

		/// <summary>
		/// Sets the configuration values associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetConfigValues(String key, params String[] values) {
			this.framework.SetConfigValues(key, values);
		} // SetConfigValues
		#endregion

		// TODO: Some current user key/value storage, perhaps in the registry.

		#region Logging events.
		/// <summary>
		/// Occurs when something is logged, and event logging is enabled.
		/// </summary>
		public event LoggerEventHandler OnLog {
			add {
				this.framework.OnLog += value;
			}
			remove {
				this.framework.OnLog -= value;
			}
		} // OnLog
		#endregion

		#region Logging methods.
		/// <summary>
		/// Writes the text to the log.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		public void Log(String text, params Object[] formatArgs) {
			this.framework.Log(text, formatArgs);
		} // Log

		/// <summary>
		/// Writes the text to the debug log.
		/// The information is only logged, if DEBUG logging is configured.
		/// </summary>
		/// <param name="text">The debug text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		public void LogDebug(String text, params Object[] formatArgs) {
			this.framework.LogDebug(text, formatArgs);
		} // LogDebug

		/// <summary>
		/// Writes the text to the error log.
		/// The information is only logged, if ERROR logging is configured.
		/// </summary>
		/// <param name="text">The error text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		public void LogError(String text, params Object[] formatArgs) {
			this.framework.LogError(text, formatArgs);
		} // LogError

		/// <summary>
		/// Writes the exception and stack traces to the error log.
		/// The information is only logged, if ERROR logging is configured.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public void LogError(Exception exception) {
			this.framework.LogError(exception);
		} // LogError
		#endregion

		#region Resource methods.
		/// <summary>
		/// Gets the resource keys, to the resources embedded in the calling assembly.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <returns>The resource keys.</returns>
		public String[] GetResourceKeys() {
			return this.framework.GetResourceKeys();
		} // GetResourceKeys

		/// <summary>
		/// Gets the embedded resource string from the calling assembly, identified by the key.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <param name="key">The resource key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The resource data.</returns>
		public String GetResourceStr(String key, String defaultValue = null) {
			return this.framework.GetResourceStr(key, defaultValue);
		} // GetResourceStr

		/// <summary>
		/// Gets the embedded resource image from the calling assembly, identified by the key.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <param name="key">The resource key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The resource data.</returns>
		public Image GetResourceImage(String key, Image defaultValue = null) {
			return this.framework.GetResourceImage(key, defaultValue);
		} // GetResourceImage
		#endregion

		#region Mail methods.
		/// <summary>
		/// Send e-mail message as plain text to the configured service desk recepient.
		/// </summary>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		public Boolean SendMail(String subject, String text, params String[] attachments) {
			return this.framework.SendMail(subject, text, attachments);
		} // SendMail

		/// <summary>
		/// Send e-mail message as plain text.
		/// </summary>
		/// <param name="to">One or more to addresses.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		public Boolean SendMail(String to, String subject, String text, params String[] attachments) {
			return this.framework.SendMail(to, subject, text, attachments);
		} // SendMail

		/// <summary>
		/// Send e-mail message as plain text or html.
		/// </summary>
		/// <param name="to">One or more to addresses.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="textIsHtml">True if the message text is html code.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		public Boolean SendMail(String to, String subject, String text, Boolean textIsHtml, params String[] attachments) {
			return this.framework.SendMail(to, subject, text, textIsHtml, attachments);
		} // SendMail

		/// <summary>
		/// Send e-mail message as plain text or html.
		/// </summary>
		/// <param name="from">The from address.</param>
		/// <param name="to">One or more to addresses.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="textIsHtml">True if the message text is html code.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		public Boolean SendMailFrom(String from, String to, String subject, String text, Boolean textIsHtml, params String[] attachments) {
			return this.framework.SendMailFrom(from, to, subject, text, textIsHtml, attachments);
		} // SendMailFrom
		#endregion

		#region Database methods.
		/// <summary>
		/// Connects to the database identified with the key in the configuration.
		/// </summary>
		/// <param name="key">The database connection identifier.</param>
		/// <returns>The database connection or null.</returns>
		public IDbConnection GetSqlConnection(String key) {
			return this.framework.GetSqlConnection(key);
		} // GetSqlConnection

		/// <summary>
		/// Executes the sql.
		/// The connection must be open.
		/// The sql must use Quoted Identifiers.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="sql">The sql.</param>
		/// <returns>The data reader result, or null.</returns>
		public IDataReader ExecuteSql(IDbConnection connection, String sql) {
			return this.framework.ExecuteSql(connection, sql);
		} // ExecuteSql
		#endregion

		#region ActiveDirectory methods.
		/// <summary>
		/// Gets the current user.
		/// </summary>
		/// <returns>The current user.</returns>
		public Person GetCurrentUser() {
			return this.framework.GetCurrentUser();
		} // GetCurrentUser

		/// <summary>
		/// Gets the user identified by the user id.
		/// The user id can be person number (CPR), Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The user id to find.</param>
		/// <returns>The matching user or null.</returns>
		public Person GetUser(String userId) {
			return this.framework.GetUser(userId);
		} // GetUser

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <returns>All users.</returns>
		public List<Person> GetAllUsers(UserQuery userFilter = UserQuery.ALL) {
			return this.framework.GetAllUsers(userFilter);
		} // GetAllUsers

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <param name="advancedUserFilterDays">Days added/substracted when using advanced user filters.</param>
		/// <returns>All users.</returns>
		public List<Person> GetAllUsers(UserQuery userFilter, Int32 advancedUserFilterDays = 0) {
			return this.framework.GetAllUsers(userFilter, advancedUserFilterDays);
		} // GetAllUsers

		/// <summary>
		/// Gets the group identified by the group id.
		/// The group id can be Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The group id to find.</param>
		/// <returns>The matching group or null.</returns>
		public GroupPrincipal GetGroup(String groupId) {
			return this.framework.GetGroup(groupId);
		} // GetGroup

		/// <summary>
		/// Gets all groups.
		/// </summary>
		/// <returns>All groups.</returns>
		public List<GroupPrincipal> GetAllGroups() {
			return this.framework.GetAllGroups();
		} // GetAllGroups

		/// <summary>
		/// Gets if the current user is member of the group.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns>True if the current user is member of the group.</returns>
		public Boolean IsUserMemberOfGroup(GroupPrincipal group, Boolean recursive = true) {
			return this.framework.IsUserMemberOfGroup(group, recursive);
		} // IsUserMemberOfGroup

		/// <summary>
		/// Gets if the user is member of the group.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns>True if the user is member of the group.</returns>
		public Boolean IsUserMemberOfGroup(Person user, GroupPrincipal group, Boolean recursive = true) {
			return this.framework.IsUserMemberOfGroup(user, group, recursive);
		} // IsUserMemberOfGroup
		#endregion

		// TODO: SOFD methods.
		#region SOFD methods.

		#endregion

		#region Event methods.
		/// <summary>
		/// Executes the RunEvent method on all the loaded plugins.
		/// Exceptions are caught and logged.
		/// 
		/// Only event id lover then 1000 is allowed to be send to all loaded plugins, because you don't know
		/// which plugins might be loaded and available.
		/// 
		/// The keyValuesPairs object must be an anonymous object containing key/value pairs, like this:
		///		new { Key1 = valueObject1, Key2 = valueObject2 }
		/// </summary>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="keyValuePairs">The anonymous object containing key/value pairs.</param>
		public void TrySendEvent(PluginEvents eventId, Object keyValuePairs = null) {
			this.framework.TrySendEvent(eventId, keyValuePairs);
		} // TrySendEvent

		/// <summary>
		/// Executes the RunEvent method on the plugin identified by the guid.
		/// Exceptions are caught and logged.
		/// 
		/// The keyValuesPairs object must be an anonymous object containing key/value pairs, like this:
		///		new { Key1 = valueObject1, Key2 = valueObject2 }
		/// </summary>
		/// <param name="pluginGuid">The plugin guid</param>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="keyValuePairs">The anonymous object containing key/value pairs.</param>
		public void TrySendEvent(Guid pluginGuid, Int32 eventId, Object keyValuePairs = null) {
			this.framework.TrySendEvent(pluginGuid, eventId, keyValuePairs);
		} // TrySendEvent

		/// <summary>
		/// Executes the RunEvent method on all the loaded plugins.
		/// Exceptions are thrown from the RunEvent method.
		/// 
		/// Only event id lover then 1000 is allowed to be send to all loaded plugins, because you don't know
		/// which plugins might be loaded and available.
		/// 
		/// The keyValuesPairs object must be an anonymous object containing key/value pairs, like this:
		///		new { Key1 = valueObject1, Key2 = valueObject2 }
		/// </summary>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="keyValuePairs">The anonymous object containing key/value pairs.</param>
		public void SendEvent(PluginEvents eventId, Object keyValuePairs = null) {
			// Initialize a dictionary from the anonymous object
			IDictionary<String, Object> eventObjects = new Dictionary<String, Object>();
			if (keyValuePairs != null) {
				PropertyInfo[] properties = keyValuePairs.GetType().GetProperties();
				foreach (PropertyInfo prop in properties) {
					eventObjects.Add(prop.Name, prop.GetValue(keyValuePairs, null));
				}
			}

			// Execute the RunEvent methods.
			foreach (IPlugin plugin in this.Plugins) {
				// Log.
				this.Logger.LogDebug("Event: Triggering event id {0} in plugin {1}   {2}.", eventId, plugin.GetGuid(), plugin.GetName());

				// Call the RunEvent method.
				plugin.RunEvent(this.Guid, (Int32)eventId, eventObjects);
			}
		} // SendEvent

		/// <summary>
		/// Executes the RunEvent method on the plugin identified by the guid.
		/// Exceptions are thrown, when the plugin isn't available or from the RunEvent method.
		/// 
		/// The keyValuesPairs object must be an anonymous object containing key/value pairs, like this:
		///		new { Key1 = valueObject1, Key2 = valueObject2 }
		/// </summary>
		/// <param name="pluginGuid">The plugin guid</param>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="keyValuePairs">The anonymous object containing key/value pairs.</param>
		public void SendEvent(Guid pluginGuid, Int32 eventId, Object keyValuePairs = null) {
			// Initialize a dictionary from the anonymous object
			IDictionary<String, Object> eventObjects = new Dictionary<String, Object>();
			if (keyValuePairs != null) {
				PropertyInfo[] properties = keyValuePairs.GetType().GetProperties();
				foreach (PropertyInfo prop in properties) {
					eventObjects.Add(prop.Name, prop.GetValue(keyValuePairs, null));
				}
			}

			// Find plugin and execute the RunEvent method.
			foreach (IPlugin plugin in this.Plugins) {
				if (plugin.GetGuid().Equals(pluginGuid) == true) {
					// Log.
					this.Logger.LogDebug("Event: Triggering event id {0} in plugin {1}   {2}.", eventId, plugin.GetGuid(), plugin.GetName());

					// Call the RunEvent method.
					plugin.RunEvent(this.Guid, eventId, eventObjects);

					// Exit.
					return;
				}
			}

			// The plugin was not found.
			throw new Exception(String.Format("Unable to trigger event in {0} in plugin {1}. No plugin with this guid is loaded.", eventId, pluginGuid));
		} // SendEvent
		#endregion

		#region Abstract and Virtual methods.
		/// <summary>
		/// Gets the unique form guid used when referencing resources.
		/// When implementing a form, this method should return the same unique guid every time. 
		/// </summary>
		/// <returns></returns>
		public virtual Guid GetGuid() {
			// Because the WinForms designer in VisualStudio need to create an instance of this class, it can not be abstract.
			if (base.DesignMode == true) {
				return new Guid("{5FAD8E9F-709D-4DFC-A9A7-C063E7C11367}");
			} else {
				throw new NotImplementedException("The GetGuid method must be overridden and implemented.");
			}
		} // GetGuid

		/// <summary>
		/// Gets the the form name.
		/// When implementing a form, this method should return a proper display name.
		/// </summary>
		/// <returns></returns>
		public virtual String GetName() {
			// Because the WinForms designer in VisualStudio need to create an instance of this class, it can not be abstract.
			if (base.DesignMode == true) {
				return "*** Design Mode ***";
			} else {
				throw new NotImplementedException("The GetGuid method must be overridden and implemented.");
			}
		} // GetName
		#endregion

	} // PluginForm
	#endregion

} // NDK.Framework