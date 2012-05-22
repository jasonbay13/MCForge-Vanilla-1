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
using MCForge.Interface.Command;
using MCForge.Utils.Settings;
using MCForge.Utils;
using System.IO;
using MCForge.Core;

namespace MCForge.Groups
{
    class CommandPermissionOverrides
    {
        /// <summary>
        /// The list of all the permissions in <Command, Byte> Form
        /// </summary>
        internal static Dictionary<ICommand, byte> overrides = new Dictionary<ICommand, byte>();
        static string PropertiesPath = FileUtils.PropertiesPath + "command.properties";

        /// <summary>
        /// Loads the data for the command permissions.
        /// </summary>
        /// <remarks></remarks>
        public static void Load()
        {
            if (!File.Exists(PropertiesPath)) SaveDefaults();
            string line;
            StreamReader properties = new StreamReader(PropertiesPath);
            while ((line = properties.ReadLine()) != null)
            {
                if (line[0] == '#') 
                {
                    continue;
                }
                string[] linesplit = line.Split(':');

                if (linesplit.Length < 2)
                {
                    Logger.Log("Line: \"" + line + "is not recognised");//, LogType.Error);
                }

                ICommand cmd = null;
                cmd = FindCommandByName(linesplit[0]);
                if (cmd == null)
                {
                    Logger.Log("Command \"" + linesplit[0] + "\" could not be found");//, LogType.Error);
                    continue;
                }

                byte perm;

                try { perm = byte.Parse(linesplit[1]); }
                catch { Logger.Log("Permission cannot be greater then 128 (is " + linesplit[1] + ")");//, LogType.Error);
                    continue; }

                if (!overrides.ContainsKey(cmd))
                    overrides.Add(cmd, perm);
            }
            properties.Close();
            properties.Dispose();
        }

        static ICommand FindCommandByName(string name)
        {
            foreach (ICommand cmd in Command.Commands.Values)
            {
                if (name.ToLower() == cmd.Name.ToLower())
                {
                    return cmd;
                }
            }
            return null;
        }

        /// <summary>
        /// Saves the defaults from all initialised commands.
        /// </summary>
        /// <remarks></remarks>
        public static void SaveDefaults()
        {
            List<ICommand> written = new List<ICommand>();
            StreamWriter properties = new StreamWriter(PropertiesPath);

            foreach (ICommand cmd in Command.Commands.Values)
            {
                if (!written.Contains(cmd))
                {
                    written.Add(cmd);
                    properties.WriteLine(cmd.Name + ':' + cmd.Permission);
                }
            }

            properties.Close();
            properties.Dispose();
        }

        /// <summary>
        /// Saves the permissions to file.
        /// </summary>
        /// <remarks></remarks>
        public static void Save()
        {
            List<ICommand> written = new List<ICommand>();
            StreamWriter properties = new StreamWriter(PropertiesPath);

            foreach (KeyValuePair<ICommand, byte> cmd in overrides)
            {
                if (!written.Contains(cmd.Key))
                {
                    written.Add(cmd.Key);
                    properties.WriteLine(cmd.Key.Name + ':' + cmd.Value);
                }
            }

            properties.Close();
            properties.Dispose();
        }

        /// <summary>
        /// Sets the permission of the command.
        /// </summary>
        /// <param name="cmd">The command to set the new position.</param>
        /// <param name="newpermission">The new permission value.</param>
        /// <remarks></remarks>
        public void SetPermission(ICommand cmd, byte newpermission)
        {
            if (overrides.ContainsKey(cmd))
                overrides.Remove(cmd);
            overrides.Add(cmd, newpermission);
        }

        public byte GetPermission(ICommand cmd)
        {
            foreach (KeyValuePair<ICommand, byte> kv in CommandPermissionOverrides.overrides)
                if (kv.Key == cmd)
                    return kv.Value;
            throw new ArgumentOutOfRangeException();
        }
    }
}
