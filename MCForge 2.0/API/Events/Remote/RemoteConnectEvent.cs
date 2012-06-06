using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Remote;
using System.Reflection;
using MCForge.Core;

namespace MCForge.API.Events.Remote {
    /// <summary>
    /// PlayerConnection event class
    /// </summary>
    public class RemoteConnectEvent : Event<string, RemoteConnectEventArgs> {
    }
    /// <summary>
    /// PlayConnectionEventArgs
    /// </summary>
    public class RemoteConnectEventArgs : EventArgs, ICancelable {

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteConnectEventArgs"/> class.
        /// </summary>
        public RemoteConnectEventArgs() :
            this(Server.ServerAssembly) {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="assemblyToLoadFrom">The assembly to load from.</param>
        public RemoteConnectEventArgs(Assembly assemblyToLoadFrom) {
            this.Assembly = assemblyToLoadFrom;
        }

        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public Assembly Assembly { get; set; }


        #region Extra code that doesn't need to be there, but it is >.>

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

        #endregion
    }
}
