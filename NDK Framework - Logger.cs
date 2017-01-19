using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace NDK.Framework {

	#region ILogger interface.
	/// <summary>
	/// Logger interface.
	/// The implementing class should know the configured logging levels, and only actually log the configured events.
	/// </summary>
	public interface ILogger {

		/// <summary>
		/// Writes the text to the log.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		void Log(String text, params Object[] formatArgs);

		/// <summary>
		/// Writes the text to the debug log.
		/// The information is only logged, if DEBUG logging is configures.
		/// </summary>
		/// <param name="text">The debug text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		void LogDebug(String text, params Object[] formatArgs);

		/// <summary>
		/// Writes the text to the error log.
		/// The information is only logged, if ERROR logging is configures.
		/// </summary>
		/// <param name="text">The error text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		void LogError(String text, params Object[] formatArgs);

		/// <summary>
		/// Writes the exception and stack traces to the error log.
		/// The information is only logged, if ERROR logging is configures.
		/// </summary>
		/// <param name="exception">The exception.</param>
		void LogError(Exception exception);

	} // ILogger
	#endregion

	#region LoggerFlags enum.
	/// <summary>
	/// Logger flags.
	/// </summary>
	[Flags]
	public enum LoggerFlags {
		/// <summary>
		/// Log normal information.
		/// </summary>
		Empty = 0x0000,

		/// <summary>
		/// Log normal information.
		/// Written as information in the Windows event log.
		/// </summary>
		Normal = 0x0002,

		/// <summary>
		/// Log debug information.
		/// Written as warnings in the Windows event log.
		/// </summary>
		Debug = 0x0004,

		/// <summary>
		/// Log error information.
		/// Written as errors in the Windows event log.
		/// </summary>
		Error = 0x0008,

		/// <summary>
		/// Log all information.
		/// </summary>
		Everything = LoggerFlags.Normal | LoggerFlags.Debug | LoggerFlags.Error,

		/// <summary>
		/// Log to the console.
		/// The log is written to STDOUT.
		/// </summary>
		Console = 0x0128,

		/// <summary>
		/// Log to the file system.
		/// The log file name is the same as the entry assembly (.exe), but with the ".log" file extension.
		/// </summary>
		File = 0x0256,

		/// <summary>
		/// Log to the windows event log.
		/// The log is written to the default "Application" event log.
		/// </summary>
		Windows = 0x0512,

		/// <summary>
		/// Log to all destinations.
		/// </summary>
		Everywhere = LoggerFlags.Console | LoggerFlags.File | LoggerFlags.Windows,
	} // LoggerFlags
	#endregion

	#region Logger class.
	/// <summary>
	/// Default ILogger implementation, that can log to the console, the windows event log or to the file system.
	/// </summary>
	public class Logger : ILogger {
		private LoggerFlags logFlags = LoggerFlags.Empty;
		private String logFileName = null;
		private StreamWriter logFile = null;

		/// <summary>
		/// Initialzes a new logger, using the flags provided.
		/// The default log filename is the path and name of the executeable file, but with the ".log" extension.
		/// </summary>
		/// <param name="logFlags">The logger flags.</param>
		public Logger(LoggerFlags logFlags) {
			this.logFlags = logFlags;
			this.logFileName = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "log");
			this.logFile = null;

			// Log.
			this.Log(
				"***** begin new process *****" + Environment.NewLine +
				"Log options: From application." + Environment.NewLine +
				"Log file: {1}." + Environment.NewLine +
				"Log flags: {0}",
				this.logFlags,
				(this.logFileName != null) ? this.logFileName : "<not used>"
			);
		} // Logger

		/// <summary>
		/// Initializes a new logger, using the configuration provided.
		/// The following global properties can be configured to enable logging:
		/// 
		///		LogNormal		true | 1
		///		LogDebug		true | 1
		///		LogError		true | 1
		///		LogConsole		true | 1
		///		LogFile			true | 1 | [full log filename]
		///		LogWindows		true | 1
		/// 
		/// The default log filename is the path and name of the executeable file, but with the ".log" extension.
		/// When configuring a log filename, the directory must exist.
		/// </summary>
		/// <param name="config">The logger configuration.</param>
		public Logger(IConfiguration config) {
			// Default.
			this.logFlags = LoggerFlags.Empty;
			this.logFileName = null;
			this.logFile = null;

			if ((config.GetValue("LogNormal", String.Empty).ToLower() == "true") ||
				(config.GetValue("LogNormal", String.Empty).ToLower() == "1")) {
				this.logFlags |= LoggerFlags.Normal;
			}

			if ((config.GetValue("LogDebug", String.Empty).ToLower() == "true") ||
				(config.GetValue("LogDebug", String.Empty).ToLower() == "1")) {
				this.logFlags |= LoggerFlags.Debug;
			}

			if ((config.GetValue("LogError", String.Empty).ToLower() == "true") ||
				(config.GetValue("LogError", String.Empty).ToLower() == "1")) {
				this.logFlags |= LoggerFlags.Error;
			}

			if ((config.GetValue("LogConsole", String.Empty).ToLower() == "true") ||
				(config.GetValue("LogConsole", String.Empty).ToLower() == "1")) {
				this.logFlags |= LoggerFlags.Console;
			}

			if ((config.GetValue("LogFile", String.Empty).ToLower() == "true") ||
				(config.GetValue("LogFile", String.Empty).ToLower() == "1")) {
				this.logFlags |= LoggerFlags.File;
				this.logFileName = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "log");
			}

			if ((config.GetValue("LogFile", String.Empty).ToLower() != String.Empty) &&
				(Directory.Exists(Path.GetDirectoryName(config.GetValue("LogFile", String.Empty))) == true)) {
				this.logFlags |= LoggerFlags.File;
				this.logFileName = config.GetValue("LogFile");
			}

			if ((config.GetValue("LogWindows", String.Empty).ToLower() == "true") ||
				(config.GetValue("LogWindows", String.Empty).ToLower() == "1")) {
				this.logFlags |= LoggerFlags.Windows;
			}

			// Log.
			this.Log(
				"***** begin new process *****" + Environment.NewLine +
				"Log options: From configuration file." + Environment.NewLine +
				"Log file: {1}." + Environment.NewLine +
				"Log flags: {0}",
				this.logFlags,
				(this.logFileName != null) ? this.logFileName  : "<not used>"
			);
		} // Logger

		public LoggerFlags LogFlags {
			get {
				return this.logFlags;
			}
		} // LogFlags

		public String LogFileName {
			get {
				return this.logFileName;
			}
		} // LogFileName

		/// <summary>
		/// Writes the text to the log.
		/// </summary>
		/// <param name="logFlags">The desired log flags.</param>
		/// <param name="text">The text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		private void Log(LoggerFlags logFlags, String text, params Object[] formatArgs) {
			// Format the text.
			if (formatArgs.Length > 0) {
				try {
					text = String.Format(text, formatArgs);
				} catch { }
			}

			// Split the text at each newline.
			String[] textLines = text.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);

			// Log to the console.
			if ((this.logFlags & LoggerFlags.Console) == LoggerFlags.Console) {
				try {
					// Add header.
					String textHeader = String.Format("{0:ddd dd MMM HH:mm:ss} | {1:000000} | ", DateTime.Now, Process.GetCurrentProcess().Id);

					// Log normal information.
					if (((this.logFlags & LoggerFlags.Normal) == LoggerFlags.Normal) &&
						((logFlags & LoggerFlags.Normal) == LoggerFlags.Normal)) {
						Console.ResetColor();
						foreach (String textLine in textLines) {
							Console.WriteLine(textHeader + "Norml: " + textLine);
						}
					}

					// Log debug information.
					if (((this.logFlags & LoggerFlags.Debug) == LoggerFlags.Debug) &&
						((logFlags & LoggerFlags.Debug) == LoggerFlags.Debug)) {
						Console.ForegroundColor = ConsoleColor.DarkBlue;
						foreach (String textLine in textLines) {
							Console.WriteLine(textHeader + "Debug: " + textLine);
						}
						Console.ResetColor();
					}

					// Log error information.
					if (((this.logFlags & LoggerFlags.Error) == LoggerFlags.Error) &&
						((logFlags & LoggerFlags.Error) == LoggerFlags.Error)) {
						Console.ForegroundColor = ConsoleColor.DarkRed;
						foreach (String textLine in textLines) {
							Console.WriteLine(textHeader + "Error: " + textLine);
						}
						Console.ResetColor();
					}
				} catch {}
			}

			// Log to the file system.
			if ((this.logFlags & LoggerFlags.File) == LoggerFlags.File) {
				try {
					// Open the log file.
					if (this.logFile == null) {
						this.logFile = new StreamWriter(this.logFileName, true);
						this.logFile.AutoFlush = true;
					}
				} catch {}

				try {
					// Add header.
					String textHeader = String.Format("{0:ddd dd MMM HH:mm:ss} | {1:000000} | ", DateTime.Now, Process.GetCurrentProcess().Id);

					// Log normal information.
					if (((this.logFlags & LoggerFlags.Normal) == LoggerFlags.Normal) &&
						((logFlags & LoggerFlags.Normal) == LoggerFlags.Normal)) {
						foreach (String textLine in textLines) {
							this.logFile.WriteLine(textHeader + "Norml: " + textLine);
						}
					}

					// Log debug information.
					if (((this.logFlags & LoggerFlags.Debug) == LoggerFlags.Debug) &&
						((logFlags & LoggerFlags.Debug) == LoggerFlags.Debug)) {
						foreach (String textLine in textLines) {
							this.logFile.WriteLine(textHeader + "Debug: " + textLine);
						}
					}

					// Log error information.
					if (((this.logFlags & LoggerFlags.Error) == LoggerFlags.Error) &&
						((logFlags & LoggerFlags.Error) == LoggerFlags.Error)) {
						foreach (String textLine in textLines) {
							this.logFile.WriteLine(textHeader + "Error: " + textLine);
						}
					}
				} catch { }
			}

			// Log to the windows event log.
			if ((this.logFlags & LoggerFlags.Windows) == LoggerFlags.Windows) {
				try {
					// Log normal information.
					if (((this.logFlags & LoggerFlags.Normal) == LoggerFlags.Normal) &&
						((logFlags & LoggerFlags.Normal) == LoggerFlags.Normal)) {
						EventLog.WriteEntry("NDK Framework", text, EventLogEntryType.Information);
					}

					// Log debug information.
					if (((this.logFlags & LoggerFlags.Debug) == LoggerFlags.Debug) &&
						((logFlags & LoggerFlags.Debug) == LoggerFlags.Debug)) {
						EventLog.WriteEntry("NDK Framework", text, EventLogEntryType.Warning);
					}

					// Log error information.
					if (((this.logFlags & LoggerFlags.Error) == LoggerFlags.Error) &&
						((logFlags & LoggerFlags.Error) == LoggerFlags.Error)) {
						EventLog.WriteEntry("NDK Framework", text, EventLogEntryType.Error);
					}
				} catch {}
			}
		} // Log

		#region Implement ILogger interface.
		/// <summary>
		/// Writes the text to the log.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		public void Log(String text, params Object[] formatArgs) {
			this.Log(LoggerFlags.Normal, text, formatArgs);
		} // Log

		/// <summary>
		/// Writes the text to the debug log.
		/// The information is only logged, if DEBUG logging is configures.
		/// </summary>
		/// <param name="text">The debug text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		public void LogDebug(String text, params Object[] formatArgs) {
			this.Log(LoggerFlags.Debug, text, formatArgs);
		} // LogDebug

		/// <summary>
		/// Writes the text to the error log.
		/// The information is only logged, if ERROR logging is configures.
		/// </summary>
		/// <param name="text">The error text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		public void LogError(String text, params Object[] formatArgs) {
			this.Log(LoggerFlags.Error, text, formatArgs);
		} // LogError

		/// <summary>
		/// Writes the exception and stack traces to the error log.
		/// The information is only logged, if ERROR logging is configures.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public void LogError(Exception exception) {
			if (exception.Message != null) {
				this.Log(LoggerFlags.Error, exception.Message);
			}
			if (exception.StackTrace != null) {
				this.Log(LoggerFlags.Debug, exception.StackTrace);
			}
		} // LogError
		#endregion

	} // Logger
	#endregion

} // NDK.Framework