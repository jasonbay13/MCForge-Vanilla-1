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
