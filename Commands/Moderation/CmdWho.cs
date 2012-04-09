using System;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Core;

namespace CommandDll.Moderation {
    public class CmdWho : ICommand {
        public string Name { get { return "Who"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Nerketur"; } }
        public Version Version { get { return new Version(1,0); } }
        public string CUD { get { return ""; } }
        string[] CommandStrings = new string[3] { "whois", "whowas", "whoip" };
        public byte Permission { get { return 80; } }
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
					p.SendMessage("The specified player is not online.");
				} else {
					p.SendMessage(found.Username + " is on " + found.Level.Name);
					p.SendMessage(/*found.title + " " + */found.Username + " has:");
					p.SendMessage("> the rank of " + found.group.color + found.group.name);
					//p.SendMessage("> modified " + found.allmodified + " blocks and " + found.modified + " were changed since logging in.");
					//p.SendMessage("> time spent on server: " + found.totalTimeOnline);
					//p.SendMessage("> been logged in for " + found.timeLoggedOn);
					//p.SendMessage("> First logged into the server on " + found.firstLogin);
					//p.SendMessage("> Logged in " + found.timesOnline + " times, " + found.timesKicked + " of which ended in a kick.");
					//p.SendMessage("> " + found.numAwarded + "/" + Server.numAwards + " awards.");
					p.SendMessage("> the IP of " + found.ip);
					if (Server.devs.Contains(found.Username))
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
