using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;

namespace MCForge.API.Events {
    /// <summary>
    /// The PacketEvent
    /// </summary>
    public class PacketEvent : Event <Player, PacketEventArgs> {
    }
    /// <summary>
    /// The PacketEventArgs
    /// </summary>
    public class PacketEventArgs : EventArgs, ICancelable, ICloneable, IEquatable<PacketEventArgs> {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="data">The packet data</param>
        /// <param name="incoming">True for incoming, false for outgoing packet.</param>
        public PacketEventArgs(byte[] data, bool incoming, packet.types type) {
            this.Data = data;
            this.Incoming = incoming;
            this.Type = type;
        }
        /// <summary>
        /// The packet type
        /// </summary>
        public packet.types Type { get; private set; }
        /// <summary>
        /// Packet data
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// Gets the packet was incoming or is outgoing
        /// </summary>
        public bool Incoming { get; private set; }
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

        /// <summary>
        /// Compares equality (ICancelable and IStoppable are not part of the comparison)
        /// </summary>
        /// <param name="other">The value to be compared to.</param>
        /// <returns>Whether they are equal or not.</returns>
        public bool Equals(PacketEventArgs other) {

            if (this.Data.Length != other.Data.Length || this.Type != other.Type || this.Incoming != other.Incoming) return false;
            for (int i = 0; i < this.Data.Length; i++) {
                if (this.Data[i] != other.Data[i]) return false;
            }
            return true;
        }
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <returns>A new instance</returns>
        public object Clone() {
            return new PacketEventArgs(this.Data, this.Incoming, this.Type);
        }
    }
}
