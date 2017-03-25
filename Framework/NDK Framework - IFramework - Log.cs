using System;
using System.Collections.Generic;

namespace NDK.Framework {

	#region IFramework
	/// <summary>
	/// This partial part of the interface, defines logging.
	/// </summary>
	public partial interface IFramework {

		#region Logging events.
		/// <summary>
		/// Occurs when something is logged, and event logging is enabled.
		/// </summary>
		event LoggerEventHandler OnLog;
		#endregion

		#region Logging methods.
		/// <summary>
		/// Writes the text to the log.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		void Log(String text, params Object[] formatArgs);

		/// <summary>
		/// Writes the text to the debug log.
		/// The information is only logged, if DEBUG logging is configured.
		/// </summary>
		/// <param name="text">The debug text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		void LogDebug(String text, params Object[] formatArgs);

		/// <summary>
		/// Writes the text to the internal debug log.
		/// The information is only logged, if INTNL logging is configured.
		/// </summary>
		/// <param name="text">The debug text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		//internal void LogInternal(String text, params Object[] formatArgs);

		/// <summary>
		/// Writes the text to the error log.
		/// The information is only logged, if ERROR logging is configured.
		/// </summary>
		/// <param name="text">The error text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		void LogError(String text, params Object[] formatArgs);

		/// <summary>
		/// Writes the exception and stack traces to the error log.
		/// The information is only logged, if ERROR logging is configured.
		/// </summary>
		/// <param name="exception">The exception.</param>
		void LogError(Exception exception);
		#endregion

	} // IFramework
	#endregion

} // NDK.Framework