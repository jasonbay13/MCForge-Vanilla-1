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
    public class CmdHide : ICommand
    {
        public string Name { get { return "Hide"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Snowl"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (!p.IsHidden)
                p.GlobalDie();

            p.IsHidden = !p.IsHidden;

            if (!p.IsHidden)
                p.SpawnThisPlayerToOtherPlayers();

            bool sendOpMessage = true;
            bool sendDisconnectMessage = true;
            if (args.Length > 0)
            {
                if (args.Contains("#"))
                    sendOpMessage = false;
                if (args.Contains("!"))
                    sendDisconnectMessage = false;
            }
            if (sendDisconnectMessage && p.IsHidden)
                Player.UniversalChat(p.Username + " has disconnected");
            else if (sendDisconnectMessage && !p.IsHidden)
                Player.UniversalChat(p.Username + " joined the game!");
            if (sendOpMessage)
                Player.UniversalChatOps(p.Username + " has turned invisible!");
        }

        public void Help(Player p)
        {
            p.SendMessage("/hide - Hide from other players");
            p.SendMessage("/hide # - Hide from other players without notifying OPs");
            p.SendMessage("/hide ! - Hide from other players without any messages");
            p.SendMessage("/hide # ! - Hide from other players without any messages (including OP message)");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "hide" });
        }
    }
}

