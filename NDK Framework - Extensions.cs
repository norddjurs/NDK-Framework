using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace NDK.Framework {

	#region String extension class.
	public static class StringExtensions {

		/// <summary>
		/// Gets the null value, if the string value is NULL.
		/// </summary>
		/// <param name="value">The string.</param>
		/// <param name="nullValue">The returned value when the string is null.</param>
		/// <returns></returns>
		public static String GetNotNull(this String value, String nullValue = "") {
			if (value == null) {
				return nullValue;
			} else {
				return value;
			}
		} // GetNotNull

		/// <summary>
		/// Gets true if the argumented value is either null, empty or consists entirely of white-spaces.
		/// </summary>
		/// <param name="value">The string.</param>
		/// <returns></returns>
		public static Boolean IsNullOrWhiteSpace(this String value) {
			return String.IsNullOrWhiteSpace(value);
		} // IsNullOrWhiteSpace

	} // StringExtensions
	#endregion

	#region DateTime extension class.
	public static class DateTimeExtensions {

		/// <summary>
		/// Gets true if the argumented value is either DateTime.MinValue or DateTime.MaxValue.
		/// </summary>
		/// <param name="value">The date time.</param>
		/// <returns></returns>
		public static Boolean IsMinOrMax(this DateTime value) {
			return ((value.Equals(DateTime.MinValue) == true) || (value.Equals(DateTime.MaxValue) == true));
		} // IsMinOrMax

	} // DateTimeExtensions
	#endregion

	#region IDataReader extension class.
	public static class IDataReaderExtensions {

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

	} // IDataReaderExtensions
	#endregion

	#region Format extension class.
	public static class FormatExtensions {

		/// <summary>
		/// Format a string containing a CPR number to standard string "XXXXXX-XXXX".
		/// </summary>
		/// <param name="value">The string to format.</param>
		/// <returns>The formatted string.</returns>
		public static String FormatStringCpr(this String value) {
			try {
				value				= value.Trim().Replace(" ", "").Replace("-", "");
				return String.Format("{0:000000-0000}", Int64.Parse(value.Substring(0, 10)));
			} catch {
				return String.Empty;
			}
		} // FormatStringCpr

		/// <summary>
		/// Format a string containing a phone number to standard string "XX XX XX XX".
		/// </summary>
		/// <param name="value">The string to format.</param>
		/// <returns>The formatted string.</returns>
		public static String FormatStringPhone(this String value) {
			try {
				if (value.IsNullOrWhiteSpace() == false) {
					// Trim the string.
					value	= value.Replace(" ", "");
					if (value.Length <= 8) {
						return String.Format("{0:## ## ## ##}", Int64.Parse(value.Substring(0, 8)));
					} else {
						return String.Format("{0:## ## ## ##} {1}", Int64.Parse(value.Substring(0, 8)), value.Substring(8));
					}
				} else {
					return String.Empty;
				}
			} catch {
				return String.Empty;
			}
		} // FormatStringPhone

		/// <summary>
		/// Format a date to standard string "dd.MM.yyyy".
		/// </summary>
		/// <param name="value">The DateTime to format.</param>
		/// <returns>The formatted string.</returns>
		public static String FormatStringDate(this DateTime value) {
			try {
				if ((value.IsMinOrMax() == false) &&
					(value.Date.Year > 1900)) {
					return value.ToString("dd.MM.yyyy");
				} else {
					return String.Empty;
				}
			} catch {
				return String.Empty;
			}
		} // FormatStringDate

		/// <summary>
		/// Format a date/time to standard string "dd.MM.yyyy  HH.mm".
		/// </summary>
		/// <param name="value">The DateTime to format.</param>
		/// <returns>The formatted string.</returns>
		public static String FormatStringDateTime(this DateTime value) {
			try {
				if ((value.IsMinOrMax() == false) &&
					(value.Date.Year > 1900)) {
					return value.ToString("dd.MM.yyyy  HH.mm");
				} else {
					return String.Empty;
				}
			} catch {
				return String.Empty;
			}
		} // FormatStringDateTime

	} // FormatExtensions
	#endregion

} // NDK.Framework