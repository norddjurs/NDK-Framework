using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace NDK.Framework {

	#region Framework
	/// <summary>
	/// This partial part of the class, implements event messaging.
	/// </summary>
	public abstract partial class Framework : IFramework {
		private static List<IFramework> eventFrameworkList = new List<IFramework>();

		#region Private event initialization
		private void EventInitialize() {
			// Register this framework class instance.
			if (Framework.eventFrameworkList.Contains(this) == false) {
				Framework.eventFrameworkList.Add(this);
			}
		} // EventInitialize
		#endregion

		#region Public send event methods.
		/// <summary>
		/// Executes the RunEvent method on all the loaded plugins.
		/// Exceptions are caught and logged.
		/// 
		/// Only event id lover then 1000 is allowed to be send to all loaded plugins, because you don't know
		/// which plugins might be loaded and available.
		/// 
		/// The keyValuesPairs object must be an anonymous object containing key/value pairs, like this:
		///		new { Key1 = valueObject1, Key2 = valueObject2 }
		/// </summary>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="keyValuePairs">The anonymous object containing key/value pairs.</param>
		public void TrySendEvent(PluginEvents eventId, Object keyValuePairs = null) {
			try {
				// Send event.
				this.SendEvent(eventId, keyValuePairs);
			} catch (Exception exception) {
				// Log.
				this.LogError(exception);
			}
		} // TrySendEvent

		/// <summary>
		/// Executes the RunEvent method on the plugin identified by the guid.
		/// Exceptions are caught and logged.
		/// 
		/// The keyValuesPairs object must be an anonymous object containing key/value pairs, like this:
		///		new { Key1 = valueObject1, Key2 = valueObject2 }
		/// </summary>
		/// <param name="pluginGuid">The plugin guid</param>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="keyValuePairs">The anonymous object containing key/value pairs.</param>
		public void TrySendEvent(Guid pluginGuid, Int32 eventId, Object keyValuePairs = null) {
			try {
				// Send event.
				this.SendEvent(pluginGuid, eventId, keyValuePairs);
			} catch (Exception exception) {
				// Log.
				this.LogError(exception);
			}
		} // TrySendEvent

		/// <summary>
		/// Executes the RunEvent method on all the loaded plugins.
		/// Exceptions are thrown from the RunEvent method.
		/// 
		/// Only event id lover then 1000 is allowed to be send to all loaded plugins, because you don't know
		/// which plugins might be loaded and available.
		/// 
		/// The keyValuesPairs object must be an anonymous object containing key/value pairs, like this:
		///		new { Key1 = valueObject1, Key2 = valueObject2 }
		/// </summary>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="keyValuePairs">The anonymous object containing key/value pairs.</param>
		public void SendEvent(PluginEvents eventId, Object keyValuePairs = null) {
			// Initialize a dictionary from the anonymous object
			IDictionary<String, Object> eventObjects = new Dictionary<String, Object>();
			if (keyValuePairs != null) {
				PropertyInfo[] properties = keyValuePairs.GetType().GetProperties();
				foreach (PropertyInfo prop in properties) {
					eventObjects.Add(prop.Name, prop.GetValue(keyValuePairs, null));
				}
			}

			// Execute the RunEvent methods.
			foreach (IFramework framework in Framework.eventFrameworkList) {
				// Log.
				this.LogDebug("Event: Triggering event id {0} in framework class {1}   {2}.", eventId, framework.GetGuid(), framework.GetName());

				// Call the RunEvent method.
				framework.RunEvent(this.GetGuid(), (Int32)eventId, eventObjects);
			}
		} // SendEvent

		/// <summary>
		/// Executes the RunEvent method on the plugin identified by the guid.
		/// Exceptions are thrown, when the plugin isn't available or from the RunEvent method.
		/// 
		/// The keyValuesPairs object must be an anonymous object containing key/value pairs, like this:
		///		new { Key1 = valueObject1, Key2 = valueObject2 }
		/// </summary>
		/// <param name="pluginGuid">The plugin guid</param>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="keyValuePairs">The anonymous object containing key/value pairs.</param>
		public void SendEvent(Guid pluginGuid, Int32 eventId, Object keyValuePairs = null) {
			// Initialize a dictionary from the anonymous object
			IDictionary<String, Object> eventObjects = new Dictionary<String, Object>();
			if (keyValuePairs != null) {
				PropertyInfo[] properties = keyValuePairs.GetType().GetProperties();
				foreach (PropertyInfo prop in properties) {
					eventObjects.Add(prop.Name, prop.GetValue(keyValuePairs, null));
				}
			}

			// Find plugin and execute the RunEvent method.
			foreach (IFramework framework in Framework.eventFrameworkList) {
				if (framework.GetGuid().Equals(pluginGuid) == true) {
					// Log.
					this.LogDebug("Event: Triggering event id {0} in framework class {1}   {2}.", eventId, framework.GetGuid(), framework.GetName());

					// Call the RunEvent method.
					framework.RunEvent(this.GetGuid(), eventId, eventObjects);

					// Exit.
					return;
				}
			}

			// The plugin was not found.
			throw new Exception(String.Format("Unable to trigger event in {0} in framework class {1}. No framework class with this guid is loaded.", eventId, pluginGuid));
		} // SendEvent
		#endregion

		#region Public virtual run event methods.
		/// <summary>
		/// Handle events.
		/// This method is invoked by another framework class.
		/// 
		/// When implementing a framework class, only use your own event id greater then 1000. Event id less then 1000 is reserved
		/// for global events. They will be declared in the PluginEvents enum.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="eventObjects">The event objects.</param>
		public virtual void RunEvent(Guid sender, Int32 eventId, IDictionary<String, Object> eventObjects) {
			// Do nothing.
		} // Event
		#endregion

	} // Framework
	#endregion

} // NDK.Framework