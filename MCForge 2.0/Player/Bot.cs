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
        public Bot()
        {
        }

        /// <summary>
        /// Handles bot AI
        /// </summary>
        public static void HandleBots()
        {
            foreach (Bot Bot in Server.Bots)
            {
                Random Random = new Random();
                //Bot.Player.Pos.x = (short)(Bot.Player.Pos.x + 14); //Around running speed of normal client, 16-18 for WoM
                Bot.Player.UpdatePosition(false);
            }
        }

    }
}
