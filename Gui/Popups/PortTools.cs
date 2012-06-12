using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using MCForge.Utils.Settings;
using MCForge.Core;
using System.Net.Sockets;

namespace MCForge.Gui.Popups {
    public partial class PortTools : Form {

        private BackgroundWorker mWorker;

        public PortTools() {
            InitializeComponent();
            mWorker = new BackgroundWorker();
            mWorker.WorkerSupportsCancellation = true;
            mWorker.DoWork += new DoWorkEventHandler(mWorker_DoWork);
            mWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mWorker_RunWorkerCompleted);
        }

        private void linkManually_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://www.canyouseeme.org/");
        }

        private void linkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://www.mcforge.net/forums/forumdisplay.php?fid=19");
        }

        private void btnCheck_Click(object sender, EventArgs e) {
            int port = 25565;
            if (String.IsNullOrWhiteSpace(txtPort.Text))
                txtPort.Text = "25565";

            try {
                port = int.Parse(txtPort.Text);
            }
            catch {
                txtPort.Text = "25565";
            }

            btnCheck.Enabled = false;
            txtPort.Enabled = false;
            lblStatus.Text = "Checking...";
            mWorker.RunWorkerAsync(port);
        }

        void mWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Cancelled)
                return;

            btnCheck.Enabled = true;
            txtPort.Enabled = true;

            int result = (int)e.Result;
            switch (result) {
                case 0:
                    lblStatus.Text = "Problems Occurred";
                    lblStatus.ForeColor = Color.Red;
                    return;
                case 1:
                    lblStatus.Text = "Open";
                    lblStatus.ForeColor = Color.Green;
                    return;
                case 2:
                    lblStatus.Text = "Closed";
                    lblStatus.ForeColor = Color.Red;
                    return;
                case 3:
                    lblStatus.Text = "Web site error";
                    lblStatus.ForeColor = Color.Yellow;
                    return;
            }
        }

        void mWorker_DoWork(object sender, DoWorkEventArgs e) {
            try {
                using (var webClient = new WebClient()) {
                    string response = webClient.DownloadString("http://www.mcforge.net/ports.php?port=" + e.Argument);
                    switch (response.ToLower()) {
                        case "open":
                            e.Result = 1;
                            return;
                        case "closed":
                            e.Result = 2;
                            return;
                        default:
                            e.Result = 3;
                            return;
                    }
                }
            }
            catch {
                e.Result = 0;
                return;
            }
        }

        private void PortChecker_FormClosing(object sender, FormClosingEventArgs e) {
            mWorker.CancelAsync();
        }

        private void linkHelpForward_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://github.com/MCForge/MCForge-Vanilla/wiki/Setup%20MCForge%205.5.0.2");
        }

        private void btnForward_Click(object sender, EventArgs e) {
            int port = 25565;
            if (String.IsNullOrWhiteSpace(txtPortForward.Text))
                txtPortForward.Text = "25565";

            try {
                port = int.Parse(txtPortForward.Text);
            }
            catch {
                txtPortForward.Text = "25565";
            }

            try {
                if (!UPnP.Discover()) {
                    lblForward.Text = "Your router does not support UPnP, you must manually forward the port";
                    lblForward.ForeColor = Color.Red;
                }
                else {

                    UPnP.ForwardPort(port, ProtocolType.Tcp, "MCForgeServer");
                    lblForward.Text = "Port forwarded automatically using UPnP";
                    lblForward.ForeColor = Color.Green;

                }
            }
            catch {
                lblForward.Text = "Something Weird just happened, try again.";
                lblForward.ForeColor = Color.Black;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            int port = 25565;
            if (String.IsNullOrWhiteSpace(txtPortForward.Text))
                txtPortForward.Text = "25565";

            try {
                port = int.Parse(txtPortForward.Text);
            }
            catch {
                txtPortForward.Text = "25565";
            }

            try {
                if (!UPnP.Discover()) {
                    lblForward.Text = "Your router does not support UPnP";
                    lblForward.ForeColor = Color.Red;
                }
                else {

                    UPnP.DeleteForwardingRule(port, ProtocolType.Tcp);
                    lblForward.Text = "Port Deleted";
                    lblForward.ForeColor = Color.Green;

                }
            }
            catch {
                lblForward.Text = "Was that port forwarded?.";
                lblForward.ForeColor = Color.Black;
            }
        }
    }
}
