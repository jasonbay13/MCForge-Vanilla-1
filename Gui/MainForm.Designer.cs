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
namespace MCForge.Gui {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.kickAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unloadEmptyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.killPhysicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.pluginsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.kickEverybodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopPhysicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unloadEmptyLevelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbLevels = new System.Windows.Forms.TabPage();
            this.tbPlayers = new System.Windows.Forms.TabPage();
            this.grpInfo = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstPlayersBig = new System.Windows.Forms.ListBox();
            this.tbMain = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbChatType = new System.Windows.Forms.ComboBox();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.txtLog = new MCForge.Gui.Components.ColoredLogReader(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstLevels = new System.Windows.Forms.ListBox();
            this.mPlayerGroupBox = new System.Windows.Forms.GroupBox();
            this.lstPlayers = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.nfMain = new MCForge.Gui.Components.NewsFeeder(this.components);
            this.menuStrip1.SuspendLayout();
            this.tbPlayers.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tbMain.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.mPlayerGroupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.pluginsToolStripMenuItem.Text = "Plugins";
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.serverToolStripMenuItem.Text = "Server";
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // kickAllToolStripMenuItem
            // 
            this.kickAllToolStripMenuItem.Name = "kickAllToolStripMenuItem";
            this.kickAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.kickAllToolStripMenuItem.Text = "Kick All";
            // 
            // unloadEmptyToolStripMenuItem
            // 
            this.unloadEmptyToolStripMenuItem.Name = "unloadEmptyToolStripMenuItem";
            this.unloadEmptyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.unloadEmptyToolStripMenuItem.Text = "Unload Empty";
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            // 
            // killPhysicsToolStripMenuItem
            // 
            this.killPhysicsToolStripMenuItem.Name = "killPhysicsToolStripMenuItem";
            this.killPhysicsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.killPhysicsToolStripMenuItem.Text = "Kill Physics";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pluginsMenuItem,
            this.serverMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(964, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // pluginsMenuItem
            // 
            this.pluginsMenuItem.Name = "pluginsMenuItem";
            this.pluginsMenuItem.Size = new System.Drawing.Size(58, 20);
            this.pluginsMenuItem.Text = "Plugins";
            // 
            // serverMenuItem
            // 
            this.serverMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shutdownMenuItem,
            this.restartMenuItem,
            this.propertiesMenuItem,
            this.toolStripSeparator2,
            this.kickEverybodyToolStripMenuItem,
            this.stopPhysicsToolStripMenuItem,
            this.unloadEmptyLevelsToolStripMenuItem,
            this.saveAllToolStripMenuItem1});
            this.serverMenuItem.Name = "serverMenuItem";
            this.serverMenuItem.Size = new System.Drawing.Size(51, 20);
            this.serverMenuItem.Text = "Server";
            // 
            // shutdownMenuItem
            // 
            this.shutdownMenuItem.Name = "shutdownMenuItem";
            this.shutdownMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.shutdownMenuItem.Size = new System.Drawing.Size(184, 22);
            this.shutdownMenuItem.Text = "Shutdown";
            // 
            // restartMenuItem
            // 
            this.restartMenuItem.Name = "restartMenuItem";
            this.restartMenuItem.Size = new System.Drawing.Size(184, 22);
            this.restartMenuItem.Text = "Restart";
            // 
            // propertiesMenuItem
            // 
            this.propertiesMenuItem.Name = "propertiesMenuItem";
            this.propertiesMenuItem.Size = new System.Drawing.Size(184, 22);
            this.propertiesMenuItem.Text = "Properties";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // kickEverybodyToolStripMenuItem
            // 
            this.kickEverybodyToolStripMenuItem.Name = "kickEverybodyToolStripMenuItem";
            this.kickEverybodyToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.kickEverybodyToolStripMenuItem.Text = "Kick Everybody";
            // 
            // stopPhysicsToolStripMenuItem
            // 
            this.stopPhysicsToolStripMenuItem.Name = "stopPhysicsToolStripMenuItem";
            this.stopPhysicsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.stopPhysicsToolStripMenuItem.Text = "Stop Physics";
            // 
            // unloadEmptyLevelsToolStripMenuItem
            // 
            this.unloadEmptyLevelsToolStripMenuItem.Name = "unloadEmptyLevelsToolStripMenuItem";
            this.unloadEmptyLevelsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.unloadEmptyLevelsToolStripMenuItem.Text = "Unload Empty Levels";
            // 
            // saveAllToolStripMenuItem1
            // 
            this.saveAllToolStripMenuItem1.Name = "saveAllToolStripMenuItem1";
            this.saveAllToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.saveAllToolStripMenuItem1.Text = "Save All";
            // 
            // shutdownToolStripMenuItem
            // 
            this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
            this.shutdownToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.shutdownToolStripMenuItem.Text = "Shutdown";
            // 
            // tbLevels
            // 
            this.tbLevels.Location = new System.Drawing.Point(4, 24);
            this.tbLevels.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbLevels.Name = "tbLevels";
            this.tbLevels.Size = new System.Drawing.Size(956, 486);
            this.tbLevels.TabIndex = 3;
            this.tbLevels.Text = "Levels";
            this.tbLevels.UseVisualStyleBackColor = true;
            // 
            // tbPlayers
            // 
            this.tbPlayers.Controls.Add(this.grpInfo);
            this.tbPlayers.Controls.Add(this.groupBox3);
            this.tbPlayers.Location = new System.Drawing.Point(4, 24);
            this.tbPlayers.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbPlayers.Name = "tbPlayers";
            this.tbPlayers.Size = new System.Drawing.Size(956, 486);
            this.tbPlayers.TabIndex = 2;
            this.tbPlayers.Text = "Players";
            this.tbPlayers.UseVisualStyleBackColor = true;
            // 
            // grpInfo
            // 
            this.grpInfo.Location = new System.Drawing.Point(546, 3);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(402, 480);
            this.grpInfo.TabIndex = 1;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "$Name info";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstPlayersBig);
            this.groupBox3.Font = new System.Drawing.Font("Ubuntu", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(9, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(531, 481);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Player List";
            // 
            // lstPlayersBig
            // 
            this.lstPlayersBig.Font = new System.Drawing.Font("Ubuntu", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPlayersBig.FormattingEnabled = true;
            this.lstPlayersBig.ItemHeight = 16;
            this.lstPlayersBig.Location = new System.Drawing.Point(7, 23);
            this.lstPlayersBig.Name = "lstPlayersBig";
            this.lstPlayersBig.Size = new System.Drawing.Size(516, 452);
            this.lstPlayersBig.TabIndex = 0;
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.groupBox2);
            this.tbMain.Controls.Add(this.groupBox1);
            this.tbMain.Controls.Add(this.mPlayerGroupBox);
            this.tbMain.Location = new System.Drawing.Point(4, 24);
            this.tbMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbMain.Name = "tbMain";
            this.tbMain.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbMain.Size = new System.Drawing.Size(956, 486);
            this.tbMain.TabIndex = 0;
            this.tbMain.Text = "Main";
            this.tbMain.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbChatType);
            this.groupBox2.Controls.Add(this.txtChat);
            this.groupBox2.Controls.Add(this.txtLog);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(7, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(707, 476);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chat";
            // 
            // cmbChatType
            // 
            this.cmbChatType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChatType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbChatType.FormattingEnabled = true;
            this.cmbChatType.Items.AddRange(new object[] {
            "Chat",
            "OpChat",
            "AdminChat"});
            this.cmbChatType.Location = new System.Drawing.Point(588, 445);
            this.cmbChatType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbChatType.Name = "cmbChatType";
            this.cmbChatType.Size = new System.Drawing.Size(111, 21);
            this.cmbChatType.TabIndex = 14;
            // 
            // txtChat
            // 
            this.txtChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChat.ForeColor = System.Drawing.Color.Gray;
            this.txtChat.Location = new System.Drawing.Point(7, 446);
            this.txtChat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size(573, 20);
            this.txtChat.TabIndex = 13;
            this.txtChat.Text = "Enter a message or a command";
            this.txtChat.Enter += new System.EventHandler(this.txtChat_Enter);
            this.txtChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Chat);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(7, 21);
            this.txtLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(693, 419);
            this.txtLog.TabIndex = 12;
            this.txtLog.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstLevels);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(721, 247);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(224, 232);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Levels";
            // 
            // lstLevels
            // 
            this.lstLevels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLevels.FormattingEnabled = true;
            this.lstLevels.Location = new System.Drawing.Point(7, 22);
            this.lstLevels.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstLevels.Name = "lstLevels";
            this.lstLevels.Size = new System.Drawing.Size(209, 199);
            this.lstLevels.TabIndex = 0;
            // 
            // mPlayerGroupBox
            // 
            this.mPlayerGroupBox.Controls.Add(this.lstPlayers);
            this.mPlayerGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mPlayerGroupBox.Location = new System.Drawing.Point(721, 4);
            this.mPlayerGroupBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mPlayerGroupBox.Name = "mPlayerGroupBox";
            this.mPlayerGroupBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mPlayerGroupBox.Size = new System.Drawing.Size(224, 242);
            this.mPlayerGroupBox.TabIndex = 9;
            this.mPlayerGroupBox.TabStop = false;
            this.mPlayerGroupBox.Text = "Players";
            // 
            // lstPlayers
            // 
            this.lstPlayers.BackColor = System.Drawing.Color.White;
            this.lstPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPlayers.FormattingEnabled = true;
            this.lstPlayers.Location = new System.Drawing.Point(8, 21);
            this.lstPlayers.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstPlayers.Name = "lstPlayers";
            this.lstPlayers.Size = new System.Drawing.Size(208, 212);
            this.lstPlayers.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbMain);
            this.tabControl1.Controls.Add(this.tbPlayers);
            this.tabControl1.Controls.Add(this.tbLevels);
            this.tabControl1.Location = new System.Drawing.Point(0, 31);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(964, 514);
            this.tabControl1.TabIndex = 8;
            // 
            // nfMain
            // 
            this.nfMain.BackColor = System.Drawing.Color.White;
            this.nfMain.Items.Add("1234");
            this.nfMain.Items.Add("1234");
            this.nfMain.Items.Add("12334");
            this.nfMain.Items.Add("1234");
            this.nfMain.Items.Add("12");
            this.nfMain.Items.Add("34125123651346136");
            this.nfMain.Items.Add("dsfdcv3e1r");
            this.nfMain.Items.Add("123 r13424");
            this.nfMain.Items.Add("c1c34");
            this.nfMain.Location = new System.Drawing.Point(0, 551);
            this.nfMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nfMain.Name = "nfMain";
            this.nfMain.ReadOnly = true;
            this.nfMain.Size = new System.Drawing.Size(964, 24);
            this.nfMain.TabIndex = 7;
            this.nfMain.Text = "Connecting...";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 574);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.nfMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MCForge Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tbPlayers.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tbMain.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.mPlayerGroupBox.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.NewsFeeder nfMain;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem kickAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unloadEmptyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem killPhysicsToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pluginsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem kickEverybodyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopPhysicsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unloadEmptyLevelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem1;
        private System.Windows.Forms.TabPage tbLevels;
        private System.Windows.Forms.TabPage tbPlayers;
        private System.Windows.Forms.TabPage tbMain;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbChatType;
        private System.Windows.Forms.TextBox txtChat;
        private Components.ColoredLogReader txtLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstLevels;
        private System.Windows.Forms.GroupBox mPlayerGroupBox;
        private System.Windows.Forms.ListBox lstPlayers;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstPlayersBig;
        private System.Windows.Forms.GroupBox grpInfo;
    }
}
