using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NDK.Framework.CprBroker.Admin;
using NDK.Framework.CprBroker.Part;
using NDK.Framework.Kombit.CprService;

namespace NDK.Framework.CprBroker {

	#region CprSearchResult
	public class CprSearchResult {

		#region Constructors.
		public CprSearchResult(String cprNumber, NDK.Framework.CprBroker.Part.LaesOutputType cprBrokerResult) {
			this.CprNumber = cprNumber;
			this.FirstName = cprBrokerResult.ToPartPersonFirstName();
			this.MiddleName = cprBrokerResult.ToPartPersonMiddleName();
			this.LastName = cprBrokerResult.ToPartPersonLastName();
			this.FullName = cprBrokerResult.ToPartPersonFullName();

			/* debug code
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)cprBrokerResult.LaesResultat.Item;
			NDK.Framework.CprBroker.Part.CprBorgerType partBorgerType = (NDK.Framework.CprBroker.Part.CprBorgerType)partPersonType.AttributListe.RegisterOplysning[0].Item;
			NDK.Framework.CprBroker.Part.PersonNameStructureType partPersonName = partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameStructure;

			Console.WriteLine("=====================================================");
			Console.WriteLine(CprNumber);
			Console.WriteLine("Note = " + partPersonType.AttributListe.Egenskab[0].NavnStruktur.NoteTekst);
			Console.WriteLine("Kaldenavn = " + partPersonType.AttributListe.Egenskab[0].NavnStruktur.KaldenavnTekst);
			Console.WriteLine("Adressenavn = " + partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameForAddressingName);
			Console.WriteLine("Fornavn = " + partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameStructure.PersonGivenName);
			Console.WriteLine("Mellemnavn = " + partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameStructure.PersonMiddleName);
			Console.WriteLine("Efternavn = " + partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameStructure.PersonSurnameName);
			Console.WriteLine("=====================================================");
			*/
		} // CprSearchResult


		public CprSearchResult(String cprNumber, Dictionary<String, String> cprKombitResult) {
			this.CprNumber = cprNumber;
			if (cprKombitResult.ContainsKey("CNVN_FORNVN") == true) {
				this.FirstName = cprKombitResult["CNVN_FORNVN"];
			}
			if (cprKombitResult.ContainsKey("CNVN_MELNVN") == true) {
				this.MiddleName = cprKombitResult["CNVN_MELNVN"];
			}
			if (cprKombitResult.ContainsKey("CNVN_EFTERNVN") == true) {
				this.LastName = cprKombitResult["CNVN_EFTERNVN"];
			}
			if (cprKombitResult.ContainsKey("CNVN_ADRNVN") == true) {
				this.FullName = cprKombitResult["CNVN_ADRNVN"];
			}

			/* debug code
			Console.WriteLine("=====================================================");
			Console.WriteLine(CprNumber);
			foreach (String key in cprKombitResult.Keys) {
				Console.WriteLine(key + " = " + cprKombitResult[key]);
			}
			Console.WriteLine("=====================================================");
			*/
		} // CprSearchResult
		#endregion

		#region Properties.
		public String CprNumber { get; }

		public String FirstName { get; }

		public String MiddleName { get; }

		public String LastName { get; }

		public String FullName { get; }

		public String Address { get; }

		#endregion

	} // CprSearchResult
	#endregion

	#region CprBroker
	/// <summary>
	/// Access the CPR Broker service.
	///	Demo code at: https://svn.softwareborsen.dk/CPR-broker/Source/Demo/CPR%20Business%20Application%20Demo/
	/// 
	/// WDSL Admin: http://cprbroker.intern.norddjurs.dk/Services/Admin.asmx?wsdl
	/// WDSL Part: http://cprbroker.intern.norddjurs.dk/Services/Part.asmx?wsdl
	///
	/// This code is from: RPC Scandinavia / René Paw Christensen.
	/// </summary>
	public class RpcCprBroker {
		private String serviceUrl;
		private String applicationName;
		private String applicationToken;
		private String userToken;
		private AdminSoap12Client clientAdmin;          // applicationsHandler = new AdminSoap12Client("AdminSoap12", applicationsWsUrl);
		private PartSoap12Client clientPart;

