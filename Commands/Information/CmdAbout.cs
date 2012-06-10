using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.API.Events;
using MCForge.SQL;
using MCForge.Utils;
using MCForge.Core;
using MCForge.World;
namespace MCForge.Commands.Information {
    public class CmdAbout : ICommand {
        #region ICommand Members

        public string Name {
            get { return "About"; }
        }

        public CommandTypes Type {
            get { return CommandTypes.Information; }
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

        public void Use(MCForge.Entity.Player p, string[] args) {
            p.SendMessage("Break block to get info");
            p.OnPlayerBlockChange.Normal += OnBlockChange;
        }

        public void Help(MCForge.Entity.Player p) {
           
        }

        public void Initialize() {
            Command.AddReference(this, "b", "about");
        }

        #endregion

        void OnBlockChange(Player sender, BlockChangeEventArgs e) {
            sender.OnPlayerBlockChange.Normal -= OnBlockChange;
            e.Cancel();
            using (var data = Database.fillData("SELECT * FROM Blocks WHERE X = '" + e.X + "' AND Y = '" + e.Y + "' AND Z = '" + e.Z + "' AND Level = '" + sender.Level.Name.MySqlEscape() + "';")) {

                if (data.Rows.Count == 0) {
                    sender.SendMessage("This block has not been modified since the map was cleared or created.");
                    return;
                }

                for (int i = 0; i < data.Rows.Count; i++) {
                    string username;
                    string color;
                    string block;
                    string time;
                    bool deleted;

                    using (var playerData = Database.fillData("SELECT * FROM _players WHERE UID = " + data.Rows[i]["UID"].ToString())) {
                        username = playerData.Rows[0]["Name"].ToString();
                        color = playerData.Rows[0]["color"].ToString();
                    }

                    block = ((Block)byte.Parse(data.Rows[i]["Block"].ToString())).Name;
                    time = DateTime.Parse(data.Rows[i]["Date"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    deleted = data.Rows[i]["Deleted"].ToString().ToLower() == "true";
                    sender.SendMessage((deleted ? "&4Destroyed by " : "&3Created by ") + Server.DefaultColor + color + username + Server.DefaultColor + ", using &3" + block + Server.DefaultColor + " At " + time);
                }
            }
            if (sender.StaticCommandsEnabled) {
                sender.SendMessage("Break block to get info");
                sender.OnPlayerBlockChange.Normal += OnBlockChange;
            }
        }
    }
}
