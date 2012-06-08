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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;
using MCForge.World;

namespace MCForge.Games
{
    public static class Countdown
    {
        public enum CountdownGameStatus  { 
            Disabled, 
            Enabled,
            AboutToStart, 
            InProgress,
            Finished 
        }

        public static List<Player> players = new List<Player>();
        public static List<Player> playersleftlist = new List<Player>();
        public static List<string> squaresleft = new List<string>();
        public static Level mapon;
        public static int playersleft;
        public static int speed;
        public static bool freezemode = false;
        public static bool cancel = false;
        public static string speedtype;
        public static CountdownGameStatus gamestatus = CountdownGameStatus.Disabled;

        public static void GameStart(Player p)
        {
            switch (gamestatus)
            {
                case CountdownGameStatus.Disabled:
                    p.SendMessage("Please enable Countdown first!"); return;
                case CountdownGameStatus.AboutToStart:
                    p.SendMessage("The game is about to start!"); return;
                case CountdownGameStatus.InProgress:
                    p.SendMessage("The game is already in progress!"); return;
                case CountdownGameStatus.Finished:
                    p.SendMessage("The game has finished!"); return;
                case CountdownGameStatus.Enabled:
                    gamestatus = CountdownGameStatus.AboutToStart;
                    Thread.Sleep(2000);
                    break;
            }
            #region Blockchanges
            Block glass = Block.BlockList.GLASS;
            Block air = Block.BlockList.AIR;
            mapon.BlockChange(15, 16, 27, glass);
            mapon.BlockChange(16, 15, 27, glass);
            mapon.BlockChange(16, 16, 27, glass);
            mapon.BlockChange(15, 15, 27, glass);

            mapon.BlockChange(15, 14, 18, glass);
            mapon.BlockChange(16, 14, 18, glass);
            mapon.BlockChange(15, 14, 17, glass);
            mapon.BlockChange(16, 14, 17, glass);

            mapon.BlockChange(14, 15, 17, glass);
            mapon.BlockChange(14, 16, 18, glass);
            mapon.BlockChange(14, 16, 17, glass);
            mapon.BlockChange(14, 15, 18, glass);

            mapon.BlockChange(15, 17, 17, glass);
            mapon.BlockChange(16, 17, 18, glass);
            mapon.BlockChange(15, 17, 18, glass);
            mapon.BlockChange(16, 17, 17, glass);

            mapon.BlockChange(17, 16, 17, glass);
            mapon.BlockChange(17, 15, 18, glass);
            mapon.BlockChange(17, 16, 18, glass);
            mapon.BlockChange(17, 15, 17, glass);

            mapon.BlockChange(16, 15, 16, glass);
            mapon.BlockChange(15, 16, 16, glass);
            mapon.BlockChange(15, 15, 16, glass);
            mapon.BlockChange(16, 16, 16, glass);
            #endregion
            Player.LevelChat(mapon, "Countdown is about to start!");
            //TODO: perbuild to nobody
            ushort x1 = (ushort)((15.5) * 32);
            ushort y1 = (ushort)((30) * 32);
            ushort z1 = (ushort)((15.5) * 32);
            foreach (Player pl in Server.Players)  { 
                if (pl.Level != mapon) { 
                    Command.Find("goto").Use(pl, new string[1] { mapon.Name });
                    while (p.IsLoading) { Thread.Sleep(500); } pl.SendToPos(new Vector3(x1, z1, y1), new byte[] { 0, 0 });
                }          
            }
            squaresleft.Clear();
            PopulateSquaresLeft();
            Player.LevelChat(mapon, freezemode ? "Countdown starting with difficulty " + speedtype + " and mode freeze in:" : "Countdown starting with difficulty " + speedtype + " and mode normal in:");
            Thread.Sleep(2000);
            for (int i = 5; i >= 1; i--) {
                Player.LevelChat(mapon, "--&b" + i.ToString() + Server.DefaultColor + "--");
                if (i == 5) { mapon.BlockChange(16, 15, 16, air); mapon.BlockChange(15, 16, 16, air); mapon.BlockChange(15, 15, 16, air); mapon.BlockChange(16, 16, 16, air); }
                if (i == 3) { mapon.BlockChange(15, 16, 27, air); mapon.BlockChange(16, 15, 27, air); mapon.BlockChange(16, 16, 27, air); mapon.BlockChange(15, 15, 27, air); }
                Thread.Sleep(1000);
            }
            Player.LevelChat(mapon, "&bGO!!!");
            playersleft = players.Count();
            playersleftlist = players;
            foreach (Player play in players) { play.ExtraData.CreateIfNotExist("IsInCountdown", true); play.ExtraData["IsInCountdown"] = true; }
            AfterStart();
            Play();
        }