		#region Constructors and destructors
		//*******************************************************************************************
		//	Constructors and destructors.
		//*******************************************************************************************
		public RpcCprBroker(String serviceUrl, String applicationName, String applicationToken, String userToken) {
			//	Save settings.
			this.serviceUrl = serviceUrl.TrimEnd('/');
			this.applicationName = applicationName;
			this.applicationToken = applicationToken;
			this.userToken = userToken;

			//	Set up the binding element to match the "app.config" settings.
			//	This avoids the requirement of a ".config" file.
			TextMessageEncodingBindingElement bindingTMEE = new TextMessageEncodingBindingElement();
			bindingTMEE.MessageVersion = MessageVersion.Soap12;
			bindingTMEE.MaxReadPoolSize = 64;
			bindingTMEE.MaxWritePoolSize = 16;
			bindingTMEE.WriteEncoding = Encoding.UTF8;
			bindingTMEE.ReaderQuotas.MaxDepth = 32;
			bindingTMEE.ReaderQuotas.MaxStringContentLength = 8192;
			bindingTMEE.ReaderQuotas.MaxArrayLength = 16384;
			bindingTMEE.ReaderQuotas.MaxBytesPerRead = 4096;
			bindingTMEE.ReaderQuotas.MaxNameTableCharCount = 16384;

			HttpTransportBindingElement bindingHTBE = new HttpTransportBindingElement();
			bindingHTBE.ManualAddressing = false;
			bindingHTBE.MaxBufferPoolSize = 524288;
			bindingHTBE.MaxReceivedMessageSize = 65536;
			bindingHTBE.AllowCookies = false;
			bindingHTBE.AuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous;
			bindingHTBE.BypassProxyOnLocal = false;
			bindingHTBE.DecompressionEnabled = true;
			bindingHTBE.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
			bindingHTBE.KeepAliveEnabled = true;
			bindingHTBE.MaxBufferPoolSize = 65536;
			bindingHTBE.ProxyAuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous;
			bindingHTBE.Realm = String.Empty;
			bindingHTBE.TransferMode = TransferMode.Buffered;
			bindingHTBE.UnsafeConnectionNtlmAuthentication = false;
			bindingHTBE.UseDefaultWebProxy = true;

			EndpointAddress endpointAddressAdmin = new EndpointAddress(this.serviceUrl + "/Admin.asmx");
			CustomBinding bindingAdmin = new CustomBinding();
			bindingAdmin.Name = "AdminSoap12";
			bindingAdmin.Namespace = "http://tempuri.org/";
			bindingAdmin.OpenTimeout = TimeSpan.FromMinutes(1);
			bindingAdmin.ReceiveTimeout = TimeSpan.FromMinutes(10);
			bindingAdmin.SendTimeout = TimeSpan.FromMinutes(1);
			bindingAdmin.CloseTimeout = TimeSpan.FromMinutes(1);
			bindingAdmin.Elements.Add(bindingTMEE);
			bindingAdmin.Elements.Add(bindingHTBE);

			EndpointAddress endpointAddressPart = new EndpointAddress(this.serviceUrl + "/Part.asmx");
			CustomBinding bindingPart = new CustomBinding();
			bindingPart.Name = "PartSoap12";
			bindingPart.Namespace = "http://tempuri.org/";
			bindingPart.OpenTimeout = TimeSpan.FromMinutes(1);
			bindingPart.ReceiveTimeout = TimeSpan.FromMinutes(10);
			bindingPart.SendTimeout = TimeSpan.FromMinutes(1);
			bindingPart.CloseTimeout = TimeSpan.FromMinutes(1);
			bindingPart.Elements.Add(bindingTMEE);
			bindingPart.Elements.Add(bindingHTBE);

			// Create the client.
			//	Url example: "http://cprbroker.intern.norddjurs.dk/Services/Part.asmx"
			this.clientAdmin = new AdminSoap12Client(bindingAdmin, endpointAddressAdmin);
			this.clientPart = new PartSoap12Client(bindingPart, endpointAddressPart);

			// Set the timeout to avoid hanging the application for too long when wrong urls are given.
			//this.clientAdmin.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 15);
		} // RpcCprBroker
		#endregion

		#region Application Registration
		//*******************************************************************************************
		//	Application Registration.
		//*******************************************************************************************
		public Boolean TestServiceConnection() {
			try {
				// If we have a successful connection the server will send some information
				// back stating who it thinks we are, etc. If we cannot connect we will get
				// an exception. Just to be sure - we expect the pingResult to contain at least something.
				NDK.Framework.CprBroker.Admin.BasicOutputTypeOfBoolean pingResult = IsImplementing(GetApplicationHeader(), "Read", "1.0");
				return pingResult.Item;
			} catch (Exception exception) {
				Console.WriteLine("Ping exception for URL '" + this.serviceUrl + "': " + exception.Message);
				return false;
			}
		} // TestServiceConnection
		#endregion

		#region Application methods
		//*******************************************************************************************
		//	Application methods.
		//*******************************************************************************************
		/// <summary>
		/// Checks whether the application is registered.
		/// </summary>
		/// <returns>Returns null if the application is not registered, otherwise the registered application is returned</returns>
		public ApplicationType ApplicationIsRegistered() {
			try {
				BasicOutputTypeOfArrayOfApplicationType applicationTypes = clientAdmin.ListAppRegistrations(GetApplicationHeader());
				if ((applicationTypes == null) || (applicationTypes.Item == null) || (applicationTypes.Item.Length == 0)) {
					return null;
				} else {
					// Loop through all registered application to see if one of them is this application.
					foreach (ApplicationType applicationType in applicationTypes.Item) {
						if (applicationType.Token == this.applicationToken) {
							return applicationType;
						}
					}
					return null;
				}
			} catch (Exception exception) {
				return null;
			}
		} // ApplicationIsRegistered

		/// <summary>
		/// Registers the application, if it is not registed before.
		/// The application still need to be approved by the Service Administrator, before the application can query data.
		/// </summary>
		/// <returns>Returns the application token generated, if the application is not registed.</returns>
		public String ApplicationRegister() {
			try {
				//	This fails if the application already is registed.
				BasicOutputTypeOfApplicationType result = this.clientAdmin.RequestAppRegistration(this.applicationName);
				if ((result != null) && (result.Item != null)) {
					return result.Item.Token;
				} else {
					return String.Empty;
				}
			} catch (Exception exception) {
				return String.Empty;
			}
		} // ApplicationRegister

