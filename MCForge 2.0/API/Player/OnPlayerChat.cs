using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    /// <summary>
    /// The OnPlayerChat event is excuted everytime a player chats on the server
    /// This event can be canceled
    /// </summary>
    public class OnPlayerChat : PlayerEvent
    {

		/// <summary>
		/// Creates a new event.  This is NOT meant to be used by user-code, only internally by events.
		/// </summary>
		/// <param name="callback">the method used for the delegate to callback upon event fire</param>
		/// <param name="target">The target Player we want the event for.</param>
		/// <param name="tag">The tag of this event, so it can be cancelled, unregistered, etc.</param>
		internal OnPlayerChat(OnCall callback, Player target, string tag) {
			_type = EventType.Player;
			this.tag = tag;
			_target = target;
			_queue += callback;
		}

		/// <summary>
		/// The messsage sent by the player
		/// </summary>
		public string message { get; set; }

		/// <summary>
		/// This is meant to be called from the code where you mean for the event to happen.
		/// 
		/// In this case, it is called from the command processing code.
		/// </summary>
		/// <param name="p">The player that caused the event.</param>
		/// <param name="msg">The message sent by the player.</param>
		/// <returns> A boolean value specifying whether or not to cancel the event.</returns>
		internal static string Call(Player p, string msg) {
			//Event was called from the code.
			List<PlayerEvent> opcList = new List<PlayerEvent>();
			//Do we keep or discard the event?
			_eventQueue.ForEach(playerEvent => {
				if (playerEvent.GetType() != Type.GetType("OnPlayerChat"))
					return;
				OnPlayerChat opc = (OnPlayerChat)playerEvent;
				if (opc.target == p) {// We keep it
					//Set up variables, then fire all callbacks.
					opc.message = msg;
					opc._queue(opc); // fire callback
					opcList.Add(opc); // add to used list
				}
			});
			return (opcList.Any(pe => pe.cancel) ? "" : (opcList.Count > 0 ? ((OnPlayerChat)opcList.Last()).message : msg )); //Return if last canceled the event. (empty string)
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
			tag += "OPCha";
			PlayerEvent pe = _eventQueue.Find(match => match.tag == tag);
			if (pe != null)
				//It already exists, so we just add it to the queue.
				((OnPlayerChat)pe)._queue += callback;
			else {
				//Doesn't exist yet.  Make a new one.
				pe = new OnPlayerChat(callback, target, tag);
				_eventQueue.Add(pe);
			}
			return pe;
		}

		/// <summary>
		/// Unregisters the event with the specified tag.
		/// </summary>
		/// <param name="tag">The tag to unregister</param>
		public static void Unregister(string tag) {
			tag += "OPCha";
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
