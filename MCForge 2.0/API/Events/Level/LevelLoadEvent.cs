using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.World;

namespace MCForge.API.Events {
    public class LevelLoadEvent:Event<Level,LevelLoadEventArgs>{
    }
    public class LevelLoadEventArgs : EventArgs {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="loaded">Whether the level is loaded or not.</param>
        public LevelLoadEventArgs(bool loaded) {
            this.Loaded = loaded;
        }
        /// <summary>
        /// Whether the level is loaded or not.
        /// </summary>
        public bool Loaded;
    }
}
