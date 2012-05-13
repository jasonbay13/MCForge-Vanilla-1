using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MCForge.SQL;
using MCForge.Entity;

namespace MCForge.Utils {
    /// <summary>
    /// Misc utils and extentions.
    /// </summary>
    public static class MiscUtils {

	    
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

        /// <summary>
        /// Puts object in list if it does not exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dict.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
		public static void CreateIfNotExist<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) {
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


	}
}
