/*
Copyright 2011 MCForge
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
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll
{
    public class CmdRTitle : ICommand
    {
        public string Name { get { return "RTitle"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "7imekeeper"; } }
        public decimal Version { get { return 1.00m; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            string message = "";
            if (args.Length == 0)
            {
                if (p.title == "")
                {
                    Help(p);
                    return;
                }
                message = p.title;
            }
            else
            {
                foreach (string s in args) { message += s + " "; }
            }
            int max;
            int skip;
            char[] rainbow = "6ea95".ToCharArray();
            Player who;
            if (args.Length > 1)
            {
                who = Player.Find(message.Split(' ')[0]);
                if (who == null) { p.SendMessage("Could not find player."); return; }
                if (p.group.permission <= who.group.permission) { p.SendMessage("You can't change the title of someone of equal or higher rank!"); return; }
                if (Server.devs.Contains(who.Username) && !Server.devs.Contains(p.Username)) { p.SendMessage("You can't change a dev's title!"); return; }
                message = message.Substring(message.IndexOf(' ') + 1);
            }
            else
                who = p;
            if (message != who.title)
                message = message.Substring(0, message.Length - 1);
            max = (19 - message.Length) / 2;
            int temp = message.Length / max;
            skip = message.Length % max > 0 ? temp + 1 : temp;
            if (skip <= 0)
                skip = 1;
            if (max > message.Length)
                max = message.Length;
            if (max <= 1)
            {
                p.SendMessage("Can not rainbow that title. Try using /title and /tcolor.");
                return;
            }
            int i = 0;
            for (int j = skip; j < message.Length; j += skip + 2)
            {
                message = message.Substring(0, j) + "&" + rainbow[i] + message.Substring(j, message.Length - j);
                temp = (message.Length - (j + 2)) / (max - (i + 1));
                skip = (message.Length - (j + 2)) % (max - (i + 1)) > 0 ? temp + 1 : temp;
                i++;
                if (i > max - 1)
                    break;
            }
            who.titleColor = "&c";
            who.title = message;
            who.SetPrefix();
            Player.UniversalChat(who.color + who.Username + Server.DefaultColor + " had thier title set to &b[" + who.titleColor + who.title + "&b]");
            //TODO Save to database.
        }

        public void Help(Player p)
        {
            p.SendMessage("/rtitle - Rainbows your current title.");
            p.SendMessage("/rtitle <title> - Gives you a rainbow title.");
            p.SendMessage("/rtitle <player> <title> - Gives <player> a rainbow title.");
            p.SendMessage("Note: Rainbow titles can only be up to 15 letters long.");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "rtitle" });
        }
    }
}

