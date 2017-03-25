using System;
using System.Collections.Generic;
using System.Data;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines option storage.
	/// </summary>
	public partial interface IFramework {

		#region Option methods.
		/// <summary>
		/// Gets the user option keys associated with the framework class guid.
		/// </summary>
		/// <returns>All the keys associated with the framework class guid.</returns>
		String[] GetOptionKeys();

		/// <summary>
		/// Gets the user option value associated with the framework class guid and key.
		/// If more then one value is associated with the framework class guid and key, the first value is returned.
		/// If no value is associated with the framework class guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Boolean. The default value is returned on parse errors.
		/// True values are "true", "yes" and "1" in any case.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Boolean GetOptionValue(String key, Boolean defaultValue);

		/// <summary>
		/// Gets the user option value associated with the framework class guid and key.
		/// If more then one value is associated with the framework class guid and key, the first value is returned.
		/// If no value is associated with the framework class guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Int32. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Int32 GetOptionValue(String key, Int32 defaultValue);

		/// <summary>
		/// Gets the user option value associated with the framework class guid and key.
		/// If more then one value is associated with the framework class guid and key, the first value is returned.
		/// If no value is associated with the framework class guid and key, the default value is returned.
		/// 
		/// The value is parsed as a DateTime. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		DateTime GetOptionValue(String key, DateTime defaultValue);

		/// <summary>
		/// Gets the user option value associated with the framework class guid and key.
		/// If more then one value is associated with the framework class guid and key, the first value is returned.
		/// If no value is associated with the framework class guid and key, the default value is returned.
		/// 
		/// The value is parsed as a Guid. The default value is returned on parse errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		Guid GetOptionValue(String key, Guid defaultValue);

		/// <summary>
		/// Gets the user option value associated with the framework class guid and the key.
		/// If more then one value is associated with the framework class guid and the key, the first value is returned.
		/// If no value is associated with the framework class guid and the key, the default value is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The optional default value.</param>
		/// <returns>The value.</returns>
		String GetOptionValue(String key, String defaultValue = null);

		/// <summary>
		/// Gets the user option values associated with the framework class guid and the key.
		/// If no value is associated with the framework class guid and the key, an empty list is returned.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value list.</returns>
		List<String> GetOptionValues(String key);

		/// <summary>
		/// Sets the user option value associated with the framework class guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetOptionValue(String key, Boolean value);

		/// <summary>
		/// Sets the user option value associated with the framework class guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetOptionValue(String key, Int32 value);

		/// <summary>
		/// Sets the user option value associated with the framework class guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetOptionValue(String key, DateTime value);

		/// <summary>
		/// Sets the user option value associated with the framework class guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value</param>
		void SetOptionValue(String key, Guid value);

		/// <summary>
		/// Sets the user option values associated with the framework class guid and the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values</param>
		void SetOptionValues(String key, params String[] values);
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework