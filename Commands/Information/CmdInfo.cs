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
using System.Reflection;
using MCForge.API.PlayerEvent;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utilities.Settings;
using MCForge.World;
using MCForge.Utils;
namespace CommandDll
{
    public class CmdInfo : ICommand
    {
        public string Name { get { return "Info"; } }
        public CommandTypes Type { get { return CommandTypes.information; } }
        public string Author { get { return "jasonbay13"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length != 0) { Help(p); return; }
            p.SendMessage("This server's name is &b" + ServerSettings.GetSetting("servername") + Server.DefaultColor + ".");
            p.SendMessage(Server.Players.Count == 1 ? "There is no one else on the server" : "There are currently " + Server.Players.Count + " players on this server"); //TODO dont include hidden if above current rank
            //p.SendMessage("This server currently has $banned people that are &8banned" + Server.DefaultColor + ".");
            p.SendMessage("This server currently has " + Level.Levels.Count + " levels loaded.");
            //p.SendMessage("This server's currency is: " + Server.moneys); // later for when money works.
            p.SendMessage("This server runs on &bMCForge 2.0" + Server.DefaultColor + ".");
            p.SendMessage("This server's version: &a" + Assembly.GetExecutingAssembly().GetName().Version);
            TimeSpan up = DateTime.Now - Server.StartTime;
            string upTime = "Time online: &b";
            if (up.Days == 1) upTime += up.Days + " day, ";
            else if (up.Days > 0) upTime += up.Days + " days, ";
            if (up.Hours == 1) upTime += up.Hours + " hour, ";
            else if (up.Days > 0 || up.Hours > 0) upTime += up.Hours + " hours, ";
            if (up.Minutes == 1) upTime += up.Minutes + " minute and ";
            else if (up.Hours > 0 || up.Days > 0 || up.Minutes > 0) upTime += up.Minutes + " minutes and ";
            upTime += up.Seconds == 1 ? up.Seconds + " second" : up.Seconds + " seconds";
            p.SendMessage(upTime);
            p.SendMessage("Type \"yes\" to see the devs list.");

            OnPlayerChat.Register((t) =>
            {
                p.ExtraData.CreateIfNotExist("LastCmd", "");
                if (t.message.ToLower() == "yes" && t.Player.ExtraData["LastCmd"] == "info") 
                    Command.all["devs"].Use(p, new string[0]); 
                t.Cancel(); 
                t.Unregister();
            }, p);
        }

        public void Help(Player p)
        {
            p.SendMessage("/info - Shows the server info.");
        }

        public void Initialize()
        {
            Command.AddReference(this, "info");
        }
    }
}
