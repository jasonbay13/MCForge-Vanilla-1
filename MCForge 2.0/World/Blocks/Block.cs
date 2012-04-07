using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Core;
using MCForge.World.Blocks;

namespace MCForge.World
{
    public abstract class Block
    {
        static List<Block> blocks = new List<Block>();
        public abstract byte VisibleBlock { get; }
        public abstract string Name { get; }
        public virtual void SetBlock(Vector3 pos, Level l)
        {
            l.SetBlock(pos.x, pos.z, pos.y, VisibleBlock);
        }
        public virtual void SetBlock(int x, int z, int y, Level l)
        {
            l.SetBlock((ushort)x, (ushort)z, (ushort)y, VisibleBlock);
        }
        public virtual void SetBlock(ushort x, ushort z, ushort y, Level l)
        {
            l.SetBlock(x, z, y, VisibleBlock);
        }
        public virtual void SetBlock(int pos, Level l)
        {
            l.SetBlock(pos, VisibleBlock);
        }
        public static void Add(Block b)
        {
            if (!blocks.Contains(b))
                blocks.Add(b);
        }
        internal static void InIt()
        {
            Server.Log("Loading blocks...");
            Add(new Adminium());
            Add(new Air());
            Add(new Black());
            Add(new Blue());
            Add(new BlueViolet());
            Add(new Bookcase());
            Add(new Brick());
            Add(new BrownShroom());
            Add(new Coal());
            Add(new Cobblestone());
            Add(new Cyan());
            Add(new Dirt());
            Add(new DoubleStair());
            Add(new Glass());
            Add(new Gold());
            Add(new Gold_Ore());
            Add(new Grass());
            Add(new Gravel());
            Add(new Gray());
            Add(new Green());
            Add(new GreenYellow());
            Add(new Indigo());
            Add(new Iron());
            Add(new Iron_Ore());
            Add(new Lava());
            Add(new Leaves());
            Add(new Magenta());
            Add(new MossyCobbleStone());
            Add(new Obsidian());
            Add(new Orange());
            Add(new Pink());
            Add(new Purple());
            Add(new Red());
            Add(new RedFlower());
            Add(new RedShroom());
            Add(new Sand());
            Add(new Shrub());
            Add(new Sponge());
            Add(new SpringGreen());
            Add(new Stair());
            Add(new Stone());
            Add(new TNT());
            Add(new Tree());
            Add(new UNKNOWN());
            Add(new Water());
            Add(new White());
            Add(new Wood());
            Add(new Yellow());
            Add(new YellowFlower());
            Add(new Wood());
        }
        public static byte NameToByte(string name)
        {
            byte nametoreturn = new UNKNOWN().VisibleBlock;
            blocks.ForEach(b => { if (b.Name == name.ToLower()) nametoreturn = b.VisibleBlock; });
            return nametoreturn;
        }
        public static bool ValidBlockName(string name)
        {
            return NameToByte(name) != 255;
        }
        public static string ByteToName(byte b)
        {
            string nametoreturn = new UNKNOWN().Name;
            blocks.ForEach(b1 => { if (b1.VisibleBlock == b) nametoreturn = b1.Name; });
            return nametoreturn;
        }
    }
}
