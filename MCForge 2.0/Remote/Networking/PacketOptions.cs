using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Remote.Networking {
    public struct PacketOptions {

        /// <summary>
        /// Gets or sets a value indicating whether [use big endian].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use big endian]; otherwise, <c>false</c>.
        /// </value>
        public bool UseBigEndian { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use short as header size].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [use short as header size]; otherwise, <c>false</c>.
        /// </value>
        public bool UseShortAsHeaderSize { get; set; }

    }
}