        public static void Play()
        {
            Block air = Block.BlockList.AIR;
            if (!freezemode)
            {
                while (squaresleft.Any() && playersleft != 0 && (gamestatus == CountdownGameStatus.InProgress || gamestatus == CountdownGameStatus.Finished))
                {
                    Random number = new Random();
                    int randnum = number.Next(squaresleft.Count);
                    string nextsquare = squaresleft.ElementAt(randnum);
                    squaresleft.Remove(nextsquare);
                    RemoveSquare(nextsquare);
                    if (squaresleft.Count % 10 == 0 && gamestatus != CountdownGameStatus.Finished) { Player.LevelChat(mapon, "There's " + squaresleft.Count + " squares left, and " + playersleft.ToString() + " players left!"); }
                    if (cancel == true) { End(null); }
                }
                return;
            }
            else
            {
                Player.LevelChat(mapon, "Welcome to Freeze Mode of coundown");
                Player.LevelChat(mapon, "You have 15 seconds to choose your square, after that you're stuck on that square!");
                Player.LevelChat(mapon, "Then the squares start dissapearing!");
                Thread.Sleep(500);
                for (int i = 15; i >= 1; i--) { Player.LevelChat(mapon, "---&b" + i.ToString() + Server.DefaultColor + "---"); Thread.Sleep(1000); }
                mapon.InCountdown = true;
                gamestatus = CountdownGameStatus.InProgress;
                //TODO: actually freeze players here, when somebody bothers adding /freeze
                ushort x3 = 5, z4 = 26, z3 = 5, x4 = 4;
                while (x3 <= 26) { while (z4 >= 4) { mapon.BlockChange(x3, z4, 4, air); z4--; } x3 += 3; }
                while (z3 <= 26) { while (x4 <= 26) { mapon.BlockChange(x4, z3, 4, air); x4++; } z3 += 3; }
                while (squaresleft.Any() && playersleft != 0 && (gamestatus == CountdownGameStatus.InProgress || gamestatus == CountdownGameStatus.Finished))
                {
                    Random number = new Random();
                    int randnum = number.Next(squaresleft.Count);
                    string nextsquare = squaresleft.ElementAt(randnum);
                    squaresleft.Remove(nextsquare);
                    RemoveSquare(nextsquare);
                    if (squaresleft.Count % 10 == 0 && gamestatus != CountdownGameStatus.Finished) { Player.LevelChat(mapon, "There's " + squaresleft.Count + " squares left, and " + playersleft.ToString() + " players left!"); }
                    if (cancel == true) { End(null); }
                }
            }
        }

        public static void End(Player winner)
        {
            gamestatus = CountdownGameStatus.Finished;
            squaresleft.Clear();
            if (winner != null) { winner.SendMessage("Congratulations! You won!"); Command.Find("spawn").Use(winner, null); winner.ExtraData["IsInCountdown"] = false; }
            playersleftlist.Clear();
            if (winner == null) { 
                foreach (Player pl in players) { pl.SendMessage("The game was canceled!"); Command.Find("spawn").Use(pl, null); }
                gamestatus = CountdownGameStatus.Enabled;
                playersleft = 0; 
                playersleftlist.Clear(); 
                players.Clear(); 
                //TODO: reset here
                cancel = false;
                return;
            }      
        }

