using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;

namespace MCForge.API.PlayerEvent
{
	public class OnPlayerCommand : PlayerEvent {

		protected OnPlayerCommand(OnCall callback, Player target, string tag) {
			_type = EventType.Player;
			this.tag = tag;
			_target = target;
			_queue += callback;
		}

		public string cmd { get; set; }
		public string[] args { get; set; }
		public bool cancel { get; set; }

		internal static bool Call(Player p, string cmd, string[] args) {
			//Event was called from the code.
			List<PlayerEvent> opcList = new List<PlayerEvent>();
			//Do we keep or discard the event?
			_eventQueue.ForEach(playerEvent => {
				OnPlayerCommand opc = (OnPlayerCommand)playerEvent;
				if (opc._target == p) {// We keep it
					//Set up variables, then fire all callbacks.
					opc.cmd = cmd;
					opc.args = args;
					opc._queue(opc);
					opcList.Add(opc);
				}
			});
			return opcList.Any(pe => pe.cancel); //Retern list to see if any canceled the event.
		}

		public static PlayerEvent Register(PlayerEvent.OnCall callback, Player target, String tag) {
			//We add it to the list here
			tag += "OPC";
			PlayerEvent pe = _eventQueue.Find(match => match.tag == tag);
			if (pe != null)
				//It already exists, so wqe just add it to the queue.
				((OnPlayerCommand)pe)._queue += callback;
			else {
				//Doesn't exist yet.  Make a new one.
				pe = new OnPlayerCommand(callback, target, tag);
				_eventQueue.Add(pe);
			}
			return pe;
		}

		public static void Unregister(string tag) {
			tag += "OPC";
			PlayerEvent pe = _eventQueue.Find(match => match.tag == tag);
			if (pe != null)
				_eventQueue.Remove(pe);
		}
	}
}
