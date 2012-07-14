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
using MCForge.API.Events;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.World;
using MCForge.Utils;
using System.Linq;

namespace MCForge.Commands
{
    public class CmdReplaceNot : ICommand
    {
        public string Name { get { return "ReplaceNot"; } }
        public CommandTypes Type { get { return CommandTypes.Building; } }
        public string Author { get { return "Gamemakergm"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length != 2)
            {
                p.SendMessage("Invalid number of arguments!");
                Help(p);
                return;
            }

            CatchPos cpos = new CatchPos();
            List<string> ignore;

            if (args[0].Contains(","))
                ignore = new List<string>(args[0].Split(','));
            else
                ignore = new List<string>() { args[0] };

            ignore = ignore.Distinct().ToList(); // Remove duplicates

            List<string> invalid = new List<string>(); //Check for invalid blocks
            foreach (string name in ignore)
                if (!Block.ValidBlockName(name))
                    invalid.Add(name);
            if (!Block.ValidBlockName(args[1]))
                invalid.Add(args[1]);
            if (invalid.Count > 0)
            {
                p.SendMessage(String.Format("Invalid block{0}: {1}", invalid.Count == 1 ? "" : "s", String.Join(", ", invalid)));
                return;
            }

            if (ignore.Contains(args[1]))
                ignore.Remove(args[1]);

            cpos.ignore = new List<byte>();
            foreach (string name in ignore)
                cpos.ignore.Add(Block.NameToBlock(name));
            cpos.newType = Block.NameToBlock(args[1]);

            p.SendMessage("Place two blocks to determine the edges.");
            p.SetDatapass(this.Name, cpos);
            p.OnPlayerBlockChange.Normal += new Event<Player, BlockChangeEventArgs>.EventHandler(CatchBlock1);
        }
       
        public void Help(Player p)
        {
            p.SendMessage("/replacenot <block> <block2> - Replaces everything but <block> with <block2> inside a selected cuboid.");
            p.SendMessage("Shortcut: /rn");
        }

        public void Initialize()
        {
            string[] CommandStrings = new string[2] { "replacenot", "rn" };
            Command.AddReference(this, CommandStrings);
        }

        public void CatchBlock1(Player p, BlockChangeEventArgs args)
        {
            CatchPos cpos = (CatchPos)p.GetDatapass(this.Name);
            cpos.pos = new Vector3S(args.X, args.Z, args.Y);
            args.Cancel();
            p.OnPlayerBlockChange.Normal -= new Event<Player, BlockChangeEventArgs>.EventHandler(CatchBlock1);
            p.SetDatapass(this.Name, cpos);
            p.OnPlayerBlockChange.Normal += new BlockChangeEvent.EventHandler(CatchBlock2);
        }

        public void CatchBlock2(Player p, BlockChangeEventArgs args)
        {
            p.OnPlayerBlockChange.Normal -= new Event<Player, BlockChangeEventArgs>.EventHandler(CatchBlock2);
            CatchPos cpos = (CatchPos)p.GetDatapass(this.Name);
            cpos.pos2 = new Vector3S(args.X, args.Z, args.Y);
            args.Cancel();
            Replace(cpos, p);
        }

        private void Replace(CatchPos cpos, Player p)
        {
            List<Pos> buffer = new List<Pos>();
            ushort x, z, y;
            ushort xmin = Math.Min((ushort)cpos.pos.x, (ushort)cpos.pos2.x),
                xmax = Math.Max((ushort)cpos.pos.x, (ushort)cpos.pos2.x),
                zmin = Math.Min((ushort)cpos.pos.z, (ushort)cpos.pos2.z),
                zmax = Math.Max((ushort)cpos.pos.z, (ushort)cpos.pos2.z),
                ymin = Math.Min((ushort)cpos.pos.y, (ushort)cpos.pos2.y),
                ymax = Math.Max((ushort)cpos.pos.y, (ushort)cpos.pos2.y);

            for (x = xmin; x <= xmax; ++x)
                for (z = zmin; z <= zmax; ++z)
                    for (y = ymin; y <= ymax; ++y)
                        if (!cpos.ignore.Contains(p.Level.GetBlock(x, z, y)))
                            BufferAdd(buffer, new Vector3S(x, z, y));

            p.SendMessage(buffer.Count.ToString() + " blocks.");
            buffer.ForEach(delegate(Pos pos)
            {
                p.Level.BlockChange((ushort)(pos.pos.x), (ushort)(pos.pos.z), (ushort)(pos.pos.y), cpos.newType, p);
            });
        }

        protected void BufferAdd(List<Pos> list, Vector3S position)
        {
            Pos pos;
            pos.pos = position;
            list.Add(pos);
        }

        protected struct CatchPos
        {
            public List<byte> ignore;
            public byte newType;
            public Vector3S pos;
            public Vector3S pos2;
        }

        protected struct Pos
        {
            public Vector3S pos;
        }
    }
}