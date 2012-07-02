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
using System.Data;
using MCForge.SQL;
using MCForge.Entity;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections;

namespace MCForge.Utils {
    /// <summary>
    /// Misc utils and extentions.
    /// </summary>
    public static class MiscUtils {


        /// <summary>
        /// Determines whether [contains ignore case] [the specified array].
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="test">The test.</param>
        /// <returns>
        ///   <c>true</c> if [contains ignore case] [the specified array]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsIgnoreCase(this string[] array, string test) {
            for (int i = 0; i < array.Length; i++)
                if (array[i].ToLower() == test.ToLower())
                    return true;
            return false;
        }


        /// <summary>
        /// Gets the object if it exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dict.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static object GetIfExist<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) {
            if (key == null)
                return null;
            if (dict.ContainsKey(key))
                return dict[key];
            return null;
        }

        public static bool RemoveValue<TKey, TValue>(this Dictionary<TKey, IList<TValue>> dict, TKey key, TValue valueToRemove) {
            if (!dict.ContainsKey(key))
                return false;

            foreach (var value in dict) {
                if (value.Key.Equals(key)) {
                    return value.Value.Remove(valueToRemove);
                }
            }
            return false;
        }

        public static void AddValue<TKey, TValue>(this Dictionary<TKey, IList<TValue>> dict, TKey key, TValue valueToAdd) {
            if (!dict.CreateIfNotExist<TKey, IList<TValue>>(key, new List<TValue> { valueToAdd }))
                return;

            foreach (var value in dict)
                if (value.Key.Equals(key))
                    value.Value.Add(valueToAdd);

        }

        /// <summary>
        /// Puts object in list if it does not exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dict.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>If it exists, returns true. Else, returns false</returns>
        public static bool CreateIfNotExist<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) {
            if (!dict.ContainsKey(key)) {
                dict.Add(key, value);
                return false;
            }
            return true;
        }

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
        public static string ToString(this object o) {
            if (o.GetType() == typeof(List<string>)) {
                return MiscUtils.ToString((List<string>)o);
            }
            return o.ToString();
        }

        /// <summary>
        /// Converts the list into a string
        /// </summary>
        /// <param name="list"></param>
        /// <returns>The string value of the list</returns>
        public static string ToString(this List<string> list) {
            string ret = "";
            foreach (string item in list) {
                ret += MiscUtils.ToHexString(item) + "\n";
            }
            return ret;
        }

       /// <summary>
        /// Converts the list into a string
        /// </summary>
        /// <param name="list"></param>
        /// <returns>The string value of the list</returns>
        public static string ToString<T>(this T[] array) {
            string ret = "";
            foreach (T item in array) {
                ret += item.ToString() + "\n";
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
            for (int i = 1; i < hex.Length; i+=2)
                ret += (char)Convert.ToByte(hex[i - 1] + "" + hex[i], 16);
            return ret;
        }


        /// <summary>
        /// Save data to the database
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="p">The player that has the data</param>
        /// <param name="key">The key to locate the value</param>
        public static void Save(this Dictionary<object, object> dict, Player p, object key) {
            var cleanedMessage = key.ToString().MySqlEscape();
            if (dict.ContainsKey(cleanedMessage)) {
                if (!p.IsInTable(cleanedMessage))
                    Database.executeQuery("INSERT INTO extra (setting, value, UID) VALUES ('" + cleanedMessage + "', '" + dict[cleanedMessage].ToString() + "', " + p.UID + ")");
                else
                    Database.executeQuery("UPDATE extra SET value='" + dict[cleanedMessage].ToString() + "' WHERE setting='" + cleanedMessage + "' AND UID=" + p.UID);
            }
        }

        /// <summary>
        /// Changes the value or Creates it if it doesnt exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dict.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void ChangeOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) {
            dict.CreateIfNotExist<TKey, TValue>(key, value);
            dict[key] = value;
        }

        /// <summary>
        /// Get an object with out the need to cast
        /// </summary>
        /// <typeparam name="TKey">The type of key</typeparam>
        /// <typeparam name="TValue">The type of object to return</typeparam>
        /// <param name="dict">The dictionary to use</param>
        /// <param name="key">The key of the dictionary</param>
        /// <returns>An object casted to the specified type, or null if not found</returns>
        /// <remarks>Must have a nullable type interface</remarks>
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) {
            return (TValue)dict.GetIfExist<TKey, TValue>(key);
        }
        /// <summary>
        /// Cleans a string for input into a database
        /// </summary>
        /// <param name="stringToClean">The string to clean.</param>
        /// <returns>A cleaned string</returns>
        [DebuggerStepThrough]
        public static string MySqlEscape(this string stringToClean) {
            if (stringToClean == null) {
                return null;
            }
            return Regex.Replace(stringToClean, @"[\r\n\x00\x1a\\'""]", @"\$0");
        }

        /// <summary>
        /// If an array contains that object it returns <c>true</c> otherwise <c>false</c>
        /// </summary>
        /// <typeparam name="T">Type of the array and object</typeparam>
        /// <param name="theArray">The array to check</param>
        /// <param name="obj">object to check</param>
        /// <returns>If an array contains that object it returns <c>true</c> otherwise <c>false</c></returns>
        public static bool Contains<T>(this T[] theArray, T obj) {
            for (int i = 0; i < theArray.Length; i++) {
                T d = theArray[i];
                if (d.Equals(obj))
                    return true;
            }
            return false;
        }
        public static IEnumerable<Vector3S> GetNeighbors(this Vector3S v) {
            yield return new Vector3S((short)(v.x + 1), v.z, v.y);
            yield return new Vector3S(v.x, (short)(v.z + 1), v.y);
            yield return new Vector3S(v.x, v.z, (short)(v.y + 1));
            yield return new Vector3S((short)(v.x - 1), v.z, v.y);
            yield return new Vector3S(v.x, (short)(v.z - 1), v.y);
            yield return new Vector3S(v.x, v.z, (short)(v.y - 1));
        }


        public static IEnumerable<Vector3S> GetNearBlocks(this Vector3S v, int radiusX, int radiusZ, int radiusY) {
            if (radiusX == radiusZ && radiusZ == radiusY) {
                for (int x = 0; x <radiusX; x++) {
                    for (int z =0; z < radiusX; z++) {
                        for (int y = 0; y < radiusX; y++) {
                            Vector3S ret = new Vector3S((short)(v.x + x), (short)(v.z + z), (short)(v.y + y));
                            double l = (v - ret).Length;
                            if (l <= radiusX) {
                                yield return ret;
                                if (x != 0) {
                                    yield return new Vector3S((short)(v.x - x), (short)(v.z + z), (short)(v.y + y));
                                    if (z != 0) {
                                        yield return new Vector3S((short)(v.x - x), (short)(v.z - z), (short)(v.y + y));
                                        if (y != 0) yield return new Vector3S((short)(v.x - x), (short)(v.z - z), (short)(v.y - y));
                                    }

                                }
                                if (z != 0) {
                                    yield return new Vector3S((short)(v.x + x), (short)(v.z - z), (short)(v.y + y));
                                    if (y != 0) yield return new Vector3S((short)(v.x + x), (short)(v.z - z), (short)(v.y - y));
                                }
                                if (y != 0) {
                                    yield return new Vector3S((short)(v.x + x), (short)(v.z + z), (short)(v.y - y));
                                    if (x != 0) yield return new Vector3S((short)(v.x - x), (short)(v.z + z), (short)(v.y - y));
                                }
                                
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<Vector3S> GetNearBlocksHollow(this Vector3S v, int radiusX, int radiusZ, int radiusY) {
            if (radiusX == radiusZ && radiusZ == radiusY) {
                for (int x = 0; x < radiusX; x++) {
                    for (int z = 0; z < radiusX; z++) {
                        for (int y = 0; y < radiusX; y++) {
                            Vector3S ret = new Vector3S((short)(v.x + x), (short)(v.z + z), (short)(v.y + y));
                            double l = (v - ret).Length;
                            if (l <= radiusX && l >= radiusX - 1) {
                                yield return ret;
                                if (x != 0) {
                                    yield return new Vector3S((short)(v.x - x), (short)(v.z + z), (short)(v.y + y));
                                    if (z != 0) {
                                        yield return new Vector3S((short)(v.x - x), (short)(v.z - z), (short)(v.y + y));
                                        if (y != 0) yield return new Vector3S((short)(v.x - x), (short)(v.z - z), (short)(v.y - y));
                                    }

                                }
                                if (z != 0) {
                                    yield return new Vector3S((short)(v.x + x), (short)(v.z - z), (short)(v.y + y));
                                    if (y != 0) yield return new Vector3S((short)(v.x + x), (short)(v.z - z), (short)(v.y - y));
                                }
                                if (y != 0) {
                                    yield return new Vector3S((short)(v.x + x), (short)(v.z + z), (short)(v.y - y));
                                    if (x != 0) yield return new Vector3S((short)(v.x - x), (short)(v.z + z), (short)(v.y - y));
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}