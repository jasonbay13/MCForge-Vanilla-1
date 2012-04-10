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
			PlayerGroup wanted = null;
			if (args.Count() == 1) {
				wanted = PlayerGroup.Find(args[0].ToLower()); // will be null if none found.
				if (wanted == null)
					p.SendMessage("Spcified group doesn't exist.  Using all groups below you...");
			}
			if (wanted != null && wanted.permission >= p.group.permission) {
				wanted = null;
				p.SendMessage("Sorry, you can only patrol groups of a lower rank.  Using all groups below you...");
			}
			p.SendMessage("Finding a person " + (wanted==null?"under you":"of the specified rank") + " to patrol...");
			ICommand gotoCmd = Command.Find("goto"); //If goto exists, we can use it to go to the new level before teleporting.
			List<Player> allUnder = Server.Players.FindAll(plr => (wanted == null ? true : plr.group.permission == wanted.permission) && plr.group.permission < p.group.permission && (gotoCmd == null ? p.Level == plr.Level : true));
			if (allUnder.Count == 0) {
				p.SendMessage("There are no people " + (wanted == null ? "under your" : "of the specified") + " rank that are " + (gotoCmd == null ? "in your level." : "currently online."));
				return;
			}
			Player found = allUnder[(new Random()).Next(allUnder.Count)];
			p.SendMessage("Player found!  Transporting you to " + found.color + found.Username + Server.DefaultColor + "!");
			if (p.Level != found.Level) {
				//Go to the level first
				gotoCmd.Use(p, new string[] { found.Level.Name });
			}
			if (found.isLoading) {
				p.SendMessage("Waiting for " + found.color + found.Username + Server.DefaultColor + " to spawn...");
				
				while (found.isLoading) { } // until event works
			}
			while (p.isLoading) { } // until event works.
			p.SendToPos(found.Pos, found.Rot);
		}

		public void Help(Player p) {
			p.SendMessage("/patrol - teleports the user to 'patrol' a user with a lower rank than them.");
			p.SendMessage("/patrol [rank] - teleports the user to 'patrol' a random user within a certain rank");
		}

		public void Initialize() {
			Command.AddReference(this, CommandStrings);
		}
	}
}
