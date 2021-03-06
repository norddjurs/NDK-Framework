using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace NDK.Framework {

	#region ServiceManager class.
	public class ServiceManager : ServiceBase {
		public static String SERVICE_NAME = "NDK Framework Service";
		private IFramework framework = null;
		private String[] serviceArguments = null;
		private ManualResetEvent threadShutdownEvent = null;
		private Thread thread = null;

		/// <summary>
		/// Create a new instance of the service object, and start the service.
		/// </summary>
		/// <param name="framework">The framework.</param>
		/// <param name="args">The arguments.</param>
		public static void Run(IFramework framework, String[] args) {
			ServiceBase.Run(new ServiceManager(framework, args));
		} // Run

		/// <summary>
		/// Create a new instance of the service object.
		/// </summary>
		/// <param name="framework">The framework.</param>
		/// <param name="args">The arguments.</param>
		public ServiceManager(IFramework framework, String[] args) {
			// Save the arguments.
			this.framework = framework;
			this.serviceArguments = args;
			this.threadShutdownEvent = new ManualResetEvent(false);

			// Set the service name.
			base.ServiceName = ServiceManager.SERVICE_NAME;
			base.EventLog.Log = "Application";

			// Set whether or not to handle specific service events.
			base.CanHandlePowerEvent = false;
			base.CanHandleSessionChangeEvent = false;
			base.CanPauseAndContinue = false;
			base.CanShutdown = true;
			base.CanStop = true;
		} // ServiceManager

		#region Plugin tag object.
		private class PluginTag {
			public DateTime LastRunTime = DateTime.MinValue;
			public Thread Thread = null;
			public List<String> ScheduleMatchYear = new List<String>();
			public List<String> ScheduleMatchMonth = new List<String>();
			public List<String> ScheduleMatchDate = new List<String>();
			public List<String> ScheduleMatchDay = new List<String>();
			public List<String> ScheduleMatchHour = new List<String>();
			public List<String> ScheduleMatchMinute = new List<String>();

			public List<Int32> ScheduleMatchYearInt32 {
				get {
					List<Int32> values = new List<Int32>();
					foreach (String valueStr in this.ScheduleMatchYear) {
						try {
							values.Add(Int32.Parse(valueStr));
						} catch { }
					}
					return values;
				}
			} // ScheduleMatchYearInt32

			public List<Int32> ScheduleMatchMonthInt32 {
				get {
					List<Int32> values = new List<Int32>();
					foreach (String valueStr in this.ScheduleMatchMonth) {
						try {
							values.Add(Int32.Parse(valueStr));
						} catch { }
					}
					return values;
				}
			} // ScheduleMatchMonthInt32

			public List<Int32> ScheduleMatchDateInt32 {
				get {
					List<Int32> values = new List<Int32>();
					foreach (String valueStr in this.ScheduleMatchDate) {
						try {
							values.Add(Int32.Parse(valueStr));
						} catch { }
					}
					return values;
				}
			} // ScheduleMatchDateInt32

			public List<Int32> ScheduleMatchDayInt32 {
				get {
					List<Int32> values = new List<Int32>();
					foreach (String valueStr in this.ScheduleMatchDay) {
						try {
							values.Add(Int32.Parse(valueStr));
						} catch { }
					}
					return values;
				}
			} // ScheduleMatchDayInt32

			public List<Int32> ScheduleMatchHourInt32 {
				get {
					List<Int32> values = new List<Int32>();
					foreach (String valueStr in this.ScheduleMatchHour) {
						try {
							values.Add(Int32.Parse(valueStr));
						} catch { }
					}
					return values;
				}
			} // ScheduleMatchHourInt32

			public List<Int32> ScheduleMatchMinuteInt32 {
				get {
					List<Int32> values = new List<Int32>();
					foreach (String valueStr in this.ScheduleMatchMinute) {
						try {
							values.Add(Int32.Parse(valueStr));
						} catch { }
					}
					return values;
				}
			} // ScheduleMatchMinuteInt32

		} // PluginTag
		#endregion

		#region Thread methods.
		private void StartThread() {
			try {
				if (this.thread == null) {
					// Start the thread.
					this.threadShutdownEvent = new ManualResetEvent(false);
					this.thread = new Thread(this.RunThread);
					this.thread.Name = "NDK Framework Service Main Thread";
					this.thread.IsBackground = false;
					this.thread.Start();
					this.framework.Log("Service: Thread is started.");
				}
			} catch (Exception exception) {
				this.framework.LogError("Service: Thread start error.");
				this.framework.LogError(exception);
			}
		} // StartThread

		private void PauseThread() {
			try {
			} catch (Exception exception) {
				this.framework.LogError("Service: Thread pause error.");
				this.framework.LogError(exception);
			}
		} // PauseThread

		private void StopThread() {
			try {
				if (this.thread != null) {
					// Stop the thread.
					this.threadShutdownEvent.Set();
					if (this.thread.Join(30000)) {      // give the thread 30 seconds to stop.
						this.thread.Abort();
						this.framework.Log("Service: Thread has stopped.");
					} else {
						this.framework.Log("Service: Thread did not stop as requested within the 30 second time limit.");
					}
				}
			} catch (Exception exception) {
				this.framework.LogError("Service: Thread stop error.");
				this.framework.LogError(exception);
			}
		} // StopThread

		public void RunThread() {
			try {
				Int32 pluginsReloadInterval = 60;
				Int32 pluginsReloadIntervalConfig = 60;
				DateTime pluginsLastLoadTime = DateTime.MinValue;
				List<String> pluginsEnabledGuidStr = new List<String>();
				List<IPlugin> plugins = null;

				// Loop until the service is stopped.
				while (this.threadShutdownEvent.WaitOne(0) == false) {
					// Read plugin reload interval from the configuration.
					// This is done every 30 minutes as default.
					if (Int32.TryParse(this.framework.GetSystemValue("ServicePluginReloadInterval", "30"), out pluginsReloadIntervalConfig) == true) {
						if ((pluginsReloadInterval != pluginsReloadIntervalConfig) && (pluginsReloadIntervalConfig > 0)) {
							pluginsReloadInterval = pluginsReloadIntervalConfig;
							this.framework.LogDebug("Service: Loading plugins every {0} minute(s).", pluginsReloadInterval);
						}
					}

					// Loading plugins.
					if (pluginsLastLoadTime.AddMinutes(pluginsReloadInterval).CompareTo(DateTime.Now) < 0) {
						this.framework.LogDebug("Service: Loading plugins.");
						plugins = new List<IPlugin>();
						plugins.AddRange(this.framework.GetPlugins(true));
						pluginsEnabledGuidStr = this.framework.GetSystemValues("ServicePluginEnabled");
						pluginsLastLoadTime = DateTime.Now;
						this.framework.LogDebug("Service: {0} plugin(s) found.", plugins.Count);

						// Initialize plugins that are enabled in the configuration, remove plugins, that are not enabled in the configuration.
						for (Int32 pluginIndex = 0; pluginIndex < plugins.Count;) {
							IPlugin plugin = plugins[pluginIndex];
							Guid pluginEnabledGuid = Guid.Empty;
							Boolean pluginEnabled = false;

							// Initialize the plugin, if it is enabled.
							foreach (String pluginEnabledGuidStr in pluginsEnabledGuidStr) {
								if ((Guid.TryParse(pluginEnabledGuidStr, out pluginEnabledGuid) == true) && (plugin.GetGuid().Equals(pluginEnabledGuid) == true)) {
									pluginEnabled = true;
									plugin.Tag = new PluginTag();
								}
							}

							// Iterate.
							if (pluginEnabled == true) {
								pluginIndex++;
								this.framework.LogDebug("Service:  {0}   {1}  (enabled)", plugin.GetGuid(), plugin.GetName());
							} else {
								plugins.RemoveAt(pluginIndex);
								this.framework.LogDebug("Service:  {0}   {1}", plugin.GetGuid(), plugin.GetName());
							}
						}

						// Read plugin schedule from the configurations.
						foreach (BasePlugin plugin in plugins) {
							// Get the plugin tag.
							PluginTag pluginTag = (PluginTag)plugin.Tag;

							// Read plugin schedule from the configurations.
							pluginTag.ScheduleMatchYear = this.framework.GetConfigValues(plugin.GetGuid(), "ServiceScheduleMatchYear");
							pluginTag.ScheduleMatchMonth = this.framework.GetConfigValues(plugin.GetGuid(), "ServiceScheduleMatchMonth");
							pluginTag.ScheduleMatchDate = this.framework.GetConfigValues(plugin.GetGuid(), "ServiceScheduleMatchDate");
							pluginTag.ScheduleMatchDay = this.framework.GetConfigValues(plugin.GetGuid(), "ServiceScheduleMatchDay");
							pluginTag.ScheduleMatchHour = this.framework.GetConfigValues(plugin.GetGuid(), "ServiceScheduleMatchHour");
							pluginTag.ScheduleMatchMinute = this.framework.GetConfigValues(plugin.GetGuid(), "ServiceScheduleMatchMinute");

							// Log.
							this.framework.LogDebug("Service: Schedule for plugin  {0}   {1}", plugin.GetGuid(), plugin.GetName());
							if (pluginTag.ScheduleMatchYear.Count > 0) {
								this.framework.LogDebug("Service:  Year(s): {0}", String.Join(", ", pluginTag.ScheduleMatchYear));
							} else {
								this.framework.LogDebug("Service:  Year(s): {0}", "Every year.");
							}
							if (pluginTag.ScheduleMatchMonth.Count > 0) {
								this.framework.LogDebug("Service:  Month(s): {0}", String.Join(", ", pluginTag.ScheduleMatchMonth));
							} else {
								this.framework.LogDebug("Service:  Month(s): {0}", "Every month.");
							}
							if (pluginTag.ScheduleMatchDate.Count > 0) {
								this.framework.LogDebug("Service:  Date(s): {0}", String.Join(", ", pluginTag.ScheduleMatchDate));
							} else {
								this.framework.LogDebug("Service:  Date(s): {0}", "Every date.");
							}
							if (pluginTag.ScheduleMatchDay.Count > 0) {
								this.framework.LogDebug("Service:  Day(s): {0}  (Monday = 1, today = {1})", String.Join(", ", pluginTag.ScheduleMatchDay), ((Int32)DateTime.Now.DayOfWeek));
							} else {
								this.framework.LogDebug("Service:  Day(s): {0}  (Monday = 1, today = {1})", "Every day of the week.", ((Int32)DateTime.Now.DayOfWeek));
							}
							if (pluginTag.ScheduleMatchHour.Count > 0) {
								this.framework.LogDebug("Service:  Hour(s): {0}", String.Join(", ", pluginTag.ScheduleMatchHour));
							} else {
								this.framework.LogDebug("Service:  Hour(s): {0}", "Every hour.");
							}
							if (pluginTag.ScheduleMatchMinute.Count > 0) {
								this.framework.LogDebug("Service:  Minute(s): {0}", String.Join(", ", pluginTag.ScheduleMatchMinute));
							} else {
								this.framework.LogDebug("Service:  Minute(s): {0}", "Every minute.");
							}
						}
					}

					// Process all enabled plugins.
					foreach (BasePlugin plugin in plugins) {
						// Get the plugin tag.
						PluginTag pluginTag = (PluginTag)plugin.Tag;

						// Process the plugin, if it is not running.
						if (pluginTag.Thread == null) {
							if ((pluginTag.LastRunTime.AddMinutes(1).CompareTo(DateTime.Now) < 0) &&
								((pluginTag.ScheduleMatchYear.Count == 0) || (pluginTag.ScheduleMatchYearInt32.Contains(DateTime.Now.Year) == true)) &&
								((pluginTag.ScheduleMatchMonth.Count == 0) || (pluginTag.ScheduleMatchMonthInt32.Contains(DateTime.Now.Month) == true)) &&
								((pluginTag.ScheduleMatchDate.Count == 0) || (pluginTag.ScheduleMatchDateInt32.Contains(DateTime.Now.Day) == true)) &&
								((pluginTag.ScheduleMatchDay.Count == 0) || (pluginTag.ScheduleMatchDayInt32.Contains(((Int32)(DateTime.Now.DayOfWeek))) == true)) &&
								(((pluginTag.ScheduleMatchHour.Count == 0) && (pluginTag.ScheduleMatchMinute.Count > 0)) || (pluginTag.ScheduleMatchHourInt32.Contains(DateTime.Now.Hour) == true)) &&
								(((pluginTag.ScheduleMatchMinute.Count == 0) && (pluginTag.ScheduleMatchHour.Count > 0)) || (pluginTag.ScheduleMatchMinuteInt32.Contains(DateTime.Now.Minute) == true))) {
								// Remember when the plugin was last executed.
								pluginTag.LastRunTime = DateTime.Now;

								// Execute the plugin on its own thread.
								(new Thread(new ThreadStart(delegate () {
									this.framework.Log("Service: The plugin execution is starting.");
									try {
										// Register the thread.
										pluginTag.Thread = Thread.CurrentThread;

										// Execute the plugin.
										plugin.Run();
									} catch (ThreadAbortException exception) {
										this.framework.Log("Service: The plugin execution was aborted.");
									} catch (Exception exception) {
										this.framework.LogError(exception);
									} finally {
										// Unregister the thread.
										pluginTag.Thread = null;
									}
									this.framework.Log("Service: The plugin execution has ended.");
								}))).Start();
							}
						}
					}

					// Sleep for a short while.
					Thread.Sleep(15000);
				}
			} catch (ThreadAbortException exception) {
				this.framework.Log("Service: Thread was aborted.");
			} catch (Exception exception) {
				this.framework.LogError("Service: Thread run error.");
				this.framework.LogError(exception);
			}
		} // RunThread
		#endregion

		#region Override ServiceBase methods.
		/// <summary>
		/// Dispose.
		/// </summary>
		/// <param name="disposing">Whether or not disposing is going on.</param>
//		protected override void Dispose(Boolean disposing) {
//			this.StopThread();
//			//base.Dispose(disposing);
//		} // Dispose

		/// <summary>
		/// Starts the service.
		/// </summary>
		/// <param name="args">The arguments.</param>
		protected override void OnStart(String[] args) {
			this.serviceArguments = args;
			this.StartThread();
			//base.OnStart(args);
		} // OnStart

		/// <summary>
		/// Stop the service.
		/// </summary>
		protected override void OnStop() {
			this.StopThread();
			//base.OnStop();
		} // OnStop

		/// <summary>
		/// Pause the service.
		/// </summary>
//		protected override void OnPause() {
//			//this.PauseThread();
//			base.OnPause();
//		} // OnPause

		/// <summary>
		/// Continue the service.
		/// </summary>
//		protected override void OnContinue() {
//			this.StartThread();
//			//base.OnContinue();
//		} // OnContinue

		/// <summary>
		/// Called when the System is shutting down.
		/// </summary>
//		protected override void OnShutdown() {
//			this.StopThread();
//			//base.OnShutdown();
//		} // OnShutdown

		/// <summary>
		/// Called when the service receives a custom command.
		/// If you need to send a command to your service without the need for Remoting or Sockets, use
		/// this method to do custom methods.
		/// 
		/// A custom command can be sent to a service by using this method:
		///		Int32 command = 128; //Some Arbitrary number between 128 and 256.
		///		ServiceController sc = new ServiceController("NameOfService");
		///		sc.ExecuteCommand(command);
		/// </summary>
		/// <param name="command">Arbitrary Integer between 128 and 256.</param>
//		protected override void OnCustomCommand(Int32 command) {
//			// Not used.
//			base.OnCustomCommand(command);
//		} // OnCustomCommand

		/// <summary>
		/// Called when the machine power status changes, such as going into Suspend mode or Low Battery for laptops.
		/// </summary>
		/// <param name="powerStatus">The Power Broadcast Status (BatteryLow, Suspend, etc.).</param>
//		protected override Boolean OnPowerEvent(PowerBroadcastStatus powerStatus) {
//			// Not used.
//			return base.OnPowerEvent(powerStatus);
//		} // OnPowerEvent

		/// <summary>
		/// Called when a user session status changes.
		/// Use to handle a change event from a Terminal Server session.
		/// Useful if you need to determine when a user logs in remotely or logs off, or when someone logs into the console.
		/// </summary>
		/// <param name="changeDescription">The Session Change Event that occured.</param>
//		protected override void OnSessionChange(SessionChangeDescription changeDescription) {
//			// Not used.
//			base.OnSessionChange(changeDescription);
//		} // OnSessionChange
		#endregion

		#region Static service control methods.
		private static AssemblyInstaller GetInstaller() {
			AssemblyInstaller installer = new AssemblyInstaller(Assembly.GetEntryAssembly(), null);
			installer.UseNewContext = true;
			return installer;
		} // GetInstaller

		/// <summary>
		/// Gets a value indicating if the service is installed.
		/// </summary>
		/// <returns>True if the service is installed.</returns>
		public static Boolean IsInstalled() {
			try {
				using (ServiceController controller = new ServiceController(ServiceManager.SERVICE_NAME)) {
					ServiceControllerStatus status = controller.Status;
				}
				return true;
			} catch {
				return false;
			}
		} // InInstalled

		/// <summary>
		/// Gets a value indicating if the service is running.
		/// </summary>
		/// <returns>True if the server is running.</returns>
		public static Boolean IsRunning() {
			try {
				using (ServiceController controller = new ServiceController(ServiceManager.SERVICE_NAME)) {
					if (ServiceManager.IsInstalled() == true) {
						return (controller.Status == ServiceControllerStatus.Running);
					}
				}
				return false;
			} catch {
				return false;
			}
		} // IsRunning

		/// <summary>
		/// Install the service.
		/// </summary>
		/// <param name="args">The arguments set after the executeable name.</param>
		public static void Install(params String[] args) {
			if (ServiceManager.IsInstalled() == false) {
				//----- Alternative installation method.
				// ManagedInstallerClass.InstallHelper(new String[] { Assembly.GetEntryAssembly().Location });
				//-----

				using (AssemblyInstaller installer = GetInstaller()) {
					// Add the arguments.
					if (args.Length > 0) {

						Console.WriteLine("====>   " + installer.CommandLine);
						// TODO: Set arguments.							
					}

					// Try to install the service.
					IDictionary state = new Hashtable();
					try {
						installer.Installers.Add(new ServiceInstaller("service"));
						installer.Install(state);
						installer.Commit(state);
					} catch {
						// Rollback the failed installation.
						// Rollback exceptions are caught and thrown away.
						try {
							installer.Rollback(state);
						} catch {}

						// Throw the original installation exception.
						throw;
					}
				}
			}
		} // Install

		/// <summary>
		/// Uninstall the service.
		/// </summary>
		public static void Uninstall() {
			if (ServiceManager.IsInstalled() == true) {
				//----- Alternative uninstallation method.
				// ManagedInstallerClass.InstallHelper(new String[] { "/u", Assembly.GetEntryAssembly().Location });
				//-----

				// Try to uninstall the service.
				using (AssemblyInstaller installer = GetInstaller()) {
					IDictionary state = new Hashtable();
					installer.Installers.Add(new ServiceInstaller());
					installer.Uninstall(state);
				}
			}
		} // Uninstall

		/// <summary>
		/// Start the service.
		/// </summary>
		public static void Start() {
			if (ServiceManager.IsInstalled() == true) {
				using (ServiceController controller = new ServiceController(ServiceManager.SERVICE_NAME)) {
					if (controller.Status != ServiceControllerStatus.Running) {
						controller.Start();
						controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
					}
				}
			}
		} // Start

		/// <summary>
		/// Stop the service.
		/// </summary>
		public static new void Stop() {
			if (ServiceManager.IsInstalled() == true) {
				using (ServiceController controller =
					new ServiceController(ServiceManager.SERVICE_NAME)) {
					if (controller.Status != ServiceControllerStatus.Stopped) {
						controller.Stop();
						controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
					}
				}
			}
		} // Stop
		#endregion

	} // ServiceManager
	#endregion

	#region ServiceInstaller class.
	[RunInstallerAttribute(true)]
	public class ServiceInstaller : Installer {
		private System.ServiceProcess.ServiceInstaller serviceInstaller1;
		private ServiceProcessInstaller processInstaller;
		private String[] arguments = new String[0];

		public ServiceInstaller(params String[] args) {
			// Save the arguments.
			this.arguments = args;

			// Instantiate installer for process and service.
			processInstaller = new ServiceProcessInstaller();
			this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();

			// The service runs under the system account.
			processInstaller.Account = ServiceAccount.LocalSystem;

			// The service is started manually.
			serviceInstaller1.StartType = ServiceStartMode.Manual;

			// ServiceName must equal those on ServiceBase derived classes.
			serviceInstaller1.ServiceName = ServiceManager.SERVICE_NAME;
			serviceInstaller1.DisplayName = ServiceManager.SERVICE_NAME;
			serviceInstaller1.Description = "NDK Framework Service instance 0";

			// Add installer to collection. Order is not important if more than one service.
			Installers.Add(serviceInstaller1);
			Installers.Add(processInstaller);
		} // ServiceInstaller

		protected override void OnBeforeInstall(IDictionary savedState) {
			// Modify the command line that executes the service.
			String assemblyPath = Context.Parameters["assemblypath"];
			if ((assemblyPath.Length > 0) && (assemblyPath.StartsWith("\"") == false)) {
				assemblyPath = "\"" + assemblyPath + "\"";
			}

			// Add the arguments.
			foreach (String argument in this.arguments) {
				if ((argument != null) && (argument.Trim().Length > 0)) {
					assemblyPath += "  " + argument.Trim();
				}
			}

			Context.Parameters["assemblypath"] = assemblyPath;
			base.OnBeforeInstall(savedState);
		} // OnBeforeInstall

		protected override void OnBeforeUninstall(IDictionary savedState) {
			base.OnBeforeUninstall(savedState);
		} // OnBeforeUninstall

	} // ServiceProjectInstaller
	#endregion

} // NDK.Framework