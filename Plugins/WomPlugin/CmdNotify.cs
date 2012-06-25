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
using MCForge.Utils;
using MCForge.Entity;
using MCForge.World;
using MCForge.Interface.Command;
using MCForge;

namespace Plugins.WoMPlugin
{
    public class CmdNotify : ICommand
    {
        public string Name { get { return "Notify"; } }
        public CommandTypes Type { get { return CommandTypes.Custom; } }
        public string Author { get { return "Gamemakergm and givo"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            string messageToSend = StringArrayToString(args);
            //Givo - gah ill make it send the rest below another time
            if (messageToSend.Length >= 44)
            {
                p.SendMessage("You can only send messages with 44 characters or less");
                return;
            }
            switch (args.Length)
            {
                case 1:
                    WOM.SendAlert(p, messageToSend);
                    return;
                case 2:
                    if (args[0] == "all")
                    {
                        WOM.GlobalSendAlert(messageToSend);
                        return;
                    }
                    Level l = Level.FindLevel(args[0]);
                    Player pl = Player.Find(args[0]);
                    if (l != null)
                    {
                        WOM.LevelSendAlert(l, messageToSend);
                        return;
                    }
                    else if (pl != null)
                    {
                        WOM.SendAlert(pl, messageToSend);
                        return;
                    }
                    return;
                default:
                    p.SendMessage((args.Length < 1) ? "You need to specify a message!" : "Invalid arguments!");
                    return;
            }
        }

        private string StringArrayToString(string[] array)
        {
            string t = "";
            foreach (string str in array)
            {
                t += str + " ";
            }
            return t;
        }

        public void Help(Player p)
        {
            p.SendMessage("/notify [user] <message> - Sends a message up the top right corner to WoM users.");
            p.SendMessage("If no user is specified the message is sent to yourself.");
            p.SendMessage("Supported users: [all], a specified level, or a specified player.");
            p.SendMessage("Shortcuts: /alert");
        }

        public void Initialize()
        {
            Command.AddReference(this, "notify", "alert");
        }
    }
}
