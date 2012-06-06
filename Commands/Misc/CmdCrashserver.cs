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
using MCForge.Interface.Command;
using MCForge.Entity;
using System.IO;

namespace CommandDll
{
    public class CmdCrashServer : ICommand
    {
        public string Name { get { return "CrashServer"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Givo"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length > 0) { Help(p); }
            string random = Path.GetRandomFileName();
            random = random.Replace(".", "");
            p.Kick("SERVER CRASH ERROR CODE x8" + random.ToUpper());
        }
        public void Help(Player p)
        {
            p.SendMessage("/crashserver - Crash the server, they'll never know its you"); 
        }
        public void Initialize()
        {
            Command.AddReference(this, "crashserver");
        }
    }
}
