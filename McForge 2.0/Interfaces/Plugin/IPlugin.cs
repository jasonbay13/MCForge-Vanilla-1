﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Interface.Plugin
{
    public interface IPlugin
    {
        /// <summary>
        /// The name of the plugin
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The author of the plugin (to add multiple authors just make the string like "Merlin33069, someone else"
        /// </summary>
        string Author { get; }
        /// <summary>
        /// The command version
        /// </summary>
        int Version { get; }
        /// <summary>
        /// Unique identifier for this plugin, will be used later to link to McForge databases
        /// </summary>
        string CUD { get; }

        /// <summary>
        /// The initialization of the plugin.
        /// </summary>
        void Initialize();

    }
}
