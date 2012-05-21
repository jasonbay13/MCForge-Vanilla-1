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
using MCForge.World;
using MCForge.API.Events;

namespace CommandDll
{
    public class CmdRainbow : ICommand
    {
        public string Name { get { return "Rainbow"; } }
        public CommandTypes Type { get { return CommandTypes.building; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            p.SendMessage("Place two block to determine the edges...");
            p.OnPlayerBlockChange.Normal += FirstChange;
        }
        void FirstChange(Player p, BlockChangeEventArgs args)
        {
            CatchPos cpos = new CatchPos();
            cpos.pos = new Vector3(args.X, args.Z, args.Y);
            cpos.block = args.Holding;
            //args.Cancel();
            p.OnPlayerBlockChange.Normal -= FirstChange;
            p.setDatapass(this.Name, cpos);
        }
        void SecondChange(Player p, BlockChangeEventArgs args)
        {
            try
            {
                p.OnPlayerBlockChange.Normal -= SecondChange;
                CatchPos cpos = (CatchPos)(p.GetDatapass(this.Name));
                for (int i = cpos.pos.x; i <= args.X; i++) { p.Level.BlockChange((ushort)(i), args.Z, args.Y, Block.BlockList.RED_CLOTH); }
                //args.Cancel();
            }
            catch { p.SendMessage("MEEP!"); }
        }
        public void Help(Player p)
        {
            p.SendMessage("/click [x z y]- Fakes a click");
            p.SendMessage("if no xyz is given, it uses the last place clicked.");
            p.SendMessage("/click 200 z 200 will cuase it to click at 200x, last z, and 200y");
            p.SendMessage("Shortcut: /x");
        }
        protected struct CatchPos
        {
            public byte block;
            public Vector3 pos;
        }
        public void Initialize()
        {
            string[] CommandStrings = new string[1] { "rainbow" };
            Command.AddReference(this, CommandStrings);
        }
    }
}
