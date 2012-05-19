using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using MCForge.Core;
using MCForge.Groups;
using MCForge.Interface.Command;
using MCForge.Utils.Settings;
using MCForge.World;
using MCForge.Utils;
using MCForge.Utils;
using System.Linq;
using MCForge.SQL;
using System.Data;
using MCForge.Entity;
using MCForge.API.Events;

//This namespace should get it's own directory
namespace MCForge.Robot
{
    public sealed partial class Bot
    {
        public static TargetPlayerEvent OnBotTargetPlayer = new TargetPlayerEvent();

        public bool FollowPlayers = false;
        public bool BreakBlocks = false;
        public bool Jumping = false;
        bool Movement = true;

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
            Player.IsBot = true;
            Player.Username = Username;
            Player.Pos = Position;
            Player.oldPos = new Vector3S(Position.x, Position.z, Position.y);
            Player.Rot = Rotation;
            Player.Level = level;
            Player.id = FreeId();
            Server.Bots.Add(this);
            SpawnThisBotToOtherPlayers(this);
            this.FollowPlayers = FollowPlayers;
            this.BreakBlocks = BreakBlocks;
            this.Jumping = Jumping;
        }

        /// <summary>
        /// Handles bot AI
        /// </summary>
        public static void HandleBots()
        {
            foreach (Bot Bot in Server.Bots)
            {
                Random Random = new Random();
                bool PlayerBelow = false;
                if (Bot.Movement)
                {
                    Vector3S TemporaryLocation = new Vector3S(Bot.Player.Pos.x, Bot.Player.Pos.z, Bot.Player.Pos.y);
                    if (Bot.FollowPlayers) //TODO - Fix jumping (you can jump infinately), fix bot locking on target (locks on one target only)
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
                            TemporaryLocation = new Vector3S(Bot.Player.Pos);
                            TemporaryLocation.Move(13, ClosestLocation);
                        }
                    }

                    bool ShouldBreakBlock = true;

                    if (Block.CanWalkThrough(Bot.Player.Level.GetBlock(Vector3S.MinusY(TemporaryLocation, 64) / 32)) && Bot.Player.Pos.y / 32 > 1)
                        TemporaryLocation.y = (short)(Bot.Player.Pos.y - 21); //Gravity, 21 is a nice value, doesn't float too much and doesnt fall too far.

                    if (Block.CanWalkThrough(Bot.Player.Level.GetBlock(TemporaryLocation / 32)) &&
                        Block.CanWalkThrough(Bot.Player.Level.GetBlock(Vector3S.MinusY(TemporaryLocation, 32) / 32)))
                    {
                        Bot.Player.Pos = TemporaryLocation; //Make sure the bot doesnt walk through walls
                    }
                    else if (Bot.Jumping) //Jumping
                    {
                            if (Block.CanWalkThrough(Bot.Player.Level.GetBlock(TemporaryLocation / 32)) &&
                                Block.CanWalkThrough(Bot.Player.Level.GetBlock(Vector3S.MinusY(TemporaryLocation, -32) / 32)))
                            {
                            Bot.Player.Pos.y = (short)(Bot.Player.Pos.y + 21);
                            ShouldBreakBlock = false;
                            }
                    }
                    if (Bot.BreakBlocks && ShouldBreakBlock) //Can't go through dat wall, try and break it
                    {
                        if (Random.Next(1, 5) == 3 && !Block.IsOPBlock(Bot.Player.Level.GetBlock(TemporaryLocation / 32)))
                            Bot.Player.Level.BlockChange(Convert.ToUInt16(TemporaryLocation.x / 32), Convert.ToUInt16(TemporaryLocation.z / 32), Convert.ToUInt16(TemporaryLocation.y / 32), Block.BlockList.AIR);
                        if (Random.Next(1, 5) == 3 && !Block.IsOPBlock(Bot.Player.Level.GetBlock(new Vector3S(Convert.ToUInt16(TemporaryLocation.x / 32), Convert.ToUInt16(TemporaryLocation.z / 32), Convert.ToUInt16((TemporaryLocation.y - 32) / 32)))))
                            Bot.Player.Level.BlockChange(Convert.ToUInt16(TemporaryLocation.x / 32), Convert.ToUInt16(TemporaryLocation.z / 32), Convert.ToUInt16((TemporaryLocation.y - 32) / 32), Block.BlockList.AIR);
                        if (PlayerBelow)
                        {
                            try
                            {
                                if (Random.Next(1, 5) == 3 && !Block.IsOPBlock(Bot.Player.Level.GetBlock(new Vector3S(Convert.ToUInt16(TemporaryLocation.x / 32), Convert.ToUInt16(TemporaryLocation.z / 32), Convert.ToUInt16((TemporaryLocation.y - 64) / 32)))))
                                    Bot.Player.Level.BlockChange(Convert.ToUInt16(TemporaryLocation.x / 32), Convert.ToUInt16(TemporaryLocation.z / 32), Convert.ToUInt16((TemporaryLocation.y - 64) / 32), Block.BlockList.AIR);
                            }
                            catch { }
                        }
                    }

                    Bot.Player.UpdatePosition(true); //if false I get kicked.
                }
            }
        }

        protected byte FreeId()
        {
            List<byte> usedIds = new List<byte>();

            Server.ForeachPlayer(p => usedIds.Add(p.id));
            Server.ForeachBot(p => usedIds.Add(p.Player.id));

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
