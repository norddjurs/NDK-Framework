using System;
using System.Collections.Generic;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines configuration.
	/// </summary>
	public partial interface IFramework {

		#region System configuration methods.
		/// <summary>
		/// Gets the system configuration keys associated with the empty guid.
		/// </summary>
		/// <returns>All the keys associated with the empty guid.</returns>
		String[] GetSystemKeys();

		/// <summary>
		/// Gets the system configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		Boolean GetSystemValue(String key, Boolean defaultValue);

		/// <summary>
		/// Gets the system configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		Int32 GetSystemValue(String key, Int32 defaultValue);

		/// <summary>
		/// Gets the system configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		DateTime GetSystemValue(String key, DateTime defaultValue);

		/// <summary>
		/// Gets the system configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		Guid GetSystemValue(String key, Guid defaultValue);

		/// <summary>
		/// Gets the system configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		String GetSystemValue(String key, String defaultValue = null);

		/// <summary>
		/// Gets the system configuration values associated with the empty guid and key.
		/// If no value is associated with the empty guid and key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetSystemValues(String key);

		/// <summary>
		/// Sets the system configuration value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetSystemValue(String key, Boolean value);

		/// <summary>
		/// Sets the system configuration value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetSystemValue(String key, Int32 value);

		/// <summary>
		/// Sets the system configuration value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetSystemValue(String key, DateTime value);

		/// <summary>
		/// Sets the system configuration value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetSystemValue(String key, Guid value);

		/// <summary>
		/// Sets the system configuration values associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetSystemValues(String key, params String[] values);
		#endregion

		#region Local configuration methods.
		/// <summary>
		/// Gets the local configuration keys associated with the framework class guid.
		/// </summary>
		/// <returns>All the keys associated with the framework class guid.</returns>
		String[] GetLocalKeys();

		/// <summary>
		/// Gets the local configuration value associated with the framework class guid and key.
		/// If more then one value is associated with the framework class guid and key, the first value is returned.
		/// If no value is associated with the framework class guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		Boolean GetLocalValue(String key, Boolean defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the framework class guid and key.
		/// If more then one value is associated with the framework class guid and key, the first value is returned.
		/// If no value is associated with the framework class guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		Int32 GetLocalValue(String key, Int32 defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the framework class guid and key.
		/// If more then one value is associated with the framework class guid and key, the first value is returned.
		/// If no value is associated with the framework class guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		DateTime GetLocalValue(String key, DateTime defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the framework class guid and key.
		/// If more then one value is associated with the framework class guid and key, the first value is returned.
		/// If no value is associated with the framework class guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		Guid GetLocalValue(String key, Guid defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the framework class guid and key.
		/// If more then one value is associated with the framework class guid and key, the first value is returned.
		/// If no value is associated with the framework class guid and key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		String GetLocalValue(String key, String defaultValue = null);

		/// <summary>
		/// Gets the local configuration values associated with the framework class guid and key.
		/// If no value is associated with the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetLocalValues(String key);

		/// <summary>
		/// Sets the local configuration value associated with the framework class guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetLocalValue(String key, Boolean value);

		/// <summary>
		/// Sets the local configuration value associated with the framework class guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetLocalValue(String key, Int32 value);

		/// <summary>
		/// Sets the local configuration value associated with the framework class guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetLocalValue(String key, DateTime value);

		/// <summary>
		/// Sets the local configuration value associated with the framework class guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetLocalValue(String key, Guid value);

		/// <summary>
		/// Sets the local configuration values associated with the framework class guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetLocalValues(String key, params String[] values);
		#endregion

		#region Public configuration methods.
		/// <summary>
		/// Gets the configuration guids.
		/// </summary>
		/// <returns>All the guids.</returns>
		Guid[] GetConfigGuids();

		/// <summary>
		/// Gets the configuration keys associated with the guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the guid.</returns>
		String[] GetConfigKeys(Guid guid);

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
		Boolean GetConfigValue(Guid guid, String key, Boolean defaultValue);

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
		Int32 GetConfigValue(Guid guid, String key, Int32 defaultValue);

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
		DateTime GetConfigValue(Guid guid, String key, DateTime defaultValue);

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
		Guid GetConfigValue(Guid guid, String key, Guid defaultValue);

		/// <summary>
		/// Gets the configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		String GetConfigValue(Guid guid, String key, String defaultValue = null);

		/// <summary>
		/// Gets the configuration values associated with the guid and key.
		/// If no value is associated with the guid and key, an empty list is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetConfigValues(Guid guid, String key);

		/// <summary>
		/// Sets the configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetConfigValue(Guid guid, String key, Boolean value);

		/// <summary>
		/// Sets the configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetConfigValue(Guid guid, String key, Int32 value);

		/// <summary>
		/// Sets the local configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetConfigValue(Guid guid, String key, DateTime value);

		/// <summary>
		/// Sets the configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetConfigValue(Guid guid, String key, Guid value);

		/// <summary>
		/// Sets the configuration values associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetConfigValues(Guid guid, String key, params String[] values);
		#endregion


	} // IFramework
	#endregion

} // NDK.Framework