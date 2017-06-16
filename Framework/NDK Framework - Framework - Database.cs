using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NDK.Framework {

	#region Framework
	/// <summary>
	/// This partial part of the class, implements database access.
	/// </summary>
	public abstract partial class Framework : IFramework {

		#region Private database initialization
		private void DatabaseInitialize() {
		} // DatabaseInitialize
		#endregion

		#region Public database methods.
		/// <summary>
		/// Connects to the database identified with the key in the configuration.
		/// </summary>
		/// <param name="key">The database connection identifier.</param>
		/// <returns>The database connection or null.</returns>
		public IDbConnection GetDatabaseConnection(String key) {
			try {
				// Get configuration.
				Int32 dbTimeout = 30;
				String dbEngine = this.GetSystemValue("SqlEngine" + key, "0");
				String dbHost = this.GetSystemValue("SqlHost" + key, "localhost");
				String dbName = this.GetSystemValue("SqlDatabase" + key);
				String dbUserName = this.GetSystemValue("SqlUserid" + key);
				String dbUserPassword = this.GetSystemValue("SqlPassword" + key);

				// Log.
				this.Log("SQL: Connecting to database '{2}' at '{1}' as '{3}'", dbEngine, dbHost, dbName, ((dbUserName != null) ? dbUserName : "SSPI (" + Environment.UserName + ")"));

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
				this.LogError(exception);

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
				this.LogInternal("SQL: Execute sql on '{0}':", connection.Database);
				this.LogInternal(command.CommandText);

				// Execute the SQL command.
				IDataReader dataReader = command.ExecuteReader();

				// Log.
				if (dataReader.RecordsAffected < 0) {
					this.LogInternal("SQL: {0} fields in each record.", dataReader.FieldCount);
				} else {
					this.LogInternal("SQL: {0} rows affected.", dataReader.RecordsAffected);
				}

				// Return the data reader.
				return dataReader;
			} catch (Exception exception) {
				// Log error.
				this.LogError(exception);

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

		/// <summary>
		/// Executes a insert query on the schema and table.
		/// The connection must be open.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="schemaName">The schema name.</param>
		/// <param name="tableName">The table name.</param>
		/// <param name="values">The field names (key) and values (value).</param>
		/// <returns>The first column of the first row in the result set.</returns>
		public Object ExecuteInsertSql(IDbConnection connection, String schemaName, String tableName, params KeyValuePair<String, Object>[] values) {
			// Build sql.
			String sqlNames = String.Empty;
			String sqlValues = String.Empty;
			foreach (KeyValuePair<String, Object> value in values) {
				sqlNames += String.Format("\"{0}\",", value.Key);
				sqlValues += String.Format("@{0},", value.Key);
			}
			sqlNames = sqlNames.Trim(',');
			sqlValues = sqlValues.Trim(',');
			String sql = String.Format("INSERT INTO \"{0}\".\"{1}\" ({2}) VALUES ({3}); SELECT SCOPE_IDENTITY();", schemaName, tableName, sqlNames, sqlValues);

			// Create the command object.
			IDbCommand command = connection.CreateCommand();
			command.CommandText = sql;
			foreach (KeyValuePair<String, Object> value in values) {
				IDbDataParameter commandParameter = command.CreateParameter();
				commandParameter.ParameterName = value.Key;
				commandParameter.Value = value.Value ?? (Object)DBNull.Value;
				command.Parameters.Add(commandParameter);
			}
			//command.Transaction = null;
			//command.CommandTimeout = 30 * 1000;

			// Log.
			this.LogInternal("SQL: Execute sql on '{0}':", connection.Database);
			this.LogInternal(command.CommandText);

			// Execute the SQL command.
			Object dataRecordKey = command.ExecuteScalar();

			// Log.
			this.LogInternal("SQL: {0} key affected.", dataRecordKey.ToString());

			// Return the new key.
			return dataRecordKey;
		} // ExecuteInsertSql

		/// <summary>
		/// Executes a update query on the schema and table.
		/// The connection must be open.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="schemaName">The schema name.</param>
		/// <param name="tableName">The table name.</param>
		/// <param name="values">The field names (key) and values (value).</param>
		/// <param name="whereFilters">The WHERE filters.</param>
		/// <returns>The number of records affected.</returns>
		public Int32 ExecuteUpdateSql(IDbConnection connection, String schemaName, String tableName, IList<KeyValuePair<String, Object>> values, params SqlWhereFilterBase[] whereFilters) {
			// Build sql name values.
			String sqlNameValues = String.Empty;
			foreach (KeyValuePair<String, Object> value in values) {
				sqlNameValues += String.Format("\"{0}\" = @{0},", value.Key);
			}
			sqlNameValues = sqlNameValues.Trim(',');

			// Build sql WHERE filters.
			String sqlWhereFilters = String.Empty;
			if (whereFilters.Length > 0) {
				Boolean firstFilter = true;
				sqlWhereFilters += " WHERE ";
				foreach (SqlWhereFilterBase whereFilter in whereFilters) {
					if (firstFilter == false) {
						firstFilter = (whereFilter.GetType() == typeof(SqlWhereFilterEndGroup));
					}
					sqlWhereFilters += whereFilter.ToString(firstFilter);
					firstFilter = (whereFilter.GetType() == typeof(SqlWhereFilterBeginGroup));
				}
			}

			// Build sql.
			String sql = String.Format("UPDATE \"{0}\".\"{1}\" SET {2} {3}", schemaName, tableName, sqlNameValues, sqlWhereFilters);

			// Create the command object.
			IDbCommand command = connection.CreateCommand();
			command.CommandText = sql;
			foreach (KeyValuePair<String, Object> value in values) {
				IDbDataParameter commandParameter = command.CreateParameter();
				commandParameter.ParameterName = value.Key;
				commandParameter.Value = value.Value ?? (Object)DBNull.Value;
				command.Parameters.Add(commandParameter);
			}
			//command.Transaction = null;
			//command.CommandTimeout = 30 * 1000;

			// Log.
			this.LogInternal("SQL: Execute sql on '{0}':", connection.Database);
			this.LogInternal(command.CommandText);

			// Execute the SQL command.
			Int32 dataRecordAffected = command.ExecuteNonQuery();

			// Log.
			this.LogInternal("SQL: {0} records affected.", dataRecordAffected);

			// Return the number of records affected.
			return dataRecordAffected;
		} // ExecuteUpdateSql
		#endregion

	} // Framework
	#endregion

} // NDK.Framework