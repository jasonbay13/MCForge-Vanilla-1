using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
namespace MCForge.Network {

    public class NetClient {
        private TcpClient Client { get; set; }

        private NetworkStream NStream { get; set; }

        private Queue<Packet> PacketQueue { get; set; }



    }
}
