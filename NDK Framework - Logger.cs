using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace NDK.Framework {

	#region LoggerFlags enum.
	/// <summary>
	/// Logger flags.
	/// </summary>
	[Flags]
	public enum LoggerFlags {
		/// <summary>
		/// Log normal information.
		/// </summary>
		Empty = 0000,

		/// <summary>
		/// Log normal information.
		/// Written as information in the Windows event log.
		/// </summary>
		Normal = 0002,

		/// <summary>
		/// Log debug information.
		/// Written as warnings in the Windows event log.
		/// </summary>
		Debug = 0004,

		/// <summary>
		/// Log internal framework debug information.
		/// Written as warnings in the Windows event log.
		/// </summary>
		Internal = 0008,

		/// <summary>
		/// Log error information.
		/// Written as errors in the Windows event log.
		/// </summary>
		Error = 0016,

		/// <summary>
		/// Log all information.
		/// </summary>
		Everything = LoggerFlags.Normal | LoggerFlags.Debug | LoggerFlags.Internal | LoggerFlags.Error,

		/// <summary>
		/// Log to the console.
		/// The log is written to STDOUT.
		/// </summary>
		Console = 0128,

		/// <summary>
		/// Log to the file system.
		/// The log file name is the same as the entry assembly (.exe), but with the ".log" file extension.
		/// </summary>
		File = 0256,

		/// <summary>
		/// Log to the windows event log.
		/// The log is written to the default "Application" event log.
		/// </summary>
		Windows = 0512,

		/// <summary>
		/// Log to C# events.
		/// The log is send to all registed event handlers.
		/// </summary>
		Event = 1024,

		/// <summary>
		/// Log to all destinations.
		/// </summary>
		Everywhere = LoggerFlags.Console | LoggerFlags.File | LoggerFlags.Windows | LoggerFlags.Event,
	} // LoggerFlags
	#endregion

	#region Logger delegates.
	public delegate void LoggerEventHandler(LoggerFlags logFlags, String text, String formattedText);
	#endregion

} // NDK.Framework