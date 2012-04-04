using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Groups;

namespace CommandDll
{
    public class CmdSetRank : ICommand
    {
        public string Name { get { return "SetRank"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "cazzar"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }
        string[] CommandStrings = new string[2] { "setrank", "rank" };

        public void Use(Player p, string[] args)
        {
            if (args.Length != 2)
                Help(p);

            Player who = null;
            who = Player.Find(args[0]);
            if (who == null)
            {
                p.SendMessage("Player not found");
                return;
            }

            PlayerGroup group = null;
            group = PlayerGroup.Find(args[1]);

            if (group == null)
            {
                p.SendMessage("Rank not found");
                return;
            }

            if (who.group.permission >= p.group.permission)
            {
                p.SendMessage("You cannot change the rank of someone of an equal or greater rank!");
                return;
            }
            if (group.permission >= p.group.permission)
            {
                p.SendMessage("You cannot promote someone to an equal or greater rank!");
                return;
            }
            if (who.group == group)
            {
                p.SendMessage(group.colour + who.USERNAME + Server.DefaultColor + "is already that rank");
                return;
            }
            group.AddPlayer(who);
            Player.UniversalChat(group.colour + who.USERNAME + Server.DefaultColor + " had their rank set to " + group.colour + group.name);

        }

        public void Help(Player p)
        {
            p.SendMessage("/setrank <player> <rank> - changes the rank of the specified player.");
        }

        public void Initialize()
        {
            Command.AddReference(this, CommandStrings);
        }
    }
}
