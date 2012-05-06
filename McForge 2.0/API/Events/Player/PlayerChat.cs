using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.Events {
    /// <summary>
    /// PlayerChat event class
    /// </summary>
    public class PlayerChat :Event<Player,PlayerChatEventArgs> {
    }
    /// <summary>
    /// PlayerChatEventArgs
    /// </summary>
    public class PlayerChatEventArgs : EventArgs, ICloneable, ICancelable {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="message">The message</param>
        public PlayerChatEventArgs(string message) {
            this.Message = message;
        }
        /// <summary>
        /// The message
        /// </summary>
        public string Message;
        /// <summary>
        /// Returns a new instance representing this instance
        /// </summary>
        /// <returns>A new instance</returns>
        public override object Clone() {
            return new PlayerChatEventArgs(Message);
        }
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
