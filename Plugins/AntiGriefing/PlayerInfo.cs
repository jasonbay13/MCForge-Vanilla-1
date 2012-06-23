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
using MCForge.Interface.Plugin;
using MCForge.Entity;
using System.Net.Sockets;
using MCForge.API.Events;

namespace Plugins.AntiGriefingPlugin {
    public class PlayerInfo {

        /// <summary>
        /// The time the player last placed a block
        /// </summary>
        public DateTime LastPlace { get; set; }
        
        /// <summary>
        /// The last time a player sent a message
        /// </summary>
        public DateTime LastMessageSent { get; set; }
        
        /// <summary>
        /// The last message the player sent
        /// </summary>
        public string LastMessage { get; set; }
        
        /// <summary>
        /// Weather the player has been kicked for griefing
        /// </summary>
        public bool kicked { get; set; }
        
        /// <summary>
        /// Gets or sets the player griefing offenses (strikes)
        /// </summary>
        public int offense { 
        	get { return _strikes; } 
        	set {
        		if (value >= 0)
        		{
        			if (value > 5)
        			{
        				Player.Kick("You were caught griefing!");
        				kicked = true;
        			}
        			_strikes = value;
        		}
        	}
        }
        
        protected int _strikes;

        /// <summary>
        /// Gets or sets the last block.
        /// </summary>
        /// <value>
        /// The last block.
        /// </value>
        public byte LastBlock { get; set; }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public Player Player { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerInfo"/> class.
        /// </summary>
        /// <param name="p">The player.</param>
        public PlayerInfo(Player p) {
            Player = p;
            LastMessage = "";
            LastBlock = 0;
            LastPlace = DateTime.Now;
        }
    }
}
