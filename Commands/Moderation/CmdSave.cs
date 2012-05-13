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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.World;
using System;
namespace CommandDll
{
    public class CmdSave : ICommand
    {
        public string Name { get { return "Save"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Snowl"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0)
            {
                p.Level.SaveToBinary();
                Player.UniversalChat("Saved " + p.Level.Name);
            }
            else
            {
                try
                {
                    Level.FindLevel(args[0]).SaveToBinary();
                    Player.UniversalChat("Saved " + args[0]);
                }
                catch { p.SendMessage("This level does not exist!"); }
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/save - Saves the level you are currently on.");
            p.SendMessage("/save [level] - Saves [level].");
        }

        public void Initialize()
        {
            Command.AddReference(this, "save");
        }
    }
}

