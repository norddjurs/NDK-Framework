using System;
using System.Collections.Generic;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;

namespace NDK.Framework {

	#region IOption interface.
	/// <summary>
	/// Option interface.
	/// System options are global for the system and not identified by a guid.
	/// User options are for each individual user on the system, and multiple user options can be stored and identified by a guid.
	/// </summary>
	public interface IOption {

		#region System option methods.
		/// <summary>
		/// Gets the system option keys associated with the empty guid.
		/// </summary>
		/// <returns>All the keys associated with the empty guid.</returns>
		String[] GetSystemKeys();

		/// <summary>
		/// Gets the system option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Boolean GetSystemValue(String key, Boolean defaultValue);

		/// <summary>
		/// Gets the system option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Int32 GetSystemValue(String key, Int32 defaultValue);

		/// <summary>
		/// Gets the system option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		DateTime GetSystemValue(String key, DateTime defaultValue);

		/// <summary>
		/// Gets the system option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Guid GetSystemValue(String key, Guid defaultValue);

		/// <summary>
		/// Gets the system option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		String GetSystemValue(String key, String defaultValue = null);

		/// <summary>
		/// Gets the system option values associated with the empty guid and key.
		/// If no value is associated with the empty guid and key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetSystemValues(String key);

		/// <summary>
		/// Sets the system option value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetSystemValue(String key, Boolean value);

		/// <summary>
		/// Sets the system option value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetSystemValue(String key, Int32 value);

		/// <summary>
		/// Sets the system option value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetSystemValue(String key, DateTime value);

		/// <summary>
		/// Sets the system option value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetSystemValue(String key, Guid value);

		/// <summary>
		/// Sets the system option values associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetSystemValues(String key, params String[] values);
		#endregion

		#region User option methods.
		/// <summary>
		/// Gets the user option guids.
		/// </summary>
		/// <returns>All the guids.</returns>
		Guid[] GetUserGuids();

		/// <summary>
		/// Gets the user option keys associated with the guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the guid.</returns>
		String[] GetUserKeys(Guid guid);

		/// <summary>
		/// Gets the user option value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Boolean GetUserValue(Guid guid, String key, Boolean defaultValue);

		/// <summary>
		/// Gets the user option value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Int32 GetUserValue(Guid guid, String key, Int32 defaultValue);

		/// <summary>
		/// Gets the user option value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		DateTime GetUserValue(Guid guid, String key, DateTime defaultValue);

		/// <summary>
		/// Gets the user option value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Guid GetUserValue(Guid guid, String key, Guid defaultValue);

		/// <summary>
		/// Gets the user option value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		String GetUserValue(Guid guid, String key, String defaultValue = null);

		/// <summary>
		/// Gets the user option values associated with the guid and key.
		/// If no value is associated with the key, an empty list is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetUserValues(Guid guid, String key);

		/// <summary>
		/// Sets the user option value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetUserValue(Guid guid, String key, Boolean value);

		/// <summary>
		/// Sets the user option value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetUserValue(Guid guid, String key, Int32 value);

		/// <summary>
		/// Sets the user option value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetUserValue(Guid guid, String key, DateTime value);

		/// <summary>
		/// Sets the user option value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetUserValue(Guid guid, String key, Guid value);

		/// <summary>
		/// Sets the user option values associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetUserValues(Guid guid, String key, params String[] values);
		#endregion

	} // IOption
	#endregion

	#region Option class.
	/// <summary>
	/// Default option class, that stores the data in the Windows Registry.
	/// 
	/// User options are identified by a guid.
	/// </summary>
	public class Option : IOption {
		private IConfiguration config = null;
		private ILogger logger = null;
		private String defaultRegistryUserKey = null;

		#region Constructor methods.
		public Option(IConfiguration config, ILogger logger) {
			this.config = config;
			this.logger = logger;
			this.defaultRegistryUserKey = this.config.GetSystemValue("RegistryUserKey", "\\Software\\NDK Framework").TrimEnd('\\');
		} // Option
		#endregion