		/// <summary>
		/// Approves the application if it is registed.
		/// Note that this requires the Administrative Application Token.
		/// </summary>
		/// <param name="adminAppToken">The Administrative Application Token.</param>
		/// <returns>Returns true if the application is approved.</returns>
		public Boolean ApplicationApprove(String adminAppToken) {
			try {
				// Here we change the application to the built-in admin token in CPR Broker, to
				// fool CPR Broker into believing that we are actually allowed to approve
				// an application.
				NDK.Framework.CprBroker.Admin.ApplicationHeader applicationHeader = GetApplicationHeader();
				applicationHeader.ApplicationToken = adminAppToken;
				return this.clientAdmin.ApproveAppRegistration(applicationHeader, this.applicationToken).Item;
			} catch (Exception exception) {
				Console.WriteLine("Admin approve exception:" + exception.Message);
				return false;
			}
		} // ApplicationApprove
		#endregion

		#region Part methods
		//*******************************************************************************************
		//	Part methods.
		//*******************************************************************************************
		public Guid PartGetUUID(String cpr) {
			// Validate the CPR.
			if (cpr == null) {
				throw new NullReferenceException("CPR Person Master Query Error: CPR-number is null.");
			}
			cpr = cpr.Replace("-", String.Empty);

			// Get UUID from CPR Person Master.
			NDK.Framework.CprBroker.Part.GetUuidOutputType partUUID = this.clientPart.GetUuid(GetPartApplicationHeader(), cpr);

			// Throw exception if the UUID query failed.
			if (partUUID == null) {
				throw new NullReferenceException("CPR Person Master Query Error: A unknown error occured, and null was returned from the Web Service.");
			}
			if ((partUUID.StandardRetur.StatusKode != "200") && (partUUID.StandardRetur.StatusKode != "400")) {
				throw new Exception(String.Format("CPR Person Master Query Error {0}: {1}", partUUID.StandardRetur.StatusKode, partUUID.StandardRetur.FejlbeskedTekst));
			}

			// Return the UUID.
			return new Guid(partUUID.UUID);
		} // PartGetUUID

		public NDK.Framework.CprBroker.Part.LaesOutputType PartRead(String cpr) {
			return PartRead(PartGetUUID(cpr), false);
		} // PartRead

		public NDK.Framework.CprBroker.Part.LaesOutputType PartRead(String cpr, Boolean refresh) {
			return PartRead(PartGetUUID(cpr), refresh);
		} // PartRead

		public NDK.Framework.CprBroker.Part.LaesOutputType PartRead(Guid uuid) {
			return PartRead(uuid, false);
		} // PartRead

		public NDK.Framework.CprBroker.Part.LaesOutputType PartRead(Guid uuid, Boolean refresh) {
			// Query person data.
			NDK.Framework.CprBroker.Part.SourceUsageOrderHeader partOrderHeader = new NDK.Framework.CprBroker.Part.SourceUsageOrderHeader();
			NDK.Framework.CprBroker.Part.LaesInputType partInput = new NDK.Framework.CprBroker.Part.LaesInputType() {
				RegistreringFraFilter = null,
				RegistreringTilFilter = null,
				UUID = uuid.ToString(),
				VirkningFraFilter = null,
				VirkningTilFilter = null
			};
			NDK.Framework.CprBroker.Part.LaesOutputType partOutput = null;
			NDK.Framework.CprBroker.Part.QualityHeader partQuality = null;

			// Read or RefreshRead.
			if (refresh == false) {
				partQuality = this.clientPart.RefreshRead(GetPartApplicationHeader(), partInput, out partOutput);
			} else {
				partQuality = this.clientPart.Read(GetPartApplicationHeader(), partOrderHeader, partInput, out partOutput);
			}

			// Throw exception if the query failed.
			if ((partQuality == null) || (partOutput == null)) {
				throw new NullReferenceException("CPR Part Read Error: A unknown error occured, and null was returned from the Web Service.");
			}
			//if ((partOutput.StandardRetur.StatusKode != "200") && (partOutput.StandardRetur.StatusKode != "400")) {
			if (partOutput.StandardRetur.StatusKode != "200") {
				throw new Exception(String.Format("CPR Part Read Error {0}: {1}", partOutput.StandardRetur.StatusKode, partOutput.StandardRetur.FejlbeskedTekst));
			}

			// Return the output.
			return partOutput;
		} // PartRead

		public List<NDK.Framework.CprBroker.Part.LaesOutputType> PartList(params String[] cprs) {
			// Convert CPRs to UUIDs.
			List<Guid> listUuids = new List<Guid>();
			foreach (String cpr in cprs) {
				try {
					listUuids.Add(PartGetUUID(cpr));
				} catch { }
			}
			return PartList(listUuids.ToArray());
		} // PartList

		public List<NDK.Framework.CprBroker.Part.LaesOutputType> PartList(params Guid[] uuids) {
			// Convert Guids to Strings.
			List<String> listUuids = new List<String>();
			foreach (Guid uuid in uuids) {
				if (uuid.Equals(Guid.Empty) == false) {
					listUuids.Add(uuid.ToString());
				}
			}

			NDK.Framework.CprBroker.Part.SourceUsageOrderHeader partOrderHeader = new NDK.Framework.CprBroker.Part.SourceUsageOrderHeader();
			NDK.Framework.CprBroker.Part.ListInputType partInput = new NDK.Framework.CprBroker.Part.ListInputType() {
				RegistreringFraFilter = null,
				RegistreringTilFilter = null,
				UUID = listUuids.ToArray(),
				VirkningFraFilter = null,
				VirkningTilFilter = null
			};
			NDK.Framework.CprBroker.Part.ListOutputType1 partOutput = null;
			NDK.Framework.CprBroker.Part.QualityHeader partQuality = null;

			// Read.
			partQuality = this.clientPart.List(GetPartApplicationHeader(), partOrderHeader, partInput, out partOutput);

			// Throw exception if the query failed.
			if ((partQuality == null) || (partOutput == null)) {
				throw new NullReferenceException("CPR Part List Error: A unknown error occured, and null was returned from the Web Service.");
			}
			//if ((partOutput.StandardRetur.StatusKode != "200") && (partOutput.StandardRetur.StatusKode != "400")) {
			if (partOutput.StandardRetur.StatusKode != "200") {
				throw new Exception(String.Format("CPR Part List Error {0}: {1}", partOutput.StandardRetur.StatusKode, partOutput.StandardRetur.FejlbeskedTekst));
			}

			// Return the output.
			List<NDK.Framework.CprBroker.Part.LaesOutputType> result = new List<NDK.Framework.CprBroker.Part.LaesOutputType>();
			foreach (NDK.Framework.CprBroker.Part.LaesResultatType resultType in partOutput.LaesResultat) {
				result.Add((NDK.Framework.CprBroker.Part.LaesOutputType)resultType.Item);
			}
			return result;
		} // PartList

		/*
				public string[] Search(string applicationToken, string cprNumber)
				{
					try
					{
						PartService.SoegInputType1 input = new CPR_Business_Application_Demo.PartService.SoegInputType1()
						{
							MaksimalAntalKvantitet = "10",
							FoersteResultatReference = "0",
							SoegObjekt = new CPR_Business_Application_Demo.PartService.SoegObjektType()
							{
								BrugervendtNoegleTekst = cprNumber
							}
						};

						SoegOutputType output;
						PartHandler.Search(CreateApplicationHeader(applicationToken), input, out output);

						if (output != null && output.Idliste != null)
						{
							return output.Idliste;
						}
					}
					catch (Exception)
					{

					}
					return null;
				}



				public string[] SearchRelation(string applicationToken, string cprNumber)
				{
					try
					{
						PartService.SoegInputType1 input = new CPR_Business_Application_Demo.PartService.SoegInputType1()
						{
							MaksimalAntalKvantitet = "10",
							FoersteResultatReference = "0",
							SoegObjekt = new CPR_Business_Application_Demo.PartService.SoegObjektType()
							{
								BrugervendtNoegleTekst = cprNumber

							}
						};

						SoegOutputType output;
						PartHandler.Search(CreateApplicationHeader(applicationToken), input, out output);

						if (output != null && output.Idliste != null)
						{
							return output.Idliste;
						}
					}
					catch (Exception)
					{

					}
					return null;
				}

		*/

		#endregion

		#region Private methods
		//*******************************************************************************************
		//	Private methods.
		//*******************************************************************************************
		private NDK.Framework.CprBroker.Admin.ApplicationHeader GetApplicationHeader() {
			return new NDK.Framework.CprBroker.Admin.ApplicationHeader() {
				UserToken = this.userToken,
				ApplicationToken = this.applicationToken
			};
		} // GetApplicationHeader

		private NDK.Framework.CprBroker.Part.ApplicationHeader GetPartApplicationHeader() {
			return new NDK.Framework.CprBroker.Part.ApplicationHeader() {
				UserToken = this.userToken,
				ApplicationToken = this.applicationToken
			};
		} // GetPartApplicationHeader


		/*
				public ServiceVersionType[] GetCapabillities()
				{
					try
					{
						var cprAdministrationAdapter = new ApplicationAdapter(CprAdminWebServiceUrl);
						return cprAdministrationAdapter.GetCapabillities(GetApplicationHeader()).Item;

					}
					catch (Exception)
					{
						return new ServiceVersionType[] {};
					}
				}
		*/

		/*
				public BasicOutputTypeOfArrayOfServiceVersionType GetCapabillities(ApplicationHeader applicationHeader) {
					return applicationsHandler.GetCapabilities(applicationHeader);
				}
		*/

		private NDK.Framework.CprBroker.Admin.BasicOutputTypeOfBoolean IsImplementing(NDK.Framework.CprBroker.Admin.ApplicationHeader applicationHeader, String method, String version) {
			return this.clientAdmin.IsImplementing(applicationHeader, method, version);
		} // IsImplementing
		#endregion

	} // RpcCprBroker
	#endregion

	#region CprBrokerExtend
	public static class RpcCprBrokerExtend {

		#region Convert methods
		//*******************************************************************************************
		//	Convert methods.
		//*******************************************************************************************
		public static NDK.Framework.CprBroker.Part.PersonNameStructureType ToPartPersonName(this NDK.Framework.CprBroker.Part.LaesOutputType partOutput) {
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)partOutput.LaesResultat.Item;
			return partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameStructure;
		} // ToPartPersonName

