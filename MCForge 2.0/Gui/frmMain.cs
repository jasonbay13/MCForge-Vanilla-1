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

namespace MCForge
{
	public partial class frmMain : Form
	{
        public static ListBox players = new ListBox();
		public frmMain()
		{
			InitializeComponent();
		}

        private void frmMain_Load(object sender, EventArgs e) {
            new Thread(new ThreadStart(Server.Init)).Start();
            Logger.OnRecieveLog += (obj, args) => {
                coloredTextBox1.LogText(args.Message + Environment.NewLine);
            };
            players = mPlayersListBox;
            chatButtonChange.Text = "Chat";
        }
        private void Chat(object sender, KeyEventArgs e) 
        {
            if (e.KeyCode == Keys.Enter) {
                if (String.IsNullOrWhiteSpace(chatBox.Text)) { Logger.Log("Please specify a valid message!" + Environment.NewLine); return; }
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

                Player.UniversalChat("%a[%fConsole%a]:%f " + chatBox.Text); 
                Logger.Log("<Console> " + chatBox.Text); 
                chatBox.Clear(); return;
            }
        }
	}
}
