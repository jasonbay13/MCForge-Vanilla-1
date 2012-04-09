using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.World;

namespace MCForge.API.PlayerEvent
{
    public enum ActionType : byte
    {
        Delete,
        Place
    }
    public class OnPlayerBlockChange : PlayerEvent
    {
		/// <summary>
		/// Creates a new event.  This is NOT meant to be used by user-code, only internally by events.
		/// </summary>
		/// <param name="callback">the method used for the delegate to callback upon event fire</param>
		/// <param name="target">The target Player we want the event for.</param>
		/// <param name="tag">The tag of this event, so it can be cancelled, unregistered, etc.</param>
		internal OnPlayerBlockChange(OnCall callback, Player target, object datapass, string tag) {
			_type = EventType.Player;
			this.datapass = datapass;
			this.tag = tag;
			_target = target;
			_queue += callback;
		}

		/// <summary>
		/// What we arre doing with this block
		/// </summary>
		public ActionType action { get; set; }
		/// <summary>
		/// The block at the coordinates
		/// </summary>
		public byte holding { get; set; }
		/// <summary>
		/// The x coordinate of the block changed.
		/// </summary>
		public ushort x { get; set; }
		/// <summary>
		/// The y coordinate of the block changed.
		/// </summary>
		public ushort y { get; set; }
		/// <summary>
		/// The z coordinate of the block changed.
		/// </summary>
		public ushort z { get; set; }
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
		internal static bool Call(ushort x, ushort y, ushort z, ActionType action, Player p, byte holding) {
			//Event was called from the code.
			List<PlayerEvent> opbcList = new List<PlayerEvent>();
			//Do we keep or discard the event?
			_eventQueue.ForEach(playerEvent => {
				OnPlayerBlockChange opbc = (OnPlayerBlockChange)playerEvent;
				if (opbc.target == p) {// We keep it
					//Set up variables, then fire all callbacks.
					opbc.action = action;
					opbc.holding = holding;
					opbc.x = x;
					opbc.y = y;
					opbc.z = z;
					opbc._queue(opbc); // fire callback
					opbcList.Add(opbc); // add to used list
				}
			});
			return opbcList.Any(pe => pe.cancel); //Return if any canceled the event.
		}

		/// <summary>
		/// Used to register a method to be executed when the event is fired.
		/// </summary>
		/// <param name="callback">The method to call</param>
		/// <param name="target">The player to watch for. (null for any players)</param>
		/// <param name="tag">The tag to use (Required if you ever want to unregister the event.</param>
		/// <returns></returns>
		public static PlayerEvent Register(PlayerEvent.OnCall callback, Player target, object datapass, String tag) {
			//We add it to the list here
			tag += "OPBC";
			PlayerEvent pe = _eventQueue.Find(match => match.tag == tag);
			if (pe != null)
				//It already exists, so we just add it to the queue.
				((OnPlayerBlockChange)pe)._queue += callback;
			else {
				//Doesn't exist yet.  Make a new one.
				pe = new OnPlayerBlockChange(callback, target, datapass, tag);
				_eventQueue.Add(pe);
			}
			return pe;
		}

		/// <summary>
		/// Unregisters the event with the specified tag.
		/// </summary>
		/// <param name="tag">The tag to unregister</param>
		public static void Unregister(string tag) {
			tag += "OPBC";
			PlayerEvent pe = _eventQueue.Find(match => match.tag == tag);
			if (pe != null)
				_eventQueue.Remove(pe);
		}

		public override void Unregister() {
			_eventQueue.Remove(this);
		}
	}
}
