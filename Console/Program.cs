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
using System.Threading;
using MCForge.Utilities.Settings;
using MCForge.Utilities;
using MCForge.Utils;
using MCForge.Interface;

namespace MCForge.Core {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Logger.Init();
            bool checker = CheckArgs(args);
            if (!checker)
            	ServerSettings.Init();
            else
            	Logger.Log("Aborting setup..", LogType.Critical);
            ColorUtils.Init();

            Console.Title = ServerSettings.GetSetting("ServerName") + " - MCForge 2.0"; //Don't know what MCForge version we are using yet.
            new Thread(new ThreadStart(Server.Init)).Start();
            Logger.OnRecieveLog += new EventHandler<LogEventArgs>(Server.OnLog);
            while (true) {
                string input = Console.ReadLine();
                if (input.ToLower() == "/stop") break;
                //You can use this to talk to the players, someone is probably going to improve this.
                /*else
                {
                    Player.UniversalChat(Colors.lime + "[Console] " + Colors.yellow + input);
                    Server.Log("[Console] " + input);
                }*/

            }

        }
        /// <summary>
        /// Check the args that were passed on program startup
        /// </summary>
        /// <param name="args">The args</param>
        /// <returns>If returns false, run normal setup. If returns true, cancel normal setup. In this case something has already started the server.</returns>
        static bool CheckArgs(string[] args)
        {
        	if (args.Length == 0)
        		return false;
        	string name = args[0];
        	switch (name)
        	{
        		case "load-plugin":
        			if (args.Length == 1)
        				return false;
        			string plugin = args[1];
        			string[] pargs = new string[] { "-force" };
        			for (int i = 1; i < args.Length; i++)
        				pargs[i] = args[i];
        			LoadAllDlls.LoadDLL(plugin, pargs);
        			break;
        		case "debug":
        			Server.DebugMode = true;
        			return false;
        		case "abort-setup":
        			return true;
        	}
        	return args[args.Length - 1] == "abort-setup";
        }
    }
}
