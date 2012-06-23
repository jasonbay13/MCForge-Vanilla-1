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
using System.Linq;
using System.Text;

namespace MCForge.Robot
{
    /// <summary>    
    /// Author: Roy Triesscheijn (http://www.royalexander.wordpress.com)
    /// Point3D class mimics some of the Microsoft.Xna.Framework.Vector3
    /// but uses Int32's instead of floats.
    /// </summary>
    public class Point3D
    {
        public int X;
        public int Y;
        public int Z;

        public Point3D(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public int GetDistanceSquared(Point3D point)
        {
            int dx = this.X - point.X;
            int dy = this.Y - point.Y;
            int dz = this.Z - point.Z;
            return (dx * dx) + (dy * dy) + (dz * dz);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point3D)
            {
                Point3D p3d = (Point3D)obj;
                return p3d.X == this.X && p3d.Z == this.Z && p3d.Y == this.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {

            return (X + " " + Y + " " + Z).GetHashCode();
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }

        public static bool operator ==(Point3D one, Point3D two)
        {
            return one.Equals(two);
        }

        public static bool operator !=(Point3D one, Point3D two)
        {
            return !one.Equals(two);
        }

        public static Point3D operator +(Point3D one, Point3D two)
        {
            return new Point3D(one.X + two.X, one.Y + two.Y, one.Z + two.Z);
        }

        public static Point3D operator -(Point3D one, Point3D two)
        {
            return new Point3D(one.X - two.X, one.Y - two.Y, one.Z - two.Z);
        }

        public static Point3D Zero = new Point3D(0, 0, 0);
    }
}
