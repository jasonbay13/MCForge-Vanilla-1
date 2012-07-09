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
using MCForge.Utils;

namespace MCForge.World.Physics
{
    public class Active_Water : PhysicsBlock
    {
        public override string Name
        {
            get { return "active_water"; }
        }
        public override byte VisibleBlock
        {
            get { return 8; }
        }
        public override byte Permission
        {
            get { return 80; }
        }
        public Active_Water(int x, int z, int y)
            : base(x, z, y)
        {
        }
        public Active_Water() {}
        
        public override object Clone()
        {
            Active_Water aw = new Active_Water();
            aw.X = X;
            aw.Y = Y;
            aw.Z = Z;
            return aw;
        }
        
        public override void Tick(Level l)
        {
            if (l.GetBlock(X, Z, Y - 1) == Block.BlockList.AIR) {
                Add(l, new Active_Water(X, Z, Y - 1));
                Remove(l);
                //l.BlockChange((ushort)X, (ushort)Z, (ushort)(Y - 1), this);
            }
            else {
                if (l.GetBlock(X + 1, Z, Y) == Block.BlockList.AIR) {
                    Add(l, new Active_Water(X + 1, Z, Y));
                    //l.BlockChange((ushort)(X + 1), (ushort)Z, (ushort)Y, this);
                }
                if (l.GetBlock(X - 1, Z, Y) == Block.BlockList.AIR) {
                    Add(l, new Active_Water(X - 1, Z, Y));
                    //l.BlockChange((ushort)(X - 1), (ushort)Z, (ushort)Y, this);
                }
                if (l.GetBlock(X, Z + 1, Y) == Block.BlockList.AIR) {
                    Add(l, new Active_Water(X, Z + 1, Y));
                    //l.BlockChange((ushort)X, (ushort)(Z + 1), (ushort)Y, this);
                }
                if (l.GetBlock(X, Z - 1, Y) == Block.BlockList.AIR) {
                    Add(l, new Active_Water(X, Z - 1, Y));
                   // l.BlockChange((ushort)X, (ushort)(Z - 1), (ushort)Y, this);
                }
            }
        }
    }
}
