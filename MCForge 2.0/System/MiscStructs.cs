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
using System.Linq;
using System.Text;
using MCForge.Entity;
using System.Net;
using System.IO.Compression;

namespace MCForge.Core {
    public struct Vector2 {
        public ushort x;
        public ushort y;

        public Vector2(ushort X, ushort Y) {
            x = X;
            y = Y;
        }

        public static Vector2 operator -(Vector2 a, Vector2 b) {
            return new Vector2((ushort)(a.x - b.x),  (ushort)(a.y - b.y));
        }
        public static Vector2 operator +(Vector2 a, Vector2 b) {
            return new Vector2((ushort)(a.x + b.x),  (ushort)(a.y + b.y));
        }
        public static Vector2 operator *(Vector2 a, Vector2 b) {
            return new Vector2((ushort)(a.x * b.x),  (ushort)(a.y * b.y));
        }
        public static Vector2 operator /(Vector2 a, Vector2 b) {
            return new Vector2((ushort)(a.x / b.x),  (ushort)(a.y / b.y));
        }
        public static bool operator ==(Vector2 a, Vector2 b) {
            return (a.x == b.x && a.y == b.y);
        }
        public static bool operator !=(Vector2 a, Vector2 b) {
            return !(a.x == b.x && a.y == b.y);
        }
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override string ToString() {
            return String.Format("x:{0} y:{1}", x, y);
        }
    }
    public struct Vector3 {
        public short x;
        public short y;
        public short z;

        public Vector3(short X, short Z, short Y) {
            x = X;
            y = Y;
            z = Z;
        }
        public Vector3(ushort X, ushort Z, ushort Y) {
            x = (short)X;
            y = (short)Y;
            z = (short)Z;
        }

