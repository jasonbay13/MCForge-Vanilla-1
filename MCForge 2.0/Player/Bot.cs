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
        bool Movement = false;
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
        }

        /// <summary>
        /// Handles bot AI
        /// </summary>
        public static void HandleBots()
        {
            foreach (Bot Bot in Server.Bots)
            {
                Random Random = new Random();
                if (Bot.Movement && Bot.FollowPlayers)
                {
                    Bot.Player.Pos.x = (short)(Bot.Player.Pos.x + 14); //Around running speed of normal client, 16-18 for WoM
                    Bot.Player.UpdatePosition(false);
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
