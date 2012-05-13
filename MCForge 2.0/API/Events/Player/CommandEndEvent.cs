using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Interface.Command;

namespace MCForge.API.Events {
    /// <summary>
    /// Event Class for when a command that a player called has just ended
    /// <remarks>Ended = End of <see cref="MCForge.Interface.Command.ICommand"/> Use()</remarks>
    /// </summary>
    public class CommandEndEvent: Event<Player, CommandEndEventArgs> {
    }
    /// <summary>
    /// Event Class for when a command that a player called has just ended
    /// <remarks>Ended = End of <see cref="MCForge.Interface.Command.ICommand"/> Use()</remarks>
    /// </summary>
    public class CommandEndEventArgs : EventArgs {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="args">The arguments that was passed to the command</param>
        public CommandEndEventArgs(ICommand command, string[] args) {
            this.Command = command;
            this.Args = args;
        }
        /// <summary>
        /// Gets the the command that was used
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Gets the argument that was used in the command.
        /// </summary>
        public string[] Args { get; private set; }
    }
}