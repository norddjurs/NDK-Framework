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

	#region IFramework interface
	/// <summary>
	/// Framework interface.
	/// The reason this interface exist, is to ensure that all framework classes provides the minimum set of methods.
	/// </summary>
	public interface IFramework {

		#region Properties.
		/// <summary>
		/// The initialized configuration class.
		/// </summary>
		IConfiguration Config { get; }

		/// <summary>
		/// Gets the initialized logger class.
		/// </summary>
		ILogger Logger { get; }

		/// <summary>
		/// Gets the initialized arguments.
		/// </summary>
		String[] Arguments { get; }

		/// <summary>
		/// Gets the initialized and plugins.
		/// </summary>
		PluginList<IPlugin> Plugins { get; }

		/// <summary>
		/// Gets the initialized active directory.
		/// </summary>
		ActiveDirectory ActiveDirectory { get; }

		/// <summary>
		/// Gets the initialized guid used when referencing resources.
		/// </summary>
		Guid Guid { get; }

		/// <summary>
		/// Gets if the object is initialized.
		/// </summary>
		Boolean IsInitialized { get; }

		/// <summary>
		/// Gets or sets the tagged object.
		/// </summary>
		Object Tag { get; set; }
		#endregion

		#region Initialize methods.
		/// <summary>
		/// Initialize the framework, with defaults.
		/// </summary>
		/// <param name="guid">The guid used when referencing resources.</param>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="arguments">The arguments.</param>
		void Initialize(Guid guid, PluginList<IPlugin> plugins, params String[] arguments);

		/// <summary>
		/// Initialize the framework.
		/// </summary>
		/// <param name="guid">The guid used when referencing resources.</param>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="config">The configuration.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="arguments">The arguments.</param>
		void Initialize(Guid guid, PluginList<IPlugin> plugins, IConfiguration config, ILogger logger, params String[] arguments);
		#endregion

		#region Configuration methods.
		/// <summary>
		/// Gets the configuration keys associated with the initialized guid.
		/// </summary>
		/// <returns>All the keys associated with the initialized guid.</returns>
		String[] GetConfigKeys();

		/// <summary>
		/// Gets the configuration value associated with the initialized guid and the key.
		/// If more then one value is associated with the initialized guid and the key, the first value is returned.
		/// If no value is associated with the initialized guid and the key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		String GetConfigValue(String key, String defaultValue = null);

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
		Boolean GetValue(String key, Boolean defaultValue);

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
		Int32 GetValue(String key, Int32 defaultValue);

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
		DateTime GetValue(String key, DateTime defaultValue);

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
		Guid GetValue(String key, Guid defaultValue);

		/// <summary>
		/// Gets the configuration values associated with the initialized guid and the key.
		/// If no value is associated with the initialized guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetConfigValues(String key);

		/// <summary>
		/// Sets the configuration values associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetConfigValues(String key, params String[] values);
		#endregion

		// TODO: Some current user key/value storage, perhaps in the registry.

		#region Logging events.
		/// <summary>
		/// Occurs when something is logged, and event logging is enabled.
		/// </summary>
		event LoggerEventHandler OnLog;
		#endregion

		#region Logging methods.
		/// <summary>
		/// Writes the text to the log.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		void Log(String text, params Object[] formatArgs);

		/// <summary>
		/// Writes the text to the debug log.
		/// The information is only logged, if DEBUG logging is configured.
		/// </summary>
		/// <param name="text">The debug text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		void LogDebug(String text, params Object[] formatArgs);

		/// <summary>
		/// Writes the text to the error log.
		/// The information is only logged, if ERROR logging is configured.
		/// </summary>
		/// <param name="text">The error text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		void LogError(String text, params Object[] formatArgs);

		/// <summary>
		/// Writes the exception and stack traces to the error log.
		/// The information is only logged, if ERROR logging is configured.
		/// </summary>
		/// <param name="exception">The exception.</param>
		void LogError(Exception exception);
		#endregion

		#region Resource methods.
		/// <summary>
		/// Gets the resource keys, to the resources embedded in the calling assembly.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <returns>The resource keys.</returns>
		String[] GetResourceKeys();

		/// <summary>
		/// Gets the embedded resource string from the calling assembly, identified by the key.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <param name="key">The resource key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The resource data.</returns>
		String GetResourceStr(String key, String defaultValue = null);

		/// <summary>
		/// Gets the embedded resource image from the calling assembly, identified by the key.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <param name="key">The resource key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The resource data.</returns>
		Image GetResourceImage(String key, Image defaultValue = null);
		#endregion

		#region Mail methods.
		/// <summary>
		/// Send e-mail message as plain text to the configured service desk recepient.
		/// </summary>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		Boolean SendMail(String subject, String text, params String[] attachments);

		/// <summary>
		/// Send e-mail message as plain text.
		/// </summary>
		/// <param name="to">One or more to addresses.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		Boolean SendMail(String to, String subject, String text, params String[] attachments);

		/// <summary>
		/// Send e-mail message as plain text or html.
		/// </summary>
		/// <param name="to">One or more to addresses.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="textIsHtml">True if the message text is html code.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		Boolean SendMail(String to, String subject, String text, Boolean textIsHtml, params String[] attachments);

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
		Boolean SendMailFrom(String from, String to, String subject, String text, Boolean textIsHtml, params String[] attachments);
		#endregion

		#region Database methods.
		/// <summary>
		/// Connects to the database identified with the key in the configuration.
		/// </summary>
		/// <param name="key">The database connection identifier.</param>
		/// <returns>The database connection or null.</returns>
		IDbConnection GetSqlConnection(String key);

		/// <summary>
		/// Executes the sql.
		/// The connection must be open.
		/// The sql must use Quoted Identifiers.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="sql">The sql.</param>
		/// <returns>The data reader result, or null.</returns>
		IDataReader ExecuteSql(IDbConnection connection, String sql);
		#endregion

		#region ActiveDirectory methods.
		/// <summary>
		/// Gets the current user.
		/// </summary>
		/// <returns>The current user.</returns>
		Person GetCurrentUser();

		/// <summary>
		/// Gets the user identified by the user id.
		/// The user id can be person number (CPR), Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The user id to find.</param>
		/// <returns>The matching user or null.</returns>
		Person GetUser(String userId);

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <returns>All users.</returns>
		List<Person> GetAllUsers(UserQuery userFilter = UserQuery.ALL);

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <param name="advancedUserFilterDays">Days added/substracted when using advanced user filters.</param>
		/// <returns>All users.</returns>
		List<Person> GetAllUsers(UserQuery userFilter, Int32 advancedUserFilterDays = 0);

		/// <summary>
		/// Gets the group identified by the group id.
		/// The group id can be Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The group id to find.</param>
		/// <returns>The matching group or null.</returns>
		GroupPrincipal GetGroup(String groupId);

		/// <summary>
		/// Gets all groups.
		/// </summary>
		/// <returns>All groups.</returns>
		List<GroupPrincipal> GetAllGroups();

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
		Boolean IsUserMemberOfGroup(Person user, GroupPrincipal group, Boolean recursive = true);
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
		void TrySendEvent(PluginEvents eventId, Object keyValuePairs = null);

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
		void TrySendEvent(Guid pluginGuid, Int32 eventId, Object keyValuePairs = null);

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
		void SendEvent(PluginEvents eventId, Object keyValuePairs = null);

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
		void SendEvent(Guid pluginGuid, Int32 eventId, Object keyValuePairs = null);
		#endregion

	} // IFramework
	#endregion

	#region FrameworkBase
	/// <summary>
	/// FrameworkBase class.
	/// The reason this class exist, is to provide one implemtation that is used in all framework classes.
	/// </summary>
	public class FrameworkBase : IFramework {

		#region Properties.
		/// <summary>
		/// The initialized configuration class.
		/// </summary>
		public IConfiguration Config { get; private set; }

		/// <summary>
		/// Gets the initialized logger class.
		/// </summary>
		public ILogger Logger { get; private set; }

		/// <summary>
		/// Gets the initialized arguments.
		/// </summary>
		public String[] Arguments { get; private set; }

		/// <summary>
		/// Gets the initialized plugins.
		/// </summary>
		public PluginList<IPlugin> Plugins { get; private set; }

		/// <summary>
		/// Gets the initialized active directory.
		/// </summary>
		public ActiveDirectory ActiveDirectory { get; private set; }

		/// <summary>
		/// Gets the initialized guid used when referencing resources.
		/// </summary>
		public Guid Guid { get; private set; }

		/// <summary>
		/// Gets if the object is initialized.
		/// </summary>
		public Boolean IsInitialized {
			get {
				return ((this.Config != null) || (this.Logger != null));
			}
		} // IsInitialized

		/// <summary>
		/// Gets or sets the tagged object.
		/// </summary>
		public Object Tag { get; set; }
		#endregion

		#region Initialize methods.
		/// <summary>
		/// Initialize the framework, with defaults.
		/// </summary>
		/// <param name="guid">The guid used when referencing resources.</param>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="arguments">The arguments.</param>
		public void Initialize(Guid guid, PluginList<IPlugin> plugins, params String[] arguments) {
			this.Guid = guid;
			this.Config = new Configuration();
			this.Logger = new Logger(this.Config);
			this.Plugins = plugins;
			this.Arguments = arguments;
			this.ActiveDirectory = new ActiveDirectory(this.Config);

			// Initialize all plugins, that are not initialized.
			foreach (IPlugin plugin in this.Plugins) {
				if (plugin.IsInitialized == false) {
					plugin.Initialize(this.Plugins, this.Config, this.Logger, this.Arguments);
				}
			}
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
			this.Guid = guid;
			this.Config = config;
			this.Logger = logger;
			this.Plugins = plugins;
			this.Arguments = arguments;
			this.ActiveDirectory = new ActiveDirectory(this.Config);

			// Initialize all plugins, that are not initialized.
			foreach (IPlugin plugin in this.Plugins) {
				if (plugin.IsInitialized == false) {
					plugin.Initialize(this.Plugins, this.Config, this.Logger, this.Arguments);
				}
			}
		} // Initialize
		#endregion

		#region Configuration methods.
		/// <summary>
		/// Gets the configuration keys associated with the initialized guid.
		/// </summary>
		/// <returns>All the keys associated with the initialized guid.</returns>
		public String[] GetConfigKeys() {
			return this.Config.GetKeys(this.Guid);
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
			return this.Config.GetValue(this.Guid, key, defaultValue);
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
			return this.Config.GetValue(this.Guid, key, defaultValue);
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
			return this.Config.GetValue(this.Guid, key, defaultValue);
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
			return this.Config.GetValue(this.Guid, key, defaultValue);
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
			return this.Config.GetValue(this.Guid, key, defaultValue);
		} // GetValue

		/// <summary>
		/// Gets the configuration values associated with the initialized guid and the key.
		/// If no value is associated with the initialized guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetConfigValues(String key) {
			return this.Config.GetValues(this.Guid, key);
		} // GetConfigValues

		/// <summary>
		/// Sets the configuration values associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetConfigValues(String key, params String[] values) {
			this.Config.SetValues(this.Guid, key, values);
		} // SetConfigValues
		#endregion

		// TODO: Some current user key/value storage, perhaps in the registry.

		#region Logging events.
		/// <summary>
		/// Occurs when something is logged, and event logging is enabled.
		/// </summary>
		public event LoggerEventHandler OnLog {
			add {
				this.Logger.OnLog += value;
			}
			remove {
				this.Logger.OnLog -= value;
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
			this.Logger.Log(text, formatArgs);
		} // Log

		/// <summary>
		/// Writes the text to the debug log.
		/// The information is only logged, if DEBUG logging is configured.
		/// </summary>
		/// <param name="text">The debug text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		public void LogDebug(String text, params Object[] formatArgs) {
			this.Logger.LogDebug(text, formatArgs);
		} // LogDebug

		/// <summary>
		/// Writes the text to the error log.
		/// The information is only logged, if ERROR logging is configured.
		/// </summary>
		/// <param name="text">The error text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		public void LogError(String text, params Object[] formatArgs) {
			this.Logger.LogError(text, formatArgs);
		} // LogError

		/// <summary>
		/// Writes the exception and stack traces to the error log.
		/// The information is only logged, if ERROR logging is configured.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public void LogError(Exception exception) {
			this.Logger.LogError(exception);
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
			List<String> keys = new List<String>();

			// Get all resources in the calling assembly, and strip the "<namespace>.resources" part from the resource names.
			foreach (String name in Assembly.GetCallingAssembly().GetManifestResourceNames()) {
				Int32 index = name.ToLower().IndexOf(".resources.");
				if (index > -1) {
					keys.Add(name.Substring(index + 11));
				}
			}

			// Return the names.
			return keys.ToArray();
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
			try {
				// Log.
				this.Logger.LogDebug("Resource: Reading resource '{0}' as text.", key);

				// Find the resource with the name matching the key.
				foreach (String name in Assembly.GetCallingAssembly().GetManifestResourceNames()) {
					Int32 index = name.ToLower().IndexOf(".resources.");
					if ((index > -1) && (name.ToLower().Substring(index + 11).Equals(key.ToLower()) == true)) {
						using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(name)) {
							using (TextReader text = new StreamReader(stream, Encoding.Default)) {
								return text.ReadToEnd();
							}
						}
					}
				}

				// Log.
				this.Logger.LogDebug("Resource: Resource not found.");

				// Return the default value.
				return defaultValue;
			} catch (Exception exception) {
				this.Logger.LogError(exception);

				// Return the default value.
				return defaultValue;
			}
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
			try {
				// Log.
				this.Logger.LogDebug("Resource: Reading resource '{0}' as image.", key);

				// Find the resource with the name matching the key.
				foreach (String name in Assembly.GetCallingAssembly().GetManifestResourceNames()) {
					Int32 index = name.ToLower().IndexOf(".resources.");
					if ((index > -1) && (name.ToLower().Substring(index + 11).Equals(key.ToLower()) == true)) {
						return new Bitmap(Assembly.GetCallingAssembly().GetManifestResourceStream(name));
					}
				}

				// Log.
				this.Logger.LogDebug("Resource: Resource not found.");

				// Return the default value.
				return defaultValue;
			} catch (Exception exception) {
				this.Logger.LogError(exception);

				// Return the default value.
				return defaultValue;
			}
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
			// Get configuration.
			String from = this.Config.GetValue("SmtpFrom", "noreply@internal");
			String to = this.Config.GetValue("SmtpTo", "noreply@internal");

			// Send the message.
			return this.SendMailFrom(from, to, subject, text, false, attachments);
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
			// Get configuration.
			String from = this.Config.GetValue("SmtpFrom", "noreply@internal");

			// Send the message.
			return this.SendMailFrom(from, to, subject, text, false, attachments);
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
			// Get configuration.
			String from = this.Config.GetValue("SmtpFrom", "noreply@internal");

			// Send the message.
			return this.SendMailFrom(from, to, subject, text, textIsHtml, attachments);
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
			try {
				// Get configuration.
				String smtpHost = this.Config.GetValue("SmtpHost");
				Int32 smtpPort = 25;
				Int32.TryParse(this.Config.GetValue("SmtpPort", "25"), out smtpPort);

				// Log.
				this.Logger.LogDebug("Mail: Sending '{2}' to '{1}' from '{0}'. Message contain {3} character(s).", from, to, subject, text.Length);

				using (MailMessage message = new MailMessage()) {
					// Create the message.
					message.From = new MailAddress(from);
					message.To.Add(to);
					message.SubjectEncoding = Encoding.UTF8;
					message.Subject = subject;
					message.BodyEncoding = Encoding.UTF8;
					message.IsBodyHtml = textIsHtml;
					message.Body = text;

					// Attach files.
					foreach (String attachment in attachments) {
						if ((attachment != null) && (File.Exists(attachment) == true)) {
							message.Attachments.Add(new Attachment(attachment));
							this.Logger.LogDebug("Mail: Attaching file '{0}'.", attachment);
						} else {
							this.Logger.LogError("Mail: Attachment not found '{0}'.", attachment);
						}
					}

					// Send the message.
					using (SmtpClient client = new SmtpClient(smtpHost, smtpPort)) {
						client.Send(message);
					}
				}

				// Success.
				return true;
			} catch (Exception exception) {
				// Log.
				this.Logger.LogError(exception);

				// Failure.
				return false;
			}
		} // SendMailFrom
		#endregion

		#region Database methods.
		/// <summary>
		/// Connects to the database identified with the key in the configuration.
		/// </summary>
		/// <param name="key">The database connection identifier.</param>
		/// <returns>The database connection or null.</returns>
		public IDbConnection GetSqlConnection(String key) {
			try {
				// Get configuration.
				Int32 dbTimeout = 30;
				String dbEngine = this.Config.GetValue("SqlEngine" + key, "0");
				String dbHost = this.Config.GetValue("SqlHost" + key, "localhost");
				String dbName = this.Config.GetValue("SqlDatabase" + key);
				String dbUserName = this.Config.GetValue("SqlUserid" + key);
				String dbUserPassword = this.Config.GetValue("SqlPassword" + key);

				// Log.
				this.Logger.Log("SQL: Connecting to database '{2}' at '{1}' as '{3}'", dbEngine, dbHost, dbName, ((dbUserName != null) ? dbUserName : "SSPI (" + Environment.UserName + ")"));

				// Connect to the database.
				IDbConnection dbConnection = null;
				switch (dbEngine.ToLower()) {
					case "0":
					case "mssql":
					default:
						// Create the connection.
						dbConnection = new SqlConnection();
						if ((dbUserName != null) && (dbUserName != String.Empty) && (dbUserPassword != null) && (dbUserPassword != String.Empty)) {
							// SQL server authentication.
							dbConnection.ConnectionString = String.Format("data source = {0}; database = {1}; user id = {2}; password = {3}; Connect Timeout = {4}; Pooling = False;", dbHost, dbName, dbUserName, dbUserPassword, dbTimeout);
						} else {
							// Windows authentication.
							dbConnection.ConnectionString = String.Format("data source = {0}; database = {1}; integrated security = SSPI; Connect Timeout = {4}; Pooling = False;", dbHost, dbName, dbUserName, dbUserPassword, dbTimeout);
						}

						// Open the connection.
						dbConnection.Open();

						// Enable quoted identifiers.
						// Execute the SQL command.
						if (dbConnection.State == ConnectionState.Open) {
							using (IDataReader dataReader = this.ExecuteSql(dbConnection, "SET QUOTED_IDENTIFIER ON;")) {
							}
						}
						break;
				}

				// Return the database connection.
				return dbConnection;
			} catch (Exception exception) {
				// Log.
				this.Logger.LogError(exception);

				// Return null;
				return null;
			}
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
			try {
				// Create the command object.
				IDbCommand command = connection.CreateCommand();
				command.CommandText = sql;
				//command.Transaction = null;
				//command.CommandTimeout = 30 * 1000;

				// Log.
				this.Logger.LogDebug("SQL: Execute sql on '{0}':", connection.Database);
				this.Logger.LogDebug(command.CommandText);

				// Execute the SQL command.
				IDataReader dataReader = command.ExecuteReader();

				// Log.
				if (dataReader.RecordsAffected < 0) {
					this.Logger.LogDebug("SQL: {0} fields in each record.", dataReader.FieldCount);
				} else {
					this.Logger.LogDebug("SQL: {0} rows affected.", dataReader.RecordsAffected);
				}

				// Return the data reader.
				return dataReader;
			} catch (Exception exception) {
				// Log.
				this.Logger.LogError(exception);

				// Return null;
				return null;
			}
		} // ExecuteSql
		#endregion

		#region ActiveDirectory methods.
		/// <summary>
		/// Gets the current user.
		/// </summary>
		/// <returns>The current user.</returns>
		public Person GetCurrentUser() {
			return this.ActiveDirectory.GetCurrentUser();
		} // GetCurrentUser

		/// <summary>
		/// Gets the user identified by the user id.
		/// The user id can be person number (CPR), Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The user id to find.</param>
		/// <returns>The matching user or null.</returns>
		public Person GetUser(String userId) {
			return this.ActiveDirectory.GetUser(userId);
		} // GetUser

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <returns>All users.</returns>
		public List<Person> GetAllUsers(UserQuery userFilter = UserQuery.ALL) {
			return this.ActiveDirectory.GetAllUsers(userFilter, 0);
		} // GetAllUsers

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <param name="advancedUserFilterDays">Days added/substracted when using advanced user filters.</param>
		/// <returns>All users.</returns>
		public List<Person> GetAllUsers(UserQuery userFilter, Int32 advancedUserFilterDays = 0) {
			return this.ActiveDirectory.GetAllUsers(userFilter, advancedUserFilterDays);
		} // GetAllUsers

		/// <summary>
		/// Gets the group identified by the group id.
		/// The group id can be Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The group id to find.</param>
		/// <returns>The matching group or null.</returns>
		public GroupPrincipal GetGroup(String groupId) {
			return this.ActiveDirectory.GetGroup(groupId);
		} // GetGroup

		/// <summary>
		/// Gets all groups.
		/// </summary>
		/// <returns>All groups.</returns>
		public List<GroupPrincipal> GetAllGroups() {
			return this.ActiveDirectory.GetAllGroups();
		} // GetAllGroups

		/// <summary>
		/// Gets if the current user is member of the group.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns>True if the current user is member of the group.</returns>
		public Boolean IsUserMemberOfGroup(GroupPrincipal group, Boolean recursive = true) {
			return this.ActiveDirectory.IsUserMemberOfGroup(group, recursive);
		} // IsUserMemberOfGroup

		/// <summary>
		/// Gets if the user is member of the group.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns>True if the user is member of the group.</returns>
		public Boolean IsUserMemberOfGroup(Person user, GroupPrincipal group, Boolean recursive = true) {
			return this.ActiveDirectory.IsUserMemberOfGroup(user, group, recursive);
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
			try {
				// Send event.
				this.SendEvent(eventId, keyValuePairs);
			} catch (Exception exception) {
				// Log.
				this.Logger.LogError(exception);
			}
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
			try {
				// Send event.
				this.SendEvent(pluginGuid, eventId, keyValuePairs);
			} catch (Exception exception) {
				// Log.
				this.Logger.LogError(exception);
			}
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

	} // FrameworkBase
	#endregion

} // NDK.Framework