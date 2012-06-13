/*
Copyright 2011 MCForge
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
using MCForge.Utils;
using System.Threading;
using MCForge.Core;
using MCForge.Interface;
using MCForge.Entity;
using MCForge.API.Events;
using MCForge.Gui.API;
using MCForge.World;

namespace MCForge.Gui {
    internal partial class MainForm : Form {

        private MCForgeGuiManager pluginManager;
        private LogoForm splashScreen;

        public MainForm() {

            InitializeComponent();
            this.Visible = false;
            pluginManager = new MCForgeGuiManager(pluginsToolStripMenuItem);

            //Splash Form
            new Thread(new ThreadStart(() => {
                splashScreen = new LogoForm();
                splashScreen.ShowDialog();
            })).Start();


            //Event Handlers
            Logger.OnRecieveLog += OnLog;
            Logger.OnRecieveErrorLog += OnErrorLog;
            Server.OnServerFinishSetup += OnCompletedStartUp;

            try {
                new Thread(new ThreadStart(Server.Init)).Start();
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }

        private void frmMain_Load(object sender, System.EventArgs e) {

            //Custom Component Init
            cmbChatType.Text = "Chat";
            nfMain.StartRead();
            pluginManager.Init();
            pluginManager.AttachItems();

            //Gui Handlers
            Player.OnAllPlayersConnect.Important += OnConnect;
            Player.OnAllPlayersDisconnect.Important += OnDisconnect;
            //TODO: Level.OnLevelLoad += OnLevelLoad; 
            //TODO: Level.OnLevelUnload += OnLevelUnload;
        }

        #region Gui Handles

        void Chat(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                if (String.IsNullOrWhiteSpace(txtChat.Text)) {
                    Logger.Log("Please specify a valid message!", Color.Red, Color.White, LogType.Warning);
                    return;
                }

                if (cmbChatType.Text == "OpChat") {
                    Player.UniversalChatOps("&a<&fTo Ops&a> %a[%fConsole%a]:%f " + txtChat.Text);
                    Logger.Log("<OpChat> <Console> " + txtChat.Text);
                    txtChat.Clear();
                    return;
                }

                if (cmbChatType.Text == "AdminChat") {
                    Player.UniversalChatAdmins("&a<&fTo Admins&a> %a[%fConsole%a]:%f " + txtChat.Text);
                    Logger.Log("<AdminChat> <Console> " + txtChat.Text);
                    txtChat.Clear();
                    return;
                }

                Player.UniversalChat("&a[&fConsole&a]:&f " + txtChat.Text);
                Logger.Log("&5[&1Console&5]: &1" + txtChat.Text);
                txtChat.Clear(); return;
            }

        }

        void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            switch (MessageBox.Show("Would you like to save all?", "Save?", MessageBoxButtons.YesNoCancel)) {
                case DialogResult.Yes:
                    Server.SaveAll();
                    Server.Stop();
                    System.Diagnostics.Process pr = System.Diagnostics.Process.GetCurrentProcess();
                    pr.Kill();
                    break;
                case DialogResult.No:
                    Server.Stop();
                    System.Diagnostics.Process pro = System.Diagnostics.Process.GetCurrentProcess();
                    pro.Kill();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }

        private void txtChat_Enter(object sender, System.EventArgs e) {
            txtChat.ForeColor = Color.Black;
            txtChat.Clear();
        }

        private void shutdownMenuItem_Click(object sender, System.EventArgs e) {
            Close();
        }

        #endregion

        //Every Event handler from the library needs to be invoked before it inteacts with the UI
        #region EventHandlers




        void OnConnect(Player sender, ConnectionEventArgs args) {
#if !DEBUG
            try {
#endif

            if (lstPlayers.InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate { OnConnect(sender, args); });
                return;
            }

            lstPlayers.Items.Add(sender.Username);

#if !DEBUG   
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (NotImplementedException) { }
#endif

        }

        void OnDisconnect(Player sender, ConnectionEventArgs args) {

#if !DEBUG
            try {
#endif

            if (lstPlayers.InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate { OnDisconnect(sender, args); });
                return;
            }

            lstPlayers.Items.Remove(sender.Username);

#if !DEBUG   
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (NotImplementedException) { }
#endif

        }

        void OnLog(object sender, LogEventArgs args) {
#if !DEBUG
            try {
#endif
            if (InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate { OnLog(sender, args); });
                return;
            }

            if (splashScreen != null && !splashScreen.IsDisposed) {
                splashScreen.Log(args.Message);
                return;
            }

            if (args.LogType == LogType.Debug)
                return;

            txtLog.AppendLog(args.Message + Environment.NewLine, args.LogType == LogType.Normal ? Color.Black : args.TextColor);
            txtLog.ScrollToEnd();
#if !DEBUG   
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (NotImplementedException) { }
#endif

        }

        void OnErrorLog(object sender, LogEventArgs args) {
#if !DEBUG
            try {
#endif
            if (splashScreen != null && !splashScreen.IsDisposed) {
                switch (Popups.PopupError.Create(args.Message)) {
                    case DialogResult.Ignore:
                        break;
                    case System.Windows.Forms.DialogResult.Retry:
                        //TODO: report bug
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                        //TODO: dispose of stuff
                        return;
                }
            }

            txtLog.AppendLog("%c" + args.Message + Environment.NewLine, Color.Black);

#if !DEBUG   
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (NotImplementedException) { }
#endif

        }

        void OnCompletedStartUp() {
#if !DEBUG
            try {
#endif

            if (InvokeRequired) {
                BeginInvoke((MethodInvoker)OnCompletedStartUp);
                return;
            }

            splashScreen.Dispose();
            splashScreen = null;

            this.ShowDialog();
            this.BringToFront();
            Server.OnServerFinishSetup -= OnCompletedStartUp;

#if !DEBUG   
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (NotImplementedException) { }
#endif
        }



        #endregion






    }
}
