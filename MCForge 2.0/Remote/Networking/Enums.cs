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

namespace MCForge.Remote {
    /// <summary>
    /// Packet identifier.
    /// </summary>
    public enum PacketID {

        Login,
        Handshake,
        Request,
        Fulfill,
        Chat,
        Ping,
        Disconnect,
        Edit,
        Crypto,
        Invalid

    }

    public enum RequestType {
        /// <summary>
        /// 
        /// </summary>
        All,
        Player,
        Settings,
        Groups,
        /// <summary>
        /// 
        /// </summary>
        Maps
    }

    public enum LoginResponse {
        Correct,
        Cred,
        Version,
        Banned,
        BadUsername
    }

    public enum ChatType {
        Player,
        Op,
        Admin,
        Logs,
        Command
    }

    public enum MoveType {
        Adding,
        Removing,
        Editing
    }

    public enum LevelActionType {
        Loading,
        Unloading
    }

    public enum EditType {
        Group,
        Player,
        Map,
        Settings,
    }
}

