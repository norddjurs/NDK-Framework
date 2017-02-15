using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace NDK.Framework {

	#region IConfiguration interface.
	/// <summary>
	/// Configuration interface.
	/// System configurations are global for the framework installation and not identified by a guid.
	/// Multiple local configurations can be stored and identified by a guid.
	/// </summary>
	public interface IConfiguration {

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
		/// Gets the local configuration guids.
		/// </summary>
		/// <returns>All the guids.</returns>
		Guid[] GetLocalGuids();

		/// <summary>
		/// Gets the local configuration keys associated with the guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the guid.</returns>
		String[] GetLocalKeys(Guid guid);

		/// <summary>
		/// Gets the local configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		Boolean GetLocalValue(Guid guid, String key, Boolean defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		Int32 GetLocalValue(Guid guid, String key, Int32 defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		DateTime GetLocalValue(Guid guid, String key, DateTime defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		Guid GetLocalValue(Guid guid, String key, Guid defaultValue);

		/// <summary>
		/// Gets the local configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		String GetLocalValue(Guid guid, String key, String defaultValue = null);

		/// <summary>
		/// Gets the local configuration values associated with the guid and key.
		/// If no value is associated with the key, an empty list is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetLocalValues(Guid guid, String key);

		/// <summary>
		/// Sets the local configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetLocalValue(Guid guid, String key, Boolean value);

		/// <summary>
		/// Sets the local configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetLocalValue(Guid guid, String key, Int32 value);

		/// <summary>
		/// Sets the local configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetLocalValue(Guid guid, String key, DateTime value);

		/// <summary>
		/// Sets the local configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetLocalValue(Guid guid, String key, Guid value);

		/// <summary>
		/// Sets the local configuration values associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetLocalValues(Guid guid, String key, params String[] values);
		#endregion

	} // IConfiguration
	#endregion

	#region Configuration class.
	/// <summary>
	/// Default configuration class, that stores the data in a XML file.
	/// The configuration file name is the same as the framework assembly (.dll), but with the ".xml" file extension.
	/// 
	/// System configurations are identified by a Guid.Empty.
	/// Local configurations are identified by an other guid.
	/// </summary>
	public class Configuration : IConfiguration {
		private String configFileName = null;

		#region Constructor methods.
		public Configuration() {
			this.configFileName = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "xml");
		} // Configuration
		#endregion

		#region System configuration methods.
		/// <summary>
		/// Gets the system configuration keys associated with the empty guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the empty guid.</returns>
		public String[] GetSystemKeys() {
			return this.GetLocalKeys(Guid.Empty);
		}// GetSystemKeys

		/// <summary>
		/// Gets the system configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// True values are "true", "yes" and "1" in any case.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public Boolean GetSystemValue(String key, Boolean defaultValue) {
			try {
				String value = this.GetSystemValue(key);
				return ((value.ToLower() == "true") || (value.ToLower() == "yes") || (value.ToLower() == "1"));
			} catch {
				// Return the default value.
				return defaultValue;
			}
		} // GetSystemValue

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
		public Int32 GetSystemValue(String key, Int32 defaultValue) {
			try {
				String value = this.GetSystemValue(key);
				return Int32.Parse(value);
			} catch {
				// Return the default value.
				return defaultValue;
			}
		} // GetSystemValue

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
		public DateTime GetSystemValue(String key, DateTime defaultValue) {
			try {
				String value = this.GetSystemValue(key);
				return DateTime.Parse(value);
			} catch {
				// Return the default value.
				return defaultValue;
			}
		} // GetSystemValue

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
		public Guid GetSystemValue(String key, Guid defaultValue) {
			try {
				String value = this.GetSystemValue(key);
				return Guid.Parse(value);
			} catch {
				// Return the default value.
				return defaultValue;
			}
		} // GetSystemValue

		/// <summary>
		/// Gets the system configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public String GetSystemValue(String key, String defaultValue = null) {
			return this.GetLocalValue(Guid.Empty, key, defaultValue);
		} // GetSystemValue

		/// <summary>
		/// Gets the system configuration values associated with the empty guid and key.
		/// If no value is associated with the empty guid and key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetSystemValues(String key) {
			return this.GetLocalValues(Guid.Empty, key);
		} // GetSystemValues

		/// <summary>
		/// Sets the system configuration value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, Boolean value) {
			this.SetLocalValues(Guid.Empty, key, value.ToString());
		} // SetSystemValue

		/// <summary>
		/// Sets the system configuration value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, Int32 value) {
			this.SetLocalValues(Guid.Empty, key, value.ToString());
		} // SetSystemValue

		/// <summary>
		/// Sets the system configuration value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, DateTime value) {
			this.SetLocalValues(Guid.Empty, key, value.ToString());
		} // SetSystemValue

		/// <summary>
		/// Sets the system configuration value associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetSystemValue(String key, Guid value) {
			this.SetLocalValues(Guid.Empty, key, value.ToString());
		} // SetSystemValue

		/// <summary>
		/// Sets the system configuration values associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetSystemValues(String key, params String[] values) {
			this.SetLocalValues(Guid.Empty, key, values);
		} // SetSystemValues
		#endregion

		#region Local configuration methods.
		/// <summary>
		/// Gets the local configuration guids.
		/// </summary>
		/// <returns>All the guids.</returns>
		public Guid[] GetLocalGuids() {
			try {
				List<Guid> result = new List<Guid>();
				XmlDocument xmlDocument = new XmlDocument();
				XmlNodeList guidNodes = null;

				// Read existing xml document.
				xmlDocument.Load(this.configFileName);

				// Get guid nodes.
				guidNodes = xmlDocument.SelectNodes("//Section[@Guid]");
				foreach (XmlNode guidNode in guidNodes) {
					try {
						result.Add(new Guid(guidNode.Attributes.GetNamedItem("Guid").Value));
					} catch {}
				}

				// Return the result.
				return result.ToArray();
			} catch (Exception exception) {
				// Return empty list.
				return new Guid[0];
			}
		} // GetLocalGuids

		/// <summary>
		/// Gets the local configuration keys associated with the guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the guid.</returns>
		public String[] GetLocalKeys(Guid guid) {
			try {
				List<String> result = new List<String>();
				XmlDocument xmlDocument = new XmlDocument();
				XmlNodeList keyNodes = null;

				// Read existing xml document.
				xmlDocument.Load(this.configFileName);

				// Get key nodes.
				keyNodes = xmlDocument.SelectNodes(String.Format("//Section[translate(@Guid, 'ABCDEFGHIJKLMNOPQRSTUVWXYZØÆÅ', 'abcdefghijklmnopqrstuvwxyzøæå')='{0}']/Property[@Key]", guid.ToString().ToLower()));
				foreach (XmlNode keyNode in keyNodes) {
					try {
						result.Add(keyNode.Attributes.GetNamedItem("Key").Value);
					} catch { }
				}

				// Return the result.
				return result.ToArray();
			} catch (Exception exception) {
				// Return empty list.
				return new String[0];
			}
		} // GetLocalKeys

		/// <summary>
		/// Gets the local configuration value associated with the guid and key.
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
		public Boolean GetLocalValue(Guid guid, String key, Boolean defaultValue) {
			try {
				String value = this.GetLocalValue(guid, key);
				return ((value.ToLower() == "true") || (value.ToLower() == "yes") || (value.ToLower() == "1"));
			} catch {
				// Return the default value.
				return defaultValue;
			}
		} // GetLocalValue

		/// <summary>
		/// Gets the local configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public Int32 GetLocalValue(Guid guid, String key, Int32 defaultValue) {
			try {
				String value = this.GetLocalValue(guid, key);
				return Int32.Parse(value);
			} catch {
				// Return the default value.
				return defaultValue;
			}
		} // GetLocalValue

		/// <summary>
		/// Gets the local configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public DateTime GetLocalValue(Guid guid, String key, DateTime defaultValue) {
			try {
				String value = this.GetLocalValue(guid, key);
				return DateTime.Parse(value);
			} catch {
				// Return the default value.
				return defaultValue;
			}
		} // GetLocalValue

		/// <summary>
		/// Gets the local configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public Guid GetLocalValue(Guid guid, String key, Guid defaultValue) {
			try {
				String value = this.GetSystemValue(key);
				return Guid.Parse(value);
			} catch {
				// Return the default value.
				return defaultValue;
			}
		} // GetLocalValue

		/// <summary>
		/// Gets the local configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public String GetLocalValue(Guid guid, String key, String defaultValue = null) {
			List<String> values = this.GetLocalValues(guid, key);
			if (values.Count > 0) {
				// Return the first value.
				return values[0];
			} else {
				// Return the default value.
				return defaultValue;
			}
		} // GetLocalValue

		/// <summary>
		/// Gets the local configuration values associated with the guid and key.
		/// If no value is associated with the guid and key, an empty list is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetLocalValues(Guid guid, String key) {
			try {
				List<String> result = new List<String>();
				XmlDocument xmlDocument = new XmlDocument();
				XmlNodeList valueNodes = null;

				// Read existing xml document.
				xmlDocument.Load(this.configFileName);
				
				// Get value node.
				valueNodes = xmlDocument.SelectNodes(String.Format("//Section[translate(@Guid, 'ABCDEFGHIJKLMNOPQRSTUVWXYZØÆÅ', 'abcdefghijklmnopqrstuvwxyzøæå')='{0}']/Property[translate(@Key, 'ABCDEFGHIJKLMNOPQRSTUVWXYZØÆÅ', 'abcdefghijklmnopqrstuvwxyzøæå') = '{1}']/Value", guid.ToString().ToLower(), key.ToLower()));
				foreach (XmlNode valueNode in valueNodes) {
					try {
						result.Add(valueNode.InnerText);
					} catch {}
				}

				// Return the result.
				return result;
			} catch (Exception exception) {
				// Return empty list.
				return new List<String>();
			}
		} // GetLocalValues

		/// <summary>
		/// Sets the local configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetLocalValue(Guid guid, String key, Boolean value) {
			this.SetLocalValues(guid, key, value.ToString());
		} // SetLocalValue

		/// <summary>
		/// Sets the local configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetLocalValue(Guid guid, String key, Int32 value) {
			this.SetLocalValues(guid, key, value.ToString());
		} // SetLocalValue

		/// <summary>
		/// Sets the local configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetLocalValue(Guid guid, String key, DateTime value) {
			this.SetLocalValues(guid, key, value.ToString());
		} // SetLocalValue

		/// <summary>
		/// Sets the local configuration value associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		public void SetLocalValue(Guid guid, String key, Guid value) {
			this.SetLocalValues(guid, key, value.ToString());
		} // SetLocalValue

		/// <summary>
		/// Sets the local configuration values associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetLocalValues(Guid guid, String key, params String[] values) {
			try {
				XmlDocument xmlDocument = new XmlDocument();
				XmlNode rootNode = null;
				XmlNode guidNode = null;
				XmlAttribute guidAttribute = null;
				XmlNode keyNode = null;
				XmlAttribute keyAttribute = null;
				XmlNode valueNode = null;

				// Read existing xml document, or create new xml document.
				try {
					xmlDocument.Load(this.configFileName);
				} catch {
					xmlDocument = new XmlDocument();
				}

				// Get the root node, or create missing root node.
				if ((xmlDocument.FirstChild == null) || (xmlDocument.FirstChild.Name != "NdkFrameworkConfiguration")) {
					rootNode = xmlDocument.CreateElement("NdkFrameworkConfiguration");
					xmlDocument.AppendChild(rootNode);
				}
				rootNode = xmlDocument["NdkFrameworkConfiguration"];

				// Get guid node or create missing guid node.
				guidNode = xmlDocument.SelectSingleNode(String.Format("//Section[translate(@Guid, 'ABCDEFGHIJKLMNOPQRSTUVWXYZØÆÅ', 'abcdefghijklmnopqrstuvwxyzøæå')='{0}']", guid.ToString().ToLower()));
				if (guidNode == null) {
					guidNode = xmlDocument.CreateElement("Section");
					guidAttribute = xmlDocument.CreateAttribute("Guid");
					guidAttribute.Value = guid.ToString();
					guidNode.Attributes.Append(guidAttribute);
					rootNode.AppendChild(guidNode);
				}

				// Get the key node or create missing key node.
				keyNode = xmlDocument.SelectSingleNode(String.Format("//Section[translate(@Guid, 'ABCDEFGHIJKLMNOPQRSTUVWXYZØÆÅ', 'abcdefghijklmnopqrstuvwxyzøæå')='{0}']/Property[translate(@Key, 'ABCDEFGHIJKLMNOPQRSTUVWXYZØÆÅ', 'abcdefghijklmnopqrstuvwxyzøæå') = '{1}']", guid.ToString().ToLower(), key.ToLower()));
				if (keyNode == null) {
					keyNode = xmlDocument.CreateElement("Property");
					keyAttribute = xmlDocument.CreateAttribute("Key");
					keyAttribute.Value = key;
					keyNode.Attributes.Append(keyAttribute);
					guidNode.AppendChild(keyNode);
				}

				// Clear existing values, and add new values.
				keyNode.InnerText = String.Empty;
				foreach (String value in values) {
					valueNode = xmlDocument.CreateElement("Value");
					valueNode.InnerText = value;
					keyNode.AppendChild(valueNode);
				}

				// Write the xml document.
				xmlDocument.Save(this.configFileName);
			} catch (Exception exception) {
				//throw;
			}
		} // SetLocalValues
		#endregion

	} // Configuration
	#endregion

} // NDK.Framework