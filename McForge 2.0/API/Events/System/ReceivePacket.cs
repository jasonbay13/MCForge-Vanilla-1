using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.Events {
    public class ReceivePacket:Event<Player,ReceivePacketEventArgs> {
    }
    public class ReceivePacketEventArgs:EventArgs,ICancelable {
        public ReceivePacketEventArgs(byte[] data) {
            this.Data = data;
        }
        /// <summary>
        /// Data Recieved
        /// </summary>
        public byte[] Data { get; private set; }
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