        public static void Death(Player p) 
        {
            playersleft--;
            Player.LevelChat(mapon, p.ExtraData["Color"] + p.Username + Server.DefaultColor + " is out of the countdown!");
            p.ExtraData["IsInCountdown"] = false;
            playersleftlist.Remove(p);
            if (playersleft == 1) {
                Player left = playersleftlist.Last();
                Player.LevelChat(mapon, left.ExtraData["Color"] + left.Username + Server.DefaultColor + " is the winner!");
                End(left);
                return;
            }
        }

        public static void PlayerLeft(Player p)
        {
            playersleft--;
            Player.LevelChat(mapon, p.ExtraData["Color"] + p.Username + Server.DefaultColor + " disconnected, so he is out of the countdown!");
            p.ExtraData["IsInCountdown"] = false;
            playersleftlist.Remove(p);
            if (playersleft == 1) {
                Player left = playersleftlist.Last();
                Player.LevelChat(mapon, left.ExtraData["Color"] + left.Username + Server.DefaultColor + " is the winner!");
                End(left);
                return;
            }
        }

        public static void Reset(Player p, bool all)
        {
            Block air = Block.BlockList.AIR, glass = Block.BlockList.GLASS, green = Block.BlockList.GREEN_CLOTH;
            if (gamestatus == CountdownGameStatus.Enabled || gamestatus == CountdownGameStatus.Finished || gamestatus == CountdownGameStatus.Disabled) {
                if (all) {
                    gamestatus = CountdownGameStatus.Disabled;
                    playersleft = 0; playersleftlist.Clear();
                    squaresleft.Clear(); speed = 750;
                }
                #region BlockChanges
                mapon.BlockChange(15, 14, 18, air);
                mapon.BlockChange(16, 14, 18, air);
                mapon.BlockChange(15, 14, 17, air);
                mapon.BlockChange(16, 14, 17, air);

                mapon.BlockChange(14, 15, 17, air);
                mapon.BlockChange(14, 16, 18, air);
                mapon.BlockChange(14, 16, 17, air);
                mapon.BlockChange(14, 15, 18, air);

                mapon.BlockChange(15, 17, 17, air);
                mapon.BlockChange(16, 17, 18, air);
                mapon.BlockChange(15, 17, 18, air);
                mapon.BlockChange(16, 17, 17, air);

                mapon.BlockChange(17, 16, 17, air);
                mapon.BlockChange(17, 15, 18, air);
                mapon.BlockChange(17, 16, 18, air);
                mapon.BlockChange(17, 15, 17, air);

                mapon.BlockChange(16, 15, 16, air);
                mapon.BlockChange(15, 16, 16, air);
                mapon.BlockChange(15, 15, 16, air);
                mapon.BlockChange(16, 16, 16, air);

                {
                    ushort x1 = 27, x2 = 4, z1 = 27, z2 = 4, x3 = 5, z4 = 26, z3 = 5, x4 = 4;
                    while (x1 >= 4) { mapon.BlockChange(x1, 4, 4, glass); x1--; }
                    while (x2 <= 27) { mapon.BlockChange(x2, 27, 4, glass); x2++; }
                    while (z1 >= 4) { mapon.BlockChange(4, z1, 4, glass); z1--; }
                    while (z2 <= 27) { mapon.BlockChange(27, z2, 4, glass); z2++; }
                    while (x3 <= 26) { while (z4 >= 4) { mapon.BlockChange(x3, z4, 4, glass); z4--; } x3 += 3; }
                    while (z3 <= 26) { while (x4 <= 26) { mapon.BlockChange(x4, z3, 4, glass); x4++; } z3 += 3; }
                }
                PopulateSquaresLeft();
                while (squaresleft.Count > 0) {
                    Random number = new Random();
                    int randnum = number.Next(squaresleft.Count);
                    string nextsquare = squaresleft.ElementAt(randnum);
                    squaresleft.Remove(nextsquare);
                    int column = int.Parse(nextsquare.Split(':')[0]);
                    int row = int.Parse(nextsquare.Split(':')[1]);
                    ushort x1 = (ushort)(27 - (row * 3)), x2 = (ushort)(28 - (row * 3)), y = 4, z1 = (ushort)(27 - (column * 3)), z2 = (ushort)(28 - (column * 3));
                    mapon.BlockChange(x1, z1, y, green); mapon.BlockChange(x2, z1, y, green); mapon.BlockChange(x2, z2, y, green); mapon.BlockChange(x1, z2, y, green);
                }
                #endregion
                if (!all) {
                    p.SendMessage("The Countdown map has been reset!"); if (gamestatus == CountdownGameStatus.Finished) { p.SendMessage("You don't need to re-enable it!"); }
                    gamestatus = CountdownGameStatus.Enabled;
                    foreach (Player pl in Server.Players) {
                        if ((bool)pl.ExtraData["IsInCountdown"]) {
                            if (pl.Level == mapon) {
                                Command.Find("coundown").Use(pl, new string[1] { "join" });
                                pl.SendMessage("You've rejoined the countdown!");
                            }
                            else {
                                pl.SendMessage("You've been removed from the countdown because you aren't on the map!");
                                players.Remove(pl); pl.ExtraData["IsInCountdown"] = false;
                            }
                        }
                    }
                }
                else if (all && p != null) {
                        p.SendMessage("Countdown has been reset!"); if (gamestatus == CountdownGameStatus.Finished) { p.SendMessage("You do not need to re-enable it"); }
                        gamestatus = CountdownGameStatus.Enabled;
                        playersleft = 0; playersleftlist.Clear(); players.Clear();
                        foreach (Player pl in Server.Players) { pl.ExtraData["IsInCountdown"] = false; }
                }
                return;
            }
            else {
                if (p == null) { return; }
                if (gamestatus == CountdownGameStatus.Disabled) { p.SendMessage("Please enable the game first!"); }
                else { p.SendMessage("Please wait till the end of the game!"); }
            }
        }

