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
using MCForge.Interface.Command;
using MCForge.Entity;
using System.Collections.Generic;
using System.IO;

namespace CommandDll.Moderation
{
    class CmdEditBan : ICommand
    {
        public string Name { get { return "EditBan"; } }
        public CommandTypes Type { get { return CommandTypes.mod; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Initialize() { Command.AddReference(this, new string[2] { "editban", "banedit" }); }
        public void Use(Player p, string[] args)
        {
            int _ = 0;
            string _newreason = "";
            string newreason = _newreason.Substring(args[0].IndexOf(" ") + 1);
            string[] lines = File.ReadAllLines("Bans/Ban Info.txt");
            if (lines.Length < 1) { p.SendMessage("Could not find ban information for \"" + args[0] + "\"."); return; }
            foreach (string line in lines)
            {
                if (line.Split('`')[0] == args[0])
                {
                    string date = line.Split('`')[2];
                    string time = line.Split('`')[3];
                    string banner = line.Split('`')[4];
                    List<string> temp = new List<string>();
                    foreach (string l in lines)
                    {
                        if (!l.StartsWith(args[0]))
                            temp.Add(l);
                        temp.Add(args[0] + "`" + newreason + "`" + date + "`" + time + "`" + banner);
                    }
                    File.WriteAllLines("baninfo.txt", temp.ToArray());
                    p.SendMessage("Successfully set " + args[0] + "'s ban reason to \"" + newreason + "\".");
                }
                else
                {
                    _++;
                    if (_ == 1)
                        p.SendMessage("Could not find ban information for \"" + args[0] + "\".");
                }
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/editban <player> <reason> - Set <player>'s ban reason to <reason>.");
        }
    }
}