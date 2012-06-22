/*
Copyright 2012 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
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
