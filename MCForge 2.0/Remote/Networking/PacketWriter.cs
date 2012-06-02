using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MCForge.Remote {
    public class PacketWriter {

        private Queue<Packet> _packetQueue;
        private IRemote Remote;
        public PacketWriter(IRemote remote) {
            Remote = remote;
            _packetQueue = new Queue<Packet>();
            new Thread(new ThreadStart(WriteThread)).Start();
        }

        public void WritePacket(Packet p) {
            if (Remote.CanProcessPackets)
                _packetQueue.Enqueue(p);
        }

        private void WriteThread() {
            while (Remote.CanProcessPackets) {
                if (_packetQueue.Count > 0) {
                    var packet = _packetQueue.Dequeue();
                    packet.WritePacket(Remote);
                }
                else {
                    Thread.Sleep(5);
                }
            }
        }
    }
}
