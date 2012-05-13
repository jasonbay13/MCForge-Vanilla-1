using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge;
using MCForge.Interface.Command;
using MCForge.Core;
using MCForge.Entity;
using MCForge.API;
using MCForge.API.Events;

namespace CommandDll {
    public class CmdDataPassExample : ICommand {
        string _Name = "DataPassExample";
        public string Name { get { return _Name; } }

        CommandTypes _Type = CommandTypes.Misc;
        public CommandTypes Type { get { return _Type; } }

        string _Author = "Merlin33069";
        public string Author { get { return _Author; } }

        int _Version = 1;
        public int Version { get { return _Version; } }

        string _CUD = "";
        public string CUD { get { return _CUD; } }

        byte _Permission = 120;
        public byte Permission { get { return _Permission; } }


        string[] CommandStrings = new string[1] { "example" };

        public void Use(Player p, string[] args) {
            p.SendMessage("Please place a block...");
            p.OnPlayerBlockChange.Normal += new BlockChangeEvent.EventHandler(CatchBlock);
        }
        public void CatchBlock(Player sender, BlockChangeEventArgs args) {
            args.Unregister();
            Vector3 FirstBlock = new Vector3(args.X, args.Z, args.Y);
            sender.SendMessage("Please place another block...");
            sender.SetDatapass("CmdDatapassExample_FirstBlock", FirstBlock);
            sender.OnPlayerBlockChange.Normal += new BlockChangeEvent.EventHandler(CatchBlock2);
        }
        public void CatchBlock2(Player sender, BlockChangeEventArgs args) {
            args.Unregister();
            Vector3 FirstBlock = (Vector3)sender.GetDatapass("CmdDatapassExample_FirstBlock");
            Vector3 SecondBlock = new Vector3(args.X, args.Z, args.Y);
            sender.SendMessage("This is where we would initiate a Cuboid!");
        }

        public void Help(Player p) {

        }

        public void Initialize() {
            Command.AddReference(this, CommandStrings);
        }
    }
}
