/*
	Copyright 2011 MCForge
		
	Dual-licensed under the	Educational Community License, Version 2.0 and
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
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using System.Text;
using System.Linq;

namespace MCForge
{
    public class ZombieSurvival
    {
        public bool StartOnStartup;
        public static Random Random;
        public static System.Timers.Timer StartTimer;
        public static System.Timers.Timer EndTimer;
        public static Level ZombieLevel;
        public static int ElapsedRounds;
        public bool MoreThanTwoPlayers = true;

        // Constructors
        public ZombieSurvival()
        {
            Random = new Random();
            StartOnStartup = true;
            StartTimer = new System.Timers.Timer(1000);
            EndTimer = new System.Timers.Timer(1000);
        }

        public void Start(int x)
        {
            Server.ZombieMode = x;
            Start();
        }

        public void Start()
        {
            ZombieLevel = Level.Find("main");
            int loop = 6;
            StartTimer.Start(); StartTimer.Elapsed += delegate
            {
                if (loop != 0)
                {
                    int amountOfPlayers = Player.players.Count();
                    Player.players.ForEach(delegate(Player p)
                    {
                        if (p.referee)
                            amountOfPlayers--; 
                    });
                    if (amountOfPlayers >= 2)
                    {
                        MoreThanTwoPlayers = true;
                        loop--;
                        Player.GlobalMessage(c.gray + " - " + Server.DefaultColor + "Zombie Survival starts in " + loop + " seconds on level " +
                                             ZombieLevel.name + c.gray + " - ");
                    }
                    else
                    {
                        if (MoreThanTwoPlayers)
                            Player.GlobalMessage(c.gray + " - " + Server.DefaultColor + "Zombie Survival requires more than 2 non-referee players online to play" + c.gray + " - ");
                        MoreThanTwoPlayers = false;
                    }
                }
                else
                {
                    Server.ZombieRound = true;
                    Player.GlobalMessage(c.gray + " - " + "Zombie Survival has started on level " + ZombieLevel.name + "! Type /g " + ZombieLevel.name + " to join! "+ c.gray + " - ");
                    StartTimer.Stop();
                    Server.s.zombieTimer = new System.Timers.Timer((60000 * Random.Next(7, 11)) + 10);
                    Server.s.zombieTimer.Start();
                }
            };
        }

        public void EndRound()
        {
            int loop = 6;
            EndTimer.Start(); EndTimer.Elapsed += delegate
            {
                if (loop != 0)
                {
                    int amountOfPlayers = Player.players.Count();
                    Player.players.ForEach(delegate(Player p)
                    {
                        if (p.referee)
                            amountOfPlayers--; 
                    });
                    if (amountOfPlayers >= 2)
                    {
                        MoreThanTwoPlayers = true;
                        loop--;
                        Player.GlobalMessage(c.gray + " - " + Server.DefaultColor + "Zombie Survival ends in " + loop + " seconds" +
                                             ZombieLevel.name + c.gray + " - ");
                    }
                    else
                    {
                        loop = 0;
                    }
                }
                else
                {
                    ElapsedRounds++;
                    Server.ZombieRound = false;
                    Player.GlobalMessage(c.gray + " - " + "Zombie Survival has ended! "+ c.gray + " - ");
                    EndTimer.Stop();
                    Server.s.zombieTimer.Stop();
                    ChooseLevel();
                    Start();
                }
            };
        }

        public void ChooseLevel()
        {
            ArrayList al = new ArrayList();
            DirectoryInfo di = new DirectoryInfo("levels/");
            FileInfo[] fi = di.GetFiles("*.lvl");
            foreach (FileInfo fil in fi)
            {
                al.Add(fil.Name.Split('.')[0]);
            }
            if (al.Count <= 2) { Server.s.Log("You must have more than 2 levels to choose levels in Zombie Survival! Choosing last level!"); return; }
            int x = 0; string level = "";
            x = Random.Next(0, al.Count);
            level = al[x].ToString();
            ZombieLevel = Level.Find(level);
        }

        public void CheckLocation(Player p)
        {
            Player.players.ForEach(delegate(Player p2)
            {

            });
        }
}
