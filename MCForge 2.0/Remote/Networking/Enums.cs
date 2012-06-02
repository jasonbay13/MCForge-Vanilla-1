using System;

namespace MCForge.Remote {
    /// <summary>
    /// Packet identifier.
    /// </summary>
    public enum PacketID {

        Login,
        Handshake,
        Request,
        Chat,
        Ping, 
        Disconnect,
        Edit,
        Crypto

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

    public enum ChatType {
        Player,
        Op,
        Admin,
        Logs
    }

    public enum MoveType {
        Adding,
        Removing,
        Editing
    }

    public enum LevelActionType {
        Unloading,
        Loading
    }

    public enum EditType {
        Group,
        Player,
        Map, 
        Settings,
    }
}

