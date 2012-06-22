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

namespace MCForge.Entity
{
   public class WOM
    {
       /// <summary>
       /// When a player joins using the WOM client (World of Minecraft)
       /// </summary>
        /// <param name="Username">Players Username</param>
       public static void SendJoin(string Username)
       {
           foreach (Player p in Server.Players)
           {
               if (p.UsingWom)
               {
                   p.SendMessage("^detail.user.join=%e" + Username);
               }
           }
       }

       /// <summary>
       /// When a player leaves using the WOM client (World of Minecraft)
       /// </summary>
       /// <param name="name"></param>
       public static void SendLeave(string Username)
       {
           foreach (Player p in Server.Players)
           {
               if (p.UsingWom)
               {
                   p.SendMessage("^detail.user.part=%e" + Username);
               }
           }
       }

       /// <summary>
       /// Send initial login message to a player using the WOM client (World of Minecraft)
       /// </summary>
       /// <param name="p"></param>
       public static void SendDetail(Player p)
       {
           p.SendMessage("^detail.user=%aWelcome to MCForge 6.0");
       }

       /// <summary>
       /// Send initial login message to a player using the WOM client (World of Minecraft)
       /// </summary>
       /// <param name="p"></param>
       public static void SendDetail(Player p, string message)
       {
           p.SendMessage("^detail.user=%e" + message);
       }
       /// <summary>
       /// Sends a message to a client
       /// </summary>
       /// <param name="message"></param>
       public static void NotifyClient(string message)
       {
           foreach (Player p in Server.Players)
           {
               if (p.UsingWom)
               {
                   p.SendMessage("^detail.user.alert=" + message);
               }
           }
       }
    }
}
