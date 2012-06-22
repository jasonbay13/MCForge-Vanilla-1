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
using System.Drawing;
using System.Drawing.Text;
using MCForge.Properties;

namespace MCForge.World.Drawing {
    public class Fonts {

        private static PrivateFontCollection fonts;

        unsafe static Fonts() {
            fonts = new PrivateFontCollection();

            fixed (byte* data = Resources.Ubuntu)
                fonts.AddMemoryFont((IntPtr)data, Resources.Ubuntu.Length);

            fixed (byte* data = Resources.minecraft)
                fonts.AddMemoryFont((IntPtr)data, Resources.minecraft.Length);

        }

        /// <summary>
        /// Gets the ubuntu font family.
        /// </summary>
        public static FontFamily Ubuntu {
            get {
                return fonts.Families[1];
            }
        }

        /// <summary>
        /// Gets the minecraft font family.
        /// </summary>
        public static FontFamily Minecraft {
            get {
                return fonts.Families[0];
            }
        }

    }
}
