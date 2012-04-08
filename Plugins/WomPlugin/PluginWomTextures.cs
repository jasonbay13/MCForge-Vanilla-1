/*
Copyright 2012 MCForge
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
using MCForge;
using System.Timers;
using MCForge.Interface.Plugin;
using MCForge.Entity;
using MCForge.Core;
using MCForge.Utilities.Settings;
using System.IO;

namespace Plugins.WomPlugin {
    public class PluginWomTextures : IPlugin {

        public string Name {
            get { return "WomTextures"; }
        }

        public string Author {
            get { return "headdetect"; }
        }

       public Version Version {
            get { return new Version(1, 0); }
        }

        public string CUD {
            get { return "com.headdetect.womtextures"; }
        }

        private WomSettings WomSettings { get; set; }
        public void OnLoad() {
            WomSettings = new WomSettings();
            WomSettings.OnLoad();
            //WomSettings.L
        }

        public void OnUnload() {
            throw new NotImplementedException();
        }
    }
}