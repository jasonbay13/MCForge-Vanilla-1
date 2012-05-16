using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Core;

namespace MCForge.Utils {
    public class Vector3D {
        public double x;
        public double y;
        public double z;
        public Vector3D() {
            z = 0;
            x = 0;
            y = 0;
        }
        public Vector3D(Vector3D v) :
            this(v.x, v.z, v.y) {
        }

        public Vector3D(Vector3S v) :
            this(v.x, v.z, v.y) {
        }

        public Vector3D(ushort X, ushort Z, ushort Y) :
            this((double)X, (double)Z, (double)Y) {
        }

        public Vector3D(double X, double Z, double Y) {
            x = X;
            y = Y;
            z = Z;
        }


        public static Vector3D MinusAbs(Vector3D a, Vector3D b) { //Get positive int
            return new Vector3D((double)Math.Abs(a.x - b.x), (double)Math.Abs(a.z - b.z), (double)Math.Abs(a.y - b.y));
        }
        public static Vector3D MinusY(Vector3D a, int b) {
            return new Vector3D(a.x, a.z, (double)(a.y - b));
        }
        public static Vector3D operator -(Vector3D a, Vector3D b) {
            return new Vector3D((double)(a.x - b.x), (double)(a.z - b.z), (double)(a.y - b.y));
        }
        public static Vector3D operator +(Vector3D a, Vector3D b) {
            return new Vector3D((double)(a.x + b.x), (double)(a.z + b.z), (double)(a.y + b.y));
        }
        public static Vector3D operator *(Vector3D a, int b) {
            return new Vector3D((double)(a.x * b), (double)(a.z * b), (double)(a.y * b));
        }
        public static Vector3D operator *(Vector3D a, Vector3D b) {
            return new Vector3D((double)(a.x * b.x), (double)(a.z * b.z), (double)(a.y * b.y));
        }
        public static Vector3D operator /(Vector3D a, Vector3D b) {
            return new Vector3D((double)(a.x / b.x), (double)(a.z / b.z), (double)(a.y / b.y));
        }
        public static Vector3D operator /(Vector3D a, int b) {
            return new Vector3D((double)(a.x / b), (double)(a.z / b), (double)(a.y / b));
        }
        public static bool operator ==(Vector3D a, Vector3D b) {
            return (a.x == b.x && a.y == b.y && a.z == b.z);
        }
        public static bool operator !=(Vector3D a, Vector3D b) {
            return !(a.x == b.x && a.y == b.y && a.z == b.z);
        }
        public static bool operator >(Vector3D a, Vector3D b) {
            return a.x * a.x + a.y * a.y + a.z * a.z > b.x * b.x + b.y * b.y + b.z * b.z;
        }
        public static bool operator <(Vector3D a, Vector3D b) {
            return a.x * a.x + a.y * a.y + a.z * a.z < b.x * b.x + b.y * b.y + b.z * b.z;
        }
        public Vector2S Horizontal {
            get { return new Vector2S((short)x, (short)z); }
            set {
                this.x = value.x;
                this.z = value.z;
            }
        }
        public Vector3D GetMove(double distance, Vector3D towards) {
            Vector3D ret = new Vector3D(x, z, y);
            ret.Move(distance, towards);
            return ret;
        }
        public void Move(double distance, Vector3D towards) {
            Vector3D way = towards - this;
            double length = way.Length;
            x += (double)Math.Round(((way.x / length) * distance));
            y += (double)Math.Round(((way.y / length) * distance));
            z += (double)Math.Round(((way.z / length) * distance));
        }
        public double Length {
            get {
                return Math.Sqrt(x * x + y * y + z * z);
            }
        }

        public double GetDimention(int dimention) {
            switch (dimention) {
                case 0: return x;
                case 1: return z;
                default: return y;
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
