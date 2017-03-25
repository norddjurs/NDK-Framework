using System;
using System.Collections.Generic;
using System.Data;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines email messaging.
	/// </summary>
	public partial interface IFramework {

		#region Mail methods.
		/// <summary>
		/// Send e-mail message as plain text or html to the configured service desk recepient.
		/// </summary>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="textIsHtml">True if the message text is html code.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		Boolean SendMail(String subject, String text, Boolean textIsHtml, params String[] attachments);

		/// <summary>
		/// Send e-mail message as plain text.
		/// </summary>
		/// <param name="to">One or more to addresses.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		Boolean SendMail(String to, String subject, String text, params String[] attachments);

		/// <summary>
		/// Send e-mail message as plain text or html.
		/// </summary>
		/// <param name="to">One or more to addresses.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="text">The message text.</param>
		/// <param name="textIsHtml">True if the message text is html code.</param>
		/// <param name="attachments">The attachments (filenames).</param>
		/// <returns>True if the e-mail was send.</returns>
		Boolean SendMail(String to, String subject, String text, Boolean textIsHtml, params String[] attachments);

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
		Boolean SendMailFrom(String from, String to, String subject, String text, Boolean textIsHtml, params String[] attachments);
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework