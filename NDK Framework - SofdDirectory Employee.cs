using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace NDK.Framework {

	#region SofdEmployee class.
	public class SofdEmployee : IEqualityComparer<SofdEmployee>, IEquatable<SofdEmployee>, IComparable {
		private IFramework framework;

		#region Field name constants.
		public const String SCHEMA_NAME = "etl";
		public const String TABLE_NAME = "Medarbejder";
		public const String FIELD_MEDARBEJDER_HISTORIK_ID = "MedarbejderHistorikId";
		public const String FIELD_MEDARBEJDER_ID = "MedarbejderId";
		public const String FIELD_AKTIV = "Aktiv";
		public const String FIELD_SENEST_AKTIV = "SenestAktiv";
		public const String FIELD_AKTIV_FRA = "AktivFra";
		public const String FIELD_AKTIV_TIL = "AktivTil";
		public const String FIELD_FOERSTE_ANSAETTELSES_DATO = "FoersteAnsaettelsesdato";
		public const String FIELD_ANSAETTELSES_DATO = "Ansaettelsesdato";
		public const String FIELD_ANSAETTELSES_OPHOERS_DATO = "Ophoersdato";
		public const String FIELD_JUBILAEUMS_ANCIENNITETS_DATO = "Jubilaeumsanciennitetsdato";
		public const String FIELD_ANSAETTELSE_AKTIV = "AnsaettelseAktiv";
		public const String FIELD_SIDST_AENDRET = "SidstAendret";
		public const String FIELD_OPUS_SIDSST_AENDRET = "OPUSSidsstAendret";
		public const String FIELD_OPUS_BRUGER_NAVN = "OPUSBrugerNavn";
		public const String FIELD_AD_BRUGER_NAVN = "ADBrugerNavn";
		public const String FIELD_MA_NUMMER = "MaNr";
		public const String FIELD_CPR_NUMMER = "CPR";
		public const String FIELD_CPR_EKSTRA_CIFFER = "EkstraCiffer";
		public const String FIELD_FOR_NAVN = "Fornavn";
		public const String FIELD_EFTER_NAVN = "Efternavn";
		public const String FIELD_NAVN = "Navn";
		public const String FIELD_KALDE_NAVN = "Kaldenavn";
		public const String FIELD_ADRESSE = "Adresse";
		public const String FIELD_STED_NAVN = "Stednavn";
		public const String FIELD_POST_NUMMER = "PostNr";
		public const String FIELD_BY = "By";
		public const String FIELD_LAND = "Land";
		public const String FIELD_ADRESSE_BESKYTTET = "AdresseBeskyttet";
		public const String FIELD_TELEFON_NUMMER = "Telefonnummer";
		public const String FIELD_MOBIL_NUMMER = "Mobilnummer";
		public const String FIELD_MOBIL_NUMMER2 = "Mobilnummer2";
		public const String FIELD_EPOST = "Email";
		public const String FIELD_AFDELINGS_NUMMER = "Afdelingsnummer";
		public const String FIELD_GRUPPE_NUMMER = "Gruppenummer";
		public const String FIELD_STILLINGS_ID = "StillingsId";
		public const String FIELD_STILLINGS_BETEGNELSE = "Stillingsbetegnelse";
		public const String FIELD_LOS_ORGANISATION_ID = "LOSOrgId";
		public const String FIELD_ORGANISATION_ID = "OrganisationId";
		public const String FIELD_ORGANISATION_KORT_NAVN = "OrganisationKortNavn";
		public const String FIELD_ORGANISATION_NAVN = "OrganisationNavn";
		public const String FIELD_ANSAT_FORHOLD = "AnsatForhold";
		public const String FIELD_ANSAT_FORHOLD_TEXT = "AnsatForholdText";
		public const String FIELD_LOEN_KLASSE = "LoenKlasse";
		public const String FIELD_ARBEJDS_TID_TAELLER = "ArbejdstidTaeller";
		public const String FIELD_ARBEJDS_TID_NAEVNER = "ArbejdstidNaevner";
		public const String FIELD_LEDER = "Leder";
		public const String FIELD_SR = "SR";
		public const String FIELD_TR = "TR";
		public const String FIELD_TR_SUPPLEANT = "TRSuppleant";
		public const String FIELD_MED_UDVALG = "MedUdvalg";
		public const String FIELD_INTERN = "Intern";
		public const String FIELD_EKSTERN = "Ekstern";
		public const String FIELD_UUID = "UUID";
		public const String FIELD_MIFARE_ID = "MifareId";
		public const String FIELD_P_NUMMER = "PNr";
		public const String FIELD_FAKTURA_GODKENDER = "Fakturagodkender";
		public const String FIELD_FAKTURA_GODKENDER_NIVEAU1 = "FakturagodkenderNiveau1";
		public const String FIELD_FAKTURA_GODKENDER_NIVEAU1_BESKRIVELSE = "FakturagodkenderNiveau1Beskrivelse";
		public const String FIELD_FAKTURA_GODKENDER_NIVEAU2 = "FakturagodkenderNiveau2";
		public const String FIELD_FAKTURA_GODKENDER_NIVEAU2_BESKRIVELSE = "FakturagodkenderNiveau2Beskrivelse";
		public const String FIELD_NAERMESTE_LEDER_MA_NUMMER = "NaermesteLederMaNr";
		public const String FIELD_NAERMESTE_LEDER_CPR_NUMMER = "NaermesteLederCPR";
		public const String FIELD_NAERMESTE_LEDER_NAVN = "NaermesteLederNavn";
		public const String FIELD_NAERMESTE_LEDER_AD_BRUGER_NAVN = "NaermesteLederADBrugerNavn";
		#endregion

		#region Variables.
		private Int32 medarbejderHistorikId = -1;
		private Int32 medarbejderId = -1;
		private Boolean aktiv = false;
		private Boolean senestAktiv = false;
		private DateTime aktivFra = DateTime.MinValue;
		private DateTime aktivTil = DateTime.MinValue;
		private DateTime foersteAnsaettelsesDato = DateTime.MinValue;
		private DateTime ansaettelsesDato = DateTime.MinValue;
		private DateTime ansaettelsesOphoersDato = DateTime.MinValue;
		private DateTime jubilaeumsAnciennitetsDato = DateTime.MinValue;
		private Boolean ansaettelseAktiv = false;
		private DateTime sidstAendret = DateTime.MinValue;
		private DateTime opusSidsstAendret = DateTime.MinValue;
		private String opusBrugerNavn = String.Empty;
		private String adBrugerNavn = String.Empty;
		private Int32 maNummer = -1;
		private String cprNummer = String.Empty;
		private Int16 cprEkstraCiffer = 0;
		private String forNavn = String.Empty;
		private String efterNavn = String.Empty;
		private String navn = String.Empty;
		private String kaldeNavn = String.Empty;
		private String adresse = String.Empty;
		private String stedNavn = String.Empty;
		private String postNummer = String.Empty;
		private String by = String.Empty;
		private String land = String.Empty;
		private Boolean adresseBeskyttet = false;
		private String telefonNummer = String.Empty;
		private String mobilNummer = String.Empty;
		private String mobilNummer2 = String.Empty;
		private String epost = String.Empty;
		private String afdelingsNummer = String.Empty;
		private String gruppeNummer = String.Empty;
		private Int32 stillingsId = -1;
		private String stillingsBetegnelse = String.Empty;
		private Int32 losOrganisationId = -1;
		private Int32 organisationId = -1;
		private String organisationKortNavn = String.Empty;
		private String organisationNavn = String.Empty;
		private String ansatForhold = String.Empty;
		private String ansatForholdText = String.Empty;
		private String loenKlasse = String.Empty;
		private Decimal arbejdstidTaeller = 0.0M;
		private Decimal arbejdstidNaevner = 0.0M;
		private Boolean leder = false;
		private Boolean sikkerhedsRepresentant = false;
		private Boolean tillidsRepresentant = false;
		private Boolean tillidsRepresentantSuppleant = false;
		private Boolean medUdvalg = false;
		private Boolean intern = false;
		private Boolean ekstern = false;
		private Guid uuid = Guid.Empty;
		private String mifareId = String.Empty;
		private Int32 pNummer = -1;
		private Boolean fakturaGodkender = false;
		private String fakturaGodkenderNiveau1 = String.Empty;
		private String fakturaGodkenderNiveau1Beskrivelse = String.Empty;
		private String fakturaGodkenderNiveau2 = String.Empty;
		private String fakturaGodkenderNiveau2Beskrivelse = String.Empty;
		private Int32 naermesteLederMaNummer = -1;
		private String naermesteLederCprNummer = String.Empty;
		private String naermesteLederNavn = String.Empty;
		private String naermesteLederAdBrugerNavn = String.Empty;
		#endregion

		#region Constructor methods.
		/// <summary>
		/// Instantiates a new SOFD employee object, and initializes with values from the current
		/// record in the data reader.
		/// </summary>
		/// <param name="dbReader">The data reader.</param>
		/// <param name="sofdDirectory">The SOFD directory.</param>
		public SofdEmployee(IFramework framework, IDataReader dbReader) {
			this.framework = framework;

			this.medarbejderHistorikId = dbReader.GetInt32(SofdEmployee.FIELD_MEDARBEJDER_HISTORIK_ID);
			this.medarbejderId = dbReader.GetInt32(SofdEmployee.FIELD_MEDARBEJDER_ID);
			this.aktiv = dbReader.GetBoolean(SofdEmployee.FIELD_AKTIV);
			this.senestAktiv = dbReader.GetBoolean(SofdEmployee.FIELD_SENEST_AKTIV);
			this.aktivFra = dbReader.GetDateTime(SofdEmployee.FIELD_AKTIV_FRA);
			this.aktivTil = dbReader.GetDateTime(SofdEmployee.FIELD_AKTIV_TIL);
			this.foersteAnsaettelsesDato = dbReader.GetDateTime(SofdEmployee.FIELD_FOERSTE_ANSAETTELSES_DATO);
			this.ansaettelsesDato = dbReader.GetDateTime(SofdEmployee.FIELD_ANSAETTELSES_DATO);
			this.ansaettelsesOphoersDato = dbReader.GetDateTime(SofdEmployee.FIELD_ANSAETTELSES_OPHOERS_DATO);
			this.jubilaeumsAnciennitetsDato = dbReader.GetDateTime(SofdEmployee.FIELD_JUBILAEUMS_ANCIENNITETS_DATO);
			this.ansaettelseAktiv = dbReader.GetBoolean(SofdEmployee.FIELD_ANSAETTELSE_AKTIV);
			this.sidstAendret = dbReader.GetDateTime(SofdEmployee.FIELD_SIDST_AENDRET);
			this.opusSidsstAendret = dbReader.GetDateTime(SofdEmployee.FIELD_OPUS_SIDSST_AENDRET);
			this.opusBrugerNavn = dbReader.GetString(SofdEmployee.FIELD_OPUS_BRUGER_NAVN);
			this.adBrugerNavn = dbReader.GetString(SofdEmployee.FIELD_AD_BRUGER_NAVN);
			this.maNummer = dbReader.GetInt32(SofdEmployee.FIELD_MA_NUMMER);
			this.cprNummer = dbReader.GetString(SofdEmployee.FIELD_CPR_NUMMER);
			this.cprEkstraCiffer = dbReader.GetInt16(SofdEmployee.FIELD_CPR_EKSTRA_CIFFER);
			this.forNavn = dbReader.GetString(SofdEmployee.FIELD_FOR_NAVN);
			this.efterNavn = dbReader.GetString(SofdEmployee.FIELD_EFTER_NAVN);
			this.navn = dbReader.GetString(SofdEmployee.FIELD_NAVN);
			this.kaldeNavn = dbReader.GetString(SofdEmployee.FIELD_KALDE_NAVN);
			this.adresse = dbReader.GetString(SofdEmployee.FIELD_ADRESSE);
			this.stedNavn = dbReader.GetString(SofdEmployee.FIELD_STED_NAVN);
			this.postNummer = dbReader.GetString(SofdEmployee.FIELD_POST_NUMMER);
			this.by = dbReader.GetString(SofdEmployee.FIELD_BY);
			this.land = dbReader.GetString(SofdEmployee.FIELD_LAND);
			this.adresseBeskyttet = dbReader.GetBoolean(SofdEmployee.FIELD_ADRESSE_BESKYTTET);
			this.telefonNummer = dbReader.GetString(SofdEmployee.FIELD_TELEFON_NUMMER);
			this.mobilNummer = dbReader.GetString(SofdEmployee.FIELD_MOBIL_NUMMER);
			this.mobilNummer2 = dbReader.GetString(SofdEmployee.FIELD_MOBIL_NUMMER2);
			this.epost = dbReader.GetString(SofdEmployee.FIELD_EPOST);
			this.afdelingsNummer = dbReader.GetString(SofdEmployee.FIELD_AFDELINGS_NUMMER);
			this.gruppeNummer = dbReader.GetString(SofdEmployee.FIELD_GRUPPE_NUMMER);
			this.stillingsId = dbReader.GetInt32(SofdEmployee.FIELD_STILLINGS_ID);
			this.stillingsBetegnelse = dbReader.GetString(SofdEmployee.FIELD_STILLINGS_BETEGNELSE);
			this.losOrganisationId = dbReader.GetInt32(SofdEmployee.FIELD_LOS_ORGANISATION_ID);
			this.organisationId = dbReader.GetInt32(SofdEmployee.FIELD_ORGANISATION_ID);
			this.organisationKortNavn = dbReader.GetString(SofdEmployee.FIELD_ORGANISATION_KORT_NAVN);
			this.organisationNavn = dbReader.GetString(SofdEmployee.FIELD_ORGANISATION_NAVN);
			this.ansatForhold = dbReader.GetString(SofdEmployee.FIELD_ANSAT_FORHOLD);
			this.ansatForholdText = dbReader.GetString(SofdEmployee.FIELD_ANSAT_FORHOLD_TEXT);
			this.loenKlasse = dbReader.GetString(SofdEmployee.FIELD_LOEN_KLASSE);
			this.arbejdstidTaeller = dbReader.GetDecimal(SofdEmployee.FIELD_ARBEJDS_TID_TAELLER);
			this.arbejdstidNaevner = dbReader.GetDecimal(SofdEmployee.FIELD_ARBEJDS_TID_NAEVNER);
			this.leder = dbReader.GetBoolean(SofdEmployee.FIELD_LEDER);
			this.sikkerhedsRepresentant = dbReader.GetBoolean(SofdEmployee.FIELD_SR);
			this.tillidsRepresentant = dbReader.GetBoolean(SofdEmployee.FIELD_TR);
			this.tillidsRepresentantSuppleant = dbReader.GetBoolean(SofdEmployee.FIELD_TR_SUPPLEANT);
			this.medUdvalg = dbReader.GetBoolean(SofdEmployee.FIELD_MED_UDVALG);
			this.intern = dbReader.GetBoolean(SofdEmployee.FIELD_INTERN);
			this.ekstern = dbReader.GetBoolean(SofdEmployee.FIELD_EKSTERN);
			this.uuid = dbReader.GetGuid(SofdEmployee.FIELD_UUID);
			this.mifareId = dbReader.GetString(SofdEmployee.FIELD_MIFARE_ID);
			this.pNummer = dbReader.GetInt32(SofdEmployee.FIELD_P_NUMMER);
			this.fakturaGodkender = dbReader.GetBoolean(SofdEmployee.FIELD_FAKTURA_GODKENDER);
			this.fakturaGodkenderNiveau1 = dbReader.GetString(SofdEmployee.FIELD_FAKTURA_GODKENDER_NIVEAU1);
			this.fakturaGodkenderNiveau1Beskrivelse = dbReader.GetString(SofdEmployee.FIELD_FAKTURA_GODKENDER_NIVEAU1_BESKRIVELSE);
			this.fakturaGodkenderNiveau2 = dbReader.GetString(SofdEmployee.FIELD_FAKTURA_GODKENDER_NIVEAU2);
			this.fakturaGodkenderNiveau2Beskrivelse = dbReader.GetString(SofdEmployee.FIELD_FAKTURA_GODKENDER_NIVEAU2_BESKRIVELSE);
			this.naermesteLederMaNummer = dbReader.GetInt32(SofdEmployee.FIELD_NAERMESTE_LEDER_MA_NUMMER);
			this.naermesteLederCprNummer = dbReader.GetString(SofdEmployee.FIELD_NAERMESTE_LEDER_CPR_NUMMER);
			this.naermesteLederNavn = dbReader.GetString(SofdEmployee.FIELD_NAERMESTE_LEDER_NAVN);
			this.naermesteLederAdBrugerNavn = dbReader.GetString(SofdEmployee.FIELD_NAERMESTE_LEDER_AD_BRUGER_NAVN);
		} // SofdEmployee
		#endregion

		#region Compare and Equal methods.
		/// <summary>
		/// Gets true if the two instances are equal.
		/// The medarbejderHistorikId is used for comparison.
		/// </summary>
		/// <param name="x">The X instance.</param>
		/// <param name="y">The Y instance.</param>
		/// <returns>True if equal.</returns>
		public Boolean Equals(SofdEmployee x, SofdEmployee y) {
			return x.medarbejderHistorikId.Equals(y.medarbejderHistorikId);
		} // Equals

		/// <summary>
		/// Gets true if the given instance is equal to this instance.
		/// The medarbejderHistorikId is used for comparison.
		/// </summary>
		/// <param name="other">The other instance.</param>
		/// <returns>True if equal.</returns>
		public Boolean Equals(SofdEmployee other) {
			return this.medarbejderHistorikId.Equals(other.medarbejderHistorikId);
		} // Equals

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates
		/// whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// 
		/// The medarbejderHistorikId is used for comparison.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns>Less then zero, zero or more then zero to indicate sort order.</returns>
		public Int32 CompareTo(Object obj) {
			if ((obj == null) || (this.GetType() != obj.GetType())) {
				// The objects are not the same type.
				return 1;
			} else {
				// Compare.
				return this.medarbejderHistorikId.CompareTo(((SofdEmployee)obj).medarbejderHistorikId);
			}
		} // CompareTo

		/// <summary>
		/// Gets true if the given instance is equal to this instance.
		/// The medarbejderHistorikId is used for comparison.
		/// </summary>
		/// <param name="other">The other instance.</param>
		/// <returns>True if equal.</returns>
		public override Boolean Equals(Object obj) {
			if ((obj == null) || (this.GetType() != obj.GetType())) {
				return false;
			} else {
				// Compare.
				return this.medarbejderHistorikId.Equals(((SofdEmployee)obj).medarbejderHistorikId);
			}
		} // Equals

		/// <summary>
		/// Gets the hash code from the MaNummer.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>The hash code.</returns>
		public Int32 GetHashCode(SofdEmployee obj) {
			return obj.medarbejderHistorikId.GetHashCode();
		} // GetHashCode

		/// <summary>
		/// Gets the hash code from the medarbejderHistorikId.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>The hash code.</returns>
		public override Int32 GetHashCode() {
			return this.medarbejderHistorikId.GetHashCode();
		} // GetHashCode
		#endregion

		#region ToString methods.
		public override String ToString() {
			return this.ToString(false, false);
		} // ToString

		public String ToString(Boolean addHeader, Boolean oneLine) {
			// Only oneLine == true is implemented.
			String textSeparator = "\t";
			StringBuilder text = new StringBuilder();

			// Add header.
			if (addHeader == true) {

				text.Append("medarbejderHistorikId");
				text.Append(textSeparator);
				text.Append("medarbejderId");
				text.Append(textSeparator);
				text.Append("aktiv");
				text.Append(textSeparator);
				text.Append("senestAktiv");
				text.Append(textSeparator);
				text.Append("aktivFra");
				text.Append(textSeparator);
				text.Append("aktivTil");
				text.Append(textSeparator);
				text.Append("foersteAnsaettelsesDato");
				text.Append(textSeparator);
				text.Append("ansaettelsesDato");
				text.Append(textSeparator);
				text.Append("ansaettelsesOphoersDato");
				text.Append(textSeparator);
				text.Append("jubilaeumsAnciennitetsDato");
				text.Append(textSeparator);
				text.Append("ansaettelseAktiv");
				text.Append(textSeparator);
				text.Append("sidstAendret");
				text.Append(textSeparator);
				text.Append("opusSidsstAendret");
				text.Append(textSeparator);
				text.Append("opusBrugerNavn");
				text.Append(textSeparator);
				text.Append("adBrugerNavn");
				text.Append(textSeparator);
				text.Append("maNummer");
				text.Append(textSeparator);
				text.Append("cprNummer");
				text.Append(textSeparator);
				text.Append("cprEkstraCiffer");
				text.Append(textSeparator);
				text.Append("forNavn");
				text.Append(textSeparator);
				text.Append("efterNavn");
				text.Append(textSeparator);
				text.Append("navn");
				text.Append(textSeparator);
				text.Append("kaldeNavn");
				text.Append(textSeparator);
				text.Append("adresse");
				text.Append(textSeparator);
				text.Append("stedNavn");
				text.Append(textSeparator);
				text.Append("postNummer");
				text.Append(textSeparator);
				text.Append("by");
				text.Append(textSeparator);
				text.Append("land");
				text.Append(textSeparator);
				text.Append("adresseBeskyttet");
				text.Append(textSeparator);
				text.Append("telefonNummer");
				text.Append(textSeparator);
				text.Append("mobilNummer");
				text.Append(textSeparator);
				text.Append("mobilNummer2");
				text.Append(textSeparator);
				text.Append("epost");
				text.Append(textSeparator);
				text.Append("afdelingsNummer");
				text.Append(textSeparator);
				text.Append("gruppeNummer");
				text.Append(textSeparator);
				text.Append("stillingsId");
				text.Append(textSeparator);
				text.Append("stillingsBetegnelse");
				text.Append(textSeparator);
				text.Append("losOrganisationId");
				text.Append(textSeparator);
				text.Append("organisationId");
				text.Append(textSeparator);
				text.Append("organisationKortNavn");
				text.Append(textSeparator);
				text.Append("organisationNavn");
				text.Append(textSeparator);
				text.Append("ansatForhold");
				text.Append(textSeparator);
				text.Append("ansatForholdText");
				text.Append(textSeparator);
				text.Append("loenKlasse");
				text.Append(textSeparator);
				text.Append("arbejdstidTaeller");
				text.Append(textSeparator);
				text.Append("arbejdstidNaevner");
				text.Append(textSeparator);
				text.Append("leder");
				text.Append(textSeparator);
				text.Append("sikkerhedsRepresentant");
				text.Append(textSeparator);
				text.Append("tillidsRepresentant");
				text.Append(textSeparator);
				text.Append("tillidsRepresentantSuppleant");
				text.Append(textSeparator);
				text.Append("medUdvalg");
				text.Append(textSeparator);
				text.Append("intern");
				text.Append(textSeparator);
				text.Append("ekstern");
				text.Append(textSeparator);
				text.Append("uuid");
				text.Append(textSeparator);
				text.Append("mifareId");
				text.Append(textSeparator);
				text.Append("pNummer");
				text.Append(textSeparator);
				text.Append("fakturaGodkender");
				text.Append(textSeparator);
				text.Append("fakturaGodkenderNiveau1");
				text.Append(textSeparator);
				text.Append("fakturaGodkenderNiveau1Beskrivelse");
				text.Append(textSeparator);
				text.Append("fakturaGodkenderNiveau2");
				text.Append(textSeparator);
				text.Append("fakturaGodkenderNiveau2Beskrivelse");
				text.Append(textSeparator);
				text.Append("naermesteLederMaNummer");
				text.Append(textSeparator);
				text.Append("naermesteLederCprNummer");
				text.Append(textSeparator);
				text.Append("naermesteLederNavn");
				text.Append(textSeparator);
				text.Append("naermesteLederAdBrugerNavn");
				text.AppendLine();
			}

			// Add data.
			text.Append(this.medarbejderHistorikId);
			text.Append(textSeparator);
			text.Append(this.medarbejderId);
			text.Append(textSeparator);
			text.Append(this.aktiv);
			text.Append(textSeparator);
			text.Append(this.senestAktiv);
			text.Append(textSeparator);
			text.Append(this.aktivFra);
			text.Append(textSeparator);
			text.Append(this.aktivTil);
			text.Append(textSeparator);
			text.Append(this.foersteAnsaettelsesDato);
			text.Append(textSeparator);
			text.Append(this.ansaettelsesDato);
			text.Append(textSeparator);
			text.Append(this.ansaettelsesOphoersDato);
			text.Append(textSeparator);
			text.Append(this.jubilaeumsAnciennitetsDato);
			text.Append(textSeparator);
			text.Append(this.ansaettelseAktiv);
			text.Append(textSeparator);
			text.Append(this.sidstAendret);
			text.Append(textSeparator);
			text.Append(this.opusSidsstAendret);
			text.Append(textSeparator);
			text.Append(this.opusBrugerNavn);
			text.Append(textSeparator);
			text.Append(this.adBrugerNavn);
			text.Append(textSeparator);
			text.Append(this.maNummer);
			text.Append(textSeparator);
			text.Append(this.cprNummer);
			text.Append(textSeparator);
			text.Append(this.cprEkstraCiffer);
			text.Append(textSeparator);
			text.Append(this.forNavn);
			text.Append(textSeparator);
			text.Append(this.efterNavn);
			text.Append(textSeparator);
			text.Append(this.navn);
			text.Append(textSeparator);
			text.Append(this.kaldeNavn);
			text.Append(textSeparator);
			text.Append(this.adresse);
			text.Append(textSeparator);
			text.Append(this.stedNavn);
			text.Append(textSeparator);
			text.Append(this.postNummer);
			text.Append(textSeparator);
			text.Append(this.by);
			text.Append(textSeparator);
			text.Append(this.land);
			text.Append(textSeparator);
			text.Append(this.adresseBeskyttet);
			text.Append(textSeparator);
			text.Append(this.telefonNummer);
			text.Append(textSeparator);
			text.Append(this.mobilNummer);
			text.Append(textSeparator);
			text.Append(this.mobilNummer2);
			text.Append(textSeparator);
			text.Append(this.epost);
			text.Append(textSeparator);
			text.Append(this.afdelingsNummer);
			text.Append(textSeparator);
			text.Append(this.gruppeNummer);
			text.Append(textSeparator);
			text.Append(this.stillingsId);
			text.Append(textSeparator);
			text.Append(this.stillingsBetegnelse);
			text.Append(textSeparator);
			text.Append(this.losOrganisationId);
			text.Append(textSeparator);
			text.Append(this.organisationId);
			text.Append(textSeparator);
			text.Append(this.organisationKortNavn);
			text.Append(textSeparator);
			text.Append(this.organisationNavn);
			text.Append(textSeparator);
			text.Append(this.ansatForhold);
			text.Append(textSeparator);
			text.Append(this.ansatForholdText);
			text.Append(textSeparator);
			text.Append(this.loenKlasse);
			text.Append(textSeparator);
			text.Append(this.arbejdstidTaeller);
			text.Append(textSeparator);
			text.Append(this.arbejdstidNaevner);
			text.Append(textSeparator);
			text.Append(this.leder);
			text.Append(textSeparator);
			text.Append(this.sikkerhedsRepresentant);
			text.Append(textSeparator);
			text.Append(this.tillidsRepresentant);
			text.Append(textSeparator);
			text.Append(this.tillidsRepresentantSuppleant);
			text.Append(textSeparator);
			text.Append(this.medUdvalg);
			text.Append(textSeparator);
			text.Append(this.intern);
			text.Append(textSeparator);
			text.Append(this.ekstern);
			text.Append(textSeparator);
			text.Append(this.uuid);
			text.Append(textSeparator);
			text.Append(this.mifareId);
			text.Append(textSeparator);
			text.Append(this.pNummer);
			text.Append(textSeparator);
			text.Append(this.fakturaGodkender);
			text.Append(textSeparator);
			text.Append(this.fakturaGodkenderNiveau1);
			text.Append(textSeparator);
			text.Append(this.fakturaGodkenderNiveau1Beskrivelse);
			text.Append(textSeparator);
			text.Append(this.fakturaGodkenderNiveau2);
			text.Append(textSeparator);
			text.Append(this.fakturaGodkenderNiveau2Beskrivelse);
			text.Append(textSeparator);
			text.Append(this.naermesteLederMaNummer);
			text.Append(textSeparator);
			text.Append(this.naermesteLederCprNummer);
			text.Append(textSeparator);
			text.Append(this.naermesteLederNavn);
			text.Append(textSeparator);
			text.Append(this.naermesteLederAdBrugerNavn);

			// Return the text.
			return text.ToString();
		} // ToString
		#endregion

		#region Properties.
		public Int32 MedarbejderHistorikId {
			get {
				return this.medarbejderHistorikId;
			}
		} // MedarbejderHistorikId

		public Int32 MedarbejderId {
			get {
				return this.medarbejderId;
			}
		} // MedarbejderId

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

		public DateTime FoersteAnsaettelsesDato {
			get {
				return this.foersteAnsaettelsesDato;
			}
		} // FoersteAnsaettelsesDato

		public DateTime AnsaettelsesDato {
			get {
				return this.ansaettelsesDato;
			}
		} // AnsaettelsesDato

		public DateTime AnsaettelsesOphoersDato {
			get {
				return this.ansaettelsesOphoersDato;
			}
		} // AnsaettelsesOphoersDato

		public DateTime JubilaeumsAnciennitetsDato {
			get {
				return this.jubilaeumsAnciennitetsDato;
			}
		} // JubilaeumsAnciennitetsDato

		public Boolean AnsaettelseAktiv {
			get {
				return this.ansaettelseAktiv;
			}
		} // AnsaettelseAktiv

		public DateTime SidstAendret {
			get {
				return this.sidstAendret;
			}
		} // SidstAendret

		public DateTime OpusSidsstAendret {
			get {
				return this.opusSidsstAendret;
			}
		} // OpusSidsstAendret

		public String OpusBrugerNavn {
			get {
				return this.opusBrugerNavn;
			}
		} // OpusBrugerNavn

		public String AdBrugerNavn {
			get {
				return this.adBrugerNavn;
			}
		} // AdBrugerNavn

		public Int32 MaNummer {
			get {
				return this.maNummer;
			}
		} // MaNummer

		public String CprNummer {
			get {
				return this.cprNummer;
			}
		} // CprNummer

		public Int16 CprEkstraCiffer {
			get {
				return this.cprEkstraCiffer;
			}
		} // CprEkstraCiffer

		public String ForNavn {
			get {
				return this.forNavn;
			}
		} // ForNavn

		public String EfterNavn {
			get {
				return this.efterNavn;
			}
		} // EfterNavn

		public String Navn {
			get {
				return this.navn;
			}
		} // Navn

		public String KaldeNavn {
			get {
				return this.kaldeNavn;
			}
		} // KaldeNavn

		public String AdresseText {
			get {
				String resultAddress = this.adresse ?? String.Empty;
				String resultPlace = this.stedNavn ?? String.Empty;
				String resultPostNumber = this.postNummer ?? String.Empty;
				String resultCity = this.by ?? String.Empty;
				String resultCountry = this.land ?? String.Empty;

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

		public String Adresse {
			get {
				return this.adresse;
			}
		} // Adresse

		public String StedNavn {
			get {
				return this.stedNavn;
			}
		} // StedNavn

		public String PostNummer {
			get {
				return this.postNummer;
			}
		} // PostNummer

		public String By {
			get {
				return this.by;
			}
		} // By

		public String Land {
			get {
				return this.land;
			}
		} // Land

		public Boolean AdresseBeskyttet {
			get {
				return this.adresseBeskyttet;
			}
		} // AdresseBeskyttet

		public String TelefonNummer {
			get {
				return this.telefonNummer;
			}
		} // TelefonNummer

		public String MobilNummer {
			get {
				return this.mobilNummer;
			}
		} // MobilNummer

		public String MobilNummer2 {
			get {
				return this.mobilNummer2;
			}
		} // MobilNummer2

		public String Epost {
			get {
				return this.epost;
			}
		} // Epost

		public String AfdelingsNummer {
			get {
				return this.afdelingsNummer;
			}
		} // AfdelingsNummer

		public String GruppeNummer {
			get {
				return this.gruppeNummer;
			}
		} // GruppeNummer

		public Int32 StillingsId {
			get {
				return this.stillingsId;
			}
		} // StillingsId

		public String StillingsBetegnelse {
			get {
				return this.stillingsBetegnelse;
			}
		} // StillingsBetegnelse

		public Int32 LosOrganisationId {
			get {
				return this.losOrganisationId;
			}
		} // LosOrganisationId

		public Int32 OrganisationId {
			get {
				return this.organisationId;
			}
		} // OrganisationId

		public String OrganisationKortNavn {
			get {
				return this.organisationKortNavn;
			}
		} // OrganisationKortNavn

		public String OrganisationNavn {
			get {
				return this.organisationNavn;
			}
		} // OrganisationNavn

		public String AnsatForhold {
			get {
				return this.ansatForhold;
			}
		} // AnsatForhold

		public String AnsatForholdText {
			get {
				return this.ansatForholdText;
			}
		} // AnsatForholdText

		public String LoenKlasse {
			get {
				return this.loenKlasse;
			}
		} // LoenKlasse

		public Decimal ArbejdstidTaeller {
			get {
				return this.arbejdstidTaeller;
			}
		} // ArbejdstidTaeller

		public Decimal ArbejdstidNaevner {
			get {
				return this.arbejdstidNaevner;
			}
		} // ArbejdstidNaevner

		public Boolean Leder {
			get {
				return this.leder;
			}
		} // Leder

		public Boolean SikkerhedsRepresentant {
			get {
				return this.sikkerhedsRepresentant;
			}
		} // SikkerhedsRepresentant

		public Boolean TillidsRepresentant {
			get {
				return this.tillidsRepresentant;
			}
		} // TillidsRepresentant

		public Boolean TillidsRepresentantSuppleant {
			get {
				return this.tillidsRepresentantSuppleant;
			}
		} // TillidsRepresentantSuppleant

		public Boolean MedUdvalg {
			get {
				return this.medUdvalg;
			}
		} // MedUdvalg

		public Boolean Intern {
			get {
				return this.intern;
			}
		} // Intern

		public Boolean Ekstern {
			get {
				return this.ekstern;
			}
		} // Ekstern

		public Guid Uuid {
			get {
				return this.uuid;
			}
		} // Uuid

		public String MifareId {
			get {
				return this.mifareId;
			}
		} // MifareId

		public Int32 PNummer {
			get {
				return this.pNummer;
			}
		} // PNummer

		public Boolean FakturaGodkender {
			get {
				return this.fakturaGodkender;
			}
		} // FakturaGodkender

		public String FakturaGodkenderNiveau1 {
			get {
				return this.fakturaGodkenderNiveau1;
			}
		} // FakturaGodkenderNiveau1

		public String FakturaGodkenderNiveau1Beskrivelse {
			get {
				return this.fakturaGodkenderNiveau1Beskrivelse;
			}
		} // FakturaGodkenderNiveau1Beskrivelse

		public String FakturaGodkenderNiveau2 {
			get {
				return this.fakturaGodkenderNiveau2;
			}
		} // FakturaGodkenderNiveau2

		public String FakturaGodkenderNiveau2Beskrivelse {
			get {
				return this.fakturaGodkenderNiveau2Beskrivelse;
			}
		} // FakturaGodkenderNiveau2Beskrivelse

		public Int32 NaermesteLederMaNummer {
			get {
				return this.naermesteLederMaNummer;
			}
		} // NaermesteLederMaNummer

		public String NaermesteLederCprNummer {
			get {
				return this.naermesteLederCprNummer;
			}
		} // NaermesteLederCprNummer

		public String NaermesteLederNavn {
			get {
				return this.naermesteLederNavn;
			}
		} // NaermesteLederNavn

		public String NaermesteLederAdBrugerNavn {
			get {
				return this.naermesteLederAdBrugerNavn;
			}
		} // NaermesteLederAdBrugerNavn
		#endregion

		#region Get methods.
		/// <summary>
		/// Gets the nearest leader associated with this employee.
		/// </summary>
		/// <returns>The matching employee.</returns>
		public SofdEmployee GetNearestLeader() {
			// Get all matching employees.
			List<SofdEmployee> employees = this.framework.GetAllEmployees(
				new SofdEmployeeFilter_MaNummer(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.naermesteLederMaNummer),
				new SofdOrganizationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
			);

			// Return the employee, only if one matched the filters.
			if (employees.Count == 1) {
				return employees[0];
			} else {
				// Return null.
				return null;
			}
		} // GetNearestLeader

		/// <summary>
		/// Gets the employees associated with this employee as nearest leader.
		/// </summary>
		/// <returns>The matching employees.</returns>
		public List<SofdEmployee> GetWorkers() {
			// Get all matching employees.
			List<SofdEmployee> employees = this.framework.GetAllEmployees(
				new SofdEmployeeFilter_NaermesteLederMaNummer(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.maNummer),
				new SofdOrganizationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
			);

			// Return the employees.
			return employees;
		} // GetWorkers

		/// <summary>
		/// Gets the employees associated with the same nearest leader as this employee.
		/// </summary>
		/// <param name="removeThisEmployee">True to remove this employee from the result.</param>
		/// <returns>The matching employees.</returns>
		public List<SofdEmployee> GetEmployeeWithSameNearestLeader(Boolean removeThisEmployee = false) {
			// Get all matching employees.
			List<SofdEmployee> employees = this.framework.GetAllEmployees(
				new SofdEmployeeFilter_NaermesteLederMaNummer(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.NaermesteLederMaNummer),
				new SofdOrganizationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
			);

			// Remove this employee from the result.
			// The Equals override, makes this work.
			if (removeThisEmployee == true) {
				employees.Remove(this);
			}

			// Return the employees.
			return employees;
		} // GetEmployeeWithSameNearestLeader

		/// <summary>
		/// Gets the organisation associated with this employee.
		/// </summary>
		/// <returns>The matching organisation.</returns>
		public SofdOrganization GetOrganisation() {
			// Get all matching organisations.
			List<SofdOrganization> organisations = this.framework.GetAllOrganizations(
				new SofdOrganizationFilter_OrganisationId(SqlWhereFilterOperator.OR, SqlWhereFilterValueOperator.Equals, this.organisationId),
				new SofdOrganizationFilter_Aktiv(SqlWhereFilterOperator.AND, SqlWhereFilterValueOperator.Equals, true)
			);

			// Return the organisation, only if one matched the filters.
			if (organisations.Count == 1) {
				return organisations[0];
			} else {
				// Return null.
				return null;
			}
		} // GetOrganisation
		#endregion

	} // SofdEmployee
	#endregion

	#region SofdEmployeeFilter_MedarbejderHistorikId class
	/// <summary>
	/// Employee filter on MedarbejderHistorikId.
	/// </summary>
	public class SofdEmployeeFilter_MedarbejderHistorikId : SqlWhereFilterInt32 {

		/// <summary>
		/// Employee filter on MedarbejderHistorikId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_MedarbejderHistorikId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdEmployee.FIELD_MEDARBEJDER_HISTORIK_ID, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_MedarbejderHistorikId

	} // SofdEmployeeFilter_MedarbejderHistorikId
	#endregion

	#region SofdEmployeeFilter_MedarbejderId class
	/// <summary>
	/// Employee filter on MedarbejderId.
	/// </summary>
	public class SofdEmployeeFilter_MedarbejderId : SqlWhereFilterInt32 {

		/// <summary>
		/// Employee filter on MedarbejderId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_MedarbejderId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdEmployee.FIELD_MEDARBEJDER_ID, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_MedarbejderId

	} // SofdEmployeeFilter_MedarbejderId
	#endregion

	#region SofdEmployeeFilter_Aktiv class
	/// <summary>
	/// Employee filter on Aktiv.
	/// </summary>
	public class SofdEmployeeFilter_Aktiv : SqlWhereFilterInt32 {

		/// <summary>
		/// Employee filter on Aktiv.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_Aktiv(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Boolean filterValue) : base(filterOperator, SofdEmployee.FIELD_AKTIV, filterValueOperator, ((filterValue == true) ? 1 : 0)) {
		} // SofdEmployeeFilter_Aktiv

	} // SofdEmployeeFilter_Aktiv
	#endregion

	#region SofdEmployeeFilter_MaNummer class
	/// <summary>
	/// Employee filter on MaNummer.
	/// </summary>
	public class SofdEmployeeFilter_MaNummer : SqlWhereFilterInt32 {

		/// <summary>
		/// Employee filter on MaNummer.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_MaNummer(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdEmployee.FIELD_MA_NUMMER, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_MaNummer

	} // SofdEmployeeFilter_MaNummer
	#endregion

	#region SofdEmployeeFilter_AktivFra class
	/// <summary>
	/// Employee filter on AktivFra.
	/// </summary>
	public class SofdEmployeeFilter_AktivFra : SqlWhereFilterDateTime {

		/// <summary>
		/// Employee filter on AktivFra.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_AktivFra(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, DateTime filterValue) : base(filterOperator, SofdEmployee.FIELD_AKTIV_FRA, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_AktivFra

	} // SofdEmployeeFilter_AktivFra
	#endregion

	#region SofdEmployeeFilter_AktivTil class
	/// <summary>
	/// Employee filter on AktivTil.
	/// </summary>
	public class SofdEmployeeFilter_AktivTil : SqlWhereFilterDateTime {

		/// <summary>
		/// Employee filter on AktivTil.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_AktivTil(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, DateTime filterValue) : base(filterOperator, SofdEmployee.FIELD_AKTIV_TIL, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_AktivTil

	} // SofdEmployeeFilter_AktivTil
	#endregion

	#region SofdEmployeeFilter_FoersteAnsaettelsesDato class
	/// <summary>
	/// Employee filter on FoersteAnsaettelsesDato.
	/// </summary>
	public class SofdEmployeeFilter_FoersteAnsaettelsesDato : SqlWhereFilterDateTime {

		/// <summary>
		/// Employee filter on FoersteAnsaettelsesDato.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_FoersteAnsaettelsesDato(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, DateTime filterValue) : base(filterOperator, SofdEmployee.FIELD_FOERSTE_ANSAETTELSES_DATO, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_FoersteAnsaettelsesDato

	} // SofdEmployeeFilter_FoersteAnsaettelsesDato
	#endregion

	#region SofdEmployeeFilter_AnsaettelsesDato class
	/// <summary>
	/// Employee filter on AnsaettelsesDato.
	/// </summary>
	public class SofdEmployeeFilter_AnsaettelsesDato : SqlWhereFilterDateTime {

		/// <summary>
		/// Employee filter on AnsaettelsesDato.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_AnsaettelsesDato(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, DateTime filterValue) : base(filterOperator, SofdEmployee.FIELD_ANSAETTELSES_DATO, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_AnsaettelsesDato

	} // SofdEmployeeFilter_AnsaettelsesDato
	#endregion

	#region SofdEmployeeFilter_AnsaettelsesOphoersDato class
	/// <summary>
	/// Employee filter on AnsaettelsesOphoersDato.
	/// </summary>
	public class SofdEmployeeFilter_AnsaettelsesOphoersDato : SqlWhereFilterDateTime {

		/// <summary>
		/// Employee filter on AnsaettelsesOphoersDato.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_AnsaettelsesOphoersDato(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, DateTime filterValue) : base(filterOperator, SofdEmployee.FIELD_ANSAETTELSES_OPHOERS_DATO, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_AnsaettelsesOphoersDato

	} // SofdEmployeeFilter_AnsaettelsesOphoersDato
	#endregion

	#region SofdEmployeeFilter_JubilaeumsAncinnitetsDato class
	/// <summary>
	/// Employee filter on JubilaeumsAncinnitetsDato.
	/// </summary>
	public class SofdEmployeeFilter_JubilaeumsAncinnitetsDato : SqlWhereFilterDateTime {

		/// <summary>
		/// Employee filter on JubilaeumsAncinnitetsDato.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_JubilaeumsAncinnitetsDato(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, DateTime filterValue) : base(filterOperator, SofdEmployee.FIELD_JUBILAEUMS_ANCIENNITETS_DATO, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_JubilaeumsAncinnitetsDato

	} // SofdEmployeeFilter_JubilaeumsAncinnitetsDato
	#endregion

	#region SofdEmployeeFilter_SidstAendret class
	/// <summary>
	/// Employee filter on SidstAendret.
	/// </summary>
	public class SofdEmployeeFilter_SidstAendret : SqlWhereFilterDateTime {

		/// <summary>
		/// Employee filter on SidstAendret.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_SidstAendret(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, DateTime filterValue) : base(filterOperator, SofdEmployee.FIELD_SIDST_AENDRET, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_SidstAendret

	} // SofdEmployeeFilter_SidstAendret
	#endregion

	#region SofdEmployeeFilter_OpusBrugerNavn class
	/// <summary>
	/// Employee filter on OpusBrugerNavn.
	/// </summary>
	public class SofdEmployeeFilter_OpusBrugerNavn : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on OpusBrugerNavn.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_OpusBrugerNavn(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_OPUS_BRUGER_NAVN, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_OpusBrugerNavn

	} // SofdEmployeeFilter_OpusBrugerNavn
	#endregion

	#region SofdEmployeeFilter_AdBrugerNavn class
	/// <summary>
	/// Employee filter on AdBrugerNavn.
	/// </summary>
	public class SofdEmployeeFilter_AdBrugerNavn : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on AdBrugerNavn.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_AdBrugerNavn(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_AD_BRUGER_NAVN, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_AdBrugerNavn

	} // SofdEmployeeFilter_AdBrugerNavn
	#endregion

	#region SofdEmployeeFilter_CprNummer class
	/// <summary>
	/// Employee filter on CprNummer.
	/// </summary>
	public class SofdEmployeeFilter_CprNummer : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on CprNummer.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_CprNummer(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_CPR_NUMMER, filterValueOperator, (filterValue == null) ? null : filterValue.Trim().Replace("-", "")) {
		} // SofdEmployeeFilter_CprNummer

	} // SofdEmployeeFilter_CprNummer
	#endregion

	#region SofdEmployeeFilter_Navn class
	/// <summary>
	/// Employee filter on Navn.
	/// </summary>
	public class SofdEmployeeFilter_Navn : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on Navn.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_Navn(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_NAVN, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_Navn

	} // SofdEmployeeFilter_Navn
	#endregion

	#region SofdEmployeeFilter_ForNavn class
	/// <summary>
	/// Employee filter on ForNavn.
	/// </summary>
	public class SofdEmployeeFilter_ForNavn : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on ForNavn.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_ForNavn(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_FOR_NAVN, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_ForNavn

	} // SofdEmployeeFilter_ForNavn
	#endregion

	#region SofdEmployeeFilter_EfterNavn class
	/// <summary>
	/// Employee filter on EfterNavn.
	/// </summary>
	public class SofdEmployeeFilter_EfterNavn : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on EfterNavn.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_EfterNavn(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_EFTER_NAVN, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_EfterNavn

	} // SofdEmployeeFilter_EfterNavn
	#endregion

	#region SofdEmployeeFilter_KaldeNavn class
	/// <summary>
	/// Employee filter on KaldeNavn.
	/// </summary>
	public class SofdEmployeeFilter_KaldeNavn : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on KaldeNavn.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_KaldeNavn(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_KALDE_NAVN, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_KaldeNavn

	} // SofdEmployeeFilter_KaldeNavn
	#endregion

	#region SofdEmployeeFilter_TelefonNummer class
	/// <summary>
	/// Employee filter on TelefonNummer.
	/// </summary>
	public class SofdEmployeeFilter_TelefonNummer : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on TelefonNummer.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_TelefonNummer(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_TELEFON_NUMMER, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_TelefonNummer

	} // SofdEmployeeFilter_Epost
	#endregion

	#region SofdEmployeeFilter_MobilNummer class
	/// <summary>
	/// Employee filter on MobilNummer.
	/// </summary>
	public class SofdEmployeeFilter_MobilNummer : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on MobilNummer.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_MobilNummer(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_MOBIL_NUMMER, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_MobilNummer

	} // SofdEmployeeFilter_MobilNummer
	#endregion

	#region SofdEmployeeFilter_MobilNummer2 class
	/// <summary>
	/// Employee filter on MobilNummer2.
	/// </summary>
	public class SofdEmployeeFilter_MobilNummer2 : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on MobilNummer2.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_MobilNummer2(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_MOBIL_NUMMER2, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_MobilNummer2

	} // SofdEmployeeFilter_MobilNummer2
	#endregion

	#region SofdEmployeeFilter_Epost class
	/// <summary>
	/// Employee filter on Epost.
	/// </summary>
	public class SofdEmployeeFilter_Epost : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on Epost.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_Epost(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_EPOST, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_Epost

	} // SofdEmployeeFilter_Epost
	#endregion

	#region SofdEmployeeFilter_StillingsId class
	/// <summary>
	/// Employee filter on StillingsId.
	/// </summary>
	public class SofdEmployeeFilter_StillingsId : SqlWhereFilterInt32 {

		/// <summary>
		/// Employee filter on StillingsId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_StillingsId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdEmployee.FIELD_STILLINGS_ID, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_StillingsId

	} // SofdEmployeeFilter_StillingsId
	#endregion

	#region SofdEmployeeFilter_StillingsBetegnelse class
	/// <summary>
	/// Employee filter on StillingsBetegnelse.
	/// </summary>
	public class SofdEmployeeFilter_StillingsBetegnelse : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on StillingsBetegnelse.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_StillingsBetegnelse(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_STILLINGS_BETEGNELSE, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_StillingsBetegnelse

	} // SofdEmployeeFilter_StillingsBetegnelse
	#endregion

	#region SofdEmployeeFilter_OrganisationId class
	/// <summary>
	/// Employee filter on OrganisationId.
	/// </summary>
	public class SofdEmployeeFilter_OrganisationId : SqlWhereFilterInt32 {

		/// <summary>
		/// Employee filter on OrganisationId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_OrganisationId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdEmployee.FIELD_ORGANISATION_ID, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_OrganisationId

	} // SofdEmployeeFilter_OrganisationId
	#endregion

	#region SofdEmployeeFilter_OrganisationNavn class
	/// <summary>
	/// Employee filter on OrganisationNavn.
	/// </summary>
	public class SofdEmployeeFilter_OrganisationNavn : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on OrganisationNavn.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_OrganisationNavn(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_ORGANISATION_NAVN, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_OrganisationNavn

	} // SofdEmployeeFilter_OrganisationNavn
	#endregion

	#region SofdEmployeeFilter_LoenKlasse class
	/// <summary>
	/// Employee filter on LoenKlasse.
	/// </summary>
	public class SofdEmployeeFilter_LoenKlasse : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on LoenKlasse.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_LoenKlasse(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_LOEN_KLASSE, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_LoenKlasse

	} // SofdEmployeeFilter_LoenKlasse
	#endregion

	#region SofdEmployeeFilter_Uuid class
	/// <summary>
	/// Employee filter on Uuid.
	/// </summary>
	public class SofdEmployeeFilter_Uuid : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on Uuid.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_Uuid(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Guid filterValue) : base(filterOperator, SofdEmployee.FIELD_UUID, filterValueOperator, filterValue.ToString()) {
		} // SofdEmployeeFilter_Uuid

	} // SofdEmployeeFilter_Uuid
	#endregion

	#region SofdEmployeeFilter_MifareId class
	/// <summary>
	/// Employee filter on MifareId.
	/// </summary>
	public class SofdEmployeeFilter_MifareId : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on MifareId.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_MifareId(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_MIFARE_ID, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_MifareId

	} // SofdEmployeeFilter_MifareId
	#endregion

	#region SofdEmployeeFilter_NaermesteLederMaNummer class
	/// <summary>
	/// Employee filter on NaermesteLederMaNummer.
	/// </summary>
	public class SofdEmployeeFilter_NaermesteLederMaNummer : SqlWhereFilterInt32 {

		/// <summary>
		/// Employee filter on NaermesteLederMaNummer.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_NaermesteLederMaNummer(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, Int32 filterValue) : base(filterOperator, SofdEmployee.FIELD_NAERMESTE_LEDER_MA_NUMMER, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_NaermesteLederMaNummer

	} // SofdEmployeeFilter_NaermesteLederMaNummer
	#endregion

	#region SofdEmployeeFilter_NaermesteLederCprNummer class
	/// <summary>
	/// Employee filter on NaermesteLederCprNummer.
	/// </summary>
	public class SofdEmployeeFilter_NaermesteLederCprNummer : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on NaermesteLederCprNummer.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_NaermesteLederCprNummer(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_NAERMESTE_LEDER_CPR_NUMMER, filterValueOperator, (filterValue == null) ? null : filterValue.Trim().Replace("-", "")) {
		} // SofdEmployeeFilter_NaermesteLederCprNummer

	} // SofdEmployeeFilter_NaermesteLederCprNummer
	#endregion

	#region SofdEmployeeFilter_NaermesteLederAdBrugerNavn class
	/// <summary>
	/// Employee filter on NaermesteLederAdBrugerNavn.
	/// </summary>
	public class SofdEmployeeFilter_NaermesteLederAdBrugerNavn : SqlWhereFilterString {

		/// <summary>
		/// Employee filter on NaermesteLederAdBrugerNavn.
		/// </summary>
		/// <param name="filterOperator">The filter operator.</param>
		/// <param name="filterValueOperator">The filter value operator.</param>
		/// <param name="filterValue">The filter value.</param>
		public SofdEmployeeFilter_NaermesteLederAdBrugerNavn(SqlWhereFilterOperator filterOperator, SqlWhereFilterValueOperator filterValueOperator, String filterValue) : base(filterOperator, SofdEmployee.FIELD_NAERMESTE_LEDER_AD_BRUGER_NAVN, filterValueOperator, filterValue) {
		} // SofdEmployeeFilter_NaermesteLederAdBrugerNavn

	} // SofdEmployeeFilter_NaermesteLederAdBrugerNavn
	#endregion

} // NDK.Framework