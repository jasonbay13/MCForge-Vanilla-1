/*
Copyright 2011 MCForge
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
using MCForge.Utils;

namespace MCForge.World.Physics
{
    public class Active_Lava : PhysicsBlock
    {
        public override string Name
        {
            get { return "active_lava"; }
        }
        public override byte VisibleBlock
        {
            get { return 10; }
        }
        public override byte Permission
        {
            get { return 80; }
        }
        public Active_Lava(int x, int y, int z)
            : base(x, y, z)
        {

        }
        public Active_Lava() { }
        
        public override object Clone()
        {
            Active_Lava al = new Active_Lava();
            al.X = X;
            al.Y = Y;
            al.Z = Z;
            return al;
        }
        
        public override void Tick(Level l)
        {
            /* .....-----===== Explosion =====-----..... */
            /*List<Vector3S> tweet = new List<Vector3S>();
            Random r = new Random();
            int size = 10;
            for (ushort xx = (ushort)(X - (size + 1)); xx <= (ushort)(X + (size + 1)); xx++)
                for (ushort zz = (ushort)(Z - (size + 1)); zz <= (ushort)(Z + (size + 1)); zz++)
                    for (ushort yy = (ushort)(Y - (size + 1)); yy <= (ushort)(Y + (size + 1)); yy++)
                    {
                        if (r.Next(1, 11) <= 4)
                        {
                            l.BlockChange(xx, zz, yy, Block.BlockList.LAVA);
                            tweet.Add(new Vector3S(xx, zz, yy));
                        }
                        else if (r.Next(1, 11) <= 8)
                        {
                            l.BlockChange(xx, zz, yy, Block.BlockList.AIR);
                        }
                    }
            for (ushort xx = (ushort)(X - (size + 2)); xx <= (ushort)(X + (size + 2)); xx++)
                for (ushort zz = (ushort)(Z - (size + 2)); zz <= (ushort)(Z + (size + 2)); zz++)
                    for (ushort yy = (ushort)(Y - (size + 2)); yy <= (ushort)(Y + (size + 2)); yy++)
                    {
                        if (r.Next(1, 11) <= 4)
                        {
                            l.BlockChange(xx, zz, yy, Block.BlockList.LAVA);
                            tweet.Add(new Vector3S(xx, zz, yy));
                        }
                        else if (r.Next(1, 11) <= 8)
                        {
                            l.BlockChange(xx, zz, yy, Block.BlockList.AIR);
                        }
                    }

                        for (ushort xx = (ushort)(X - (size + 3)); xx <= (ushort)(X + (size + 3)); xx++)
                for (ushort zz = (ushort)(Z - (size + 3)); zz <= (ushort)(Z + (size + 3)); zz++)
                    for (ushort yy = (ushort)(Y - (size + 3)); yy <= (ushort)(Y + (size + 3)); yy++)
                    {
                        if (r.Next(1, 11) <= 4)
                        {
                            l.BlockChange(xx, zz, yy, Block.BlockList.LAVA);
                            tweet.Add(new Vector3S(xx, zz, yy));
                        }
                        else if (r.Next(1, 11) <= 8)
                        {
                            l.BlockChange(xx, zz, yy, Block.BlockList.AIR);
                        }
                    }
                        //Clean up
                        foreach (Vector3S lol in tweet)
                        {
                            if (l.GetBlock(lol) == Block.BlockList.LAVA)
                            {
                                l.BlockChange(lol, Block.BlockList.AIR);
                            }
                        }*/
            
            /* .....-----===== Fireworks =====-----..... */

            /*Random r = new Random();
            List<Vector3S> tweet = new List<Vector3S>();
            int storedRand1 = r.Next(21, 36);
            int storedRand2 = r.Next(21, 36);
            int size = 15;
            for (ushort xx = (ushort)(X - (size + 1)); xx <= (ushort)(X + (size + 1)); xx++)
                for (ushort zz = (ushort)(Z - (size + 1)); zz <= (ushort)(Z + (size + 1)); zz++)
                    for (ushort yy = (ushort)(Y - (size + 1)); yy <= (ushort)(Y + (size + 1)); yy++)
                    {
                        if (r.Next(1, 40) < 2)
                        {
                            byte b = (byte)(r.Next(Math.Min(storedRand1, storedRand2), Math.Max(storedRand1, storedRand2)));
                            l.BlockChange(xx, zz, yy, b);
                            tweet.Add(new Vector3S(xx, zz, yy));
                        }
                    }

            //Clean up
            foreach (Vector3S lol in tweet)
            {
                if (l.GetBlock(lol) == Block.BlockList.LAVA)
                {
                    l.BlockChange(lol, Block.BlockList.AIR);
                }
            }*/

        }
    }
}
