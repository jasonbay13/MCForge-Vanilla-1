using MCForge.Interface.Command;
using MCForge.Entity;

namespace CommandDll.Misc {
    public class CmdNickname : ICommand {
        public string Name { get { return "Nickname"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "headdetect, ninedrafted"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission {
            get {
#if DEBUG
                return 0;
#else
                return 30;
#endif
            }
        }

        public void Use(Player p, string[] args) {
            if (args.Length == 0 && p.Username != p.DisplayName) p.DisplayName = p.Username;
            string nick = string.Join(" ", args);
            p.DisplayName = nick;
        }

        public void Help(Player p) {
            p.SendMessage("/nickname [name] to set your nickname");
            p.SendMessage("/nickname to remove your nickname");
        }

        public void Initialize() {
            Command.AddReference(this, new string[] { "nickname", "nick" });
        }
    }
}