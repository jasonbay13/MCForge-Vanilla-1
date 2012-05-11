using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.Events {
    /// <summary>
    /// PlayerConnection event class
    /// </summary>
    public class ConnectionEvent : Event<Player, ConnectionEventArgs> {
    }
    /// <summary>
    /// PlayConnectionEventArgs
    /// </summary>
    public class ConnectionEventArgs : EventArgs, ICancelable {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="connected">Whether the player is connected or not</param>
        public ConnectionEventArgs(bool connected) {
            this.Connected = connected;
        }
        /// <summary>
        /// Whether the player is connected or not
        /// </summary>
        public bool Connected { get; private set; }
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
