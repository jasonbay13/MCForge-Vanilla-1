using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using MCForge.Remote.Networking;

namespace MCForge.Remote {
    public interface IRemote {

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets the remote ID.
        /// </summary>
        /// <value>
        /// The remote ID.
        /// </value>
        int RemoteID { get; set; }

        /// <summary>
        /// Gets or sets the packet options.
        /// </summary>
        /// <value>
        /// The packet options.
        /// </value>
        PacketOptions PacketOptions { get; set; }

        /// <summary>
        /// Gets or sets the network stream.
        /// </summary>
        /// <value>
        /// The network stream.
        /// </value>
        NetworkStream NetworkStream { get; set; }

        /// <summary>
        /// Gets or sets the packet reader.
        /// </summary>
        /// <value>
        /// The packet reader.
        /// </value>
        PacketReader PacketReader { get; set; }

        /// <summary>
        /// Gets or sets the packet writer.
        /// </summary>
        /// <value>
        /// The packet writer.
        /// </value>
        PacketWriter PacketWriter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can process packets.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can process packets; otherwise, <c>false</c>.
        /// </value>
        bool CanProcessPackets { get; set; }

        /// <summary>
        /// Disconnects the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Disconnect(string message);

        /// <summary>
        /// Starts the remote for input/output.
        /// </summary>
        /// <param name="e">always null, ignore it.</param>
        void Run(object e);
    }
}
