/*
Copyright 2012 MCForge
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
using MCForge.Core;
using MCForge.Entity;
using MCForge.Utils;
using System;
using MCForge.World;

namespace MCForge.Commands.Information
{
    public class CmdLevels : ICommand
    {
        public string Name { get { return "Levels"; } }
        public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "Snowl"; } }
        public int Version { get { return 1; } } public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            //Todo - if not allowed to go to don't show, show physics
            try
            {
                string message = "";
                string message2 = "";
                bool Once = false;
                Level.Levels.ForEach(delegate(Level level)
                {
                    message += ", " + level.Name;
                });
                p.SendMessage("Loaded: " + message.Remove(0, 2));
                p.SendMessage("Use &4/unloaded for unloaded levels.");
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/levels - Lists all loaded levels and their physics levels.");
        }

        public void Initialize()
        {
            Command.AddReference(this, "levels"); 
        }
    }
}