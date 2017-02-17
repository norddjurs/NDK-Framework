using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace NDK.Framework {

	#region IDateReader extension class.
	public static class IDateReaderExtensions {

		/// <summary>
		/// Gets the boolean value of the field identified by the name.
		/// </summary>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="name">The field name.</param>
		/// <param name="defaultValue">The default value (false).</param>
		/// <returns></returns>
		public static Boolean GetBoolean(this IDataReader dataReader, String name, Boolean defaultValue = false) {
			try {
				return dataReader.GetBoolean(dataReader.GetOrdinal(name));
			} catch {
				return defaultValue;
			}
		} // GetBoolean

		/// <summary>
		/// Gets the integer value of the field identified by the name.
		/// </summary>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="name">The field name.</param>
		/// <param name="defaultValue">The default value (0).</param>
		/// <returns></returns>
		public static Int16 GetInt16(this IDataReader dataReader, String name, Int16 defaultValue = 0) {
			try {
				return dataReader.GetInt16(dataReader.GetOrdinal(name));
			} catch {
				return defaultValue;
			}
		} // GetInt16

		/// <summary>
		/// Gets the integer value of the field identified by the name.
		/// </summary>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="name">The field name.</param>
		/// <param name="defaultValue">The default value (0).</param>
		/// <returns></returns>
		public static Int32 GetInt32(this IDataReader dataReader, String name, Int32 defaultValue = 0) {
			try {
				return dataReader.GetInt32(dataReader.GetOrdinal(name));
			} catch {
				return defaultValue;
			}
		} // GetInt32

		/// <summary>
		/// Gets the integer value of the field identified by the name.
		/// </summary>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="name">The field name.</param>
		/// <param name="defaultValue">The default value (0).</param>
		/// <returns></returns>
		public static Int64 GetInt64(this IDataReader dataReader, String name, Int64 defaultValue = 0) {
			try {
				return dataReader.GetInt64(dataReader.GetOrdinal(name));
			} catch {
				return defaultValue;
			}
		} // GetInt64

		/// <summary>
		/// Gets the single value of the field identified by the name.
		/// </summary>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="name">The field name.</param>
		/// <param name="defaultValue">The default value (0.0F).</param>
		/// <returns></returns>
		public static Single GetFloat(this IDataReader dataReader, String name, Single defaultValue = 0.0F) {
			try {
				return dataReader.GetFloat(dataReader.GetOrdinal(name));
			} catch {
				return defaultValue;
			}
		} // GetSingle

		/// <summary>
		/// Gets the double value of the field identified by the name.
		/// </summary>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="name">The field name.</param>
		/// <param name="defaultValue">The default value (0.0F).</param>
		/// <returns></returns>
		public static Double GetDouble(this IDataReader dataReader, String name, Double defaultValue = 0.0F) {
			try {
				return dataReader.GetDouble(dataReader.GetOrdinal(name));
			} catch {
				return defaultValue;
			}
		} // GetDouble

		/// <summary>
		/// Gets the decimal value of the field identified by the name.
		/// </summary>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="name">The field name.</param>
		/// <param name="defaultValue">The default value (0.0M).</param>
		/// <returns></returns>
		public static Decimal GetDecimal(this IDataReader dataReader, String name, Decimal defaultValue = 0.0M) {
			try {
				return dataReader.GetDecimal(dataReader.GetOrdinal(name));
			} catch {
				return defaultValue;
			}
		} // GetDecimal

		/// <summary>
		/// Gets the datetime value of the field identified by the name.
		/// </summary>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="name">The field name.</param>
		/// <param name="defaultValue">The default value (new DateTime()).</param>
		/// <returns></returns>
		public static DateTime GetDateTime(this IDataReader dataReader, String name, DateTime defaultValue = new DateTime()) {
			try {
				return dataReader.GetDateTime(dataReader.GetOrdinal(name));
			} catch {
				return defaultValue;
			}
		} // GetDateTime

		/// <summary>
		/// Gets the guid value of the field identified by the name.
		/// </summary>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="name">The field name.</param>
		/// <param name="defaultValue">The default value (new Guid()).</param>
		/// <returns></returns>
		public static Guid GetGuid(this IDataReader dataReader, String name, Guid defaultValue = new Guid()) {
			try {
				return dataReader.GetGuid(dataReader.GetOrdinal(name));
			} catch {
				return defaultValue;
			}
		} // GetGuid

		/// <summary>
		/// Gets the string value of the field identified by the name.
		/// </summary>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="name">The field name.</param>
		/// <param name="defaultValue">The default value (null).</param>
		/// <returns></returns>
		public static String GetString(this IDataReader dataReader, String name, String defaultValue = null) {
			try {
				return dataReader.GetString(dataReader.GetOrdinal(name));
			} catch {
				return defaultValue;
			}
		} // GetString

	} // IDateReaderExtensions
	#endregion

} // NDK.Framework