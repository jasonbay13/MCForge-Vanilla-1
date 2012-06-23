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

namespace MCForge.Utils {
    public class Vector2S {
        public short x;
        public short z;
        public Vector2S() {
            this.x = 0;
            this.z = 0;
        }
        public Vector2S(short X, short Z) {
            x = X;
            z = Z;
        }

        public static Vector2S operator -(Vector2S a, Vector2S b) {
            return new Vector2S((short)(a.x - b.x), (short)(a.z - b.z));
        }
        public static Vector2S operator +(Vector2S a, Vector2S b) {
            return new Vector2S((short)(a.x + b.x), (short)(a.z + b.z));
        }
        public static Vector2S operator *(Vector2S a, Vector2S b) {
            return new Vector2S((short)(a.x * b.x), (short)(a.z * b.z));
        }
        public static Vector2S operator /(Vector2S a, Vector2S b) {
            return new Vector2S((short)(a.x / b.x), (short)(a.z / b.z));
        }
        public static bool operator ==(Vector2S a, Vector2S b) {
            return (a.x == b.x && a.z == b.z);
        }
        public static bool operator !=(Vector2S a, Vector2S b) {
            return !(a.x == b.x && a.z == b.z);
        }
        public Vector2S GetMove(short distance, Vector2S towards) {
            Vector2S ret = new Vector2S(x, z);
            ret.Move(distance, towards);
            return ret;
        }
        public void Move(short distance, Vector2S towards) {
            Vector2S way = towards - this;
            double length = way.Length;
            x += (short)Math.Round(((way.x / length) * distance));
            z += (short)Math.Round(((way.z / length) * distance));
        }
        public double Length {
            get {
                return Math.Sqrt(x * x + z * z);
            }
        }
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override string ToString() {
            return String.Format("x:{0} y:{1}", x, z);
        }
    }
}
