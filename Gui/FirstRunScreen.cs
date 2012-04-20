using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MCForge.Core;
using System.Diagnostics;
//Made by Sinjai

namespace MCForge.Gui
{
    public partial class FirstRunScreen : Form
    {
        public FirstRunScreen()
        {
            InitializeComponent();
        }
        private void DoNotDisplayAgain_CheckedChanged(object sender, EventArgs e)
        {
            Server.ShowFirstRunScreen = !Server.ShowFirstRunScreen;
        }
        private void MCForge_Logo_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://mcforge.net/");
        }
    }
}
