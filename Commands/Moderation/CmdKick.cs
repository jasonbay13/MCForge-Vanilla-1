/*
Copyright 2012 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll.Moderation {
	/// <summary>
	/// Cpommand for kicking someone!
	/// </summary>
	public class CmdKick : ICommand {
		string _Name = "Kick";
		public string Name { get { return _Name; } }

		CommandTypes _Type = CommandTypes.mod;
		public CommandTypes Type { get { return _Type; } }

		string _Author = "Nerketur";
		public string Author { get { return _Author; } }

		int _Version = 1;
		public int Version { get { return _Version; } }

		string _CUD = "";
		public string CUD { get { return _CUD; } }

		string[] CommandStrings = new string[1] { "kick" };

        byte _Permission = 80;
        public byte Permission { get { return _Permission; } }

		public void Use(Player p, string[] args) {
			if (args.Length == 0) {
				//Kick the user
				p.Kick("Congrats!  You kicked yourself!");
			} else {
				List<Player> kickeeList = new List<Player>();

				//Is it an IP or a name?
				Server.ForeachPlayer(delegate(Player plr)
				{
					if (plr.ip == args[0] || plr.username.StartsWith(args[0].ToLower()))
						kickeeList.Add(plr); //When kicking someone, we don't care for case.
				});
				if (kickeeList.Count == 0) {
					p.SendMessage("Sorry, but the specified player/IP is not online!");
					return;
				}
				string reason = string.Join(" ", args, 1).Trim();
				if (reason.Length == 0)
					reason = "You were kicked by " + p.USERNAME;
				bool kickPlayer = false;
				foreach (Player kickee in kickeeList) {
					if (Server.devs.Contains(kickee.USERNAME)) {
						kickPlayer = true;
						p.SendMessage("You can't kick the developer " + kickee.USERNAME + "!");
					} else
						kickee.Kick(reason);
				}
				if (kickPlayer) {
					String msg = "You tried to kick a developer!  Shame on you!";
					if (Server.devs.Contains(p.USERNAME))
						p.SendMessage(msg);
					else
						p.Kick(msg);
				}
			}
		}

		public void Help(Player p) {
			p.SendMessage("/kick [username/ip [message]] - Kicks a username/ip from the server");
			p.SendMessage("/kick - Kicks YOU from the server.");
			p.SendMessage("With an IP, it kicks every user with a matching name and/or IP");
			p.SendMessage("With a username, it kicks every user with a matching beginning name.");
			p.SendMessage("For example.. /kick ner will kick Ner, ner, Nerk, Nerketur, etc.");
		}

		public void Initialize() {
			Command.AddReference(this, CommandStrings);
		}
	}
}
