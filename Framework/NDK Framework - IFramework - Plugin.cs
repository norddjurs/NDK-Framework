using System;
using System.Collections.Generic;
using System.Data;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines plugin loading.
	/// </summary>
	public partial interface IFramework {

		#region Plugin methods.
		/// <summary>
		/// Gets the plugins loaded.
		/// The plugins are objects implementing the IPlugin interface.
		/// The assemblies (DLL and EXE) in the same directory as the "NDK Framework.dll" assembly, are scanned.
		/// </summary>
		/// <param name="reload">Reload new instances of the plugins (false).</param>
		/// <returns>The loaded plugins.</returns>
		IPlugin[] GetPlugins(Boolean reload = false);
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework