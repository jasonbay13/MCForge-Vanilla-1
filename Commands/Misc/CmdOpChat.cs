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
using MCForge.Utils;
namespace CommandDll
{
    public class CmdOpChat : ICommand
    {
        public string Name { get { return "OpChat"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            p.ExtraData.CreateIfNotExist("OpChat", true);
            if (!(bool)p.ExtraData["AdminChat"]) {
                p.SendMessage("OpChat activated. All messages will be sent to operators!");
                p.ExtraData["OpChat"] = true;

                p.ExtraData.CreateIfNotExist("AdminChat", false);
                if ((bool)p.ExtraData["AdminChat"]) {
                    p.SendMessage("AdminChat deactivated!");
                    p.ExtraData["AdminChat"] = false;
                }
                return;
            }
            else {
                p.SendMessage("OpChat off!");
                p.ExtraData["OpChat"] = false;
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/opchat - makes all messages sent go to operators");
        }

        public void Initialize()
        {
            Command.AddReference(this, "opchat");
        }
    }
}

