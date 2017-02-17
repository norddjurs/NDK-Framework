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
		/// Gets the initialized logger class.
		/// </summary>
		ILogger Logger { get; }

		/// <summary>
		/// The initialized configuration class.
		/// </summary>
		IConfiguration Config { get; }

		/// <summary>
		/// The initialized option class.
		/// </summary>
		IOption Option { get; }

		/// <summary>
		/// Gets the initialized arguments.
		/// </summary>
		String[] Arguments { get; }

		/// <summary>
		/// Gets the initialized and plugins.
		/// </summary>
		PluginList<IPlugin> Plugins { get; }

		/// <summary>
		/// Gets the initialized database manager.
		/// </summary>
		Database Database { get; }

		/// <summary>
		/// Gets the initialized active directory.
		/// </summary>
		ActiveDirectory ActiveDirectory { get; }

		/// <summary>
		/// Gets the initialized sofd directory.
		/// </summary>
		SofdDirectory SofdDirectory { get; }

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

		#region Configuration methods.
		/// <summary>
		/// Gets the local configuration keys associated with the initialized guid.
		/// </summary>
		/// <returns>All the keys associated with the initialized guid.</returns>
		String[] GetConfigKeys();

		/// <summary>
		/// Gets the local configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// True values are "true", "yes" and "1" in any case.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Boolean GetConfigValue(String key, Boolean defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Int32 GetConfigValue(String key, Int32 defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		DateTime GetConfigValue(String key, DateTime defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Guid GetConfigValue(String key, Guid defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the initialized guid and the key.
		/// If more then one value is associated with the initialized guid and the key, the first value is returned.
		/// If no value is associated with the initialized guid and the key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		String GetConfigValue(String key, String defaultValue = null);

		/// <summary>
		/// Gets the local configuration values associated with the initialized guid and the key.
		/// If no value is associated with the initialized guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetConfigValues(String key);

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetConfigValue(String key, Boolean value);

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetConfigValue(String key, Int32 value);

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetConfigValue(String key, DateTime value);

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetConfigValue(String key, Guid value);

		/// <summary>
		/// Sets the local configuration values associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetConfigValues(String key, params String[] values);
		#endregion

		#region Option methods.
		/// <summary>
		/// Gets the user option keys associated with the initialized guid.
		/// </summary>
		/// <returns>All the keys associated with the initialized guid.</returns>
		String[] GetOptionKeys();

		/// <summary>
		/// Gets the user option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// True values are "true", "yes" and "1" in any case.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Boolean GetOptionValue(String key, Boolean defaultValue);

		/// <summary>
		/// Gets the user option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Int32 GetOptionValue(String key, Int32 defaultValue);

		/// <summary>
		/// Gets the user option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		DateTime GetOptionValue(String key, DateTime defaultValue);

		/// <summary>
		/// Gets the user option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Guid GetOptionValue(String key, Guid defaultValue);

		/// <summary>
		/// Gets the user option value associated with the initialized guid and the key.
		/// If more then one value is associated with the initialized guid and the key, the first value is returned.
		/// If no value is associated with the initialized guid and the key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		String GetOptionValue(String key, String defaultValue = null);

		/// <summary>
		/// Gets the user option values associated with the initialized guid and the key.
		/// If no value is associated with the initialized guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetOptionValues(String key);

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetOptionValue(String key, Boolean value);

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetOptionValue(String key, Int32 value);

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetOptionValue(String key, DateTime value);

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetOptionValue(String key, Guid value);

		/// <summary>
		/// Sets the user option values associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetOptionValues(String key, params String[] values);
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
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The resource data.</returns>
		String GetResourceStr(String key, String defaultValue = null);

		/// <summary>
		/// Gets the embedded resource image from the calling assembly, identified by the key.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <param name="key">The resource key.</param>
		/// <param name="defaultValue">The optional default value.</param>
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
		/// <param name="formatArgs">The optional sql string format arguments.</param>
		/// <returns>The data reader result, or null.</returns>
		IDataReader ExecuteSql(IDbConnection connection, String sql, params Object[] formatArgs);

		/// <summary>
		/// Executes a query on the schema and table, filtering the result using the WHERE filters.
		/// The connection must be open.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="schemaName">The schema name.</param>
		/// <param name="tableName">The table name.</param>
		/// <param name="whereFilters">The optional WHERE filters.</param>
		/// <returns>The data reader result, or null.</returns>
		IDataReader ExecuteSql(IDbConnection connection, String schemaName, String tableName, params SqlWhereFilterBase[] whereFilters);
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
		/// Gets the initialized logger class.
		/// </summary>
		public ILogger Logger { get; private set; }

		/// <summary>
		/// The initialized configuration class.
		/// </summary>
		public IConfiguration Config { get; private set; }

		/// <summary>
		/// The initialized option class.
		/// </summary>
		public IOption Option { get; private set; }

		/// <summary>
		/// Gets the initialized arguments.
		/// </summary>
		public String[] Arguments { get; private set; }

		/// <summary>
		/// Gets the initialized plugins.
		/// </summary>
		public PluginList<IPlugin> Plugins { get; private set; }

		/// <summary>
		/// Gets the initialized database manager.
		/// </summary>
		public Database Database { get; private set; }

		/// <summary>
		/// Gets the initialized active directory.
		/// </summary>
		public ActiveDirectory ActiveDirectory { get; private set; }

		/// <summary>
		/// Gets the initialized sofd directory.
		/// </summary>
		public SofdDirectory SofdDirectory { get; private set; }

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
			this.Option = new Option(this.Config, this.Logger);
			this.Plugins = plugins;
			this.Arguments = arguments;
			this.Database = new Database(this);
			this.ActiveDirectory = new ActiveDirectory(this.Config, this.Logger);
			this.SofdDirectory = new SofdDirectory(this);

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
			this.Option = new Option(this.Config, this.Logger);
			this.Plugins = plugins;
			this.Arguments = arguments;
			this.Database = new Database(this);
			this.ActiveDirectory = new ActiveDirectory(this.Config, this.Logger);
			this.SofdDirectory = new SofdDirectory(this);

			// Initialize all plugins, that are not initialized.
			foreach (IPlugin plugin in this.Plugins) {
				if (plugin.IsInitialized == false) {
					plugin.Initialize(this.Plugins, this.Config, this.Logger, this.Arguments);
				}
			}
		} // Initialize
		#endregion

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

		#region Configuration methods.
		/// <summary>
		/// Gets the configuration keys associated with the initialized guid.
		/// </summary>
		/// <returns>All the keys associated with the initialized guid.</returns>
		public String[] GetConfigKeys() {
			return this.Config.GetLocalKeys(this.Guid);
		} // GetConfigKeys

		/// <summary>
		/// Gets the configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// True values are "true", "yes" and "1" in any case.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Boolean GetConfigValue(String key, Boolean defaultValue) {
			return this.Config.GetLocalValue(this.Guid, key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Int32 GetConfigValue(String key, Int32 defaultValue) {
			return this.Config.GetLocalValue(this.Guid, key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public DateTime GetConfigValue(String key, DateTime defaultValue) {
			return this.Config.GetLocalValue(this.Guid, key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Guid GetConfigValue(String key, Guid defaultValue) {
			return this.Config.GetLocalValue(this.Guid, key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration value associated with the initialized guid and the key.
		/// If more then one value is associated with the initialized guid and the key, the first value is returned.
		/// If no value is associated with the initialized guid and the key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public String GetConfigValue(String key, String defaultValue = null) {
			return this.Config.GetLocalValue(this.Guid, key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration values associated with the initialized guid and the key.
		/// If no value is associated with the initialized guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetConfigValues(String key) {
			return this.Config.GetLocalValues(this.Guid, key);
		} // GetConfigValues

		/// <summary>
		/// Sets the configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetConfigValue(String key, Boolean value) {
			this.Config.SetLocalValue(this.Guid, key, value);
		} // SetConfigValue

		/// <summary>
		/// Sets the configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetConfigValue(String key, Int32 value) {
			this.Config.SetLocalValue(this.Guid, key, value);
		} // SetConfigValue

		/// <summary>
		/// Sets the configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetConfigValue(String key, DateTime value) {
			this.Config.SetLocalValue(this.Guid, key, value);
		} // SetConfigValue

		/// <summary>
		/// Sets the configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetConfigValue(String key, Guid value) {
			this.Config.SetLocalValue(this.Guid, key, value);
		} // SetConfigValue

		/// <summary>
		/// Sets the configuration values associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetConfigValues(String key, params String[] values) {
			this.Config.SetLocalValues(this.Guid, key, values);
		} // SetConfigValues
		#endregion

		#region Option methods.
		/// <summary>
		/// Gets the user option keys associated with the initialized guid.
		/// </summary>
		/// <returns>All the keys associated with the initialized guid.</returns>
		public String[] GetOptionKeys() {
			return this.Option.GetUserKeys(this.Guid);
		} // GetOptionKeys

		/// <summary>
		/// Gets the user option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// True values are "true", "yes" and "1" in any case.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Boolean GetOptionValue(String key, Boolean defaultValue) {
			return this.Option.GetUserValue(this.Guid, key, defaultValue);
		} // GetOptionValue

		/// <summary>
		/// Gets the user option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Int32 GetOptionValue(String key, Int32 defaultValue) {
			return this.Option.GetUserValue(this.Guid, key, defaultValue);
		} // GetOptionValue

		/// <summary>
		/// Gets the user option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public DateTime GetOptionValue(String key, DateTime defaultValue) {
			return this.Option.GetUserValue(this.Guid, key, defaultValue);
		} // GetOptionValue

		/// <summary>
		/// Gets the user option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Guid GetOptionValue(String key, Guid defaultValue) {
			return this.Option.GetUserValue(this.Guid, key, defaultValue);
		} // GetOptionValue

		/// <summary>
		/// Gets the user option value associated with the initialized guid and the key.
		/// If more then one value is associated with the initialized guid and the key, the first value is returned.
		/// If no value is associated with the initialized guid and the key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public String GetOptionValue(String key, String defaultValue = null) {
			return this.Option.GetUserValue(this.Guid, key, defaultValue);
		} // GetOptionValue

		/// <summary>
		/// Gets the user option values associated with the initialized guid and the key.
		/// If no value is associated with the initialized guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetOptionValues(String key) {
			return this.Option.GetUserValues(this.Guid, key);
		} // GetOptionValues

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetOptionValue(String key, Boolean value) {
			this.Option.SetUserValue(this.Guid, key, value);
		} // SetOptionValue

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetOptionValue(String key, Int32 value) {
			this.Option.SetUserValue(this.Guid, key, value);
		} // SetOptionValue

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetOptionValue(String key, DateTime value) {
			this.Option.SetUserValue(this.Guid, key, value);
		} // SetOptionValue

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetOptionValue(String key, Guid value) {
			this.Option.SetUserValue(this.Guid, key, value);
		} // SetOptionValue

		/// <summary>
		/// Sets the user option values associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetOptionValues(String key, params String[] values) {
			this.Option.SetUserValues(this.Guid, key, values);
		} // SetOptionValues
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
		/// <param name="defaultValue">The optional default value.</param>
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
		/// <param name="defaultValue">The optional default value.</param>
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
			String from = this.Config.GetSystemValue("SmtpFrom", "noreply@internal");
			String to = this.Config.GetSystemValue("SmtpTo", String.Empty);

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
			String from = this.Config.GetSystemValue("SmtpFrom", "noreply@internal");

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
			String from = this.Config.GetSystemValue("SmtpFrom", "noreply@internal");

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
				String smtpHost = this.Config.GetSystemValue("SmtpHost");
				Int32 smtpPort = 25;
				Int32.TryParse(this.Config.GetSystemValue("SmtpPort", "25"), out smtpPort);

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
			return this.Database.GetDatabaseConnection(key);
		} // GetSqlConnection

		/// <summary>
		/// Executes the sql.
		/// The connection must be open.
		/// The sql must use Quoted Identifiers.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="sql">The sql.</param>
		/// <param name="formatArgs">The optional sql string format arguments.</param>
		/// <returns>The data reader result, or null.</returns>
		public IDataReader ExecuteSql(IDbConnection connection, String sql, params Object[] formatArgs) {
			return this.Database.ExecuteSql(connection, sql, formatArgs);
		} // ExecuteSql

		/// <summary>
		/// Executes a query on the schema and table, filtering the result using the WHERE filters.
		/// The connection must be open.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="schemaName">The schema name.</param>
		/// <param name="tableName">The table name.</param>
		/// <param name="whereFilters">The optional WHERE filters.</param>
		/// <returns>The data reader result, or null.</returns>
		public IDataReader ExecuteSql(IDbConnection connection, String schemaName, String tableName, params SqlWhereFilterBase[] whereFilters) {
			return this.Database.ExecuteSql(connection, schemaName, tableName, whereFilters);
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