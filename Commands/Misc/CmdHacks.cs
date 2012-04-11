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
using System.IO;
using MCForge.Entity;
using MCForge.Interface.Command;

namespace CommandDll
{
    public class CmdHacks : ICommand
    {
        public string Name { get { return "Hacks"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Givo"; } }
        public int Version { get { return 1; ; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public static List<string> hacksmessages = new List<string>();

        public void Use(Player p, string[] args)
        {
            int random = new Random().Next(1, 6);
            switch (random)
            {
                case 1: p.Kick("Your IP has been backtraced & reported to the FBI Cyber Crimes Unit."); //classic :P
                    break;
                case 2: p.Kick("Your IP has been backtraced & reported to the FBI Cyber Crimes Unit.");
                    break;
                case 3: p.Kick("Vaše IP byla backtraced & hlášeny FBI Cyber ​​zločiny jednotky.");
                    break;
                case 4: usermessage(p);
                    break;
                case 5: p.Kick("IP juaj është backtraced & raportuar të FBI Cyber ​​Njësisë për Krime.");
                    break;
            }
        }

        void usermessage(Player p)
        {
            if (!File.Exists("text/hacksmessages.txt")) { File.Create("text/hacksmessages.txt").Close(); }
            string text = File.ReadAllText("text/hacksmessages.txt");
            if (text == "") { File.WriteAllText("text/hacksmessages.txt", "Your IP has been backtraced & reported to FBI Cyber Crimes Unit."); }
            StreamReader r = File.OpenText("text/hacksmessages.txt");
            while (!r.EndOfStream) hacksmessages.Add(r.ReadLine());
            r.Dispose();
            p.Kick(hacksmessages[new Random().Next(0, hacksmessages.Count)]);
        }

        public void Help(Player p)
        {
            p.SendMessage("/Hacks - |-|4(|< 7|-|3 |*14||37"); 
        }
        public void Initialize()
        {
            Command.AddReference(this, new string[3] { "hacks", "hax", "haxor" });
        }
    }
}
