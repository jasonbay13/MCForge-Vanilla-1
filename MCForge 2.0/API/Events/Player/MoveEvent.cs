using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Utils;

namespace MCForge.API.Events {
    /// <summary>
    /// PlayerMove event class
    /// </summary>
    public class MoveEvent : Event<Player, MoveEventArgs> {
    }
    /// <summary>
    /// PlayerMoveEventArgs
    /// </summary>
    public class MoveEventArgs : EventArgs, ICancelable {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="fromPosition">The position where the move started</param>
        public MoveEventArgs(Vector3S fromPosition, Vector3S toPosition) {
            this.FromPosition = fromPosition;
            this.ToPosition = toPosition;
        }
        /// <summary>
        /// The position where the move started
        /// </summary>
        public Vector3S FromPosition { get; private set; }
        public Vector3S ToPosition { get; private set; }
        private bool canceled = false;
        /// <summary>
        /// Whether or not the handling should be canceled
        /// </summary>
        public bool Canceled {
            get { return canceled; }
        }
        /// <summary>
        /// Cancels the handling
        /// </summary>
        public void Cancel() {
            canceled = true;
        }
        /// <summary>
        /// Allows the handling
        /// </summary>
        public void Allow() {
            canceled = false;
        }
    }
}