        public static Vector3 MinusAbs(Vector3 a, Vector3 b){ //Get positive int
            return new Vector3((short)Math.Abs(a.x - b.x), (short)Math.Abs(a.z - b.z), (short)Math.Abs(a.y - b.y));
        }
        public static Vector3 MinusY(Vector3 a, int b){
            return new Vector3(a.x, a.z, (short)(a.y - b));
        }
        public static Vector3 operator -(Vector3 a, Vector3 b) {
            return new Vector3((short)(a.x - b.x), (short)(a.z - b.z), (short)(a.y - b.y));
        }
        public static Vector3 operator +(Vector3 a, Vector3 b) {
            return new Vector3((short)(a.x + b.x), (short)(a.z + b.z), (short)(a.y + b.y));
        }
        public static Vector3 operator *(Vector3 a, Vector3 b) {
            return new Vector3((short)(a.x * b.x), (short)(a.z * b.z), (short)(a.y * b.y));
        }
        public static Vector3 operator /(Vector3 a, Vector3 b) {
            return new Vector3((short)(a.x / b.x), (short)(a.z / b.z), (short)(a.y / b.y));
        }
        public static Vector3 operator /(Vector3 a, int b){
            return new Vector3((short)(a.x / b), (short)(a.z / b), (short)(a.y / b));
        }
        public static bool operator ==(Vector3 a, Vector3 b) {
            return (a.x == b.x && a.y == b.y && a.z == b.z);
        }
        public static bool operator !=(Vector3 a, Vector3 b) {
            return !(a.x == b.x && a.y == b.y && a.z == b.z);
        }
        public static bool operator >(Vector3 a, Vector3 b){
            return a.x * a.x + a.y * a.y + a.z * a.z > b.x * b.x + b.y * b.y + b.z * b.z;
        }
        public static bool operator <(Vector3 a, Vector3 b) {
            return a.x * a.x + a.y * a.y + a.z * a.z < b.x * b.x + b.y * b.y + b.z * b.z;
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

    public static class Colors {
        public const string black = "&0";
        public const string navy = "&1";
        public const string green = "&2";
        public const string teal = "&3";
        public const string maroon = "&4";
        public const string purple = "&5";
        public const string gold = "&6";
        public const string silver = "&7";
        public const string gray = "&8";
        public const string blue = "&9";
        public const string lime = "&a";
        public const string aqua = "&b";
        public const string red = "&c";
        public const string pink = "&d";
        public const string yellow = "&e";
        public const string white = "&f";

        public static string Parse(string str) {
            switch (str.ToLower()) {
                case "black": return black;
                case "navy": return navy;
                case "green": return green;
                case "teal": return teal;
                case "maroon": return maroon;
                case "purple": return purple;
                case "gold": return gold;
                case "silver": return silver;
                case "gray": return gray;
                case "blue": return blue;
                case "lime": return lime;
                case "aqua": return aqua;
                case "red": return red;
                case "pink": return pink;
                case "yellow": return yellow;
                case "white": return white;
                default: return "";
            }
        }
        public static string Name(string str) {
            switch (str) {
                case black: return "black";
                case navy: return "navy";
                case green: return "green";
                case teal: return "teal";
                case maroon: return "maroon";
                case purple: return "purple";
                case gold: return "gold";
                case silver: return "silver";
                case gray: return "gray";
                case blue: return "blue";
                case lime: return "lime";
                case aqua: return "aqua";
                case red: return "red";
                case pink: return "pink";
                case yellow: return "yellow";
                case white: return "white";
                default: return "";
            }
        }
    }

	public struct packet
	{
		public byte[] bytes;

		#region Constructors
		public packet(byte[] data)
		{
			bytes = data;
		}
		public packet(packet p)
		{
			bytes = p.bytes;
		}
		#endregion
		#region Adds
		public void AddStart(byte[] data)
		{
			byte[] temp = bytes;

			bytes = new byte[temp.Length + data.Length];

			data.CopyTo(bytes, 0);
			temp.CopyTo(bytes, data.Length);
		}

		public void Add(byte[] data)
		{
			if (bytes == null)
			{
				bytes = data;
			}
			else
			{
				byte[] temp = bytes;

				bytes = new byte[temp.Length + data.Length];

				temp.CopyTo(bytes, 0);
				data.CopyTo(bytes, temp.Length);
			}
		}
		public void Add(sbyte a)
		{
			Add(new byte[1] { (byte)a });
		}
		public void Add(byte a)
		{
			Add(new byte[1] { a });
		}
		public void Add(types a)
		{
			Add((byte)a);
		}
		public void Add(short a)
		{
			Add(HTNO(a));
		}
		public void Add(ushort a)
		{
			Add(HTNO(a));
		}
		public void Add(int a)
		{
			Add(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(a)));
		}
		public void Add(string a)
		{
			Add(a, a.Length);
		}
		public void Add(string a, int size)
		{
			Add(Player.enc.GetBytes(a.PadRight(size).Substring(0, size)));
		}
		#endregion
		#region Sets
		public void Set(int offset, short a)
		{
			HTNO(a).CopyTo(bytes, offset);
		}
		public void Set(int offset, ushort a)
		{
			HTNO(a).CopyTo(bytes, offset);
		}
		public void Set(int offset, string a, int length)
		{
			Player.enc.GetBytes(a.PadRight(length).Substring(0, length)).CopyTo(bytes, offset);
		}
		#endregion

		public void GZip()
		{
			using (var ms = new System.IO.MemoryStream())
			{

				using (var gs = new GZipStream(ms, CompressionMode.Compress, true))
					gs.Write(bytes, 0, bytes.Length);

				ms.Position = 0;
				bytes = new byte[ms.Length];
				ms.Read(bytes, 0, (int)ms.Length);
			}
		}

		#region == Host <> Network ==
		public static byte[] HTNO(ushort x)
		{
			byte[] y = BitConverter.GetBytes(x); Array.Reverse(y); return y;
		}
		public static ushort NTHO(byte[] x, int offset)
		{
			byte[] y = new byte[2];
			Buffer.BlockCopy(x, offset, y, 0, 2); Array.Reverse(y);
			return BitConverter.ToUInt16(y, 0);
		}
		public static byte[] HTNO(short x)
		{
			byte[] y = BitConverter.GetBytes(x); Array.Reverse(y); return y;
		}
		#endregion

		public enum types
		{
			Message = 13,
			MOTD = 0,
			MapStart = 2,
			MapData = 3,
			MapEnd = 4,
			SendSpawn = 7,
			SendDie = 12,
			SendBlockchange = 6,
			SendKick = 14,
			SendPing = 1,

			SendPosChange = 10,
			SendRotChange = 11,
			SendPosANDRotChange = 9,
			SendTeleport = 8,

		}
	}
}
