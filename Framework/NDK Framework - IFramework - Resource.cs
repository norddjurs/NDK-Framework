using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines resource access.
	/// </summary>
	public partial interface IFramework {

		#region Resource methods.
		/// <summary>
		/// Gets the resource keys, to the resources embedded in the assemblies.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <returns>The resource keys.</returns>
		String[] GetResourceKeys();

		/// <summary>
		/// Gets the embedded resource string from the assemblies, identified by the key.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <param name="key">The resource key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The resource data.</returns>
		String GetResourceStr(String key, String defaultValue = null);

		/// <summary>
		/// Gets the embedded resource image from the assemblies, identified by the key.
		/// 
		/// The resources must reside inside the "Resources" directory in the VS project.
		/// The resource keys are probably the filenames of the embedded resources, unless some fiddeling with the ".csproj" file has been done.
		/// </summary>
		/// <param name="key">The resource key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The resource data.</returns>
		Image GetResourceImage(String key, Image defaultValue = null);
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework