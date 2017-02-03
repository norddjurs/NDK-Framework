using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.Mail;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.DirectoryServices.AccountManagement;

namespace NDK.Framework {

	#region PluginList class.
	/// <summary>
	/// Use this class to load specifig types from all assemblies in a directory.
	/// Original code from: https://github.com/rpc-scandinavia/rpc-base/blob/master/Base/RPC%20Base%20-%20Plugin.cs
	/// </summary>
	public class PluginList<PluginType> : List<PluginType> {

        /// <summary>
        /// Load types from all assemblies in the directory where the application executable exists.
        /// Sub directories are not searched.
        /// </summary>
        public PluginList() : this(Path.GetDirectoryName(Application.ExecutablePath), false) {
        } // PluginList

        /// <summary>
        /// Load types from all assemblies in the directory. Sub directories are not searched.
        /// </summary>
        /// <param name="directory">The root directory to search.</param>
        public PluginList(String directory) : this(directory, false) {
        } // PluginList

        /// <summary>
        /// Load types from all assemblies in the directory.
        /// </summary>
        /// <param name="directory">The root directory to search.</param>
        /// <param name="recursive">True is sub directories should be searched.</param>
        public PluginList(String directory, Boolean recursive) : base() {
            try {
                // Declare variables and find all DLL files in the argumented directory.
                List<String> pluginFiles = new List<String>();
                if (recursive == true) {
                    pluginFiles.AddRange(Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories));
                    pluginFiles.AddRange(Directory.GetFiles(directory, "*.exe", SearchOption.AllDirectories));
                } else {
                    pluginFiles.AddRange(Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly));
                    pluginFiles.AddRange(Directory.GetFiles(directory, "*.exe", SearchOption.TopDirectoryOnly));
                }

