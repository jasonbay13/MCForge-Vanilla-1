using MCForge.API.Events;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
namespace CommandDll {
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

        public void Use(Player p, string[] args) {
            p.SendMessage("Move event activated!");
            p.OnPlayerMove.Normal += new MoveEvent.EventHandler(CallBack);
            //OnPlayerChat pe = OnPlayerChat.Register(CallBack, p);
            //OnPlayerChat pe2 = OnPlayerChat.Register(CallBack2, p);
            //pe.Cancel();
            //p.SendMessage("Please place/destroy a block.");
            //p.CatchNextBlockchange(new Player.BlockChangeDelegate(BlockChange), null);
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
