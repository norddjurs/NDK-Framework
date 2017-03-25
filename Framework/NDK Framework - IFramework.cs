using System;
using System.Collections.Generic;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// All NDK Framework classes, must implement this interface.
	/// 
	/// I have decided to place all NDK Framework functionality in one class, instead of multiple classes.
	/// </summary>
	public partial interface IFramework {

		#region Properties.
		/// <summary>
		/// Gets or sets the tagged object.
		/// </summary>
		Object Tag { get; set; }
		#endregion

		#region Abstract and Virtual methods.
		/// <summary>
		/// Gets the unique guid used when referencing resources.
		/// When implementing a class, this method should return the same unique guid every time. 
		/// </summary>
		/// <returns></returns>
		Guid GetGuid();

		/// <summary>
		/// Gets the name.
		/// When implementing a class, this method should return a proper display name.
		/// </summary>
		/// <returns></returns>
		String GetName();
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework