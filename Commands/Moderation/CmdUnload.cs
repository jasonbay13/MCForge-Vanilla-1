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
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.World;
using MCForge.Core;
namespace CommandDll
{
    public class CmdUnload : ICommand
    {
        public string Name { get { return "Unload"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Snowl"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            Level isLoaded = Level.FindLevel(args[0]);
            if (isLoaded.Name.ToLower() == Server.Mainlevel.Name.ToLower())
            {
                p.SendMessage("You cannot unload the main level.");
                return;
            }
            if (isLoaded == null)
            {
                p.SendMessage(args[0] + " is already unloaded.");
            }
            else
            {
                foreach (Player z in Server.Players.ToArray())
                {
                    if (z.Level == isLoaded)
                        z.Level = Server.Mainlevel;
                }
                Level.Levels.Remove(isLoaded);
                Player.UniversalChat(args[0] + " has been unloaded.");
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/unload <level> - Unloads <level>.");
        }

        public void Initialize()
        {
            Command.AddReference(this, "unload");
        }
    }
}

