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
using System;
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
            Random Random = new Random();
            string margs = ArrayToString(args);
            margs = margs.Replace('%', '&');
            Bot TemporaryPlayer = new Bot(margs, p.Pos, p.Rot, p.Level, true);
            TemporaryPlayer.Player.Level.ExtraData.Add("Bot" + Random.Next(0, 9999999), margs + " " + TemporaryPlayer.FollowPlayers +  //TODO - Random INT so the dictionary doesnt clash - this should be fixed
                " " + TemporaryPlayer.Player.Pos.x + " " + TemporaryPlayer.Player.Pos.y + " " + TemporaryPlayer.Player.Pos.z + " "
                + TemporaryPlayer.Player.Rot[0] + " " + TemporaryPlayer.Player.Rot[1]); //Add bot to level metadata
                                                                                        //This enables cross server bot transfer
                                                                                        //And returns when level is loaded
            p.SendMessage("Spawned " + ArrayToString(args) + Server.DefaultColor + "!");
        }

        public void Help(Player p)
        {
            p.SendMessage("/botadd [name] - creates a bot where you are standing.");
        }

        static string ArrayToString(string[] array)
        {
            string result = string.Join(" ", array);
            return result;
        }

        public void Initialize()
        {
            Command.AddReference(this, "botadd");
        }
    }
}

