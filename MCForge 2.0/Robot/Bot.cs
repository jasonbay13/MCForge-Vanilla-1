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
using MCForge.API.Events;
using MCForge.API.Events.Robot;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Utils;
using MCForge.Utils.Settings;
using MCForge.World;
using System.Collections;
using AStar;

namespace MCForge.Robot
{
    public sealed partial class Bot
    {
        public static TargetPlayerEvent OnBotTargetPlayer = new TargetPlayerEvent();

        public static MoveBotEvent OnBotMove = new MoveBotEvent();

        public bool FollowPlayers = false;
        public bool BreakBlocks = false;
        public bool Jumping = false;
        bool Movement = true;

        //AStar Variables
        public BotMap2D LevelMap;
        public List<Location> Waypoint = new List<Location>();
        public int shouldCheckAgainLoopInt = 1000;
        public int intLoop = 0;
        public bool shouldCheckAgain
        {
            get
            {
                if (shouldCheckAgainLoopInt > 999 || shouldCheckAgainLoopInt >= Waypoint.Count)
                {
                    shouldCheckAgainLoopInt = 0;
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Player which is assigned to the bot
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// A robot (entity) that appears in the world.
        /// </summary>
        public Bot(string Username, Vector3S Position, byte[] Rotation, Level level, bool FollowPlayers, bool BreakBlocks, bool Jumping)
        {
            Player = new Player();
            Player.IsLoggedIn = false;
            Player.DisplayName = Username;
            Player.IsBot = true;
            Player.Username = Username;
            Player.Pos = Position;
            Player.oldPos = new Vector3S(Position.x, Position.z, Position.y);
            Player.Rot = Rotation;
            Player.Level = level;
            Player.ID = FreeId();
            Server.Bots.Add(this);
            SpawnThisBotToOtherPlayers(this);
            Player.IsLoggedIn = true;
            this.FollowPlayers = FollowPlayers;
            this.BreakBlocks = BreakBlocks;
            this.Jumping = Jumping;
            LevelMap = new BotMap2D(level, Position.y);
        }

        /// <summary>
        /// Handles bot AI
        /// </summary>
        public static void HandleBots()
        {
            foreach (Bot Bot in Server.Bots.ToArray())
            {
                Random Random = new Random();
                if (Bot.Movement)
                {
                    Vector3S TemporaryLocation = new Vector3S(Bot.Player.Pos.x, Bot.Player.Pos.z, Bot.Player.Pos.y);
                    if (Bot.FollowPlayers)
                    {
                        #region Find Closest Player
                        bool HitAPlayer = false;
                        Vector3S ClosestLocation = Bot.Player.Level.Size * 32;
                        foreach (Player p in Server.Players)
                        {
                            if (p.Level == Bot.Player.Level)
                            {
                                TargetPlayerArgs eargs = new TargetPlayerArgs(p);
                                bool cancel = OnBotTargetPlayer.Call(Bot, eargs).Canceled;
                                if (!cancel)
                                {
                                    HitAPlayer = true;
                                    if (p.Pos - Bot.Player.Pos < ClosestLocation - Bot.Player.Pos)
                                    {
                                        ClosestLocation = new Vector3S(p.Pos);
                                    }
                                }
                            }
                        }
                        #endregion
                        if (HitAPlayer)
                        {
                            Vector3S TempLocation = new Vector3S(Bot.Player.Pos);
                            TemporaryLocation = new Vector3S(Bot.Player.Pos);

                            Vector3S Pathfound = new Vector3S(Bot.Player.Pos);

                            #region AStar

                            if (Bot.shouldCheckAgain)
                            {
                                Bot.Waypoint = Pathfind(Bot, ClosestLocation);
                                if (Bot.Waypoint == null) //Hit the player!
                                {
                                    Bot.Waypoint = new List<Location>();
                                    Bot.Waypoint.Add(new Location(Pathfound.x / 32, Pathfound.z / 32));
                                }
                            }
                            try
                            {
                                Pathfound.x = (short)(Bot.Waypoint[Bot.shouldCheckAgainLoopInt].X * 32);
                                Pathfound.z = (short)(Bot.Waypoint[Bot.shouldCheckAgainLoopInt].Y * 32);
                            }
                            catch { Bot.shouldCheckAgainLoopInt = 0; }

                            if (Bot.intLoop >= 1) //Slows down the bots so they arent insta-propogate, it slows them a bit too much though, need to fix
                            {                     //Also makes them a bit less accurate than instant, but much more accurate than Vector2D.Move()
                                Bot.intLoop = 0;
                                Bot.shouldCheckAgainLoopInt++;
                            }
                            else
                            {
                                Bot.intLoop += 1;
                            }

                            TemporaryLocation.x += (short)((Pathfound.x - TemporaryLocation.x) / 2);
                            TemporaryLocation.z += (short)((Pathfound.z - TemporaryLocation.z) / 2);
                            #endregion

                            Block Block1 = Bot.Player.Level.GetBlock(TemporaryLocation / 32);
                            Block Block2 = Bot.Player.Level.GetBlock((TemporaryLocation.x / 32), (TemporaryLocation.z / 32), (TemporaryLocation.y / 32) - 1);
                            Block BlockUnderneath = Bot.Player.Level.GetBlock((TemporaryLocation.x / 32), (TemporaryLocation.z / 32), (TemporaryLocation.y / 32) - 2);
                            Block BlockAbove = Bot.Player.Level.GetBlock((TemporaryLocation.x / 32), (TemporaryLocation.z / 32), (TemporaryLocation.y / 32) + 1);

                            Vector3S delta = new Vector3S((short)Math.Abs(ClosestLocation.x - TemporaryLocation.x),
                                (short)Math.Abs(ClosestLocation.z - TemporaryLocation.z),
                                (short)Math.Abs(ClosestLocation.y - TemporaryLocation.y));

                            if (Block.CanWalkThrough(BlockUnderneath) && Block.CanWalkThrough(Block2)
                                && !Block.CanEscalate(Block1) && !Block.CanEscalate(Block2))
                            {
                                TemporaryLocation.y -= 21;
                                Bot.shouldCheckAgainLoopInt = 1000;
                            }

                            if (Block.CanWalkThrough(Block1) && !Block.CanWalkThrough(Block2) && !Block.CanWalkThrough(BlockUnderneath))
                            {
                                TemporaryLocation.y += 21;
                                Bot.shouldCheckAgainLoopInt = 1000;
                            }
                            else if (Block.CanEscalate(Block1) && Block.CanEscalate(Block2))
                            {
                                TemporaryLocation.y += 21;
                                Bot.shouldCheckAgainLoopInt = 1000;
                            }
                            else if (Block.CanWalkThrough(BlockAbove) && !Block.CanWalkThrough(BlockUnderneath) && ClosestLocation.y > TemporaryLocation.y)
                            {
                                TemporaryLocation.y += 21;
                                Bot.shouldCheckAgainLoopInt = 1000;
                                Bot.Player.Level.BlockChange((ushort)(TemporaryLocation.x / 32), (ushort)(TemporaryLocation.z / 32), (ushort)((TemporaryLocation.y / 32) - 2), 1);
                            }
                            else if (!Block.CanWalkThrough(BlockAbove) && !Block.CanWalkThrough(BlockUnderneath))
                            {
                                Bot.Player.Level.BlockChange((ushort)(TemporaryLocation.x / 32), (ushort)(TemporaryLocation.z / 32), (ushort)((TemporaryLocation.y / 32) + 1), 0);
                            }

                            MoveEventArgs eargs = new MoveEventArgs(TemporaryLocation, Bot.Player.Pos);
                            bool cancel = OnBotMove.Call(Bot, eargs).Canceled;
                            if (cancel)
                            {
                                TemporaryLocation = TempLocation;
                            }
                        }
                    }

                    if (TemporaryLocation.y != Bot.Player.Pos.y)
                    {
                        Bot.LevelMap = new BotMap2D(Bot.Player.Level, TemporaryLocation.y);
                    }
                    Bot.Player.Pos = TemporaryLocation;
                    Bot.Player.UpdatePosition(true); //Pls leave this true, bots dont appear properly otherwise
                }
            }
        }

        public static List<Location> Pathfind(Bot Bot, Vector3S ClosestLocation)
        {
            bool Calculate = true;

            RouteFinder routeFinder = new RouteFinder(0, 0, Bot.Player.Level.Size.x, Bot.Player.Level.Size.z, true);
            routeFinder.InitialLocation = new Location((Bot.Player.Pos.x / 32), (Bot.Player.Pos.z / 32));
            try
            {
                routeFinder.AddGoal(new Location((ClosestLocation.x / 32), (ClosestLocation.z / 32)));
            }
            catch { Calculate = false; }

            if (Calculate)
            {
                for (int x = 0; x < Bot.Player.Level.Size.x; x++)
                {
                    for (int z = 0; z < Bot.Player.Level.Size.z; z++)
                    {
                        Location loc = new Location(x, z);

                        if (Bot.LevelMap.GetMap(x, z) == -1)
                        {
                            routeFinder.AddObstacle(loc);
                            /*
                             * TODO - If his path is invalid... stop targeting that player.
                             * Sometimes the bot walks through walls... why?
                             * Change location (which is pretty close, but changing it causes issues) to Vector2S
                             * */
                        }
                    }
                }

                List<Location> LocationList = routeFinder.CalculateRoute();

                return routeFinder.CalculateRoute();
            }

            return null;
        }

        protected byte FreeId()
        {
            List<byte> usedIds = new List<byte>();

            Server.ForeachPlayer(p => usedIds.Add(p.ID));
            Server.ForeachBot(p => usedIds.Add(p.Player.ID));

            for (byte i = 1; i < ServerSettings.GetSettingInt("maxplayers"); ++i)
            {
                if (usedIds.Contains(i)) continue;
                return i;
            }

            Logger.Log("Too many players O_O");
            return 254;
        }

        protected void SpawnThisBotToOtherPlayers(Bot z)
        {
            Server.ForeachPlayer(delegate(Player p)
            {
                if (p != z.Player && p.Level == z.Player.Level)
                    p.SendSpawn(z.Player);
            });
        }

    }
}
