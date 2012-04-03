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

		public void Use(Player p, string[] args) {
			if (args.Length == 0) {
				p.SendMessage("Use &b/help ranks" + Server.DefaultColor + " for a list of ranks.");
				p.SendMessage("Use &b/help build" + Server.DefaultColor + " for a list of building commands.");
				p.SendMessage("Use &b/help mod" + Server.DefaultColor + " for a list of moderation commands.");
				p.SendMessage("Use &b/help information" + Server.DefaultColor + " for a list of information commands.");
				//p.SendMessage("Use &b/help games" + Server.DefaultColor + " for a list of game commands.");
				p.SendMessage("Use &b/help other" + Server.DefaultColor + " for a list of other commands.");
				p.SendMessage("Use &b/help colors" + Server.DefaultColor + " to view the color codes.");
				//p.SendMessage("Use &b/help short" + Server.DefaultColor + " for a list of shortcuts.");
				//p.SendMessage("Use &b/help old" + Server.DefaultColor + " to view the Old help menu.");
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
					case "info":
						cmdTypeName = "Informative";
						cmdType = CommandTypes.information;
						break;
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
					default:
						try {
							p.SendMessage("Trying to find command...");
							ICommand cmd = Command.Find(args[0]);
							if (cmd != null) {
								cmd.Help(p);
								p.SendMessage("Displayed help for /" + args[0]);
							} else if (Blocks.NameToByte(args[0]) != (byte)Blocks.Types.zero) {
								//TODO: Find Block
								p.SendMessage("Find block");
							} else {
								p.SendMessage("Could not find command or block specified");
							}
						} catch (Exception) {
							p.SendMessage("Could not find command or block specified");
						}
						return;
				}
				p.SendMessage(cmdTypeName + " commands you may use:");
				StringBuilder sb = new StringBuilder();
				int count = 0;
				foreach (KeyValuePair<string, ICommand> c in Command.all) {
					if (c.Value.Type == cmdType) {
						//TODO: Check rank
						sb.Append(", ").Append(c.Value.Name.ToLower());
						count = (count + 1) % 5; // 5 commands per line.
						if (count == 0) {
							p.SendMessage(sb.Remove(0, 2).ToString());
							sb = new StringBuilder();
						}
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
        public override string name { get { return "help"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdHelp() { }

        public override void Use(Player p, string message)
        {
            try
            {
                message.ToLower();
                switch (message)
                {
                    case "":
                        if (Server.oldHelp)
                        {
                            goto case "old";
                        }
                        else
                        {
                            Player.SendMessage(p, "Use &b/help ranks" + Server.DefaultColor + " for a list of ranks.");
                            Player.SendMessage(p, "Use &b/help build" + Server.DefaultColor + " for a list of building commands.");
                            Player.SendMessage(p, "Use &b/help mod" + Server.DefaultColor + " for a list of moderation commands.");
                            Player.SendMessage(p, "Use &b/help information" + Server.DefaultColor + " for a list of information commands.");
                            Player.SendMessage(p, "Use &b/help games" + Server.DefaultColor + " for a list of game commands.");
                            Player.SendMessage(p, "Use &b/help other" + Server.DefaultColor + " for a list of other commands.");
                            Player.SendMessage(p, "Use &b/help colors" + Server.DefaultColor + " to view the color codes.");
                            Player.SendMessage(p, "Use &b/help short" + Server.DefaultColor + " for a list of shortcuts.");
                            Player.SendMessage(p, "Use &b/help old" + Server.DefaultColor + " to view the Old help menu.");
                            Player.SendMessage(p, "Use &b/help [command] or /help [block] " + Server.DefaultColor + "to view more info.");
                        } break;
                    case "ranks":
                        message = "";
                        foreach (Group grp in Group.GroupList)
                        {
                            if (grp.name != "nobody") // Note that -1 means max undo.  Undo anything and everything.
                                Player.SendMessage(p, grp.color + grp.name + " - &bCmd: " + grp.maxBlocks + " - &2Undo: " + ((grp.maxUndo != -1) ? grp.maxUndo.ToString() : "max") + " - &cPerm: " + (int)grp.Permission);
                        }
                        break;
                    case "build":
                        message = "";
                        foreach (Command comm in Command.all.commands)
                        {
                            if (p == null || p.group.commands.All().Contains(comm))
                            {
                                if (comm.type.Contains("build")) message += ", " + getColor(comm.name) + comm.name;
                            }
                        }

                        if (message == "") { Player.SendMessage(p, "No commands of this type are available to you."); break; }
                        Player.SendMessage(p, "Building commands you may use:");
                        Player.SendMessage(p, message.Remove(0, 2) + ".");
                        break;
                    case "mod": case "moderation":
                        message = "";
                        foreach (Command comm in Command.all.commands)
                        {
                            if (p == null || p.group.commands.All().Contains(comm))
                            {
                                if (comm.type.Contains("mod")) message += ", " + getColor(comm.name) + comm.name;
                            }
                        }

                        if (message == "") { Player.SendMessage(p, "No commands of this type are available to you."); break; }
                        Player.SendMessage(p, "Moderation commands you may use:");
                        Player.SendMessage(p, message.Remove(0, 2) + ".");
                        break;
                    case "information":
                        message = "";
                        foreach (Command comm in Command.all.commands)
                        {
                            if (p == null || p.group.commands.All().Contains(comm))
                            {
                                if (comm.type.Contains("info")) message += ", " + getColor(comm.name) + comm.name;
                            }
                        }

                        if (message == "") { Player.SendMessage(p, "No commands of this type are available to you."); break; }
                        Player.SendMessage(p, "Information commands you may use:");
                        Player.SendMessage(p, message.Remove(0, 2) + ".");
                        break;
                    case "games": case "game":
                        message = "";
                        foreach (Command comm in Command.all.commands)
                        {
                            if (p == null || p.group.commands.All().Contains(comm))
                            {
                                if (comm.type.Contains("game")) message += ", " + getColor(comm.name) + comm.name;
                            }
                        }

                        if (message == "") { Player.SendMessage(p, "No commands of this type are available to you."); break; }
                        Player.SendMessage(p, "Game commands you may use:");
                        Player.SendMessage(p, message.Remove(0, 2) + ".");
                        break;
                    case "other":
                        message = "";
                        foreach (Command comm in Command.all.commands)
                        {
                            if (p == null || p.group.commands.All().Contains(comm))
                            {
                                if (comm.type.Contains("other")) message += ", " + getColor(comm.name) + comm.name;
                            }
                        }

                        if (message == "") { Player.SendMessage(p, "No commands of this type are available to you."); break; }
                        Player.SendMessage(p, "Other commands you may use:");
                        Player.SendMessage(p, message.Remove(0, 2) + ".");
                        break;
                    case "short":
                        message = "";
                        foreach (Command comm in Command.all.commands)
                        {
                            if (p == null || p.group.commands.All().Contains(comm))
                            {
                                if (comm.shortcut != "") message += ", &b" + comm.shortcut + " " + Server.DefaultColor + "[" + comm.name + "]";
                            }
                        }
                        Player.SendMessage(p, "Available shortcuts:");
                        Player.SendMessage(p, message.Remove(0, 2) + ".");
                        break;
                    case "colours":
                    case "colors":
                            Player.SendMessage(p, "&fTo use a color simply put a '%' sign symbol before you put the color code.");
                            Player.SendMessage(p, "Colors Available:");
                            Player.SendMessage(p, "0 - &0Black " + Server.DefaultColor + "| 8 - &8Gray");
                            Player.SendMessage(p, "1 - &1Navy " + Server.DefaultColor + "| 9 - &9Blue");
                            Player.SendMessage(p, "2 - &2Green " + Server.DefaultColor + "| a - &aLime");
                            Player.SendMessage(p, "3 - &3Teal " + Server.DefaultColor + "| b - &bAqua");
                            Player.SendMessage(p, "4 - &4Maroon " + Server.DefaultColor + "| c - &cRed");
                            Player.SendMessage(p, "5 - &5Purple " + Server.DefaultColor + "| d - &dPink");
                            Player.SendMessage(p, "6 - &6Gold " + Server.DefaultColor + "| e - &eYellow");
                            Player.SendMessage(p, "7 - &7Silver " + Server.DefaultColor + "| f - &fWhite");
                            break;
                    case "old":
                        string commandsFound = "";
                        foreach (Command comm in Command.all.commands)
                        {
                            if (p == null || p.group.commands.All().Contains(comm))
                            {
                                try { commandsFound += ", " + comm.name; } catch { }
                            }
                        }
                        Player.SendMessage(p, "Available commands:");
                        Player.SendMessage(p, commandsFound.Remove(0, 2));
                        Player.SendMessage(p, "Type \"/help <command>\" for more help.");
                        Player.SendMessage(p, "Type \"/help shortcuts\" for shortcuts.");
                        break;
                    default:
                        Command cmd = Command.all.Find(message);
                        if (cmd != null)
                        {
                            cmd.Help(p);
                            string foundRank = Level.PermissionToName(GrpCommands.allowedCommands.Find(grpComm => grpComm.commandName == cmd.name).lowestRank);
                            Player.SendMessage(p, "Rank needed: " + getColor(cmd.name) + foundRank);
                            return;
                        }
                        byte b = Block.Byte(message);
                        if (b != Block.Zero)
                        {
                            Player.SendMessage(p, "Block \"" + message + "\" appears as &b" + Block.Name(Block.Convert(b)));
                            Group foundRank = Group.findPerm(Block.BlockList.Find(bs => bs.type == b).lowestRank);
                            Player.SendMessage(p, "Rank needed: " + foundRank.color + foundRank.name);
                            return;
                        }
                        Plugin plugin = null;
                        foreach (Plugin p1 in Plugin.all)
                        {
                            if (p1.name.ToLower() == message.ToLower())
                            {
                                plugin = p1;
                                break;
                            }
                        }
                        if (plugin != null)
                        {
                            plugin.Help(p);
                        }
                        Player.SendMessage(p, "Could not find command, plugin or block specified.");
                        break;
                }
                
            }
            catch (Exception e) { Server.ErrorLog(e); Player.SendMessage(p, "An error occured"); }
        }

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

        public override void Help(Player p)
        {
            Player.SendMessage(p, "...really? Wow. Just...wow.");
        }
    }
}
*/