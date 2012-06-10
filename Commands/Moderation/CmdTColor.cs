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
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;

namespace MCForge.Commands
{
    public class CmdTColor : ICommand
    {
        public string Name { get { return "TColor"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "7imekeeper"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0 || args.Length > 2) { Help(p); return; }
            Player who;
            string titleColor;

            p.ExtraData.CreateIfNotExist("TitleColor", Server.DefaultColor);

            if (args.Length == 1)
            {
                who = p;
                titleColor = args[0] == "del" ? "del" : Colors.Parse(args[0]);
                if ((string)p.ExtraData.GetIfExist("TitleColor") == titleColor) { p.SendMessage("Your title is already that color!"); return; }
            }
            else
            {
                who = Player.Find(args[0]);
                if (who == null) { p.SendMessage("Could not find player."); return; }
                if (p.Group.Permission <= who.Group.Permission) { p.SendMessage("You can't change the title color of someone of equal or higher rank!"); return; }
                //devs should be able to change their own color
                if (Server.devs.Contains(who.Username) && !Server.devs.Contains(p.Username)) { p.SendMessage("You can't change a dev's title color!"); return; }
                titleColor = args[1] == "del" ? "del" : Colors.Parse(args[1]);
                if (who.Color == titleColor) { p.SendMessage("Their title is already that color!"); return; }
            }
            if (titleColor == "") { p.SendMessage("Could not find color."); return; }

            string message = "";

            who.ExtraData.CreateIfNotExist("TitleColor", Server.DefaultColor);

            if (titleColor == "del")
            {
                who.ExtraData["TitleColor"] = "";
                message = "was removed.";
            }
            else
            {
                who.ExtraData["TitleColor"] = titleColor;
                message = "was set to " + titleColor + Colors.Name(titleColor) + who.Color + ".";
            }
            Player.UniversalChat(who.Color+ "*" + who.Username + (who.Username.EndsWith("s") || who.Username.EndsWith("x") ? "'" : "'s") + " title color " + message);

            who.SetPrefix();
            //TODO Save to database.
        }

        public void Help(Player p)
        {
            p.SendMessage("/titlecolor <color> - Change your own title color.");
            p.SendMessage("/titlecolor [player] <color> - Changes [player]'s title color.");
            p.SendMessage("Available colors: &0black &1navy &2green &3teal &4maroon &5purple &6gold &7silver");
            p.SendMessage("&8gray &9blue &alime &baqua &cred &dpink &eyellow &fwhite");
            p.SendMessage("Shortcut: /tcolor");
        }
        public void Initialize()
        {
            Command.AddReference(this, new string[] { "tcolor", "titlecolor" });
        }
    }
}

