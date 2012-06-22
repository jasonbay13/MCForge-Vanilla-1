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
