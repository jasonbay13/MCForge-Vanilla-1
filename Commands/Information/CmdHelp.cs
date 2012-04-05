/*
Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.osedu.org/licenses/ECL-2.0
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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.World;
using MCForge.Core;
using MCForge.Groups;

namespace CommandDll.Information {
	public class CmdHelp : ICommand {
		string _Name = "Help";
		public string Name { get { return _Name; } }

		CommandTypes _Type = CommandTypes.information;
		public CommandTypes Type { get { return _Type; } }

		string _Author = "Nerketur";
		public string Author { get { return _Author; } }

		int _Version = 1;
		public int Version { get { return _Version; } }

		string _CUD = "";
		public string CUD { get { return _CUD; } }

		string[] CommandStrings = new string[1] { "help" };

		byte perm = 0;
		public byte Permission {
			get { return perm; }
		}
		public void Use(Player p, string[] args) {
			if (args.Length == 0) {
				p.SendMessage("Use &b/help ranks" + Server.DefaultColor + " for a list of ranks.");
				p.SendMessage("Use &b/help build" + Server.DefaultColor + " for a list of building commands.");
				p.SendMessage("Use &b/help mod" + Server.DefaultColor + " for a list of moderation commands.");
				p.SendMessage("Use &b/help information" + Server.DefaultColor + " for a list of information commands.");
				p.SendMessage("Use &b/help other" + Server.DefaultColor + " for a list of other commands.");
				p.SendMessage("Use &b/help colors" + Server.DefaultColor + " to view the color codes.");
				p.SendMessage("Use &b/help [command] or /help [block] " + Server.DefaultColor + "to view more info.");
			} else if (args.Length == 1) {
				//Help about a particular command
				string cmdTypeName = "Unknown";
				CommandTypes cmdType = CommandTypes.misc;

				switch (args[0]) {
					case "build":
						cmdTypeName = "Building";
						cmdType = CommandTypes.building;
						break;
					case "mod":
						cmdTypeName = "Moderation";
						cmdType = CommandTypes.mod;
						break;
					case "information":
						cmdTypeName = "Informative";
						cmdType = CommandTypes.information;
						break;
					case "misc":
					case "other":
						cmdTypeName = "Miscellaneous";
						cmdType = CommandTypes.misc;
						break;
					case "colours":
					case "colors":
						p.SendMessage("&fTo use a color simply put a '%' sign symbol before you put the color code.");
						p.SendMessage("Colors Available:");
						p.SendMessage("0 - &0Black " + Server.DefaultColor + "| 8 - &8Gray");
						p.SendMessage("1 - &1Navy " + Server.DefaultColor + "| 9 - &9Blue");
						p.SendMessage("2 - &2Green " + Server.DefaultColor + "| a - &aLime");
						p.SendMessage("3 - &3Teal " + Server.DefaultColor + "| b - &bAqua");
						p.SendMessage("4 - &4Maroon " + Server.DefaultColor + "| c - &cRed");
						p.SendMessage("5 - &5Purple " + Server.DefaultColor + "| d - &dPink");
						p.SendMessage("6 - &6Gold " + Server.DefaultColor + "| e - &eYellow");
						p.SendMessage("7 - &7Silver " + Server.DefaultColor + "| f - &fWhite");
						return;
					case "ranks": 
						foreach (PlayerGroup grp in PlayerGroup.groups) {
							if (grp.name != "nobody") // Note that -1 means max undo.  Undo anything and everything.
								//p.SendMessage(grp.color + grp.name + " - &bCmd: " + grp..maxBlocks + " - &2Undo: " + ((grp.maxUndo != -1) ? grp.maxUndo.ToString() : "max") + " - &cPerm: " + (int)grp.Permission);
								p.SendMessage(grp.color + grp.name + " - &cPerm: " + (int)grp.permission);
						}
						return;
					default:
						try {
							ICommand cmd = Command.Find(args[0]);
							cmd.Help(p);
							//string foundRank = Level.PermissionToName(GrpCommands.allowedCommands.Find(grpComm => grpComm.commandName == cmd.name).lowestRank);
							//Player.SendMessage(p, "Rank needed: " + getColor(cmd.name) + foundRank);
							PlayerGroup cmdGroup = PlayerGroup.groups.Find(grp => grp.permission == cmd.Permission);
							string foundRank = cmdGroup.name;
							p.SendMessage("Rank needed: " + cmdGroup.color + foundRank);
							return;
						} catch (Exception) { }
						
						try {
							byte b = Blocks.NameToByte(args[0]);
							KeyValuePair<byte, Blocks.CustomBlock> customBlock = Blocks.CustomBlocks.ToList().Find(want => want.Key == b);
							string custom;
							if (customBlock.Value.Name == null) // only happens when there is no such block.
								custom = args[0];
							else
								custom = Blocks.ByteToName(customBlock.Value.VisibleType); // Custom block is this.
							p.SendMessage("Block \"" + args[0] + "\" appears as &b" + custom);
							//PlayerGroup foundRank = PlayerGroup.findPerm(Blocks.Types.BlockList.Find(bs => bs.type == b).lowestRank);
							//Player.SendMessage(p, "Rank needed: " + foundRank.color + foundRank.name);
							return;
						} catch (Exception) { }
						p.SendMessage("Could not find command or block specified");
						return;
				}
				p.SendMessage(cmdTypeName + " commands you may use:");
				//First get them all, just names, in a list.
				List<string> cmdList = new List<string>();
				foreach (KeyValuePair<string, ICommand> c in Command.all.ToList().FindAll(match => (match.Value.Permission <= p.group.permission) && (match.Value.Type == cmdType))) {
					cmdList.Add(c.Key);
				}
				StringBuilder sb = new StringBuilder();
				int count = 0;
				if (cmdList.Count == 0) {
					p.SendMessage("You cannot use any " + cmdTypeName + " commands");
					return;
				}
				foreach (string c in cmdList) {
					sb.Append(", ").Append(PlayerGroup.groups.Find(grp => grp.permission == Command.Find(c).Permission).color).Append(c);
					count = (count + 1) % 5; // 5 commands per line.
					if (count == 0) {
						p.SendMessage(sb.Remove(0, 2).ToString());
						sb.Clear();
					}
				}
			}
		}

		public void Help(Player p) {
			p.SendMessage("...really? Wow. Just...wow.");
		}

		public void Initialize() {
			Command.AddReference(this, CommandStrings);
		}
	}
}

/*
    public class CmdHelp : Command
    {

        private string getColor(string commName)
        {
            foreach (GrpCommands.rankAllowance aV in GrpCommands.allowedCommands)
            {
                if (aV.commandName == commName)
                {
                    if (Group.findPerm(aV.lowestRank) != null)
                        return Group.findPerm(aV.lowestRank).color;
                }
            }

            return "&f";
        }

    }
}
*/