using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace MCForge.Remote {
    public class AndroidRemote : IRemote {

        public AndroidRemote(TcpClient socket) {
            NetworkStream = socket.GetStream();
            PacketWriter = new PacketWriter(this);
            PacketReader = new PacketReader(this);
        }

        #region IRemote Members

        public System.Net.Sockets.NetworkStream NetworkStream { get; set;}

        public PacketReader PacketReader { get; set; }

        public PacketWriter PacketWriter { get; set; }

        public bool CanProcessPackets { get; set; }

        public void Disconnect(string message) {

            //Check if user is still connected, if so send disconnect message

            CanProcessPackets = false;
            NetworkStream.Close();
        }

        #endregion



        #region Event Handlers


        #endregion
    }
}
