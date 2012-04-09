using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;

namespace MCForge.API.PlayerEvent
{
	/// <summary>
	/// The OnPlayerCommand event is used to catch whenever a player uses a command.
	/// The command need not be valid.
	/// </summary>
	public class OnPlayerCommand : PlayerEvent {

		/// <summary>
		/// Creates a new event.  This is NOT meant to be used by user-code, only internally by events.
		/// </summary>
		/// <param name="callback">the method used for the delegate to callback upon event fire</param>
		/// <param name="target">The target Player we want the event for.</param>
		/// <param name="tag">The tag of this event, so it can be cancelled, unregistered, etc.</param>
		internal OnPlayerCommand(OnCall callback, Player target, string tag) {
			_type = EventType.Player;
			this.tag = tag;
			_target = target;
			_queue += callback;
		}

		/// <summary>
		/// The command that the player tried to use
		/// </summary>
		public string cmd { get; set; }
		/// <summary>
		/// The arguments given for the command.
		/// </summary>
		public string[] args { get; set; }

		/// <summary>
		/// This is meant to be called from the code where you mean for the event to happen.
		/// 
		/// In this case, it is called from the command processing code.
		/// </summary>
		/// <param name="p">The player that caused the event.</param>
		/// <param name="cmd">The command the player gave</param>
		/// <param name="args">The arguments the player gave for the command.</param>
		/// <returns> A boolean value specifying whether or not to cancel the event.</returns>
		internal static bool Call(Player p, string cmd, string[] args) {
			//Event was called from the code.
			List<PlayerEvent> opcList = new List<PlayerEvent>();
			//Do we keep or discard the event?
			_eventQueue.ForEach(playerEvent => {
				if (playerEvent.GetType().Name != "OnPlayerCommand")
					return;
				OnPlayerCommand opc = (OnPlayerCommand)playerEvent;
				if (opc.target == null || opc.target.username == p.username) {// We keep it
					//Set up variables, then fire all callbacks.
					opc.cmd = cmd;
					opc.args = args;
					opc._queue(opc); // fire callback
					opcList.Add(opc); // add to used list
				}
			});
			return opcList.Any(pe => pe.cancel); //Return if any canceled the event.
		}

		/// <summary>
		/// Used to register a method to be executed when the event is fired.
		/// </summary>
		/// <param name="callback">The method to call</param>
		/// <param name="target">The player to watch for. (null for any players)</param>
		/// <param name="tag">The tag to use (Required if you ever want to unregister the event.</param>
		/// <returns></returns>
		public static PlayerEvent Register(PlayerEvent.OnCall callback, Player target, String tag) {
			//We add it to the list here
			tag += "OPCom";
			PlayerEvent pe = _eventQueue.Find(match => match.tag == tag);
			if (pe != null)
				//It already exists, so we just add it to the queue.
				((OnPlayerCommand)pe)._queue += callback;
			else {
				//Doesn't exist yet.  Make a new one.
				pe = new OnPlayerCommand(callback, target, tag);
				_eventQueue.Add(pe);
			}
			return pe;
		}

		/// <summary>
		/// Unregisters the event with the specified tag.
		/// </summary>
		/// <param name="tag">The tag to unregister</param>
		public static void Unregister(string tag) {
			tag += "OPCom";
			PlayerEvent pe = _eventQueue.Find(match => match.tag == tag);
			if (pe != null)
				_eventQueue.Remove(pe);
		}

		/// <summary>
		/// Unregisters this event.
		/// </summary>
		public override void Unregister() {
			_eventQueue.Remove(this);
		}
	}
}
