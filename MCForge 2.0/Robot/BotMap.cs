using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.World;
using MCForge.Core;

namespace MCForge.Robot {
    class BotMap {
        public BotMap(Level l) {
            AirMap = new bool[l.Size.x, l.Size.z, l.Size.y];//return x + z * Size.x + y * Size.x * Size.z;
            for (int i = 0; i < l.Data.Length; i++) {
                Vector3 pos = l.IntToPos(i);
                AirMap[pos.x, pos.z, pos.y] = isAir(l.GetBlock(i));
            }
            for (int x = 0; x < AirMap.GetLength(0); x++) {
                for (int z = 0; z < AirMap.GetLength(1); z++) {
                    for (int y = 0; y < AirMap.GetLength(2); y++) {

                    }
                }
            }
        }
        bool[, ,] AirMap;
        bool[, ,] PosMap;
        bool isAir(byte block) {
            return block == Block.BlockList.AIR;
        }
        bool onlyAirBetween(Vector3 start, Vector3 end) {
            PreciseVector3 s = new PreciseVector3(start);
            PreciseVector3 e = new PreciseVector3(end);
            while ((s - e).Length > 1) {
                if (!AirMap[s.GetX(), s.GetZ(), s.GetY()]) return false;
            }
            return true;
        }
        List<PreciseVector3> fromList=new List<PreciseVector3>();
        List<PreciseVector3> toList=new List<PreciseVector3>();
        int Add(PreciseVector3 from, PreciseVector3 to) {
            fromList.Add(from);
            toList.Add(to);
            return fromList.Count;
        }
        class WayPoint {
            public PreciseVector3 Position;
            public List<PreciseVector3> Connecteds = new List<PreciseVector3>();
            public List<double> Distances = new List<double>();
        }
        public class PreciseVector2 {
            public double x;
            public double z;
            public PreciseVector2() {
                this.x = 0;
                this.z = 0;
            }
            public PreciseVector2(double X, double Z) {
                x = X;
                z = Z;
            }

            public static PreciseVector2 operator -(PreciseVector2 a, PreciseVector2 b) {
                return new PreciseVector2((double)(a.x - b.x), (double)(a.z - b.z));
            }
            public static PreciseVector2 operator +(PreciseVector2 a, PreciseVector2 b) {
                return new PreciseVector2((double)(a.x + b.x), (double)(a.z + b.z));
            }
            public static PreciseVector2 operator *(PreciseVector2 a, PreciseVector2 b) {
                return new PreciseVector2((double)(a.x * b.x), (double)(a.z * b.z));
            }
            public static PreciseVector2 operator /(PreciseVector2 a, PreciseVector2 b) {
                return new PreciseVector2((double)(a.x / b.x), (double)(a.z / b.z));
            }
            public static bool operator ==(PreciseVector2 a, PreciseVector2 b) {
                return (a.x == b.x && a.z == b.z);
            }
            public static bool operator !=(PreciseVector2 a, PreciseVector2 b) {
                return !(a.x == b.x && a.z == b.z);
            }
            public PreciseVector2 GetMove(double distance, PreciseVector2 towards) {
                PreciseVector2 ret = new PreciseVector2(x, z);
                ret.Move(distance, towards);
                return ret;
            }
            public void Move(double distance, PreciseVector2 towards) {
                PreciseVector2 way = towards - this;
                double length = way.Length;
                x += (double)((way.x / length) * distance);
                z += (double)((way.z / length) * distance);
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
        public class PreciseVector3 : PreciseVector2 {
            public double y;
            public PreciseVector3()
                : base() {
                y = 0;
            }
            public PreciseVector3(PreciseVector3 v) : this(v.x, v.z, v.y) { }
            public PreciseVector3(Vector3 v) : this(v.x, v.y, v.z) { }
            public PreciseVector3(double X, double Z, double Y)
                : base(X, Z) {
                y = Y;
            }
            public PreciseVector3(short X, short Z, short Y) : this((double)X, (double)Z, (double)Y) { }

            public static PreciseVector3 MinusAbs(PreciseVector3 a, PreciseVector3 b) { //Get positive int
                return new PreciseVector3((double)Math.Abs(a.x - b.x), (double)Math.Abs(a.z - b.z), (double)Math.Abs(a.y - b.y));
            }
            public static PreciseVector3 MinusY(PreciseVector3 a, int b) {
                return new PreciseVector3(a.x, a.z, (double)(a.y - b));
            }
            public static PreciseVector3 operator -(PreciseVector3 a, PreciseVector3 b) {
                return new PreciseVector3((double)(a.x - b.x), (double)(a.z - b.z), (double)(a.y - b.y));
            }
            public static PreciseVector3 operator +(PreciseVector3 a, PreciseVector3 b) {
                return new PreciseVector3((double)(a.x + b.x), (double)(a.z + b.z), (double)(a.y + b.y));
            }
            public static PreciseVector3 operator *(PreciseVector3 a, int b) {
                return new PreciseVector3((double)(a.x * b), (double)(a.z * b), (double)(a.y * b));
            }
            public static PreciseVector3 operator *(PreciseVector3 a, PreciseVector3 b) {
                return new PreciseVector3((double)(a.x * b.x), (double)(a.z * b.z), (double)(a.y * b.y));
            }
            public static PreciseVector3 operator /(PreciseVector3 a, PreciseVector3 b) {
                return new PreciseVector3((double)(a.x / b.x), (double)(a.z / b.z), (double)(a.y / b.y));
            }
            public static PreciseVector3 operator /(PreciseVector3 a, int b) {
                return new PreciseVector3((double)(a.x / b), (double)(a.z / b), (double)(a.y / b));
            }
            public static bool operator ==(PreciseVector3 a, PreciseVector3 b) {
                return (a.x == b.x && a.y == b.y && a.z == b.z);
            }
            public static bool operator !=(PreciseVector3 a, PreciseVector3 b) {
                return !(a.x == b.x && a.y == b.y && a.z == b.z);
            }
            public static bool operator >(PreciseVector3 a, PreciseVector3 b) {
                return a.x * a.x + a.y * a.y + a.z * a.z > b.x * b.x + b.y * b.y + b.z * b.z;
            }
            public static bool operator <(PreciseVector3 a, PreciseVector3 b) {
                return a.x * a.x + a.y * a.y + a.z * a.z < b.x * b.x + b.y * b.y + b.z * b.z;
            }
            public int GetX() {
                return (int)Math.Round(x);
            }
            public int GetY() {
                return (int)Math.Round(y);
            }
            public int GetZ() {
                return (int)Math.Round(z);
            }
            public PreciseVector2 Horizontal {
                get { return this; }
                set {
                    this.x = value.x;
                    this.z = value.z;
                }
            }
            public PreciseVector3 GetMove(double distance, PreciseVector3 towards) {
                PreciseVector3 ret = new PreciseVector3(x, z, y);
                ret.Move(distance, towards);
                return ret;
            }
            public void Move(double distance, PreciseVector3 towards) {
                PreciseVector3 way = towards - this;
                double length = way.Length;
                x += (double)((way.x / length) * distance);
                y += (double)((way.y / length) * distance);
                z += (double)((way.z / length) * distance);
            }
            public double Length {
                get {
                    return Math.Sqrt(x * x + y * y + z * z);
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
}
