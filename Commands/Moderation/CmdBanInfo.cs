using System;
using MCForge.Entity;
using MCForge.Interface.Command;
using System.IO;

namespace CommandDll.Moderation
{
    class CmdBanInfo : ICommand
    {
        public string Name { get { return "BanInfo"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Initialize() { Command.AddReference(this, new string[2] { "baninfo", "baninformation" }); }
        public int _ = 0;
        public void Use(Player p, string[] args)
        {
            string[] lines = File.ReadAllLines("baninfo.txt");
            if (lines.Length < 1) { p.SendMessage("Could not find ban information for \"" + args[0] + "\"."); return; }
            foreach (string line in lines)
            {
                string name = line.Split('`')[0];
                string reason = line.Split('`')[1];
                string date = line.Split('`')[2];
                string time = line.Split('`')[3];
                string banner = line.Split('`')[4];
                if (args[0] == name)
                {
                    p.SendMessage(name + " was banned at " + time + " on " + date + " by " + banner + ".");
                    p.SendMessage("Reason: " + reason);
                }
                else
                {
                    _++;
                    if (_ == 1)
                    {
                        p.SendMessage("Could not find ban information for \"" + args[0] + "\".");
                    }
                }
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/baninfo <player> - See information about <player>'s ban.");
        }
    }
}