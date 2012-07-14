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
using MCForge.Utils;
using System.Linq;

namespace MCForge.Commands
{
    public class CmdReplaceAll : ICommand
    {
        public string Name { get { return "ReplaceAll"; } }
        public CommandTypes Type { get { return CommandTypes.Building; } }
        public string Author { get { return "Gamemakergm, 7imekeeper"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 100; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length != 2)
            {
                p.SendMessage("Invalid number of arguments!");
                Help(p);
                return;
            }

            List<string> temp;

            if (args[0].Contains(","))
                temp = new List<string>(args[0].Split(','));
            else
                temp = new List<string>() { args[0] };

            temp = temp.Distinct().ToList(); // Remove duplicates

            List<string> invalid = new List<string>(); //Check for invalid blocks
            foreach (string name in temp)
                if (!Block.ValidBlockName(name))
                    invalid.Add(name);
            if (!Block.ValidBlockName(args[1]))
                invalid.Add(args[1]);
            if (invalid.Count > 0)
            {
                p.SendMessage(String.Format("Invalid block{0}: {1}", invalid.Count == 1 ? "" : "s", String.Join(", ", invalid)));
                return;
            }

            if (temp.Contains(args[1]))
                temp.Remove(args[1]);
            if (temp.Count < 1)
            {
                p.SendMessage("Replacing a block with the same one would be pointless!");
                return;
            }

            List<byte> oldType = new List<byte>();
            foreach (string name in temp)
                oldType.Add(Block.NameToBlock(name));
            byte newType = Block.NameToBlock(args[1]);

            List<Vector3S> buffer = new List<Vector3S>();

            int currentBlock = 0;
            foreach (byte b in p.Level.Data)
            {
                if (oldType.Contains(b))
                    buffer.Add(p.Level.IntToPos(currentBlock));
                currentBlock++;
            }
            
            p.SendMessage(buffer.Count.ToString() + " blocks.");
            buffer.ForEach(delegate(Vector3S pos)
            {
                p.Level.BlockChange((ushort)(pos.x), (ushort)(pos.z), (ushort)(pos.y), newType, p);
            });
            p.SendMessage("&4/replaceall finished!");
        }

        public void Help(Player p)
        {
            p.SendMessage("/replaceall <block,block2,...> <new> - Replaces all of <block> with <new> in the map.");
            p.SendMessage("If multiple<block>s are specified they will all be replaced with <new>.");
            p.SendMessage("Shortcut: /ra");
        }

        public void Initialize()
        {
            string[] CommandStrings = new string[2] { "replaceall", "ra" };
            Command.AddReference(this, CommandStrings);
        }
    }
}