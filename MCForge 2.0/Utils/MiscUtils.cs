using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MCForge.SQL;
using MCForge.Entity;

namespace MCForge.Utils {
<<<<<<< HEAD
	public static class MiscUtils {

	    
        /// <summary>
        /// Gets the object if it exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dict.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
		public static object GetIfExist(this Dictionary<object, object> dict, object key) {
			if (dict.ContainsKey(key))
				return dict[key];
			return null;
		}

        /// <summary>
        /// Puts object in list if it does not exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dict.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
		public static void CreateIfNotExist(this Dictionary<object, object> dict, object key, object value) {
			if (!dict.ContainsKey(key))
				dict.Add(key, value);
		}
		
		/// <summary>
		/// Save data to the database
		/// </summary>
		/// <param name="dict"></param>
		/// <param name="p">The player that has the data</param>
		/// <param name="key">The key to locate the value</param>
		public static void Save(this Dictionary<object, object> dict, Player p, object key) {
			if (dict.ContainsKey(key))
			{
				if (!p.IsInTable(key))
					Database.executeQuery("INSERT INTO extra (key, value, UID) VALUES ('" + key.ToString() + "', '" + dict[key].ToString() + "', " + p.UID + ")");
				else
					Database.executeQuery("UPDATE extra SET value='" + dict[key].ToString() + "' WHERE key='" + key.ToString() + "' AND UID=" + p.UID);
			}
		}
		
		/// <summary>
		/// Convert the list into a string
		/// </summary>
		/// <param name="list"></param>
		/// <returns>The string value of the list</returns>
		public static string ListToString(this List<string> list) {
			string ret = "";
			foreach (string item in list) {
				ret += item + "\n";
			}
			return ret;
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
        /// Converts List&lt;string&gt; to a multiple lines hex string.
        /// </summary>
        /// <param name="list">The list to convert</param>
        /// <returns>A multiple lines hex string</returns>
        public static string ToHexString(this List<string> list) {
            string ret = "";
            foreach (string item in list) {
                byte[] b = ASCIIEncoding.ASCII.GetBytes(item);
                for (int i = 0; i < b.Length; i++) {
                    ret += Convert.ToString(b[i], 16);
                }
                ret += "\n";
            }
            return ret;
        }
        /// <summary>
        /// Converts a multiple line hex string to a List&lt;string&gt;. 
        /// </summary>
        /// <param name="list">The multiple line hex string</param>
        /// <returns>A List&lt;string&gt;</returns>
        public static List<string> ToListFromHexString(this string list) {
            string[] lines = list.Split('\n');
            List<string> ret = new List<string>();
            for (int i = 0; i < lines.Length; i++) {
                if (lines[i].Length % 2 != 0) break;
                string tmp = "";
                for (int ii = 0; ii < lines[i].Length; ii += 2) {
                    tmp += (char)Convert.ToByte(lines[i].Substring(ii, 2), 16);
                }
                ret.Add(tmp);
            }
            return ret;
        }

	}
}
