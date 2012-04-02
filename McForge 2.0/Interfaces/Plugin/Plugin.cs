﻿using System;
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
            foreach (IPlugin ip in Plugins)
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
            if (plugin.GetType().GetInterface("ICommand", false) != null) //lolwut
            {
            }
            else
            {
                Plugins.Add(plugin);
            }
        }
    }

}