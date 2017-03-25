using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace NDK.Framework {

	#region Framework
	/// <summary>
	/// This partial part of the class, implements logging.
	/// </summary>
	public abstract partial class Framework : IFramework {
		private static event LoggerEventHandler logEvents = null;
		private LoggerFlags logFlags = LoggerFlags.Empty;
		private EventWaitHandle logFileWaitHandle = null;
		private String logFileName = null;
		private Int32 logFileRollSize = 0;
		private Int32 logFileRollCount = 0;
		private StreamWriter logFile = null;

		#region Private logging initialization
		/// <summary>
		/// Initializes a new logger, using the configuration provided.
		/// The following global properties can be configured to enable logging:
		/// 
		///		LogNormal			true | 1
		///		LogDebug			true | 1
		///		LogInternal			true | 1
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
		private void LogInitialize() {
			// Default.
			this.logFlags = LoggerFlags.Empty;
			this.logFileWaitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, "NDK_FRAMEWORK_LOG_FILE_LOCK");
			this.logFileName = null;
			this.logFileRollSize = 0;
			this.logFileRollCount = 0;
			this.logFile = null;

			if (this.GetSystemValue("LogNormal", false) == true) {
				this.logFlags |= LoggerFlags.Normal;
			}

			if (this.GetSystemValue("LogDebug", false) == true) {
				this.logFlags |= LoggerFlags.Debug;
			}

			if (this.GetSystemValue("LogInternal", false) == true) {
				this.logFlags |= LoggerFlags.Internal;
			}

			if (this.GetSystemValue("LogError", false) == true) {
				this.logFlags |= LoggerFlags.Error;
			}

			if (this.GetSystemValue("LogConsole", false) == true) {
				this.logFlags |= LoggerFlags.Console;
			}

			if (this.GetSystemValue("LogWindows", false) == true) {
				this.logFlags |= LoggerFlags.Windows;
			}

			if (this.GetSystemValue("LogEvent", false) == true) {
				this.logFlags |= LoggerFlags.Event;
			}

			if (this.GetSystemValue("LogFile", false) == true) {
				this.logFlags |= LoggerFlags.File;
				this.logFileName = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "log");
			}

			if ((this.GetSystemValue("LogFile", String.Empty).ToLower() != String.Empty) &&
				(Directory.Exists(Path.GetDirectoryName(this.GetSystemValue("LogFile", String.Empty))) == true)) {
				this.logFlags |= LoggerFlags.File;
				this.logFileName = this.GetSystemValue("LogFile");
			}

			if (this.GetSystemValue("LogFileRollSizeMB", 0) > 0) {
				this.logFileRollSize = this.GetSystemValue("LogFileRollSizeMB", 0) * 1024 * 1024; // MB to bytes.
			}

			if (this.GetSystemValue("LogFileRollCount", 0) > 0) {
				this.logFileRollCount = this.GetSystemValue("LogFileRollCount", 0);
			}

			// Log initialization, but only once.
			if (Framework.frameworkFirstInitialization == true) {
				this.Log(
					"***** begin new process *****" + Environment.NewLine +
					"Log flags: {1}. Process id: {0}" + Environment.NewLine +
					"Log options: From configuration file." + Environment.NewLine +
					"Log file: {2}  (Roll at {3} bytes, Keep {4} log files).",
					Process.GetCurrentProcess().Id,
					this.logFlags,
					(this.logFileName != null) ? this.logFileName : "<not used>",
					(this.logFileRollSize > 0) ? this.logFileRollSize : 0,
					(this.logFileRollCount > 0) ? this.logFileRollCount : 0
				);
			}
		} // LogInitialize
		#endregion

		#region Public logging events.
		/// <summary>
		/// Occurs when something is logged, and event logging is enabled.
		/// </summary>
		public event LoggerEventHandler OnLog {
			add {
				Framework.logEvents += value;
			}
			remove {
				Framework.logEvents -= value;
			}
		} // OnLog
		#endregion

		#region Public logging properties.
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

		#region Public logging methods.
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

		#region Internal logging methods.
		/// <summary>
		/// Writes the text to the internal debug log.
		/// The information is only logged, if INTERNAL logging is configures.
		/// </summary>
		/// <param name="text">The debug text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		internal void LogInternal(String text, params Object[] formatArgs) {
			this.Log(LoggerFlags.Internal, text, formatArgs);
		} // LogInternal
		#endregion

		#region Private logging methods.
		/// <summary>
		/// Writes the text to the log.
		/// </summary>
		/// <param name="logFlags">The desired log flags.</param>
		/// <param name="text">The text.</param>
		/// <param name="formatArgs">The optional string format arguments.</param>
		private void Log(LoggerFlags logFlags, String text, params Object[] formatArgs) {
			try {
				// Format the text.
				if (formatArgs.Length > 0) {
					try {
						text = String.Format(text, formatArgs);
					} catch { }
				}

				// Get flags.
				Boolean logToConsole = ((this.logFlags & LoggerFlags.Console) == LoggerFlags.Console);
				Boolean logToWindows = ((this.logFlags & LoggerFlags.Windows) == LoggerFlags.Windows);
				Boolean logToFile = ((this.logFlags & LoggerFlags.File) == LoggerFlags.File);
				Boolean logToEvent = ((this.logFlags & LoggerFlags.Event) == LoggerFlags.Event);

				//Boolean logNormal = (((this.logFlags & LoggerFlags.Normal) == LoggerFlags.Normal) && ((logFlags & LoggerFlags.Normal) == LoggerFlags.Normal));
				//Boolean logDebug = (((this.logFlags & LoggerFlags.Debug) == LoggerFlags.Debug) && ((logFlags & LoggerFlags.Debug) == LoggerFlags.Debug));
				//Boolean logInternal = (((this.logFlags & LoggerFlags.Internal) == LoggerFlags.Internal) && ((logFlags & LoggerFlags.Internal) == LoggerFlags.Internal));
				//Boolean logError = (((this.logFlags & LoggerFlags.Error) == LoggerFlags.Error) && ((logFlags & LoggerFlags.Error) == LoggerFlags.Error));
				Boolean logNormal = ((this.logFlags & LoggerFlags.Normal) == LoggerFlags.Normal);
				Boolean logDebug = ((this.logFlags & LoggerFlags.Debug) == LoggerFlags.Debug);
				Boolean logInternal = ((this.logFlags & LoggerFlags.Internal) == LoggerFlags.Internal);
				Boolean logError = ((this.logFlags & LoggerFlags.Error) == LoggerFlags.Error);

				// Split the text at each newline.
				String[] textLines = text.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);

				// Build text.
				// The logger flags, passed to this method, comes from the public log methods and is used to
				// specify the type of logging. It only contains one of the flags NOrmal, Debug, Internal or Error.
				String textHeader = String.Format("{0:ddd dd MMM HH:mm:ss} | {1:000000} | ", DateTime.Now, Process.GetCurrentProcess().Id);
				String textType = null;
				ConsoleColor textColor = ConsoleColor.Black;
				EventLogEntryType textEntryType = EventLogEntryType.Information;
				switch (logFlags) {
					case LoggerFlags.Normal:
						textType = "Norml";
						textColor = ConsoleColor.Black;
						textEntryType = EventLogEntryType.Information;
						break;
					case LoggerFlags.Debug:
						textType = "Debug";
						textColor = ConsoleColor.DarkBlue;
						textEntryType = EventLogEntryType.Warning;
						break;
					case LoggerFlags.Internal:
						textType = "Debug";
						textColor = ConsoleColor.DarkMagenta;
						textEntryType = EventLogEntryType.Warning;
						break;
					case LoggerFlags.Error:
						textType = "Error";
						textColor = ConsoleColor.DarkRed;
						textEntryType = EventLogEntryType.Error;
						break;
				}

				// Log to the console.
				if ((logToConsole == true) && (textType != null)) {
					Console.ResetColor();
					Console.ForegroundColor = textColor;
					foreach (String textLine in textLines) {
						Console.WriteLine(textHeader + textType + ": " + textLine);
					}
				}

				// Log to the windows event log.
				if ((logToWindows == true) && (textType != null)) {
					EventLog.WriteEntry("NDK Framework", text, textEntryType);
				}

				// Log to the file system.
				if ((logToFile == true) && (textType != null)) {
					this.AppendToLogFile(textHeader, textType, textLines);
				}

				// Log to the registed event handlers.
				if ((logToEvent == true) && (textType != null)) {
					foreach (String textLine in textLines) {
						this.FireLogEvent(logFlags, text, textHeader + textType + ": " + textLine);
					}
				}
			} catch (Exception exception) {
				System.Windows.Forms.MessageBox.Show(exception.Message);
			}
		} // Log

		private void FireLogEvent(LoggerFlags logFlags, String text, String formattedText) {
			if (Framework.logEvents != null) {
				Delegate[] subscribers = Framework.logEvents.GetInvocationList();
				foreach (Delegate subscriber in subscribers) {
					try {
						((LoggerEventHandler)subscriber)(logFlags, text, formattedText);
					} catch { }
				}
			}
		} // FireLogEvent

		private void AppendToLogFile(String textHeader, String textType, params String[] textLines) {
			try {
				// Wait for thread lock.
				this.logFileWaitHandle.WaitOne();

				// Roll the log file.
				// Get the size of the current log file.
				if (File.Exists(this.logFileName) == true) {
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
				}

				// Open the log file.
				if (this.logFile == null) {
					this.logFile = new StreamWriter(this.logFileName, true);
					this.logFile.AutoFlush = true;
				}

				foreach (String textLine in textLines) {
					this.logFile.WriteLine(textHeader + textType + ": " + textLine);
				}
			} catch {
			} finally {
				try {
					// Close the log file.
					if (this.logFile != null) {
						this.logFile.Flush();
						this.logFile.Close();
						this.logFile = null;
					}
				} catch { }

				// Release the thread lock.
				this.logFileWaitHandle.Set();
			}
		} // AppendToLogFile
		#endregion

	} // Framework
	#endregion

} // NDK.Framework