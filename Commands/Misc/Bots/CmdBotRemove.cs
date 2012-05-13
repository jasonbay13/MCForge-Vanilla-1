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
    public class CmdBotRemove : ICommand
    {
        public string Name { get { return "BotRemove"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
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
            bool hitBot = false;
            foreach (Bot b in Server.Bots.ToArray())
            {
                if (b.Player.Username.ToLower() == margs.ToLower() &&
                    b.Player.Level == p.Level)
                {
                    hitBot = true;
                    b.Player.GlobalDie();
                    Server.Bots.Remove(b);
                }
            }
            List<string> tempArray = new List<string>();
            foreach (var b in p.Level.ExtraData)
            {
                Server.Log(b.Value.ToLower() + " " + margs);
                if (b.Value.ToLower().Split(' ')[0].Equals(margs.ToLower()))
                {
                    tempArray.Add(b.Key);
                }
            }
            foreach (string s in tempArray)
            {
                hitBot = true;
                p.Level.ExtraData.Remove(s);
            }
            if (hitBot)
                p.SendMessage("Removed " + ArrayToString(args) + Server.DefaultColor + "!");
            else
                p.SendMessage("Could not find " + ArrayToString(args) + Server.DefaultColor + "!");
        }

        public void Help(Player p)
        {
            p.SendMessage("/botremove [name] - removes bot from the level you are in");
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
            Command.AddReference(this, "botremove");
        }
    }
}

