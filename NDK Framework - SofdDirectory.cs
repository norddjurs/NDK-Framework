using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace NDK.Framework {

	#region EtlMedarbejder class.
	public class EtlMedarbejder {
		private Int32 MedarbejderHistorikId = -1;
		private Int32 MedarbejderId = -1;
		private DateTime AktivFra = DateTime.MinValue;
		private DateTime AktivTil = DateTime.MinValue;
		private Boolean Aktiv = false;
		private DateTime SidstAendret = DateTime.MinValue;
		private DateTime Ansaettelsesdato = DateTime.MinValue;
		private DateTime Ophoersdato = DateTime.MinValue;
		private Boolean AnsaettelseAktiv = false;
		private DateTime OPUSSidsstAendret = DateTime.MinValue;
		private Int32 MaNr = -1;
		private String ADBrugerNavn = String.Empty;
		private String CPR = String.Empty;
		private String Fornavn = String.Empty;
		private String Efternavn = String.Empty;
		private String Navn = String.Empty;
		private String Adresse = String.Empty;
		private String Stednavn = String.Empty;
		private String PostNrString = String.Empty;
		private String By = String.Empty;
		private String Land = String.Empty;
		private Boolean AdresseBeskyttet = false;
		private String Telefonnummer = String.Empty;
		private String Mobilnummer = String.Empty;
		private String Mobilnummer2 = String.Empty;
		private String Afdelingsnummer = String.Empty;
		private String Gruppenummer = String.Empty;
		private String AnsatForhold = String.Empty;
		private String AnsatForholdText = String.Empty;
		private Int32 StillingsId = -1;
		private String Stillingsbetegnelse = String.Empty;
		private Int32 OrganisationId = -1;
		private Int32 LOSOrgId = -1;
		private String OrganisationNavn = String.Empty;
		private String Email = String.Empty;
		private String LoenKlasse = String.Empty;
		private Decimal ArbejdstidTaeller = 0.0M;
		private Decimal ArbejdstidNaevner = 0.0M;
		private Int32 NaermesteLederMaNr = -1;
		private String NaermesteLederNavn = String.Empty;
		private Boolean Intern = false;
		private Boolean Ekstern = false;
		private Boolean Leder = false;
		private Boolean MedUdvalg = false;
		private Boolean SR = false;
		private Boolean TR = false;
		private Boolean TRSuppleant = false;
		private Boolean SenestAktiv = false;
		private Guid UUID = Guid.Empty;
		private Int16 EkstraCiffer = 0;
		private String MifareId = String.Empty;
		private String Kaldenavn = String.Empty;
		private Boolean Fakturagodkender = false;
		private String FakturagodkenderNiveau1 = String.Empty;
		private String FakturagodkenderNiveau1Beskrivelse = String.Empty;
		private String FakturagodkenderNiveau2 = String.Empty;
		private String FakturagodkenderNiveau2Beskrivelse = String.Empty;
		private Int32 PNr = -1;
		private DateTime FoersteAnsaettelsesdato = DateTime.MinValue;
		private DateTime Jubilaeumsanciennitetsdato = DateTime.MinValue;
		private String OPUSBrugerNavn = String.Empty;
		private String OrganisationKortNavn = String.Empty;
		private String NaermesteLederADBrugerNavn = String.Empty;
		private String NaermesteLederCPR = String.Empty;

		private SofdDirectory SofdDirectory;

		/// <summary>
		/// Instantiates a new SOFD employee object, and initializes with values from the current
		/// record in the data reader.
		/// </summary>
		/// <param name="dbReader">The data reader.</param>
		/// <param name="sofdDirectory">The SOFD directory.</param>
		public EtlMedarbejder(SofdDirectory sofdDirectory, IDataReader dbReader) {
			this.SofdDirectory = sofdDirectory;
		} // EtlMedarbejder

		#region Properties.

		#endregion

	} // EtlMedarbejder
	#endregion

	#region EtlOrganisation class.
	public class EtlOrganisation {
		private Int32 OrganisationHistorikId = -1;
		private Int32 OrganisationId = -1;
		private DateTime AktivFra = DateTime.MinValue;
		private DateTime AktivTil = DateTime.MinValue;
		private Boolean Aktiv = false;
		private DateTime SidstAendret = DateTime.MinValue;
		private Int32 LOSOrgId = -1;
		private Int32 ParentLOSOrgId = -1;
		private DateTime LOSSidstAendret = DateTime.MinValue;
		private String KortNavn = String.Empty;
		private String Navn = String.Empty;
		private String Gade = String.Empty;
		private String Stednavn = String.Empty;
		private Int16 Postnr = 0;
		private String By = String.Empty;
		private String Telefon = String.Empty;
		private Int32 CvrNr = -1;
		private Int64 EanNr = -1;
		private Int32 SeNr = -1;
		private Int32 PNr = -1;
		private Int64 Omkostningssted = -1;
		private Int16 OrgTypeID = -1;
		private String OrgType = String.Empty;
		private Guid UUID = Guid.Empty;
		private Boolean SenestAktiv = false;
		private Int32 LederMedarbejderId = -1;
		private Boolean LederNedarvet = false;
		private String LederNavn = String.Empty;
		private Int32 LederMaNr = -1;
		private String LederADBrugerNavn = String.Empty;

		private SofdDirectory SofdDirectory;

		/// <summary>
		/// Instantiates a new SOFD organization object, and initializes with values from the current
		/// record in the data reader.
		/// </summary>
		/// <param name="dbReader">The data reader.</param>
		/// <param name="sofdDirectory">The SOFD directory.</param>
		public EtlOrganisation(SofdDirectory sofdDirectory, IDataReader dbReader) {
			this.SofdDirectory = sofdDirectory;
		} // EtlOrganisation

		#region Properties.

		#endregion

	} // EtlOrganisation
	#endregion


	/*

	-- Create etl schema if it doesn't exist
	IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'etl')
	BEGIN
		EXEC( 'CREATE SCHEMA etl' );    
	END

	-- Create Organisation table if it doesn't exist
	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Organisation' AND type='U' )
	BEGIN
		CREATE TABLE [etl].[Organisation](
		[OrganisationHistorikId] [int] IDENTITY(1,1) NOT NULL,
		[OrganisationId] [int] NULL,
		[AktivFra] [date] NOT NULL,
		[AktivTil] [date] NULL,
		[Aktiv] [bit] NOT NULL,
		[SidstAendret] [datetime2](7) NULL,
		[LOSOrgId] [int] NOT NULL,
		[ParentLOSOrgId] [int] NULL,
		[LOSSidstAendret] [datetime2](7) NULL,
		[KortNavn] [nvarchar](255) NULL,
		[Navn] [nvarchar](255) NULL,
		[Gade] [nvarchar](255) NULL,
		[Stednavn] [nvarchar](255) NULL,
		[Postnr] [smallint] NULL,
		[By] [nvarchar](255) NULL,
		[Telefon] [nvarchar](255) NULL,
		[CvrNr] [int] NULL,
		[EanNr] [bigint] NULL,
		[SeNr] [int] NULL,
		[PNr] [int] NULL,
		[Omkostningssted] [bigint] NULL,
		[OrgTypeID] [smallint] NULL,
		[OrgType] [nvarchar](255) NULL,
		[UUID] [uniqueidentifier] NULL,
		[SenestAktiv] [bit] NULL,
		[LederMedarbejderId] [int] NULL,
		[LederNedarvet] [bit] NULL,
		[LederNavn] [nvarchar](100) NULL,
		[LederMaNr] [int] NULL,
		[LederADBrugerNavn] [nvarchar](10) NULL,
		 CONSTRAINT [PK_Organisation] PRIMARY KEY CLUSTERED 
		(
			[OrganisationHistorikId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]


		ALTER TABLE [etl].[Organisation] ADD  CONSTRAINT [DF_Organisation_Aktiv]  DEFAULT ((1)) FOR [Aktiv]

		ALTER TABLE [etl].[Organisation] ADD  CONSTRAINT [DF_Organisation_SenestAktiv]  DEFAULT ((1)) FOR [SenestAktiv]
	END

	-- Create Medarbejder table if it doesn't exist
	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Medarbejder' AND type='U' )
	BEGIN
	CREATE TABLE [etl].[Medarbejder](
		[MedarbejderHistorikId] [int] IDENTITY(1,1) NOT NULL,
		[MedarbejderId] [int] NULL,
		[AktivFra] [date] NOT NULL,
		[AktivTil] [date] NULL,
		[Aktiv] [bit] NOT NULL,
		[SidstAendret] [datetime2](7) NOT NULL,
		[Ansaettelsesdato] [date] NULL,
		[Ophoersdato] [date] NULL,
		[AnsaettelseAktiv] [bit] NOT NULL,
		[OPUSSidsstAendret] [datetime2](7) NULL,
		[MaNr] [int] NULL,
		[ADBrugerNavn] [nvarchar](50) NULL,
		[CPR] [nvarchar](10) NULL,
		[Fornavn] [nvarchar](50) NULL,
		[Efternavn] [nvarchar](50) NULL,
		[Navn] [nvarchar](100) NULL,
		[Adresse] [nvarchar](100) NULL,
		[Stednavn] [nvarchar](50) NULL,
		[PostNr] [nvarchar](10) NULL,
		[By] [nvarchar](50) NULL,
		[Land] [nvarchar](50) NULL,
		[AdresseBeskyttet] [bit] NOT NULL,
		[Telefonnummer] [nvarchar](20) NULL,
		[Mobilnummer] [nvarchar](20) NULL,
		[Mobilnummer2] [nvarchar](20) NULL,
		[Afdelingsnummer] [nvarchar](20) NULL,
		[Gruppenummer] [nvarchar](20) NULL,
		[AnsatForhold] [nvarchar](2) NULL,
		[AnsatForholdText] [nvarchar](50) NULL,
		[StillingsId] [int] NULL,
		[Stillingsbetegnelse] [nvarchar](100) NULL,
		[OrganisationId] [int] NULL,
		[LOSOrgId] [int] NULL,
		[OrganisationNavn] [nvarchar](255) NULL,
		[email] [nvarchar](100) NULL,
		[LoenKlasse] [nvarchar](100) NULL,
		[ArbejdstidTaeller] [decimal](28, 3) NULL,
		[ArbejdstidNaevner] [decimal](28, 3) NULL,
		[NaermesteLederMaNr] [int] NULL,
		[NaermesteLederNavn] [nvarchar](100) NULL,
		[Intern] [bit] NOT NULL,
		[Ekstern] [bit] NOT NULL,
		[Leder] [bit] NOT NULL,
		[MedUdvalg] [bit] NOT NULL,
		[SR] [bit] NOT NULL,
		[TR] [bit] NOT NULL,
		[TRSuppleant] [bit] NOT NULL,
		[SenestAktiv] [bit] NOT NULL,
		[UUID] [uniqueidentifier] NULL,
		[EkstraCiffer] [tinyint] NULL,
		[MifareId] [nvarchar](20) NULL,
		[Kaldenavn] [nvarchar](100) NULL,
		[Fakturagodkender] [bit] NULL,
		[FakturagodkenderNiveau1] [nvarchar](50) NULL,
		[FakturagodkenderNiveau1Beskrivelse] [nvarchar](255) NULL,
		[FakturagodkenderNiveau2] [nvarchar](50) NULL,
		[FakturagodkenderNiveau2Beskrivelse] [nvarchar](255) NULL,
		[PNr] [int] NULL,
		[FoersteAnsaettelsesdato] [datetime2](7) NULL,
		[Jubilaeumsanciennitetsdato] [datetime2](7) NULL,
		[OPUSBrugerNavn] [nvarchar](20) NULL,
		[OrganisationKortNavn] [nvarchar](255) NULL,
		[NaermesteLederADBrugerNavn] [nvarchar](50) NULL,
		[NaermesteLederCPR] [nvarchar](10) NULL,
	 CONSTRAINT [PK_Medarbejder] PRIMARY KEY CLUSTERED 
	(
		[MedarbejderHistorikId] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [etl].[Medarbejder] ADD  CONSTRAINT [DF_Medarbejder_Aktiv]  DEFAULT ((1)) FOR [Aktiv]

	ALTER TABLE [etl].[Medarbejder] ADD  CONSTRAINT [DF_Medarbejder_AnsaettelseAktiv]  DEFAULT ((0)) FOR [AnsaettelseAktiv]

	ALTER TABLE [etl].[Medarbejder] ADD  CONSTRAINT [DF_Medarbejder_SenestAktiv]  DEFAULT ((1)) FOR [SenestAktiv]

	END

*/


	#region SofdDirectory class.
	public class SofdDirectory {
		private IConfiguration config = null;

		/// <summary>
		/// Connect to the SOFD database.
		/// </summary>
		public SofdDirectory(IConfiguration config) {
			this.config = config;

			// Connect to the SOFD database.
			
		} // SofdDirectory

	} // SofdDirectory
	#endregion

} // NDK.Framework