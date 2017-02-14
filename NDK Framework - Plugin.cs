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

	#region IPlugin interface
	/// <summary>
	/// Plugin interface.
	/// The reason this interface exist, is to ensure that all plugin classes implement the minimum set of methods.
	/// </summary>
	public interface IPlugin {

		#region Properties.
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
		/// Initialize the plugin, with defaults.
		/// </summary>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="arguments">The arguments.</param>
		void Initialize(PluginList<IPlugin> plugins, params String[] arguments);

		/// <summary>
		/// Initialize the plugin.
		/// </summary>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="config">The configuration.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="arguments">The arguments.</param>
		void Initialize(PluginList<IPlugin> plugins, IConfiguration config, ILogger logger, params String[] arguments);
		#endregion

		#region Abstract and Virtual methods.
		/// <summary>
		/// Gets the unique plugin guid used when referencing resources.
		/// When implementing a plugin, this method should return the same unique guid every time. 
		/// </summary>
		/// <returns></returns>
		Guid GetGuid();

		/// <summary>
		/// Gets the the plugin name.
		/// When implementing a plugin, this method should return a proper display name.
		/// </summary>
		/// <returns></returns>
		String GetName();

		/// <summary>
		/// Run the plugin.
		/// This method is invoked by the service application or the commandline application.
		/// 
		/// If the method finishes when invoked by the service application, it is reinvoked after a short while as long as the
		/// service application is running.
		/// 
		/// Take care to write good comments in the code, log as much as possible, as correctly as possible (little normal, much debug).
		/// </summary>
		void Run();

		/// <summary>
		/// Handle events.
		/// This method is invoked by another plugin.
		/// 
		/// When implementing a plugin, only use your own event id greater then 1000. Event id less then 1000 is reserved
		/// for global events. They will be declared in the PluginEvents enum.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="eventObjects">The event objects.</param>
		void RunEvent(Guid sender, Int32 eventId, IDictionary<String, Object> eventObjects);
		#endregion

	} // IPlugin
	#endregion

	#region PluginBase class.
	/// <summary>
	/// Extend this abstract class and implement abstract methods to create a NDK Framework plugin, that
	/// can be executed as a service og from the commandline.
	/// 
	/// Note that the NDK Framework implementations of IConfig and ILogger are awailable, but it is recommended
	/// to use the configuration and logging methods inherited from this class, because they automatically
	/// use the guid etc.
	/// </summary>
	public abstract class PluginBase : FrameworkBase, IFramework, IPlugin {

		#region Initialize methods.
		/// <summary>
		/// Initialize the plugin, with defaults.
		/// </summary>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="arguments">The arguments.</param>
		public void Initialize(PluginList<IPlugin> plugins, params String[] arguments) {
			base.Initialize(this.GetGuid(), plugins, arguments);
		} // Initialize

		/// <summary>
		/// Initialize the plugin.
		/// </summary>
		/// <param name="plugins">The loaded plugins.</param>
		/// <param name="config">The configuration.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="arguments">The arguments.</param>
		public void Initialize(PluginList<IPlugin> plugins, IConfiguration config, ILogger logger, params String[] arguments) {
			base.Initialize(this.GetGuid(), plugins, config, logger, arguments);
		} // Initialize
		#endregion

		#region Abstract and Virtual methods.
		/// <summary>
		/// Gets the unique plugin guid used when referencing resources.
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
		/// for global events. They will be declared in the PluginEvents enum.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="eventObjects">The event objects.</param>
		public virtual void RunEvent(Guid sender, Int32 eventId, IDictionary<String, Object> eventObjects) {
			// Do nothing.
		} // Event
		#endregion

	} // PluginBase
	#endregion

	#region PluginEvents
	public enum PluginEvents {
		/// <summary>
		/// Default event id.
		/// </summary>
		EVENT_NONE = 0

	} // PluginEvents
	#endregion

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

} // NDK.Framework