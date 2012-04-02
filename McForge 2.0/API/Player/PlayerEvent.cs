using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    public interface PlayerEvent
    {
        Player GetPlayer();
    }
}
