using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;


namespace MCForge.API.System {

	public class OnReceivePacket : SystemEvent {
		public byte[] Data { get; set; }
		
		public Player player { get; set; }
		
		public delegate void OnCall(OnReceivePacket pe);
		private OnCall _queue;
		
		internal OnReceivePacket(Player target, OnCall callback)
		{
			this.player = player;
			_queue += callback;
		}
		
		/// <summary>
		/// The list of all events currently active of a SystemEvent type.
		/// </summary>
		protected static List<OnReceivePacket> _eventQueue = new List<OnReceivePacket>(); // Same across all events of this kind
		
		/// <summary>
		/// Used to register a method to be executed when the event is fired.
		/// </summary>
		/// <param name="callback">The method to call</param>
		/// <param name="target">The player to watch for. (null for any players)</param>
		/// <returns>A reference to the event</returns>
		public static OnReceivePacket Register(OnCall callback, Player target) {
			//We add it to the list here
			OnReceivePacket pe = _eventQueue.Find(match => match.player == null || match.player.username == target.username);
			if (pe != null)
				//It already exists, so we just add it to the queue.
				pe._queue += callback;
			else {
				//Doesn't exist yet.  Make a new one.
				pe = new OnReceivePacket(target, callback);
				_eventQueue.Add(pe);
			}
			return pe;
		}
		
		/// <summary>
		/// Unregisters the sxpecified event
		/// </summary>
		/// <param name="pe">The event to unregister</param>
		public static void Unregister(OnReceivePacket pe) {
			pe.Unregister();
		}
		
		/// <summary>
		/// Unregister the event
		/// </summary>
		public override void Unregister()
		{
			_eventQueue.Remove(this);
		}
		
		/// <summary>
		/// This is meant to be called from the code where you mean for the event to happen.
		/// 
		/// In this case, it is called from the command processing code.
		/// </summary>
		/// <param name="p">The player that caused the event.</param>
		/// <param name="oldPos">The old position of the person.</param>
		internal static bool Call(Player p, byte[] data) {
			//Event was called from the code.
			List<OnReceivePacket> opcList = new List<OnReceivePacket>();
			//Do we keep or discard the event?
			_eventQueue.ForEach(opc => {
				if (opc.player == null || opc.player.username == p.username) {// We keep it
			        opc.player = p;
			        opc.Data = data;
					opc._queue(opc); // fire callback
					opcList.Add(opc); // add to used list
				}
			});
			return opcList.Any(pe => pe.cancel); //Return if any canceled the event.
		}
	}
}
