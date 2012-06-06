using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using MCForge.Remote.Networking;
using MCForge.Remote.Packets;
using MCForge.Entity;

namespace MCForge.Remote {
    public class AndroidRemote :  IRemote {

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidRemote"/> class.
        /// </summary>
        /// <param name="socket">The socket.</param>
        public AndroidRemote(TcpClient socket) {
            NetworkStream = socket.GetStream();
            PacketWriter = new PacketWriter(this);
            PacketReader = new PacketReader(this);
            PacketOptions = new PacketOptions() {
                UseBigEndian = true,
                UseShortAsHeaderSize = true
            };

        }

        #region IRemote Members

        public string Username { get; set; }

        public int RemoteID { get; set; }

        public PacketOptions PacketOptions { get; set; }

        public NetworkStream NetworkStream { get; set; }

        public PacketReader PacketReader { get; set; }

        public PacketWriter PacketWriter { get; set; }

        public bool CanProcessPackets { get; set; }

        public void Disconnect(string message) {

            //Check if user is still connected, if so send disconnect message

            CanProcessPackets = false;
            NetworkStream.Close();
        }

        /// <summary>
        /// Its the alternative to the PSVM
        /// </summary>
        /// <param name="e">always null</param>
        public void Run(object e) {
        
            PacketReader.StartRead();
            PacketReader.OnReadPacket += ProcessPackets;

            PacketWriter.StartWrite();
        }

        #endregion



        #region Event Handlers

        void ProcessPackets(object sender, PacketReadEventArgs args) {
            switch (args.Packet.PacketID) {
                case PacketID.Login:
                    OnLogin((PacketLogin)args.Packet);
                    return;
                case PacketID.Ping:
                    OnPing();
                    return;
                default:
                    return;
            }
        }

        void OnLogin(PacketLogin packet) {
        }

        void OnPing() {

        }
        #endregion


    }
}
