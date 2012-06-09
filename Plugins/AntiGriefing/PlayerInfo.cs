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
            LastBlock = 0;
            LastPlace = DateTime.Now;
        }
    }
}
