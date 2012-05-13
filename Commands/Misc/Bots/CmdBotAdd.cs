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
using MCForge.Core;
using MCForge.Robot;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;
using System.Collections.Generic;
namespace CommandDll
{
    public class CmdBotAdd : ICommand
    {
        public string Name { get { return "BotAdd"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Snowl"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length < 1)
            {
                p.SendMessage("You must specify a name!");
                return;
            }
            string margs = ArrayToString(args);
            margs = margs.Replace('%', '&');
            Bot TemporaryPlayer = new Bot();
            TemporaryPlayer.Player = new Player();
            TemporaryPlayer.Player.Username = margs;
            TemporaryPlayer.Player.Pos.x = p.Pos.x;
            TemporaryPlayer.Player.Pos.y = p.Pos.y;
            TemporaryPlayer.Player.Pos.z = p.Pos.z;
            TemporaryPlayer.Player.Rot = new byte[2] { 0, 0 };
            TemporaryPlayer.Player.Level = p.Level;
            TemporaryPlayer.Player.id = FreeId();
            Server.Bots.Add(TemporaryPlayer);
            SpawnThisBotToOtherPlayers(TemporaryPlayer);
            p.SendMessage("Spawned " + ArrayToString(args) + Server.DefaultColor + "!");
        }

        public void Help(Player p)
        {
            p.SendMessage("/botadd [name] - creates a temporary bot where you are standing.");
        }

        protected byte FreeId()
        {
            List<byte> usedIds = new List<byte>();

            Server.ForeachBot(p => usedIds.Add(p.Player.id));

            for (byte i = 0; i < 253; ++i)
            {
                if (usedIds.Contains(i)) continue;
                return i;
            }

            return 254;
        }

        protected void SpawnThisBotToOtherPlayers(Bot z)
        {
            Server.ForeachPlayer(delegate(Player p)
            {
                if (p != z.Player)
                    p.SendSpawn(z.Player);
            });
        }

        static string ArrayToString(string[] array)
        {
            //
            // Use string Join to concatenate the string elements.
            //
            string result = string.Join(" ", array);
            return result;
        }

        public void Initialize()
        {
            Command.AddReference(this, "botadd");
            Command.AddReference(this, "bota");
        }
    }
}

