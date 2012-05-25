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
    public class ChatEventArgs : EventArgs, ICancelable, ICloneable, IEquatable<ChatEventArgs> {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="username">The username</param>
        public ChatEventArgs(string message, string username) {
            this.Message = message;
            this.Username = username;
        }
        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the Username
        /// </summary>
        public string Username { get; set; }
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

        /// <summary>
        /// Compares equality (ICancelable and IStoppable are not part of the comparison)
        /// </summary>
        /// <param name="other">The value to be compared to.</param>
        /// <returns>Whether they are equal or not.</returns>
        public bool Equals(ChatEventArgs other) {
            return other.Message == this.Message && other.Username == this.Username;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <returns>A new instance</returns>
        public object Clone() {
            return new ChatEventArgs(Message, Username);
        }
    }
}