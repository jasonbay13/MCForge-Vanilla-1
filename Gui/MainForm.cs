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
using MCForge.Utilities;
using System.Threading;
using MCForge.Core;
using MCForge.Interface;
using MCForge.Utilities;
using MCForge.Utils;
using MCForge.Entity;
using MCForge.API.PlayerEvent;
using MCForge.Gui.API;

namespace MCForge.Gui {
    internal partial class MainForm : Form {
        private MCForgeGuiManager pluginManager;
        public MainForm() {
            InitializeComponent();
            pluginManager = new MCForgeGuiManager(pluginsToolStripMenuItem);
        }

        private void frmMain_Load(object sender, EventArgs e) {
            new Thread(new ThreadStart(Server.Init)).Start();
            pluginManager.Init();
            pluginManager.AttachItems();
            Logger.OnRecieveLog += (obj, args) => {
                coloredTextBox1.LogText(args.Message + Environment.NewLine);
            };

            OnPlayerConnect.Register(OnConnect);

            chatButtonChange.Text = "Chat";

            newsFeeder1.StartRead();
            newsFeeder1.DisplayPosts();
        }
        private void Chat(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                if (String.IsNullOrWhiteSpace(chatBox.Text)) {
                    Logger.Log("Please specify a valid message!" + Environment.NewLine);
                    return;
                }

                if (chatButtonChange.Text == "OpChat") {
                    Player.UniversalChatOps("&a<&fTo Ops&a> %a[%fConsole%a]:%f " + chatBox.Text);
                    Logger.Log("<OpChat> <Console> " + chatBox.Text);
                    chatBox.Clear();
                    return;
                }

                if (chatButtonChange.Text == "AdminChat") {
                    Player.UniversalChatAdmins("&a<&fTo Admins&a> %a[%fConsole%a]:%f " + chatBox.Text);
                    Logger.Log("<AdminChat> <Console> " + chatBox.Text);
                    chatBox.Clear();
                    return;
                }

                Player.UniversalChat("&a[&fConsole&a]:&f " + chatBox.Text);
                Logger.Log("&a[&fConsole&a]:&f " + chatBox.Text);
                chatBox.Clear(); return;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            switch (MessageBox.Show("Would you like to save all?", "Save?", MessageBoxButtons.YesNoCancel)) {
                case DialogResult.Yes:
                    Server.SaveAll();
                    Server.Stop();
                    break;
                case DialogResult.No:
                    Server.Stop();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }

        #region EventHandlers

        void OnConnect(OnPlayerConnect args) {
            if (mPlayersListBox.InvokeRequired) {
                mPlayersListBox.Invoke((MethodInvoker)delegate { OnConnect(args); });
                return;
            }
            mPlayersListBox.Items.Add(args.Player.username);
        }

        void OnDisconnect(OnPlayerDisconnect args) {
            if (mPlayersListBox.InvokeRequired) {
                mPlayersListBox.Invoke((MethodInvoker)delegate { OnDisconnect(args);  });
                return;
            }
            mPlayersListBox.Items.Remove(args.Player.username);
        }
        #endregion

    }
}
