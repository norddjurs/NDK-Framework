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
	/// This partial part of the class, implements Sofd Directory access.
	/// </summary>
	public abstract partial class Framework : IFramework {
		private String sofdDatabaseKey = null;

		#region Private Sofd Directory initialization
		private void SofdDirectoryInitialize() {
			this.sofdDatabaseKey = this.GetSystemValue("SofdDirectoryDatabaseKey", "MDM-PROD");
		} // SofdDirectoryInitialize
		#endregion

		#region Public Sofd Directory employee methods.
		/// <summary>
		/// Gets the employee identified by the employee id.
		/// The employee id can be MaNummer, OpusBrugerNavn, AdBrugerNavn, CprNummer, Epost, Uuid.
		/// </summary>
		/// <param name="employeeId">The employee id to find.</param>
		/// <returns>The matching employee or null.</returns>
		public SofdEmployee GetEmployee(String employeeId) {
			try {
				// Log.
				this.Log("SOFD: Getting employee identified by '{0}'.", employeeId);

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
				this.LogError(exception);

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
				this.Log("SOFD: Getting all employees identified by {0} filters.", employeeFilters.Length);

				// Connect to the database.
				using (IDbConnection dataConnection = this.GetDatabaseConnection(this.sofdDatabaseKey)) {
					// Execute the query.
					using (IDataReader dataReader = this.ExecuteSql(dataConnection, SofdEmployee.SCHEMA_NAME, SofdEmployee.TABLE_NAME, employeeFilters)) {
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
				this.LogError(exception);

				// Return empty list;
				return new List<SofdEmployee>();
			}
		} // GetAllEmployees
		#endregion

		#region Public Sofd Directory organization methods.
		/// <summary>
		/// Gets the organization identified by the organization id.
		/// The organization id can be OrganisationId, CvrNummer, SeNummer, EanNummer, PNummer, Uuid.
		/// </summary>
		/// <param name="organizationId">The organization id to find.</param>
		/// <returns>The matching organization or null.</returns>
		public SofdOrganization GetOrganization(String organizationId) {
			try {
				// Log.
				this.Log("SOFD: Getting organization identified by '{0}'.", organizationId);

				// Add filters.
				Int32 parsedNumber;
				Guid parsedGuid;
				List<SqlWhereFilterBase> organizationFilters = new List<SqlWhereFilterBase>();

				organizationFilters.Add(new SqlWhereFilterBeginGroup());

				parsedNumber = 0;
				Int32.TryParse(organizationId, out parsedNumber);
				if (parsedNumber > 0) {
					organizationFilters.Add(new SofdOrganizationFilter_OrganisationId(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				parsedNumber = 0;
				Int32.TryParse(organizationId, out parsedNumber);
				if (parsedNumber > 0) {
					organizationFilters.Add(new SofdOrganizationFilter_CvrNumber(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				parsedNumber = 0;
				Int32.TryParse(organizationId, out parsedNumber);
				if (parsedNumber > 0) {
					organizationFilters.Add(new SofdOrganizationFilter_SeNumber(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				parsedNumber = 0;
				Int32.TryParse(organizationId, out parsedNumber);
				if (parsedNumber > 0) {
					organizationFilters.Add(new SofdOrganizationFilter_EanNumber(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				parsedNumber = 0;
				Int32.TryParse(organizationId, out parsedNumber);
				if (parsedNumber > 0) {
					organizationFilters.Add(new SofdOrganizationFilter_PNumber(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedNumber));
				}

				parsedGuid = Guid.Empty;
				Guid.TryParse(organizationId, out parsedGuid);
				if (parsedGuid.Equals(Guid.Empty) == false) {
					organizationFilters.Add(new SofdOrganizationFilter_Uuid(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, parsedGuid));
				}

				organizationFilters.Add(new SqlWhereFilterEndGroup());

				organizationFilters.Add(new SofdOrganizationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true));

				// Get all matching organizations.
				List<SofdOrganization> organizations = this.GetAllOrganizations(organizationFilters.ToArray());
				if (organizations.Count == 1) {
					// Return the organization.
					return organizations[0];
				} else {
					// Return null.
					return null;
				}
			} catch (Exception exception) {
				// Log error.
				this.LogError(exception);

				// Return null;
				return null;
			}
		} // GetOrganization

		/// <summary>
		/// Gets all organizations or filtered organizations.
		/// </summary>
		/// <param name="organizationFilters">Sql WHERE filters.</param>
		/// <returns>All matching organizations.</returns>
		public List<SofdOrganization> GetAllOrganizations(params SqlWhereFilterBase[] organizationFilters) {
			try {
				List<SofdOrganization> organizations = new List<SofdOrganization>();

				// Log.
				this.Log("SOFD: Getting all organizations identified by {0} filters.", organizationFilters.Length);

				// Connect to the database.
				using (IDbConnection dataConnection = this.GetDatabaseConnection(this.sofdDatabaseKey)) {
					// Execute the query.
					using (IDataReader dataReader = this.ExecuteSql(dataConnection, SofdOrganization.SCHEMA_NAME, SofdOrganization.TABLE_NAME, organizationFilters)) {
						// Read all organizations.
						while (dataReader.Read() == true) {
							SofdOrganization organization = new SofdOrganization(this, dataReader);
							organizations.Add(organization);
						}
					}
				}

				// Return the result.
				return organizations;
			} catch (Exception exception) {
				// Log error.
				this.LogError(exception);

				// Return empty list;
				return new List<SofdOrganization>();
			}
		} // GetAllOrganizations
		#endregion

	} // Framework
	#endregion

} // NDK.Framework