using MCForge.API.Events;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;
namespace MCForge.Commands {
    public class CmdTest : ICommand {
        string _Name = "test";
        public string Name { get { return _Name; } }

        CommandTypes _Type = CommandTypes.Misc;
        public CommandTypes Type { get { return _Type; } }

        string _Author = "Merlin33069";
        public string Author { get { return _Author; } }

        int _Version = 1;
        public int Version { get { return _Version; } }

        string _CUD = "";
        public string CUD { get { return _CUD; } }

        byte _Permission = 0;
        public byte Permission { get { return _Permission; } }

        string[] CommandStrings = new string[1] { "test" };

        public void Use(Player p,string[] args) {
            p.OnPlayerBlockChange.Normal += new Event<Player, BlockChangeEventArgs>.EventHandler(OnPlayerBlockChange_Normal);
            p.SendMessage("Click twice!");
            //p.OnPlayerMove.Normal += new MoveEvent.EventHandler(CallBack);
            //OnPlayerChat pe = OnPlayerChat.Register(CallBack, p);
            //OnPlayerChat pe2 = OnPlayerChat.Register(CallBack2, p);
            //pe.Cancel();
            //p.SendMessage("Please place/destroy a block.");
            //p.CatchNextBlockchange(new Player.BlockChangeDelegate(BlockChange), null);
        }

        void OnPlayerBlockChange_Normal(Player sender, BlockChangeEventArgs args) {
            sender.OnPlayerBlockChange.Normal -= OnPlayerBlockChange_Normal;
            sender.ExtraData["Datapass"] = new Vector3S(args.X, args.Z, args.Y);
            sender.OnPlayerBlockChange.Normal += new Event<Player, BlockChangeEventArgs>.EventHandler(OnPlayerBlockChange_Normal2);

        }

        void OnPlayerBlockChange_Normal2(Player sender, BlockChangeEventArgs args) {
            sender.OnPlayerBlockChange.Normal -= OnPlayerBlockChange_Normal2;
            Vector3S v = (Vector3S)sender.ExtraData["Datapass"];
            Vector3S start = new Vector3S((v.x < args.X) ? (ushort)v.x : args.X, (v.z < args.Z) ? (ushort)v.z : args.Z, (v.y < args.Y) ? (ushort)v.y : args.Y);
            Vector3S end = new Vector3S((v.x > args.X) ? (ushort)v.x : args.X, (v.z > args.Z) ? (ushort)v.z : args.Z, (v.y > args.Y) ? (ushort)v.y : args.Y);
            for (int x = start.x; x <= end.x; x++) {
                for (int z = start.z; z <= end.z; z++) {
                    for (int y = start.y; y <= end.y; y++) {
                        MCForge.Interfaces.Blocks.Block.SetBlock(MCForge.Interfaces.Blocks.Block.GetBlock("BlockDoor"), new Vector3S((ushort)x, (ushort)z, (ushort)y), sender.Level);
                    }
                }
            }
        }

        public void CallBack(Player sender, MoveEventArgs args) {
            MCForge.Utils.Logger.Log("Test: " + sender.Username + " moved!");
            sender.SendMessage("Hi!");
            sender.OnPlayerChat.All -= new ChatEvent.EventHandler(CallBack2);
        }
        public void CallBack2(Player sender, ChatEventArgs args) {
            //Logger.Log("Test: " + e.target.Username + " disconnected!");
            args.Message += "  Yeah, and Pikachu ROCKS!";
            sender.OnPlayerChat.All -= new ChatEvent.EventHandler(CallBack2);
        }

        public void Help(Player p) {

        }

        public void Initialize() {
            Command.AddReference(this, CommandStrings);
        }
    }
}
