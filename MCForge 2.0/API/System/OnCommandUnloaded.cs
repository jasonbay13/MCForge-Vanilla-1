/*
Copyright 2011 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
using System;
using MCForge.Utilities;
using System.Linq;
using System.Collections.Generic;
using MCForge.Interface.Command;

namespace MCForge.API.System
{
	/// <summary>
	/// This event is called when a command unloads
	/// </summary>
	public class OnCommandUnloaded : SystemEvent
	{
		/// <summary>
		/// The command that is being unloaded
		/// </summary>
		public Command command { get; set; }
		
		/// <summary>
		/// The delegate used for callbacks.  The caller will have this method run when the event fires.
		/// </summary>
		/// <param name="e">The Event that fired</param>
		public delegate void OnCall(OnCommandUnloaded args);
		internal OnCommandUnloaded(OnCall callback)
		{
			_queue += callback;
		}
		/// <summary>
		/// The queue of delegates to call for the particular tag (One for each event)
		/// </summary>
		private OnCall _queue;

		/// <summary>
		/// The list of all events currently active of a PlayerEvent type.
		/// </summary>
		protected static List<OnCommandUnloaded> _eventQueue = new List<OnCommandUnloaded>(); // Same across all events of this kind
		
		/// <summary>
		/// This is meant to be called from the code where you mean for the event to happen.
		/// 
		/// In this case, it is called from the command processing code.
		/// </summary>
		/// <param name="command">The command that is being unloaded</param>
		internal static bool Call(Command command) {
			Logger.Log("Calling OnCommandUnloaded event", LogType.Debug);
			//Event was called from the code.
			List<OnCommandUnloaded> opcList = new List<OnCommandUnloaded>();
			//Do we keep or discard the event?
			_eventQueue.ForEach(opc => {
					//Set up variables, then fire all callbacks.
					opc.command = command;
					opc._queue(opc); // fire callback
					opcList.Add(opc); // add to used list
			});
			return opcList.Any(pe => pe.cancel); //Return if any canceled the event.
		}
		/// <summary>
		/// Used to register a method to be executed when the event is fired.
		/// </summary>
		/// <param name="callback">The method to call</param>
		/// <returns>A reference to the event</returns>
		public static OnCommandUnloaded Register(OnCall callback) {
			Logger.Log("OnCommandUnloaded registered to the method " + callback.Method.Name, LogType.Debug);
			OnCommandUnloaded pe = new OnCommandUnloaded(callback);
			_eventQueue.Add(pe);
			return pe;
		}

		/// <summary>
		/// Unregisters the specific event
		/// </summary>
		/// <param name="pe">The event to unregister</param>
		public static void Unregister(OnCommandUnloaded pe) {
			pe.Unregister();
		}
		/// <summary>
		/// Unregisters the specific event
		/// </summary>
		/// <param name="pe">The event to unregister</param>
		public override void Unregister() {
			_eventQueue.Remove(this);
		}
	}
}
