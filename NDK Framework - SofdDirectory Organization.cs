using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace NDK.Framework {

	#region SofdOrganization class.
	public class SofdOrganization : IEqualityComparer<SofdOrganization>, IEquatable<SofdOrganization>, IComparable {
		private IFramework framework;
		private Boolean isNew = false;
		private Boolean isChanged = false;

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
		// Value.
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

		// Changed.
		private Boolean organisationHistorikIdChanged = false;
		private Boolean organisationIdChanged = false;
		private Boolean aktivChanged = false;
		private Boolean senestAktivChanged = false;
		private Boolean aktivFraChanged = false;
		private Boolean aktivTilChanged = false;
		private Boolean sidstAendretChanged = false;
		private Boolean losOrganisationIdChanged = false;
		private Boolean losForaelderOrganisationIdChanged = false;
		private Boolean losSidstAendretChanged = false;
		private Boolean kortNavnChanged = false;
		private Boolean navnChanged = false;
		private Boolean gadeChanged = false;
		private Boolean stedNavnChanged = false;
		private Boolean postNummerChanged = false;
		private Boolean byChanged = false;
		private Boolean telefonNummerChanged = false;
		private Boolean cvrNummerChanged = false;
		private Boolean seNummerChanged = false;
		private Boolean eanNummerChanged = false;
		private Boolean pNummerChanged = false;
		private Boolean omkostningsstedChanged = false;
		private Boolean organisationTypeIDChanged = false;
		private Boolean organisationTypeChanged = false;
		private Boolean uuidChanged = false;
		private Boolean lederMaNummerChanged = false;
		private Boolean lederMedarbejderIdChanged = false;
		private Boolean lederNedarvetChanged = false;
		private Boolean lederNavnChanged = false;
		private Boolean lederAdBrugerNavnChanged = false;
		#endregion

		#region Constructor methods.
		/// <summary>
		/// Instantiates a new SOFD organization object, that does not exist in the database.
		/// </summary>
		/// <param name="framework">The framework.</param>
		public SofdOrganization(IFramework framework) {
			this.framework = framework;
			this.isNew = true;
			this.isChanged = false;

			// Initialize values into the fields that do not allow NULL.
			this.aktivFra = DateTime.Now.Date;
			this.sidstAendret = DateTime.Now.Date;
			this.aktiv = false;
			this.losOrganisationId = 0;
		} // SofdOrganization

		/// <summary>
		/// Instantiates a new SOFD organization object, and initializes with values from the current
		/// record in the data reader.
		/// </summary>
		/// <param name="framework">The framework.</param>
		/// <param name="dbReader">The data reader.</param>
		public SofdOrganization(IFramework framework, IDataReader dbReader) {
			this.framework = framework;
			this.isNew = false;
			this.isChanged = false;

			this.organisationHistorikId = dbReader.GetInt32(SofdOrganization.FIELD_ORGANISATION_HISTORIK_ID);
			this.organisationId = dbReader.GetInt32(SofdOrganization.FIELD_ORGANISATION_ID);
			this.aktiv = dbReader.GetBoolean(SofdOrganization.FIELD_AKTIV);
			this.senestAktiv = dbReader.GetBoolean(SofdOrganization.FIELD_SENEST_AKTIV);
			this.aktivFra = dbReader.GetDateTime(SofdOrganization.FIELD_AKTIV_FRA);
			this.aktivTil = dbReader.GetDateTime(SofdOrganization.FIELD_AKTIV_TIL);
			this.sidstAendret = dbReader.GetDateTime(SofdOrganization.FIELD_SIDST_AENDRET);
			this.losOrganisationId = dbReader.GetInt32(SofdOrganization.FIELD_LOS_ORGANISATION_ID);
			this.losForaelderOrganisationId = dbReader.GetInt32(SofdOrganization.FIELD_PARENT_LOS_ORGANISATION_ID);
			this.losSidstAendret = dbReader.GetDateTime(SofdOrganization.FIELD_LOS_SIDST_AENDRET);
			this.kortNavn = dbReader.GetString(SofdOrganization.FIELD_KORT_NAVN);
			this.navn = dbReader.GetString(SofdOrganization.FIELD_NAVN);
			this.gade = dbReader.GetString(SofdOrganization.FIELD_GADE);
			this.stedNavn = dbReader.GetString(SofdOrganization.FIELD_STED_NAVN);
			this.postNummer = dbReader.GetInt16(SofdOrganization.FIELD_POST_NUMMER);
			this.by = dbReader.GetString(SofdOrganization.FIELD_BY);
			this.telefonNummer = dbReader.GetString(SofdOrganization.FIELD_TELEFON_NUMMER);
			this.cvrNummer = dbReader.GetInt32(SofdOrganization.FIELD_CVR_NUMMER);
			this.seNummer = dbReader.GetInt32(SofdOrganization.FIELD_SE_NUMMER);
			this.eanNummer = dbReader.GetInt64(SofdOrganization.FIELD_EAN_NUMMER);
			this.pNummer = dbReader.GetInt32(SofdOrganization.FIELD_P_NUMMER);
			this.omkostningssted = dbReader.GetInt64(SofdOrganization.FIELD_OMKOSTNINGS_STED);
			this.organisationTypeID = dbReader.GetInt16(SofdOrganization.FIELD_ORGANISATION_TYPE_ID);
			this.organisationType = dbReader.GetString(SofdOrganization.FIELD_ORGANISATION_TYPE);
			this.uuid = dbReader.GetGuid(SofdOrganization.FIELD_UUID);
			this.lederMaNummer = dbReader.GetInt32(SofdOrganization.FIELD_LEDER_MA_NUMMER);
			this.lederMedarbejderId = dbReader.GetInt32(SofdOrganization.FIELD_LEDER_MEDARBEJDER_ID);
			this.lederNedarvet = dbReader.GetBoolean(SofdOrganization.FIELD_LEDER_NEDARVET);
			this.lederNavn = dbReader.GetString(SofdOrganization.FIELD_LEDER_NAVN);
			this.lederAdBrugerNavn = dbReader.GetString(SofdOrganization.FIELD_LEDER_AD_BRUGER_NAVN);
		} // SofdOrganization
		#endregion

		#region Compare and Equal methods.
		/// <summary>
		/// Gets true if the two instances are equal.
		/// The OrganisationId is used for comparison.
		/// </summary>
		/// <param name="x">The X instance.</param>
		/// <param name="y">The Y instance.</param>
		/// <returns>True if equal.</returns>
		public Boolean Equals(SofdOrganization x, SofdOrganization y) {
			return x.organisationId.Equals(y.organisationId);
		} // Equals

		/// <summary>
		/// Gets true if the given instance is equal to this instance.
		/// The OrganisationId is used for comparison.
		/// </summary>
		/// <param name="other">The other instance.</param>
		/// <returns>True if equal.</returns>
		public Boolean Equals(SofdOrganization other) {
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
				return this.organisationId.CompareTo(((SofdOrganization)obj).organisationId);
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
				return this.organisationId.Equals(((SofdOrganization)obj).organisationId);
			}
		} // Equals

		/// <summary>
		/// Gets the hash code from the OrganisationId.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>The hash code.</returns>
		public Int32 GetHashCode(SofdOrganization obj) {
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
			set {
				if (value.Equals(this.aktiv) == false) {
					this.aktiv = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
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
			set {
				if (value.Equals(this.losOrganisationId) == false) {
					this.losOrganisationId = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // LosOrganisationId

		public Int32 LosForaelderOrganisationId {
			get {
				return this.losForaelderOrganisationId;
			}
			set {
				if (value.Equals(this.losForaelderOrganisationId) == false) {
					this.losForaelderOrganisationId = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // LosForaelderOrganisationId

		public DateTime LosSidstAendret {
			get {
				return this.losSidstAendret;
			}
			set {
				if (value.Equals(this.losSidstAendret) == false) {
					this.losSidstAendret = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // LosSidstAendret

		public String KortNavn {
			get {
				return this.kortNavn;
			}
			set {
				if (value.EqualsHandleNull(this.kortNavn) == false) {
					this.kortNavn = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // KortNavn

		public String Navn {
			get {
				return this.navn;
			}
			set {
				if (value.EqualsHandleNull(this.navn) == false) {
					this.navn = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // Navn

		public String AdresseText {
			get {
				String resultAddress = this.gade ?? String.Empty;
				String resultPlace = this.stedNavn ?? String.Empty;
				String resultPostNumber = this.postNummer.ToString() ?? String.Empty;
				String resultCity = this.by ?? String.Empty;
				String resultCountry = (this.by.IsNullOrWhiteSpace() == false) ? "DK" : String.Empty;

				// First line: Address, Place
				String result1 = resultAddress + ", " + resultPlace;
				result1.Trim(',');
				result1.Trim();

				// Second line: Country PostNumber City
				String result2 = resultCountry + "-" + resultPostNumber + "  " + resultCity;
				result1.Trim('-');
				result1.Trim();

				// Return the address text.
				return result1 + Environment.NewLine + result2;
			}
		} // AdresseText

		public String Gade {
			get {
				return this.gade;
			}
			set {
				if (value.EqualsHandleNull(this.gade) == false) {
					this.gade = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // Gade

		public String StedNavn {
			get {
				return this.stedNavn;
			}
			set {
				if (value.EqualsHandleNull(this.stedNavn) == false) {
					this.stedNavn = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // StedNavn

		public Int16 PostNummer {
			get {
				return this.postNummer;
			}
			set {
				if (value.Equals(this.postNummer) == false) {
					this.postNummer = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // PostNummer

		public String By {
			get {
				return this.by;
			}
			set {
				if (value.EqualsHandleNull(this.by) == false) {
					this.by = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // By

		public String TelefonNummer {
			get {
				return this.telefonNummer;
			}
			set {
				if (value.EqualsHandleNull(this.telefonNummer) == false) {
					this.telefonNummer = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // TelefonNummer

		public Int32 CvrNummer {
			get {
				return this.cvrNummer;
			}
			set {
				if (value.Equals(this.cvrNummer) == false) {
					this.cvrNummer = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // CvrNummer

		public Int32 SeNummer {
			get {
				return this.seNummer;
			}
			set {
				if (value.Equals(this.seNummer) == false) {
					this.seNummer = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // SeNummer

		public Int64 EanNummer {
			get {
				return this.eanNummer;
			}
			set {
				if (value.Equals(this.eanNummer) == false) {
					this.eanNummer = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // EanNummer

		public Int32 PNummer {
			get {
				return this.pNummer;
			}
			set {
				if (value.Equals(this.pNummer) == false) {
					this.pNummer = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // PNummer

		public Int64 Omkostningssted {
			get {
				return this.omkostningssted;
			}
			set {
				if (value.Equals(this.omkostningssted) == false) {
					this.omkostningssted = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // Omkostningssted

		public Int16 OrganisationTypeID {
			get {
				return this.organisationTypeID;
			}
			set {
				if (value.Equals(this.organisationTypeID) == false) {
					this.organisationTypeID = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // OrganisationTypeID

		public String OrganisationType {
			get {
				return this.organisationType;
			}
			set {
				if (value.EqualsHandleNull(this.organisationType) == false) {
					this.organisationType = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // OrganisationType

		public Guid Uuid {
			get {
				return this.uuid;
			}
			set {
				if (value.Equals(this.uuid) == false) {
					this.uuid = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // Uuid

		public Int32 LederMaNummer {
			get {
				return this.lederMaNummer;
			}
			set {
				if (value.Equals(this.lederMaNummer) == false) {
					this.lederMaNummer = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // LederMaNummer

		public Int32 LederMedarbejderId {
			get {
				return this.lederMedarbejderId;
			}
			set {
				if (value.Equals(this.lederMedarbejderId) == false) {
					this.lederMedarbejderId = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // LederMedarbejderId

		public Boolean LederNedarvet {
			get {
				return this.lederNedarvet;
			}
			set {
				if (value.Equals(this.lederNedarvet) == false) {
					this.lederNedarvet = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // LederNedarvet

		public String LederNavn {
			get {
				return this.lederNavn;
			}
			set {
				if (value.EqualsHandleNull(this.lederNavn) == false) {
					this.lederNavn = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // LederNavn

		public String LederAdBrugerNavn {
			get {
				return this.lederAdBrugerNavn;
			}
			set {
				if (value.EqualsHandleNull(this.lederAdBrugerNavn) == false) {
					this.lederAdBrugerNavn = value;
					this.aktivChanged = true;
					this.isChanged = true;
				}
			}
		} // LederAdBrugerNavn
		#endregion

		#region Properties (other).
		public Boolean IsNew {
			get {
				return this.isNew;
			}
		} // IsNew

		public Boolean IsChanged {
			get {
				return this.isChanged;
			}
		} // IsChanged
		#endregion

		#region Save methods.
		/// <summary>
		/// Saves the data to the database.
		/// Note that the SOFD history works like this:
		///		* If the current date is different from "SidstAendret", a new record is created with a new history id.
		///		  Dates and the old record is updated accordingly.
		///		* Multiple updates the same date, are saved in the same history record.
		/// 
		/// Only the fields (properties) with their CHANGED variable set to true are updated.
		/// All CHANGED variables are set to false after the update.
		/// </summary>
		/// <param name="forceUpdateExistingRecord">If true, the existing record is updated and no history record is created.</param>
		public void Save(Boolean forceUpdateExistingRecord) {
			String sofdDatabaseKey = this.framework.GetSystemValue("SofdDirectoryDatabaseKey", "MDM-PROD");
			Int32 previousHistoryId = this.organisationHistorikId;
			Boolean dataUpdateExistingRecord = true;
			Boolean dataUpdateAllFields = false;
			List<KeyValuePair<String, Object>> dataFields = new List<KeyValuePair<String, Object>>();

			// Create new record.
			if (this.isNew == true) {
				// Create new record.
				dataUpdateExistingRecord = false;
				dataUpdateAllFields = false;

				// Update mandatory field values.
				this.aktivFra = DateTime.Now.Date;
				this.aktivFraChanged = true;
				this.sidstAendret = DateTime.Now;
				this.sidstAendretChanged = true;
				this.aktiv = true;
				this.aktivChanged = true;

				// Add fields that do not allow NULL.
				this.aktivFraChanged = true;
				this.sidstAendretChanged = true;
				this.aktivChanged = true;
				this.losOrganisationIdChanged = true;
			} else if ((forceUpdateExistingRecord == false) && (this.isChanged == true) && (this.sidstAendret.Date.Equals(DateTime.Now.Date) == false)) {
				// Always create a new record (copy to create history), when the data in the database was last updated before today.
				dataUpdateExistingRecord = false;
				dataUpdateAllFields = true;

				// Update mandatory field values.
				this.aktivFra = DateTime.Now.Date;
				this.aktivFraChanged = true;
				this.sidstAendret = DateTime.Now;
				this.sidstAendretChanged = true;
			} else if (this.isChanged == true) {
				// Update existing record (no history copy) when it has been updated earlier the same day.
				dataUpdateExistingRecord = true;
				dataUpdateAllFields = false;

				// Update mandatory field values.
				this.sidstAendret = DateTime.Now;
				this.sidstAendretChanged = true;
			}

			// Add updated fields.
			dataFields.Clear();
			if ((this.organisationIdChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_ORGANISATION_ID, this.organisationId));
			}

			if ((this.aktivChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_AKTIV, this.aktiv));
			}

			if ((this.senestAktivChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_SENEST_AKTIV, this.senestAktiv));
			}

			if ((this.aktivFraChanged == true) || (dataUpdateAllFields == true)) {
				if (this.aktivFra.IsMinOrMax() == false) {
					dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_AKTIV_FRA, this.aktivFra));
				} else {
					dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_AKTIV_FRA, null));
				}
			}

			if ((this.aktivTilChanged == true) || (dataUpdateAllFields == true)) {
				if (this.aktivTil.IsMinOrMax() == false) {
					dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_AKTIV_TIL, this.aktivTil));
				} else {
					dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_AKTIV_TIL, null));
				}
			}

			if ((this.sidstAendretChanged == true) || (dataUpdateAllFields == true)) {
				if (this.sidstAendret.IsMinOrMax() == false) {
					dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_SIDST_AENDRET, this.sidstAendret));
				} else {
					dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_SIDST_AENDRET, null));
				}
			}

			if ((this.losOrganisationIdChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_LOS_ORGANISATION_ID, this.losOrganisationId));
			}

			if ((this.losForaelderOrganisationIdChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_PARENT_LOS_ORGANISATION_ID, this.losForaelderOrganisationId));
			}

			if ((this.losSidstAendretChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_LOS_SIDST_AENDRET, this.losSidstAendret));
			}

			if ((this.kortNavnChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_KORT_NAVN, this.kortNavn));
			}

			if ((this.navnChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_NAVN, this.navn));
			}

			if ((this.gadeChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_GADE, this.gade));
			}

			if ((this.stedNavnChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_STED_NAVN, this.stedNavn));
			}

			if ((this.postNummerChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_POST_NUMMER, this.postNummer));
			}

			if ((this.byChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_BY, this.by));
			}

			if ((this.telefonNummerChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_TELEFON_NUMMER, this.telefonNummer));
			}

			if ((this.cvrNummerChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_CVR_NUMMER, this.cvrNummer));
			}

			if ((this.seNummerChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_SE_NUMMER, this.seNummer));
			}

			if ((this.eanNummerChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_EAN_NUMMER, this.eanNummer));
			}

			if ((this.pNummerChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_P_NUMMER, this.pNummer));
			}

			if ((this.omkostningsstedChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_OMKOSTNINGS_STED, this.omkostningssted));
			}

			if ((this.organisationTypeIDChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_ORGANISATION_TYPE_ID, this.organisationTypeID));
			}

			if ((this.organisationTypeChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_ORGANISATION_TYPE, this.organisationType));
			}

			if ((this.uuidChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_UUID, this.uuid));
			}

			if ((this.lederMaNummerChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_LEDER_MA_NUMMER, this.lederMaNummer));
			}

			if ((this.lederMedarbejderIdChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_LEDER_MEDARBEJDER_ID, this.lederMedarbejderId));
			}

			if ((this.lederNedarvetChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_LEDER_NEDARVET, this.lederNedarvet));
			}

			if ((this.lederNavnChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_LEDER_NAVN, this.lederNavn));
			}

			if ((this.lederAdBrugerNavnChanged == true) || (dataUpdateAllFields == true)) {
				dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_LEDER_AD_BRUGER_NAVN, this.LederAdBrugerNavn));
			}

			// Either create a new record, or update the existing record.
			if (dataFields.Count > 0) {
				if (dataUpdateExistingRecord == false) {
					// Create new record.
					using (IDbConnection dataConnection = this.framework.GetDatabaseConnection(sofdDatabaseKey)) {
						Object result = this.framework.ExecuteInsertSql(
							dataConnection,
							SofdOrganization.SCHEMA_NAME,
							SofdOrganization.TABLE_NAME,
							dataFields.ToArray()
						);

						// Set the key field "OrganisationHistorikId".
						this.organisationHistorikId = Int32.Parse(result.ToString());

						// Post update:
						// Set and update the field "OrganisationId" in the new record.
						if (this.isNew == true) {
							this.organisationId = this.organisationHistorikId;

							dataFields.Clear();
							dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_ORGANISATION_ID, this.organisationId));

							this.framework.ExecuteUpdateSql(
								dataConnection,
								SofdOrganization.SCHEMA_NAME,
								SofdOrganization.TABLE_NAME,
								dataFields,
								new SofdOrganizationFilter_OrganisationHistorikId(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, this.organisationHistorikId)
							);
						}

						// History update:
						// Update the history fields in the previous record.
						if (this.isNew == false) {
							dataFields.Clear();
							dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_AKTIV_TIL, this.aktivFra.Date.AddDays(-1)));
							dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_SIDST_AENDRET, this.sidstAendret));
							dataFields.Add(new KeyValuePair<String, Object>(SofdOrganization.FIELD_AKTIV, false));

							this.framework.ExecuteUpdateSql(
								dataConnection,
								SofdOrganization.SCHEMA_NAME,
								SofdOrganization.TABLE_NAME,
								dataFields,
								new SofdOrganizationFilter_OrganisationHistorikId(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, previousHistoryId)
							);
						}

						// Log.
						this.framework.Log("SOFD: Created new organization record identified by history id '{0}'.", this.organisationHistorikId);
					}
				} else {
					// Update existing record.
					using (IDbConnection dataConnection = this.framework.GetDatabaseConnection(sofdDatabaseKey)) {
						this.framework.ExecuteUpdateSql(
							dataConnection,
							SofdOrganization.SCHEMA_NAME,
							SofdOrganization.TABLE_NAME,
							dataFields,
							new SofdOrganizationFilter_OrganisationHistorikId(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, this.organisationHistorikId)
						);

						// Log.
						this.framework.Log("SOFD: Updated existing organization record identified by history id '{0}'.", this.organisationHistorikId);
					}
				}
			}

			// The record is not new or changed any more.
			this.isNew = false;
			this.isChanged = false;

			this.organisationHistorikIdChanged = false;
			this.organisationIdChanged = false;
			this.aktivChanged = false;
			this.senestAktivChanged = false;
			this.aktivFraChanged = false;
			this.aktivTilChanged = false;
			this.sidstAendretChanged = false;
			this.losOrganisationIdChanged = false;
			this.losForaelderOrganisationIdChanged = false;
			this.losSidstAendretChanged = false;
			this.kortNavnChanged = false;
			this.navnChanged = false;
			this.gadeChanged = false;
			this.stedNavnChanged = false;
			this.postNummerChanged = false;
			this.byChanged = false;
			this.telefonNummerChanged = false;
			this.cvrNummerChanged = false;
			this.seNummerChanged = false;
			this.eanNummerChanged = false;
			this.pNummerChanged = false;
			this.omkostningsstedChanged = false;
			this.organisationTypeIDChanged = false;
			this.organisationTypeChanged = false;
			this.uuidChanged = false;
			this.lederMaNummerChanged = false;
			this.lederMedarbejderIdChanged = false;
			this.lederNedarvetChanged = false;
			this.lederNavnChanged = false;
			this.lederAdBrugerNavnChanged = false;
		} // Save
		#endregion

		#region Get methods.
		/// <summary>
		/// Gets the parent organisation associated with this organisation.
		/// </summary>
		/// <returns>The matching organisation.</returns>
		public SofdOrganization GetParentOrganisation() {
			// Get all matching organisations.
			List<SofdOrganization> organisations = this.framework.GetAllOrganizations(
				new SofdOrganizationFilter_LosOrganisationId(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.losForaelderOrganisationId),
				new SofdOrganizationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
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
		public List<SofdOrganization> GetSiblingOrganisations(Boolean removeThisOrganisation = false) {
			// Get all matching organisations.
			List<SofdOrganization> organisations = this.framework.GetAllOrganizations(
				new SofdOrganizationFilter_LosForaelderOrganisationId(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.losForaelderOrganisationId),
				new SofdOrganizationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
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
			List<SofdEmployee> employees = this.framework.GetAllEmployees(
				new SofdEmployeeFilter_MaNummer(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.LederMaNummer),
				new SofdOrganizationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
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
			List<SofdEmployee> employees = this.framework.GetAllEmployees(
				new SofdEmployeeFilter_OrganisationId(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.organisationId),
				new SofdOrganizationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
			);

			// Return the employees.
			return employees;
		} // GetEmployees

		#endregion

	} // SofdOrganization
	#endregion

	#region SofdOrganizationFilter_OrganisationHistorikId class
	/// <summary>
	/// Organisation filter on OrganisationHistorikId.
	/// </summary>
	public class SofdOrganizationFilter_OrganisationHistorikId : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on OrganisationHistorikId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganizationFilter_OrganisationHistorikId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganization.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdOrganizationFilter_OrganisationHistorikId

	} // SofdOrganizationFilter_OrganisationHistorikId
	#endregion

	#region SofdOrganizationFilter_OrganisationId class
	/// <summary>
	/// Organisation filter on OrganisationId.
	/// </summary>
	public class SofdOrganizationFilter_OrganisationId : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on OrganisationId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganizationFilter_OrganisationId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganization.FIELD_ORGANISATION_ID, filterValueOperator, filterValue) {
		} // SofdOrganizationFilter_OrganisationId

	} // SofdOrganizationFilter_OrganisationId
	#endregion

	#region SofdOrganizationFilter_Aktiv class
	/// <summary>
	/// Organisation filter on Aktiv.
	/// </summary>
	public class SofdOrganizationFilter_Aktiv : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on Aktiv.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganizationFilter_Aktiv(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Boolean filterValue) : base(filterOperator, SofdOrganization.FIELD_AKTIV, filterValueOperator, ((filterValue == true) ? 1 : 0)) {
		} // SofdOrganizationFilter_Aktiv

	} // SofdOrganizationFilter_Aktiv
	#endregion

	#region SofdOrganizationFilter_LosOrganisationId class
	/// <summary>
	/// Organisation filter on LosOrganisationId.
	/// </summary>
	public class SofdOrganizationFilter_LosOrganisationId : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on LosOrganisationId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganizationFilter_LosOrganisationId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganization.FIELD_LOS_ORGANISATION_ID, filterValueOperator, filterValue) {
		} // SofdOrganizationFilter_LosOrganisationId

	} // SofdOrganizationFilter_LosOrganisationId
	#endregion

	#region SofdOrganizationFilter_LosForaelderOrganisationId class
	/// <summary>
	/// Organisation filter on LosForaelderOrganisationId.
	/// </summary>
	public class SofdOrganizationFilter_LosForaelderOrganisationId : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on LosForaelderOrganisationId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganizationFilter_LosForaelderOrganisationId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganization.FIELD_PARENT_LOS_ORGANISATION_ID, filterValueOperator, filterValue) {
		} // SofdOrganizationFilter_LosForaelderOrganisationId

	} // SofdOrganizationFilter_LosForaelderOrganisationId
	#endregion

	#region SofdOrganizationFilter_CvrNumber class
	/// <summary>
	/// Organisation filter on CvrNumber.
	/// </summary>
	public class SofdOrganizationFilter_CvrNumber : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on CvrNumber.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganizationFilter_CvrNumber(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganization.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdOrganizationFilter_CvrNumber

	} // SofdOrganizationFilter_CvrNumber
	#endregion

	#region SofdOrganizationFilter_SeNumber class
	/// <summary>
	/// Organisation filter on SeNumber.
	/// </summary>
	public class SofdOrganizationFilter_SeNumber : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on SeNumber.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganizationFilter_SeNumber(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganization.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdOrganizationFilter_SeNumber

	} // SofdOrganizationFilter_SeNumber
	#endregion

	#region SofdOrganizationFilter_EanNumber class
	/// <summary>
	/// Organisation filter on EanNumber.
	/// </summary>
	public class SofdOrganizationFilter_EanNumber : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on EanNumber.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganizationFilter_EanNumber(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganization.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdOrganizationFilter_EanNumber

	} // SofdOrganizationFilter_EanNumber
	#endregion

	#region SofdOrganizationFilter_PNumber class
	/// <summary>
	/// Organisation filter on PNumber.
	/// </summary>
	public class SofdOrganizationFilter_PNumber : SqlWhereFilterInt32 {

		/// <summary>
		/// Organisation filter on PNumber.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganizationFilter_PNumber(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdOrganization.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdOrganizationFilter_PNumber

	} // SofdOrganizationFilter_PNumber
	#endregion

	#region SofdOrganizationFilter_Uuid class
	/// <summary>
	/// Organisation filter on Uuid.
	/// </summary>
	public class SofdOrganizationFilter_Uuid : SqlWhereFilterString {

		/// <summary>
		/// Organisation filter on Uuid.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdOrganizationFilter_Uuid(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Guid filterValue) : base(filterOperator, SofdOrganization.FIELD_ORGANISATION_HISTORIK_ID, filterValueOperator, filterValue.ToString()) {
		} // SofdOrganizationFilter_Uuid

	} // SofdOrganizationFilter_Uuid
	#endregion

} // NDK.Framework