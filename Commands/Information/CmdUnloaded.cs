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
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return "com.mcforge.unloaded"; } }
        public byte Permission { get { return (byte)(PermissionLevel.Guest); } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0)
            {
                if (Level.UnloadedLevels.Count == 0) { p.SendMessage("There are no unloaded levels!"); return; }
                p.SendMessage("Unloaded levels:&4" + string.Join(", ", Level.UnloadedLevels));
                if (Level.UnloadedLevels.Count > 50) { p.SendMessage("Use &b/unloaded <1/2/3> " + Server.DefaultColor + " for a more structured list!"); }
                return;
            }
            if (args.Length == 1)
            {
                if (StringUtils.IsNumeric(args[0]))
                {
                    int end = int.Parse(args[0]) * 50, start = end - 50;
                    if (end >= Level.UnloadedLevels.Count) { end = Level.UnloadedLevels.Count - 1; }
                    string send = "";
                    for (int i = start - 1; i <= end; i++) { send += i == end ? Level.UnloadedLevels[i] : Level.UnloadedLevels[i] + ", "; }
                    if (send == "") { p.SendMessage(String.Format("There are no{0}unloaded levels!", int.Parse(args[0]) == 1 ? " " : " more ")); return; }
                    p.SendMessage("Unloaded levels from &b" + start + Server.DefaultColor + " to &b" + end + Server.DefaultColor + ": &4" + send);
                }
                else if (args[0].ToLower() == "count")
                {
                    int count = Level.UnloadedLevels.Count;
                    p.SendMessage(String.Format("There {0} {1} unloaded level{2}!", count == 1 ? "is" : "are", count == 0 ? "no" : count.ToString(), count == 1 ? "" : "s"));
                }
                else
                {
                    string send = string.Join(", ", Level.UnloadedLevels.FindAll(name => name.Contains(args[0])));
                    if (send == "") { p.SendMessage("There are no unloaded levels containing &b" + args[0] + Server.DefaultColor + "!"); return; }
                    p.SendMessage("Unloaded levels containing &b" + args[0] + Server.DefaultColor + ": &4" + send);
                }
            }
            if (args.Length == 2)
            {
                if (StringUtils.IsNumeric(args[1]))
                {
                    string filter = args[0], send = "";
                    int end = int.Parse(args[1]) * 50, start = end - 50;
                    List<string> filtered = Level.UnloadedLevels.FindAll(name => name.Contains(filter));
                    //if (end >= filtered.Count) { end = filtered.Count - 1; }
                    for (int i = start - 1; i < end; i++) { send += i == end ? filtered[i] : filtered[i] + ", "; }
                    if (send == "") { p.SendMessage(String.Format("There are no {0}unloaded levels containing &b" + filter + Server.DefaultColor + "!", int.Parse(args[1]) == 1 ? "" : "more ")); return; }
                    p.SendMessage("Unloaded levels containing &b" + filter + Server.DefaultColor + "from &b" + start + Server.DefaultColor + " to &b" + end + Server.DefaultColor + ": &4" + send);
                }

                else if (args[1] == "count")
                {
                    int count = Level.UnloadedLevels.FindAll(name => name.Contains(args[0])).Count;
                    p.SendMessage(String.Format("There {0} {1} unloaded level{2} containing &b" + args[0] + Server.DefaultColor + "!", count == 1 ? "is" : "are", count == 0 ? "no" : count.ToString(), count == 1 ? "" : "s"));
                }
                else { /*p.SendMessage("You did stuff wrong!");*/ Help(p); } //somebody tell the player what he did wrong. i'm a stupid fuck. thank you
            }

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