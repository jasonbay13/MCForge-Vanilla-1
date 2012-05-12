using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using MCForge.Core;
using MCForge.Groups;
using MCForge.Interface.Command;
using MCForge.Utilities.Settings;
using MCForge.World;
using MCForge.Utilities;
using MCForge.Utils;
using System.Linq;
using MCForge.SQL;
using System.Data;
using MCForge.Entity;
using MCForge.API.Events;

namespace MCForge.Robot
{
    public sealed partial class Bot
    {
        public bool FollowPlayers = false;

        ushort[] FoundPlayerPosition = new ushort[3] { 0, 0, 0 };
        byte[] FoundPlayerRotation = new byte[2] { 0, 0 };
        bool Movement = true;
        public int Speed = 24;
        bool Jumping = false;
        int CurrentJump = 0;

        /// <summary>
        /// Player which is assigned to the bot
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// A robot (entity) that appears in the world.
        /// </summary>
        public Bot(string Username, Vector3 Position, byte[] Rotation, Level level, bool FollowPlayers)
        {
            Player = new Player();
            Player.Username = Username;
            Player.Pos.x = Position.x;
            Player.Pos.y = Position.y;
            Player.Pos.z = Position.z;
            Player.Rot = Rotation;
            Player.Level = level;
            Player.id = FreeId();
            Server.Bots.Add(this);
            SpawnThisBotToOtherPlayers(this);
            this.FollowPlayers = FollowPlayers;
        }

        /// <summary>
        /// Handles bot AI
        /// </summary>
        public static void HandleBots()
        {
            foreach (Bot Bot in Server.Bots)
            {
                if (Bot.Movement && Bot.FollowPlayers)
                {
                    #region Find Closest Player
                    bool HitAPlayer = false;
                    Vector3 ClosestLocation = new Vector3(0, 0, 0);
                    foreach (Player p in Server.Players)
                    {
                        if (p.Level == Bot.Player.Level)
                        {
                            HitAPlayer = true;
                            if (Vector3.MinusAbs(p.Pos, Bot.Player.Pos) > ClosestLocation)
                            {
                                ClosestLocation = p.Pos;
                            }
                        }
                    }
                    #endregion
                    Vector3 TemporaryLocation = new Vector3(0, 0, 0);
                    if (HitAPlayer)
                    {
                        if (ClosestLocation.x < Bot.Player.Pos.x)
                            TemporaryLocation.x = (short)(Bot.Player.Pos.x - 13); //Around running speed of normal client, 16-18 for WoM
                        else if (ClosestLocation.x >= Bot.Player.Pos.x)
                            TemporaryLocation.x = (short)(Bot.Player.Pos.x + 13);
                        if (ClosestLocation.z < Bot.Player.Pos.z)
                            TemporaryLocation.z = (short)(Bot.Player.Pos.z - 13);
                        else if (ClosestLocation.z >= Bot.Player.Pos.z)
                            TemporaryLocation.z = (short)(Bot.Player.Pos.z + 13);

                        TemporaryLocation.y = Bot.Player.Pos.y;
                        if (Bot.Player.Level.GetBlock(Vector3.MinusY(TemporaryLocation, 64) / 32) == Block.BlockList.AIR ||
                                Bot.Player.Level.GetBlock(Vector3.MinusY(TemporaryLocation, 64) / 32) == Block.BlockList.WATER ||
                                Bot.Player.Level.GetBlock(Vector3.MinusY(TemporaryLocation, 64) / 32) == Block.BlockList.LAVA ||
                                Bot.Player.Level.GetBlock(Vector3.MinusY(TemporaryLocation, 64) / 32) == Block.BlockList.ACTIVE_LAVA ||
                                Bot.Player.Level.GetBlock(Vector3.MinusY(TemporaryLocation, 64) / 32) == Block.BlockList.ACTIVE_WATER)
                            TemporaryLocation.y = (short)(Bot.Player.Pos.y - 21); //Gravity, 21 is a nice value, doesn't float too much and doesnt fall too far.

                        if (
                                (Bot.Player.Level.GetBlock(TemporaryLocation / 32) == Block.BlockList.AIR ||
                                Bot.Player.Level.GetBlock(TemporaryLocation / 32) == Block.BlockList.WATER ||
                                Bot.Player.Level.GetBlock(TemporaryLocation / 32) == Block.BlockList.LAVA ||
                                Bot.Player.Level.GetBlock(TemporaryLocation / 32) == Block.BlockList.ACTIVE_LAVA ||
                                Bot.Player.Level.GetBlock(TemporaryLocation / 32) == Block.BlockList.ACTIVE_WATER) 
                            &&
                                (Bot.Player.Level.GetBlock(Vector3.MinusY(TemporaryLocation, 32) / 32) == Block.BlockList.AIR ||
                                Bot.Player.Level.GetBlock(Vector3.MinusY(TemporaryLocation, 32) / 32) == Block.BlockList.WATER ||
                                Bot.Player.Level.GetBlock(Vector3.MinusY(TemporaryLocation, 32) / 32) == Block.BlockList.LAVA ||
                                Bot.Player.Level.GetBlock(Vector3.MinusY(TemporaryLocation, 32) / 32) == Block.BlockList.ACTIVE_LAVA ||
                                Bot.Player.Level.GetBlock(Vector3.MinusY(TemporaryLocation, 32) / 32) == Block.BlockList.ACTIVE_WATER)
                            )
                            Bot.Player.Pos = TemporaryLocation; //Make sure the bot doesnt walk through walls

                        Bot.Player.UpdatePosition(false);
                    }
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
