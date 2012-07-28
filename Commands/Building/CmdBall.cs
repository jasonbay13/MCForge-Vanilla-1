using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.World;
using MCForge.Utils;

namespace MCForge.Commands.Building
{
    public class CmdBall : ICommand
    {
        public string Name
        {
            get { return "Spheroid"; }
        }

        public CommandTypes Type
        {
            get { return CommandTypes.Building; }
        }

        public string Author
        {
            get { return "ninedrafted"; }
        }

        public int Version
        {
            get { return 1; }
        }

        public string CUD
        {
            get { return ""; }
        }

        public byte Permission
        {
            get { return 0; }
        }

        public void Use(Player p, string[] args)
        {
            int rx = 10;
            if (args.Length > 0)
            {
                try
                {
                    rx = Int32.Parse(args[0]);
                    if (args.Length > 1) p.ExtraData["BallType"] = args[1];
                    else p.ExtraData["BallType"] = null;
                }
                catch
                {
                    p.ExtraData["BallType"] = args[0];
                }
            }
            p.ExtraData["BallRadius"] = rx;
            p.OnPlayerBlockChange.Normal += new API.Events.Event<Player, API.Events.BlockChangeEventArgs>.EventHandler(OnPlayerBlockChange_Normal);
            p.SendMessage("Define center");
        }

        void OnPlayerBlockChange_Normal(Player sender, API.Events.BlockChangeEventArgs args)
        {
            sender.OnPlayerBlockChange.Normal -= OnPlayerBlockChange_Normal;
            int rx = (int)sender.ExtraData["BallRadius"];
            int count = 0;
            IEnumerable<Vector3S> blocks;
            if (sender.ExtraData["BallType"] != null && ((string)sender.ExtraData["BallType"] == "h" || (string)sender.ExtraData["BallType"] == "hollow"))
                blocks = (new Vector3S(args.X, args.Z, args.Y)).GetNearBlocksHollow(rx, rx, rx);
            else
                blocks = (new Vector3S(args.X, args.Z, args.Y)).GetNearBlocks(rx, rx, rx);
            foreach (Vector3S v in blocks)
            {
                if (v.x > 0 && v.z > 0 && v.y > 0 && v.x < sender.Level.Size.x && v.z < sender.Level.Size.z && v.y < sender.Level.Size.y)
                {
                    sender.Level.BlockChange(v, args.Holding, sender);
                    count++;
                }
            }
            sender.SendMessage(count + " Blocks");
        }

        public void Help(Player p)
        {
            p.SendMessage("/spheroid - uses radius 10 and type fill");
            p.SendMessage("/spheroid [radius] - uses given radius and type fill");
            p.SendMessage("/spheroid [type] - uses radius 10 and given type");
            p.SendMessage("/spheroid [radius] [type] - uses given radius and given type");
            p.SendMessage("Creates a sphere");
            p.SendMessage("Accepted types are: fill, hollow");
        }

        public void Initialize()
        {
            Command.AddReference(this, "spheroid");
        }
    }
}
