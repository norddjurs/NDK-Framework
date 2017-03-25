using System;
using System.Collections.Generic;
using System.Data;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines database access.
	/// </summary>
	public partial interface IFramework {

		#region Database methods.
		/// <summary>
		/// Connects to the database identified with the key in the configuration.
		/// </summary>
		/// <param name="key">The database connection identifier.</param>
		/// <returns>The database connection or null.</returns>
		IDbConnection GetDatabaseConnection(String key);

		/// <summary>
		/// Executes the sql.
		/// The connection must be open.
		/// The sql must use Quoted Identifiers.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="sql">The sql.</param>
		/// <param name="formatArgs">The optional sql string format arguments.</param>
		/// <returns>The data reader result, or null.</returns>
		IDataReader ExecuteSql(IDbConnection connection, String sql, params Object[] formatArgs);

		/// <summary>
		/// Executes a query on the schema and table, filtering the result using the WHERE filters.
		/// The connection must be open.
		/// </summary>
		/// <param name="connection">The database connection.</param>
		/// <param name="schemaName">The schema name.</param>
		/// <param name="tableName">The table name.</param>
		/// <param name="whereFilters">The optional WHERE filters.</param>
		/// <returns>The data reader result, or null.</returns>
		IDataReader ExecuteSql(IDbConnection connection, String schemaName, String tableName, params SqlWhereFilterBase[] whereFilters);
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework