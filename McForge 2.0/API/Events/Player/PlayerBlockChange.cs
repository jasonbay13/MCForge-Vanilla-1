using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.API.PlayerEvent;

namespace MCForge.API.Events {
    public class PlayerBlockChange : Event<Player, PlayerBlockChangeEventArgs> {
    }
    public class PlayerBlockChangeEventArgs : EventArgs, ICloneable, ICancelable {
        public PlayerBlockChangeEventArgs(ushort x, ushort y, ushort z, ActionType action, byte holding) : this(action, holding, x, y, z) { }
        public PlayerBlockChangeEventArgs(ActionType action, byte holding, ushort x, ushort y, ushort z) {
            this.Action = action;
            this.Holding = holding;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        /// <summary>
        /// What we arre doing with this block
        /// </summary>
        public ActionType Action { get; set; }
        /// <summary>
        /// The block at the coordinates
        /// </summary>
        public byte Holding { get; set; }
        /// <summary>
        /// The x coordinate of the block changed.
        /// </summary>
        public ushort X { get; set; }
        /// <summary>
        /// The y coordinate of the block changed.
        /// </summary>
        public ushort Y { get; set; }
        /// <summary>
        /// The z coordinate of the block changed.
        /// </summary>
        public ushort Z { get; set; }
        public override object Clone() {
            return new PlayerBlockChangeEventArgs(Action, Holding, X, Y, Z);
        } 
        private bool canceled = false;
        public bool Canceled {
            get { return canceled; }
        }

        public void Cancel() {
            canceled = true;
        }

        public void Allow() {
            canceled = false;
        }
    }
    
}
