using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using MCForge.Core;
using MCForge.Groups;
using MCForge.Interface.Command;
using MCForge.Utilities.Settings;
using MCForge.World;
using MCForge.Utilities;
using MCForge.Utils;
using System.Linq;
using MCForge.SQL;
using System.Data;
using MCForge.Entity;
using MCForge.API.Events;

namespace MCForge.Robot
{
    public sealed partial class Bot
    {
        public Player Player { get; set; }
        public Bot()
        {
        }

    }
}
