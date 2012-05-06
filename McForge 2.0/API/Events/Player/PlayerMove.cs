using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;

namespace MCForge.API.Events {
    public class PlayerMove:Event<Player, PlayerMoveEventArgs> {
    }
    public class PlayerMoveEventArgs : EventArgs, ICloneable {
        public PlayerMoveEventArgs(Vector3 fromPosition) {
            this.FromPosition = fromPosition;
        }
        public Vector3 FromPosition;
        public override object Clone() {
            return new PlayerMoveEventArgs(FromPosition);
        }
    }

}
