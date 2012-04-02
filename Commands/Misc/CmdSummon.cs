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
using System.Linq;
using System.Text;
using System.Threading;
using MCForge;

namespace CommandDll
{
    public class CmdSummon : ICommand
    {
        public string Name { get { return "Summon"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Gamemakergm"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length != 1)
            {
                p.SendMessage("Invalid arguments!");
                return;
            }
            else
            {
                if(args[0].ToLower() == "all")
                {
                        foreach(Player pl in Server.Players.ToArray())
                        {
                            if (pl.level == p.level && pl != p) //Missing permissions
                            {
                                pl.SendToPos(p.Pos, p.Rot);
                                pl.SendMessage("You were summoned by " + p.color + p.USERNAME + Server.DefaultColor + ".");
                            }
                        }
                    Player.UniversalChat(p.color + p.USERNAME + Server.DefaultColor + " summoned everyone!");
                    return;
                }
                else
                {
                Player who = Player.Find(args[0]);
                if (who == null || who.isHidden) //Permissions needed
                {
                    p.SendMessage("Player: " + args[0] + " not found!");
                    return;
                }
                else if (who == p)
                {
                    p.SendMessage("Why are you trying to summon yourself?");
                    return;
                }
                /*else if (p.permission < who.permission)
                {
                    //Permissions to check ranks.
                    p.SendMessage("You cannot summon someone ranked higher thank you!");
                    return;
                }*/
                else
                {
                    if (p.level != who.level)
                    {
                        p.SendMessage(who.USERNAME + " is in a different level. Forcefetching has started!");
                        Level where = p.level;
                        //Need to use goto here
                        Thread.Sleep(1000); //Let them load;   
                            while (who.isLoading) 
                            { 
                                Thread.Sleep(250); 
                            }
                        }
                    }
                    who.SendToPos(p.Pos, p.Rot);
                }
                return;
        }
        }

        public void Help(Player p)
        {
            p.SendMessage("/summon <player> - Summons player to your position.");
            p.SendMessage("/summon all - Summons all players in the map.");
        }

        public void Initialize()
        {
            Command.AddReference(this, "summon");
        }
    }
}
