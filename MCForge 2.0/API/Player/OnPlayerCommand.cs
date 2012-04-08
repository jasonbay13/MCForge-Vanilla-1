using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;

namespace MCForge.API.PlayerEvent
{
	public class OnPlayerCommand : PlayerEvent {

		protected OnPlayerCommand(OnCall callback, Priority pri, Player target) {
			_type = EventType.Player;
			_pri = pri;
			_target = target;
			_queue += callback;
		}

		public string cmd { get; set; }
		public string[] args { get; set; }
		public bool canceled { get; set; }

		internal static List<PlayerEvent> Call(Player p, string cmd, string[] args) {
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
			return opcList; //Retern list to see if any canceled the event.
		}

		public static void Register(PlayerEvent.OnCall callback, Priority pri, Player target) {
			//We add it to the list here
			OnPlayerCommand pe = new OnPlayerCommand(callback, pri, target);
			_eventQueue.Add(pe);
		}
	}
}
