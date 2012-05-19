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
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;
using MCForge.Utils.Settings;
using MCForge.World;
using MCForge.World.Blocks;

namespace CommandDll {
    public class CmdMode : ICommand {
        public string Name { get { return "Mode"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args) {
            if (args.Length == 0) { Help(p); return; }
            if (!p.ExtraData.ContainsKey("Mode")) {
                p.ExtraData.Add("Mode", false);
            }

            if (!(bool)p.ExtraData["Mode"]) {


                Block b = Block.NameToBlock(args[0]);
                if (b is UNKNOWN) {
                    p.SendMessage("Cannot find block\"" + args[0] + "\"!");
                    return;
                }
                if (b == Block.BlockList.AIR) {
                    p.SendMessage("Cannot use air mode!");
                    return;
                }
                if (b.Permission > p.Group.Permission) {
                    p.SendMessage("Cannot place " + StringUtils.TitleCase(b.Name) + "!");
                    return;
                }

                p.ExtraData["Mode"] = true;
                if (!p.ExtraData.ContainsKey("BlockMode"))
                    p.ExtraData.Add("BlockMode", b);

                p.SendMessage("&b" + StringUtils.TitleCase(b.Name) + Server.DefaultColor + " mode &9on");
                return;
            }
            else {
                if (args[0] != ((Block)p.ExtraData["BlockMode"]).Name) {
                    Block b = Block.NameToBlock(args[0]);
                    if (b is UNKNOWN) { 
                        p.SendMessage("Cannot find block\"" + args[0] + "\"!");
                        return; 
                    }
                    if (b == Block.BlockList.AIR) {
                        p.SendMessage("Cannot use air mode!");
                        return; 
                    }
                    if (b.Permission > p.Group.Permission) { 
                        p.SendMessage("Cannot place " + StringUtils.TitleCase(b.Name) + "!");
                        return; 
                    }

                    p.ExtraData["Mode"] = true;
                    if (!p.ExtraData.ContainsKey("BlockMode"))
                        p.ExtraData.Add("BlockMode", b);
                    p.SendMessage("&b" + StringUtils.TitleCase(b.Name) + Server.DefaultColor + " mode &9on");
                    return;
                }
                if (!p.ExtraData.ContainsKey("BlockMode"))
                    throw new Exception("No block set in block mode");

                Block prev = (Block)p.ExtraData["BlockMode"];
                p.SendMessage("&b" + StringUtils.TitleCase(prev.Name) + Server.DefaultColor + " mode &coff");
                p.ExtraData["Mode"] = false;
                p.ExtraData["BlockMode"] = null;
            }
        }

        public void Help(Player p) {
            p.SendMessage("/mode <block> - makes every placed block turn into the block specified");
            p.SendMessage("/<block> will also work");
        }

        public void Initialize() {
            Command.AddReference(this, "mode");
        }
    }
}

