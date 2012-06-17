using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.World.Loading_and_Saving {
    public interface IMap {

        Level.SaveTypes Type {get; }

        Level Load(string levelName, string path);

    }
}
