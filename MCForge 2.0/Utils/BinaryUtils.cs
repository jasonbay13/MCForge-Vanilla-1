using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace MCForge.Utils {
    static class BinaryUtils  {
        static void Write(this BinaryWriter bw, Vector3S v) {
            bw.Write(v.x);
            bw.Write(v.z);
            bw.Write(v.y);
        }
        static Vector3S ReadVector3S(this BinaryReader br) {
            Vector3S ret = new Vector3S();
            ret.x = br.ReadInt16();
            ret.z = br.ReadInt16();
            ret.y = br.ReadInt16();
            return ret;
        }
        static void Write(this BinaryWriter bw, Vector3S[] list) {
            bw.Write(list.Length);
            for (int i = 0; i < list.Length; i++) {
                bw.Write(list[i]);
            }
        }
        static Vector3S[] ReadVector3SArray(this BinaryReader br) {
            Vector3S[] ret = new Vector3S[br.ReadInt32()];
            for (int i = 0; i < ret.Length; i++) {
                ret[i] = br.ReadVector3S();
            }
            return ret;
        }
        static void Write(this BinaryWriter bw, string[] list) {
            bw.Write(list.Length);
            for (int i = 0; i < list.Length; i++) {
                bw.Write(list[i]);
            }
        }
        static string[] ReadStringArray(this BinaryReader br) {
            string[] ret = new string[br.ReadInt32()];
            for (int i = 0; i < ret.Length; i++) {
                ret[i] = br.ReadString();
            }
            return ret;
        }
    }
}
