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
using MCForge;
using System.Threading;
using System;

namespace CommandDll
{
    public class CmdVote : ICommand
    {
        public string Name { get { return "Vote"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { p.SendMessage("You have to specify the vote message!"); return; }
            string message = null; foreach (string s in args) { message += s + " "; }
            if (Server.voting) { p.SendMessage("A vote is already in progress!"); return; }
            ResetVotes();
            Server.voting = true;
            Player.UniversalChat("VOTE: " + message);
            Player.UniversalChat("Use: %aYes " + Server.DefaultColor + "or %cNo " + Server.DefaultColor + "to vote!");
            Thread.Sleep(15000);
            Player.UniversalChat("The votes are in! %aYes: " + Server.YesVotes + " %cNo: " + Server.NoVotes + Server.DefaultColor + "!");
            foreach (Player pl in Server.Players.ToArray()) { pl.voted = false; }
            Server.voting = false;
        }

        public void Help(Player p)
        {
            p.SendMessage("/vote <message> - Starts a 15 second vote");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "vote", "vo" });
        }
        public void ResetVotes() { Server.YesVotes = 0; Server.NoVotes = 0; }
    }
}
