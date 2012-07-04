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
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;
using System;

namespace MCForge.Commands.Moderation
{
    /// <summary>
    /// Command for kicking someone!
    /// </summary>
    public class CmdKick : ICommand
    {
        public string Name { get { return "Kick"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }
        public void Use(Player p, string[] args)
        {

            if (args.Length < 1) {
                Help(p);
                return;
            }

            string kickmsg = "";

            if (args.Length > 1)
                kickmsg = String.Join(" ", args, 1, args.Length - 1);

            if (args[0].Contains("."))
            {
                Server.Players.ForEach(pl =>
                {
                    if (pl.Ip == args[0])
                    {
                            pl.Kick(kickmsg.Trim() != "" ? kickmsg : "Kicked by " + p.Username + "!");
                    }
                    else
                    {
                        p.SendMessage("No player found with the IP of " + args[0] + ".");
                    }
                });
            }
            else
            {
                Player who = Player.Find(args[0]);
                if (who == null) { p.SendMessage("Player \"" + args[0] + "\" not found!"); return; }
                if (who.Group.Permission > p.Group.Permission) { p.SendMessage("You cannot kick your superiors!"); Player.UniversalChat(p.Color + p.Username + Server.DefaultColor + " tried to kick " + who.Color + who.Username + Server.DefaultColor + " but failed!"); return; }
                if (who == p) { p.Kick(p.Username + " kicked himself!"); return; }
                    who.Kick(kickmsg.Trim() != "" ? kickmsg : "Kicked by " + p.Username + "!");
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/kick <player/IP> [message] - Kicks a player from the server with optional [message].");
        }
        public void Initialize()
        {
            Command.AddReference(this, new string[] { "kick", "k" });
        }
    }
}
