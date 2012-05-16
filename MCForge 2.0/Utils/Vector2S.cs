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
