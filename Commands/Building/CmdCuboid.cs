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
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.World;
using MCForge.API.Events;
using MCForge.Utils;

namespace MCForge.Commands
{
    public class CmdCuboid : ICommand
    {
        public string Name { get { return "Cuboid"; } }
        public CommandTypes Type { get { return CommandTypes.Building; } }
        public string Author { get { return "Gamemakergm, 7imekeeper"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        CatchPos main;

        public void Use(Player p, string[] args)
        {
            main = new CatchPos();
            main.block = 255;
            main.cuboidType = SolidType.solid;
            switch (args.Length)
            {
                case 0:
                    break;
                case 1: // Block or Type ONLY
                    if (ValidSolidType(args[0]) || ValidBlockName(args[0]))
                        break;
                    p.SendMessage("Invalid block or type!");
                    return;
                case 2: // Block AND Type
                    if (ValidSolidType(args[0]) || ValidSolidType(args[1]))
                        if (ValidBlockName(args[0]) || ValidBlockName(args[1]))
                            break;
                    p.SendMessage("Invalid block or type!");
                    return;
                case 6:
                    main.block = 1;
                    ParseCoordinates(p, args);
                    return;
                case 7:
                    if (ValidSolidType(args[6]) || ValidBlockName(args[6]))
                        ParseCoordinates(p, args);
                    else
                        p.SendMessage("Invalid block!");
                    return;
                case 8:
                    if (ValidSolidType(args[6]) || ValidSolidType(args[7]))
                        if (ValidBlockName(args[6]) || ValidBlockName(args[7]))
                            ParseCoordinates(p, args);
                    else
                        p.SendMessage("Invalid block or type!");
                    return;
                default:
                    p.SendMessage("Invalid number of arguments!");
                    Help(p);
                    return;
            }

            p.SendMessage("Place two blocks to determine the corners.");
            p.OnPlayerBlockChange.Normal += new Event<Player, BlockChangeEventArgs>.EventHandler(CatchBlock1);
        }

        public void Help(Player p)
        {
            p.SendMessage("/cuboid [block] [type] - Creates a cuboid of blocks.");
            p.SendMessage("/cuboid x1 z1 y1 x2 z2 y2 [block] [type] - Creates a cuboid at the specified coordinates. If no block is specified, stone will be used.");
            p.SendMessage("Available types: <solid/hollow/walls/holes/wire/random>");
            p.SendMessage("Note that [block] and [type] are optional and can be in any order.");
            p.SendMessage("Shortcut: /z");
        }

        public void Initialize()
        {
            string[] CommandStrings = new string[2] { "cuboid", "z" };
            Command.AddReference(this, CommandStrings);
        }

        public void CatchBlock1(Player p, BlockChangeEventArgs args)
        {
            CatchPos cpos = new CatchPos();
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
            cpos.secondPos = new Vector3S(args.X, args.Z, args.Y);
            cpos.block = args.Holding;
            args.Cancel();
            Cuboid(cpos, p);
        }

        private void Cuboid(CatchPos cpos, Player p)
        {
            List<Pos> buffer = new List<Pos>();
            ushort x, z, y;
            ushort xmin = Math.Min((ushort)cpos.pos.x, (ushort)cpos.secondPos.x),
                xmax = Math.Max((ushort)cpos.pos.x, (ushort)cpos.secondPos.x),
                zmin = Math.Min((ushort)cpos.pos.z, (ushort)cpos.secondPos.z),
                zmax = Math.Max((ushort)cpos.pos.z, (ushort)cpos.secondPos.z),
                ymin = Math.Min((ushort)cpos.pos.y, (ushort)cpos.secondPos.y),
                ymax = Math.Max((ushort)cpos.pos.y, (ushort)cpos.secondPos.y);
            if (main.block != 255)
                cpos.block = main.block;
            switch (main.cuboidType)
            {
                case SolidType.solid:
                    for (x = xmin; x <= xmax; ++x)
                        for (z = zmin; z <= zmax; ++z)
                            for (y = ymin; y <= ymax; ++y)
                                if (p.Level.GetBlock(x, z, y) != cpos.block)
                                    BufferAdd(buffer, x, z, y);
                    break;
                case SolidType.hollow:
                    for (x = xmin; x <= xmax; ++x)
                        for (z = zmin; z <= zmax; ++z)
                            for (y = ymin; y <= ymax; ++y)
                                if (x == xmin || x == xmax || z == zmin || z == zmax || y == ymin || y == ymax)
                                    if (p.Level.GetBlock(x, z, y) != cpos.block)
                                        BufferAdd(buffer, x, z, y);
                    break;
                case SolidType.walls:
                    for (x = xmin; x <= xmax; ++x)
                        for (z = zmin; z <= zmax; ++z)
                            for (y = ymin; y <= ymax; ++y)
                                if (x == xmin || x == xmax || z == zmin || z == zmax)
                                    if (p.Level.GetBlock(x, z, y) != cpos.block)
                                        BufferAdd(buffer, x, z, y);
                    break;
                case SolidType.holes:
                    for (x = xmin; x <= xmax; ++x)
                        for (z = zmin; z <= zmax; ++z)
                            for (y = ymin; y <= ymax; ++y)
                                if ((x - xmin + z - zmin + y - ymin) % 2 == 0)
                                    if (p.Level.GetBlock(x, z, y) != cpos.block)
                                        BufferAdd(buffer, x, z, y);
                    break;
                case SolidType.wire:
                    for (x = xmin; x <= xmax; ++x)
                    {
                        BufferAdd(buffer, x, zmin, ymin);
                        BufferAdd(buffer, x, zmin, ymax);
                        BufferAdd(buffer, x, zmax, ymin);
                        BufferAdd(buffer, x, zmax, ymax);
                    }
                    for (z = zmin; z <= zmax; ++z)
                    {
                        BufferAdd(buffer, xmin, z, ymin);
                        BufferAdd(buffer, xmin, z, ymax);
                        BufferAdd(buffer, xmax, z, ymin);
                        BufferAdd(buffer, xmax, z, ymax);
                    }
                    for (y = ymin; y <= ymax; ++y)
                    {
                        BufferAdd(buffer, xmin, zmin, y);
                        BufferAdd(buffer, xmin, zmax, y);
                        BufferAdd(buffer, xmax, zmin, y);
                        BufferAdd(buffer, xmax, zmax, y);
                    }
                    break;
                case SolidType.random:
                    Random rand = new Random();
                    for (x = xmin; x <= xmax; ++x)
                        for (z = zmin; z <= zmax; ++z)
                            for (y = ymin; y <= ymax; ++y)
                                if (rand.Next(1, 11) <= 5 && p.Level.GetBlock(x, z, y) != cpos.block)
                                    BufferAdd(buffer, x, z, y);
                    break;
            }
            //Anti-tunneling permissions

            //Server force cuboid

            //Group Max Blocks permissions here
            //if(buffer.Count > p.group.maxBlocks)
            //{
            //p.SendMessage("You tried to cuboid + " buffer.Count + "blocks.");
            //p.SendMessage("You cannot cuboid more than " + p.group.maxBlocks + ".");
            //}

            //Silent pyramids == false

            //Level bufferblocks and level not instant

            //Level Blockqueue
            p.SendMessage(buffer.Count.ToString() + " blocks.");
            buffer.ForEach(delegate(Pos pos)
            {
                p.Level.BlockChange((ushort)(pos.pos.x), (ushort)(pos.pos.z), (ushort)(pos.pos.y), cpos.block);
            });
        }

        protected bool ValidSolidType(string solidTypeName)
        {
            switch (solidTypeName)
            {
                case "solid":
                    main.cuboidType = SolidType.solid;
                    break;
                case "hollow":
                    main.cuboidType = SolidType.hollow;
                    break;
                case "walls":
                    main.cuboidType = SolidType.walls;
                    break;
                case "holes":
                    main.cuboidType = SolidType.holes;
                    break;
                case "wire":
                    main.cuboidType = SolidType.wire;
                    break;
                case "random":
                    main.cuboidType = SolidType.random;
                    break;
                default:
                    return false;
            }
            return true;
        }

        protected bool ValidBlockName(string blockName)
        {
            if (Block.ValidBlockName(blockName))
            {
                main.block = Block.NameToBlock(blockName);
                return true;
            }
            return false;
        }

        protected void ParseCoordinates(Player p, string[] coordinates)
        {
            CatchPos cpos = new CatchPos();
            try
            {
                cpos.pos = new Vector3S(ushort.Parse(coordinates[0]), ushort.Parse(coordinates[1]), ushort.Parse(coordinates[2]));
                cpos.secondPos = new Vector3S(ushort.Parse(coordinates[3]), ushort.Parse(coordinates[4]), ushort.Parse(coordinates[5]));
            }
            catch
            {
                p.SendMessage("Invalid coordinates!");
                return;
            }
            Cuboid(cpos, p);
        }

        protected void BufferAdd(List<Pos> list, ushort x, ushort z, ushort y)
        {
            BufferAdd(list, new Vector3S(x, z, y));
        }

        protected void BufferAdd(List<Pos> list, Vector3S type)
        {
            Pos pos;
            pos.pos = type;
            list.Add(pos);
        }

        protected struct CatchPos
        {
            public SolidType cuboidType;
            public byte block;
            public Vector3S pos;
            public Vector3S secondPos;
        }

        protected struct Pos
        {
            public Vector3S pos;
        }

        protected enum SolidType { solid, hollow, walls, holes, wire, random };
    }
}