        public static void PopulateSquaresLeft()
        {
            int column = 1, row = 1;
            while (column <= 7) { row = 1;
                while (row <= 7) { squaresleft.Add(column.ToString() + ":" + row.ToString()); row++; }
                column++;
            }
        }

        public static void RemoveSquare(string square)
        {
            Block yellow = Block.BlockList.YELLOW_CLOTH, orange = Block.BlockList.ORANGE_CLOTH, red = Block.BlockList.RED_CLOTH, air = Block.BlockList.AIR;

            int column = int.Parse(square.Split(':')[0]);
            int row = int.Parse(square.Split(':')[1]);
            ushort x1 = (ushort)(27 - (row * 3));
            ushort x2 = (ushort)(28 - (row * 3));
            ushort y = 4;
            ushort z1 = (ushort)(27 - (column * 3));
            ushort z2 = (ushort)(28 - (column * 3));

            mapon.BlockChange(x1, z1, y, yellow);
            mapon.BlockChange(x2, z1, y, yellow);
            mapon.BlockChange(x2, z2, y, yellow);
            mapon.BlockChange(x1, z2, y, yellow);
            Thread.Sleep(speed);
            mapon.BlockChange(x1, z1, y, orange);
            mapon.BlockChange(x2, z1, y, orange);
            mapon.BlockChange(x2, z2, y, orange);
            mapon.BlockChange(x1, z2, y, orange);
            Thread.Sleep(speed);
            mapon.BlockChange(x1, z1, y, red);
            mapon.BlockChange(x2, z1, y, red);
            mapon.BlockChange(x2, z2, y, red);
            mapon.BlockChange(x1, z2, y, red);
            Thread.Sleep(speed);
            mapon.BlockChange(x1, z1, y, air);
            mapon.BlockChange(x2, z1, y, air);
            mapon.BlockChange(x2, z2, y, air);
            mapon.BlockChange(x1, z2, y, air);

            bool up = false, left = false, right = false, down = false;

            if (mapon.GetBlock(x1, (ushort)(z2 + 2), y) == air) {
                mapon.BlockChange(x1, (ushort)(z2 + 1), y, air); mapon.BlockChange(x2, (ushort)(z2 + 1), y, air); right = true;
            }
            if (mapon.GetBlock(x1, (ushort)(z1 - 2), y) == air) {
                mapon.BlockChange(x1, (ushort)(z1 - 1), y, air); mapon.BlockChange(x2, (ushort)(z1 - 1), y, air); left = true;
            }
            if (mapon.GetBlock((ushort)(x2 + 2), z1, y) == air) {
                mapon.BlockChange((ushort)(x2 + 1), z1, y, air); mapon.BlockChange((ushort)(x2 + 1), z2, y, air); up = true;
            }
            if (mapon.GetBlock((ushort)(x1 - 2), z1, y) == air) {
                mapon.BlockChange((ushort)(x1 - 1), z1, y, air); mapon.BlockChange((ushort)(x1 - 1), z2, y, air); down = true;
            }

            if (mapon.GetBlock((ushort)(x1 - 2), (ushort)(z1 - 2), y) == air && left == true && down == true) {
                mapon.BlockChange((ushort)(x1 - 1), (ushort)(z1 - 1), y, air);
            }
            if (mapon.GetBlock((ushort)(x1 - 2), (ushort)(z2 + 2), y) == air && right == true && down == true) {
                mapon.BlockChange((ushort)(x1 - 1), (ushort)(z2 + 1), y, air);
            }
            if (mapon.GetBlock((ushort)(x2 + 2), (ushort)(z1 - 2), y) == air && left == true && up == true) {
                mapon.BlockChange((ushort)(x2 + 1), (ushort)(z1 - 1), y, air);
            }
            if (mapon.GetBlock((ushort)(x2 + 2), (ushort)(z2 + 2), y) == air && right == true && up == true) {
                mapon.BlockChange((ushort)(x2 + 1), (ushort)(z2 + 1), y, air);
            }

        }

