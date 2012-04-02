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
        /// Add an array of referances to your command here
        /// </summary>
        /// <param name="command">the command that this referance... referances, you should most likely use 'this'</param>
        /// <param name="reference">the array of strings you want players to type to use your command</param>
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