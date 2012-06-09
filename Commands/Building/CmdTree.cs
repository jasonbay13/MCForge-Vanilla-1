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
using MCForge.Groups;
using MCForge.World;
using MCForge.API.Events;

namespace CommandDll
{
    public class CmdTree : ICommand
    {
        public string Name { get { return "Tree"; } }
        public CommandTypes Type { get { return CommandTypes.Building; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return "com.mcforge.tree"; } }
        public byte Permission { get { return (byte)PermissionLevel.Builder; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) {
                p.SendMessage("Place a block where you would like your tree to grow!");
                p.ExtraData.ChangeOrCreate("TreeType", TreeType.Classic); 
                p.OnPlayerBlockChange.Normal += BlockChange; 
            }
            else {
                switch (args[0].ToLower()) { 
                    case "classic":
                    case "default":
                        p.ExtraData.ChangeOrCreate("TreeType", TreeType.Classic);
                        break;
                    case "swamp":
                        p.ExtraData.ChangeOrCreate("TreeType", TreeType.Swamp);
                        break;
                    case "cactus":
                    case "cacti":
                        p.SendMessage("Not implemented!"); return;
                        //break;
                    case "notch":
                        p.ExtraData.ChangeOrCreate("TreeType", TreeType.Notch);
                        break;
                    case "bush":
                        p.ExtraData.ChangeOrCreate("TreeType", TreeType.Bush);
                        break;
                    default:
                        p.SendMessage("Invalid tree type!"); 
                        return;
                }
                p.SendMessage("Place a block where you would like your tree to grow!");
                p.OnPlayerBlockChange.Normal += BlockChange; 
            }
            
        }
        public void BlockChange(Player p, BlockChangeEventArgs args) {
            p.OnPlayerBlockChange.Normal -= BlockChange;
            WorldComponent.GenerateTree(p, args.X, args.Z, args.Y, (TreeType)p.ExtraData["TreeType"], false);
            p.ExtraData.Remove("TreeType");
        }
        public void Help(Player p)
        {
            p.SendMessage("/tree [type] - generates a tree");
            p.SendMessage("Valid types: classic, cactus, notch, swamp");
        }

        public void Initialize()
        {
            Command.AddReference(this, "tree");
        }
    }
}
