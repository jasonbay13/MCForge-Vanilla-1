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
    public class CmdTitle : ICommand
    {
        public string Name { get { return "Title"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "7imekeeper"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { p.SendMessage("You have to specify a message!"); return; }
            Player who;
            string title = "";

            p.ExtraData.CreateIfNotExist("Title", "");

            if (args.Length < 2)
            {
                who = p;
                title = args[0];
            }
            else
            {
                who = Player.Find(args[1]);
                if (who == null) { p.SendMessage("Could not find player."); return; }
                //TODO Cases involving ranks
                if (args[1] == "del")
                    title = "del";
                else
                {
                    foreach (string s in args) { title += s + " "; }
                }
            }
            if (title.Length > 17) { p.SendMessage("Title must be under 17 letters."); return; }
            if (!Server.Devs.Contains(p.Username))
            {
                if (Server.Devs.Contains(who.Username) || title.ToLower() == "dev") { p.SendMessage("Can't let you do that, starfox."); return; }
            }

            who.ExtraData.CreateIfNotExist("Title", "");
            string message = "";
            if (title == "del")
            {
                who.ExtraData["Title"] = "";
                message = "removed.";
            }
            else
            {
                who.ExtraData["Title"] = title;
                message = "set to &b[" + title + "]";
            }
            who.SetPrefix();
            Player.UniversalChat(who.Color + who.Username + Server.DefaultColor + " had their title " + message);
            //TODO Save to database.
        }
        public void Help(Player p)
        {
            p.SendMessage("/title <title> - Sets your title");
            p.SendMessage("/title [player] <title> - Sets [player]'s title");
            p.SendMessage("If <title> is \"del\", the current title will be deleted.");
        }

        public void Initialize()
        {
            Command.AddReference(this, "title");
        }
    }
}

