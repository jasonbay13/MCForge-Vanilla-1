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
    public class MoveEvent : Event<Player, MoveEventArgs> {
    }
    /// <summary>
    /// PlayerMoveEventArgs
    /// </summary>
    public class MoveEventArgs : EventArgs {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="fromPosition">The position where the move started</param>
        public MoveEventArgs(Vector3 fromPosition) {
            this.FromPosition = fromPosition;
        }
        /// <summary>
        /// The position where the move started
        /// </summary>
        public Vector3 FromPosition { get; private set; }
    }
}
