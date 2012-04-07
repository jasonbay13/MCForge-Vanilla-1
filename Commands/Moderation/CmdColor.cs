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
    public class CmdColor : ICommand
    {
        public string Name { get { return "Color"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "7imekeeper"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0 || args.Length > 2) { Help(p); return; }
            Player who;
            string color;
            if (args.Length == 1)
            {
                who = p;
                color = args[0] == "del" ? p.group.color : Colors.Parse(args[0]);
                if (p.color == color) { p.SendMessage("You are already that color!"); return; }
            }
            else
            {
                who = Player.Find(args[0]);
                if (who == null) { p.SendMessage("Could not find player."); return; }
                if (p.group.permission <= who.group.permission) { p.SendMessage("You can't change the color of someone of equal or higher rank!"); return; }
                if (Server.devs.Contains(who.USERNAME) && !Server.devs.Contains(p.USERNAME)) { p.SendMessage("You can't change a dev's color!"); return; }
                color = args[1] == "del" ? who.group.color : Colors.Parse(args[1]);
                if (who.color == color) { p.SendMessage("They are already that color!"); return; }
            }
            if (color == "") { p.SendMessage("Could not find color."); return; }

            string message = "";
            if (color == who.group.color)
                message = "their groups default.";
            else
                message = color + Colors.Name(color) + who.color + ".";
            Player.UniversalChat(who.color + "*" + who.USERNAME + (who.USERNAME.EndsWith("s") || who.USERNAME.EndsWith("x") ? "'" : "'s") + " color was changed to " + message);
            who.color = color;

            who.GlobalDie();
            who.SendSpawn(who);
            who.SetPrefix();
            //TODO Save to database.
        }

        public void Help(Player p)
        {
            p.SendMessage("/color [player] <color> - Changes the nick color.");
            p.SendMessage("&0black &1navy &2green &3teal &4maroon &5purple &6gold &7silver");
            p.SendMessage("&8gray &9blue &alime &baqua &cred &dpink &eyellow &fwhite");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "color" });
        }
    }
}

