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
*/﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Interface.Plugin
{
    /// <summary>
    /// The command class, used to store commands for players to use
    /// </summary>
    public class PluginManager
    {
        internal static List<IPlugin> Plugins = new List<IPlugin>();

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
        /// Add an array of references to your command here
        /// </summary>
        /// <param name="plugin">The plugin d that this reference... references, you should most likely use 'this'</param>    
        public static void AddReference(IPlugin plugin)
        {
            if (plugin.GetType().GetInterface("IPlugin", false) == null)
                return;
            Plugins.Add(plugin);
        }
    }

}