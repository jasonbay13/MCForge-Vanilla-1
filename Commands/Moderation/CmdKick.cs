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
using System;
using System.Collections.Generic;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;

namespace CommandDll.Moderation
{
    /// <summary>
    /// Command for kicking someone!
    /// </summary>
    public class CmdKick : ICommand
    {
        public string Name { get { return "Kick"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Nerketur"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0)
            {
                //Kick the user
                p.Kick("Congrats! You kicked yourself!");
            }
            else
            {
                List<Player> kickeeList = new List<Player>();

                //Is it an IP or a name?
                Server.ForeachPlayer(delegate(Player plr)
                {
                    if (plr.Ip == args[0] || plr.Username.StartsWith(args[0].ToLower()))
                        kickeeList.Add(plr); //When kicking someone, we don't care for case.
                });
                if (kickeeList.Count == 0)
                {
                    p.SendMessage("Sorry, but the specified player/IP is not online!");
                    return;
                }
                string reason = string.Join(" ", args, 1).Trim();
                if (reason.Length == 0)
                    reason = "You were kicked by " + p.Username;
                bool kickPlayer = false;
                foreach (Player kickee in kickeeList)
                {
                    if (Server.devs.Contains(kickee.Username))
                    {
                        kickPlayer = true;
                        p.SendMessage("You can't kick the developer " + kickee.Username + "!");
                    }
                    else
                        kickee.Kick(reason);
                }
                if (kickPlayer)
                {
                    String msg = "You tried to kick a developer!  Shame on you!";
                    if (Server.devs.Contains(p.Username))
                        p.SendMessage(msg);
                    else
                        p.Kick(msg);
                }
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/kick <player/IP> [message] - Kicks a player from the server");
            p.SendMessage("/kick - Kicks YOU from the server.");
            p.SendMessage("When using an IP, it kicks every user with a matching name and/or IP.");
            p.SendMessage("When using a username, it kicks every user with a matching beginning name.");
            p.SendMessage("For example: /kick ner will kick Ner, ner, Nerk, Nerketur, etc.");
            p.SendMessage("Shortcut: /k");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[] { "kick", "k" });
        }
    }
}
