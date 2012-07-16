using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.World;
using MCForge.Entity;

namespace MCForge.API.Events {
    public class LevelChangeEvent : Event<Player, LevelChangeEventArgs> {
    }
    public class LevelChangeEventArgs : EventArgs, ICancelable {
        public LevelChangeEventArgs(Level oldLevel, Level newLevel) {
            this.OldLevel = oldLevel;
            this.NewLevel = newLevel;
        }
        public Level OldLevel { get; private set; }
        public Level NewLevel { get; private set; }
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
