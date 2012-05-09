using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.Events {
    /// <summary>
    /// PlayerChat event class
    /// </summary>
    public class ChatEvent : Event<Player, ChatEventArgs> {
    }
    /// <summary>
    /// PlayerChatEventArgs
    /// </summary>
    public class ChatEventArgs : EventArgs, ICancelable {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="message">The message</param>
        public ChatEventArgs(string message) {
            this.Message = message;
        }
        /// <summary>
        /// The message
        /// </summary>
        public string Message { get; private set; }
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