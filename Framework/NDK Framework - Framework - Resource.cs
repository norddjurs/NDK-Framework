using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;

namespace NDK.Framework {

	#region Framework
	/// <summary>
	/// This partial part of the class, implements resource access.
	/// </summary>
	public abstract partial class Framework : IFramework {

		#region Private resource initialization
		private void ResourceInitialize() {
			// Register the custom assembly resolver.
			// This loades embedded assembly files (DLL).
			if (Framework.frameworkFirstInitialization == true) {
				this.LogInternal("Resource: Attached ResolveEventHandler.");
				AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.ResourceAssemblyResolver);
			}
		} // ResourceInitialize
		#endregion

		#region Public resource methods.
		/// <summary>
		/// Gets the resource keys, to the resources embedded in the assemblies.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <returns>The resource keys.</returns>
		public String[] GetResourceKeys() {
			List<String> keys = new List<String>();

			/* At this point, I have choosen not to load Strings and Images from all loaded assemblies.
			// Get all resources in all assemblies, and strip the "<namespace>.resources" part from the resource names.
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				foreach (String name in assembly.GetManifestResourceNames()) {
					Int32 index = name.ToLower().IndexOf(".resources.");
					String name1 = name.Substring(index + 11);
					if ((index > -1) && (keys.Contains(name1) == false)) {
						keys.Add(name1);
					}
				}
			}
			*/

			// Get all resources in the calling assembly, and strip the "<namespace>.resources" part from the resource names.
			foreach (String name in Assembly.GetCallingAssembly().GetManifestResourceNames()) {
				Int32 index = name.ToLower().IndexOf(".resources.");
				String name1 = name.Substring(index + 11);
				if ((index > -1) && (keys.Contains(name1) == false)) {
					keys.Add(name1);
				}
			}

			// Get all resources in the executing assembly, and strip the "<namespace>.resources" part from the resource names.
			foreach (String name in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
				Int32 index = name.ToLower().IndexOf(".resources.");
				String name1 = name.Substring(index + 11);
				if ((index > -1) && (keys.Contains(name1) == false)) {
					keys.Add(name1);
				}
			}

			// Get all resources in the entry assembly, and strip the "<namespace>.resources" part from the resource names.
			foreach (String name in Assembly.GetEntryAssembly().GetManifestResourceNames()) {
				Int32 index = name.ToLower().IndexOf(".resources.");
				String name1 = name.Substring(index + 11);
				if ((index > -1) && (keys.Contains(name1) == false)) {
					keys.Add(name1);
				}
			}

			// Return the names.
			return keys.ToArray();
		} // GetResourceKeys

		/// <summary>
		/// Gets the embedded resource string from the assemblies, identified by the key.
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
				this.LogInternal("Resource: Reading resource '{0}' as text.", key);

				// Find the resource with the name matching the key in the calling assembly.
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

