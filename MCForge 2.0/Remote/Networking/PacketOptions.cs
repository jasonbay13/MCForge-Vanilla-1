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
