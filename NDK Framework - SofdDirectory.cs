using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace NDK.Framework {

	#region SofdDirectory class.
	public class SofdDirectory {
		private IFramework framework = null;
		private IConfiguration config = null;
		private ILogger logger = null;
		private String sofdDatabaseKey = null;

		#region Constructor methods.
		/// <summary>
		/// Connect to the SOFD database.
		/// </summary>
		public SofdDirectory(IFramework framework) {
			this.framework = framework;
			this.config = this.framework.Config;
			this.logger = this.framework.Logger;
			this.sofdDatabaseKey = this.config.GetSystemValue("SofdDirectoryDatabaseKey", "MDM-PROD");
		} // SofdDirectory
		#endregion

		#region Employee methods.
		/// <summary>
		/// Gets the employee identified by the employee id.
		/// The employee id can be MaNummer, OpusBrugerNavn, AdBrugerNavn, CprNummer, Epost, Uuid.
		/// </summary>
		/// <param name="employeeId">The employee id to find.</param>
		/// <returns>The matching employee or null.</returns>
		public SofdEmployee GetEmployee(String employeeId) {
			try {
				// Log.
				this.logger.Log("SOFD: Getting employee identified by '{0}'.", employeeId);

				// Add filters.
				// MedarbejderId is not included, because it conflicts with MaNummer.
				Int32 parsedNumber;
				Guid parsedGuid;
				List<SqlWhereFilterBase> employeeFilters = new List<SqlWhereFilterBase>();

				employeeFilters.Add(new SqlWhereFilterBeginGroup());

				parsedNumber = 0;
				Int32.TryParse(employeeId, out parsedNumber);
				if (parsedNumber > 0) {
					employeeFilters.Add(new SofdEmployeeFilter_MaNummer(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				employeeFilters.Add(new SofdEmployeeFilter_OpusBrugerNavn(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, employeeId));

				employeeFilters.Add(new SofdEmployeeFilter_AdBrugerNavn(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, employeeId));

				employeeFilters.Add(new SofdEmployeeFilter_CprNummer(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, employeeId));

				employeeFilters.Add(new SofdEmployeeFilter_Epost(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, employeeId));

				parsedGuid = Guid.Empty;
				Guid.TryParse(employeeId, out parsedGuid);
				if (parsedGuid.Equals(Guid.Empty) == false) {
					employeeFilters.Add(new SofdEmployeeFilter_Uuid(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedGuid));
				}

				employeeFilters.Add(new SqlWhereFilterEndGroup());

				employeeFilters.Add(new SofdEmployeeFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true));

				// Get all matching employees.
				List<SofdEmployee> employees = this.GetAllEmployees(employeeFilters.ToArray());
				if (employees.Count == 1) {
					// Return the employee.
					return employees[0];
				} else {
					// Return null.
					return null;
				}
			} catch (Exception exception) {
				// Log error.
				this.logger.LogError(exception);

				// Return null;
				return null;
			}
		} // GetEmployee

		/// <summary>
		/// Gets all employees or filtered employees.
		/// </summary>
		/// <param name="employeeFilters">Sql WHERE filters.</param>
		/// <returns>All matching employees.</returns>
		public List<SofdEmployee> GetAllEmployees(params SqlWhereFilterBase[] employeeFilters) {
			try {
				List<SofdEmployee> employees = new List<SofdEmployee>();

				// Log.
				this.logger.Log("SOFD: Getting all employees identified by {0} filters.", employeeFilters.Length);

				// Connect to the database.
				using (IDbConnection dataConnection = this.framework.GetSqlConnection(this.sofdDatabaseKey)) {
					// Execute the query.
					using (IDataReader dataReader = this.framework.ExecuteSql(dataConnection, SofdEmployee.SCHEMA_NAME, SofdEmployee.TABLE_NAME, employeeFilters)) {
						// Read all employees.
						while (dataReader.Read() == true) {
							SofdEmployee employee = new SofdEmployee(this, dataReader);
							employees.Add(employee);
						}
					}
				}

				// Return the result.
				return employees;
			} catch (Exception exception) {
				// Log error.
				this.logger.LogError(exception);

				// Return empty list;
				return new List<SofdEmployee>();
			}
		} // GetAllEmployees
		#endregion

		#region Organization methods.
		/// <summary>
		/// Gets the organisation identified by the organisation id.
		/// The organisation id can be OrganisationId, CvrNummer, SeNummer, EanNummer, PNummer, Uuid.
		/// </summary>
		/// <param name="organisationId">The organisation id to find.</param>
		/// <returns>The matching organisation or null.</returns>
		public SofdOrganisation GetOrganization(String organisationId) {
			try {
				// Log.
				this.logger.Log("SOFD: Getting organisation identified by '{0}'.", organisationId);

				// Add filters.
				Int32 parsedNumber;
				Guid parsedGuid;
				List<SqlWhereFilterBase> organisationFilters = new List<SqlWhereFilterBase>();

				organisationFilters.Add(new SqlWhereFilterBeginGroup());

				parsedNumber = 0;
				Int32.TryParse(organisationId, out parsedNumber);
				if (parsedNumber > 0) {
					organisationFilters.Add(new SofdOrganisationFilter_OrganisationId(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				parsedNumber = 0;
				Int32.TryParse(organisationId, out parsedNumber);
				if (parsedNumber > 0) {
					organisationFilters.Add(new SofdOrganisationFilter_CvrNumber(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				parsedNumber = 0;
				Int32.TryParse(organisationId, out parsedNumber);
				if (parsedNumber > 0) {
					organisationFilters.Add(new SofdOrganisationFilter_SeNumber(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				parsedNumber = 0;
				Int32.TryParse(organisationId, out parsedNumber);
				if (parsedNumber > 0) {
					organisationFilters.Add(new SofdOrganisationFilter_EanNumber(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				parsedNumber = 0;
				Int32.TryParse(organisationId, out parsedNumber);
				if (parsedNumber > 0) {
					organisationFilters.Add(new SofdOrganisationFilter_PNumber(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				parsedGuid = Guid.Empty;
				Guid.TryParse(organisationId, out parsedGuid);
				if (parsedGuid.Equals(Guid.Empty) == false) {
					organisationFilters.Add(new SofdOrganisationFilter_Uuid(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedGuid));
				}

				organisationFilters.Add(new SqlWhereFilterEndGroup());

				organisationFilters.Add(new SofdOrganisationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true));

				// Get all matching organisations.
				List<SofdOrganisation> organisations = this.GetAllOrganisations(organisationFilters.ToArray());
				if (organisations.Count == 1) {
					// Return the organisation.
					return organisations[0];
				} else {
					// Return null.
					return null;
				}
			} catch (Exception exception) {
				// Log error.
				this.logger.LogError(exception);

				// Return null;
				return null;
			}
		} // GetOrganization

		/// <summary>
		/// Gets all organisations or filtered organisations.
		/// </summary>
		/// <param name="organisationFilters">Sql WHERE filters.</param>
		/// <returns>All matching organisations.</returns>
		public List<SofdOrganisation> GetAllOrganisations(params SqlWhereFilterBase[] organisationFilters) {
			try {
				List<SofdOrganisation> organisations = new List<SofdOrganisation>();

				// Log.
				this.logger.Log("SOFD: Getting all organisations identified by {0} filters.", organisationFilters.Length);

				// Connect to the database.
				using (IDbConnection dataConnection = this.framework.GetSqlConnection(this.sofdDatabaseKey)) {
					// Execute the query.
					using (IDataReader dataReader = this.framework.ExecuteSql(dataConnection, SofdOrganisation.SCHEMA_NAME, SofdOrganisation.TABLE_NAME, organisationFilters)) {
						// Read all organisations.
						while (dataReader.Read() == true) {
							SofdOrganisation organisation = new SofdOrganisation(this, dataReader);
							organisations.Add(organisation);
						}
					}
				}

				// Return the result.
				return organisations;
			} catch (Exception exception) {
				// Log error.
				this.logger.LogError(exception);

				// Return empty list;
				return new List<SofdOrganisation>();
			}
		} // GetAllOrganisations
		#endregion

	} // SofdDirectory
	#endregion

} // NDK.Framework