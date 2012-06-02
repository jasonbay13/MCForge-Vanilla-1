using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;

namespace MCForge
{
   public class WOM
    {
       public static void sendjoin(string name)
       {
           foreach (Player p in Server.Players)
           {
               if (p.usingwom)
               {
                   p.SendMessage("^detail.user.join=%e" + name);
               }
           }
       }
       public static void sendleave(string name)
       {
           foreach (Player p in Server.Players)
           {
               if (p.usingwom)
               {
                   p.SendMessage("^detail.user.part=%e" + name);
               }
           }
       }
       public static void senddetail(Player p)
       {
           p.SendMessage("^detail.user=%aWelcome to MCForge 6.0");
       }
       public static void notify(string message)
       {
           foreach (Player p in Server.Players)
           {
               if (p.usingwom)
               {
                   p.SendMessage("^detail.user.alert=" + message);
               }
           }
       }
    }
}
