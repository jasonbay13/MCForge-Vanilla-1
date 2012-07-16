using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Networking {
    public enum PacketIDs : byte {

        Identification = 0x00,

        Ping = 0x01,

        LevelInitialize = 0x02,
        LevelDataChunk = 0x03,
        LevelFinalize = 0x04,

        PlayerSetBlock = 0x05,
        ServerSetBlock = 0x06,

        SpawnPlayer = 0x07,

        PosAndRot = 0x08,
        PosAndRotUpdate = 0x09,
        PosUpdate = 0x0a,
        RotUpdate = 0x0b,
        Update = 0x0c,

        Message = 0x0d,

        KickPlayer = 0x0e,

        UpdateUserType = 0x0f

    }
}
