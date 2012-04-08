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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Groups;
using System;
namespace CommandDll
{
    public class CmdReview : ICommand
    {
        public string Name { get { return "Review"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "Arrem"; } }
        public Version Version { get { return new Version(1,0); } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { Help(p); return; }
            string function = args[0].ToLower();
            switch (function)
            {
                #region ==Enter==
                case "enter":
                    bool ops = false;
                    Server.ForeachPlayer(delegate(Player pl)
                    {
                        if (pl.group.permission >= Server.reviewnextperm) { ops = true; }
                    });
                    if (!ops) { p.SendMessage("There are no ops online! Please wait for one to come on before asking for review"); return; }
                    if (Server.reviewlist.Contains(p))
                    {
                        p.SendMessage("You're already in the review queue!");
                        SendPositon(false, p);
                        return;
                    }
                        Server.reviewlist.Add(p);
                        p.SendMessage("You have been added to the review queue!");
                        Player.UniversalChatOps(p.Username + " has entered the review queue!");
                        int poss = Server.reviewlist.IndexOf(p);
                        SendPositon(false, p);
                    break;
                #endregion
                #region ==View==
                case "view":
                    if (Server.reviewlist.Count == 0) { p.SendMessage("There are no players in the review queue!"); return; }
                    string send = "Players in the review queue: ";
                    foreach (Player pl in Server.reviewlist) { send += pl.Username + ", "; }
                    send = send.Trim().TrimEnd(',');
                    p.SendMessage(send);
                    break;
                #endregion
                #region ==Position==
                case "position":
                case "pos":
                case "place":
                    if (!Server.reviewlist.Contains(p))
                    {
                        p.SendMessage("You're not in the review queue!");
                        p.SendMessage("Use &9/review &benter " + Server.DefaultColor + "to enter the queue!");
                        return;
                    }
                    SendPositon(false, p);
                    break;
                #endregion
                #region ==Leave==
                case "leave":
                    if (!Server.reviewlist.Contains(p)) 
                    { 
                        p.SendMessage("You're not in the review queue!");
                        p.SendMessage("Use &9/review &benter " + Server.DefaultColor + "to enter the queue!");
                        return;
                    }
                    Server.reviewlist.Remove(p);
                    Player.UniversalChat(p.Username + " has left the queue!");
                    p.SendMessage("You have been removed from the review queue!");
                    SendPositon(true);
                    break;
                #endregion
                #region ==Next==
                case "next":
                    if (p.group.permission < Server.reviewnextperm) 
                    { 
                        p.SendMessage("You can't use &9/review &bnext" + Server.DefaultColor + "!"); 
                        p.SendMessage("Use &9/review &benter " + Server.DefaultColor + "to enter the queue!");
                        return;
                    }
                    Player rev = Server.reviewlist[0];
                    if (rev == null) { p.SendMessage(rev.Username + " isn't online! Removing..."); Server.reviewlist.Remove(rev); SendPositon(true); return; }
                    if (rev == p) { p.SendMessage("Cannot review yourself! Removing..."); Server.reviewlist.Remove(rev); SendPositon(true); return; }              
                    SendPositon(true);
                    string[] meep = new string[1] { rev.Username };
                    ICommand cmd = Command.Find("tp");
                    p.SendMessage(cmd.Name);
                    cmd.Use(p, meep);
                    Server.reviewlist.Remove(rev);
                    p.SendMessage("You are reviewing " + rev.Username + "!");
                    p.SendMessage("Rank: " + rev.group.name);
                    p.SendMessage(p.Username + " has came to review you!");
                    break;
                #endregion
                case "clear":
                    if (p.group.permission < Server.reviewnextperm) { p.SendMessage("You can't clear the review queue!"); return; }
                    Player.UniversalChat("The review queue has been cleared by " + p.Username + "!");
                    Server.reviewlist.Clear();
                    break;
                default:
                    p.SendMessage("You have specified a wrong option!");
                    Help(p);
                    break;
            }
        }
        
        public void Help(Player p)
        {
            p.SendMessage("/review enter - join the review queue");
            p.SendMessage("/review leave - leave the review queue");
            p.SendMessage("/review view - shows the review queue");
            p.SendMessage("/review position - shows your place in the review queue");
            if (p.group.permission >= Server.reviewnextperm)
            {
                p.SendMessage("/review next - review the next player in the review queue");
                p.SendMessage("/review clear - clears the review queue");
            }
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[1] { "review" });
        }
        void SendPositon(bool all, Player player = null)
        {
            if (all)
            {
                foreach (Player pl in Server.reviewlist.ToArray())
                {
                    int position = Server.reviewlist.IndexOf(pl);
                    if (position == 0) { pl.SendMessage("You're next in the review queue!"); return; }
                    pl.SendMessage(position == 1 ? "There is " + position + " players in front of you!" : "There are " + position + " players in front of you!");
                }
                return;
            }
            else
            {
                if (!Server.reviewlist.Contains(player)) { return; }
                int position = Server.reviewlist.IndexOf(player);
                if (position == 0) { player.SendMessage("You're next in the review queue!"); return; }
                player.SendMessage(position == 1 ? "There is " + position + " players in front of you!" : "There are " + position + " players in front of you!");
            }
        }

    }
}//TODO: Add player colors and all