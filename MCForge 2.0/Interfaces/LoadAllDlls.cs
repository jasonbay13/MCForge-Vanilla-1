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
using MCForge.Interface.Plugin;
using MCForge.Interface.Command;
using MCForge.Core;
using MCForge.API.Events;
using MCForge.Utilities;
using MCForge.Utilities.Settings;

namespace MCForge.Interface {
    public static class LoadAllDlls {
        public static void Init() {
            Logger.Log("[System]: Initializing Commands");
            InitCommandsAndPlugins();

        }
        public static Assembly LoadFile(string file) {
            try {
                Assembly lib = null;
                using (FileStream fs = File.Open(file, FileMode.Open)) {
                    using (MemoryStream ms = new MemoryStream()) {
                        byte[] buffer = new byte[1024];
                        int read = 0;
                        while ((read = fs.Read(buffer, 0, 1024)) > 0)
                            ms.Write(buffer, 0, read);
                        lib = Assembly.Load(ms.ToArray());
                        ms.Close();
                        ms.Dispose();
                    }
                    fs.Close();
                    fs.Dispose();
                }
                return lib;
            }
            catch { return null; }
        }
		/// <summary>
		/// Load a DLL
		/// </summary>
		/// <param name="s">The filepath of the DLL</param>
        /// <param name="args">The args to passed to the plugin OnLoad method.</param>
		public static void LoadDLL(string s, string[] args)
		{
			 Assembly DLLAssembly = LoadFile(s); //Prevents the dll from being in use inside windows
                try
                {
                    foreach (Type ClassType in DLLAssembly.GetTypes())
                    {
                        if (ClassType.IsPublic)
                        {
                            if (!ClassType.IsAbstract)
                            {
                                Type typeInterface = ClassType.GetInterface("ICommand", true);

                                if (typeInterface != null)
                                {
                                    ICommand instance = (ICommand)Activator.CreateInstance(DLLAssembly.GetType(ClassType.ToString()));
                                    if (!Command.Command.OnCommandLoad.Call(instance, new CommandLoadEventArgs(true)).Canceled) {
                                        instance.Initialize();
                                        Logger.Log("[Command]: " + instance.Name + " Initialized!");
                                    }
                                }
                                else
                                {
                                    typeInterface = ClassType.GetInterface("IPlugin", true);
                                    if (typeInterface != null)
                                    {
                                        IPlugin instance = (IPlugin)Activator.CreateInstance(DLLAssembly.GetType(ClassType.ToString()));
                                        if (!Plugin.Plugin.OnPluginLoad.Call(instance, new PluginLoadEventArgs(true)).Canceled) {
                                            instance.OnLoad(args);
                                            Plugin.Plugin.AddReference(instance);
                                            Logger.Log("[Plugin]: " + instance.Name + " Initialized!");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch { } //Stops loading bad DLL files
		}
        internal static void InitCommandsAndPlugins() {
            string path = Directory.GetCurrentDirectory();
            string[] DLLFiles = Directory.GetFiles(path, "*.dll");
            foreach (string s in DLLFiles)
            	LoadDLL(s, new string[] { "-normal" });
            if (ServerSettings.HasKey("PluginsPath")) {
                string pluginspath = ServerSettings.GetSetting("PluginsPath");
                if (Directory.Exists(pluginspath)) {
                    DLLFiles = Directory.GetFiles(pluginspath, "*.dll");
                    foreach (string s in DLLFiles)
                        LoadDLL(s, new string[] { "-normal" });
                }
            }
            if (ServerSettings.HasKey("CommandsPath")) {
                string commandspath = ServerSettings.GetSetting("CommandsPath");
                if (Directory.Exists(commandspath)) {
                    DLLFiles = Directory.GetFiles(commandspath, "*.dll");
                    foreach (string s in DLLFiles)
                        LoadDLL(s, new string[] { "-normal" });
                }
            }
        }
        
    }
}
