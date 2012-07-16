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
using System.Globalization;

namespace MCForge.Utils {

    /// <summary>
    /// Set of utils for manipulating strings
    /// </summary>
    public static class StringUtils {
        #region common
        /// <summary>
        /// Change your string's first character to uppercase 
        /// </summary>
        /// <param name="StringToChange">The String message to change</param>
        /// <returns>String Version of CapitolizeFirstChar</returns>
        public static string CapitolizeFirstChar(string StringToChange) {
            if (String.IsNullOrWhiteSpace(StringToChange))
                return StringToChange;

            //Ex: StringToChange = "foobar"
            //Sets "foobar" to "Ffoobar"
            StringToChange = Char.ToUpper(StringToChange[0]) + StringToChange;

            //Removes the 2nd char (which was the char to capitolize) 
            //resulting in Foobar
            StringToChange = StringToChange.Remove(1, 1);
            return StringToChange;
        }

        public static string Truncate(string source, int length, string ending = "...") {
            if (source.Length > length) {
                return source.Substring(0, length) + ending;
            }
            return source;
        }
        /// <summary>
        /// Checks if a string is numeric
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <returns></returns>
        public static bool IsNumeric(string s) {
            int parser;
            return int.TryParse(s, out parser);
        }
        /// <summary>
        /// Change the specified string to title case
        /// EX: String = "foo bar"
        /// Returns "Foo Bar"
        /// </summary>
        /// <param name="StringToChange">The string to change</param>
        /// <returns>A string version of TitleCase</returns>
        public static string TitleCase(string StringToChange) {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(StringToChange.ToLower());
        }

        //TODO: Comment...
        /// <summary>
        /// sdfsdf
        /// </summary>
        /// <param name="message">sdfdsf</param>
        /// <returns>sdfsdf</returns>
        public static bool ContainsBadChar(string message) {
            foreach (char ch in message)
                if (ch < 32 || ch > 128 || ch == '&')
                    return true;
            return false;

        }
        #endregion

        #region Extenders
        
        /*public static string ToString(this object o) {
            Type t = o.GetType();
            if (t == typeof(string)) {
                return ToHexString((string)o);
            }
            if (t == typeof(List<string>)) {
                return ToHexString((List<string>)o);
            }
            if (t == typeof(List<Vector3S>)) {
                return ToHexString((List<Vector3S>)o);
            }
            if (t == typeof(Vector3S)) {
                return ToHexString((Vector3S)o);
            }
            if (t == typeof(byte) || t == typeof(ushort) || t == typeof(uint) || t == typeof(ulong)) {
                return ((ulong)o).ToString();
            }
            if (t == typeof(sbyte) || t == typeof(short) || t == typeof(int) || t == typeof(long)) {
                return ((long)o).ToString();
            }
            if (t == typeof(double)) {
                return ((double)o).ToString();
            }
            if (t == typeof(float)) {
                return ((float)o).ToString();
            }
            if (t == typeof(bool)) {
                return ((bool)o).ToString();
            }
            return o.ToString();
        }*/

        #region Vector3S&Strings
        public static string ToHexString(this Vector3S v) {
            return ToHexString(v.ToString());
        }
        /// <summary>
        /// Converts the list into a string.
        /// </summary>
        /// <param name="list"></param>
        /// <returns>The string value of the list</returns>
        public static string ToHexString(this List<Vector3S> list) {
            string ret = "";
            foreach (Vector3S item in list) {
                ret += ToHexString(item) + "\n"; //using Hex to not have to care about whats Vector3S.ToString() is doing
            }
            return ret;
        }

        public static void FromHexString(this Vector3S v, string s) {
            v.FromString(s.FromHexString());
        }
        public static void AddHexString(this List<Vector3S> list, string str) {
            string[] split = str.Split('\n');
            Vector3S[] vectors = new Vector3S[split.Length];
           for(int i=0;i<vectors.Length;i++){
               vectors[i] = new Vector3S();
                vectors[i].FromHexString(split[i]);
            }
           list.AddRange(vectors); //it's faster to add them all i think
        }
        #endregion

        #region String&String

        #region String
        /// <summary>
        /// Converts a string to a hexadecimal string
        /// </summary>
        /// <param name="s">The string</param>
        /// <returns>The hexadecimal string</returns>
        public static string ToHexString(this string s) {
            string ret = "";
            foreach (char c in s)
                ret += Convert.ToString((byte)c, 16);
            return ret;
        }

        /// <summary>
        /// Converts a hexadecimal string to a normal string
        /// </summary>
        /// <param name="hex">The hexadecimal string</param>
        /// <returns>The string</returns>
        public static string FromHexString(this string hex) {
            string ret = "";
            for (int i = 1; i < hex.Length; i += 2)
                ret += (char)Convert.ToByte(hex[i - 1] + "" + hex[i], 16);
            return ret;
        }
        #endregion

        #region List
        /// <summary>
        /// Converts the list into a string.
        /// </summary>
        /// <param name="list"></param>
        /// <returns>The string value of the list</returns>
        public static string ToHexString(this List<string> list) {
            string ret = "";
            foreach (string item in list) {
                ret += ToHexString(item) + "\n";
            }
            return ret;
        }

        /// <summary>
        /// Adds multiple hexadecimal strings splitted by \n as normal strings to this list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="hexs">Multiple hexadecimal string splitted by \n</param>
        public static void AddHexstrings(this List<string> list, string hexs) {
            string[] hex = hexs.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in hex)
                list.Add(s.FromHexString());
        }
        #endregion

        #region Dictionary
        /// <summary>
        /// Converts the dictionary to a string
        /// </summary>
        /// <param name="dict"><The dictionary/param>
        /// <returns>A string representing the dictionary</returns>
        public static string ToString(this Dictionary<string, string> dict) {
            string ret = "";
            foreach (string key in dict.Keys)
                ret += key.ToString() + ":" + dict[key].ToString() + "\n";
            return ret;
        }

        /// <summary>
        /// Converts a string representing a dictionary to this dictionary
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="hexKeyValues">The string representing a dictionary</param>
        public static void AddHexstrings(this Dictionary<string, string> dict, string hexKeyValues) {
            string[] keyvalue = hexKeyValues.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in keyvalue) {
                string[] kv = s.Split(':');
                dict[kv[0].FromHexString()] = kv[1].FromHexString();
            }
        }
        #endregion

        #endregion
        #endregion

    }
}
