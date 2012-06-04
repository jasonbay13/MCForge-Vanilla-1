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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace MCForge.Utils.Settings {
    /// <summary>
    /// Settings Utility
    /// </summary>
    public class ServerSettings {

        internal const byte Version = 7;
        internal static string Salt { get; set; }
        static List<string> validcolors = new List<string>( new string[] { "a", "b", "c", "d", "e", "f", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" } );

        private static bool _initCalled;
        private static List<SettingNode> Values;
        private static readonly SettingNode[] defaultValues = {
                    new SettingNode("ServerName", "[MCForge] Default", "Name of your server"),
                    new SettingNode("Wom-Alternate_Name", "[MCForge] Default", "Name of your server on WoM direct"),
                    new SettingNode("Port", "25565", null),
                    new SettingNode("Use-UPnP", "false", "if enabled will automatically forward port for you, this is not recommended, and will not work in some cases"),
                    new SettingNode("MOTD", "Welcome to my server!", "Message that shows up when you start server"),
                    new SettingNode("MaxPlayers", "20", "Max players that can play on your server at a time"),
                    new SettingNode("Public", "true", "if set to true, your server will show up on MCForge.net server list and Minecraft.net's server list"),
                    new SettingNode("Wom-Server_Description", "A MCForge server", "A Description of your server."),
                    new SettingNode("Wom-Server_Flags", "[MCForge]", null),
                    new SettingNode("UsingConsole", "true", "set to \"false\" if you want gui. If using mono set to \"true\""),
                    new SettingNode("ShutdownMessage", "Server shutting down!", "Message to show when server is shutting down"),
                    new SettingNode("WelcomeMessage", "Welcome $name to $server<br>enjoy your stay", "Welcome message, to signify a line break use \"<br>\""),
                    new SettingNode("ConfigPath", "config/", "File path for group player properties, do not mess with unless you know what you are doing"),
                    new SettingNode("MessageAppending", "true", "allow use of message appending, ex using \">\" at the end of your message will allow you to finish your statement on a new chat segment"),
                    new SettingNode("DefaultGroup", "guest", "The name of the default group, if it doesn't exist it will cause problems"),
                    new SettingNode("Offline", "false", "if set to true, it will skip authintication, causing a major security flaw"),
                    new SettingNode("AllowHigherRankTp", "true", "Allow players of a lower rank to teleport to a user of a higher rank"),
                    new SettingNode("Main-Level", "main", "The name of the main level. If tihs is empty or doesn't exist, it will generate a flat level."),
                    new SettingNode("DatabaseType", "sqlite", "The type of database you want to use (mysql/sqlite)"),
                    new SettingNode("MySQL-IP", "127.0.0.1", "The IP of the sql (mysql/sqlite) database"),
                    new SettingNode("MySQL-Port", "3306", "The port for the mysql database (sqlite does not need a port, leave this blank)"),
                    new SettingNode("MySQL-Username", "root", "The username for the mysql database"),
                    new SettingNode("MySQL-Password", "password", "The password for the mysql database"),
                    new SettingNode("MySQL-Pooling", "True", null),
                    new SettingNode("MySQL-DBName", "MCForge", "The database name for MySQL"),
                    new SettingNode("SQLite-Filepath", "_mcforge.db", "The filepath for the database"),
                    new SettingNode("SQLite-Pooling", "True", null),
                    new SettingNode("Database-Queuing", "False", null),
                    new SettingNode("Database-Flush_Interval", "20", null),
                    new SettingNode("IRC-Enabled", "false", "If set to true, IRC is enabled."),
                    new SettingNode("IRC-Server", "127.0.0.1", "IRC server to connect to"),
                    new SettingNode("IRC-Port", "6667", "IRC server port"),
                    new SettingNode("IRC-Nickname", "MCForge-" + MathUtils.Random.Next(1000, 9999), "IRC nickname"),
                    new SettingNode("IRC-Channel", "#", "IRC channel to connect to"),
                    new SettingNode("IRC-OPChannel", "#", "IRC operator channel to connect to"),
                    new SettingNode("IRC-NickServ", "password", "IRC NickServ password (optional when IRC is enabled)"),
                    new SettingNode("AgreeingToRules", "true", "If set to true players below op will need to read the rules and agree before they can use commands"),
                    new SettingNode("$Before$Name", "true", "Puts a $ before the name of the variable if used. For example, if $name was typed in chat it would bring up $headdetect instead of just headdetect"),
                    new SettingNode("ReviewModeratorPerm", "80", "Determines what rank gets to use /review next and /review remove. Use the permission number, not the rank name!"),
                    new SettingNode("OpChatPermission", "80", "Determines what rank can use the operator chat"),
                    new SettingNode("AdminChatPermission", "100", "Determines what rank can use the admin chat"),
                    new SettingNode("DefaultColor", "&a", "Determines the server's default color"),
                    new SettingNode("BackupFiles", "true", "If set to true, files will be backed up to the backup folder"),
                    new SettingNode("BackupInterval", "300", "the interval to backup your files (in seconds)")
               };


        /// <summary>
        /// This event is triggered when a setting node is changed in anyway
        /// </summary>
        public static EventHandler<SettingsChangedEventArgs> OnSettingChanged;


        /// <summary>
        /// Starts the Settings Object
        /// </summary>
        /// <remarks>Must be called before any methods are invoked</remarks>
        public static void Init() {
            //Do not set to static ServerSettings(){}

            if (_initCalled)
                throw new ArgumentException("\"Settings.Init()\" can only be called once");

            GenerateSalt();

            _initCalled = true;
            Values = new List<SettingNode>(defaultValues);
            if (!Directory.Exists(FileUtils.PropertiesPath))
                Directory.CreateDirectory(FileUtils.PropertiesPath);

            if (!File.Exists(FileUtils.PropertiesPath + "server.properties")) {

                using (var writer = File.CreateText(FileUtils.PropertiesPath + "server.properties")) {
                    foreach (var v in Values) {
                        writer.WriteLine(v.Description == null
                                             ? string.Format("{0}={1}\n", v.Key.ToLower(), v.Value)
                                             : string.Format("#{0}\r\n{1}={2}\n", v.Description, v.Key.ToLower(), v.Value));

                    }
                }
            }

            LoadSettings();
            UpgradeSettings();
        }

        private static void UpgradeSettings() {
            if (Values.Count < defaultValues.Length) {
                Logger.Log("Upgrading settings...", LogType.Warning);
                for(int i = 0; i < defaultValues.Length; i++) {
                    var value = defaultValues[i];
                    if (!HasKey(value.Key))
                        Values.Insert(i, value);
                }

                Save();
            }
        }



        /// <summary>
        /// Gets a setting
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The setting values, use [0] at end if it only has 1 value</returns>
        public static string[] GetSettingArray(string key) {
            if (key == null) return new[] { "" };
            key = key.ToLower();
            var pair = GetNode(key);
            return pair == null ? new[] { "" } : GetNode(key).Value.Split(','); //We don't want to return a null object
        }


        /// <summary>
        /// Gets a setting
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The setting value</returns>
        /// <remarks>Returns the first value if multiple values are present</remarks>
        public static string GetSetting(string key) {
            if (key == "DefaultColor") { //Gotta check if it's valid
                string value = GetSettingArray(key)[0];
                if (value.Length > 2) { Logger.Log("Invalid DefaultColor length!"); return "&a"; }
                if (value[0] != '&' && value[0] != '%') { Logger.Log("Invalid DefaultColor sign. Use % or &!"); return "&a"; }
                bool okay = false;
                foreach (string s in validcolors) { if (value[1] == char.Parse(s)) { okay = true; } }
                if (!okay) { Logger.Log("Invalid DefaultColor color!"); return "&a"; }       
            }
            key = key.ToLower();
            return GetSettingArray(key)[0];
        }

        /// <summary>
        /// Gets a setting
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The setting value specified by the key, or -1 if the setting is not found or could not be parsed</returns>
        public static int GetSettingInt(string key) {
            key = key.ToLower();
            int i;
            var pair = GetNode(key);
            if (pair == null)
                return -1;
            try {
                int.TryParse(GetNode(key).Value, out i);
                return i;
            }
            catch {
                Logger.Log("ServerSettings: integer expected as first value for '" + key + "'", Color.Red, Color.Black);
                return -1;
            }
        }

        /// <summary>
        /// Gets a setting
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The setting value specified by the key, or false if the setting is not found</returns>
        public static bool GetSettingBoolean(string key) {
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
        public static void SetSetting(string key, string description = null, params string[] values) {
            key = key.ToLower();
            var pair = GetNode(key);
            if (pair == null) {
                pair = new SettingNode(key, string.Join(",", values), description);
                Values.Add(pair);
                if (OnSettingChanged != null)
                    OnSettingChanged(null, new SettingsChangedEventArgs(key, null, pair.Value));
                return;
            }
            if (OnSettingChanged != null)
                OnSettingChanged(null, new SettingsChangedEventArgs(key, pair.Value, string.Join(",", values)));

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
        public static void SetSetting(string key, int value, string description = null) {
            key = key.ToLower();
            var pair = GetNode(key);
            if (pair == null) {
                pair = new SettingNode(key, value.ToString(CultureInfo.InvariantCulture), description);
                Values.Add(pair);
                if (OnSettingChanged != null)
                    OnSettingChanged(null, new SettingsChangedEventArgs(key, null, pair.Value));
                return;
            }
            if (OnSettingChanged != null)
                OnSettingChanged(null, new SettingsChangedEventArgs(key, pair.Value, value.ToString(CultureInfo.InvariantCulture)));

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
        public static void SetSetting(string key, bool value, string description = null) {
            key = key.ToLower();
            var pair = GetNode(key);
            if (pair == null) {
                pair = new SettingNode(key, value.ToString(CultureInfo.InvariantCulture), description);
                Values.Add(pair);
                if (OnSettingChanged != null)
                    OnSettingChanged(null, new SettingsChangedEventArgs(key, null, pair.Value));
                return;
            }
            if (OnSettingChanged != null)
                OnSettingChanged(null, new SettingsChangedEventArgs(key, pair.Value, value.ToString(CultureInfo.InvariantCulture)));

            pair.Description = description;
            pair.Value = string.Join(",", value.ToString(CultureInfo.InvariantCulture));
        }

        internal static SettingNode GetNode(string key) {
            key = key.ToLower();
            return Values.FirstOrDefault(pair => (pair.Key == null) ? false : pair.Key.ToLower() == key.ToLower());
        }

        /// <summary>
        /// Saves the settings
        /// </summary>
        public static void Save() {

            using (var writer = new StreamWriter(File.Create(FileUtils.PropertiesPath + "server.properties"))) {
                foreach (var v in Values) {

                    writer.WriteLine(v.Description == null && v.Key == null
                        ? v.Value
                        : v.Description == null
                            ? string.Format("{0}={1}" + (v != Values.Last() ? Environment.NewLine : ""), v.Key, v.Value)
                            : string.Format("#{0}" + Environment.NewLine + "{1}={2}" + (v != Values.Last() ? Environment.NewLine : ""), v.Description, v.Key, v.Value));

                }
            }
        }

        /// <summary>
        /// Loads all the settings into the memory, if no properties file is found nothing will happen
        /// </summary>
        public static void LoadSettings() {
            if (!File.Exists(FileUtils.PropertiesPath + "server.properties"))
                return;
            string[] text = File.ReadAllLines(FileUtils.PropertiesPath + "server.properties");
            Values.Clear();
            for (int i = 0; i < text.Count(); i++) {
                string read = text[i];
                SettingNode pair;

                if (String.IsNullOrWhiteSpace(read)) {
                    Values.Add(new SettingNode(null, read, null));
                    continue;
                }

                if (read[0] == '#' && ((i + 1 < text.Length) ? text[i + 1][0] == '#' || String.IsNullOrWhiteSpace(text[i + 1]) : true)) {
                    Values.Add(new SettingNode(null, read, null));
                    continue;
                }

                if (read[0] == '#' && (i + 1 < text.Length) ? text[i + 1][0] != '#' && !String.IsNullOrWhiteSpace(text[i + 1]) : false) {
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

        }


        internal static void GenerateSalt() {
            using (var numberGen = new RNGCryptoServiceProvider()) {
                var data = new byte[16];
                numberGen.GetBytes(data);
                Salt = Convert.ToBase64String(data);
            }
        }

        public static bool HasKey(string key) {
            return GetNode(key) != null;
        }
        public static string GetDescription(string key) {
            return GetNode(key).Description;
        }
    }

    /// <summary>
    /// Called When a setting node is changed
    /// </summary>
    public class SettingsChangedEventArgs : EventArgs {

        /// <summary>
        /// The key of the setting
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// The value before it was changed
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// The new value of the setting
        /// </summary>
        public string NewValue { get; set; }


        /// <summary>
        /// Create a new Settings Changed Event Class
        /// </summary>
        /// <param name="key">Name of key in lowercase</param>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">value to change</param>
        public SettingsChangedEventArgs(string key, string oldValue, string newValue) {
            Key = key.ToLower();
            OldValue = oldValue;
            NewValue = newValue;
        }

    }

    /// <summary>
    /// A simple class housing information of a setting key, value, and description
    /// </summary>
    public class SettingNode {

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public SettingNode(string key, string value, string description) {
            if (key != null)
                Key = key.ToLower();
            Value = value;
            Description = description;
        }
    }



}