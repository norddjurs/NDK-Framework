using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace NDK.Framework {

	#region Database class.
	public class Database {
		private IFramework framework = null;

		#region Constructor methods.
		/// <summary>
		/// Database manager.
		/// </summary>
		public Database(IFramework framework) {
			this.framework = framework;
		} // Sql
		#endregion

		#region Methods.
		/// <summary>
		/// Connects to the database identified with the key in the configuration.
		/// </summary>
		/// <param name="key">The database connection identifier.</param>
		/// <returns>The database connection or null.</returns>
		public IDbConnection GetDatabaseConnection(String key) {
			try {
				// Get configuration.
				Int32 dbTimeout = 30;
				String dbEngine = this.framework.Config.GetSystemValue("SqlEngine" + key, "0");
				String dbHost = this.framework.Config.GetSystemValue("SqlHost" + key, "localhost");
				String dbName = this.framework.Config.GetSystemValue("SqlDatabase" + key);
				String dbUserName = this.framework.Config.GetSystemValue("SqlUserid" + key);
				String dbUserPassword = this.framework.Config.GetSystemValue("SqlPassword" + key);

				// Log.
				this.framework.Logger.Log("SQL: Connecting to database '{2}' at '{1}' as '{3}'", dbEngine, dbHost, dbName, ((dbUserName != null) ? dbUserName : "SSPI (" + Environment.UserName + ")"));

				// Connect to the database.
				IDbConnection dbConnection = null;
				switch (dbEngine.ToLower()) {
					case "0":
					case "mssql":
					default:
						// Create the connection.
						dbConnection = new SqlConnection();
						if ((dbUserName != null) && (dbUserName != String.Empty) && (dbUserPassword != null) && (dbUserPassword != String.Empty)) {
							// SQL server authentication.
							dbConnection.ConnectionString = String.Format("data source = {0}; database = {1}; user id = {2}; password = {3}; Connect Timeout = {4}; Pooling = False;", dbHost, dbName, dbUserName, dbUserPassword, dbTimeout);
						} else {
							// Windows authentication.
							dbConnection.ConnectionString = String.Format("data source = {0}; database = {1}; integrated security = SSPI; Connect Timeout = {4}; Pooling = False;", dbHost, dbName, dbUserName, dbUserPassword, dbTimeout);
						}

						// Open the connection.
						dbConnection.Open();

						// Enable quoted identifiers.
						// Execute the SQL command.
						if (dbConnection.State == ConnectionState.Open) {
							using (IDataReader dataReader = this.ExecuteSql(dbConnection, "SET QUOTED_IDENTIFIER ON;")) {
							}
						}
						break;
				}

				// Return the database connection.
				return dbConnection;
			} catch (Exception exception) {
				// Log error.
				this.framework.Logger.LogError(exception);

				// Return null;
				return null;
			}
		} // GetDatabaseConnection

		/// <summary>
		/// Executes the sql.
		/// The connection must be open.
		/// The sql must use Quoted Identifiers.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="sql">The sql.</param>
		/// <param name="formatArgs">The optional sql string format arguments.</param>
		/// <returns>The data reader result, or null.</returns>
		public IDataReader ExecuteSql(IDbConnection connection, String sql, params Object[] formatArgs) {
			try {
				// Format the sql text.
				if (formatArgs.Length > 0) {
					try {
						sql = String.Format(sql, formatArgs);
					} catch { }
				}

				// Create the command object.
				IDbCommand command = connection.CreateCommand();
				command.CommandText = sql;
				//command.Transaction = null;
				//command.CommandTimeout = 30 * 1000;

				// Log.
				this.framework.Logger.LogDebug("SQL: Execute sql on '{0}':", connection.Database);
				this.framework.Logger.LogDebug(command.CommandText);

				// Execute the SQL command.
				IDataReader dataReader = command.ExecuteReader();

				// Log.
				if (dataReader.RecordsAffected < 0) {
					this.framework.Logger.LogDebug("SQL: {0} fields in each record.", dataReader.FieldCount);
				} else {
					this.framework.Logger.LogDebug("SQL: {0} rows affected.", dataReader.RecordsAffected);
				}

				// Return the data reader.
				return dataReader;
			} catch (Exception exception) {
				// Log error.
				this.framework.Logger.LogError(exception);

				// Return null;
				return null;
			}
		} // ExecuteSql

		/// <summary>
		/// Executes a query on the schema and table, filtering the result using the WHERE filters.
		/// The connection must be open.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="schemaName">The schema name.</param>
		/// <param name="tableName">The table name.</param>
		/// <param name="whereFilters">The optional WHERE filters.</param>
		/// <returns>The data reader result, or null.</returns>
		public IDataReader ExecuteSql(IDbConnection connection, String schemaName, String tableName, params SqlWhereFilterBase[] whereFilters) {
			// Build sql.
			String sql = String.Format("SELECT * FROM \"{0}\".\"{1}\"", schemaName, tableName);

			// Append WHERE filters.
			if (whereFilters.Length > 0) {
				Boolean firstFilter = true;
				sql += " WHERE ";
				foreach (SqlWhereFilterBase whereFilter in whereFilters) {
					if (firstFilter == false) {
						firstFilter = (whereFilter.GetType() == typeof(SqlWhereFilterEndGroup));
					}
					sql += whereFilter.ToString(firstFilter);
					firstFilter = (whereFilter.GetType() == typeof(SqlWhereFilterBeginGroup));
				}
			}

			// Execute the sql.
			return this.ExecuteSql(connection, sql);
		} // ExecuteSql
		#endregion

	} // Database
	#endregion
	
	#region SqlWhereFilterOperator enum.
	/// <summary>
	/// Operator for the sql WHERE filter, generated by a SqlWhereFilterBase subclass.
	/// </summary>
	public enum SqlWhereFilterOperator {
		/// <summary>
		/// AND.
		/// </summary>
		AND,

		/// <summary>
		/// OR.
		/// </summary>
		OR
	} // SqlWhereFilterOperator
	#endregion

	#region SqlWhereFilterValueOperator enum
	/// <summary>
	/// Operator for the sql WHERE filter values, generated by a SqlWhereFilterBase subclass.
	/// </summary>
	public enum SqlWhereFilterValueOperator {
		/// <summary>
		/// The search results include values that equal the supplied value.
		/// </summary>
		Equals,

		/// <summary>
		/// The search results include values that are greater than the supplied value.
		/// </summary>
		GreaterThan,

		/// <summary>
		/// The search results include values that are greater than or equal to the supplied value.
		/// </summary>
		GreaterThanOrEquals,

		/// <summary>
		/// The search results include values that are less than the supplied value.
		/// </summary>
		LessThan,

		/// <summary>
		/// The search results include values that are less than or equal to the supplied value.
		/// </summary>
		LessThanOrEquals,

		/// <summary>
		/// The search results include values that are not equal to the supplied value.
		/// </summary>
		NotEquals,

		/// <summary>
		/// The search results includes values that matches the supplied value.
		/// Asterisk characters "*" are interpreted as wildcards.
		/// </summary>
		Like
	} // SqlWhereFilterValueOperator
	#endregion

	#region SqlWhereFilterBase class.
	/// <summary>
	/// Inherit this class to build sql WHERE filters like "AND ("name" = "jensen")".
	/// </summary>
	public abstract class SqlWhereFilterBase {
		private SqlWhereFilterOperator filterOperator;
		private SqlWhereFilterValueOperator filterValueOperator;
		private String filterName;

		/// <summary>
		/// Inherit this class to build sql WHERE filter parts like "AND ("name" = "jensen")".
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterName">The filter name.</param>
		public SqlWhereFilterBase(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterName) {
			this.filterOperator = filterOperator;
			this.filterValueOperator = filterValueOperator;
			this.filterName = filterName;
		} // SqlWhereFilterBase

		/// <summary>
		/// Generate the sql WHERE filter.
		/// </summary>
		/// <returns>The sql WHERE filter.</returns>
		public override String ToString() {
			return this.ToString(false);
		} // ToString

		/// <summary>
		/// Generate the sql WHERE filter.
		/// </summary>
		/// <param name="isFirstFilter">True if this is the first sql WHERE filter in the group or after the WHARE keywork.</param>
		/// <returns>The sql WHERE filter.</returns>
		public String ToString(Boolean isFirstFilter) {
			String filterSql = String.Empty;

			// Add sql WHERE filter operator.
			if (isFirstFilter == false) {
				filterSql += String.Format(" {0} ", this.filterOperator.ToString());
			}

			// Add sql WHERE filter.
			filterSql += this.GetFilterString(this.filterName, this.filterValueOperator);

			// Return the sql WHERE filter.
			return filterSql;
		} // ToString

		/// <summary>
		/// Implement this method, to return the part of the sql WHERE filter that compares the field to the value.
		/// </summary>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <returns>The part of the sql WHERE filter that compares the field to the value.</returns>
		protected abstract String GetFilterString(String filterName, SqlWhereFilterValueOperator filterValueOperator);

	} // SqlWhereFilterBase
	#endregion

	#region SqlWhereFilterBeginGroup class.
	/// <summary>
	/// Use this class to build sql WHERE filter parts that begin a group "(".
	/// </summary>
	public class SqlWhereFilterBeginGroup : SqlWhereFilterBase {

		/// <summary>
		/// Use this class to build sql WHERE filter parts that begin a group "(".
		/// </summary>
		public SqlWhereFilterBeginGroup() : base(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, String.Empty) {
		} // SqlWhereFilterBeginGroup

		/// <summary>
		/// Implement this method, to return the part of the sql WHERE filter that compares the field to the value.
		/// </summary>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <returns>The part of the sql WHERE filter that compares the field to the value.</returns>
		protected override String GetFilterString(String filterName, SqlWhereFilterValueOperator filterValueOperator) {
			return " ( ";
		} // GetFilterString

	} // SqlWhereFilterBeginGroup
	#endregion

	#region SqlWhereFilterEndGroup class.
	/// <summary>
	/// Use this class to build sql WHERE filter parts that end a group ")".
	/// </summary>
	public class SqlWhereFilterEndGroup : SqlWhereFilterBase {

		/// <summary>
		/// Use this class to build sql WHERE filter parts that end a group "(".
		/// </summary>
		public SqlWhereFilterEndGroup() : base(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, String.Empty) {
		} // SqlWhereFilterEndGroup

		/// <summary>
		/// Implement this method, to return the part of the sql WHERE filter that compares the field to the value.
		/// </summary>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <returns>The part of the sql WHERE filter that compares the field to the value.</returns>
		protected override String GetFilterString(String filterName, SqlWhereFilterValueOperator filterValueOperator) {
			return " ) ";
		} // GetFilterString

	} // SqlWhereFilterEndGroup
	#endregion

	#region SqlWhereFilterInt32 class.
	/// <summary>
	/// Use this class to build sql WHERE filter parts for Int32 fields.
	/// </summary>
	public class SqlWhereFilterInt32 : SqlWhereFilterBase {
		private Int32 filterValue;

		/// <summary>
		/// Use this class to build sql WHERE filter parts for Int32 fields.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		public SqlWhereFilterInt32(SqlWhereFilterOperator filterOperator, String filterName, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, filterValueOperator, filterName) {
			this.filterValue = filterValue;
		} // SqlWhereFilterInt32

		/// <summary>
		/// Implement this method, to return the part of the sql WHERE filter that compares the field to the value.
		/// </summary>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <returns>The part of the sql WHERE filter that compares the field to the value.</returns>
		protected override String GetFilterString(String filterName, SqlWhereFilterValueOperator filterValueOperator) {
			switch (filterValueOperator) {
				case SqlWhereFilterValueOperator.Equals:
					return String.Format(" \"{0}\" = {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.GreaterThan:
					return String.Format(" \"{0}\" > {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.GreaterThanOrEquals:
					return String.Format(" \"{0}\" >= {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.LessThan:
					return String.Format(" \"{0}\" < {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.LessThanOrEquals:
					return String.Format(" \"{0}\" <= {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.NotEquals:
					return String.Format(" \"{0}\" <> {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.Like:
				default:
					throw new NotSupportedException(String.Format("The sql WHERE filter class '{0}' does not support the '{1}' value operator.", this.GetType().Name, filterValueOperator));
					break;
			}
		} // GetFilterString

	} // SqlWhereFilterInt32
	#endregion

	#region SqlWhereFilterDecimal class.
	/// <summary>
	/// Use this class to build sql WHERE filter parts for Decimal fields.
	/// </summary>
	public class SqlWhereFilterDecimal : SqlWhereFilterBase {
		private Decimal filterValue;

		/// <summary>
		/// Use this class to build sql WHERE filter parts for Decimal fields.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		public SqlWhereFilterDecimal(SqlWhereFilterOperator filterOperator, String filterName, SqlWhereFilterValueOperator filterValueOperator, Decimal filterValue) : base(filterOperator, filterValueOperator, filterName) {
			this.filterValue = filterValue;
		} // SqlWhereFilterDecimal

		/// <summary>
		/// Implement this method, to return the part of the sql WHERE filter that compares the field to the value.
		/// </summary>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <returns>The part of the sql WHERE filter that compares the field to the value.</returns>
		protected override String GetFilterString(String filterName, SqlWhereFilterValueOperator filterValueOperator) {
			switch (filterValueOperator) {
				case SqlWhereFilterValueOperator.Equals:
					return String.Format(" \"{0}\" = {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.GreaterThan:
					return String.Format(" \"{0}\" > {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.GreaterThanOrEquals:
					return String.Format(" \"{0}\" >= {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.LessThan:
					return String.Format(" \"{0}\" < {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.LessThanOrEquals:
					return String.Format(" \"{0}\" <= {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.NotEquals:
					return String.Format(" \"{0}\" <> {1}", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.Like:
				default:
					throw new NotSupportedException(String.Format("The sql WHERE filter class '{0}' does not support the '{1}' value operator.", this.GetType().Name, filterValueOperator));
					break;
			}
		} // GetFilterString

	} // SqlWhereFilterDecimal
	#endregion

	#region SqlWhereFilterDateTime class.
	/// <summary>
	/// Use this class to build sql WHERE filter parts for DateTime fields.
	/// </summary>
	public class SqlWhereFilterDateTime : SqlWhereFilterBase {
		private DateTime filterValue;

		/// <summary>
		/// Use this class to build sql WHERE filter parts for DateTime fields.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		public SqlWhereFilterDateTime(SqlWhereFilterOperator filterOperator, String filterName, SqlWhereFilterValueOperator filterValueOperator, DateTime filterValue) : base(filterOperator, filterValueOperator, filterName) {
			this.filterValue = filterValue;
		} // SqlWhereFilterDateTime

		/// <summary>
		/// Implement this method, to return the part of the sql WHERE filter that compares the field to the value.
		/// </summary>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <returns>The part of the sql WHERE filter that compares the field to the value.</returns>
		protected override String GetFilterString(String filterName, SqlWhereFilterValueOperator filterValueOperator) {
			switch (filterValueOperator) {
				case SqlWhereFilterValueOperator.Equals:
					return String.Format(" \"{0}\" = '{1}'", filterName, this.filterValue.ToString("yyyy-MM-dd HH:mm:ss.fff"));
					break;
				case SqlWhereFilterValueOperator.GreaterThan:
					return String.Format(" \"{0}\" > '{1}'", filterName, this.filterValue.ToString("yyyy-MM-dd HH:mm:ss.fff"));
					break;
				case SqlWhereFilterValueOperator.GreaterThanOrEquals:
					return String.Format(" \"{0}\" >= '{1}'", filterName, this.filterValue.ToString("yyyy-MM-dd HH:mm:ss.fff"));
					break;
				case SqlWhereFilterValueOperator.LessThan:
					return String.Format(" \"{0}\" < '{1}'", filterName, this.filterValue.ToString("yyyy-MM-dd HH:mm:ss.fff"));
					break;
				case SqlWhereFilterValueOperator.LessThanOrEquals:
					return String.Format(" \"{0}\" <= '{1}'", filterName, this.filterValue.ToString("yyyy-MM-dd HH:mm:ss.fff"));
					break;
				case SqlWhereFilterValueOperator.NotEquals:
					return String.Format(" \"{0}\" <> '{1}'", filterName, this.filterValue.ToString("yyyy-MM-dd HH:mm:ss.fff"));
					break;
				case SqlWhereFilterValueOperator.Like:
				default:
					throw new NotSupportedException(String.Format("The sql WHERE filter class '{0}' does not support the '{1}' value operator.", this.GetType().Name, filterValueOperator));
					break;
			}
		} // GetFilterString

	} // SqlWhereFilterDateTime
	#endregion

	#region SqlWhereFilterString class.
	/// <summary>
	/// Use this class to build sql WHERE filter parts for String fields.
	/// </summary>
	public class SqlWhereFilterString : SqlWhereFilterBase {
		private String filterValue;

		/// <summary>
		/// Use this class to build sql WHERE filter parts for String fields.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		public SqlWhereFilterString(SqlWhereFilterOperator filterOperator, String filterName, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, filterValueOperator, filterName) {
			this.filterValue = filterValue;
		} // SqlWhereFilterString

		/// <summary>
		/// Implement this method, to return the part of the sql WHERE filter that compares the field to the value.
		/// </summary>
		/// <param name="filterName">The filter name.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <returns>The part of the sql WHERE filter that compares the field to the value.</returns>
		protected override String GetFilterString(String filterName, SqlWhereFilterValueOperator filterValueOperator) {
			// Escape single quotes.
			this.filterValue = this.filterValue.Replace("'", "''");

			switch (filterValueOperator) {
				case SqlWhereFilterValueOperator.Equals:
					return String.Format(" \"{0}\" = '{1}'", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.GreaterThan:
					return String.Format(" \"{0}\" > '{1}'", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.GreaterThanOrEquals:
					return String.Format(" \"{0}\" >= '{1}'", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.LessThan:
					return String.Format(" \"{0}\" < '{1}'", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.LessThanOrEquals:
					return String.Format(" \"{0}\" <= '{1}'", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.NotEquals:
					return String.Format(" \"{0}\" <> '{1}'", filterName, this.filterValue);
					break;
				case SqlWhereFilterValueOperator.Like:
				default:
					// Escape underscore and percent.
					this.filterValue = this.filterValue.Replace("_", "[_]");
					this.filterValue = this.filterValue.Replace("%", "[%]");
					this.filterValue = this.filterValue.Replace("*", "%");
					this.filterValue = this.filterValue.Replace("?", "_");
					return String.Format(" \"{0}\" LIKE '{1}'", filterName, this.filterValue);
					break;
			}
		} // GetFilterString

	} // SqlWhereFilterString
	#endregion

} // NDK.Framework