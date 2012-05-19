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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Groups;
using System.Threading;
using MCForge.Utils;
using MCForge.Utils.Settings;

namespace CommandDll.Moderation
{
    public class CmdPatrol : ICommand
    {

        public string Name { get { return "Patrol"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Nerketur"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 80; } }

        public void Use(Player p, string[] args)
        {
            PlayerGroup wanted = null;
            if (args.Count() == 1)
            {
                wanted = PlayerGroup.Find(args[0].ToLower()); // will be null if none found.
                if (wanted == null)
                    p.SendMessage("Spcified group doesn't exist.  Using all groups below you...");
            }
            if (wanted != null && wanted.Permission >= p.Group.Permission)
            {
                wanted = null;
                p.SendMessage("Sorry, you can only patrol groups of a lower rank.  Using all groups below you...");
            }
            p.SendMessage("Finding a person " + (wanted == null ? "under you" : "of the specified rank") + " to patrol...");
            ICommand gotoCmd = Command.Find("goto"); //If goto exists, we can use it to go to the new level before teleporting.
            List<Player> allUnder = Server.Players.FindAll(plr => (wanted == null ? true : plr.Group.Permission == wanted.Permission) && plr.Group.Permission < p.Group.Permission && (gotoCmd == null ? p.Level == plr.Level : true));
            if (allUnder.Count == 0)
            {
                p.SendMessage("There are no people " + (wanted == null ? "under your" : "of the specified") + " rank that are " + (gotoCmd == null ? "in your level." : "currently online."));
                return;
            }
            Player found = allUnder[(new Random()).Next(allUnder.Count)];
            p.SendMessage("Player found!  Transporting you to " + (string)found.ExtraData.GetIfExist("Color") ?? "" + found.Username + Server.DefaultColor + "!");
            if (p.Level != found.Level)
            {
                //Go to the level first
                gotoCmd.Use(p, new string[] { found.Level.Name });
            }
            if (found.IsLoading)
            {
                p.SendMessage("Waiting for " + (string)found.ExtraData.GetIfExist("Color") ?? "" + found.Username + Server.DefaultColor + " to spawn...");

                while (found.IsLoading) { 
                    Thread.Sleep(5);
                } // until event works
            }
            while (p.IsLoading) {
                Thread.Sleep(5);
            } // until event works.
            p.SendToPos(found.Pos, found.Rot);
        }

        public void Help(Player p)
        {
            p.SendMessage("/patrol - Teleports you to \"patrol\" a user with a lower rank.");
            p.SendMessage("/patrol [rank] - Teleports you to \"patrol\" a random user with the rank [rank].");
        }

        public void Initialize()
        {
            Command.AddReference(this, "patrol");
        }
    }
}
