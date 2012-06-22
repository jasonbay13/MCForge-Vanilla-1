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
using System.Windows.Forms;
using MCForge.Utils;
using MCForge.Core;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using MCForge.Gui.Properties;
using System.ComponentModel;
using System.Drawing.Imaging;
using MCForge.World.Drawing;

namespace MCForge.Gui {
    public partial class LogoForm : Form {

        private Bitmap imageOrigional = global::MCForge.Gui.Properties.Resources.mcforge_logo;
        private Font ubuntuFont;
        private SolidBrush WhiteBrush;

        public LogoForm() {
            InitializeComponent();
            ubuntuFont = new Font(Fonts.Ubuntu, 15, FontStyle.Bold);
            WhiteBrush = new SolidBrush(Color.Black);
        }


        public void Log(string text) {
            if (IsDisposed)
                return;

            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate { Log(text); });
                return;
            }
            if (imageOrigional == null)
                return;
            using (var image = new Bitmap(imageOrigional)) {
                using (var graphics = Graphics.FromImage(image)) {
                    var message = StringUtils.Truncate(text, 21);
                    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.DrawString(message, ubuntuFont, WhiteBrush, 50, 240);
                }
                SetBitmap(image);
            }

            Invalidate();

        }


        #region Transparency

        /// <para>Changes the current bitmap.</para>
        public void SetBitmap(Bitmap bitmap) {
            SetBitmap(bitmap, 255);
        }


        /// <para>Changes the current bitmap with a custom opacity level.  Here is where all happens!</para>
        public void SetBitmap(Bitmap bitmap, byte opacity) {

            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

            // The ideia of this is very simple,
            // 1. Create a compatible DC with screen;
            // 2. Select the bitmap with 32bpp with alpha-channel in the compatible DC;
            // 3. Call the UpdateLayeredWindow.

            IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
            IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  // grab a GDI handle from this GDI+ bitmap
                oldBitmap = Win32.SelectObject(memDc, hBitmap);

                Win32.Size size = new Win32.Size(bitmap.Width, bitmap.Height);
                Win32.Point pointSource = new Win32.Point(0, 0);
                Win32.Point topPos = new Win32.Point(Left, Top);
                Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
                blend.BlendOp = Win32.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = Win32.AC_SRC_ALPHA;

                Win32.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);
            }
            finally {
                Win32.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero) {
                    Win32.SelectObject(memDc, oldBitmap);
                    //Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.
                    Win32.DeleteObject(hBitmap);
                }
                Win32.DeleteDC(memDc);
            }
        }


        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00080000; // This form has to have the WS_EX_LAYERED extended style
                return cp;
            }
        }

        #endregion

        internal void Shutdown() {
            if (InvokeRequired) {
                BeginInvoke((MethodInvoker)Shutdown);
                return;
            }

            Close();
            Dispose();
        }
    }
}
