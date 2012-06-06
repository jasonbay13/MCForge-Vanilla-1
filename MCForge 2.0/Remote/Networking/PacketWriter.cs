using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MCForge.Remote.Networking;

namespace MCForge.Remote {
    public class PacketWriter {

        private Queue<Packet> _packetQueue;
        private IRemote Remote;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketWriter"/> class.
        /// </summary>
        /// <param name="remote">The remote.</param>
        public PacketWriter(IRemote remote) {
            Remote = remote;
            _packetQueue = new Queue<Packet>();
        }

        /// <summary>
        /// Writes the packet.
        /// </summary>
        /// <param name="p">The packet to write.</param>
        public void WritePacket(Packet p) {
            if (Remote.CanProcessPackets)
                _packetQueue.Enqueue(p);
        }

        /// <summary>
        /// Starts the thread for writing.
        /// </summary>
        public void StartWrite() {
            new Thread(new ThreadStart(() => {
                while (Remote.CanProcessPackets) {
                    if (_packetQueue.Count > 0) {
                        var packet = _packetQueue.Dequeue();
                        byte[] data = packet.WritePacket().ReadAll();
                        byte[] lenData = PacketData.GetLength(data.Length, Remote.PacketOptions);
                        Remote.NetworkStream.Write(lenData, 0, lenData.Length);
                        Remote.NetworkStream.Write(data, 0, data.Length);
                    }
                    else {
                        Thread.Sleep(5);
                    }
                }
            })).Start();
        }

    }
}
