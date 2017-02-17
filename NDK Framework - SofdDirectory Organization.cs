using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace NDK.Framework {

	#region SofdOrganisation class.
	public class SofdOrganisation : IEqualityComparer<SofdOrganisation>, IEquatable<SofdOrganisation>, IComparable {
		private SofdDirectory sofdDirectory;

		#region Field name constants.
		public const String SCHEMA_NAME = "etl";
		public const String TABLE_NAME = "Organisation";
		public const String FIELD_ORGANISATION_HISTORIK_ID = "OrganisationHistorikId";
		public const String FIELD_ORGANISATION_ID = "OrganisationId";
		public const String FIELD_AKTIV = "Aktiv";
		public const String FIELD_SENEST_AKTIV = "SenestAktiv";
		public const String FIELD_AKTIV_FRA = "AktivFra";
		public const String FIELD_AKTIV_TIL = "AktivTil";
		public const String FIELD_SIDST_AENDRET = "SidstAendret";
		public const String FIELD_LOS_ORGANISATION_ID = "LOSOrgId";
		public const String FIELD_PARENT_LOS_ORGANISATION_ID = "ParentLOSOrgId";
		public const String FIELD_LOS_SIDST_AENDRET = "LOSSidstAendret";
		public const String FIELD_KORT_NAVN = "KortNavn";
		public const String FIELD_NAVN = "Navn";
		public const String FIELD_GADE = "Gade";
		public const String FIELD_STED_NAVN = "Stednavn";
		public const String FIELD_POST_NUMMER = "Postnr";
		public const String FIELD_BY = "By";
		public const String FIELD_TELEFON_NUMMER = "Telefon";
		public const String FIELD_CVR_NUMMER = "CvrNr";
		public const String FIELD_SE_NUMMER = "SeNr";
		public const String FIELD_EAN_NUMMER = "EanNr";
		public const String FIELD_P_NUMMER = "PNr";
		public const String FIELD_OMKOSTNINGS_STED = "Omkostningssted";
		public const String FIELD_ORGANISATION_TYPE_ID = "OrgTypeID";
		public const String FIELD_ORGANISATION_TYPE = "OrgType";
		public const String FIELD_UUID = "UUID";
		public const String FIELD_LEDER_MA_NUMMER = "LederMaNr";
		public const String FIELD_LEDER_MEDARBEJDER_ID = "LederMedarbejderId";
		public const String FIELD_LEDER_NEDARVET = "LederNedarvet";
		public const String FIELD_LEDER_NAVN = "LederNavn";
		public const String FIELD_LEDER_AD_BRUGER_NAVN = "LederADBrugerNavn";
		#endregion

		#region Variables.
		private Int32 organisationHistorikId = -1;
		private Int32 organisationId = -1;
		private Boolean aktiv = false;
		private Boolean senestAktiv = false;
		private DateTime aktivFra = DateTime.MinValue;
		private DateTime aktivTil = DateTime.MinValue;
		private DateTime sidstAendret = DateTime.MinValue;
		private Int32 losOrganisationId = -1;
		private Int32 losForaelderOrganisationId = -1;
		private DateTime losSidstAendret = DateTime.MinValue;
		private String kortNavn = String.Empty;
		private String navn = String.Empty;
		private String gade = String.Empty;
		private String stedNavn = String.Empty;
		private Int16 postNummer = 0;
		private String by = String.Empty;
		private String telefonNummer = String.Empty;
		private Int32 cvrNummer = -1;
		private Int32 seNummer = -1;
		private Int64 eanNummer = -1;
		private Int32 pNummer = -1;
		private Int64 omkostningssted = -1;
		private Int16 organisationTypeID = -1;
		private String organisationType = String.Empty;
		private Guid uuid = Guid.Empty;
		private Int32 lederMaNummer = -1;
		private Int32 lederMedarbejderId = -1;
		private Boolean lederNedarvet = false;
		private String lederNavn = String.Empty;
		private String lederAdBrugerNavn = String.Empty;
		#endregion

		#region Constructor methods.
		/// <summary>
		/// Instantiates a new SOFD organization object, and initializes with values from the current
		/// record in the data reader.
		/// </summary>
		/// <param name="dbReader">The data reader.</param>
		/// <param name="sofdDirectory">The SOFD directory.</param>
		public SofdOrganisation(SofdDirectory sofdDirectory, IDataReader dbReader) {
			this.sofdDirectory = sofdDirectory;

			this.organisationHistorikId = dbReader.GetInt32(SofdOrganisation.FIELD_ORGANISATION_HISTORIK_ID);
			this.organisationId = dbReader.GetInt32(SofdOrganisation.FIELD_ORGANISATION_ID);
			this.aktiv = dbReader.GetBoolean(SofdOrganisation.FIELD_AKTIV);
			this.senestAktiv = dbReader.GetBoolean(SofdOrganisation.FIELD_SENEST_AKTIV);
			this.aktivFra = dbReader.GetDateTime(SofdOrganisation.FIELD_AKTIV_FRA);
			this.aktivTil = dbReader.GetDateTime(SofdOrganisation.FIELD_AKTIV_TIL);
			this.sidstAendret = dbReader.GetDateTime(SofdOrganisation.FIELD_SIDST_AENDRET);
			this.losOrganisationId = dbReader.GetInt32(SofdOrganisation.FIELD_LOS_ORGANISATION_ID);
			this.losForaelderOrganisationId = dbReader.GetInt32(SofdOrganisation.FIELD_PARENT_LOS_ORGANISATION_ID);
			this.losSidstAendret = dbReader.GetDateTime(SofdOrganisation.FIELD_LOS_SIDST_AENDRET);
			this.kortNavn = dbReader.GetString(SofdOrganisation.FIELD_KORT_NAVN);
			this.navn = dbReader.GetString(SofdOrganisation.FIELD_NAVN);
			this.gade = dbReader.GetString(SofdOrganisation.FIELD_GADE);
			this.stedNavn = dbReader.GetString(SofdOrganisation.FIELD_STED_NAVN);
			this.postNummer = dbReader.GetInt16(SofdOrganisation.FIELD_POST_NUMMER);
			this.by = dbReader.GetString(SofdOrganisation.FIELD_BY);
			this.telefonNummer = dbReader.GetString(SofdOrganisation.FIELD_TELEFON_NUMMER);
			this.cvrNummer = dbReader.GetInt32(SofdOrganisation.FIELD_CVR_NUMMER);
			this.seNummer = dbReader.GetInt32(SofdOrganisation.FIELD_SE_NUMMER);
			this.eanNummer = dbReader.GetInt64(SofdOrganisation.FIELD_EAN_NUMMER);
			this.pNummer = dbReader.GetInt32(SofdOrganisation.FIELD_P_NUMMER);
			this.omkostningssted = dbReader.GetInt64(SofdOrganisation.FIELD_OMKOSTNINGS_STED);
			this.organisationTypeID = dbReader.GetInt16(SofdOrganisation.FIELD_ORGANISATION_TYPE_ID);
			this.organisationType = dbReader.GetString(SofdOrganisation.FIELD_ORGANISATION_TYPE);
			this.uuid = dbReader.GetGuid(SofdOrganisation.FIELD_UUID);
			this.lederMaNummer = dbReader.GetInt32(SofdOrganisation.FIELD_LEDER_MA_NUMMER);
			this.lederMedarbejderId = dbReader.GetInt32(SofdOrganisation.FIELD_LEDER_MEDARBEJDER_ID);
			this.lederNedarvet = dbReader.GetBoolean(SofdOrganisation.FIELD_LEDER_NEDARVET);
			this.lederNavn = dbReader.GetString(SofdOrganisation.FIELD_LEDER_NAVN);
			this.lederAdBrugerNavn = dbReader.GetString(SofdOrganisation.FIELD_LEDER_AD_BRUGER_NAVN);
		} // SofdOrganisation
		#endregion

		#region Compare and Equal methods.
		/// <summary>
		/// Gets true if the two instances are equal.
		/// The OrganisationId is used for comparison.
		/// </summary>
		/// <param name="x">The X instance.</param>
		/// <param name="y">The Y instance.</param>
		/// <returns>True if equal.</returns>
		public Boolean Equals(SofdOrganisation x, SofdOrganisation y) {
			return x.organisationId.Equals(y.organisationId);
		} // Equals

		/// <summary>
		/// Gets true if the given instance is equal to this instance.
		/// The OrganisationId is used for comparison.
		/// </summary>
		/// <param name="other">The other instance.</param>
		/// <returns>True if equal.</returns>
		public Boolean Equals(SofdOrganisation other) {
			return this.organisationId.Equals(other.organisationId);
		} // Equals

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates
		/// whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// 
		/// The OrganisationId is used for comparison.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns>Less then zero, zero or more then zero to indicate sort order.</returns>
		public Int32 CompareTo(Object obj) {
			if ((obj == null) || (this.GetType() != obj.GetType())) {
				// The objects are not the same type.
				return 1;
			} else {
				// Compare.
				return this.organisationId.CompareTo(((SofdOrganisation)obj).organisationId);
			}
		} // CompareTo

		/// <summary>
		/// Gets true if the given instance is equal to this instance.
		/// The OrganisationId is used for comparison.
		/// </summary>
		/// <param name="other">The other instance.</param>
		/// <returns>True if equal.</returns>
		public override Boolean Equals(Object obj) {
			if ((obj == null) || (this.GetType() != obj.GetType())) {
				return false;
			} else {
				// Compare.
				return this.organisationId.Equals(((SofdOrganisation)obj).organisationId);
			}
		} // Equals

		/// <summary>
		/// Gets the hash code from the OrganisationId.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>The hash code.</returns>
		public Int32 GetHashCode(SofdOrganisation obj) {
			return obj.organisationId.GetHashCode();
		} // GetHashCode

		/// <summary>
		/// Gets the hash code from the OrganisationId.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>The hash code.</returns>
		public override Int32 GetHashCode() {
			return this.organisationId.GetHashCode();
		} // GetHashCode
		#endregion

		#region Properties.
		public Int32 OrganisationHistorikId {
			get {
				return this.organisationHistorikId;
			}
		} // OrganisationHistorikId

		public Int32 OrganisationId {
			get {
				return this.organisationId;
			}
		} // OrganisationId

		public Boolean Aktiv {
			get {
				return this.aktiv;
			}
		} // Aktiv

		public Boolean SenestAktiv {
			get {
				return this.senestAktiv;
			}
		} // SenestAktiv

		public DateTime AktivFra {
			get {
				return this.aktivFra;
			}
		} // AktivFra

		public DateTime AktivTil {
			get {
				return this.aktivTil;
			}
		} // AktivTil

		public DateTime SidstAendret {
			get {
				return this.sidstAendret;
			}
		} // SidstAendret

		public Int32 LosOrganisationId {
			get {
				return this.losOrganisationId;
			}
		} // LosOrganisationId

		public Int32 LosForaelderOrganisationId {
			get {
				return this.losForaelderOrganisationId;
			}
		} // LosForaelderOrganisationId

		public DateTime LosSidstAendret {
			get {
				return this.losSidstAendret;
			}
		} // LosSidstAendret

		public String KortNavn {
			get {
				return this.kortNavn;
			}
		} // KortNavn

		public String Navn {
			get {
				return this.navn;
			}
		} // Navn

		public String Gade {
			get {
				return this.gade;
			}
		} // Gade

		public String StedNavn {
			get {
				return this.stedNavn;
			}
		} // StedNavn

		public Int16 PostNummer {
			get {
				return this.postNummer;
			}
		} // PostNummer

		public String By {
			get {
				return this.by;
			}
		} // By

		public String TelefonNummer {
			get {
				return this.telefonNummer;
			}
		} // TelefonNummer

		public Int32 CvrNummer {
			get {
				return this.cvrNummer;
			}
		} // CvrNummer

		public Int32 SeNummer {
			get {
				return this.seNummer;
			}
		} // SeNummer

		public Int64 EanNummer {
			get {
				return this.eanNummer;
			}
		} // EanNummer

		public Int32 PNummer {
			get {
				return this.pNummer;
			}
		} // PNummer

		public Int64 Omkostningssted {
			get {
				return this.omkostningssted;
			}
		} // Omkostningssted

		public Int16 OrganisationTypeID {
			get {
				return this.organisationTypeID;
			}
		} // OrganisationTypeID

		public String OrganisationType {
			get {
				return this.organisationType;
			}
		} // OrganisationType

		public Guid Uuid {
			get {
				return this.uuid;
			}
		} // Uuid

		public Int32 LederMaNummer {
			get {
				return this.lederMaNummer;
			}
		} // LederMaNummer

		public Int32 LederMedarbejderId {
			get {
				return this.lederMedarbejderId;
			}
		} // LederMedarbejderId

		public Boolean LederNedarvet {
			get {
				return this.lederNedarvet;
			}
		} // LederNedarvet

		public String LederNavn {
			get {
				return this.lederNavn;
			}
		} // LederNavn

		public String LederAdBrugerNavn {
			get {
				return this.lederAdBrugerNavn;
			}
		} // LederAdBrugerNavn
		#endregion

		#region Get methods.
		/// <summary>
		/// Gets the parent organisation associated with this organisation.
		/// </summary>
		/// <returns>The matching organisation.</returns>
		public SofdOrganisation GetParentOrganisation() {
			// Get all matching organisations.
			List<SofdOrganisation> organisations = this.sofdDirectory.GetAllOrganisations(
				new SofdOrganisationFilter_LosOrganisationId(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.losForaelderOrganisationId),
				new SofdOrganisationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
			);

			// Return the organisation, only if one matched the filters.
			if (organisations.Count == 1) {
				return organisations[0];
			} else {
				// Return null.
				return null;
			}
		} // GetParentOrganisation

		/// <summary>
		/// Gets the sibling organisations associated with the same parent as this organisation.
		/// </summary>
		/// <param name="removeThisOrganisation">True to remove this organisation from the result.</param>
		/// <returns>The matching organisation.</returns>
		public List<SofdOrganisation> GetSiblingOrganisations(Boolean removeThisOrganisation = false) {
			// Get all matching organisations.
			List<SofdOrganisation> organisations = this.sofdDirectory.GetAllOrganisations(
				new SofdOrganisationFilter_LosForaelderOrganisationId(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.losForaelderOrganisationId),
				new SofdOrganisationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
			);

			// Remove this organisation from the result.
			// The Equals override, makes this work.
			if (removeThisOrganisation == true) {
				organisations.Remove(this);
			}

			// Return the organisations.
			return organisations;
		} // GetSiblingOrganisations

		  /// <summary>
		  /// Gets the leader associated with this organisation.
		  /// </summary>
		  /// <returns>The matching employee.</returns>
		public SofdEmployee GetLeader() {
			// Get all matching employees.
			List<SofdEmployee> employees = this.sofdDirectory.GetAllEmployees(
				new SofdEmployeeFilter_MaNummer(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.LederMaNummer),
				new SofdOrganisationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
			);

			// Return the employee, only if one matched the filters.
			if (employees.Count == 1) {
				return employees[0];
			} else {
				// Return null.
				return null;
			}
		} // GetLeader

		/// <summary>
		/// Gets the employees associated with this organisation.
		/// </summary>
		/// <returns>The matching employees.</returns>
		public List<SofdEmployee> GetEmployees() {
			// Get all matching employees.
			List<SofdEmployee> employees = this.sofdDirectory.GetAllEmployees(
				new SofdEmployeeFilter_OrganisationId(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.organisationId),
				new SofdOrganisationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
			);

			// Return the employees.
			return employees;
		} // GetEmployees

		#endregion

	} // SofdOrganisation
	#endregion

	#region SofdOrganisationFilter_OrganisationHistorikId class
	/// <summary>
	/// Organisation filter on OrganisationHistorikId.
	/// </summary>
	public class SofdOrganisationFilter_OrganisationHistorikId : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on OrganisationHistorikId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganisationFilter_OrganisationHistorikId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganisation.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdOrganisationFilter_OrganisationHistorikId

	} // SofdOrganisationFilter_OrganisationHistorikId
	#endregion

	#region SofdOrganisationFilter_OrganisationId class
	/// <summary>
	/// Organisation filter on OrganisationId.
	/// </summary>
	public class SofdOrganisationFilter_OrganisationId : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on OrganisationId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganisationFilter_OrganisationId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganisation.FIELD_ORGANISATION_ID, filterValueOperator, filterValue) {
		} // SofdOrganisationFilter_OrganisationId

	} // SofdOrganisationFilter_OrganisationId
	#endregion

	#region SofdOrganisationFilter_Aktiv class
	/// <summary>
	/// Organisation filter on Aktiv.
	/// </summary>
	public class SofdOrganisationFilter_Aktiv : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on Aktiv.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganisationFilter_Aktiv(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Boolean filterValue) : base(filterOperator, SofdOrganisation.FIELD_AKTIV, filterValueOperator, ((filterValue == true) ? 1 : 0)) {
		} // SofdOrganisationFilter_Aktiv

	} // SofdOrganisationFilter_Aktiv
	#endregion

	#region SofdOrganisationFilter_LosOrganisationId class
	/// <summary>
	/// Organisation filter on LosOrganisationId.
	/// </summary>
	public class SofdOrganisationFilter_LosOrganisationId : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on LosOrganisationId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganisationFilter_LosOrganisationId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganisation.FIELD_LOS_ORGANISATION_ID, filterValueOperator, filterValue) {
		} // SofdOrganisationFilter_LosOrganisationId

	} // SofdOrganisationFilter_LosOrganisationId
	#endregion

	#region SofdOrganisationFilter_LosForaelderOrganisationId class
	/// <summary>
	/// Organisation filter on LosForaelderOrganisationId.
	/// </summary>
	public class SofdOrganisationFilter_LosForaelderOrganisationId : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on LosForaelderOrganisationId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganisationFilter_LosForaelderOrganisationId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganisation.FIELD_PARENT_LOS_ORGANISATION_ID, filterValueOperator, filterValue) {
		} // SofdOrganisationFilter_LosForaelderOrganisationId

	} // SofdOrganisationFilter_LosForaelderOrganisationId
	#endregion

	#region SofdOrganisationFilter_CvrNumber class
	/// <summary>
	/// Organisation filter on CvrNumber.
	/// </summary>
	public class SofdOrganisationFilter_CvrNumber : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on CvrNumber.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganisationFilter_CvrNumber(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganisation.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdOrganisationFilter_CvrNumber

	} // SofdOrganisationFilter_CvrNumber
	#endregion

	#region SofdOrganisationFilter_SeNumber class
	/// <summary>
	/// Organisation filter on SeNumber.
	/// </summary>
	public class SofdOrganisationFilter_SeNumber : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on SeNumber.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganisationFilter_SeNumber(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganisation.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdOrganisationFilter_SeNumber

	} // SofdOrganisationFilter_SeNumber
	#endregion

	#region SofdOrganisationFilter_EanNumber class
	/// <summary>
	/// Organisation filter on EanNumber.
	/// </summary>
	public class SofdOrganisationFilter_EanNumber : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on EanNumber.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganisationFilter_EanNumber(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganisation.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdOrganisationFilter_EanNumber

	} // SofdOrganisationFilter_EanNumber
	#endregion

	#region SofdOrganisationFilter_PNumber class
	/// <summary>
	/// Organisation filter on PNumber.
	/// </summary>
	public class SofdOrganisationFilter_PNumber : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on PNumber.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganisationFilter_PNumber(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganisation.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdOrganisationFilter_PNumber

	} // SofdOrganisationFilter_PNumber
	#endregion

	#region SofdOrganisationFilter_Uuid class
	/// <summary>
	/// Organisation filter on Uuid.
	/// </summary>
	public class SofdOrganisationFilter_Uuid : SqlWhereFilterString {

		/// <summary>
		/// Organisation filter on Uuid.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganisationFilter_Uuid(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Guid filterValue) : base(filterOperator, SofdOrganisation.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue.ToString()) {
		} // SofdOrganisationFilter_Uuid

	} // SofdOrganisationFilter_Uuid
	#endregion




} // NDK.Framework