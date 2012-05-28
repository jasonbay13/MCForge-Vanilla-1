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
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;
using MCForge.World;
using MCForge.API.Events;

namespace CommandDll
{
    public class CmdWrite : ICommand
    {
        public string Name { get { return "Write"; } }
        public CommandTypes Type { get { return CommandTypes.Building; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { p.SendMessage("Please specify a message to write!"); Help(p); return; }
            CatchPos cpos = new CatchPos();
            cpos.message = string.Join(" ", args);
            p.SetDatapass(Name, cpos);
            p.SendMessage("Place two blocks to determine the direction!");
            p.OnPlayerBlockChange.Normal += BlockChange1;
        }
        void BlockChange1(Player sender, BlockChangeEventArgs args)
        {
            CatchPos cpos = (CatchPos)sender.GetDatapass(Name);
            cpos.pos = new Vector3S((short)args.X, (short)args.Z, (short)args.Y);
            args.Cancel();
            sender.OnPlayerBlockChange.Normal -= BlockChange1;
            sender.SetDatapass(Name, cpos);
            sender.OnPlayerBlockChange.Normal += BlockChange2;
        }
        void BlockChange2(Player sender, BlockChangeEventArgs args)
        {
            string direction = null;
            sender.OnPlayerBlockChange.Normal -= BlockChange2;
            CatchPos cpos = (CatchPos)sender.GetDatapass(this.Name);
            if (args.Z > cpos.pos.z) { direction = "r"; }
            else if (cpos.pos.z > args.Z) { direction = "l"; }
            else if (args.X > cpos.pos.x) { direction = "u"; }
            else if (cpos.pos.x > args.X) { direction = "d"; }
            else { sender.SendMessage("No direction was selected!"); args.Cancel(); return; }
            WorldComponent.DrawString(sender, cpos.pos.x, cpos.pos.z, cpos.pos.y, cpos.message, direction, args.Holding);
            args.Cancel();
        }
        public void Help(Player p)
        {
            p.SendMessage("write <message> - writes a specified message in blocks");
            p.SendMessage("Shortcut: /wt");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "write", "wt"});
        }
        protected struct CatchPos
        {
            public byte block;
            public Vector3S pos;
            public string message;
        }
    }
}