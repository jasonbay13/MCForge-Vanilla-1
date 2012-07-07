/*
Copyright 2012 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using System.Threading;
using MCForge.Core;
using MCForge.World.Blocks;

namespace MCForge.World.Physics
{
    /// <summary>
    /// Create a custom block with physics
    /// </summary>
    public abstract class PhysicsBlock : Block
    {
        /// <summary>
        /// The physics time.
        /// </summary>
        protected static Thread TimerTick;

        /// <summary>
        /// The on tick method
        /// </summary>
        public abstract void Tick(Level l);

        int _x;
        int _y;
        int _z;
        //TODO
        //Check to see if the new value is out of bound or not.
        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>
        /// The X.
        /// </value>
        public virtual int X { get { return _x; } set { _x = value; } }
        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>
        /// The Y.
        /// </value>
        public virtual int Y { get { return _y; } set { _y = value; } }
        /// <summary>
        /// Gets or sets the Z.
        /// </summary>
        /// <value>
        /// The Z.
        /// </value>
        public virtual int Z { get { return _z; } set { _z = value; } }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicsBlock"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <param name="l">The l.</param>
        public PhysicsBlock(int x, int z, int y) { this.X = x; this.Y = y; this.Z = z; }
        
        public PhysicsBlock() : base() {}
        
        /// <summary>
        /// Remove this block from physics tick
        /// </summary>
        public void Remove(Level l) 
        {
            l.pblocks.Remove(this);
        }
        
        /// <summary>
        /// Add a block to the physics tick
        /// </summary>
        /// <param name="l"></param>
        public void Add(Level l, PhysicsBlock b)
        {
            l.pblocks.Add(this);
        }
        
        /// <summary>
        /// Add a physicsblock to the custom block list
        /// </summary>
        /// <param name="b"></param>
        public static void Add(PhysicsBlock b)
        {
            Block.Add(b);
        }
        
        /// <summary>
        /// Initializes physics block class
        /// </summary>
        public static new void InIt()
        {
            Level.OnAllLevelsLoad.SystemLvl += OnAllLevelsLoad_SystemLvl;
        }

        static void OnAllLevelsLoad_SystemLvl(Level sender, API.Events.LevelLoadEventArgs args) {
            sender.PhysicsThread = new Thread(() => {
                while (!Server.ShuttingDown) {
                    sender.pblocks.ForEach((pb) => pb.Tick(sender));
                    Thread.Sleep(sender.PhysicsTick);
                }
            });
        }
    }
}
