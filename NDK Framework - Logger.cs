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
		/// Log to C# events.
		/// The log is send to all registed event handlers.
		/// </summary>
		Event = 0x1024,

		/// <summary>
		/// Log to all destinations.
		/// </summary>
		Everywhere = LoggerFlags.Console | LoggerFlags.File | LoggerFlags.Windows | LoggerFlags.Event,
	} // LoggerFlags
	#endregion

	#region Logger delegates.
	public delegate void LoggerEventHandler(LoggerFlags logFlags, String text);
	#endregion

	#region Logger class.
	/// <summary>
	/// Default ILogger implementation, that can log to the console, the windows event log or to the file system.
	/// </summary>
	public class Logger : ILogger {
		private event LoggerEventHandler logLogEvents = null;
		private LoggerFlags logFlags = LoggerFlags.Empty;
		private String logFileName = null;
		private Int32 logFileRollSize = 0;
		private Int32 logFileRollCount = 0;
		private StreamWriter logFile = null;

		#region Constructors
		/// <summary>
		/// Initialzes a new logger, using the flags provided.
		/// The default log filename is the path and name of the executeable file, but with the ".log" extension.
		/// </summary>
		/// <param name="logFlags">The logger flags.</param>
		public Logger(LoggerFlags logFlags) {
			this.logFlags = logFlags;
			this.logFileName = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "log");
			this.logFileRollSize = 0;
			this.logFileRollCount = 0;
			this.logFile = null;
			this.logLogEvents = null;

			// Log.
			this.Log(
				"***** begin new process *****" + Environment.NewLine +
				"Log options: From application." + Environment.NewLine +
				"Log file: {1}  (Roll at {2} bytes, Keep {3} log files)." + Environment.NewLine +
				"Log flags: {0}",
				this.logFlags,
				(this.logFileName != null) ? this.logFileName : "<not used>",
				(this.logFileRollSize > 0) ? this.logFileRollSize : 0,
				(this.logFileRollCount > 0) ? this.logFileRollCount : 0
			);
		} // Logger

		/// <summary>
		/// Initializes a new logger, using the configuration provided.
		/// The following global properties can be configured to enable logging:
		/// 
		///		LogNormal			true | 1
		///		LogDebug			true | 1
		///		LogError			true | 1
		///		LogConsole			true | 1
		///		LogWindows			true | 1
		///		LogEvent			true | 1
		///		LogFile				true | 1 | [full log filename]
		///		LogFileRollSizeMB	Roll the file log at size in megabytes
		///		LogFileRollCount	Keep number of log rolls.
		/// 
		/// The default log filename is the path and name of the executeable file, but with the ".log" extension.
		/// When configuring a log filename, the directory must exist.
		/// </summary>
		/// <param name="config">The logger configuration.</param>
		public Logger(IConfiguration config) {
			// Default.
			this.logFlags = LoggerFlags.Empty;
			this.logFileName = null;
			this.logFileRollSize = 0;
			this.logFileRollCount = 0;
			this.logFile = null;
			this.logLogEvents = null;

			if (config.GetValue("LogNormal", false) == true) {
				this.logFlags |= LoggerFlags.Normal;
			}

			if (config.GetValue("LogDebug", false) == true) {
				this.logFlags |= LoggerFlags.Debug;
			}

			if (config.GetValue("LogError", false) == true) {
				this.logFlags |= LoggerFlags.Error;
			}

			if (config.GetValue("LogConsole", false) == true) {
				this.logFlags |= LoggerFlags.Console;
			}

			if (config.GetValue("LogWindows", false) == true) {
				this.logFlags |= LoggerFlags.Windows;
			}

			if (config.GetValue("LogEvent", false) == true) {
				this.logFlags |= LoggerFlags.Event;
			}

			if (config.GetValue("LogFile", false) == true) {
				this.logFlags |= LoggerFlags.File;
				this.logFileName = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "log");
			}

			if ((config.GetValue("LogFile", String.Empty).ToLower() != String.Empty) &&
				(Directory.Exists(Path.GetDirectoryName(config.GetValue("LogFile", String.Empty))) == true)) {
				this.logFlags |= LoggerFlags.File;
				this.logFileName = config.GetValue("LogFile");
			}

			if (config.GetValue("LogFileRollSizeMB", 0) > 0) {
				this.logFileRollSize = 20 * 1024;//config.GetValue("LogFileRollSizeMB", 0) * 1024 * 1024;	// MB to bytes.
			}

			if (config.GetValue("LogFileRollCount", 0) > 0) {
				this.logFileRollCount = config.GetValue("LogFileRollCount", 0);
			}

			// Log.
			this.Log(
				"***** begin new process *****" + Environment.NewLine +
				"Log options: From configuration file." + Environment.NewLine +
				"Log file: {1}  (Roll at {2} bytes, Keep {3} log files)." + Environment.NewLine +
				"Log flags: {0}",
				this.logFlags,
				(this.logFileName != null) ? this.logFileName  : "<not used>",
				(this.logFileRollSize > 0) ? this.logFileRollSize : 0,
				(this.logFileRollCount > 0) ? this.logFileRollCount : 0
			);
		} // Logger
		#endregion

		#region Properties.
		/// <summary>
		/// Gets the log flags.
		/// </summary>
		public LoggerFlags LogFlags {
			get {
				return this.logFlags;
			}
		} // LogFlags

		/// <summary>
		/// Gets the log filename.
		/// </summary>
		public String LogFileName {
			get {
				return this.logFileName;
			}
		} // LogFileName

		/// <summary>
		/// Gets the log file roll size in bytes.
		/// </summary>
		public Int32 LogFileRollSize {
			get {
				return this.logFileRollSize;
			}
		} // LogFileRollSize

		/// <summary>
		/// Gets the log file roll count.
		/// </summary>
		public Int32 LogFileRollCount {
			get {
				return this.logFileRollCount;
			}
		} // LogFileRollCount
		#endregion

		#region Events.
		/// <summary>
		/// Occurs when something is logged, and event logging is enabled.
		/// </summary>
		public event LoggerEventHandler OnLog {
			add {
				this.logLogEvents += value;
			}
			remove {
				this.logLogEvents -= value;
			}
		} // OnLog
		#endregion

		#region Private log methods.
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
				} catch { }
			}

			// Log to the file system.
			if ((this.logFlags & LoggerFlags.File) == LoggerFlags.File) {
				try {
					// Roll the log file.
					// Get the size of the current log file.
					FileInfo logFileInfo = new FileInfo(this.logFileName);
					if (this.logFileRollSize < logFileInfo.Length) {
						// Close the log file.
						if (this.logFile != null) {
							this.logFile.Flush();
							this.logFile.Close();
							this.logFile = null;
						}

						// Rename old log files.
						for (Int32 logFileIndex = this.logFileRollCount; logFileIndex > 0; logFileIndex--) {
							String logFileNameLow = Path.ChangeExtension(this.logFileName, "." + (logFileIndex - 1) + Path.GetExtension(this.logFileName));
							String logFileNameHigh = Path.ChangeExtension(this.logFileName, "." + (logFileIndex) + Path.GetExtension(this.logFileName));
							if (File.Exists(logFileNameHigh) == true) {
								File.Delete(logFileNameHigh);
							}
							if (File.Exists(logFileNameLow) == true) {
								File.Move(logFileNameLow, logFileNameHigh);
							}
						}

						// Delete all old (expired) log files.
						String logFileNameSearch = Path.GetFileName(Path.ChangeExtension(this.logFileName, ".*" + Path.GetExtension(this.logFileName)));
						String[] logFilesOld = Directory.GetFiles(Path.GetDirectoryName(this.logFileName), logFileNameSearch, SearchOption.TopDirectoryOnly);
						if (this.logFileRollCount < logFilesOld.Length) {
							foreach (String logFileNameOld in logFilesOld) {
								Int32 logFileIndexOld = -1;
								String logFileIndexOldStr = Path.GetFileNameWithoutExtension(logFileNameOld).Substring(Path.GetFileNameWithoutExtension(this.logFileName).Length + 1);
								Int32.TryParse(logFileIndexOldStr, out logFileIndexOld);
								if (this.LogFileRollCount < logFileIndexOld) {
									File.Delete(logFileNameOld);
								}
							}
						}

						// Rename current log file.
						if (File.Exists(this.logFileName) == true) {
							if (this.logFileRollCount > 0) {
								String logFileNameRoll = Path.ChangeExtension(this.logFileName, ".1" + Path.GetExtension(this.logFileName));
								File.Move(this.logFileName, logFileNameRoll);
							} else {
								// No roll count.
								File.Delete(this.logFileName);
							}
						}
					}
				} catch {}

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

			// Log to the registed event handlers.
			if (((this.logFlags & LoggerFlags.Event) == LoggerFlags.Event) &&
				(this.logLogEvents != null)) {
				Delegate[] subscribers = this.logLogEvents.GetInvocationList();
				foreach (Delegate subscriber in subscribers) {
					try {
						// Invoke the event delegate.
						((LoggerEventHandler)subscriber)(logFlags, text);
					} catch { }
				}
			}

		} // Log
		#endregion

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