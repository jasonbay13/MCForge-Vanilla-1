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
using System.Collections.Generic;
using System.Linq;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    /// <summary>
    /// The OnPlayerChat event is excuted everytime a player chats on the server
    /// This event can be canceled.
	/// This event can also modify what the user says. by changing the message.
    /// </summary>
    public class OnPlayerChat : PlayerEvent
    {

		/// <summary>
		/// Creates a new event.  This is NOT meant to be used by user-code, only internally by events.
		/// </summary>
		/// <param name="callback">the method used for the delegate to callback upon event fire</param>
		/// <param name="target">The target Player we want the event for.</param>
		internal OnPlayerChat(OnCall callback, Player target) : base (target) {
			_queue += callback;
		}

		/// <summary>
		/// The messsage sent by the player
		/// </summary>
		public string message { get; set; }
		
		public string OrginalMessage()
		{
			return message.Split(':')[1].Substring(1);
		}
		/// <summary>
		/// The delegate used for callbacks.  The caller will have this method run when the event fires.
		/// </summary>
		/// <param name="e">The Event that fired</param>
		public delegate void OnCall(OnPlayerChat e);

		/// <summary>
		/// The queue of delegates to call for the particular tag (One for each event)
		/// </summary>
		private OnCall _queue;

		/// <summary>
		/// The list of all events currently active of a PlayerEvent type.
		/// </summary>
		protected static List<OnPlayerChat> _eventQueue = new List<OnPlayerChat>(); // Same across all events of this kind

		/// <summary>
		/// This is meant to be called from the code where you mean for the event to happen.
		/// 
		/// In this case, it is called from the chat processing code.
		/// </summary>
		/// <param name="p">The player that caused the event.</param>
		/// <param name="msg">The message sent by the player.</param>
		/// <returns>a new (or existing) event with the modified string.</returns>
		internal static OnPlayerChat Call(Player p, string msg) {
			//Event was called from the code.
			List<OnPlayerChat> opcList = new List<OnPlayerChat>();
			//Do we keep or discard the event?
			_eventQueue.ForEach(opc => {
				if (opc.Player == null || opc.Player.username == p.username) {// We keep it
					//Set up variables, then fire all callbacks.
					opc.message = msg;
					Player oldPlayer = opc.Player;
					opc._target = p; // Set player that triggered event.
					opc._queue(opc); // fire callback
					opcList.Add(opc); // add to used list
					opc._target = oldPlayer;
				}
			});
			OnPlayerChat pc = new OnPlayerChat(null, p);
			//If the messages are equal, we return it.
			pc.message = (opcList.Count > 0 ? opcList[0].message : msg);
			if (opcList.Any(pe => pe.cancel)) {
				pc.Cancel();
			}

			//The message returned is the new message to use.  If two events return different messages, then the 'null' message is used first. (Prevents duplicates)
			return ((opcList.All((opc) => opc.message == pc.message)) ? pc : opcList.Find(opc => opc.Player == null)); // Retern an event with the message
			//return (opcList.Any(pe => pe.cancel) ? "" : (opcList.Count > 0 ? opcList.Last().message : msg )); //Return if last canceled the event. (empty string)
		}

		/// <summary>
		/// Used to register a method to be executed when the event is fired.
		/// </summary>
		/// <param name="callback">The method to call</param>
		/// <param name="target">The player to watch for. (null for any players)</param>
		/// <returns>the OnPlayerChat event</returns>
		public static OnPlayerChat Register(OnCall callback, Player target) {
			//We add it to the list here
			OnPlayerChat pe = _eventQueue.Find(match => (match.Player == null ? target == null : target != null && target.username == match.Player.username));
			if (pe != null)
				//It already exists, so we just add it to the queue.
				pe._queue += callback;
			else {
				//Doesn't exist yet.  Make a new one.
				pe = new OnPlayerChat(callback, target);
				_eventQueue.Add(pe);
			}
			return pe;
		}

		/// <summary>
		/// Unregisters the sxpecified event
		/// </summary>
		/// <param name="pe">The event to unregister</param>
		public static void Unregister(OnPlayerChat pe) {
			pe.Unregister();
		}
		/// <summary>
		/// Unregisters the sxpecified event
		/// </summary>
		/// <param name="pe">The event to unregister</param>
		public override void Unregister() {
			_eventQueue.Remove(this);
		}
	}
}
