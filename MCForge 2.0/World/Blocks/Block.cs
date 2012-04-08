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
        public static readonly Block Adminium = new Adminium();
        public static readonly Block Air = new Air();
        public static readonly Block Black = new Black();
        public static readonly Block Blue = new Blue();
        public static readonly Block BlueViolet = new BlueViolet();
        public static readonly Block Bookcase = new Bookcase();
        public static readonly Block Brick = new Brick();
        public static readonly Block BrownShroom = new BrownShroom();
        public static readonly Block Coal = new Coal();
        public static readonly Block Cobblestone = new Cobblestone();
        public static readonly Block Cyan = new Cyan();
        public static readonly Block Dirt = new Dirt();
        public static readonly Block DoubleStair = new DoubleStair();
        public static readonly Block Glass = new Glass();
        public static readonly Block Gold = new Gold();
        public static readonly Block Gold_Ore = new Gold_Ore();
        public static readonly Block Grass = new Grass();
        public static readonly Block Gravel = new Gravel();
        public static readonly Block Gray = new Gray();
        public static readonly Block Green = new Green();
        public static readonly Block GreenYellow = new GreenYellow();
        public static readonly Block Indigo = new Indigo();
        public static readonly Block Iron = new Iron();
        public static readonly Block Iron_Ore = new Iron_Ore();
        public static readonly Block Lava = new Lava();
        public static readonly Block Leaves = new Leaves();
        public static readonly Block Magenta = new Magenta();
        public static readonly Block MossyCobbleStone = new MossyCobbleStone();
        public static readonly Block Obsidian = new Obsidian();
        public static readonly Block Pink = new Pink();
        public static readonly Block Purple = new Purple();
        public static readonly Block Red = new Red();
        public static readonly Block RedFlower = new RedFlower();
        public static readonly Block RedShroom = new RedShroom();
        public static readonly Block Sand = new Sand();
        public static readonly Block Shrub = new Shrub();
        public static readonly Block Sponge = new Sponge();
        public static readonly Block SpringGreen = new SpringGreen();
        public static readonly Block Stair = new Stair();
        public static readonly Block Stone = new Stone();
        public static readonly Block TNT = new TNT();
        public static readonly Block Tree = new Tree();
        public static readonly Block UNKNOWN = new UNKNOWN();
        public static readonly Block Water = new Water();
        public static readonly Block White = new White();
        public static readonly Block Wood = new Wood();
        public static readonly Block Yellow = new Yellow();
        public static readonly Block YellowFlower = new YellowFlower();
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
        internal static void InIt() //Possibly get rid of this...we might not need it.
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
            byte bytetoreturn = new UNKNOWN().VisibleBlock;
            blocks.ForEach(b => { if (b.Name == name.ToLower()) bytetoreturn = b.VisibleBlock; });
            return bytetoreturn;
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
