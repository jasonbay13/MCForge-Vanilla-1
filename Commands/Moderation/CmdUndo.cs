/*
Copyright 2012 MCForge
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
#define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MCForge.SQL;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Groups;
using MCForge.Core;
using MCForge.World;
using System.Diagnostics;
using System.Collections.Specialized;
using MCForge.Utils;

namespace MCForge.Commands.Moderation {
    public class CmdUndo : ICommand {
        #region ICommand Members

        public string Name {
            get { return "Undo"; }
        }

        public CommandTypes Type {
            get { return CommandTypes.Mod; }
        }

        public string Author {
            get { return "MCForge Devs"; }
        }

        public int Version {
            get { return 1; }
        }

        public string CUD {
            get { return ""; }
        }

        public byte Permission {
            get { return 0; }
        }

        public void Use(Player p, string[] args) {

            int _time = 30;
            Player who = p;
            long UID = -1;
#if DEBUG
            if (args.Length == 1 && args[0] == "write") {
                p.history.WriteOut();
                p.SendMessage("BlockChangeHistroy written to filesystem");
                return;
            }
            if (args.Length == 1 && args[0] == "about") {
                p.OnPlayerBlockChange.Normal += OnPlayerBlockChange_Normal;
                p.SendMessage("Click!");
                return;
            }
#endif
            //undo <seconds>
            if (args.Length == 1) {
                try {
                    _time = int.Parse(args[0]);
                    if (_time < 1) {
                        p.SendMessage("The time must be greater than 1");
                        return;
                    }
                }
                catch {
                    p.SendMessage("That is not a vaild number");
                    return;
                }
            }


            else if (args.Length == 2) {
                who = Player.Find(args[0]);

                if (who == null) {
                	//Try getting offline player
                	DataTable playerDb = Database.fillData("SELECT * FROM _players WHERE Name='" + args[0] + "'");
                	if (playerDb.Rows.Count == 0) {
                		p.SendMessage("Player doesn't exist");
                		return;
                	}
                	UID = long.Parse(playerDb.Rows[0]["UID"].ToString());
                }

                if (args[1].ToLower() == "all") {

                    _time = int.MaxValue;
                }
                else {
                    try {
                        _time = int.Parse(args[1]);
                        if (_time < 1) {
                            p.SendMessage("The time must be greater than 1");
                            return;
                        }
                    }
                    catch {
                        p.SendMessage("That is not a valid number");
                    }
                }
            }

            if (p.Group.Permission < (byte)PermissionLevel.Operator) {
                if (who != p) {
                    p.SendMessage("You cannot undo other peoples stuff");
                    return;
                }
            }
            if (UID != -1)
            {
            	Undo(UID, _time, p.Level, who);
            	Player.UniversalChat(Server.DefaultColor + "Undid " + args[0] + " for &c" + _time + Server.DefaultColor + " seconds");
            }
            else
            {
            	Undo(who, _time);
            	Player.UniversalChat(Server.DefaultColor + "Undid " + who.Color + who.Username + Server.DefaultColor + " for &c" + _time + Server.DefaultColor + " seconds");
            }
        }

        void OnPlayerBlockChange_Normal(Player sender, API.Events.BlockChangeEventArgs args) {
            sender.OnPlayerBlockChange.Normal -= OnPlayerBlockChange_Normal;
            string[] about=sender.history.About(new Vector3S(args.X, args.Z, args.Y), sender.Level);
            sender.SendMessage("ABOUT:");
            for (int i = 0; i < about.Length; i++) {
                sender.SendMessage(about[i]);
            }
        }


        void Undo(long UID, int time, Level l, Player online)
        {
            if (online != null) {
                online.history.Undo(DateTime.Now.AddSeconds(-time).Ticks, l);
            }
            return;
        	if (UID == -1)
        		return;
            string datetime = DateTime.Now.AddSeconds(time * -1).ToString("yyyy-MM-dd HH:mm:ss.fff");
           // DataTable blockchanges = Database.fillData("SElECT * FROM Blocks WHERE UID=" + UID + " AND Date > '" + datetime + "' ORDER BY Date");
            	int x = 0;
            	int y = 0;
            	int z = 0;
            	byte was = 0;
                Stopwatch s = new Stopwatch();
                s.Start();
                int i = 0;
            	foreach(NameValueCollection nvm in Database.getData("SElECT X, Y, Z, Was, Date FROM Blocks WHERE UID=" + UID + " AND Date > '" + datetime + "' ORDER BY Date DESC")){
                    i++;
                    x = int.Parse(nvm["X"].ToString());
            		y = int.Parse(nvm["Y"].ToString());
            		z = int.Parse(nvm["Z"].ToString());
            		was = byte.Parse(nvm["Was"].ToString());
                    l.BlockChange((ushort)x, (ushort)z, (ushort)y, was);
            	}
                Database.executeQuery("DELETE FROM Blocks WHERE DATE > '" + datetime + "'");
                s.Stop();
                Logger.Log("used " + s.Elapsed + " for " + i + " undos");
           // blockchanges.Dispose();
        }
        void Undo(Player p, int time = 30) {
        	Undo(p.UID, time, p.Level, p);
        }

        public void Help(Player p) {
            p.SendMessage(Server.DefaultColor + "/undo <player> <seconds> - Undoes the block changes for <player> in the past <seconds>");
            p.SendMessage(Server.DefaultColor + "/undo <player> all - Undoes as much as it can.");
            p.SendMessage(Server.DefaultColor + "/undo <seconds> - Unodes the block changes for yourself in the past <seconds>");
            p.SendMessage(Server.DefaultColor + "/undo - Undoes the block changes for yourself in the past&c 30 " + Server.DefaultColor + " seconds");
        }

        public void Initialize() {
            Command.AddReference(this, "undo");
        }

        #endregion
    }
}
