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
