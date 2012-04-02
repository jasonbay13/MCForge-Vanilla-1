using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.World;

namespace MCForge.API.WorldEvent
{
    public interface WorldEvent
    {
        Level GetWorld();
    }
}
