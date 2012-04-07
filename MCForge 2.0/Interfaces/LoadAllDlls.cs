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

namespace MCForge.Interface
{
	static class LoadAllDlls
	{
		public static void Init()
		{
			Server.Log("[System]: Initializing Commands", ConsoleColor.Green, ConsoleColor.Black);
			InitCommands();
			
		}
        internal static Assembly LoadFile(string file)
        {
            try
            {
                Assembly lib = null;
                using (FileStream fs = File.Open(file, FileMode.Open))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
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
                try
                {
                    return lib;
                }
                catch { return null; }
            }
            catch { return null; }
        }
		internal static void InitCommands()
		{
			string path = Directory.GetCurrentDirectory();
			string[] DLLFiles = Directory.GetFiles(path, "*.DLL");

			foreach (string s in DLLFiles)
			{
                Assembly DLLAssembly = LoadFile(s); //Prevents the dll from being in use inside windows
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
                                instance.Initialize();
                                Server.Log("[Command]: " + instance.Name + " Initialized!", ConsoleColor.Magenta, ConsoleColor.Black);
                            }
                            else
                            {
                                typeInterface = ClassType.GetInterface("IPlugin", true);
                                if (typeInterface != null)
                                {
                                    IPlugin instance = (IPlugin)Activator.CreateInstance(DLLAssembly.GetType(ClassType.ToString()));
                                    instance.OnLoad();
                                    PluginManager.AddReference(instance);
                                    Server.Log("[Plugin]: " + instance.Name + " Initialized!", ConsoleColor.Magenta, ConsoleColor.Black);
                                }
                            }
						}
					}
				}
			}
		}
	}
}
