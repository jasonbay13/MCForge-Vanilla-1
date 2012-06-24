using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.World;
using MCForge.Utils;

namespace MCForge.Commands.Building {
    public class CmdBall :ICommand {
        public string Name {
            get {  return "Ball"; }
        }

        public CommandTypes Type {
            get { return CommandTypes.Building; }
        }

        public string Author {
            get { return "ninedrafted"; }
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
            int rx = 10;
            if (args.Length > 0) {
                try {
                    rx = Int32.Parse(args[0]);
                }
                catch { }
            }
            p.ExtraData["BallRadius"] = rx;
            p.OnPlayerBlockChange.Normal += new API.Events.Event<Player, API.Events.BlockChangeEventArgs>.EventHandler(OnPlayerBlockChange_Normal);
            p.SendMessage("Define center");
        }

        void OnPlayerBlockChange_Normal(Player sender, API.Events.BlockChangeEventArgs args) {
            sender.OnPlayerBlockChange.Normal -= OnPlayerBlockChange_Normal;
            int rx = (int)sender.ExtraData["BallRadius"];
            int count = 0;
            foreach (Vector3S v in (new Vector3S(args.X,args.Z,args.Y)).GetNearBlocks(rx, rx, rx)) {
                if (v.x > 0 && v.z > 0 && v.y > 0 && v.x < sender.Level.Size.x && v.z < sender.Level.Size.z && v.y < sender.Level.Size.y) {
                    sender.Level.BlockChange(v, 1, sender);
                    count++;
                }
            }
            sender.SendMessage(count + " Blocks");
        }

        public void Help(Player p) {
           
        }

        public void Initialize() {
            Command.AddReference(this, "Ball");
        }
    }
}
