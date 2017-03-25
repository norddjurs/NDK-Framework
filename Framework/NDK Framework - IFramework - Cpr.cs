using System;
using NDK.Framework.CprBroker;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines CPR lookup.
	/// </summary>
	public partial interface IFramework {

		#region CPR methods.
		CprSearchResult CprSearch(String cprNumber);
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework