using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MCForge.Remote.Networking;
using System.Threading;

namespace MCForge.Remote {
    public class PacketReader : IDisposable {

        /// <summary>
        /// Event Handler for recieving packets
        /// </summary>
        public EventHandler<PacketReadEventArgs> OnReadPacket;

        private BinaryReader mReader;
        private IRemote remote;
        private PacketOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketReader"/> class.
        /// </summary>
        /// <param name="c">The remote.</param>
        public PacketReader ( IRemote  c ) {
            mReader = new BinaryReader( c.NetworkStream );
            this.remote = c;
            options = c.PacketOptions;
        }

        /// <summary>
        /// Reads the next incoming packet.
        /// </summary>
        /// <returns></returns>
        public Packet ReadPacket ( ) {
            try {
                byte id = mReader.ReadByte();
                Packet p = Packet.GetPacket( ( PacketID ) id );
                if (p is Packets.PacketInvalid)
                    throw new IOException("Received unknown packet");
                int len = PacketData.GetLength(remote.NetworkStream, remote.PacketOptions);
                byte[] data = new byte[len];
                PacketData pData = new PacketData(data, remote.PacketOptions);
                p.ReadPacket(pData);
                return p;
            }
            catch { return null; }
        }

        /// <summary>
        /// Starts reading incoming packets
        /// </summary>
        public void StartRead ( ) {
            new Thread(new ThreadStart(() => {
                while (remote.CanProcessPackets) {
                    var p = ReadPacket();
                    if (p == null) throw new IOException("Recived packet that caused an error");
                    if (OnReadPacket != null)
                        OnReadPacket(this, new PacketReadEventArgs(p));
                }
            })).Start();
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            mReader.Dispose();
            remote.CanProcessPackets = false;
        }

        #endregion
    }


    public class PacketReadEventArgs : EventArgs {

        /// <summary>
        /// Gets the packet
        /// </summary>
        public Packet Packet { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="PacketReadEventArgs"/> class.
        /// </summary>
        /// <param name="p">The packet.</param>
        public PacketReadEventArgs(Packet p) {
            Packet = p;
        }
    }
}
