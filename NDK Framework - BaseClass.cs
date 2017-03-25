using System;
using System.Collections.Generic;

namespace NDK.Framework {

	#region BaseClass
	/// <summary>
	/// This class is used to create a NDK Framework class.
	/// 
	/// Inherit this class and implement abstract methods to create a NDK Framework class, that
	/// provides all the functionality from the framework.
	/// </summary>
	public class BaseClass : Framework, IFramework {
		private Guid guid;
		private String name;

		#region Constructors.
		/// <summary>
		/// Inherit this class and implement abstract methods to create a NDK Framework class, that
		/// provides all the functionality from the framework.
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="name"></param>
		public BaseClass(Guid guid, String name) {
			this.guid = guid;
			this.name = name;
		} // BaseClass
		#endregion

		#region Implementing IFramework methods.
		/// <summary>
		/// Gets the unique framework class guid used when referencing resources.
		/// </summary>
		/// <returns></returns>
		public override Guid GetGuid() {
			return this.guid;
		} // GetGuid

		/// <summary>
		/// Gets the the framework class display name.
		/// </summary>
		/// <returns></returns>
		public override String GetName() {
			return this.name;
		} // GetName
		#endregion

	} // BaseClass
	#endregion

} // NDK.Framework