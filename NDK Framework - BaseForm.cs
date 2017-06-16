using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Windows.Forms;
using NDK.Framework.CprBroker;

namespace NDK.Framework {

	#region BaseForm class.
	/// <summary>
	/// Inherit this class and implement abstract methods to create a NDK Framework Windows Form, that
	/// provides all the functionality from the framework.
	/// 
	/// Because the WinForms designer in VisualStudio need to create an instance of this class, it can not be abstract.
	/// </summary>
	public class BaseForm : Form, IFramework {

		#region Override methods.
		/// <summary>
		/// Initialize the framework and the form.
		/// </summary>
		protected override void OnLoad(EventArgs e) {
			// Initialize the framework.
			this.framework = new BaseClass(this.GetGuid(), this.GetName());

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

		#region System configuration methods.
		/// <summary>
		/// Gets the local configuration keys associated with the initialized guid.
		/// </summary>
		/// <returns>All the keys associated with the initialized guid.</returns>
		public String[] GetSystemKeys() {
			return this.framework.GetSystemKeys();
		} // GetSystemKeys

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
		public Boolean GetSystemValue(String key, Boolean defaultValue) {
			return this.framework.GetSystemValue(key, defaultValue);
		} // GetSystemValue

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
		public Int32 GetSystemValue(String key, Int32 defaultValue) {
			return this.framework.GetSystemValue(key, defaultValue);
		} // GetSystemValue

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
		public DateTime GetSystemValue(String key, DateTime defaultValue) {
			return this.framework.GetSystemValue(key, defaultValue);
		} // GetSystemValue

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
		public Guid GetSystemValue(String key, Guid defaultValue) {
			return this.framework.GetSystemValue(key, defaultValue);
		} // GetSystemValue

		/// <summary>
		/// Gets the local configuration value associated with the initialized guid and the key.
		/// If more then one value is associated with the initialized guid and the key, the first value is returned.
		/// If no value is associated with the initialized guid and the key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public String GetSystemValue(String key, String defaultValue = null) {
			return this.framework.GetSystemValue(key, defaultValue);
		} // GetSystemValue

