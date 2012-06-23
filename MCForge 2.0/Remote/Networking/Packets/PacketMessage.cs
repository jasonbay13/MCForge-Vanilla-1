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

namespace MCForge.Remote.Packets {
    public class PacketMessage : Packet {
        public PacketMessage(IRemote remote) :base(remote) {
        }

        public override PacketID PacketID {
            get { return Remote.PacketID.Chat; }
        }

        public override void ReadPacket(Networking.PacketData data) {
            throw new NotImplementedException();
        }

        public override Networking.PacketData WritePacket() {
            throw new NotImplementedException();
        }
    }
}

