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
using System.IO;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;
using System.Drawing;

namespace CommandDll
{
    public class CmdNews : ICommand
    {
        public string Name { get { return "News"; } }
        public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "Givo"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args)
        {
            if (!File.Exists("text/news.txt"))
            {
                File.Create("text/news.txt").Close();
                Logger.Log("[File] Created news.txt", Color.White, Color.Black);
                p.SendMessage("No News file was available!");
                return;
            }
            string[] lines = File.ReadAllLines("text/news.txt");
            DateTime editdate = File.GetLastWriteTime("text/news.txt");

            if (args.Length == 0)
            {
                p.SendMessage("News as of " + editdate.ToShortDateString() + ":");
                foreach (string line in lines)
                {
                    p.SendMessage(line);
                }
            }
            else
            {
                Player who = Player.Find(args[0].ToLower());
                who.SendMessage("News as of " + editdate.ToShortDateString() + ":");
                foreach (string line in lines)
                {
                    who.SendMessage(line);
                }
            }
        }
        public void Help(Player p)
        {
            p.SendMessage("/news <player> - Displays the news!");
        }
        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "news" });
        }
    }
}
