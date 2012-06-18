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
        public BreadCrumb Waypoint = null;
        public int WaypointAmount = 999;
        public int shouldCheckAgainLoopInt = 1000;
        public int intLoop = 0;
        public BotMap LevelMap;
        public Dictionary<string, int> BlackListPlayers;

        public bool shouldCheckAgain
        {
            get
            {
                if (shouldCheckAgainLoopInt > 999 || shouldCheckAgainLoopInt == WaypointAmount)
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
            this.LevelMap = new BotMap(level);
            this.BlackListPlayers = new Dictionary<string, int>();
            Player.OnAllPlayersBlockChange.Important += OnBlockChange;
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
                    string PlayerName = "";
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
                                    if (Bot.BlackListPlayers.ContainsKey(p.Username))
                                    {
                                        if (Math.Abs(Bot.BlackListPlayers[p.Username] - Bot.shouldCheckAgainLoopInt) > 100)
                                        {
                                            Bot.BlackListPlayers.Remove(p.Username);
                                        }
                                    }
                                    if (p.Pos - Bot.Player.Pos < ClosestLocation - Bot.Player.Pos && !Bot.BlackListPlayers.ContainsKey(p.Username))
                                    {
                                        HitAPlayer = true;
                                        ClosestLocation = new Vector3S(p.Pos);
                                        PlayerName = p.Username;
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

                            if (Bot.shouldCheckAgain || Bot.Waypoint == null)
                            {
                                Bot.Waypoint = Pathfind(Bot, ClosestLocation);
                            }
                            try
                            {
                                Pathfound.x = (short)(Bot.Waypoint.position.X * 32);
                                Pathfound.z = (short)(Bot.Waypoint.position.Z * 32);
                                Pathfound.y = (short)(Bot.Waypoint.position.Y * 32);
                            }
                            catch
                            {
                                Bot.shouldCheckAgainLoopInt = 0;
                                try
                                {
                                    Bot.BlackListPlayers.Add(PlayerName, Bot.shouldCheckAgainLoopInt);
                                }
                                catch { }
                                break;
                            }

                            if (Bot.intLoop >= 1) //Slows down the bots so they arent insta-propogate, it slows them a bit too much though, need to fix
                            {                     //Also makes them a bit less accurate than instant, but much more accurate than Vector2D.Move()
                                Bot.intLoop = 0;
                                Bot.shouldCheckAgainLoopInt++;
                                Bot.Waypoint = Bot.Waypoint.next;
                            }
                            else
                            {
                                Bot.intLoop += 1;
                            }

                            TemporaryLocation.x += (short)((Pathfound.x - TemporaryLocation.x) / 2);
                            TemporaryLocation.z += (short)((Pathfound.z - TemporaryLocation.z) / 2);
                            //TemporaryLocation.y += (short)((Pathfound.y - TemporaryLocation.y) / 2);
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
                            else if (Block.CanEscalate(Block1) && Block.CanEscalate(Block2) && Pathfound.y > TemporaryLocation.y)
                            {
                                TemporaryLocation.y += 21;
                                Bot.shouldCheckAgainLoopInt = 1000;
                            }
                            else if (Block.CanWalkThrough(BlockAbove) && !Block.CanWalkThrough(BlockUnderneath) && Pathfound.y > TemporaryLocation.y && !Block.IsOPBlock(BlockUnderneath))
                            {
                                TemporaryLocation.y += 21;
                                Bot.shouldCheckAgainLoopInt = 1000;
                                Bot.Player.Level.BlockChange((ushort)(TemporaryLocation.x / 32), (ushort)(TemporaryLocation.z / 32), (ushort)((TemporaryLocation.y / 32) - 2), 1);
                            }
                            else if (!Block.CanWalkThrough(BlockAbove) && !Block.CanWalkThrough(BlockUnderneath) && !Block.IsOPBlock(BlockAbove)) 
                            {
                                Bot.Player.Level.BlockChange((ushort)(TemporaryLocation.x / 32), (ushort)(TemporaryLocation.z / 32), (ushort)((TemporaryLocation.y / 32) + 1), 0);
                            }

                            if (Block.CanWalkThrough(BlockUnderneath) && !Block.CanWalkThrough(Bot.Player.Level.GetBlock((Bot.Player.oldPos.x / 32), (Bot.Player.oldPos.z / 32), (Bot.Player.oldPos.y / 32) - 2))
                                && !Block.IsOPBlock(BlockUnderneath) && Pathfound.y > TemporaryLocation.y)
                            {
                                Bot.Player.Level.BlockChange((ushort)(TemporaryLocation.x / 32), (ushort)(TemporaryLocation.z / 32), (ushort)((TemporaryLocation.y / 32) - 2), 1);
                            }

                            MoveEventArgs eargs = new MoveEventArgs(TemporaryLocation, Bot.Player.Pos);
                            bool cancel = OnBotMove.Call(Bot, eargs).Canceled;
                            if (cancel)
                            {
                                TemporaryLocation = TempLocation;
                            }
                        }
                        else
                        {
                            Bot.shouldCheckAgainLoopInt += 1;
                        }
                    }

                    Bot.Player.Pos = TemporaryLocation;
                    Bot.Player.UpdatePosition(true); //Pls leave this true, bots dont appear properly otherwise
                }
            }
        }

        public static BreadCrumb Pathfind(Bot Bot, Vector3S ClosestLocation)
        {
            return PathFinder.FindPath(Bot.Player.Level, Bot.LevelMap, new Point3D((Bot.Player.Pos.x / 32), (Bot.Player.Pos.y / 32), (Bot.Player.Pos.z / 32)),
                new Point3D((ClosestLocation.x / 32), (ClosestLocation.y / 32), (ClosestLocation.z / 32)));
        }

        public void OnBlockChange(Player p, BlockChangeEventArgs args)
        {
            if (args.Action == ActionType.Place)
            {
                LevelMap.AirMap[args.X, args.Z, args.Y] = false;
            }
            else if (args.Action == ActionType.Delete)
            {
                LevelMap.AirMap[args.X, args.Z, args.Y] = true;
            }
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
