using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Groups;

namespace CommandDll.Moderation {
	public class CmdPatrol : ICommand {
		string _Name = "Patrol";
		public string Name { get { return _Name; } }

		CommandTypes _Type = CommandTypes.mod;
		public CommandTypes Type { get { return _Type; } }

		string _Author = "Nerketur";
		public string Author { get { return _Author; } }

		int _Version = 1;
		public int Version { get { return _Version; } }

		string _CUD = "";
		public string CUD { get { return _CUD; } }

		byte _Permission = 80;
		public byte Permission { get { return _Permission; } }


		string[] CommandStrings = new string[1] { "patrol" };

		public void Use(Player p, string[] args) {
			p.SendMessage("Finding a person under you to patrol...");
			ICommand gotoCmd = Command.Find("goto");
			List<Player> allUnder = Server.Players.FindAll(plr => plr.group.permission < p.group.permission && (gotoCmd == null ? p.level == plr.level : true));
			if (allUnder.Count == 0) {
				p.SendMessage("There are no people under your rank that are " + (gotoCmd==null ? "in your level." : "currently online."));
				return;
			}
			Player found = allUnder[(new Random()).Next(allUnder.Count)];
			if (p.level != found.level) {
				//Go to the level first
				gotoCmd.Use(p, new string[] { found.level.name });
			}
			if (found.isLoading) {
				p.SendMessage("Waiting for " + found.color + found.USERNAME + Server.DefaultColor + " to spawn...");
				
				while (found.isLoading) { } // until event works
			}
			while (p.isLoading) { } // until event works.
			p.SendToPos(found.Pos, found.Rot);
		}

		public void Help(Player p) {
			p.SendMessage("");
		}

		public void Initialize() {
			Command.AddReference(this, CommandStrings);
		}
	}
}
