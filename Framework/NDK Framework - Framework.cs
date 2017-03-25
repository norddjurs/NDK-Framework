using System;
using System.Collections.Generic;

namespace NDK.Framework {

	#region Framework
	/// <summary>
	/// This class contains all NDK Framework functionality.
	/// 
	/// I have decided to place all NDK Framework functionality in one class, instead of multiple classes.
	/// Each functionality part, is places in its own partial class file.
	/// </summary>
	public abstract partial class Framework : IFramework {
		private static List<Framework> frameworkClasses = new List<Framework>();
		private static Boolean frameworkFirstInitialization = true;

		public Framework() {
			// Initialize.
			this.ConfigInitialize();
			this.LogInitialize();
			this.OptionInitialize();
			this.ArgumentsInitialize();
			this.ResourceInitialize();
			this.PluginInitialize();
			this.EventInitialize();
			this.MailInitialize();
			this.DatabaseInitialize();
			this.ActiveDirectoryInitialize();
			this.SofdDirectoryInitialize();
			this.CprInitialize();

			// Register this framework class.
			Framework.frameworkClasses.Add(this);

			// Set flag that the first framework class instanse has been initialized.
			Framework.frameworkFirstInitialization = false;
		} // Framework

		#region Properties.
		/// <summary>
		/// Gets or sets the tagged object.
		/// </summary>
		public Object Tag { get; set; }
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
		#endregion

	} // Framework
	#endregion

} // NDK.Framework