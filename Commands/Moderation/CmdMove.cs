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
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.World;
using System;
namespace CommandDll
{
    public class CmdMove : ICommand
    {
        public string Name { get { return "Move"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Snowl"; } }
        public Version Version { get { return new Version(1, 0); } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length != 2)
                p.SendMessage("You need to specify 2 arguments to use this command!");
            else
            {
                if (args[0] == "all")
                {
                    bool sentPlayer = false;
                    Level level = Level.FindLevel(args[1]);
                    if (level != null)
                    {
                        Server.ForeachPlayer(delegate(Player pl)
                        {
                            if (p.group.permission > pl.group.permission) //Missing permissions
                            {
                                //TODO need to despawn here
                                pl.Level = Level.FindLevel(args[1]);
                                //TODO need to respawn here
                                sentPlayer = true;
                                pl.SendMessage("You were moved by " + p.color + p.Username + Server.DefaultColor + " to the level" + args[1] + ".");
                            }
                        });
                    }
                    else
                    {
                        p.SendMessage(args[1] + " is not loaded/does not exist!");
                    }
                    if (sentPlayer)
                        p.SendMessage("Sent all players to " + args[1]);
                    else
                        p.SendMessage("Could not send any players to " + args[1]);
                }
                else
                {
                    bool sentPlayer = false;
                    Player tempPlayer = Player.Find(args[0]);
                    if (tempPlayer != null && p.group.permission > tempPlayer.group.permission)
                    {
                        Level level = Level.FindLevel(args[1]);
                        if (level != null)
                        {
                            //TODO need to despawn here
                            tempPlayer.Level = level;
                            //TODO need to respawn here
                            sentPlayer = true;
                            tempPlayer.SendMessage("You have been moved to " + tempPlayer.Level.name);
                        }
                        else
                        {
                            p.SendMessage(args[1] + " is not loaded/does not exist!");
                        }
                    }
                    if (sentPlayer)
                        p.SendMessage("Sent player " + args[0] + " to " + args[1]);
                    else
                        p.SendMessage("Could not send player " + args[0] + " to " + args[1]);
                }
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/move [player] [level] - Moves player to [level].");
            p.SendMessage("/move all [level] - Moves everyone to [level].");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "move", "mv" });
        }
    }
}

