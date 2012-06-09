using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Plugin;
using MCForge.Entity;
using System.Net.Sockets;
using MCForge.API.Events;

namespace Plugins.AntiGriefingPlugin {
    public class PluginAntiGrief : IPlugin {
        List<PlayerInfo> players = new List<PlayerInfo>();
        const string caps = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
		const string nocaps = "abcdefghijklmnopqrstuvwxyz ";
        #region IPlugin Members

        public string Name {
            get { return "AntiGrief";  }
        }

        public string Author {
            get { return "headdetect and hypereddie10"; }
        }

        public int Version {
            get { return 1; }
        }

        public string CUD {
            get { throw new NotImplementedException(); }
        }

        public void OnLoad(string[] args) {
            Player.OnAllPlayersConnect.Normal += new MCForge.API.Events.Event<Player, MCForge.API.Events.ConnectionEventArgs>.EventHandler(OnAllPlayersConnect_Normal);
            Player.OnAllPlayersBlockChange.Normal += new Event<Player, BlockChangeEventArgs>.EventHandler(OnAllPlayersBlockChange_Normal);
            Player.OnAllPlayersChat.Normal += new Event<Player, ChatEventArgs>.EventHandler(OnAllPlayersChat_Normal);
        }

        void OnAllPlayersChat_Normal(Player p, ChatEventArgs args) {
        	#region Spam Check
        	if (hasPlayerInfo(p))
        	{
        		PlayerInfo pi = getPlayerInfo(p);
        		if (pi.LastMessage.ToLower() == args.Message.ToLower())
        			pi.offense++;
        		else
        		{
        			pi.LastMessage = args.Message;
        			pi.offense--;
        		}
        	}
        	#endregion
        	
        	#region Caps Check
        	int rage = 0;
			bool skip = false;
		goagain:
			string newmessage = "";
			string message = args.Message;
			for (int i = 0; i < message.Length; i++)
			{
				char c = message[i];
				if (caps.IndexOf(c) != -1)
					rage++;
				else
					rage--;
				if (rage >= 5 && caps.IndexOf(c) != -1)
					c = nocaps[caps.IndexOf(c)];
				newmessage += c;
			}
			if (rage == message.Length)
			{
				skip = true;
				Player.SendMessage(p, "Lay off the caps :/");
				goto goagain;
			}
			else if (rage >= 7 && !skip)
			{
				Player.SendMessage(p, "Lay off the caps :/");
				if (!hasPlayerInfo(p))
				{
					PlayerInfo pi = new PlayerInfo(p);
					pi.LastMessageSent = DateTime.Now;
					pi.LastMessage = message;
					players.Add(pi);
				}
				else
				{
					PlayerInfo pi = getPlayerInfo(p);
					pi.offense++;
				}
			}
			args.Message = newmessage;
			#endregion
			
			if (hasPlayerInfo(p) && getPlayerInfo(p).kicked)
				args.Cancel();
        }

        void OnAllPlayersBlockChange_Normal(Player sender, BlockChangeEventArgs args) {
        	if (!hasPlayerInfo(sender))
        	{
        		players.Add(new PlayerInfo(sender));
        		return;
        	}
        	PlayerInfo pi = getPlayerInfo(sender);
        	
        }

        void OnAllPlayersConnect_Normal(Player sender, ConnectionEventArgs e) {
            if (!e.Connected)
                return;

            if (hasPlayerInfo(sender))
            	return;

            players.Add(new PlayerInfo(sender));
        }
        
        public PlayerInfo getPlayerInfo(Player p) {
        	PlayerInfo toreturn = null;
        	players.ForEach(pi => {
        	                	if (pi.Player == p)
        	                		toreturn = pi;
        	                });
        	return toreturn;
        }
        
        public bool hasPlayerInfo(Player p)
        {
        	return getPlayerInfo(p) != null;
        }

        public void OnUnload() {
            players.Clear();

            Player.OnAllPlayersChat.Normal -= OnAllPlayersChat_Normal;
            Player.OnAllPlayersConnect.Normal -= OnAllPlayersConnect_Normal;
            Player.OnAllPlayersBlockChange.Normal -= OnAllPlayersBlockChange_Normal;

        }

        #endregion
    }
}