                // Load the plugins.
                LoadPlugins(pluginFiles.ToArray());
            } catch { }
        } // PluginList

        /// <summary>
        /// Load types from the given assemblies.
        /// </summary>
        /// <param name="pluginFiles">The assembly files to search.</param>
        public void LoadPlugins(params String[] pluginFiles) {
            try {
                Boolean pluginFound;
                Assembly pluginAssembly;
                FileStream pluginStream;
                Byte[] pluginBinary;
                Type[] pluginClasses;
                ConstructorInfo pluginConstructor;
                PluginType plugin;

                // Load the plugin DLL and instantiate all classes implementing/inheriting the PluginType.
                // Note that each DLL can contain several classes implementing/inheriting the PluginType.
                foreach (String pluginFileName in pluginFiles) {
                    try {
                        // Load the assembly (DLL) into memory.
                        // This frees the filesystem lock on the file, when the streams Close method is executed, allowing
                        // the file to be replaced while in use.
                        // *** THIS TAKES LONG TIME, SO FOR NOW THE ASSEMBLY IS LOADED FROM THE FILE ***
                        //						pluginStream		= File.Open(pluginFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        //						pluginBinary		= new byte[pluginStream.Length];
                        //						pluginStream.Read(pluginBinary, 0, (int)pluginStream.Length);
                        //						pluginStream.Close();

                        // Inspect the assembly (DLL), to see if it already is loaded.
                        pluginFound = false;
                        pluginAssembly = null;
                        ////						pluginAssembly		= Assembly.ReflectionOnlyLoadFrom(pluginFileName);
                        //////						pluginAssembly		= Assembly.ReflectionOnlyLoad(pluginBinary);
                        ////						foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                        ////							if ((pluginAssembly != null) && (pluginAssembly.FullName == assembly.FullName)) {
                        ////								pluginAssembly	= assembly;
                        ////								pluginFound		= true;
                        ////							}
                        ////						}

                        // Load the plugin, from the binary source.
                        if (pluginFound == false) {
                            // *** THIS TAKES LONG TIME, SO FOR NOW THE ASSEMBLY IS LOADED FROM THE FILE ***
                            //							pluginAssembly		= Assembly.Load(pluginBinary);
                            //							pluginAssembly		= Assembly.LoadFile(pluginFileName);
                            pluginAssembly = Assembly.UnsafeLoadFrom(pluginFileName);
                        }

                        // Get the plugin classes from the assembly.
                        pluginClasses = pluginAssembly.GetTypes();
                        foreach (Type pluginClass in pluginClasses) {
                            try {
                                if (pluginClass == typeof(PluginType)) {
                                    // Instantiate the type, and add it to the collection.
                                    //	This requires that the pluginClass has a constructor that takes no arguments.
                                    pluginConstructor = pluginClass.GetConstructor(Type.EmptyTypes);
                                    plugin = (PluginType)pluginConstructor.Invoke(new Object[0]);
                                    this.Add(plugin);
                                } else if ((typeof(PluginType).IsClass == true) && (pluginClass.IsSubclassOf(typeof(PluginType)) == true)) {
                                    // Instantiate the type, and add it to the collection.
                                    //	This requires that the pluginClass has a constructor that takes no arguments.
                                    pluginConstructor = pluginClass.GetConstructor(Type.EmptyTypes);
                                    plugin = (PluginType)pluginConstructor.Invoke(new Object[0]);
                                    this.Add(plugin);
                                } else if ((typeof(PluginType).IsInterface == true) && (pluginClass.GetInterface(typeof(PluginType).Name) != null)) {
                                    // Instantiate the type, and add it to the collection.
                                    //	This requires that the pluginClass has a constructor that takes no arguments.
                                    pluginConstructor = pluginClass.GetConstructor(Type.EmptyTypes);
                                    plugin = (PluginType)pluginConstructor.Invoke(new Object[0]);
                                    this.Add(plugin);
                                }
                            } catch { }
                        }
                    } catch { }
                }
            } catch { }
        } // LoadPlugins

        /// <summary>
        /// Get a empty PluginList object.
        /// </summary>
        /// <returns>An empty list.</returns>
        public static PluginList<PluginType> Empty() {
            // This creates an empty PluginList object, because the LoadPlugins method can't load
            // the files from the directlry String.Empty.
            return new PluginList<PluginType>(String.Empty);
        } // Empty

    } // PluginList
	#endregion

	#region PluginBase class.
	/// <summary>
	/// Extend this abstract class and implement abstract methods to create a NDK Framework plugin, that
	/// can be executed as a service og from the commandline.
	/// 
	/// Note that the NDK Framework implementations of IConfig and ILogger are awailable, but it is recommended
	/// to use the configuration and logging methods inherited from this class, because they use automatically
	/// use the guid etc.
	/// </summary>
	public abstract class PluginBase {

		#region Properties.
		/// <summary>
		/// The initialized configuration class.
		/// </summary>
		protected IConfiguration Config { get; private set; }

		/// <summary>
		/// The initialized logger class.
		/// </summary>
		protected ILogger Logger { get; private set; }

		/// <summary>
		/// The initialized arguments.
		/// </summary>
		protected String[] Arguments { get; private set; }

		/// <summary>
		/// The initialized and plugins.
		/// </summary>
		protected PluginList<PluginBase> Plugins { get; private set; }

		/// <summary>
		/// The object can be tagged with some object.
		/// </summary>
		public Object Tag { get; set; }
		#endregion

		#region Methods.
		/// <summary>
		/// Initialize the plugin, with defaults.
		/// </summary>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="arguments">The arguments.</param>
		public void Initialize(PluginList<PluginBase> plugins, params String[] arguments) {
			this.Config = new Configuration();
			this.Logger = new Logger(this.Config);
			this.Plugins = plugins;
			this.Arguments = arguments;

			// Initialize all plugins, that are not initialized.
			foreach (PluginBase plugin in this.Plugins) {
				if (plugin.Plugins == null) {
					plugin.Initialize(this.Plugins, this.Config, this.Logger, this.Arguments);
				}
			}
		} // Initialize

		/// <summary>
		/// Initialize the plugin.
		/// </summary>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="config">The configuration.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="arguments">The arguments.</param>
		public void Initialize(PluginList<PluginBase> plugins, IConfiguration config, ILogger logger, params String[] arguments) {
			this.Config = config;
			this.Logger = logger;
			this.Plugins = plugins;
			this.Arguments = arguments;

			// Initialize all plugins, that are not initialized.
			foreach (PluginBase plugin in this.Plugins) {
				if (plugin.Plugins == null) {
					plugin.Initialize(this.Plugins, this.Config, this.Logger, this.Arguments);
				}
			}
		} // Initialize
		#endregion

		#region Configuration methods.
		/// <summary>
		/// Gets the configuration keys associated with this plugin.
		/// </summary>
		/// <returns>All the keys associated with this plugin.</returns>
		public String[] GetConfigKeys() {
			return this.Config.GetKeys(this.GetGuid());
		} // GetConfigKeys

		/// <summary>
		/// Gets the configuration value associated with this plugin and the key.
		/// If more then one value is associated with this plugin and the key, the first value is returned.
		/// If no value is associated with this plugin and the key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value (null).</param>
		/// <returns>The value.</returns>
		public String GetConfigValue(String key, String defaultValue = null) {
			return this.Config.GetValue(this.GetGuid(), key, defaultValue);
		} // GetConfigValue

		/// <summary>
		/// Gets the integer configuration value associated with this plugin and the key.
		/// If more then one value is associated with this plugin and the key, the first value is returned.
		/// If no value is associated with this plugin and the key, or the value isn't an integer, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public Int32 GetConfigValue(String key, Int32 defaultValue) {
			try {
				return Int32.Parse(this.Config.GetValue(this.GetGuid(), key));
			} catch {
				return defaultValue;
			}
		} // GetConfigValue

		/// <summary>
		/// Gets the datetime configuration value associated with this plugin and the key.
		/// If more then one value is associated with this plugin and the key, the first value is returned.
		/// If no value is associated with this plugin and the key, or the value isn't an datetime, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public DateTime GetConfigValue(String key, DateTime defaultValue) {
			try {
				return DateTime.Parse(this.Config.GetValue(this.GetGuid(), key));
			} catch {
				return defaultValue;
			}
		} // GetConfigValue

		/// <summary>
		/// Gets the guid configuration value associated with this plugin and the key.
		/// If more then one value is associated with this plugin and the key, the first value is returned.
		/// If no value is associated with this plugin and the key, or the value isn't an guid, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public Guid GetConfigValue(String key, Guid defaultValue) {
			try {
				return Guid.Parse(this.Config.GetValue(this.GetGuid(), key));
			} catch {
				return defaultValue;
			}
		} // GetConfigValue

		/// <summary>
		/// Gets the configuration values associated with this plugin and the key.
		/// If no value is associated with this plugin and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		public List<String> GetConfigValues(String key) {
			return this.Config.GetValues(this.GetGuid(), key);
		} // GetConfigValues

		/// <summary>
		/// Sets the configuration values associated with this plugin and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		public void SetConfigValues(String key, params String[] values) {
			this.Config.SetValues(this.GetGuid(), key, values);
		} // SetConfigValues
		#endregion

		// TODO: Some current user key/value storage, perhaps in the registry.

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
		/// The information is only logged, if DEBUG logging is configures.
		/// </summary>
		/// <param name="text">The debug text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		public void LogDebug(String text, params Object[] formatArgs) {
			this.Logger.LogDebug(text, formatArgs);
		} // LogDebug

		/// <summary>
		/// Writes the text to the error log.
		/// The information is only logged, if ERROR logging is configures.
		/// </summary>
		/// <param name="text">The error text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		public void LogError(String text, params Object[] formatArgs) {
			this.Logger.LogError(text, formatArgs);
		} // LogError

		/// <summary>
		/// Writes the exception and stack traces to the error log.
		/// The information is only logged, if ERROR logging is configures.
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
			return this.GetUser(Environment.UserName);
		} // GetCurrentUser

		/// <summary>
		/// Gets the user identified by the user id.
		/// The user id can be person number (CPR), Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The user id to find.</param>
		/// <returns>The matching user or null.</returns>
		public Person GetUser(String userId) {
			// Connect to the Active Directory.
			PrincipalContext context = new PrincipalContext(ContextType.Domain);
			Person user = null;

			// Search cpr number.
			// TODO: Add check for numeric userId.
			if (userId.Trim().Replace("-", String.Empty).Length == 10) {
				try {
					// Get which attribute stores the cpr number.
					String userCprAttribute = this.GetConfigValue("ActiveDirectoryCprAttribute", "EmployeeId");

					// Initialize the query.
					Person userQueryFilter = new Person(context);
					switch (userCprAttribute.ToLower()) {
						case "extensionattribute1":
							userQueryFilter.ExtensionAttribute1 = userId;
							break;
						case "extensionattribute2":
							userQueryFilter.ExtensionAttribute2 = userId;
							break;
						case "extensionattribute3":
							userQueryFilter.ExtensionAttribute3 = userId;
							break;
						case "extensionattribute4":
							userQueryFilter.ExtensionAttribute4 = userId;
							break;
						case "extensionattribute5":
							userQueryFilter.ExtensionAttribute5 = userId;
							break;
						case "extensionattribute6":
							userQueryFilter.ExtensionAttribute6 = userId;
							break;
						case "extensionattribute7":
							userQueryFilter.ExtensionAttribute7 = userId;
							break;
						case "extensionattribute8":
							userQueryFilter.ExtensionAttribute8 = userId;
							break;
						case "extensionattribute9":
							userQueryFilter.ExtensionAttribute9 = userId;
							break;
						case "extensionattribute10":
							userQueryFilter.ExtensionAttribute10 = userId;
							break;
						case "extensionattribute11":
							userQueryFilter.ExtensionAttribute11 = userId;
							break;
						case "extensionattribute12":
							userQueryFilter.ExtensionAttribute12 = userId;
							break;
						case "extensionattribute13":
							userQueryFilter.ExtensionAttribute13 = userId;
							break;
						case "extensionattribute14":
							userQueryFilter.ExtensionAttribute14 = userId;
							break;
						case "extensionattribute15":
							userQueryFilter.ExtensionAttribute15 = userId;
							break;
						case "employeeid":
						default:
							userQueryFilter.EmployeeId = userId;
							break;
					}
					PrincipalSearcher searcher = new PrincipalSearcher();
					searcher.QueryFilter = userQueryFilter;
					user = (Person)searcher.FindOne();
				} catch { }
			}

			// Search guid.
			Guid userGuid = Guid.Empty;
			if ((user == null) && (Guid.TryParse(userId, out userGuid) == true)) {
				try {
					user = Person.FindByIdentity(context, IdentityType.Guid, userId);
				} catch {}
			}

			// Search distinguished name.
			if (user == null) {
				try {
					user = Person.FindByIdentity(context, IdentityType.DistinguishedName, userId);
				} catch { }
			}

			// Search sam account name.
			if (user == null) {
				try {
					user = Person.FindByIdentity(context, IdentityType.SamAccountName, userId);
				} catch { }
			}

			// Search user principal name.
			if (user == null) {
				try {
					user = Person.FindByIdentity(context, IdentityType.UserPrincipalName, userId);
				} catch { }
			}

			// Search security identifier.
			if (user == null) {
				try {
					user = Person.FindByIdentity(context, IdentityType.Sid, userId);
				} catch { }
			}

			// Return the user or null.
			return user;
		} // GetUser


		public enum UserQuery {
			/// <summary>
			/// Get all enabled users.
			/// </summary>
			ENABLED = 1,

			/// <summary>
			/// Get all disabled users.
			/// </summary>
			DISABLED = 2,

			/// <summary>
			/// Get all users, where the password newer expires.
			/// </summary>
			PASSWORD_NEWER_EXPIRES = 3,

			/// <summary>
			/// Get all users, where the password expire at some time.
			/// </summary>
			PASSWORD_EXPIRES = 4,

			/// <summary>
			/// Get all users, where a password is not required.
			/// </summary>
			PASSWORD_NOT_REQUIRED = 5,

			/// <summary>
			/// Get all users, where a password is required.
			/// </summary>
			PASSWORD_REQUIRED = 6,

			/// <summary>
			/// Get all users, who can change their own password.
			/// </summary>
			PASSWORD_CHANGE_ENABLED = 7,

			/// <summary>
			/// Get all users, who can not change their own password.
			/// </summary>
			PASSWORD_CHANGE_DISABLED = 8,

			/// <summary>
			/// Get all users, where the account is not expired.
			/// </summary>
			ACCOUNT_NOT_EXPIRED = 101,

			/// <summary>
			/// Get all users, where the account is expired.
			/// </summary>
			ACCOUNT_EXPIRED = 102,

			/// <summary>
			/// Gets all users, where the account is not locked out.
			/// </summary>
			ACCOUNT_NOT_LOCKED_OUT = 103,

			/// <summary>
			/// Gets all users, where the account is locked out.
			/// </summary>
			ACCOUNT_LOCKED_OUT = 104,




			/// <summary>
			/// Get all users.
			/// </summary>
			ALL = 0
		} // UserQuery




		/// <summary>
		/// Gets all users.
		/// Use the filter, to only get for example disabled users.
		/// </summary>
		/// <param name="userFilter">Filter which users to query.</param>
		/// <returns>All users.</returns>
		public List<Person> GetAllUsers(UserQuery userFilter = UserQuery.ALL) {
			List<Person> users = new List<Person>();

			// Connect to the Active Directory.
			PrincipalContext context = new PrincipalContext(ContextType.Domain);

			// Create local datetime.
			DateTime localDateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

			// Find all users, matching the filter.
			Person userQueryFilter = new Person(context);
			switch (userFilter) {
				// Simple queries.
				case UserQuery.ENABLED:
					userQueryFilter.Enabled = true;
					break;
				case UserQuery.DISABLED:
					userQueryFilter.Enabled = false;
					break;
				case UserQuery.PASSWORD_NEWER_EXPIRES:
					userQueryFilter.PasswordNeverExpires = true;
					break;
				case UserQuery.PASSWORD_EXPIRES:
					userQueryFilter.PasswordNeverExpires = false;
					break;
				case UserQuery.PASSWORD_NOT_REQUIRED:
					userQueryFilter.PasswordNotRequired = true;
					break;
				case UserQuery.PASSWORD_REQUIRED:
					userQueryFilter.PasswordNotRequired = false;
					break;
				case UserQuery.PASSWORD_CHANGE_ENABLED:
					userQueryFilter.UserCannotChangePassword = false;
					break;
				case UserQuery.PASSWORD_CHANGE_DISABLED:
					userQueryFilter.UserCannotChangePassword = true;
					break;

				// Advanced queries.
				case UserQuery.ACCOUNT_NOT_EXPIRED:
					userQueryFilter.AdvancedSearchFilter.AccountExpirationDate(localDateTime, MatchType.LessThanOrEquals);
					break;
				case UserQuery.ACCOUNT_EXPIRED:
					userQueryFilter.AdvancedSearchFilter.AccountExpirationDate(localDateTime, MatchType.GreaterThan);
					break;
				case UserQuery.ACCOUNT_NOT_LOCKED_OUT:
					userQueryFilter.AdvancedSearchFilter.AccountLockoutTime(localDateTime, MatchType.LessThanOrEquals);
					break;
				case UserQuery.ACCOUNT_LOCKED_OUT:
					userQueryFilter.AdvancedSearchFilter.AccountLockoutTime(localDateTime, MatchType.GreaterThan);
					break;

				case UserQuery.ALL:
				default:
					userQueryFilter.Name = "*";
					break;
			}
			PrincipalSearcher searcher = new PrincipalSearcher();
			searcher.QueryFilter = userQueryFilter;
			PrincipalSearchResult<Principal> searchResults = searcher.FindAll();
			foreach (Principal searchResult in searchResults) {
				users.Add((Person)searchResult);
			}

			// Return the found users.
			return users;
		} // GetAllUsers

		//AccountInactive

		/// <summary>
		/// Gets the group identified by the group id.
		/// The group id can be Guid, Distinguished Name, Sam Account Name, User Principal Name or Security Identifier.
		/// </summary>
		/// <param name="userId">The group id to find.</param>
		/// <returns>The matching group or null.</returns>
		public GroupPrincipal GetGroup(String groupId) {
			// Connect to the Active Directory.
			PrincipalContext context = new PrincipalContext(ContextType.Domain);
			GroupPrincipal group = null;

			// Search guid.
			Guid groupGuid = Guid.Empty;
			if ((group ==  null) && (Guid.TryParse(groupId, out groupGuid) == true)) {
				try {
					group = GroupPrincipal.FindByIdentity(context, IdentityType.Guid, groupId);
				} catch {}
			}

			// Search distinguished name.
			if (group == null) {
				try {
					group = GroupPrincipal.FindByIdentity(context, IdentityType.DistinguishedName, groupId);
				} catch { }
			}

			// Search sam account name.
			if (group == null) {
				try {
					group = GroupPrincipal.FindByIdentity(context, IdentityType.SamAccountName, groupId);
				} catch { }
			}

			// Search user principal name.
			if (group == null) {
				try {
					group = GroupPrincipal.FindByIdentity(context, IdentityType.UserPrincipalName, groupId);
				} catch { }
			}

			// Search security identifier.
			if (group == null) {
				try {
					group = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, groupId);
				} catch { }
			}

			// Return the group or null.
			return group;
		} // GetGroup

		/// <summary>
		/// Gets all groups.
		/// </summary>
		/// <returns>All groups.</returns>
		public List<GroupPrincipal> GetAllGroups() {
			List<GroupPrincipal> groups = new List<GroupPrincipal>();

			// Connect to the Active Directory.
			PrincipalContext context = new PrincipalContext(ContextType.Domain);

			// Find all groups.
			GroupPrincipal groupQueryFilter = new GroupPrincipal(context);
			groupQueryFilter.Name = "*";
			PrincipalSearcher searcher = new PrincipalSearcher();
			searcher.QueryFilter = groupQueryFilter;
			PrincipalSearchResult<Principal> searchResults = searcher.FindAll();
			foreach (Principal searchResult in searchResults) {
				groups.Add((GroupPrincipal)searchResult);
			}

			// Return the found groups.
			return groups;
		} // GetAllGroups

		/// <summary>
		/// Gets if the current user is member of the group.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns>True if the current user is member of the group.</returns>
		public Boolean IsUserMemberOfGroup(GroupPrincipal group, Boolean recursive = true) {
			return this.IsUserMemberOfGroup(this.GetCurrentUser(), group, recursive);
		} // IsUserMemberOfGroup

		/// <summary>
		/// Gets if the user is member of the group.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="group">The group.</param>
		/// <param name="recursive">True to search recursive.</param>
		/// <returns>True if the user is member of the group.</returns>
		public Boolean IsUserMemberOfGroup(Person user, GroupPrincipal group, Boolean recursive = true) {
			return false;
		} // IsUserMemberOfGroup
		#endregion

		// TODO: SOFD methods.

		#region Event methods.
		/// <summary>
		/// Default event id.
		/// </summary>
		public const Int32 EVENT_NONE = 0;

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
		public void TrySendEvent(Int32 eventId, Object keyValuePairs = null) {
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
		public void SendEvent(Int32 eventId, Object keyValuePairs = null) {
			// Validate the event id.
			if (eventId > 999) {
				throw new Exception(String.Format("To avoid unintended execution, only global reserved event id (< 1000) is allowed to be executed on all loaded plugins.", eventId));
			}

			// Initialize a dictionary from the anonymous object
			IDictionary<String, Object> eventObjects = new Dictionary<String, Object>();
			if (keyValuePairs != null) {
				PropertyInfo[] properties = keyValuePairs.GetType().GetProperties();
				foreach (PropertyInfo prop in properties) {
					eventObjects.Add(prop.Name, prop.GetValue(keyValuePairs, null));
				}
			}

			// Execute the RunEvent methods.
			foreach (PluginBase plugin in this.Plugins) {
				// Log.
				this.Logger.LogDebug("Event: Triggering event id {0} in plugin {1}   {2}.", eventId, plugin.GetGuid(), plugin.GetName());

				// Call the RunEvent method.
				plugin.RunEvent(eventId, eventObjects);
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
			foreach (PluginBase plugin in this.Plugins) {
				if (plugin.GetGuid().Equals(pluginGuid) == true) {
					// Log.
					this.Logger.LogDebug("Event: Triggering event id {0} in plugin {1}   {2}.", eventId, plugin.GetGuid(), plugin.GetName());

					// Call the RunEvent method.
					plugin.RunEvent(eventId, eventObjects);

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
		/// Gets the unique plugin guid.
		/// When implementing a plugin, this method should return the same unique guid every time. 
		/// </summary>
		/// <returns></returns>
		public abstract Guid GetGuid();

		/// <summary>
		/// Gets the the plugin name.
		/// When implementing a plugin, this method should return a proper display name.
		/// </summary>
		/// <returns></returns>
		public abstract String GetName();

		/// <summary>
		/// Run the plugin.
		/// This method is invoked by the service application or the commandline application.
		/// 
		/// If the method finishes when invoked by the service application, it is reinvoked after a short while as long as the
		/// service application is running.
		/// 
		/// Take care to write good comments in the code, log as much as possible, as correctly as possible (little normal, much debug).
		/// </summary>
		public abstract void Run();

		/// <summary>
		/// Handle events.
		/// This method is invoked by another plugin.
		/// 
		/// When implementing a plugin, only use your own event id greater then 1000. Event id less then 1000 is reserved
		/// for global events. They will be declared as public constants in the PluginBase class like "EVENT_NONE".
		/// </summary>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="eventObjects">The event objects.</param>
		public virtual void RunEvent(Int32 eventId, IDictionary<String, Object> eventObjects) {
			// Do nothing.
		} // Event
		#endregion

	} // PluginBase
	#endregion

} // NDK.Framework