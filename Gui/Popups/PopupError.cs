using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCForge.Gui.Popups {
    /// <summary>
    /// A popup that contains an error. Report returns a DialogResult.Retry. Ignore returns DialogResult.Ignore. Quit return a DialogResult.Cancel
    /// </summary>
    public partial class PopupError : Form {
        private string ex;
        public PopupError(Exception e) {
            ex = e.Message + "\n\r" + e.StackTrace;
            InitializeComponent();
        }

        public PopupError(string e) {
            ex = e;
            InitializeComponent();
        }

        private void PopupError_Load(object sender, EventArgs e) {
            txtError.Text = ex;
        }

        private void btnReport_Click(object sender, EventArgs e) {
            DialogResult = System.Windows.Forms.DialogResult.Retry;
            Close();
        }

        private void btnQuit_Click(object sender, EventArgs e) {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void btnIgnore_Click(object sender, EventArgs e) {
            DialogResult = System.Windows.Forms.DialogResult.Ignore;
            Close();
        }

        public static DialogResult Create(Exception e) {
            return new PopupError(e).ShowDialog();
        }

        public static DialogResult Create(string e) {
            return new PopupError(e).ShowDialog();
        }
    }
}
