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
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.World;
using MCForge.API;
using MCForge.API.PlayerEvent;

namespace CommandDll
{
    public class CmdReplace : ICommand
    {
        public string Name { get { return "Replace"; } }
        public CommandTypes Type { get { return CommandTypes.Building; } }
        public string Author { get { return "Gamemakergm"; } }
        public Version Version { get { return new Version(1,0); } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 100; } }

        public void Use(Player p, string[] args)
        {
            byte type = 0;
            byte type2 = 0;
            if (args.Length != 2)
            {
                p.SendMessage("Invalid arguments!");
                Help(p);
                return;
            }
            if (!Block.ValidBlockName(args[0 | 1]))
            {
                p.SendMessage("Could not find block specified");
            }

            //Block permissions here.
            CatchPos cpos = new CatchPos();
            cpos.type = type;
            cpos.type2 = type2;

            p.SendMessage("Place two blocks to determine the edges.");
            OnPlayerBlockChange.Register(CatchBlock, Priority.Normal, cpos, p);
            //p.CatchNextBlockchange(new Player.BlockChangeDelegate(CatchBlock), (object)cpos);
        }
        public void CatchBlock(OnPlayerBlockChange args)
        {
            CatchPos cpos = (CatchPos)args.GetData();
            cpos.pos = new Vector3(args.GetX(), args.GetZ(), args.GetZ());
            args.Unregister(true);
            args.Player.CatchNextBlockchange(CatchBlock2, (object)cpos);
        }
        public void CatchBlock2(Player p, ushort x, ushort z, ushort y, byte NewType, bool placed, object DataPass)
        {
            CatchPos FirstBlock = (CatchPos)DataPass;
            unchecked
            {
                if (FirstBlock.type != (byte)-1)
                {
                    NewType = FirstBlock.type;
                }
            }
            List<Pos> buffer = new List<Pos>();

            for (ushort xx = Math.Min((ushort)(FirstBlock.pos.x), x); xx <= Math.Max((ushort)(FirstBlock.pos.x), x); ++xx)
            {
                for (ushort zz = Math.Min((ushort)(FirstBlock.pos.z), z); zz <= Math.Max((ushort)(FirstBlock.pos.z), z); ++zz)
                {
                    for (ushort yy = Math.Min((ushort)(FirstBlock.pos.y), y); yy <= Math.Max((ushort)(FirstBlock.pos.y), y); ++yy)
                    {
                        Vector3 loop = new Vector3(xx, zz, yy);
                        if (p.Level.GetBlock(loop) == NewType)
                        {
                            BufferAdd(buffer, loop);
                        }
                    }
                }
            }
            //Group Max Blocks permissions here
            p.SendMessage(buffer.Count.ToString() + " blocks.");

            //Level Blockqueue .-.

            buffer.ForEach(delegate(Pos pos)
            {
                p.Level.BlockChange((ushort)(pos.pos.x), (ushort)(pos.pos.z), (ushort)(pos.pos.y), FirstBlock.type2);
            });
        }
        public void Help(Player p)
        {
            p.SendMessage("/replace [type] [type2] - Replaces type with type2 inside a selected cuboid.");
        }

        public void Initialize()
        {
            string[] CommandStrings = new string[2] { "replace", "r" };
            Command.AddReference(this, CommandStrings);
        }
        void BufferAdd(List<Pos> list, Vector3 type)
        {
            Pos pos;
            pos.pos = type;
            list.Add(pos);
        }
        private struct CatchPos
        {
            public byte type;
            public byte type2;
            public Vector3 pos;
        }
        struct Pos
        {
            public Vector3 pos;
        }
    }
}
