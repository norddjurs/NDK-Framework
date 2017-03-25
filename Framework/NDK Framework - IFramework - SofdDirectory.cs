using System;
using System.Collections.Generic;
using System.Data;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines Sofd Directory access.
	/// </summary>
	public partial interface IFramework {

		#region SOFD employee methods.
		/// <summary>
		/// Gets the employee identified by the employee id.
		/// The employee id can be MaNummer, OpusBrugerNavn, AdBrugerNavn, CprNummer, Epost, Uuid.
		/// </summary>
		/// <param name="employeeId">The employee id to find.</param>
		/// <returns>The matching employee or null.</returns>
		SofdEmployee GetEmployee(String employeeId);

		/// <summary>
		/// Gets all employees or filtered employees.
		/// </summary>
		/// <param name="employeeFilters">Sql WHERE filters.</param>
		/// <returns>All matching employees.</returns>
		List<SofdEmployee> GetAllEmployees(params SqlWhereFilterBase[] employeeFilters);
		#endregion

		#region SOFD organization methods.
		/// <summary>
		/// Gets the organization identified by the organization id.
		/// The organization id can be OrganisationId, CvrNummer, SeNummer, EanNummer, PNummer, Uuid.
		/// </summary>
		/// <param name="organizationId">The organization id to find.</param>
		/// <returns>The matching organization or null.</returns>
		SofdOrganization GetOrganization(String organizationId);

		/// <summary>
		/// Gets all organizations or filtered organizations.
		/// </summary>
		/// <param name="organizationFilters">Sql WHERE filters.</param>
		/// <returns>All matching organizations.</returns>
		List<SofdOrganization> GetAllOrganizations(params SqlWhereFilterBase[] organizationFilters);
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework