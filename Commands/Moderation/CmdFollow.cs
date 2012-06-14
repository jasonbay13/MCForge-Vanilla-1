using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Groups;
using MCForge.Entity;
using MCForge.Utils;
using MCForge.Core;

namespace MCForge.Commands.Moderation {
   public class CmdFollow : ICommand {
       #region ICommand Members

       public string Name {
           get { return "Follow"; }
       }

       public CommandTypes Type {
           get { return CommandTypes.Mod; }
       }

       public string Author {
           get { return "headdetect"; }
       }

       public int Version {
           get { return 1; }
       }

       public string CUD {
           get { throw new NotImplementedException(); }
       }

       public byte Permission {
           get { return (byte)PermissionLevel.Operator;  }
       }

       public void Use(MCForge.Entity.Player p, string[] args) {

           if (p.GetType() == typeof(ConsolePlayer)) {
               p.SendMessage("This can only be used in game");
               return;
           }

           if (args.Length < 1) {
               if(p.ExtraData.ContainsKey("FollowData")){
                   p.ExtraData.Remove("FollowData");
                   p.SendMessage("Stopped Following");
                   p.IsHidden = !p.IsHidden;
                   if (!p.IsHidden)
                       p.SpawnThisPlayerToOtherPlayers();
                   return;
               }
               Help(p);
               return;
           }
           var who = Player.Find(args[0]);

           if (who == null || who.GetType() == typeof(ConsolePlayer)) {
               p.SendMessage("The player you want to follow doesn't exist");
               return;
           }

           p.SendMessage("You are now following " + who.Color + who.Username);
           p.ExtraData.ChangeOrCreate<object, object>("FollowData", who.Username);
           p.GlobalDie();
           p.IsHidden = !p.IsHidden;
           if (!p.IsHidden)
               p.SpawnThisPlayerToOtherPlayers();
           who.OnPlayerMove.Normal += new MCForge.API.Events.Event<Player, MCForge.API.Events.MoveEventArgs>.EventHandler(OnPlayerMove);
       }

       void OnPlayerMove(Player sender, MCForge.API.Events.MoveEventArgs args) {
           foreach (Player p in Server.Players.ToArray())
           {
               if(p.ExtraData.ContainsValue(sender.Username))
               {
                   p.Pos = args.ToPosition;
                   p.Rot = sender.Rot;
                   p.SendThisPlayerTheirOwnPos();
               }
           }
       }

       public void Help(MCForge.Entity.Player p) {
           p.SendMessage("/follow <name> - follows a user, great if you have your suspicions");
           p.SendMessage("/follow - to stop following a player (if you are already following one)");
       }

       public void Initialize() {
           Command.AddReference(this, "follow");
       }

       #endregion
   }
}
