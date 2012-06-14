/*
Copyright 2011 MCForge
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
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;

namespace MCForge.Commands.Moderation
{
    public class CmdWhois : ICommand
    {
        public string Name { get { return "Whois"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Nerketur"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }
        public void Use(Player p, string[] args)
        {
            //We can get an IP, or a name.
            //Because of the fact IPs can give multiple names, we will show all possibilities.
            //Plus, people can have a name that's identical to an IP.
            if (args.Length == 0)
                Help(p);
            else if (Player.ValidName(args[0]))
            {
                // Is it an online player?
                Player found = Player.Find(args[0]);
                if (found == null)
                {
                    p.SendMessage("The specified player is not online.");
                }
                else
                {
                    p.SendMessage(found.Username + " is on " + found.Level.Name);
                    p.SendMessage(/*found.title + " " + */found.Username + " has:");
                    p.SendMessage("> the rank of " + found.Group.Color + found.Group.Name);
                    //p.SendMessage("> modified " + found.allmodified + " blocks and " + found.modified + " were changed since logging in.");
                    //p.SendMessage("> time spent on server: " + found.totalTimeOnline);
                    //p.SendMessage("> been logged in for " + found.timeLoggedOn);
                    //p.SendMessage("> First logged into the server on " + found.firstLogin);
                    //p.SendMessage("> Logged in " + found.timesOnline + " times, " + found.timesKicked + " of which ended in a kick.");
                    //p.SendMessage("> " + found.numAwarded + "/" + Server.numAwards + " awards.");
                    p.SendMessage("> the IP of " + found.Ip);
                    if (Server.Devs.Contains(found.Username))
                        p.SendMessage("> " + found.Username + " is an MCForge Developer.");
                }
            }
            else
            {
                Help(p);
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/whois <player/IP> - Displays information about a player or IP.");
            p.SendMessage("If there are multiple results, it returns all associated accounts.");
        }
        public void Initialize()
        {
            Command.AddReference(this, new string[] { "whois", "whowas", "whoip" });
        }
    }
}
