using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge;

namespace CommandDll
{
    public class CmdBan : ICommand
    {
        string _Name = "ban";
        public string Name { get { return _Name; } }

        CommandTypes _Type = CommandTypes.mod;
        public CommandTypes Type { get { return _Type; } }

        string _Author = "cazzar";
        public string Author { get { return _Author; } }

        int _Version = 1;
        public int Version { get { return _Version; } }

        string _CUD = "";
        public string CUD { get { return _CUD; } }

        string[] CommandStrings = new string[1] { "ban" };

        public void Use(Player p, string[] args)
        {
            if (args.Length >= 1)
            {
                if (args.Length == 1)
                    args[2] = "Banned!";

                Player who = Player.Find(args[0]);
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/ban <player> [reason] - Bans the player by name only");
            p.SendMessage("/ban @<player> [reason] - Bans the player by name, IP and kicks them");
            p.SendMessage("/ban #<player> [reason] - Stealth bans the player by name, IP and kicks them");
        }

        public void Initialize()
        {
            Command.AddReference(this, CommandStrings);
        }
    }
}
