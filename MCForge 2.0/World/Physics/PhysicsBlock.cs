using System;
using System.Collections.Generic;
using System.Threading;
using MCForge.Core;

namespace MCForge.World.Physics
{
    public abstract class PhysicsBlock
    {
        protected static Thread tick;
        protected static List<PhysicsBlock> blocks = new List<PhysicsBlock>();
        public abstract void Tick();
        int _x;
        int _y;
        int _z;
        Level _l;
        public virtual Level l { get { return _l; } }
        public virtual int X { get { return _x; } set { _x = value; } }
        public virtual int Y { get { return _y; } set { _y = value; } }
        public virtual int Z { get { return _z; } set { _z = value; } }
        public PhysicsBlock(int x, int y, int z, Level l) { this.X = x; this.Y = y; this.Z = z; this._l = l; }
        public void Remove()
        {
            blocks.Remove(this);
        }
        public void AddBlock(PhysicsBlock block)
        {
            blocks.Add(block);
        }
        public static void InIt()
        {
            tick = new Thread(new ParameterizedThreadStart(delegate
                {
                    while (true)
                    {
                        Level.levels.ForEach(l =>
                        {
                            blocks.ForEach(b => { if (b.l == l) b.Tick(); });
                            Thread.Sleep(l.PhysicsTick);
                        });
                        Thread.Sleep(1);
                    }
                }));
            tick.Start();
        }
    }
}
