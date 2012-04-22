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
		/// <param name="datapass">The data we want to pass between events.</param>
		internal OnPlayerBlockChange(OnCall callback, Player target, object datapass) : base(target)		{
			this.datapass = datapass;
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
		/// The delegate used for callbacks.  The caller will have this method run when the event fires.
		/// </summary>
		/// <param name="e">The Event that fired</param>
		public delegate void OnCall(OnPlayerBlockChange e);

		/// <summary>
		/// The queue of delegates to call for the particular tag (One for each event)
		/// </summary>
		private OnCall _queue;

		/// <summary>
		/// The list of all events currently active of a PlayerEvent type.
		/// </summary>
		protected static List<OnPlayerBlockChange> _eventQueue = new List<OnPlayerBlockChange>(); // Same across all events of this kind

		/// <summary>
		/// This is meant to be called from the code where you mean for the event to happen.
		/// 
		/// In this case, it is called from the command processing code.
		/// </summary>
		/// <param name="p">The player that caused the event.</param>
		/// <param name="action">The action of the player.  Destroyed, or placed?</param>
		/// <param name="holding">What the player was holding at the time.</param>
		/// <returns> A boolean value specifying whether or not to cancel the event.</returns>
		internal static bool Call(ushort x, ushort y, ushort z, ActionType action, Player p, byte holding) {
			//Event was called from the code.
			List<OnPlayerBlockChange> opbcList = new List<OnPlayerBlockChange>();
			//Do we keep or discard the event?
			_eventQueue.ForEach(opbc => {
			                    	if (opbc.Player == null || opbc.Player.username == p.username) {// We keep it
			                    		//Set up variables, then fire all callbacks.
			                    		opbc.action = action;
			                    		opbc.holding = holding;
			                    		opbc.x = x;
			                    		opbc.y = y;
			                    		opbc.z = z;
			                    		Player oldPlayer = opbc.Player;
			                    		opbc._target = p; // Set player that triggered event.
			                    		opbc._queue(opbc); // fire callback
			                    		opbcList.Add(opbc); // add to used list
			                    		opbc._target = oldPlayer;
			                    	}
			                    });
			return opbcList.Any(pe => pe.cancel); //Return if any canceled the event.
		}

		/// <summary>
		/// Used to register a method to be executed when the event is fired.
		/// </summary>
		/// <param name="callback">The method to call</param>
		/// <param name="target">The player to watch for. (null for any players)</param>
		/// <param name="datapass">The data to return when this event fires.</param>
		/// <returns></returns>
		public static OnPlayerBlockChange Register(OnCall callback, Player target, object datapass) {
			//We add it to the list here
			OnPlayerBlockChange pe = _eventQueue.Find(match => (match.Player == null ? target == null : target != null && target.username == match.Player.username));
			if (pe != null)
				//It already exists, so we just add it to the queue.
				pe._queue += callback;
			else {
				//Doesn't exist yet.  Make a new one.
				pe = new OnPlayerBlockChange(callback, target, datapass);
				_eventQueue.Add(pe);
			}
			return pe;
		}

		/// <summary>
		/// Unregisters the sxpecified event
		/// </summary>
		/// <param name="pe">The event to unregister</param>
		public static void Unregister(OnPlayerBlockChange pe) {
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
