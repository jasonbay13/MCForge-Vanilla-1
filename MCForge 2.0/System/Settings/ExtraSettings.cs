using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using MCForge.Core;
using System.IO;
using System.Drawing;

namespace MCForge.Utilities.Settings {
    public abstract class ExtraSettings {
        public abstract string SettingsName { get; }
        public abstract List<SettingNode> Values { get; }
        public abstract void OnLoad();
        public abstract void Save();
        public abstract string PropertiesPath { get; }

        /// <summary>
        /// Gets a setting
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The setting values, use [0] at end if it only has 1 value</returns>
        public string[] GetSettingArray(string key) {
            if (key == null) return new[] { "" };
            key = key.ToLower();
            var pair = GetPair(key);
            return pair == null ? new[] { "" } : GetPair(key).Value.Split(','); //We don't want to return a null object
        }


        /// <summary>
        /// Gets a setting
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The setting value</returns>
        /// <remarks>Returns the first value if multiple values are present</remarks>
        public  string GetSetting(string key) {
            key = key.ToLower();
            return GetSettingArray(key)[0];
        }

        /// <summary>
        /// Gets a setting
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The setting value specified by the key, or -1 if the setting is not found or could not be parsed</returns>
        public int GetSettingInt(string key) {
            key = key.ToLower();
            int i;
            var pair = GetPair(key);
            if (pair == null)
                return -1;
            try {
                int.TryParse(GetPair(key).Value, out i);
                return i;
            }
            catch {
                Logger.Log(String.Format("{0}: integer expected as first value for '{1}'", SettingsName, key), Color.Red, Color.Black);
                return -1;
            }
        }

        /// <summary>
        /// Gets a setting
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The setting value specified by the key, or false if the setting is not found</returns>
        public bool GetSettingBoolean(string key) {
            key = key.ToLower();
            return GetSetting(key).ToLower() == "true";
        }

        /// <summary>
        /// Set the setting
        /// </summary>
        /// <param name="key">key to save value to</param>
        /// <param name="description">Write a description (optional)</param>
        /// <param name="values">for each string in values, it will be seperated by a comma ','</param>
        /// <remarks>If the setting does not exist, it will create a new one</remarks>
        public  void SetSetting(string key, string description = null, params string[] values) {
            key = key.ToLower();
            var pair = GetPair(key);
            if (pair == null) {
                pair = new SettingNode(key, string.Join(",", values), description);
                Values.Add(pair);
                return;
            }

            pair.Description = description;
            pair.Value = string.Join(",", values);
        }

        /// <summary>
        /// Set the setting
        /// </summary>
        /// <param name="key">key to save value to</param>
        /// <param name="value">value (or multiple values sperated by a comma ',') to set setting to</param>
        /// <param name="description">Write a description (optional)</param>
        /// <remarks>If the setting does not exist, it will create a new one</remarks>
        public void SetSetting(string key, int value, string description = null) {
            key = key.ToLower();
            var pair = GetPair(key);
            if (pair == null) {
                pair = new SettingNode(key, value.ToString(CultureInfo.InvariantCulture), description);
                Values.Add(pair);
                return;
            }
            pair.Description = description;
            pair.Value = string.Join(",", value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Set the setting
        /// </summary>
        /// <param name="key">key to save value to</param>
        /// <param name="value">value to set setting to</param>
        /// <param name="description">Write a description (optional)</param>
        /// <remarks>If the setting does not exist, it will create a new one</remarks>
        public void SetSetting(string key, bool value, string description = null) {
            key = key.ToLower();
            var pair = GetPair(key);
            if (pair == null) {
                pair = new SettingNode(key, value.ToString(CultureInfo.InvariantCulture), description);
                Values.Add(pair);
                return;
            }

            pair.Description = description;
            pair.Value = string.Join(",", value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Loads all the settings into the memory, if no properties file is found it will return an empty list
        /// </summary>
        /// <returns>A list of setting nodes</returns>
        public List<SettingNode> LoadSettings() {
            if (!File.Exists(PropertiesPath))
                return new List<SettingNode>();
            var text = File.ReadAllLines(PropertiesPath);
            var Values = new List<SettingNode>();
            for (int i = 0; i < text.Count(); i++) {
                string read = text[i];
                SettingNode pair;

                if (String.IsNullOrWhiteSpace(read))
                    continue;


                if (read[0] == '#' && (i + 1 < text.Count()) ? text[i + 1][0] != '#' && !String.IsNullOrWhiteSpace(text[i + 1]) : false) {
                    i++;
                    var split = text[i].Split('=');
                    pair = new SettingNode(split[0].Trim().ToLower(),
                                           String.Join("=", split, 1, split.Length - 1).Trim(),
                                           read.Substring(1));
                }
                else {
                    if (read[0] != '#') {
                        var split = text[i].Split('=');
                        pair = new SettingNode(split[0].Trim().ToLower(),
                                               String.Join("=", split, 1, split.Length - 1).Trim(),
                                               null);
                    }
                    else pair = new SettingNode(null, read, null);
                }
                Values.Add(pair);
            }

            return Values;

        }

        internal SettingNode GetPair(string key) {
            key = key.ToLower();
            return Values.FirstOrDefault(pair => pair.Key.ToLower() == key.ToLower());
        }
    }
}
