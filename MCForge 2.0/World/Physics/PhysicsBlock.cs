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
        public abstract void Tick(Level l);
        public abstract int X { get; }
        public abstract int Y { get; }
        public abstract int Z { get; }
        public void Remove()
        {
            blocks.Remove(this);
        }
        public static void InIt()
        {
            blocks.Add(new LavaExample()); //Remove once we get blocks
            tick = new Thread(new ParameterizedThreadStart(delegate
                {
                    while (true)
                    {
                        Level.levels.ForEach(l =>
                        {
                            blocks.ForEach(p => { p.Tick(l); });
                            Thread.Sleep(ServerSettings.PhysicsTick);
                        });
                    }
                }));
            tick.Start();
        }
    }
}
