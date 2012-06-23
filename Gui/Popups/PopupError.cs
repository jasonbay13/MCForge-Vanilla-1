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
