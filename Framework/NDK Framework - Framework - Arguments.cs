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
	/// This partial part of the class, implements command line argument parsing.
	/// </summary>
	public abstract partial class Framework : IFramework {
		private static String[] argumentList = null;

		#region Private argument initialization
		private void ArgumentsInitialize() {
			if (Framework.argumentList == null) {
				Framework.argumentList = Environment.GetCommandLineArgs();
			}
		} // ArgumentsInitialize
		#endregion

		#region Public arguments methods.
		/// <summary>
		/// Gets the arguments passed to the executing process.
		/// </summary>
		/// <returns></returns>
		public String[] GetArguments() {
			return Framework.argumentList;
		} // GetArguments
		#endregion

	} // Framework
	#endregion

} // NDK.Framework