		/// <summary>
		/// Gets the local configuration values associated with the initialized guid and the key.
		/// If no value is associated with the initialized guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetSystemValues(String key) {
			return this.framework.GetSystemValues(key);
		} // GetSystemValues

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, Boolean value) {
			this.framework.SetSystemValue(key, value);
		} // SetSystemValue

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, Int32 value) {
			this.framework.SetSystemValue(key, value);
		} // SetSystemValue

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, DateTime value) {
			this.framework.SetSystemValue(key, value);
		} // SetSystemValue

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, Guid value) {
			this.framework.SetSystemValue(key, value);
		} // SetSystemValue

		/// <summary>
		/// Sets the local configuration values associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetSystemValues(String key, params String[] values) {
			this.framework.SetSystemValues(key, values);
		} // SetSystemValues
		#endregion

		#region Local configuration methods.
		/// <summary>
		/// Gets the local configuration keys associated with the initialized guid.
		/// </summary>
		/// <returns>All the keys associated with the initialized guid.</returns>
		public String[] GetLocalKeys() {
			return this.framework.GetLocalKeys();
		} // GetLocalKeys

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
		public Boolean GetLocalValue(String key, Boolean defaultValue) {
			return this.framework.GetLocalValue(key, defaultValue);
		} // GetLocalValue

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
		public Int32 GetLocalValue(String key, Int32 defaultValue) {
			return this.framework.GetLocalValue(key, defaultValue);
		} // GetLocalValue

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
		public DateTime GetLocalValue(String key, DateTime defaultValue) {
			return this.framework.GetLocalValue(key, defaultValue);
		} // GetLocalValue

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
		public Guid GetLocalValue(String key, Guid defaultValue) {
			return this.framework.GetLocalValue(key, defaultValue);
		} // GetLocalValue

		/// <summary>
		/// Gets the local configuration value associated with the initialized guid and the key.
		/// If more then one value is associated with the initialized guid and the key, the first value is returned.
		/// If no value is associated with the initialized guid and the key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public String GetLocalValue(String key, String defaultValue = null) {
			return this.framework.GetLocalValue(key, defaultValue);
		} // GetLocalValue

		/// <summary>
		/// Gets the local configuration values associated with the initialized guid and the key.
		/// If no value is associated with the initialized guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetLocalValues(String key) {
			return this.framework.GetLocalValues(key);
		} // GetLocalValues

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetLocalValue(String key, Boolean value) {
			this.framework.SetLocalValue(key, value);
		} // SetLocalValue

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetLocalValue(String key, Int32 value) {
			this.framework.SetLocalValue(key, value);
		} // SetLocalValue

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetLocalValue(String key, DateTime value) {
			this.framework.SetLocalValue(key, value);
		} // SetLocalValue

		/// <summary>
		/// Sets the local configuration value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetLocalValue(String key, Guid value) {
			this.framework.SetLocalValue(key, value);
		} // SetLocalValue

		/// <summary>
		/// Sets the local configuration values associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetLocalValues(String key, params String[] values) {
			this.framework.SetLocalValues(key, values);
		} // SetLocalValues
		#endregion

		#region Configuration methods.
		/// <summary>
		/// Gets the configuration guids.
		/// </summary>
		/// <returns>All the guids.</returns>
		public Guid[] GetConfigGuids() {
			return this.framework.GetConfigGuids();
		} // GetConfigGuids

		/// <summary>
		/// Gets the configuration keys associated with the guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the guid.</returns>
		public String[] GetConfigKeys(Guid guid) {
			return this.framework.GetConfigKeys(guid);
		} // GetConfigKeys

		/// <summary>
		/// Gets the configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// True values are "true", "yes" and "1" in any case.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public Boolean GetConfigValue(Guid guid, String key, Boolean defaultValue) {
			return this.framework.GetConfigValue(guid, key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public Int32 GetConfigValue(Guid guid, String key, Int32 defaultValue) {
			return this.framework.GetConfigValue(guid, key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public DateTime GetConfigValue(Guid guid, String key, DateTime defaultValue) {
			return this.framework.GetConfigValue(guid, key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public Guid GetConfigValue(Guid guid, String key, Guid defaultValue) {
			return this.framework.GetConfigValue(guid, key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public String GetConfigValue(Guid guid, String key, String defaultValue = null) {
			return this.framework.GetConfigValue(guid, key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration values associated with the guid and key.
		/// If no value is associated with the guid and key, an empty list is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetConfigValues(Guid guid, String key) {
			return this.framework.GetConfigValues(guid, key);
		} // GetConfigValues

		/// <summary>
		/// Sets the configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetConfigValue(Guid guid, String key, Boolean value) {
			this.framework.SetConfigValue(guid, key, value);
		} // SetConfigValue

		/// <summary>
		/// Sets the configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetConfigValue(Guid guid, String key, Int32 value) {
			this.framework.SetConfigValue(guid, key, value);
		} // SetConfigValue

		/// <summary>
		/// Sets the local configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetConfigValue(Guid guid, String key, DateTime value) {
			this.framework.SetConfigValue(guid, key, value);
		} // SetConfigValue

		/// <summary>
		/// Sets the configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetConfigValue(Guid guid, String key, Guid value) {
			this.framework.SetConfigValue(guid, key, value);
		} // SetConfigValue

		/// <summary>
		/// Sets the configuration values associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetConfigValues(Guid guid, String key, params String[] values) {
			this.framework.SetConfigValues(guid, key, values);
		} // SetConfigValues
		#endregion

		#region Option methods.
		/// <summary>
		/// Gets the user option keys associated with the initialized guid.
		/// </summary>
		/// <returns>All the keys associated with the initialized guid.</returns>
		public String[] GetOptionKeys() {
			return this.framework.GetOptionKeys();
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
			return this.framework.GetOptionValue(key, defaultValue);
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
			return this.framework.GetOptionValue(key, defaultValue);
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
			return this.framework.GetOptionValue(key, defaultValue);
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
			return this.framework.GetOptionValue(key, defaultValue);
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
			return this.framework.GetOptionValue(key, defaultValue);
		} // GetOptionValue

		/// <summary>
		/// Gets the user option values associated with the initialized guid and the key.
		/// If no value is associated with the initialized guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetOptionValues(String key) {
			return this.framework.GetOptionValues(key);
		} // GetOptionValues

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetOptionValue(String key, Boolean value) {
			this.framework.SetOptionValue(key, value);
		} // SetOptionValue

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetOptionValue(String key, Int32 value) {
			this.framework.SetOptionValue(key, value);
		} // SetOptionValue

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetOptionValue(String key, DateTime value) {
			this.framework.SetOptionValue(key, value);
		} // SetOptionValue

		/// <summary>
		/// Sets the user option value associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetOptionValue(String key, Guid value) {
			this.framework.SetOptionValue(key, value);
		} // SetOptionValue

		/// <summary>
		/// Sets the user option values associated with the initialized guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetOptionValues(String key, params String[] values) {
			this.framework.SetOptionValues(key, values);
		} // SetOptionValues
		#endregion

		#region Arguments methods.
		/// <summary>
		/// Gets the arguments passed to the executing process.
		/// </summary>
		/// <returns></returns>
		public String[] GetArguments() {
			return this.framework.GetArguments();
		} // GetArguments
		#endregion

		#region Plugin methods.
		/// <summary>
		/// Gets the plugins loaded.
		/// The plugins are objects implementing the IPlugin interface.
		/// The assemblies (DLL and EXE) in the same directory as the "NDK Framework.dll" assembly, are scanned.
		/// </summary>
		/// <param name="reload">Reload new instances of the plugins.</param>
		/// <returns>The loaded plugins.</returns>
		public IPlugin[] GetPlugins(Boolean reload = false) {
			return this.framework.GetPlugins(reload);
		} // GetPlugins
		#endregion

		#region Send event methods.
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
			this.framework.SendEvent(eventId, keyValuePairs);
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
			this.framework.SendEvent(pluginGuid, eventId, keyValuePairs);
		} // SendEvent
		#endregion

		#region Virtual run event methods.
		/// <summary>
		/// Handle events.
		/// This method is invoked by another framework class.
		/// 
		/// When implementing a framework class, only use your own event id greater then 1000. Event id less then 1000 is reserved
		/// for global events. They will be declared in the PluginEvents enum.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="eventObjects">The event objects.</param>
		public virtual void RunEvent(Guid sender, Int32 eventId, IDictionary<String, Object> eventObjects) {
			this.framework.RunEvent(sender, eventId, eventObjects);
		} // Event
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
		/// <param name="defaultValue">The optional default value.</param>
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
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The resource data.</returns>
		public Image GetResourceImage(String key, Image defaultValue = null) {
			return this.framework.GetResourceImage(key, defaultValue);
		} // GetResourceImage
		#endregion

		#region Mail methods.
		/// <summary>
		/// Send e-mail message as plain text or html to the configured service desk recepient.
		/// </summary>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="textIsHtml">True if the message text is html code.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		public Boolean SendMail(String subject, String text, Boolean textIsHtml, params String[] attachments) {
			return this.framework.SendMail(subject, text, textIsHtml, attachments);
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
		public IDbConnection GetDatabaseConnection(String key) {
			return this.framework.GetDatabaseConnection(key);
		} // GetDatabaseConnection

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
			return this.framework.ExecuteSql(connection, sql, formatArgs);
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
			return this.framework.ExecuteSql(connection, schemaName, tableName, whereFilters);
		} // ExecuteSql

		/// <summary>
		/// Executes a insert query on the schema and table.
		/// The connection must be open.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="schemaName">The schema name.</param>
		/// <param name="tableName">The table name.</param>
		/// <param name="values">The field names (key) and values (value).</param>
		/// <returns>The first column of the first row in the result set.</returns>
		public Object ExecuteInsertSql(IDbConnection connection, String schemaName, String tableName, params KeyValuePair<String, Object>[] values) {
			return this.framework.ExecuteInsertSql(connection, schemaName, tableName, values);
		} // ExecuteInsertSql

		/// <summary>
		/// Executes a update query on the schema and table.
		/// The connection must be open.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="schemaName">The schema name.</param>
		/// <param name="tableName">The table name.</param>
		/// <param name="values">The field names (key) and values (value).</param>
		/// <param name="whereFilters">The WHERE filters.</param>
		/// <returns>The number of records affected.</returns>
		public Int32 ExecuteUpdateSql(IDbConnection connection, String schemaName, String tableName, IList<KeyValuePair<String, Object>> values, params SqlWhereFilterBase[] whereFilters) {
			return this.framework.ExecuteUpdateSql(connection, schemaName, tableName, values, whereFilters);
		} // ExecuteUpdateSql
		#endregion

		#region ActiveDirectory methods.
		/// <summary>
		/// Gets the current user.
		/// </summary>
		/// <returns>The current user.</returns>
		public AdUser GetCurrentUser() {
			return this.framework.GetCurrentUser();
		} // GetCurrentUser

		/// <summary>
		/// Gets the user identified by the user id.
		/// The user id can be person number (CPR), Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The user id to find.</param>
		/// <returns>The matching user or null.</returns>
		public AdUser GetUser(String userId) {
			return this.framework.GetUser(userId);
		} // GetUser

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <returns>All users.</returns>
		public List<AdUser> GetAllUsers(UserQuery userFilter = UserQuery.ALL) {
			return this.framework.GetAllUsers(userFilter);
		} // GetAllUsers

		/// <summary>
		/// Gets all users or filtered users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <param name="advancedUserFilterDays">Days added/substracted when using advanced user filters.</param>
		/// <returns>All users.</returns>
		public List<AdUser> GetAllUsers(UserQuery userFilter, Int32 advancedUserFilterDays = 0) {
			return this.framework.GetAllUsers(userFilter, advancedUserFilterDays);
		} // GetAllUsers

		/// <summary>
		/// Gets all users that are member of the group.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns></returns>
		public List<AdUser> GetAllUsers(AdGroup group, Boolean recursive = true) {
			return this.framework.GetAllUsers(group, recursive);
			;
		} // GetAllUsers

		/// <summary>
		/// Gets the group identified by the group id.
		/// The group id can be Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The group id to find.</param>
		/// <returns>The matching group or null.</returns>
		public AdGroup GetGroup(String groupId) {
			return this.framework.GetGroup(groupId);
		} // GetGroup

		/// <summary>
		/// Gets all groups.
		/// </summary>
		/// <returns>All groups.</returns>
		public List<AdGroup> GetAllGroups() {
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
		public Boolean IsUserMemberOfGroup(AdUser user, GroupPrincipal group, Boolean recursive = true) {
			return this.framework.IsUserMemberOfGroup(user, group, recursive);
		} // IsUserMemberOfGroup

		/// <summary>
		/// Gets if the user is member of one or all the groups.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <param name="all">True if the user must be member of all the groups.</param>
		/// <param name="groups">The groups.</param>
		/// <returns>True if the user is member of one or all the groups as specified.</returns>
		public Boolean IsUserMemberOfGroups(AdUser user, Boolean recursive, Boolean all, params GroupPrincipal[] groups) {
			return this.framework.IsUserMemberOfGroups(user, recursive, all, groups);
		} // IsUserMemberOfGroups
		#endregion

		#region SOFD methods.
		/// <summary>
		/// Gets the employee identified by the employee id.
		/// The employee id can be MaNummer, OpusBrugerNavn, AdBrugerNavn, CprNummer, Epost, Uuid.
		/// </summary>
		/// <param name="employeeId">The employee id to find.</param>
		/// <returns>The matching employee or null.</returns>
		public SofdEmployee GetEmployee(String employeeId) {
			return this.framework.GetEmployee(employeeId);
		} // GetEmployee

		/// <summary>
		/// Gets all employees or filtered employees.
		/// </summary>
		/// <param name="employeeFilters">Sql WHERE filters.</param>
		/// <returns>All matching employees.</returns>
		public List<SofdEmployee> GetAllEmployees(params SqlWhereFilterBase[] employeeFilters) {
			return this.framework.GetAllEmployees(employeeFilters);
		} // GetAllEmployees

		/// <summary>
		/// Gets the organization identified by the organization id.
		/// The organization id can be OrganisationId, CvrNummer, SeNummer, EanNummer, PNummer, Uuid.
		/// </summary>
		/// <param name="organizationId">The organization id to find.</param>
		/// <returns>The matching organization or null.</returns>
		public SofdOrganization GetOrganization(String organizationId) {
			return this.framework.GetOrganization(organizationId);
		} // GetOrganization

		/// <summary>
		/// Gets all organizations or filtered organizations.
		/// </summary>
		/// <param name="organizationFilters">Sql WHERE filters.</param>
		/// <returns>All matching organizations.</returns>
		public List<SofdOrganization> GetAllOrganizations(params SqlWhereFilterBase[] organizationFilters) {
			return this.framework.GetAllOrganizations(organizationFilters);
		} // GetAllOrganizations
		#endregion

		#region CPR methods.
		public CprSearchResult CprSearch(String cprNumber) {
			return this.framework.CprSearch(cprNumber);
		} // CprSearch
		#endregion

		#region Virtual methods.
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

	} // BaseForm
	#endregion

} // NDK.Framework