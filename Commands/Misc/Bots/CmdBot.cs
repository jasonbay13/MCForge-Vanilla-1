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
    public class CmdBot : ICommand
    {
        public string Name { get { return "BotAI"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Snowl"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length < 2)
            {
                p.SendMessage("You must specify an option and a name!");
                return;
            }
            Random Random = new Random();
            if (args[0].ToLower() == "add")
            {
                List<string> fargs = new List<string>();
                int l = 0;
                foreach (string s in args)
                {
                    if (l > 0)
                        fargs.Add(s);
                    l++;
                }
                string margs = ArrayToString(fargs.ToArray());
                margs = margs.Replace('%', '&');
                Bot TemporaryPlayer = new Bot(margs, p.Pos, p.Rot, p.Level, false, false);
                TemporaryPlayer.Player.Level.ExtraData.Add("Bot" + Random.Next(0, 9999999), margs + " " + TemporaryPlayer.FollowPlayers + " " + TemporaryPlayer.BreakBlocks +
                    " " + TemporaryPlayer.Player.Pos.x + " " + TemporaryPlayer.Player.Pos.y + " " + TemporaryPlayer.Player.Pos.z + " "
                    + TemporaryPlayer.Player.Rot[0] + " " + TemporaryPlayer.Player.Rot[1]); //Add bot to level metadata
                                                                                            //This enables cross server bot transfer
                                                                                            //And returns when level is loaded
                p.SendMessage("Spawned " + ArrayToString(fargs.ToArray()) + Server.DefaultColor + "!");
            }
            else if (args[0].ToLower() == "remove")
            {
                List<string> fargs = new List<string>();
                int l = 0;
                foreach (string s in args)
                {
                    if (l > 0)
                        fargs.Add(s);
                    l++;
                }
                string margs = ArrayToString(fargs.ToArray());
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
                    p.SendMessage("Removed " + ArrayToString(fargs.ToArray()) + Server.DefaultColor + "!");
                else
                    p.SendMessage("Could not find " + ArrayToString(fargs.ToArray()) + Server.DefaultColor + "!");
            }
            else if (args[0].ToLower() == "summon")
            {
                List<string> fargs = new List<string>();
                int l = 0;
                foreach (string s in args)
                {
                    if (l > 0)
                        fargs.Add(s);
                    l++;
                }
                string margs = ArrayToString(fargs.ToArray());
                margs = margs.Replace('%', '&');
                bool hitBot = false;
                foreach (Bot b in Server.Bots.ToArray())
                {
                    if (b.Player.Username.ToLower() == margs.ToLower() &&
                        b.Player.Level == p.Level)
                    {
                        hitBot = true;
                        b.Player.Pos = p.Pos;
                    }
                }
                if (hitBot)
                    p.SendMessage("Summoned " + ArrayToString(fargs.ToArray()) + Server.DefaultColor + "!");
                else
                    p.SendMessage("Could not find " + ArrayToString(fargs.ToArray()) + Server.DefaultColor + "!");
            }
            else if (args[0].ToLower() == "ai")
            {
                List<string> fargs = new List<string>();
                int l = 0;
                foreach (string s in args)
                {
                    if (l > 0)
                        fargs.Add(s);
                    l++;
                }
                string margs = ArrayToString(fargs.ToArray());
                margs = margs.Replace('%', '&');
                string[] blargs = margs.Split('"');
                if (blargs.Length != 3)
                {
                    p.SendMessage("You need to use \"'s in your command!");
                    return;
                }
                foreach (string s in blargs)
                {
                    s.Replace("\"", "");
                }
                string FoundPlayer = blargs[1];
                string AI = blargs[2].Remove(0, 1);
                bool HitBot = false;

                Bot Bot = null;

                foreach (Bot b in Server.Bots.ToArray())
                {
                    if (b.Player.Username.ToLower() == FoundPlayer.ToLower())
                    {
                        switch (AI)
                        {
                            case "follow":
                                b.FollowPlayers = !b.FollowPlayers;
                                HitBot = true;
                                Bot = b;
                                break;
                            case "break":
                                b.BreakBlocks = !b.BreakBlocks;
                                HitBot = true;
                                Bot = b;
                                break;
                        }
                    }
                }

                List<string> tempArray = new List<string>();
                foreach (var b in p.Level.ExtraData)
                {
                    if (b.Value.ToLower().Split(' ')[0].Equals(margs.ToLower()))
                    {
                        tempArray.Add(b.Key);
                    }
                }
                foreach (string s in tempArray)
                {
                    p.Level.ExtraData.Remove(s);
                }

                Bot.Player.Level.ExtraData.Add("Bot" + Random.Next(0, 9999999), margs + " " + Bot.FollowPlayers + " " + Bot.BreakBlocks +
                    " " + Bot.Player.Pos.x + " " + Bot.Player.Pos.y + " " + Bot.Player.Pos.z + " "
                    + Bot.Player.Rot[0] + " " + Bot.Player.Rot[1]);

                if (HitBot)
                    p.SendMessage("Changed " + FoundPlayer + "'s AI!");
                else
                {
                    p.SendMessage("Couldn't find " + FoundPlayer + " or change the AI!");
                    return;
                }
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/bot add [name] - creates a bot where you are standing.");
            p.SendMessage("/bot remove [name] - removes bot with [name] from your level.");
            p.SendMessage("/bot ai \"[name]\" [type] - toggles ai to bot. \"'s are required.");
            p.SendMessage("Available types of AI: follow, break");
        }

        static string ArrayToString(string[] array)
        {
            string result = string.Join(" ", array);
            return result;
        }

        public void Initialize()
        {
            Command.AddReference(this, "bot");
        }
    }
}

