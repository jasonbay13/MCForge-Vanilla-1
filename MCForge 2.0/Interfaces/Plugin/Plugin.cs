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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Interface.Plugin {
    public interface IPlugin {
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
        /// When the plugin is first loaded
        /// </summary>
        void OnLoad(string[] args);

        /// <summary>
        /// When the plugin is unloaded.
        /// </summary>
       void OnUnload();

    }
}
