using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace NDK.Framework {

	#region Framework
	/// <summary>
	/// This partial part of the class, implements plugin loading.
	/// </summary>
	public abstract partial class Framework : IFramework {
		private static PluginList<IPlugin> pluginList = null;

		#region Private plugin initialization
		private void PluginInitialize() {
		} // PluginInitialize
		#endregion

		#region Public plugin methods.
		/// <summary>
		/// Gets the plugins loaded.
		/// The plugins are objects implementing the IPlugin interface.
		/// The assemblies (DLL and EXE) in the same directory as the "NDK Framework.dll" assembly, are scanned.
		/// </summary>
		/// <param name="reload">Reload new instances of the plugins.</param>
		/// <returns>The loaded plugins.</returns>
		public IPlugin[] GetPlugins(Boolean reload = false) {
			try {
				// Reload by clearing the cache.
				if ((Framework.pluginList != null) && (reload == true)) {
					Framework.pluginList.Clear();
					Framework.pluginList = null;
				}

				if (Framework.pluginList == null) {
					this.LogInternal("Plugins: Loading plugins.");

					// Load plugins.
					Framework.pluginList = new PluginList<IPlugin>();

					this.LogInternal("Plugins: {0} plugin(s) found.", Framework.pluginList.Count);
				}

				// Return plugin list.
				return Framework.pluginList.ToArray();
			} catch (Exception exception) {
				// Log.
				this.LogError(exception);

				// Return empty list.
				return new IPlugin[0];
			}
		} // GetPlugins
		#endregion

	} // Framework
	#endregion

} // NDK.Framework