		#region System option methods.
		/// <summary>
		/// Gets the system option keys associated with the empty guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the empty guid.</returns>
		public String[] GetSystemKeys() {
			try {
				// Log.
				this.logger.LogDebug("Option: Read system keys");

				// Get data.
				return this.GetNames(RegistryHive.LocalMachine, this.defaultRegistryUserKey, RegistryView.Registry64, true);
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return new String[0];
			}
		}// GetSystemKeys

		/// <summary>
		/// Gets the system option value associated with the empty guid and key.
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
			try {
				// Log.
				this.logger.LogDebug("Option: Read system boolean value for key '{0}'.", key);

				// Get data.
				Object value = this.GetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, RegistryView.Registry64, defaultValue, true);
				if (value is Boolean) {
					return (Boolean)value;
				} else if (value is Int32) {
					return ((Int32)value) == 1;
				} else {
					return Boolean.Parse(value.ToString());
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return defaultValue;
			}
		} // GetSystemValue

		/// <summary>
		/// Gets the system option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Int32 GetSystemValue(String key, Int32 defaultValue) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read system integer value for key '{0}'.", key);

				// Get data.
				Object value = this.GetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, RegistryView.Registry64, defaultValue, true);
				if (value is Int32) {
					return (Int32)value;
				} else {
					return Int32.Parse(value.ToString());
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return defaultValue;
			}
		} // GetSystemValue

		/// <summary>
		/// Gets the system option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public DateTime GetSystemValue(String key, DateTime defaultValue) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read system datetime value for key '{0}'.", key);

				// Get data.
				Object value = this.GetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, RegistryView.Registry64, defaultValue, true);
				if (value is DateTime) {
					return (DateTime)value;
				} else {
					return DateTime.Parse(value.ToString());
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return defaultValue;
			}
		} // GetSystemValue

		/// <summary>
		/// Gets the system option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Guid GetSystemValue(String key, Guid defaultValue) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read system guid value for key '{0}'.", key);

				// Get data.
				Object value = this.GetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, RegistryView.Registry64, defaultValue, true);
				if (value is Guid) {
					return (Guid)value;
				} else {
					return Guid.Parse(value.ToString());
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return defaultValue;
			}
		} // GetSystemValue

		/// <summary>
		/// Gets the system option value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public String GetSystemValue(String key, String defaultValue = null) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read system string value for key '{0}'.", key);

				// Get data.
				Object value = this.GetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, RegistryView.Registry64, defaultValue, true);
				if (value is String) {
					return (String)value;
				} else {
					return value.ToString();
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return defaultValue;
			}
		} // GetSystemValue

		/// <summary>
		/// Gets the system option values associated with the empty guid and key.
		/// If no value is associated with the empty guid and key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetSystemValues(String key) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read system string values for key '{0}'.", key);

				// Get data.
				List<String> result = new List<String>();
				Object value = this.GetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, RegistryView.Registry64, new String[0], true);
				if (value is String[]) {
					result.AddRange((String[])value);
				} else if (value is Array) {
					foreach (Object valueObj in (Array)value) {
						result.Add(valueObj.ToString());
					}
				} else {
					result.Add(value.ToString());
				}
				return result;
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return new List<String>();
			}
		} // GetSystemValues

		/// <summary>
		/// Sets the system option value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, Boolean value) {
			try {
				// Log.
				this.logger.LogDebug("Option: Write system boolean value for key '{0}'.", key);

				// Set data.
				this.SetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, value, RegistryValueKind.DWord, RegistryView.Registry64, true);
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);
			}
		} // SetSystemValue

		/// <summary>
		/// Sets the system option value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, Int32 value) {
			try {
				// Log.
				this.logger.LogDebug("Option: Write system integer value for key '{0}'.", key);

				// Set data.
				if (value >= 0) {
					this.SetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, value, RegistryValueKind.DWord, RegistryView.Registry64, true);
				} else {
					this.SetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, value, RegistryValueKind.String, RegistryView.Registry64, true);
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);
			}
		} // SetSystemValue

		/// <summary>
		/// Sets the system option value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, DateTime value) {
			try {
				// Log.
				this.logger.LogDebug("Option: Write system datetime value for key '{0}'.", key);

				// Set data.
				this.SetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, value.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), RegistryValueKind.String, RegistryView.Registry64, true);
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);
			}
		} // SetSystemValue

		/// <summary>
		/// Sets the system option value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, Guid value) {
			try {
				// Log.
				this.logger.LogDebug("Option: Write system guid value for key '{0}'.", key);

				// Set data.
				this.SetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, value, RegistryValueKind.String, RegistryView.Registry64, true);
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);
			}
		} // SetSystemValue

		/// <summary>
		/// Sets the system option values associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetSystemValues(String key, params String[] values) {
			try {
				// Log.
				this.logger.LogDebug("Option: Write system string values for key '{0}'.", key);

				// Set data.
				this.SetValue(RegistryHive.LocalMachine, this.defaultRegistryUserKey, key, values, RegistryValueKind.MultiString, RegistryView.Registry64, true);
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);
			}
		} // SetSystemValues
		#endregion

		#region User option methods.
		/// <summary>
		/// Gets the user option guids.
		/// </summary>
		/// <returns>All the guids.</returns>
		public Guid[] GetUserGuids() {
			try {
				// Log.
				this.logger.LogDebug("Option: Read user guids");

				// Get data.
				List<Guid> result = new List<Guid>();
				foreach (String key in this.GetKeys(RegistryHive.CurrentUser, this.defaultRegistryUserKey, RegistryView.Registry64, true)) {
					try {
						result.Add(Guid.Parse(key));
					} catch { }
				}

				// Return the result.
				return result.ToArray();
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return new Guid[0];
			}
		} // GetUserGuids

		/// <summary>
		/// Gets the user option keys associated with the guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the guid.</returns>
		public String[] GetUserKeys(Guid guid) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read user keys in guid '{0}'.", guid);

				// Get data.
				return this.GetNames(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), RegistryView.Registry64, true);
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return new String[0];
			}
		} // GetUserKeys

		/// <summary>
		/// Gets the user option value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// True values are "true", "yes" and "1" in any case.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Boolean GetUserValue(Guid guid, String key, Boolean defaultValue) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read user boolean value for key '{1}' in guid '{0}'.", guid, key);

				// Get data.
				Object value = this.GetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, RegistryView.Registry64, defaultValue, true);
				if (value is Boolean) {
					return (Boolean)value;
				} else if (value is Int32) {
					return ((Int32)value) == 1;
				} else {
					return Boolean.Parse(value.ToString());
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return defaultValue;
			}
		} // GetUserValue

		/// <summary>
		/// Gets the user option value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Int32 GetUserValue(Guid guid, String key, Int32 defaultValue) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read user integer value for key '{1}' in guid '{0}'.", guid, key);

				// Get data.
				Object value = this.GetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, RegistryView.Registry64, defaultValue, true);
				if (value is Int32) {
					return (Int32)value;
				} else {
					return Int32.Parse(value.ToString());
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return defaultValue;
			}
		} // GetUserValue

		/// <summary>
		/// Gets the user option value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public DateTime GetUserValue(Guid guid, String key, DateTime defaultValue) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read user datetime value for key '{1}' in guid '{0}'.", guid, key);

				// Get data.
				Object value = this.GetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, RegistryView.Registry64, defaultValue, true);
				if (value is DateTime) {
					return (DateTime)value;
				} else {
					return DateTime.Parse(value.ToString());
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return defaultValue;
			}
		} // GetUserValue

		/// <summary>
		/// Gets the user option value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public Guid GetUserValue(Guid guid, String key, Guid defaultValue) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read user guid value for key '{1}' in guid '{0}'.", guid, key);

				// Get data.
				Object value = this.GetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, RegistryView.Registry64, defaultValue, true);
				if (value is Guid) {
					return (Guid)value;
				} else {
					return Guid.Parse(value.ToString());
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return defaultValue;
			}
		} // GetUserValue

		/// <summary>
		/// Gets the user option value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		public String GetUserValue(Guid guid, String key, String defaultValue = null) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read user string value for key '{1}' in guid '{0}'.", guid, key);

				// Get data.
				Object value = this.GetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, RegistryView.Registry64, defaultValue, true);
				if (value is String) {
					return (String)value;
				} else if (value is String[]) {
					return ((String[])value)[0];
				} else {
					return value.ToString();
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return defaultValue;
			}
		} // GetUserValue

		/// <summary>
		/// Gets the user option values associated with the guid and key.
		/// If no value is associated with the guid and key, an empty list is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetUserValues(Guid guid, String key) {
			try {
				// Log.
				this.logger.LogDebug("Option: Read user string values for key '{1}' in guid '{0}'.", guid, key);

				// Get data.
				List<String> result = new List<String>();
				Object value = this.GetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, RegistryView.Registry64, new String[0], true);
				if (value is String[]) {
					result.AddRange((String[])value);
				} else if (value is Array) {
					foreach (Object valueObj in (Array)value) {
						result.Add(valueObj.ToString());
					}
				} else {
					result.Add(value.ToString());
				}
				return result;
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);

				// Return default value.
				return new List<String>();
			}
		} // GetUserValues

		/// <summary>
		/// Sets the user option value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetUserValue(Guid guid, String key, Boolean value) {
			try {
				// Log.
				this.logger.LogDebug("Option: Write user boolean value for key '{1}' in guid '{0}'.", guid, key);

				// Set data.
				this.SetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, value, RegistryValueKind.DWord, RegistryView.Registry64, true);
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);
			}
		} // SetUserValue

		/// <summary>
		/// Sets the user option value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetUserValue(Guid guid, String key, Int32 value) {
			try {
				// Log.
				this.logger.LogDebug("Option: Write user integer value for key '{1}' in guid '{0}'.", guid, key);

				// Set data.
				if (value >= 0) {
					this.SetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, value, RegistryValueKind.DWord, RegistryView.Registry64, true);
				} else {
					this.SetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, value, RegistryValueKind.String, RegistryView.Registry64, true);
				}
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);
			}
		} // SetUserValue

		/// <summary>
		/// Sets the user option value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetUserValue(Guid guid, String key, DateTime value) {
			try {
				// Log.
				this.logger.LogDebug("Option: Write user datetime value for key '{1}' in guid '{0}'.", guid, key);

				// Set data.
				this.SetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, value.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), RegistryValueKind.String, RegistryView.Registry64, true);
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);
			}
		} // SetUserValue

		/// <summary>
		/// Sets the user option value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetUserValue(Guid guid, String key, Guid value) {
			try {
				// Log.
				this.logger.LogDebug("Option: Write user guid value for key '{1}' in guid '{0}'.", guid, key);

				// Set data.
				this.SetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, value, RegistryValueKind.String, RegistryView.Registry64, true);
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);
			}
		} // SetUserValue

		/// <summary>
		/// Sets the user option values associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetUserValues(Guid guid, String key, params String[] values) {
			try {
				// Log.
				this.logger.LogDebug("Option: Write user string values for key '{1}' in guid '{0}'.", guid, key);

				// Set data.
				this.SetValue(RegistryHive.CurrentUser, this.defaultRegistryUserKey + "\\" + guid.ToString(), key, values, RegistryValueKind.MultiString, RegistryView.Registry64, true);
			} catch (Exception exception) {
				// Log exception.
				this.logger.LogError(exception);
			}
		} // SetUserValues
		#endregion

		#region Registry methods.
		/// <summary>
		/// Deletes the specified key and all sub-keys in the registry, using the specified registry view.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the key to delete.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="throwExceptions">True if exceptions should be thrown, false if exceptions should be caught.</param>
		private void Delete(RegistryHive hive, String key, RegistryView view, Boolean throwExceptions) {
			try {
				// Validate the key.
				key = this.KeyValidate(key);

				// Get the last sub key from the specified key - this is the key that should be deleted.
				String delKey = null;
				List<String> keys = new List<String>(key.Split('\\'));
				if (keys.Count > 0) {
					// Delete a sub key.
					delKey = keys[keys.Count - 1];
					keys.RemoveAt(keys.Count - 1);
					key = String.Join("\\", keys.ToArray());
				} else {
					// Delete a key in the root of the hive.
					delKey = key;
					key = String.Empty;
				}

				// Try to open the registry key and delete the key.
				try {
					using (RegistryKey regKey = this.OpenRegistryKey(hive, key, view, false)) {
						// Delete the key in the registry.
						if (regKey != null) {
							regKey.DeleteSubKeyTree(delKey, false);
						}
					}
				} catch (SecurityException exception) {
					// Try to take full control and ownership of the key. Then retry to delete the key.
					using (RegistryKey regKey = OpenRegistryKey(hive, key, view, false, true, true)) {
						// Delete the key in the registry.
						if (regKey != null) {
							regKey.DeleteSubKeyTree(delKey, false);
						}
					}
				} catch (UnauthorizedAccessException exception) {
					// Try to take full control and ownership of the key. Then retry to delete the key.
					using (RegistryKey regKey = OpenRegistryKey(hive, key, view, false, true, true)) {
						// Delete the key in the registry.
						if (regKey != null) {
							regKey.DeleteSubKeyTree(delKey, false);
						}
					}
				}
			} catch {
				if (throwExceptions == true) {
					throw;
				}
			}
		} // Delete

		/// <summary>
		/// Deletes the specified name/value pair in the registry key, using the specified registry view.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the subkey to open.</param>
		/// <param name="name">The name of the value to delete.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="throwExceptions">True if exceptions should be thrown, false if exceptions should be caught.</param>
		private void Delete(RegistryHive hive, String key, String name, RegistryView view, Boolean throwExceptions) {
			try {
				// Validate the key.
				key = this.KeyValidate(key);

				// Try to open the registry key and delete the name and value.
				try {
					using (RegistryKey regKey = OpenRegistryKey(hive, key, view, false)) {
						// Delete the name and value in the registry.
						if (regKey != null) {
							regKey.DeleteValue(name, false);
						}
					}
				} catch (SecurityException exception) {
					// Try to take full control and ownership of the key. Then retry to delete the name and value.
					using (RegistryKey regKey = OpenRegistryKey(hive, key, view, false, true, true)) {
						// Delete the name and value in the registry.
						if (regKey != null) {
							regKey.DeleteValue(name, false);
						}
					}
				} catch (UnauthorizedAccessException exception) {
					// Try to take full control and ownership of the key. Then retry to delete the name and value.
					using (RegistryKey regKey = OpenRegistryKey(hive, key, view, false, true, true)) {
						// Delete the name and value in the registry.
						if (regKey != null) {
							regKey.DeleteValue(name, false);
						}
					}
				}
			} catch {
				if (throwExceptions == true) {
					throw;
				}
			}
		} // Delete

		/// <summary>
		/// Retrieves an array of strings that contains all the sub key names associated with the specified key, using the specified registry view.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the key to open.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="throwExceptions">True if exceptions should be thrown, false if exceptions should be caught.</param>
		/// <returns>An array of strings that contains the sub key names.</returns>
		private String[] GetKeys(RegistryHive hive, String key, RegistryView view, Boolean throwExceptions) {
			try {
				String[] result = new String[0];

				// Validate the key.
				key = this.KeyValidate(key);

				// Try to open the registry key and read the subkey names.
				using (RegistryKey regKey = OpenRegistryKey(hive, key, view, false)) {
					// Read the subkey names.
					if (regKey != null) {
						result = regKey.GetSubKeyNames();
					}
				}

				// Return the result.
				return result;
			} catch {
				if (throwExceptions == true) {
					throw;
				} else {
					// Return empty result.
					return new String[0];
				}
			}
		} // GetKeys

		/// <summary>
		/// Retrieves an array of strings that contains all the value names associated with the specified key, using the specified registry view.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the key to open.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="throwExceptions">True if exceptions should be thrown, false if exceptions should be caught.</param>
		/// <returns>An array of strings that contains the value names.</returns>
		private String[] GetNames(RegistryHive hive, String key, RegistryView view, Boolean throwExceptions) {
			try {
				String[] result = new String[0];

				// Validate the key.
				key = this.KeyValidate(key);

				// Try to open the registry key and read the value names.
				using (RegistryKey regKey = OpenRegistryKey(hive, key, view, false)) {
					// Read the value names.
					if (regKey != null) {
						result = regKey.GetValueNames();
					}
				}

				// Return the result.
				return result;
			} catch {
				if (throwExceptions == true) {
					throw;
				} else {
					// Return empty result.
					return new String[0];
				}
			}
		} // GetNames

		/// <summary>
		/// Returns true if the key exists, using the specified registry view.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the key to open.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="throwExceptions">True if exceptions should be thrown, false if exceptions should be caught.</param>
		/// <returns>True if the key exists.</returns>
		private Boolean Exists(RegistryHive hive, String key, RegistryView view, Boolean throwExceptions) {
			try {
				Boolean result = false;

				// Validate the key.
				key = this.KeyValidate(key);

				// Try to open the registry key.
				using (RegistryKey regKey = OpenRegistryKey(hive, key, view, false)) {
					// Get the result.
					result = (regKey != null);
				}

				// Return the result.
				return result;
			} catch {
				if (throwExceptions == true) {
					throw;
				} else {
					// Return false.
					return false;
				}
			}
		} // Exists

		/// <summary>
		/// Returns true if the value name exists in the registry key, using the specified registry view.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the key to open.</param>
		/// <param name="name">The value name to open.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="throwExceptions">True if exceptions should be thrown, false if exceptions should be caught.</param>
		/// <returns>True if the key exists.</returns>
		private Boolean Exists(RegistryHive hive, String key, String name, RegistryView view, Boolean throwExceptions) {
			try {
				Boolean result = false;

				// Validate the key.
				key = this.KeyValidate(key);
				name = name.ToLower();

				// Try to open the registry key.
				using (RegistryKey regKey = OpenRegistryKey(hive, key, view, false)) {
					// Get the result.
					if (regKey != null) {
						foreach (String regName in regKey.GetValueNames()) {
							if (regName.ToLower().Equals(name) == true) {
								result = true;
							}
						}
					}
				}

				// Return the result.
				return result;
			} catch {
				if (throwExceptions == true) {
					throw;
				} else {
					// Return false.
					return false;
				}
			}
		} // Exists

		/// <summary>
		/// Sets the value of a name/value pair in the registry key, using the specified registry data type and registry view.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the subkey to create or open.</param>
		/// <param name="name">The name of the value to be stored.</param>
		/// <param name="value">The data to be stored.</param>
		/// <param name="valueKind">The registry data type to use when storing the data.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="throwExceptions">True if exceptions should be thrown, false if exceptions should be caught.</param>
		private void SetValue(RegistryHive hive, String key, String name, Object value, RegistryValueKind valueKind, RegistryView view, Boolean throwExceptions) {
			try {
				// Validate the key.
				key = this.KeyValidate(key);

				// Try to open the registry key and write the value.
				try {
					using (RegistryKey regKey = OpenRegistryKey(hive, key, view, true)) {
						// Write the value to the registry.
						regKey.SetValue(name, value, valueKind);
					}
				} catch (SecurityException exception) {
					// Try to take full control and ownership of the key. Then retry to write the value.
					using (RegistryKey regKey = OpenRegistryKey(hive, key, view, true, true, true)) {
						// Write the value to the registry.
						regKey.SetValue(name, value, valueKind);
					}
				} catch (UnauthorizedAccessException exception) {
					// Try to take full control and ownership of the key. Then retry to write the value.
					using (RegistryKey regKey = OpenRegistryKey(hive, key, view, true, true, true)) {
						// Write the value to the registry.
						regKey.SetValue(name, value, valueKind);
					}
				}
			} catch {
				if (throwExceptions == true) {
					throw;
				}
			}
		} // SetValue

		/// <summary>
		/// Retrieves the value associated with the specified name in the specified registry key, using the specified registry view.
		/// Environment names are not expanded.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the key to open.</param>
		/// <param name="name">The value name to open.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="defaultValue">The default value returned when the name don't exist or on uncaught exceptions.</param>
		/// <param name="throwExceptions">True if exceptions should be thrown, false if exceptions should be caught.</param>
		/// <returns>The value associated with name, or defaultValue if name is not found.</returns>
		private Object GetValue(RegistryHive hive, String key, String name, RegistryView view, Object defaultValue, Boolean throwExceptions) {
			try {
				Object result = defaultValue;

				// Validate the key.
				key = KeyValidate(key);

				// Try to open the registry key and read the value.
				using (RegistryKey regKey = OpenRegistryKey(hive, key, view, false)) {
					// Read the value.
					if (regKey != null) {
						result = regKey.GetValue(name, defaultValue, RegistryValueOptions.DoNotExpandEnvironmentNames);
					}
				}

				// Return the result.
				return result;
			} catch {
				if (throwExceptions == true) {
					throw;
				} else {
					// Return the default value.
					return defaultValue;
				}
			}
		} // GetValue

		/// <summary>
		/// Gets the validated and formatted registry key.
		/// If the user can enter registry keys, use this method to re-format the entered key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The formatted key.</returns>
		private String KeyValidate(String key) {
			// Trim beginning and ending backslashes.
			key = key.Trim().Trim('\\');

			// Return the result.
			return key;
		} // KeyValidate

		/// <summary>
		/// Creates a new subkey or opens an existing subkey.
		/// The key is opened in the Registry64 view, see <see cref="RegistryView"/>.
		/// The key is created if it don't exist.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the subkey to create or open.</param>
		/// <returns>The subkey requested, or null if the operation failed.</returns>
		private RegistryKey OpenRegistryKey(RegistryHive hive, String key) {
			return this.OpenRegistryKey(hive, key, RegistryView.Registry64, true);
		} // OpenRegistryKey

		/// <summary>
		/// Creates a new subkey or opens an existing subkey.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the subkey to create or open.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="createMissingKey">Set to true if you want to create the key when it don't exist.</param>
		/// <returns>The subkey requested, or null if the operation failed.</returns>
		private RegistryKey OpenRegistryKey(RegistryHive hive, String key, RegistryView view, Boolean createMissingKey) {
			// Open the base registry key.
			RegistryKey regKey = null;
			try {
				regKey = RegistryKey.OpenBaseKey(hive, view);
			} catch {
				regKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
			}

			if (key != null) {
				// Validate and open the key.
				key = KeyValidate(key);

				if (createMissingKey == true) {
					// Open the existing key or create the key.
					// This creates missing sub keys.
					regKey = regKey.CreateSubKey(key);
				} else {
					// Try to open the existing key.
					// This returns null if the key don't exist.
					regKey = regKey.OpenSubKey(key);
				}
			}

			// Return the result.
			return regKey;
		} // OpenRegistryKey

		/// <summary>
		/// Creates a new subkey or opens an existing subkey.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the subkey to create or open.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="createMissingKey">Set to true if you want to create the key when it don't exist.</param>
		/// <param name="takeFullControl">Try to replace all not inherited security rules, with one rule that grants the current user.</param>
		/// <param name="takeOwnership">Try to set the current user as owner.</param>
		/// <returns>The subkey requested, or null if the operation failed.</returns>
		private RegistryKey OpenRegistryKey(RegistryHive hive, String key, RegistryView view, Boolean createMissingKey, Boolean takeFullControl, Boolean takeOwnership) {
			// Try to replace all not inherited security rules, with one rule that grants the current user full control.
			if (takeFullControl == true) {
				try {
					IEnumerator<RegistryKey> regKeyEnum = this.GetRegistryKeys(hive, key, view, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ChangePermissions | RegistryRights.ReadKey);
					while (regKeyEnum.MoveNext()) {
						using (RegistryKey regKey = regKeyEnum.Current) {
							RegistrySecurity regSecurity = new RegistrySecurity();
							regSecurity.AddAccessRule(new RegistryAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, RegistryRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
							regKey.SetAccessControl(regSecurity);
						}
					}
				} catch { }
			}

			// Try to set the current user as owner.
			if (takeOwnership == true) {
				try {
					IEnumerator<RegistryKey> regKeyEnum = this.GetRegistryKeys(hive, key, view, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
					while (regKeyEnum.MoveNext()) {
						using (RegistryKey regKey = regKeyEnum.Current) {
							RegistrySecurity regSecurity = regKey.GetAccessControl();
							regSecurity.SetOwner(new NTAccount(Environment.UserDomainName + "\\" + Environment.UserName));
							regKey.SetAccessControl(regSecurity);
						}
					}
				} catch { }
			}

			// Open the base registry key.
			RegistryKey regBaseKey = null;
			try {
				regBaseKey = RegistryKey.OpenBaseKey(hive, view);
			} catch {
				regBaseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
			}

			// Validate and open the registry key.
			// Return the result.
			key = KeyValidate(key);
			return regBaseKey.CreateSubKey(key);
		} // OpenRegistryKey

		/// <summary>
		/// Creates a enumerator iterating through the arguet key and all subkeys.
		/// </summary>
		/// <param name="hive">The HKEY to open.</param>
		/// <param name="key">The name or path of the top level key to create or open.</param>
		/// <param name="view">The registry view to use.</param>
		/// <param name="permissionCheck">Check that the user has the required permissions on the keys.</param>
		/// <param name="rights">Check that the user has the required rights on the keys.</param>
		/// <returns>The subkey requested, or null if the operation failed.</returns>
		private IEnumerator<RegistryKey> GetRegistryKeys(RegistryHive hive, String key, RegistryView view, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights) {
			// Open the base registry key.
			RegistryKey regBaseKey = null;
			try {
				regBaseKey = RegistryKey.OpenBaseKey(hive, view);
			} catch {
				regBaseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
			}

			// Validate and open the registry key.
			key = KeyValidate(key);
			RegistryKey regKey = regBaseKey.OpenSubKey(key, permissionCheck, rights);

			// Get the name of the opened key, and the names of the sub keys.
			// This is done here, in case the registry key is closed by the consumer during the yielding.
			String regKeyName = regKey.Name.Substring(regKey.Name.IndexOf('\\') + 1);       // Exclude the base key (hive).
			Stack<String> regKeyNames = new Stack<String>();
			foreach (String regSubKeyName in regKey.GetSubKeyNames()) {
				regKeyNames.Push(regKeyName + "\\" + regSubKeyName);
			}

			// Yield the opened registry key.
			yield return regKey;

			// Recursive iteration.
			while (regKeyNames.Count > 0) {
				// Open the registry key.
				regKey = regBaseKey.OpenSubKey(regKeyNames.Pop(), permissionCheck, rights);

				// Get the name of the opened key, and the names of the sub keys.
				// This is done here, in case the registry key is closed by the consumer during the yielding.
				regKeyName = regKey.Name.Substring(regKey.Name.IndexOf('\\') + 1);      // Exclude the base key (hive).
				foreach (String regSubKeyName in regKey.GetSubKeyNames()) {
					regKeyNames.Push(regKeyName + "\\" + regSubKeyName);
				}

				// Yield the opened registry key.
				yield return regKey;
			}
		} // GetRegistryKeys
		#endregion

	} // Option
	#endregion

} // NDK.Framework