using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Entity {

    public enum ChatType {
        Player,
        Op,
        Admin,
        PM,
        Global
    }

    public static class PlayerConstants {

        /// <summary>
        /// Standard OpenGL pixel to meter ratio
        /// </summary>
        public const byte PIXEL_TO_METER_RATIO = 32;
    }

}
