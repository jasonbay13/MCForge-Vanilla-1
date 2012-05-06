using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.Events {
    public class PlayerConnection:Event<Player,PlayerConnectionEventArgs> {
    }
    public class PlayerConnectionEventArgs : EventArgs, ICloneable {
        public PlayerConnectionEventArgs(bool connected) {
            this.Connected = connected;
        }
        public bool Connected;


        public override object Clone() {
            return new PlayerConnectionEventArgs(Connected);
        }
    }
}
