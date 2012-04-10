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
using System;
using MCForge.World;
namespace CommandDll
{
    public class CmdLoad : ICommand
    {
        public string Name { get { return "Load"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Snowl"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            Level isAlreadyLoaded = Level.FindLevel(args[0]);
            if (isAlreadyLoaded == null)
            {
                Level temp = Level.LoadLevel(args[0]);
                if (temp != null)
                {
                    Level.AddLevel(temp);
                    Player.UniversalChat("Loaded " + args[0] + "!");
                }
                else
                {
                    p.SendMessage("Could not load " + args[0] + ".");
                }
            }
            else 
            {
                p.SendMessage(args[0] + " is already loaded.");
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/load [level] - Loads [level].");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "load" });
        }
    }
}