        public static void AfterStart()
        {
            Block air = Block.BlockList.AIR;
            Block glass = Block.BlockList.GLASS;

            mapon.BlockChange(15, 14, 18, air);
            mapon.BlockChange(16, 14, 18, air);
            mapon.BlockChange(15, 14, 17, air);
            mapon.BlockChange(16, 17, 14, air);

            mapon.BlockChange(14, 15, 17, air);
            mapon.BlockChange(14, 16, 18, air);
            mapon.BlockChange(14, 16, 17, air);
            mapon.BlockChange(14, 15, 18, air);

            mapon.BlockChange(15, 17, 17, air);
            mapon.BlockChange(16, 17, 18, air);
            mapon.BlockChange(15, 17, 18, air);
            mapon.BlockChange(16, 17, 17, air);

            mapon.BlockChange(17, 16, 17, air);
            mapon.BlockChange(17, 15, 18, air);
            mapon.BlockChange(17, 16, 18, air);
            mapon.BlockChange(17, 15, 17, air);

            mapon.BlockChange(16, 15, 16, glass);
            mapon.BlockChange(15, 16, 16, glass);
            mapon.BlockChange(15, 16, 16, glass);
            mapon.BlockChange(16, 16, 16, glass);

            ushort x1 = 27, x2 = 4, z1 = 27, z2 = 4;
            while (x1 >= 4) { mapon.BlockChange(x1, 4, 4, air); x1--; }
            while (x2 <= 27) { mapon.BlockChange(x2, 27, 4, air); x2++; }
            while (z1 >= 4) { mapon.BlockChange(4, z1, 4, air); z1--; }
            while (z2 <= 27) { mapon.BlockChange(27, z2, 4, air); z2++; }
            if (!freezemode) { mapon.InCountdown = true; gamestatus = CountdownGameStatus.InProgress; }
        }
    }
}