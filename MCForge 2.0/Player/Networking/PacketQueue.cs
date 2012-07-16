using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace MCForge.Networking {
    public class PacketQueue {

        public readonly Queue<Packet> InQueue, OutQueue;

        private readonly PacketReader PacketReader;
        private readonly PacketWriter PacketWriter;

        private readonly Thread ReadThread, WriteThread;

        private static readonly object ReadLock, WriteLock;

        public bool Running { get; private set; }

        static PacketQueue() {
            ReadLock = new object();
            WriteLock = new object();
        }

        public PacketQueue(TcpClient Client) :
            this(Client.GetStream()) {

        }

        public PacketQueue(NetworkStream NetworkStream) {

            InQueue = new Queue<Packet>();
            OutQueue = new Queue<Packet>();

            PacketReader = new PacketReader(NetworkStream);
            PacketWriter = new PacketWriter(NetworkStream);

            ReadThread = new Thread(new ThreadStart(ReadPackets));
            WriteThread = new Thread(new ThreadStart(WritePackets));

        }

        public void Start() {
            if ( ReadThread != null )
                ReadThread.Start();

            if ( WriteThread != null )
                WriteThread.Start();

            Running = true;
        }

        public void Stop() {
            Running = false;
        }

        public void WritePacket(Packet packet) {
            if ( OutQueue != null )
                OutQueue.Enqueue(packet);
        }

        public void CloseConnection() {

            string reason = null;



        }



        void ReadPackets() {

            try {
                while ( Running ) {
                    Packet packet = null;
                    lock ( ReadLock )
                        packet = PacketReader.ReadPacket();

                    if ( packet != null ) {
                        InQueue.Enqueue(packet);
                    }
                }
            }
            catch {
                CloseConnection();
            }

            CloseConnection();
        }

        void WritePackets() {

            try {
                while ( Running ) {
                    Packet packet = null;
                    lock ( WriteLock )
                        packet = OutQueue.Dequeue();

                    if ( packet != null ) {
                        PacketWriter.WritePacket(packet);
                    }
                }
            }
            catch {
                CloseConnection();
            }

            CloseConnection();
        }

    }
}
