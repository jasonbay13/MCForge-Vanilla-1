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
        /// <summary>
        /// Gets the neighbors.
        /// </summary>
        /// <param name="v">The position.</param>
        /// <returns></returns>
        public static IEnumerable<Vector3S> GetNeighbors(this Vector3S v) {
            yield return new Vector3S((short)(v.x + 1), v.z, v.y);
            yield return new Vector3S(v.x, (short)(v.z + 1), v.y);
            yield return new Vector3S(v.x, v.z, (short)(v.y + 1));
            yield return new Vector3S((short)(v.x - 1), v.z, v.y);
            yield return new Vector3S(v.x, (short)(v.z - 1), v.y);
            yield return new Vector3S(v.x, v.z, (short)(v.y - 1));
        }


        /// <summary>
        /// Gets the near blocks.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="radiusX">The radius X.</param>
        /// <param name="radiusZ">The radius Z.</param>
        /// <param name="radiusY">The radius Y.</param>
        /// <returns></returns>
        public static IEnumerable<Vector3S> GetNearBlocks(this Vector3S v, int radiusX, int radiusZ, int radiusY) {
            if (radiusX == radiusZ && radiusZ == radiusY) {
                for (int x = 0; x <radiusX; x++) {
                    for (int z =0; z < radiusX; z++) {
                        for (int y = 0; y < radiusX; y++) {
                            Vector3S ret = new Vector3S((short)(v.x + x), (short)(v.z + z), (short)(v.y + y));
                            double l = (v - ret).Distance;
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

        /// <summary>
        /// Gets a layer of near blocks.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="radiusX">The radius X.</param>
        /// <param name="radiusZ">The radius Z.</param>
        /// <param name="radiusY">The radius Y.</param>
        /// <returns></returns>
        public static IEnumerable<Vector3S> GetNearBlocksHollow(this Vector3S v, int radiusX, int radiusZ, int radiusY) {
            if (radiusX == radiusZ && radiusZ == radiusY) {
                for (int x = 0; x < radiusX; x++) {
                    for (int z = 0; z < radiusX; z++) {
                        for (int y = 0; y < radiusX; y++) {
                            Vector3S ret = new Vector3S((short)(v.x + x), (short)(v.z + z), (short)(v.y + y));
                            double l = (v - ret).Distance;
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