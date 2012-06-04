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

