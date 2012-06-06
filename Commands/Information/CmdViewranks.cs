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
using System.IO;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Groups;
using MCForge.Interface.Command;

namespace CommandDll
{
    public class CmdViewranks : ICommand
    {
        public string Name { get { return "Viewranks"; } }
        public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { Help(p); }
            PlayerGroup group = PlayerGroup.Find(args[0]);
            if (group == null) { p.SendMessage("The rank \"" + args[0] + "\" doesn't exist!"); return; }
            try
            {
                string[] players = File.ReadAllLines(group.File);
                string send = "People with the rank " + group.Color + group.Name + Server.DefaultColor + ": ";
                foreach (string player in players) { send += player + "&a, " + Server.DefaultColor; }         
                p.SendMessage(send.Remove(send.Length - 4, 4));
            }
            catch { p.SendMessage("Error reading ranks!"); return; }
        }
        public void Help(Player p)
        {
            p.SendMessage("/viewranks <rank> - Shows all players with the specified rank");
        }
        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "viewranks" });
        }
    }
}
