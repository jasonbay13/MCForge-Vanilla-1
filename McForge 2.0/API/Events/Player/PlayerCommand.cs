using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.Events {
    /// <summary>
    /// PlayerCommand event class
    /// </summary>
    public class PlayerCommand:Event<Player,PlayerCommandEventArgs>{
    }
    /// <summary>
    /// PlayerCommandEventArgs
    /// </summary>
    public class PlayerCommandEventArgs : EventArgs, ICloneable, ICancelable {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="command">The command (it might does not exist)</param>
        /// <param name="args">The arguments to be passed to the command</param>
        public PlayerCommandEventArgs(string command, string[] args) {
            this.Command = command;
            this.Args = args;
        }
        /// <summary>
        /// The command (it might does not exist)
        /// </summary>
        public string Command;
        /// <summary>
        /// The arguments to be passed to the command
        /// </summary>
        public string[] Args;
        /// <summary>
        /// Returns a new instance representing this instance
        /// </summary>
        /// <returns>A new instance</returns>
        public override object Clone() {
            return new PlayerCommandEventArgs(Command, Args);
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
