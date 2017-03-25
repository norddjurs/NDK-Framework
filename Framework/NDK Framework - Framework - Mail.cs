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
	/// This partial part of the class, implements email messaging.
	/// </summary>
	public abstract partial class Framework : IFramework {

		#region Private mail initialization
		private void MailInitialize() {
		} // MailInitialize
		#endregion

		#region Public mail methods.
		/// <summary>
		/// Send e-mail message as plain text or html to the configured service desk recepient.
		/// </summary>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="textIsHtml">True if the message text is html code.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		public Boolean SendMail(String subject, String text, Boolean textIsHtml, params String[] attachments) {
			// Get configuration.
			String from = this.GetSystemValue("SmtpFrom", "noreply@internal");
			String to = this.GetSystemValue("SmtpTo", String.Empty);

			// Send the message.
			return this.SendMailFrom(from, to, subject, text, textIsHtml, attachments);
		} // SendMail

		/// <summary>
		/// Send e-mail message as plain text.
		/// </summary>
		/// <param name="to">One or more to addresses.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		public Boolean SendMail(String to, String subject, String text, params String[] attachments) {
			// Get configuration.
			String from = this.GetSystemValue("SmtpFrom", "noreply@internal");

			// Send the message.
			return this.SendMailFrom(from, to, subject, text, false, attachments);
		} // SendMail

		/// <summary>
		/// Send e-mail message as plain text or html.
		/// </summary>
		/// <param name="to">One or more to addresses.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="textIsHtml">True if the message text is html code.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		public Boolean SendMail(String to, String subject, String text, Boolean textIsHtml, params String[] attachments) {
			// Get configuration.
			String from = this.GetSystemValue("SmtpFrom", "noreply@internal");

			// Send the message.
			return this.SendMailFrom(from, to, subject, text, textIsHtml, attachments);
		} // SendMail

		/// <summary>
		/// Send e-mail message as plain text or html.
		/// </summary>
		/// <param name="from">The from address.</param>
		/// <param name="to">One or more to addresses.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="textIsHtml">True if the message text is html code.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		public Boolean SendMailFrom(String from, String to, String subject, String text, Boolean textIsHtml, params String[] attachments) {
			try {
				// Get configuration.
				String smtpHost = this.GetSystemValue("SmtpHost");
				Int32 smtpPort = 25;
				Int32.TryParse(this.GetSystemValue("SmtpPort", "25"), out smtpPort);

				// Log.
				this.LogInternal("Mail: Sending '{2}' to '{1}' from '{0}'. Message contain {3} character(s).", from, to, subject, text.Length);

				using (MailMessage message = new MailMessage()) {
					// Create the message.
					message.From = new MailAddress(from);
					foreach (String to1 in to.Split(new Char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)) {
						message.To.Add(to1);
					}
					message.SubjectEncoding = Encoding.UTF8;
					message.Subject = subject;
					message.BodyEncoding = Encoding.UTF8;
					message.IsBodyHtml = textIsHtml;
					message.Body = text;

					// Attach files.
					foreach (String attachment in attachments) {
						if ((attachment != null) && (File.Exists(attachment) == true)) {
							message.Attachments.Add(new Attachment(attachment));
							this.LogInternal("Mail: Attaching file '{0}'.", attachment);
						} else {
							this.LogError("Mail: Attachment not found '{0}'.", attachment);
						}
					}

					// Send the message.
					using (SmtpClient client = new SmtpClient(smtpHost, smtpPort)) {
						client.Send(message);
					}
				}

				// Success.
				return true;
			} catch (Exception exception) {
				// Log.
				this.LogError(exception);

				// Failure.
				return false;
			}
		} // SendMailFrom
		#endregion

	} // Framework
	#endregion

} // NDK.Framework