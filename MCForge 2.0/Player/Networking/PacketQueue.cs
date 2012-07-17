using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace MCForge.Networking {
    public class PacketQueue {

        public Queue<Packet> InQueue, OutQueue;

        private PacketReader PacketReader;
        private PacketWriter PacketWriter;

        private readonly Thread ReadThread, WriteThread;

        private static readonly object ReadLock, WriteLock;

        private readonly Thread startThread;

        /// <summary>
        /// Gets a value indicating whether this <see cref="PacketQueue"/> is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running; otherwise, <c>false</c>.
        /// </value>
        public bool Running { get; private set; }

        static PacketQueue() {
            ReadLock = new object();
            WriteLock = new object();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketQueue"/> class.
        /// </summary>
        /// <param name="Client">The client.</param>
        public PacketQueue(TcpClient Client) :
            this(Client.GetStream()) {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketQueue"/> class.
        /// </summary>
        /// <param name="NetworkStream">The network stream.</param>
        public PacketQueue(NetworkStream NetworkStream) {
            startThread = Thread.CurrentThread;

            InQueue = new Queue<Packet>();
            OutQueue = new Queue<Packet>();

            PacketReader = new PacketReader(NetworkStream);
            PacketWriter = new PacketWriter(NetworkStream);

            ReadThread = new Thread(new ThreadStart(ReadPackets));
            WriteThread = new Thread(new ThreadStart(WritePackets));

        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start() {
            if ( ReadThread != null )
                ReadThread.Start();

            if ( WriteThread != null )
                WriteThread.Start();

            Running = true;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop() {
            Running = false;
        }

        /// <summary>
        /// Writes the packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public void WritePacket(Packet packet) {
            if ( startThread != Thread.CurrentThread )
                throw new IOException("You can only send packet on thread that created packet queue");

            if ( OutQueue != null )
                OutQueue.Enqueue(packet);
        }

        /// <summary>
        /// Writes the packet now and flushes the packet queue.
        /// </summary>
        /// <param name="packetDisconnectPlayer">The packet disconnect player.</param>
        internal void WritePacketNowAndFlush(Packet packet) {

            if ( OutQueue != null )
                OutQueue.Clear();

            if ( PacketWriter != null )
                PacketWriter.WritePacket(packet);
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void CloseConnection() {

            Stop();

            try {
                if ( PacketReader != null ) {
                    PacketReader.Close();
                    PacketReader.Dispose();
                    PacketReader = null;
                }
            }
            catch ( ObjectDisposedException ) { }
            catch ( IOException ) { }

            try {
                if ( PacketWriter != null ) {
                    PacketWriter.Close();
                    PacketWriter.Dispose();
                    PacketWriter = null;
                }
            }
            catch ( ObjectDisposedException ) { }
            catch ( IOException ) { }

            if ( InQueue != null ) {
                InQueue.Clear();
                InQueue = null;
            }

            if ( OutQueue != null ) {
                OutQueue.Clear();
                OutQueue = null;
            }

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
