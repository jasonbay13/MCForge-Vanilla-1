/*
Copyright 2012 MCForge
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
using System.Text;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Utils;

namespace Plugins.WoMPlugin
{
    public class WOM
    {
        /// <summary>
        /// When a player joins using the WOM client (World of Minecraft)
        /// </summary>
        /// <param name="Username">Players Username</param>
        public static void GlobalSendJoin(string Username)
        {
            Server.ForeachPlayer(p =>
            {
                if ((bool)(p.ExtraData.GetIfExist<object, object>("UsingWoM") ?? false))
                {
                    p.SendMessage("^detail.user.join=%e" + Username);
                }
            });
        }

        /// <summary>
        /// When a player leaves using the WOM client (World of Minecraft)
        /// </summary>
        /// <param name="name"></param>
        public static void GlobalSendLeave(string Username)
        {
            Server.ForeachPlayer(p =>
            {
                if ((bool)(p.ExtraData.GetIfExist<object, object>("UsingWoM") ?? false))
                {
                    p.SendMessage("^detail.user.part=%e" + Username);
                }
            });
        }

        /// <summary>
        /// Sends the specified player the specified detail.
        /// </summary>
        /// <param name="p">Player to send to.</param>
        /// <param name="message">The message to send.</param>
        public static void SendDetail(Player p, string message)
        {
            if ((bool)(p.ExtraData.GetIfExist<object, object>("UsingWoM") ?? false))
            {
                p.SendMessage("^detail.user=%e" + message);
            }
        }
        /// <summary>
        /// Sends the specified player the specified detail.
        /// </summary>
        /// <param name="p">Player to send to.</param>
        /// <param name="message">The message to send.</param>
        public static void GlobalSendDetail(string message)
        {
            Server.ForeachPlayer(p =>
            {
                if ((bool)(p.ExtraData.GetIfExist<object, object>("UsingWoM") ?? false))
                {
                    p.SendMessage("^detail.user=%e" + message);
                }
            });
        }
        /// <summary>
        /// Sends an alert message to all clients.
        /// </summary>
        /// <param name="message"></param>
        public static void GlobalSendAlert(string message)
        {
            Server.ForeachPlayer(p =>
            {
                if ((bool)(p.ExtraData.GetIfExist<object, object>("UsingWoM") ?? false))
                {
                    p.SendMessage("^detail.user.alert=" + message);
                }
            });
        }
        /// <summary>
        /// Sends an alert message to the specified client.
        /// </summary>
        /// <param name="p">Player to send to.</param>
        /// <param name="message">The message.</param>
        public static void SendAlert(Player p, string message)
        {
                if ((bool)(p.ExtraData.GetIfExist<object, object>("UsingWoM") ?? false))
                {
                    p.SendMessage("^detail.user.alert=" + message);
                }
        }
    }
}