				// Find the resource with the name matching the key in the executing assembly.
				foreach (String name in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
					Int32 index = name.ToLower().IndexOf(".resources.");
					if ((index > -1) && (name.ToLower().Substring(index + 11).Equals(key.ToLower()) == true)) {
						using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(name)) {
							using (TextReader text = new StreamReader(stream, Encoding.Default)) {
								return text.ReadToEnd();
							}
						}
					}
				}

				// Find the resource with the name matching the key in the entry assembly.
				foreach (String name in Assembly.GetEntryAssembly().GetManifestResourceNames()) {
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
				this.LogInternal("Resource: Resource not found.");

				// Return the default value.
				return defaultValue;
			} catch (Exception exception) {
				this.LogError(exception);

				// Return the default value.
				return defaultValue;
			}
		} // GetResourceStr

		/// <summary>
		/// Gets the embedded resource image from the assemblies, identified by the key.
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
				this.LogInternal("Resource: Reading resource '{0}' as image.", key);

				// Find the resource with the name matching the key in the calling assembly.
				foreach (String name in Assembly.GetCallingAssembly().GetManifestResourceNames()) {
					Int32 index = name.ToLower().IndexOf(".resources.");
					if ((index > -1) && (name.ToLower().Substring(index + 11).Equals(key.ToLower()) == true)) {
						return new Bitmap(Assembly.GetCallingAssembly().GetManifestResourceStream(name));
					}
				}

				// Find the resource with the name matching the key in the executing assembly.
				foreach (String name in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
					Int32 index = name.ToLower().IndexOf(".resources.");
					if ((index > -1) && (name.ToLower().Substring(index + 11).Equals(key.ToLower()) == true)) {
						return new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(name));
					}
				}

				// Find the resource with the name matching the key in the entry assembly.
				foreach (String name in Assembly.GetEntryAssembly().GetManifestResourceNames()) {
					Int32 index = name.ToLower().IndexOf(".resources.");
					if ((index > -1) && (name.ToLower().Substring(index + 11).Equals(key.ToLower()) == true)) {
						return new Bitmap(Assembly.GetEntryAssembly().GetManifestResourceStream(name));
					}
				}

				// Log.
				this.LogInternal("Resource: Resource not found.");

				// Return the default value.
				return defaultValue;
			} catch (Exception exception) {
				this.LogError(exception);

				// Return the default value.
				return defaultValue;
			}
		} // GetResourceImage
		#endregion

		#region Private assembly resolver methods.
		/// <summary>
		/// Gets the embedded resource assembly from the assemblies, identified by the key.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="args">The event arguments.</param>
		/// <returns>The loaded assembly or null.</returns>
		private Assembly ResourceAssemblyResolver(Object sender, ResolveEventArgs args) {
			try {
				// Get the assembly name.
				// Example: "AcctPublicRestCommunicationLibrary, Version=1.0.6263.38058, Culture=neutral, PublicKeyToken=null".
				String key = args.Name;
				if (args.Name.Contains(",") == true) {
					key = args.Name.Substring(0, args.Name.IndexOf(","));
				}
				key = Path.ChangeExtension(key, ".dll");

				// Log.
				this.LogInternal("Resource: Reading assembly resource '{0}'.", key);

				// Find the resource with the name matching the key in the calling assembly.
				foreach (String name in Assembly.GetCallingAssembly().GetManifestResourceNames()) {
					Int32 index = name.ToLower().IndexOf(".resources.");
					if ((index > -1) && (name.ToLower().Substring(index + 11).Equals(key.ToLower()) == true)) {
						Stream resurceStream = Assembly.GetCallingAssembly().GetManifestResourceStream(name);
						using (MemoryStream memoryStream = new MemoryStream()) {
							resurceStream.CopyTo(memoryStream);
							return Assembly.Load(memoryStream.ToArray());
						}
					}
				}

				// Find the resource with the name matching the key in the executing assembly.
				foreach (String name in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
					Int32 index = name.ToLower().IndexOf(".resources.");
					if ((index > -1) && (name.ToLower().Substring(index + 11).Equals(key.ToLower()) == true)) {
						Stream resurceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
						using (MemoryStream memoryStream = new MemoryStream()) {
							resurceStream.CopyTo(memoryStream);
							return Assembly.Load(memoryStream.ToArray());
						}
					}
				}

				// Find the resource with the name matching the key in the entry assembly.
				foreach (String name in Assembly.GetEntryAssembly().GetManifestResourceNames()) {
					Int32 index = name.ToLower().IndexOf(".resources.");
					if ((index > -1) && (name.ToLower().Substring(index + 11).Equals(key.ToLower()) == true)) {
						Stream resurceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(name);
						using (MemoryStream memoryStream = new MemoryStream()) {
							resurceStream.CopyTo(memoryStream);
							return Assembly.Load(memoryStream.ToArray());
						}
					}
				}

				// Find the resource with the name matching the key in all the assemblies.
				// This is last, because I want to try the calling, executing and entry assemblies first.
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
					foreach (String name in assembly.GetManifestResourceNames()) {
						Int32 index = name.ToLower().IndexOf(".resources.");
						if ((index > -1) && (name.ToLower().Substring(index + 11).Equals(key.ToLower()) == true)) {
							Stream resurceStream = assembly.GetManifestResourceStream(name);
							using (MemoryStream memoryStream = new MemoryStream()) {
								resurceStream.CopyTo(memoryStream);
								return Assembly.Load(memoryStream.ToArray());
							}
						}
					}
				}

				// Log.
				this.LogInternal("Resource: Assembly resource not found.");

				// Return null.
				return null;
			} catch (Exception exception) {
				// Log error.
				this.LogError(exception);

				// Return null.
				return null;
			}
		} // ResourceAssemblyResolver
		#endregion

	} // Framework
	#endregion

} // NDK.Framework