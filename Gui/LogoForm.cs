using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;

namespace MCForge.Gui {
    public partial class LogoForm : Form {
        public LogoForm() {
            InitializeComponent();
        }
        private void LogoForm_Load(object sender, EventArgs e) {
            this.Opacity = .5;
        }

    }
}
