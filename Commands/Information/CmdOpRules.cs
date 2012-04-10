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
using System.Linq;
using System.Text;
using System.Threading;
using MCForge;
using System.IO;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll
{
    public class CmdOpRules : ICommand
    {
        public string Name { get { return "Operator Rules"; } }
        public CommandTypes Type { get { return CommandTypes.information; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            Player who = null;
            if (args.Length == 0) { who = p; }
            else { who = Player.Find(args[0]); }
            if (who == null) { p.SendMessage("Cannot find that player!"); return; }
            if (!File.Exists("textop/rules.txt")) { File.WriteAllText("text/oprules.txt", "No oprules added yet!"); }
            string[] rules = File.ReadAllLines("text/oprules.txt");
            who.SendMessage("Operator rules:");
            foreach (string rule in rules) { who.SendMessage(rule); }
            if (who != p) { p.SendMessage("Sent oprules to " + who.username); }
        }

        public void Help(Player p)
        {
            p.SendMessage("/oprules [player] - Shows the server's operator rules");
            p.SendMessage("Use a player's name to send the oprules to that player!");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "oprules" });
        }
    }
}
