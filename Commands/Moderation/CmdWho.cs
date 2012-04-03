using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Core;

namespace CommandDll.Moderation {
    public class CmdWho : ICommand {
        string _Name = "Who";
        public string Name { get { return _Name; } }

        CommandTypes _Type = CommandTypes.mod;
        public CommandTypes Type { get { return _Type; } }

        string _Author = "Nerketur";
        public string Author { get { return _Author; } }

        int _Version = 1;
        public int Version { get { return _Version; } }

        string _CUD = "";
        public string CUD { get { return _CUD; } }

        string[] CommandStrings = new string[] { "who", "whois", "whowas", "whoip" };

        byte _Permission = 80;
        public byte Permission { get { return _Permission; } }

        public void Use(Player p, string[] args) {
            //We can get an IP, or a name.
            //Because of the fact IPs can give multiple names, we will show all possibilities.
            //Plus, people can have a name that's identical to an IP.
			if (args.Length == 0)
				Help(p);
			else if (Player.ValidName(args[0])) {
				// Is it an online player?
				Player found = Player.Find(args[0]);
				if (found == null) {
					//The player is offline/nonexistant
				}
				if (found == null) {
					p.SendMessage("The specified player is not online.");
				} else {
					//p.SendMessage(found.USERNAME + " is on " + found.level.name); //commented out because of a build time error
					//p.SendMessage(found.title + " " + found.USERNAME + " has:");
					//p.SendMessage("> the rank of " + found.group.name);
					//p.SendMessage("> modified " + found.allmodified + " blocks and " + found.modified + " were changed since logging in.");
					//p.SendMessage("> time spent on server: " + found.totalTimeOnline);
					//p.SendMessage("> been logged in for " + found.timeLoggedOn);
					//p.SendMessage("> First logged into the server on " + found.firstLogin);
					//p.SendMessage("> Logged in " + found.timesOnline + " times, " + found.timesKicked + " of which ended in a kick.");
					//p.SendMessage("> " + found.numAwarded + "/" + Server.numAwards + " awards.");
					p.SendMessage("> the IP of " + found.ip);
					if (Server.devs.Contains(found.USERNAME))
						p.SendMessage("> Player is a Developer.");
				}
			} else {
				Help(p);
			}
        }

        public void Help(Player p) {
            p.SendMessage("/who - Displays information about a player or IP.");
            p.SendMessage("  If there are multiple results, it returns all associated accounts.");
        }

        public void Initialize() {
            Command.AddReference(this, CommandStrings);
        }
    }
}
