using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;

namespace MCForge.API.Events {
    public class CommandLoadEvent:Event<ICommand,CommandLoadEventArgs> {
    }
    public class CommandLoadEventArgs : EventArgs , ICancelable{
        public CommandLoadEventArgs(bool getsLoaded) {
            this.GetsLoaded = getsLoaded;
        }
        /// <summary>
        /// Gets whether the command gets loaded or unloaded.
        /// </summary>
        public bool GetsLoaded { get; private set; }
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
