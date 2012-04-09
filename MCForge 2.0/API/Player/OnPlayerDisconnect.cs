using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
	/// <summary>
	/// Called whenever anyone leaves the server.
	/// 
	/// <b>Note: This event CANNOT be canceled.  Cancel() will have no effect.</b>
	/// </summary>
    public class OnPlayerDisconnect : PlayerEvent
    {
		/// <summary>
		/// Creates a new event.  This is NOT meant to be used by user-code, only internally by events.
		/// </summary>
		/// <param name="callback">the method used for the delegate to callback upon event fire</param>
		/// <param name="target">The target Player we want the event for.</param>
		/// <param name="tag">The tag of this event, so it can be cancelled, unregistered, etc.</param>
		internal OnPlayerDisconnect(OnCall callback, Player target, string tag) {
			_type = EventType.Player;
			this.tag = tag;
			_target = target;
			_queue += callback;
		}

		public String reason { get; set; }

		/// <summary>
		/// This is meant to be called from the code where you mean for the event to happen.
		/// 
		/// In this case, it is called from the command processing code.
		/// </summary>
		/// <param name="p">The player that caused the event.</param>
		/// <param name="reason">The reason for disconnect.</param>
		internal static void Call(Player p, string reason) {
			//Event was called from the code.
			//Do we keep or discard the event?
			_eventQueue.ForEach(playerEvent => {
				if (playerEvent.GetType().Name != "OnPlayerDisconnect")
					return;
				OnPlayerDisconnect opc = (OnPlayerDisconnect)playerEvent;
				if (opc.target == null || opc.target.username == p.username) {// We keep it
					//Set up variables, then fire all callbacks.
					opc.reason = reason;
					opc._queue(opc); // fire callback
				}
			});
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
			tag += "OPDis";
			PlayerEvent pe = _eventQueue.Find(match => match.tag == tag);
			if (pe != null)
				//It already exists, so we just add it to the queue.
				((OnPlayerDisconnect)pe)._queue += callback;
			else {
				//Doesn't exist yet.  Make a new one.
				pe = new OnPlayerDisconnect(callback, target, tag);
				_eventQueue.Add(pe);
			}
			return pe;
		}

		/// <summary>
		/// Unregisters the event with the specified tag.
		/// </summary>
		/// <param name="tag">The tag to unregister</param>
		public static void Unregister(string tag) {
			tag += "OPDis";
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
