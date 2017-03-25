using System;
using System.Collections.Generic;
using System.Data;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines command line argument parsing.
	/// </summary>
	public partial interface IFramework {

		#region Argument methods.
		/// <summary>
		/// Gets the arguments passed to the executing process.
		/// </summary>
		/// <returns></returns>
		String[] GetArguments();
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework