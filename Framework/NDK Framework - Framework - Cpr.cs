using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;
using NDK.Framework.CprBroker;

namespace NDK.Framework {

	#region Framework
	/// <summary>
	/// This partial part of the class, implements CPR lookup.
	/// </summary>
	public abstract partial class Framework : IFramework {
		private Int32 cprEngine = 0;

		// CPR Broker.
		private String cprBrokerServiceUrl = null;
		private String cprBrokerApplicationName = null;
		private String cprBrokerApplicationToken = null;
		private String cprBrokerUserToken = null;

		// Kombit Serviceplatform NAVNE3.
		private String cprNavne3ServiceUrl = null;
		private Guid cprNavne3ServiceUuid = Guid.Empty;
		private Guid cprNavne3ServiceAgreementUuid = Guid.Empty;
		private Guid cprNavne3UserSystemUuid = Guid.Empty;
		private Guid cprNavne3UserUuid = Guid.Empty;
		private String cprNavne3AccountingInfo = null;
		private String cprNavne3AccountingUser = null;
		private String cprNavne3CertificateSerialNumber = null;

		#region Private CPR initialization
		private void CprInitialize() {
			this.cprEngine = this.GetSystemValue("CprEngine", 0);

			// CPR Broker.
			this.cprBrokerServiceUrl = this.GetSystemValue("CprBrokerServiceUrl", String.Empty);
			this.cprBrokerApplicationName = this.GetSystemValue("CprBrokerApplicationName", "NDK Framework");
			this.cprBrokerApplicationToken = this.GetSystemValue("CprBrokerApplicationToken", String.Empty);
			this.cprBrokerUserToken = Environment.UserName;

			// Kombit Serviceplatform NAVNE3.
			this.cprNavne3ServiceUrl = this.GetSystemValue("CprNavne3ServiceUrl", "https://prod.serviceplatformen.dk/service/CPRLookup/CPRLookup/2");
			this.cprNavne3ServiceUuid = this.GetSystemValue("CprNavne3ServiceUuid", new Guid("e3f26293-803e-4b8f-8a15-674a4f8abaad"));
			this.cprNavne3ServiceAgreementUuid = this.GetSystemValue("CprNavne3AgreementUuid", Guid.Empty);
			this.cprNavne3UserSystemUuid = this.GetSystemValue("CprNavne3UserSystemUuid", Guid.Empty);
			this.cprNavne3UserUuid = this.GetSystemValue("CprNavne3UserUuid", Guid.Empty);
			this.cprNavne3AccountingInfo = this.GetSystemValue("CprNavne3AccountingInfo", "NDK Framework");
			this.cprNavne3AccountingUser = Environment.UserName;
			this.cprNavne3CertificateSerialNumber = this.GetSystemValue("CprNavne3CerfiticateSN", String.Empty);
		} // MailInitialize
		#endregion

		#region Public CPR methods.
		public CprSearchResult CprSearch(String cprNumber) {
			switch (this.cprEngine) {
				case 0:
					// Search with CPR Broker.
					return this.CprBrokerSearch(cprNumber);
					break;
				case 1:
					// Search with Kombit Serviceplatform.
					return this.CprKombitSearch(cprNumber);
					break;
				default:
					// Log.
					this.LogError("The CPR search engine '{0}' is not valid.", this.cprEngine);

					// Return null.
					return null;
					break;
			}
		} // CprSearch
		#endregion

		#region Private CPR methods.
		private CprSearchResult CprBrokerSearch(String cprNumber) {
			try {
				// Format CPR number.
				cprNumber = cprNumber.FormatStringCpr();

				RpcCprBroker cprBroker = new RpcCprBroker(cprBrokerServiceUrl, cprBrokerApplicationName, cprBrokerApplicationToken, cprBrokerUserToken);
				NDK.Framework.CprBroker.Admin.ApplicationType cprBrokerAppType = cprBroker.ApplicationIsRegistered();
				if (cprBrokerAppType == null) {
					// The application is not registed in CPR Broker.
					throw new Exception(String.Format("The application has not been registered in CPR Broker.\r\nApplication Name: {0}\r\nApplication Token: {1}", cprBrokerApplicationName, cprBrokerApplicationToken));
				} else if (cprBrokerAppType.IsApproved == false) {
					// The application is not approved.
					throw new Exception(String.Format("The application has not been registered in CPR Broker, but not approved by the administrator.\r\nApplication Name: {0}\r\nApplication Token: {1}", cprBrokerApplicationName, cprBrokerApplicationToken));
				}

				// Search single item.
				Guid cprBrokerUserGuid = cprBroker.PartGetUUID(cprNumber);
				NDK.Framework.CprBroker.Part.LaesOutputType cprBrokerResult = cprBroker.PartRead(cprBrokerUserGuid);

				// Return the result.
				return new CprSearchResult(cprNumber, cprBrokerResult);
			} catch (Exception exception) {
				// Log.
				this.LogError(exception);

				// Return null.
				return null;
			}
		} // CprBrokerSearch

		private CprSearchResult CprKombitSearch(String cprNumber) {
			try {
				// Search single item.
				RpcServiceplatformCprNavne3 cpr = new RpcServiceplatformCprNavne3(
					cprNavne3ServiceUrl,
					cprNavne3ServiceUuid,
					cprNavne3ServiceAgreementUuid,
					cprNavne3UserSystemUuid,
					cprNavne3UserUuid,
					cprNavne3AccountingInfo,
					cprNavne3AccountingUser,
					cprNavne3CertificateSerialNumber
				);
				Dictionary<String, String> cprKombitResult = cpr.Query(cprNumber);

				// Return the result.
				return new CprSearchResult(cprNumber, cprKombitResult);
			} catch (Exception exception) {
				// Log.
				this.LogError(exception);

				// Return null.
				return null;
			}
		} // CprKombitSearch
		#endregion

	} // Framework
	#endregion

} // NDK.Framework