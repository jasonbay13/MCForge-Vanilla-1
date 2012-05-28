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
using System.IO;
using System.Reflection;
using MCForge.Interface.Command;
using MCForge.Core;
using MCForge.Utils;
using MCForge.Utils.Settings;
using MCForge.API.Events;


namespace MCForge.Interface.Plugin {
    public class Plugin {
        private static List<IPlugin> Plugins = new List<IPlugin>();
        /// <summary>
        /// Get the names of all plugins
        /// </summary>
        /// <returns>The names of all plugins</returns>
        public static string[] GetNames()
        {
            List<string> ret = new List<string>();
            foreach (IPlugin ip in Plugins)
            {
                ret.Add(ip.Name);
            }
            return ret.ToArray();
        }
        /// <summary>
        /// Unloads a plugin
        /// </summary>
        /// <param name="name">The name of the plugin to unload</param>
        /// <param name="ignoreCase">Wheter or not to ignore the case. (default true)</param>
        /// <returns>Wheter or not the plugin is unloaded</returns>
        public static bool unload(string name, bool ignoreCase=true)
        {
            foreach (IPlugin ip in Plugins)
            {
                if ((ignoreCase && ip.Name.ToLower() == name.ToLower()) || ip.Name == name)
                {
                    if (!Plugin.OnPluginUnload.Call(ip, new PluginLoadEventArgs(false)).Canceled) {
                        try {
                            ip.OnUnload();
                        }
                        catch { Logger.Log(ip.Name + " cannot be unloaded", LogType.Warning); }
                        Plugins.Remove(ip);
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool isLoaded(string name, bool ignoreCase=true)
        {
            foreach (IPlugin ip in Plugins)
            {
                if ((ignoreCase && ip.Name.ToLower() == name.ToLower()) || ip.Name == name)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Reload one or all unloaded plugins.
        /// </summary>
        /// <param name="name">The name of the plugin to load, or an empty string to load all plugins</param>
        /// <param name="ignoreCase">Whether the case of the name gets ignored or not</param>
        /// <returns></returns>
        public static int reload(string name = "", bool ignoreCase = true) {
            foreach (IPlugin p in Plugins.ToArray())
            {
                if (name != "")
                {
                    if (ignoreCase)
                    {
                        if (p.Name.ToLower() == name.ToLower())
                        {
                            unload(name);
                            break;
                        }
                    }
                    else
                        if (p.Name == name)
                        {
                            unload(name);
                            break;
                        }
                }
                else
                {
                    unload(p.Name);
                    Logger.Log("Unloaded " + p.Name);
                }
            }

            int ret = 0;
            string path = Directory.GetCurrentDirectory();
            string[] DLLFiles = Directory.GetFiles(path, "*.dll");
            foreach (string s in DLLFiles)
            {
                ret++;
                if (name == "")
                    LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, true);
                else if (ignoreCase)
                {
                    if (s.ToLower() == name.ToLower())
                        LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, true);
                }
                else
                    if (s == name)
                        LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, true);
            }
            if (ServerSettings.HasKey("PluginsPath"))
            {
                string pluginspath = ServerSettings.GetSetting("PluginsPath");
                if (Directory.Exists(pluginspath))
                {
                    DLLFiles = Directory.GetFiles(pluginspath, "*.dll");
                    foreach (string s in DLLFiles)
                    {
                        if (name == "")
                            LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, true);
                        else if (ignoreCase)
                        {
                            if (s.ToLower() == name.ToLower())
                                LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, true);
                        }
                        else
                            if (s == name)
                                LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, true);
                        ret++;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Add an array of references to your command here
        /// </summary>
        /// <param name="plugin">The plugin d that this reference... references, you should most likely use 'this'</param>    
        public static void AddReference(IPlugin plugin)
        {
            if (plugin.GetType().GetInterface("ICommand", false) != null) //nothing prevents a plugin from being a command (except this line)
            {
            }
            else
            {
                Plugins.Add(plugin);
            }
        }
        /// <summary>
        /// Gets a plugin by interface.
        /// </summary>
        /// <param name="name">The name of the interface</param>
        /// <returns></returns>
        public static IPlugin getByInterface(string name)
        {
            foreach (var ip in Plugins)
            {
                if (ip.GetType().GetInterface(name) != null)
                    return ip;
            }
            return null;
        }
        /// <summary>
        /// Gets called when a plugin gets loaded.
        /// </summary>
        public static PluginLoadEvent OnPluginLoad = new PluginLoadEvent();
        /// <summary>
        /// Gets called when a plugin gets unloaded.
        /// </summary>
        public static PluginLoadEvent OnPluginUnload = new PluginLoadEvent();
    }
}
