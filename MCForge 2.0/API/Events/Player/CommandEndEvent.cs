/*
Copyright 2012 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
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