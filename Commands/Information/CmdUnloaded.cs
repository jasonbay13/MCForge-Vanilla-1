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
using System;
using System.Collections.Generic;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Groups;
using MCForge.Interface.Command;
using MCForge.Utils;
using MCForge.World;

namespace MCForge.Commands
{
    public class CmdUnloaded : ICommand
    {
        public string Name { get { return "Unloaded"; } }
        public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "Arrem, 7imekeeper"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return "com.mcforge.unloaded"; } }
        public byte Permission { get { return (byte)(PermissionLevel.Guest); } }

        public void Use(Player p, string[] args)
        {
            if (args.Length > 2) { p.SendMessage("Invalid number of arguments."); Help(p); return; }
            if (args.Length == 0 && Level.UnloadedLevels.Count > 0)
            {
                p.SendMessage("Unloaded levels: &4" + string.Join(Server.DefaultColor + ", &4", Level.UnloadedLevels));
                if (Level.UnloadedLevels.Count > 50) p.SendMessage("Use &b/unloaded <1/2/3...> " + Server.DefaultColor + "for a more structured list!");
            }

            string search = args[0];
            bool countRequest = args[args.Length - 1].Equals("count", StringComparison.OrdinalIgnoreCase);
            int page = StringUtils.IsNumeric(args[args.Length - 1]) ? int.Parse(args[args.Length - 1]) : -1;

            if (StringUtils.IsNumeric(search) || search.Equals("count", StringComparison.OrdinalIgnoreCase))
                search = "";
            List<string> filtered = Level.UnloadedLevels.FindAll(name => name.Contains(search));
            int count = filtered.Count;

            if (count == 0) { p.SendMessage(String.Format("There are no unloaded levels{1}!", search.Equals("") ? "" : " containing &b" + search + Server.DefaultColor)); return; }

            int pages = count % 50 == 0 ? count / 50 : count / 50 + 1;
            if ((page < 0 && page != -1) || search.Equals("-1") || page > pages) { p.SendMessage("Invalid page!"); return; }

            if (countRequest)
                p.SendMessage("There " + (count == 1 ? "is " : "are ") + "&b" + count + Server.DefaultColor + " unloaded level" + (count == 1 ? "" : "s") + (search.Equals("") ? "" : " containing &b" + search + Server.DefaultColor) + "!");
            else if (page == -1)
                p.SendMessage("Unloaded levels containing &b" + search + Server.DefaultColor + ": &4" + string.Join(Server.DefaultColor + ", &4", filtered));
            else
                p.SendMessage("Unloaded levels" + (search.Equals("") ? "" : " containing &b" + search + Server.DefaultColor) + " Page " + page + "/" + pages + ":");
            if (count > 50) p.SendMessage("Use &b/unloaded <1/2/3> " + Server.DefaultColor + "for a more structured list!");
        }

        public void Help(Player p)
        {
            p.SendMessage("/unloaded - shows unloaded levels");
            p.SendMessage("/unloaded [filter] <1/2/3> - shows a more structured list");
            p.SendMessage("/unloaded [filter] count - shows the number of unloaded levels");
            p.SendMessage("If [filter] is specified only levels containing the word will be included");
        }

        public void Initialize()
        {
            Command.AddReference(this, "unloaded");
        }
    }
}