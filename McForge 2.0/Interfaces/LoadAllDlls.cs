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

namespace MCForge
{
	static class LoadAllDlls
	{
		public static void Init()
		{
			Server.Log("[System]: Initializing Commands", ConsoleColor.Green, ConsoleColor.Black);
			InitCommands();
			
		}
		internal static void InitCommands()
		{
			string path = Directory.GetCurrentDirectory();
			string[] CommandFiles = Directory.GetFiles(path, "*.DLL");

			foreach (string s in CommandFiles)
			{
				FileInfo commandInfo = new FileInfo(s);

				Assembly commandAssembly = Assembly.LoadFrom(s);

				foreach (Type commandType in commandAssembly.GetTypes())
				{
					if (commandType.IsPublic)
					{
						if (!commandType.IsAbstract)
						{
							Type typeInterface = commandType.GetInterface("ICommand", true);

							if (typeInterface != null)
							{
								ICommand instance = (ICommand)Activator.CreateInstance(commandAssembly.GetType(commandType.ToString()));
								instance.Initialize();
								Server.Log("[Command]: " + instance.Name + " Initialized!", ConsoleColor.Magenta, ConsoleColor.Black);
							}			
						}
					}
				}
			}
		}
	}
}
