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
using MCForge.World;
using MCForge.Utils;

namespace CommandDll
{
    public class CmdNewLevel : ICommand
    {
        public string Name { get { return "NewLevel"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Snowl"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }
        public void Use(Player p, string[] args) //TODO Make this more customizeable
        {
            Vector3S size;
            string type = "";
            switch (args.Length) // initialize depending on arguments given
            {
                case 1:
                    size = new Vector3S(64, 64, 32);
                    break;
                case 4:
                    size = new Vector3S(short.Parse(args[1]), short.Parse(args[2]), short.Parse(args[3]));
                    break;
                case 5:
                    size = new Vector3S(short.Parse(args[1]), short.Parse(args[2]), short.Parse(args[3]));
                    type = args[4];
                    break;
                default: Help(p); return;
            }
            if (Level.FindLevel(args[0]) != null)
            {
                p.SendMessage("This level already exists!");
                return;
            }

            Level temp = null;
            switch (type.ToLower()) // experimental type finding
            {
                case "flat":
                    temp = Level.CreateLevel(size, Level.LevelTypes.Flat, args[0]);
                    break;
                default:
                    temp = Level.CreateLevel(size, Level.LevelTypes.Flat, args[0]);
                    break;
            }

            if (temp == null) { p.SendMessage("Level creation failed"); return; } // something is wrong if you get this
            Level.AddLevel(temp);
            Player.UniversalChat("Created level " + args[0] + "!");
        }

        public void Help(Player p)
        {
            p.SendMessage("/newlevel <name> <x z y> [type] - Creates a new level called <name>.");
            p.SendMessage("Currently, types are experimental.");
            p.SendMessage("Shortcut: /newlvl");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "newlevel", "newlvl" });
        }
    }
}
