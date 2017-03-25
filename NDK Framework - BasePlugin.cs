using System;
using System.Collections.Generic;

namespace NDK.Framework {

	#region BasePlugin class.
	/// <summary>
	/// Inherit this abstract class and implement abstract methods to create a NDK Framework plugin that
	/// provides all the functionality from the framework.
	/// 
	/// It can be executed as a service or from the commandline by the "NDK Run.exe" application.
	/// </summary>
	public abstract class BasePlugin : Framework, IFramework, IPlugin {

		#region Abstract and Virtual methods.
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
		#endregion

	} // BasePlugin
	#endregion

} // NDK.Framework