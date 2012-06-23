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
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MCForge.Interface.Plugin;
using MCForge.Utils;
using System.IO;
using MCForge.Interface;
using System.Reflection;

namespace MCForge.Gui.API {
    public class MCForgeGuiManager {

        private List<MCForgeGui> _guiElements;
        private ToolStripMenuItem _menu;

        internal MCForgeGuiManager(ToolStripMenuItem parent) {
            _menu = parent;
            _menu.DropDownItemClicked += new ToolStripItemClickedEventHandler(OnMenuClicked);
            _guiElements = new List<MCForgeGui>();
        }

        internal void Init() {
            if (!Directory.Exists(FileUtils.DllsPath + "/gui"))
                Directory.CreateDirectory(FileUtils.DllsPath + "/gui");

            foreach (var info in Directory.GetFiles(FileUtils.DllsPath + "/gui", "*.dll")) {
                var _assem = LoadAllDlls.LoadFile(info);
                foreach (var type in _assem.GetTypes()) {
                    if (type.IsAbstract || !type.IsPublic)
                        continue;

                    var guiType = type.GetInterface("MCForgeGui", true);

                    if (guiType == null)
                        continue;

                    MCForgeGui mGui = (MCForgeGui)Activator.CreateInstance(_assem.GetType(type.ToString()));
                    _guiElements.Add(mGui);

                }
            }
        }


        internal void AttachItems() {
            if (_menu == null)
                throw new NullReferenceException("Menu Strip is null");

            foreach (var plugin in _guiElements) {
                if (plugin.MenuImage != null)
                    _menu.DropDownItems.Add(plugin.MenuTitle, plugin.MenuImage);
                else
                    _menu.DropDownItems.Add(plugin.MenuTitle);

            }
        }

        void OnMenuClicked(object sender, ToolStripItemClickedEventArgs args) {
            foreach (var i in _guiElements) {
                if (i.MenuTitle == args.ClickedItem.Text)
                    i.Form.Show();
            }
        }
    }
}
