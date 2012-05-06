using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.Events {
    /// <summary>
    /// PlayerConnection event class
    /// </summary>
    public class PlayerConnection:Event<Player,PlayerConnectionEventArgs> {
    }
    /// <summary>
    /// PlayConnectionEventArgs
    /// </summary>
    public class PlayerConnectionEventArgs : EventArgs, ICloneable {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="connected">Whether the player is connected or not</param>
        public PlayerConnectionEventArgs(bool connected) {
            this.Connected = connected;
        }
        /// <summary>
        /// Whether the player is connected or not
        /// </summary>
        public bool Connected;

        /// <summary>
        /// Returns a new instance representing this instance
        /// </summary>
        /// <returns>A new instance</returns>
        public override object Clone() {
            return new PlayerConnectionEventArgs(Connected);
        }
    }
}
