using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCForge.Gui.Components {
    public partial class TransparentPanel : Panel {
        public TransparentPanel() {
            InitializeComponent();
        }

        public TransparentPanel(IContainer container) {
            container.Add(this);
            InitializeComponent();
        }


        protected override void OnPaintBackground(PaintEventArgs pevent) {
            // Don't paint background
        }

        protected override void OnPaint(PaintEventArgs e) {
            // Set the best settings possible (quality-wise)
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            var image = global::MCForge.Gui.Properties.Resources.mcforge_logo;
            e.Graphics.DrawImage(image, 0, 0, image.Width, image.Height);
        }     

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020;
                return cp;
            }
        }
    }
}
