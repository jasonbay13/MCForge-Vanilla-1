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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Utils;
using MCForge.API.Events;

namespace MCForge
{
    public class CmdSelect : ICommand
    {
        public string Name { get { return "Select"; } }
        public CommandTypes Type { get { return CommandTypes.Building; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "select", "mark" });
        }
        public void Use(Player p, string[] args)
        {
            p.SendMessage("Break or place a block to select the first point.");
            p.OnPlayerBlockChange.Normal += new BlockChangeEvent.EventHandler(BC1);
        }
        public void BC1(Player p, BlockChangeEventArgs args)
        {
            p.ExtraData["Mark1"] = new Vector3S(args.X, args.Z, args.Y);
            p.SendMessage("First coordinate marked. (" + args.X + ", " + args.Z + ", " + args.Y + ")");
            args.Unregister();
            args.Cancel();
            p.SendMessage("Break or place a block to select the second point.");
            p.OnPlayerBlockChange.Normal += new BlockChangeEvent.EventHandler(BC2);
        }
        public void BC2(Player p, BlockChangeEventArgs args)
        {
            Vector3S _s = new Vector3S();
            p.ExtraData["Mark2"] = new Vector3S(args.X, args.Z, args.Y);
            p.SendMessage("Second coordinate marked. (" + args.X + ", " + args.Z + ", " + args.Y + ")");
            args.Unregister();
            args.Cancel();
            Vector3S m1 = (Vector3S)p.ExtraData["Mark1"];
            Vector3S m2 = (Vector3S)p.ExtraData["Mark2"];
            p.SendMessage("Marked coordinates:");
            p.SendMessage("(" + m1.x + ", " + m1.z + ", " + m1.y + "), (" + m2.x + ", " + m2.z + ", " + m2.y + ")");
            p.ExtraData["HasMarked"] = true;
        }
        public void Help(Player p)
        {
            p.SendMessage("/select - Select two points for use in build commands.");
            p.SendMessage("Shortcut: /mark");
        }
    }
}