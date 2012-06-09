using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Plugin;
using MCForge.Entity;

namespace Plugins.AntiGriefingPlugin {
    public class PluginAntiGrief : IPlugin {
		List<IPlayer> players = new List<IPlayer>();
		
        #region IPlugin Members
        
        public string Name {
            get { throw new NotImplementedException(); }
        }

        public string Author {
            get { throw new NotImplementedException(); }
        }

        public int Version {
            get { throw new NotImplementedException(); }
        }

        public string CUD {
            get { throw new NotImplementedException(); }
        }

        public void OnLoad(string[] args) {
            Player.OnAllPlayersConnect.Normal += new EventHandler(Player_OnAllPlayersConnect_Normal);
        }

        void Player_OnAllPlayersConnect_Normal(object sender, EventArgs e)
        {
        	if (!players.Contains((Player)sender))
        		players.Add((IPlayer)sender);
        }

        public void OnUnload() {
        	players.Clear();
        }

        #endregion
    }
	
	public class IPlayer : Player {
		public DateTime lastPlace = DateTime.Now;
		public IPlayer() : base() { }
		public IPlayer(TcpClient TcpClient) : base (TcpClient) { }
	}
}
