using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge;

namespace CommandDll
{
    public class CmdPlace : ICommand
    {
        public string Name { get { return "Place"; } }
        public CommandTypes Type { get { return CommandTypes.building; } }
        public string Author { get { return "Gamemakergm"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }

        public void Use(Player p, string[] args)
        {
            byte b = (byte)(Blocks.Types.zero);
            ushort x, z, y;
            Point3 pos = p.Pos;
            try
            {
                switch (args.Length)
                {
                    case 0:
                        x = (ushort)(pos.x / 32);
                        z = (ushort)(pos.z / 32);
                        y = (ushort)(pos.y / 32 - 2);
                        b = (byte)(Blocks.Types.stone);
                        break;
                    case 1:
                        x = (ushort)(pos.x / 32);
                        z = (ushort)(pos.z / 32);
                        y = (ushort)(pos.y / 32 - 1);
                        b = Blocks.NameToByte(args[0]);
                        break;
                    case 3:
                        x = Convert.ToUInt16(args[0]);
                        z = Convert.ToUInt16(args[1]);
                        y = Convert.ToUInt16(args[2]);
                        break;
                    case 4:
                        b = Blocks.NameToByte(args[0]);
                        x = Convert.ToUInt16(args[1]);
                        z = Convert.ToUInt16(args[2]);
                        y = Convert.ToUInt16(args[3]);
                        break;
                    default:
                        p.SendMessage("Invalid parameters.");
                        return;
                }
            }
            catch
            {
                p.SendMessage("Invalid parameters.");
                return;
            }
            if (b == (byte)(Blocks.Types.zero))
            {
                b = (byte)(Blocks.Types.stone);
            }
            //Need to wait for permissions for cannot place that block type.
            if (y >= p.level.Size.y)
            {
                y = (ushort)(p.level.Size.y - 1);
            }
            p.level.BlockChange(x, z, y, b);
            p.SendMessage("An " + Blocks.ByteToName(b) + " block was placed at (" + x + ", " + z + ", " + y + ").");
        }
        public void Help(Player p)
        {
            p.SendMessage("/place [block] <x z y> - Places block at your feet or <x z y>");
        }

        public void Initialize()
        {
            string[] CommandStrings = new string[2] { "place", "pl" };
            Command.AddReference(this, CommandStrings);
        }
    }
}
