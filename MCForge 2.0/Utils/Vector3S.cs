using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Core;

namespace MCForge.Utils {
    public class Vector3S{
        public short x;
        public short y;
        public short z;
        public Vector3S() {
            x = 0;
            z = 0;
            y = 0;
        }
        public Vector3S(Vector3S v) :
            this(v.x, v.z, v.y) {
        }
        public Vector3S(ushort X, ushort Z, ushort Y) :
            this((short)X, (short)Z, (short)Y) {
        }

        public Vector3S(short X, short Z, short Y) {
            x = X;
            y = Y;
            z = Z;
        }


        public static Vector3S MinusAbs(Vector3S a, Vector3S b) { //Get positive int
            return new Vector3S((short)Math.Abs(a.x - b.x), (short)Math.Abs(a.z - b.z), (short)Math.Abs(a.y - b.y));
        }
        public static Vector3S MinusY(Vector3S a, int b) {
            return new Vector3S(a.x, a.z, (short)(a.y - b));
        }
        public static Vector3S operator -(Vector3S a, Vector3S b) {
            return new Vector3S((short)(a.x - b.x), (short)(a.z - b.z), (short)(a.y - b.y));
        }
        public static Vector3S operator +(Vector3S a, Vector3S b) {
            return new Vector3S((short)(a.x + b.x), (short)(a.z + b.z), (short)(a.y + b.y));
        }
        public static Vector3S operator *(Vector3S a, int b) {
            return new Vector3S((short)(a.x * b), (short)(a.z * b), (short)(a.y * b));
        }
        public static Vector3S operator *(Vector3S a, Vector3S b) {
            return new Vector3S((short)(a.x * b.x), (short)(a.z * b.z), (short)(a.y * b.y));
        }
        public static Vector3S operator /(Vector3S a, Vector3S b) {
            return new Vector3S((short)(a.x / b.x), (short)(a.z / b.z), (short)(a.y / b.y));
        }
        public static Vector3S operator /(Vector3S a, int b) {
            return new Vector3S((short)(a.x / b), (short)(a.z / b), (short)(a.y / b));
        }
        public static bool operator ==(Vector3S a, Vector3S b) {
            return (a.x == b.x && a.y == b.y && a.z == b.z);
        }
        public static bool operator !=(Vector3S a, Vector3S b) {
            return !(a.x == b.x && a.y == b.y && a.z == b.z);
        }
        public static bool operator >(Vector3S a, Vector3S b) {
            return a.x * a.x + a.y * a.y + a.z * a.z > b.x * b.x + b.y * b.y + b.z * b.z;
        }
        public static bool operator <(Vector3S a, Vector3S b) {
            return a.x * a.x + a.y * a.y + a.z * a.z < b.x * b.x + b.y * b.y + b.z * b.z;
        }
        public Vector2S Horizontal {
            get { return new Vector2S(x, z); }
            set {
                this.x = value.x;
                this.z = value.z;
            }
        }
        public Vector3S GetMove(short distance, Vector3S towards) {
            Vector3S ret = new Vector3S(x, z, y);
            ret.Move(distance, towards);
            return ret;
        }
        public void Move(short distance, Vector3S towards) {
            Vector3S way = towards - this;
            double length = way.Length;
            x += (short)Math.Round(((way.x / length) * distance));
            y += (short)Math.Round(((way.y / length) * distance));
            z += (short)Math.Round(((way.z / length) * distance));
        }
        public double Length {
            get {
                return Math.Sqrt(x * x + y * y + z * z);
            }
        }

        /// <summary>
        /// Creates a path to another vector
        /// </summary>
        /// <param name="vectorTo">The vector to.</param>
        /// <returns>An enumeration of a path from a vector to a vector</returns>
        public IEnumerable<Vector3S> PathTo(Vector3S vectorTo) {
            Vector3D pos = new Vector3D(this);
            Vector3S rounded = pos.GetRounded();
            while (rounded != vectorTo) {
                yield return rounded;
                pos.Move(1, new Vector3D(vectorTo));
                rounded = pos.GetRounded();
            }
            yield return vectorTo;

            Vector3S tempThis = new Vector3S(this);
            Vector3S a = vectorTo - this;
            Vector3S b = MathUtils.SignVector(a);
            a = MathUtils.AbsVector(a);
            Vector3S c = a * 2;

            int x, z, y;
            if ((a.x >= a.y) && (a.x >= a.z)) {
                x = 0; z = 1; y = 2;
            }
            else if ((a.y >= a.x) && (a.y >= a.z)) {
                x = 1; z = 2; y = 0;
            }
            else {
                x = 2; z = 0; y = 1;
            }

            int right = c.GetDimention(y) - a.GetDimention(x);
            int left = c.GetDimention(z) - a.GetDimention(x);
            for (int j = 0; j < a.GetDimention(x); j++) {
                yield return tempThis;

                if (right > 0) {
                    tempThis.SetValueInDimention(y, (short)(b.GetDimention(y) + tempThis.GetDimention(y)));
                    right -= c.GetDimention(x);
                }

                if (left > 0) {
                    tempThis.SetValueInDimention(z, (short)(b.GetDimention(z) + tempThis.GetDimention(z)));
                    left -= c.GetDimention(x);
                }
                right += c.GetDimention(y);
                left += c.GetDimention(z);
                tempThis.SetValueInDimention(x, (short)(b.GetDimention(x) + tempThis.GetDimention(x)));
            }
            yield return vectorTo;
        }

        /// <summary>
        /// Boxes the specified vector.
        /// </summary>
        /// <param name="a">the vector to</param>
        /// <returns>a box like enumerator</returns>
        public IEnumerable<Vector3S> Box(Vector3S a) {
            for (ushort x = (ushort)Math.Min(this.x, a.x); x < Math.Max(this.x, a.x); x++) 
                for (ushort y = (ushort)Math.Min(this.y, a.y); y < Math.Max(this.y, a.y); y++)
                    for (ushort z = (ushort) Math.Min(this.z, a.z); z < Math.Max(this.z, a.z); z++)
                        yield return new Vector3S(x, z, y);
            
        }

        public IEnumerable<Vector3S> Layer(Vector3S to) {
            return null;
        }

        public short GetDimention(int dimention) {
            switch (dimention) {
                case 0: return x;
                case 1: return z;
                default: return y;
            }
        }

        public void SetValueInDimention(int dimention, short value) {
            switch (dimention) {
                case 0: x = value; return;
                case 1: z = value; return;
                default: y = value; return;
            }
        }
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override string ToString() {
            return String.Format("x:{0} z:{1} y:{2}", x, z, y);
        }
    }
}
