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
using System.Threading;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Utilities.Settings;

namespace CommandDll
{
    public class CmdTeleport : ICommand
    {
        public string Name { get { return "Teleport"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Gamemakergm"; } }
        public decimal Version { get { return 1.00m; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }


        public void Use(Player p, string[] args)
        {
            if (args.Length > 2)
            {
                p.SendMessage("Invalid arguments!");
                return;
            }
            else if (args.Length == 0)
            {
                Vector3 meep = new Vector3((short)(0.5 + p.Level.SpawnPos.x * 32), (short)(0.5 + p.Level.SpawnPos.z * 32), (short)(1 + p.Level.SpawnPos.y * 32));
                p.SendToPos(meep, p.Level.SpawnRot);
            }
            else if (args.Length == 1)
            {
                Player who = Player.Find(args[0]);
                if (who == null || who.isHidden)
                {
                    p.SendMessage("Player: " + args[0] + " not found!");
                    return;
                }
                else if (who == p)
                {
                    p.SendMessage("Why are you trying to teleport yourself to yourself?");
                    return;
                }
                else if (!ServerSettings.GetSettingBoolean("AllowHigherRankTp") && p.group.permission < who.group.permission)
                {
                    p.SendMessage("You cannot teleport to a player of higher rank!");
                    return;
                }
                else
                {
                    if (p.Level != who.Level)
                    {
                        //Need goto here
                        if (who.isLoading)
                        {
                            p.SendMessage("Waiting for " + who.color + who.Username + Server.DefaultColor + " to spawn...");
                            while (who.isLoading) { }
                        }
                    }
                }
                p.SendToPos(who.Pos, who.Rot);
                return;
            }
            else
            {
                Player one = Player.Find(args[0]);
                Player two = Player.Find(args[1]);
                if (one == null || two == null)
                {
                    //Hehe
                    p.SendMessage((one == null && two == null) ? "Players: " + args[0] + " and " + args[1] + " not found!" : "Player: " + ((one == null) ? args[0] : args[1]) + " not found!");
                    return;
                }
                else if (one == p && two == p || one == p)
                {
                    p.SendMessage((two == p) ? "Why are you trying to teleport yourself to yourself?" : "Why not just use /tp " + args[1] + "?");
                    return;
                }
                else if (two == p)
                {
                    p.SendMessage("Why not just use /summon " + args[0] + "?");
                    return;
                }
                else if (p.group.permission < one.group.permission)
                {
                    p.SendMessage("You cannot force a player of higher rank to tp to another player!");
                }
                else
                {
                    if (one.Level != two.Level)
                    {
                        //Need goto here
                        if (two.isLoading)
                        {
                            p.SendMessage("Waiting for " + two.color + two.Username + Server.DefaultColor + " to spawn...");
                            while (two.isLoading) { }
                        }
                    }
                }
                one.SendToPos(two.Pos, two.Rot);
                p.SendMessage(one.Username + " has been succesfully teleported to " + two.Username + "!");
                return;
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/tp <player1> [player2] - Teleports yourself to a player.");
            p.SendMessage("[player2] is optional, but if present will send player1 to the player2.");
            p.SendMessage("If <player> is blank, you are sent to spawn");
        }

        public void Initialize()
        {
            Command.AddReference(this, "tp");
        }
    }
}
