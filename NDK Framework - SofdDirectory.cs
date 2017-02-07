using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace NDK.Framework {

	#region EtlMedarbejder class.
	public class EtlMedarbejder {
	} // EtlMedarbejder
	#endregion

	#region EtlOrganisation class.
	public class EtlOrganisation {
	} // EtlOrganisation
	#endregion

	#region SofdDirectory class.
	public class SofdDirectory {
		private IConfiguration config = null;

		/// <summary>
		/// Connect to the SOFD database.
		/// </summary>
		public SofdDirectory(IConfiguration config) {
			this.config = config;

			// Connect to the SOFD database.
			
		} // SofdDirectory

	} // SofdDirectory
	#endregion

} // NDK.Framework