		public static String ToPartPersonFullName(this NDK.Framework.CprBroker.Part.LaesOutputType partOutput) {
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)partOutput.LaesResultat.Item;
			NDK.Framework.CprBroker.Part.PersonNameStructureType partPersonName = partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameStructure;
			return ((partPersonName.PersonGivenName + " " + partPersonName.PersonMiddleName).Trim() + " " + partPersonName.PersonSurnameName).Trim();
		} // ToPartPersonFullName

		public static String ToPartPersonFirstName(this NDK.Framework.CprBroker.Part.LaesOutputType partOutput) {
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)partOutput.LaesResultat.Item;
			NDK.Framework.CprBroker.Part.PersonNameStructureType partPersonName = partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameStructure;
			return partPersonName.PersonGivenName;
		} // ToPartPersonFirstName

		public static String ToPartPersonMiddleName(this NDK.Framework.CprBroker.Part.LaesOutputType partOutput) {
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)partOutput.LaesResultat.Item;
			NDK.Framework.CprBroker.Part.PersonNameStructureType partPersonName = partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameStructure;
			return partPersonName.PersonMiddleName;
		} // ToPartPersonMiddleName

		public static String ToPartPersonLastName(this NDK.Framework.CprBroker.Part.LaesOutputType partOutput) {
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)partOutput.LaesResultat.Item;
			NDK.Framework.CprBroker.Part.PersonNameStructureType partPersonName = partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameStructure;
			return partPersonName.PersonSurnameName;
		} // ToPartPersonLastName

		public static String ToPartPersonAddressName(this NDK.Framework.CprBroker.Part.LaesOutputType partOutput) {
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)partOutput.LaesResultat.Item;
			String partAddressName = partPersonType.AttributListe.Egenskab[0].NavnStruktur.PersonNameForAddressingName;
			return partAddressName;
		} // ToPartPersonAddressName



		public static String ToPartPersonBirthAuthName(this NDK.Framework.CprBroker.Part.LaesOutputType partOutput) {
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)partOutput.LaesResultat.Item;
			return partPersonType.AttributListe.Egenskab[0].FoedselsregistreringMyndighedNavn;
		} // ToPartPersonBirthAuthName

		public static String ToPartPersonBirthPlace(this NDK.Framework.CprBroker.Part.LaesOutputType partOutput) {
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)partOutput.LaesResultat.Item;
			return partPersonType.AttributListe.Egenskab[0].FoedestedNavn;
		} // ToPartPersonBirthPlace

		public static DateTime ToPartPersonBirthDate(this NDK.Framework.CprBroker.Part.LaesOutputType partOutput) {
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)partOutput.LaesResultat.Item;
			return partPersonType.AttributListe.Egenskab[0].BirthDate;
		} // ToPartPersonBirthDate



		public static String ToPartPersonGender(this NDK.Framework.CprBroker.Part.LaesOutputType partOutput) {
			NDK.Framework.CprBroker.Part.RegistreringType1 partPersonType = (NDK.Framework.CprBroker.Part.RegistreringType1)partOutput.LaesResultat.Item;
			NDK.Framework.CprBroker.Part.PersonGenderCodeType partPersonGender = partPersonType.AttributListe.Egenskab[0].PersonGenderCode;
			switch (partPersonGender) {
				case NDK.Framework.CprBroker.Part.PersonGenderCodeType.male:
					return "Male";
				case NDK.Framework.CprBroker.Part.PersonGenderCodeType.female:
					return "Female";
				default:
					return "";
			}
		} // ToPartPersonGender
		#endregion

	} // RpcCprBrokerExtend
	#endregion




	#region RpcServiceplatformBase.
	public abstract class RpcServiceplatformBase {
		//private X509Certificate2 serviceCertificate = null;
		//private String serviceCertificatePassword = null;
		private String serviceURL = null;
		private Guid serviceUUID = Guid.Empty;
		private Guid serviceAgreementUUID = Guid.Empty;
		private Guid userSystemUUID = Guid.Empty;
		private Guid userUUID = Guid.Empty;
		private String accountingInfo = null;
		private String accountingUser = null;
		private EndpointAddress clientEndpoint = null;
		private BasicHttpBinding clientBinding = null;
		private InvocationContextType serviceContext = null;
		private CprLookupServicePortTypeClient client = null;

		#region Constructors.
		public RpcServiceplatformBase(String serviceUrl, Guid serviceUuid, Guid serviceAgreementUuid, Guid userSystemUuid, Guid userUuid, String accountingInfo, String accountingUser, String certificateSerialNumber) {
			this.serviceURL = serviceUrl;
			this.serviceUUID = serviceUuid;
			this.serviceAgreementUUID = serviceAgreementUuid;
			this.userSystemUUID = userSystemUuid;
			this.userUUID = userUuid;
			this.accountingInfo = accountingInfo;
			this.accountingUser = accountingUser;

			// Initialize the client endpoint and client binding.
			InitializeClient();

			// Initialize the client certificate.
			InitializeCertificate(certificateSerialNumber);
		} // RpcServiceplatformBase
		#endregion

		#region Public properties
		/// <summary>
		/// Gets the service URL.
		/// </summary>
		public String ServiceURL {
			get {
				return this.serviceURL;
			}
		} // ServiceURL

		/// <summary>
		/// Gets the certificate used by the client.
		/// </summary>
		public X509Certificate2 ServiceCertificate {
			get {
				return this.client.ClientCredentials.ClientCertificate.Certificate;
			}
		} // ServiceCertificate

		/// <summary>
		/// Gets the service UUID (Service navn).
		/// </summary>
		public Guid ServiceUUID {
			get {
				return this.serviceUUID;
			}
		} // ServiceUUID

		/// <summary>
		/// Gets the service agreement UUID (Serviceaftale UUID).
		/// </summary>
		public Guid ServiceAgreementUUID {
			get {
				return this.serviceAgreementUUID;
			}
		} // ServiceAgreementUUID

		/// <summary>
		/// Gets the user system UUID (System UUID).
		/// </summary>
		public Guid UserSystemUUID {
			get {
				return this.userSystemUUID;
			}
		} // UserSystemUUID

		/// <summary>
		/// Gets the user UUID (Kommune UUID).
		/// </summary>
		public Guid UserUUID {
			get {
				return this.userUUID;
			}
		} // UserUUID

		/// <summary>
		/// Gets the accounting information (System navn).
		/// </summary>
		public String AccountingInfo {
			get {
				return this.accountingInfo;
			}
			set {
				this.accountingInfo = (value != null) ? value : String.Empty;
			}
		} // AccountingInfo

		/// <summary>
		/// Gets the accounting user (System brugernavn).
		/// </summary>
		public String AccountingUser {
			get {
				return this.accountingUser;
			}
			set {
				this.accountingUser = (value != null) ? value : String.Empty;
			}
		} // AccountingUser
		#endregion

		#region Protected properties.
		/// <summary>
		/// Gets the communication service context.
		/// </summary>
		protected InvocationContextType ServiceContext {
			get {
				return this.serviceContext;
			}
		} // ServiceContext

		/// <summary>
		/// Gets the communications client.
		/// </summary>
		protected CprLookupServicePortTypeClient Client {
			get {
				return this.client;
			}
		} // Client
		#endregion

		#region Private methods.
		public void InitializeClient() {
			// Initiate the client endpoint and client binding.
			// The BasicHttpBinding is used to get SOAP v 1.1 using SSL with client authentication.
			// This avoids using the ".config" configuration files.
			this.clientEndpoint = new EndpointAddress(this.serviceURL);
			this.clientBinding = new BasicHttpBinding();
			this.clientBinding.AllowCookies = false;
			this.clientBinding.BypassProxyOnLocal = false;
			this.clientBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
			this.clientBinding.TransferMode = TransferMode.Buffered;
			this.clientBinding.MessageEncoding = WSMessageEncoding.Text;
			this.clientBinding.CloseTimeout = TimeSpan.FromMinutes(1);
			this.clientBinding.OpenTimeout = TimeSpan.FromMinutes(1);
			this.clientBinding.ReceiveTimeout = TimeSpan.FromMinutes(10);
			this.clientBinding.SendTimeout = TimeSpan.FromMinutes(1);
			this.clientBinding.MaxBufferSize = 65536;
			this.clientBinding.MaxBufferPoolSize = 524288;
			this.clientBinding.MaxReceivedMessageSize = 65536;
			this.clientBinding.UseDefaultWebProxy = true;
			this.clientBinding.ReaderQuotas.MaxDepth = 32;
			this.clientBinding.ReaderQuotas.MaxStringContentLength = 8192;
			this.clientBinding.ReaderQuotas.MaxArrayLength = 16384;
			this.clientBinding.ReaderQuotas.MaxBytesPerRead = 4096;
			this.clientBinding.ReaderQuotas.MaxNameTableCharCount = 16384;
			this.clientBinding.Security.Mode = BasicHttpSecurityMode.Transport;
			this.clientBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

			// Initialize the InvocationContextType.
			this.serviceContext = new InvocationContextType();
			this.serviceContext.ServiceUUID = this.serviceUUID.ToString().Trim('{', '}');
			this.serviceContext.ServiceAgreementUUID = this.serviceAgreementUUID.ToString().Trim('{', '}');
			this.serviceContext.UserSystemUUID = this.userSystemUUID.ToString().Trim('{', '}');
			this.serviceContext.UserUUID = this.userUUID.ToString().Trim('{', '}');
			this.serviceContext.AccountingInfo = this.accountingInfo;
			this.serviceContext.OnBehalfOfUser = this.accountingUser;

			// Initiate the client.
			this.client = new CprLookupServicePortTypeClient(clientBinding, clientEndpoint);
		} // InitializeClient

		private void InitializeCertificate(String certificateSerialNumber) {
			// Trim the certificate serial number.
			certificateSerialNumber = certificateSerialNumber.Trim();
			certificateSerialNumber = certificateSerialNumber.Replace(" ", "");

			// Get the certificate identified by the serial number from the Local Machine store.
			this.client.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySerialNumber, certificateSerialNumber);
		} // InitializeCertificate

		private void InitializeCertificate(String certificateFile, String certificatePassword) {
			this.client.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(certificateFile, certificatePassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
		} // ImportCertificate
		#endregion

		#region Public methods.
		public override String ToString() {
			StringBuilder result = new StringBuilder();
			result.AppendLine(String.Format("Service"));
			result.AppendLine(String.Format("          Service Name: {0}", this.GetServiceName()));
			result.AppendLine(String.Format("           Service URL: {0}", this.serviceURL));
			result.AppendLine(String.Format("          Service UUID: {0}", this.serviceUUID));
			result.AppendLine(String.Format("Service Agreement UUID: {0}", this.serviceAgreementUUID));
			result.AppendLine(String.Format("      User System UUID: {0}", this.userSystemUUID));
			result.AppendLine(String.Format("             User UUID: {0}", this.userUUID));
			result.AppendLine();
			if (this.ServiceCertificate != null) {
				result.AppendLine(String.Format("Certificate"));
				result.AppendLine(String.Format("         Friendly name: {0}", this.ServiceCertificate.FriendlyName));
				result.AppendLine(String.Format("               Version: {0}", this.ServiceCertificate.Version));
				result.AppendLine(String.Format("         Serial number: {0}", this.ServiceCertificate.SerialNumber));
				result.AppendLine(String.Format("            Thumbprint: {0}", this.ServiceCertificate.Thumbprint));
				result.AppendLine(String.Format("               Subject: {0}", this.ServiceCertificate.Subject));
				result.AppendLine(String.Format("                Issuer: {0}", this.ServiceCertificate.Issuer));
				result.AppendLine(String.Format("            Not before: {0}", this.ServiceCertificate.NotBefore.ToString("yyyy-MM-dd")));
				result.AppendLine(String.Format("             Not after: {0}", this.ServiceCertificate.NotAfter.ToString("yyyy-MM-dd")));
				result.AppendLine(String.Format("   Signature algorithm: {0}", this.ServiceCertificate.SignatureAlgorithm.FriendlyName));
				result.AppendLine(String.Format("       Has private key: {0}", this.ServiceCertificate.HasPrivateKey));
			}
			return result.ToString();
		} // ToString
		#endregion

		#region Public abstract methods.
		public abstract String GetServiceName();

		public abstract Dictionary<String, String> Query(String cpr);
		#endregion

	} // RpcServiceplatformBase
	#endregion

	#region RpcServiceplatformCprNavne3
	public class RpcServiceplatformCprNavne3 : RpcServiceplatformBase {

		#region Constructors.
		public RpcServiceplatformCprNavne3(String serviceUrl, Guid serviceUuid, Guid serviceAgreementUuid, Guid userSystemUuid, Guid userUuid, String accountingInfo, String accountingUser, String certificateSerialNumber) : base(serviceUrl, serviceUuid, serviceAgreementUuid, userSystemUuid, userUuid, accountingInfo, accountingUser, certificateSerialNumber) {
		} // RpcServiceplatformCprNavne3
		#endregion

		#region Public methods
		public override String GetServiceName() {
			return "NAVNE3";
		} // GetServiceName

		public override Dictionary<String, String> Query(String cpr) {
			try {
				// Validate arguments.
				if ((cpr == null) || (cpr.Trim().Length == 0)) {      // CPR is empty or null.
					throw new ArgumentException("The 'cpr' argument is null or empty.");
				}
				if (cpr.IndexOf('-') == 6) {            // Remove dash in CPR.
					cpr = cpr.Substring(0, 6) + cpr.Substring(7);
				}
				if (cpr.Length != 10) {                 // CPR length is different from 10 chars.
					throw new ArgumentException("The 'cpr' argument is invalid.");
				}
				//ÆØÅ
				//if (cpr.ToInt64(-1) == -1) {            // CPR is not a number.
				//	throw new ArgumentException("The 'cpr' argument is invalid.");
				//}

				// Build the request query string.
				StringBuilder requestGCTP = new StringBuilder();
				requestGCTP.AppendLine("<Gctp v=\"1.0\">");
				requestGCTP.AppendLine("	<System r=\"CprSoeg\">");
				requestGCTP.AppendLine("		<Service r=\"NAVNE3\">");
				requestGCTP.AppendLine("			<CprServiceHeader r=\"NAVNE3\">");
				requestGCTP.AppendLine("				<Key>");
				requestGCTP.AppendLine("					<Field r=\"PNR\" v=\"" + cpr + "\"/>");
				requestGCTP.AppendLine("				</Key>");
				requestGCTP.AppendLine("			</CprServiceHeader>");
				requestGCTP.AppendLine("		</Service>");
				requestGCTP.AppendLine("	</System>");
				requestGCTP.AppendLine("</Gctp>");

				// Send the request query and get the result.
				GCTPLookupRequestType request = new GCTPLookupRequestType();
				request.gctpMessage = requestGCTP.ToString();
				request.InvocationContext = this.ServiceContext;
				GCTPLookupResponseType response = this.Client.callGctpService(request);

				// Get the returned properties.
				XmlDocument xml = new XmlDocument();
				xml.LoadXml(response.result);
				XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xml.NameTable);
				xmlNamespaceManager.AddNamespace("cpr", "http://www.cpr.dk");

				// Get the result status.
				XmlNode xmlStatusNode = xml.SelectSingleNode("//cpr:Kvit", xmlNamespaceManager);
				if (xmlStatusNode == null) {
					throw new Exception("Invalid result status received.");
				}
				Int32 statusCode = Int32.Parse(xmlStatusNode.Attributes["v"].Value);
				String statusText = xmlStatusNode.Attributes["t"].Value;

				// Get the result data.
				//XmlNode xmlRowNode = xml.SelectSingleNode("/cpr:Gctp/cpr:System/cpr:Service/cpr:CprData/cpr:Rolle/cpr:Table/cpr:Row[@k='0']", xmlNamespaceManager);
				XmlNode xmlRowNode = xml.SelectSingleNode("//cpr:Row[@k='0']", xmlNamespaceManager);
				if (xmlRowNode == null) {
					throw new Exception("Invalid result data received.");
				}
				Dictionary<String, String> result = new Dictionary<String, String>();
				foreach (XmlNode xmlFieldNode in xmlRowNode.ChildNodes) {
					result.Add(xmlFieldNode.Attributes["r"].Value, xmlFieldNode.Attributes["v"].Value);
				}

				/* Example Result:

				<root xmlns="http://www.cpr.dk">
					<Gctp v="1.0">
						<System r="CprSoeg">
							<Service r="navne3">
								<CprServiceHeader r="navne3" />
								<CprData u="O">
									<Rolle r="HovedRolle">
										<Table r="navne3">
											<Row k="0">
												<Field r="CNVN_ADRNVN" t="Christensen, René Paw" v="René Paw Christensen" />
												<Field r="CNVN_EFTERNVN" v="Christensen" />
												<Field r="CNVN_FORNVN" v="René Paw" />
												<Field r="CNVN_KOEN" v="M" />
												<Field r="CNVN_STARTDATO" v="198510092000" />
												<Field r="CNVN_STATUS" v="01" />
												<Field r="HISTORIKMRK" v="X" />
											</Row>
										</Table>
									</Rolle>
								</CprData>
								<Kvit r="Ok" t="" v="0" />
							</Service>
						</System>
					</Gctp>
				</root>

				*/

				// Return the result.
				return result;
			} catch (Exception exception) {
				// Throw the exception.
				throw exception;
			}
		} // Query
		#endregion

	} // RpcServiceplatformCprNavne3
	#endregion

	#region RpcServiceplatformCprAdresse1
	public class RpcServiceplatformCprAdresse1 : RpcServiceplatformBase {

		#region Constructors.
		public RpcServiceplatformCprAdresse1(String serviceUrl, Guid serviceUuid, Guid serviceAgreementUuid, Guid userSystemUuid, Guid userUuid, String accountingInfo, String accountingUser, String certificateSerialNumber) : base(serviceUrl, serviceUuid, serviceAgreementUuid, userSystemUuid, userUuid, accountingInfo, accountingUser, certificateSerialNumber) {
		} // RpcServiceplatformCprAdresse1
		#endregion

		#region Public methods
		public override String GetServiceName() {
			return "ADRSOG1";
		} // GetServiceName

		public override Dictionary<String, String> Query(String cpr) {
			try {
				// Validate arguments.
				if ((cpr == null) || (cpr.Trim().Length == 0)) {      // CPR is empty or null.
					throw new ArgumentException("The 'cpr' argument is null or empty.");
				}
				if (cpr.IndexOf('-') == 6) {            // Remove dash in CPR.
					cpr = cpr.Substring(0, 6) + cpr.Substring(7);
				}
				if (cpr.Length != 10) {                 // CPR length is different from 10 chars.
					throw new ArgumentException("The 'cpr' argument is invalid.");
				}
				//if (cpr.ToInt64(-1) == -1) {            // CPR is not a number.
				//	throw new ArgumentException("The 'cpr' argument is invalid.");
				//}

				// Build the request query string.
				StringBuilder requestGCTP = new StringBuilder();
				requestGCTP.AppendLine("<Gctp v=\"1.0\">");
				requestGCTP.AppendLine("	<System r=\"CprSoeg\">");
				requestGCTP.AppendLine("		<Service r=\"ADRESSE1\">");
				requestGCTP.AppendLine("			<CprServiceHeader r=\"ADRESSE1\">");
				requestGCTP.AppendLine("				<Key>");
				requestGCTP.AppendLine("					<Field r=\"PNR\" v=\"" + cpr + "\"/>");
				requestGCTP.AppendLine("					<Field r=\"AKX\" v=\"X\"/>");
				requestGCTP.AppendLine("					<Field r=\"MAXA\" v=\"100\"/>");
				requestGCTP.AppendLine("					<Field r=\"AIA\" v=\"X\"/>");
				requestGCTP.AppendLine("				</Key>");
				requestGCTP.AppendLine("			</CprServiceHeader>");
				requestGCTP.AppendLine("		</Service>");
				requestGCTP.AppendLine("	</System>");
				requestGCTP.AppendLine("</Gctp>");
				Console.WriteLine(requestGCTP.ToString());

				// Send the request query and get the result.
				GCTPLookupRequestType request = new GCTPLookupRequestType();
				request.gctpMessage = requestGCTP.ToString();
				request.InvocationContext = this.ServiceContext;
				GCTPLookupResponseType response = this.Client.callGctpService(request);
				Console.WriteLine(response.result);

				// Get the returned properties.
				XmlDocument xml = new XmlDocument();
				xml.LoadXml(response.result);
				XmlNode xmlRow = xml.SelectSingleNode("/Gctp/System/Service/CprData/Rolle/Table/Row[@k=0]");
				Dictionary<String, String> result = new Dictionary<String, String>();
				//				foreach (XmlNode xmlField in xmlRow.ChildNodes) {
				//					result.Add(xmlField.Attributes["r"].Value, xmlField.Attributes["v"].Value);
				//				}

				/* Example Result:

				<Gctp v="1.0">
					<System r="CprSoeg">
						<Service r="navne3">
							<CprServiceHeader r="navne3"/>
							<CprData u="O">
								<Rolle r="HovedRolle">
									<Table r="navne3">
										<Row k="0">
											<Field r="CNVN_ADRNVN" v="René Paw Christensen"/>
											<Field r="CNVN_EFTERNVN" v="Christensen"/>
											<Field r="CNVN_FORNVN" v="René Paw"/>
											<Field r="CNVN_KOEN" v="M"/>
											<Field r="CNVN_STARTDATO" v="198510092000"/>
											<Field r="CNVN_STATUS" v="01"/>
										</Row>
									</Table>
								</Rolle>
							</CprData>
						</Service>
					</System>
				</Gctp>

				*/

				// Return the result.
				return result;
			} catch (Exception exception) {
				// Throw the exception.
				throw exception;
			}
		} // Query
		#endregion

	} // RpcServiceplatformCprAdresse1
	#endregion

} // NDK.Framework.CprBroker