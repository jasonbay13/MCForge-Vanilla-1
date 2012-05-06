using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;

namespace MCForge.API.Events {
    /// <summary>
    /// PlayerMove event class
    /// </summary>
    public class PlayerMove:Event<Player, PlayerMoveEventArgs> {
    }
    /// <summary>
    /// PlayerMoveEventArgs
    /// </summary>
    public class PlayerMoveEventArgs : EventArgs, ICloneable {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="fromPosition">The position where the move started</param>
        public PlayerMoveEventArgs(Vector3 fromPosition) {
            this.FromPosition = fromPosition;
        }
        /// <summary>
        /// The position where the move started
        /// </summary>
        public Vector3 FromPosition;
        /// <summary>
        /// Returns a new instance representing this instance
        /// </summary>
        /// <returns>A new instance</returns>
        public override object Clone() {
            return new PlayerMoveEventArgs(FromPosition);
        }
    }

}
