using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace NDK.Framework {

	#region IConfiguration interface.
	/// <summary>
	/// Configuration interface.
	/// Multiple configurationsets can be stored and identified by a GUID.
	/// </summary>
	public interface IConfiguration {

		/// <summary>
		/// Gets the configuration guids.
		/// </summary>
		/// <returns>All the guids.</returns>
		Guid[] GetGuids();

		/// <summary>
		/// Gets the configuration keys associated with the empty guid.
		/// </summary>
		/// <returns>All the keys associated with the empty guid.</returns>
		String[] GetKeys();

		/// <summary>
		/// Gets the configuration keys associated with the guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the guid.</returns>
		String[] GetKeys(Guid guid);

		/// <summary>
		/// Gets the configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		String GetValue(String key, String defaultValue = null);

		/// <summary>
		/// Gets the configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		String GetValue(Guid guid, String key, String defaultValue = null);

		/// <summary>
		/// Gets the configuration values associated with the empty guid and key.
		/// If no value is associated with the empty guid and key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetValues(String key);

		/// <summary>
		/// Gets the configuration values associated with the guid and key.
		/// If no value is associated with the key, an empty list is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetValues(Guid guid, String key);

		/// <summary>
		/// Sets the configuration values associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetValues(String key, params String[] values);

		/// <summary>
		/// Sets the configuration values associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetValues(Guid guid, String key, params String[] values);

	} // IConfiguration
	#endregion

	#region Configuration class.
	/// <summary>
	/// Default configuration class, that stores the data in a XML file.
	/// The configuration file name is the same as the entry assembly (.exe), but with the ".xml" file extension.
	/// </summary>
	public class Configuration : IConfiguration {
		private String configFileName = null;

		public Configuration() {
			this.configFileName = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml");
		} // Configuration

		#region Implement IConfiguration
		/// <summary>
		/// Gets the configuration guids.
		/// </summary>
		/// <returns>All the guids.</returns>
		public Guid[] GetGuids() {
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
		} // GetGuids

		/// <summary>
		/// Gets the configuration keys associated with the empty guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the empty guid.</returns>
		public String[] GetKeys() {
			return this.GetKeys(Guid.Empty);
		}// GetKeys

		/// <summary>
		/// Gets the configuration keys associated with the guid.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <returns>All the keys associated with the guid.</returns>
		public String[] GetKeys(Guid guid) {
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
		} // GetKeys

		/// <summary>
		/// Gets the configuration value associated with the empty guid and key.
		/// If more then one value is associated with the empty guid and key, the first value is returned.
		/// If no value is associated with the empty guid and key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public String GetValue(String key, String defaultValue = null) {
			return this.GetValue(Guid.Empty, key, defaultValue);
		} // GetValue

		/// <summary>
		/// Gets the configuration value associated with the guid and key.
		/// If more then one value is associated with the guid and key, the first value is returned.
		/// If no value is associated with the guid and key, the default value is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public String GetValue(Guid guid, String key, String defaultValue = null) {
			List<String> values = this.GetValues(guid, key);
			if (values.Count > 0) {
				// Return the first value.
				return values[0];
			} else {
				// Return the default value.
				return defaultValue;
			}
		} // GetValue

		/// <summary>
		/// Gets the configuration values associated with the empty guid and key.
		/// If no value is associated with the empty guid and key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetValues(String key) {
			return this.GetValues(Guid.Empty, key);
		} // GetValues

		/// <summary>
		/// Gets the configuration values associated with the guid and key.
		/// If no value is associated with the guid and key, an empty list is returned.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetValues(Guid guid, String key) {
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
		} // GetValues

		/// <summary>
		/// Sets the configuration values associated with the empty guid and key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetValues(String key, params String[] values) {
			this.SetValues(Guid.Empty, key, values);
		} // SetValues

		/// <summary>
		/// Sets the configuration values associated with the guid and key.
		/// </summary>
		/// <param name="guid">The guid.</param>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetValues(Guid guid, String key, params String[] values) {
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
		} // SetValues
		#endregion

	} // Configuration
	#endregion

} // NDK.Framework