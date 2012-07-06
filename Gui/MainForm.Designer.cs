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
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbLevels = new System.Windows.Forms.TabPage();
            this.tbPlayers = new System.Windows.Forms.TabPage();
            this.grpInfo = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstPlayersBig = new System.Windows.Forms.ListBox();
            this.tbMain = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstLevels = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtLog = new MCForge.Gui.Components.ColoredLogReader(this.components);
            this.cmbChatType = new System.Windows.Forms.ComboBox();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.mPlayerGroupBox = new System.Windows.Forms.GroupBox();
            this.lstPlayers = new System.Windows.Forms.ListBox();
            this.ctxPlayer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kickMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.banMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.promoteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demoteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setrankMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setRankComboBoxItem = new System.Windows.Forms.ToolStripComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.itmShutDown = new System.Windows.Forms.MenuItem();
            this.itmRestart = new System.Windows.Forms.MenuItem();
            this.itmProperties = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.itmKickAll = new System.Windows.Forms.MenuItem();
            this.itmStopPhysics = new System.Windows.Forms.MenuItem();
            this.itmUnload = new System.Windows.Forms.MenuItem();
            this.itmSaveAll = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.portMenuItem = new System.Windows.Forms.MenuItem();
            this.tbPlayers.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tbMain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.mPlayerGroupBox.SuspendLayout();
            this.ctxPlayer.SuspendLayout();
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
            this.tbLevels.Size = new System.Drawing.Size(839, 468);
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
            this.tbPlayers.Size = new System.Drawing.Size(839, 468);
            this.tbPlayers.TabIndex = 2;
            this.tbPlayers.Text = "Players";
            this.tbPlayers.UseVisualStyleBackColor = true;
            // 
            // grpInfo
            // 
            this.grpInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInfo.Location = new System.Drawing.Point(359, 0);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(480, 442);
            this.grpInfo.TabIndex = 1;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "$Name info";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.lstPlayersBig);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(9, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(347, 439);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Player List";
            // 
            // lstPlayersBig
            // 
            this.lstPlayersBig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPlayersBig.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPlayersBig.FormattingEnabled = true;
            this.lstPlayersBig.Location = new System.Drawing.Point(3, 16);
            this.lstPlayersBig.Name = "lstPlayersBig";
            this.lstPlayersBig.Size = new System.Drawing.Size(341, 420);
            this.lstPlayersBig.TabIndex = 0;
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.groupBox1);
            this.tbMain.Controls.Add(this.groupBox2);
            this.tbMain.Controls.Add(this.mPlayerGroupBox);
            this.tbMain.Location = new System.Drawing.Point(4, 24);
            this.tbMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbMain.Name = "tbMain";
            this.tbMain.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbMain.Size = new System.Drawing.Size(839, 468);
            this.tbMain.TabIndex = 0;
            this.tbMain.Text = "Main";
            this.tbMain.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lstLevels);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(577, 221);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(256, 235);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Levels";
            // 
            // lstLevels
            // 
            this.lstLevels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLevels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLevels.FormattingEnabled = true;
            this.lstLevels.Location = new System.Drawing.Point(3, 17);
            this.lstLevels.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstLevels.Name = "lstLevels";
            this.lstLevels.Size = new System.Drawing.Size(250, 214);
            this.lstLevels.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtLog);
            this.groupBox2.Controls.Add(this.cmbChatType);
            this.groupBox2.Controls.Add(this.txtChat);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(3, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(568, 452);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chat";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.Location = new System.Drawing.Point(3, 18);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(556, 399);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // cmbChatType
            // 
            this.cmbChatType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbChatType.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbChatType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChatType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbChatType.FormattingEnabled = true;
            this.cmbChatType.Items.AddRange(new object[] {
            "Chat",
            "OpChat",
            "AdminChat"});
            this.cmbChatType.Location = new System.Drawing.Point(477, 423);
            this.cmbChatType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbChatType.Name = "cmbChatType";
            this.cmbChatType.Size = new System.Drawing.Size(85, 21);
            this.cmbChatType.TabIndex = 16;
            this.cmbChatType.SelectedIndexChanged += new System.EventHandler(this.Focus);
            // 
            // txtChat
            // 
            this.txtChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChat.ForeColor = System.Drawing.Color.Gray;
            this.txtChat.Location = new System.Drawing.Point(3, 424);
            this.txtChat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size(468, 20);
            this.txtChat.TabIndex = 15;
            this.txtChat.Text = "Enter a message or a command";
            this.txtChat.Click += new System.EventHandler(this.txtChat_Enter);
            this.txtChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Chat);
            // 
            // mPlayerGroupBox
            // 
            this.mPlayerGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mPlayerGroupBox.Controls.Add(this.lstPlayers);
            this.mPlayerGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mPlayerGroupBox.Location = new System.Drawing.Point(577, 4);
            this.mPlayerGroupBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mPlayerGroupBox.Name = "mPlayerGroupBox";
            this.mPlayerGroupBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mPlayerGroupBox.Size = new System.Drawing.Size(256, 213);
            this.mPlayerGroupBox.TabIndex = 9;
            this.mPlayerGroupBox.TabStop = false;
            this.mPlayerGroupBox.Text = "Players";
            // 
            // lstPlayers
            // 
            this.lstPlayers.BackColor = System.Drawing.Color.White;
            this.lstPlayers.ContextMenuStrip = this.ctxPlayer;
            this.lstPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPlayers.FormattingEnabled = true;
            this.lstPlayers.Location = new System.Drawing.Point(3, 17);
            this.lstPlayers.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstPlayers.Name = "lstPlayers";
            this.lstPlayers.Size = new System.Drawing.Size(250, 192);
            this.lstPlayers.TabIndex = 0;
            // 
            // ctxPlayer
            // 
            this.ctxPlayer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kickMenuItem,
            this.banMenuItem,
            this.undoMenuItem,
            this.toolStripSeparator2,
            this.promoteMenuItem,
            this.demoteMenuItem,
            this.setrankMenuItem});
            this.ctxPlayer.Name = "ctxPlayer";
            this.ctxPlayer.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ctxPlayer.Size = new System.Drawing.Size(121, 142);
            // 
            // kickMenuItem
            // 
            this.kickMenuItem.Name = "kickMenuItem";
            this.kickMenuItem.Size = new System.Drawing.Size(120, 22);
            this.kickMenuItem.Text = "Kick";
            // 
            // banMenuItem
            // 
            this.banMenuItem.Name = "banMenuItem";
            this.banMenuItem.Size = new System.Drawing.Size(120, 22);
            this.banMenuItem.Text = "Ban";
            // 
            // undoMenuItem
            // 
            this.undoMenuItem.Name = "undoMenuItem";
            this.undoMenuItem.Size = new System.Drawing.Size(120, 22);
            this.undoMenuItem.Text = "Undo All";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(117, 6);
            // 
            // promoteMenuItem
            // 
            this.promoteMenuItem.Name = "promoteMenuItem";
            this.promoteMenuItem.Size = new System.Drawing.Size(120, 22);
            this.promoteMenuItem.Text = "Promote";
            // 
            // demoteMenuItem
            // 
            this.demoteMenuItem.Name = "demoteMenuItem";
            this.demoteMenuItem.Size = new System.Drawing.Size(120, 22);
            this.demoteMenuItem.Text = "Demote";
            // 
            // setrankMenuItem
            // 
            this.setrankMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setRankComboBoxItem});
            this.setrankMenuItem.Name = "setrankMenuItem";
            this.setrankMenuItem.Size = new System.Drawing.Size(120, 22);
            this.setrankMenuItem.Text = "Set Rank";
            // 
            // setRankComboBoxItem
            // 
            this.setRankComboBoxItem.Items.AddRange(new object[] {
            "Ur fat",
            "Ur Adopted",
            "Get a job",
            "i lub u"});
            this.setRankComboBoxItem.Name = "setRankComboBoxItem";
            this.setRankComboBoxItem.Size = new System.Drawing.Size(121, 23);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tbMain);
            this.tabControl1.Controls.Add(this.tbPlayers);
            this.tabControl1.Controls.Add(this.tbLevels);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(847, 496);
            this.tabControl1.TabIndex = 8;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem3});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Plugins";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.itmShutDown,
            this.itmRestart,
            this.itmProperties,
            this.menuItem10,
            this.itmKickAll,
            this.itmStopPhysics,
            this.itmUnload,
            this.itmSaveAll});
            this.menuItem2.Text = "Server";
            // 
            // itmShutDown
            // 
            this.itmShutDown.Index = 0;
            this.itmShutDown.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this.itmShutDown.Tag = "";
            this.itmShutDown.Text = "Shutdown";
            this.itmShutDown.Click += new System.EventHandler(this.shutdownMenuItem_Click);
            // 
            // itmRestart
            // 
            this.itmRestart.Index = 1;
            this.itmRestart.Text = "Restart";
            this.itmRestart.Click += new System.EventHandler(this.itmRestart_Click);
            // 
            // itmProperties
            // 
            this.itmProperties.Index = 2;
            this.itmProperties.Text = "Properties";
            this.itmProperties.Click += new System.EventHandler(this.itmProperties_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 3;
            this.menuItem10.Text = "-";
            // 
            // itmKickAll
            // 
            this.itmKickAll.Index = 4;
            this.itmKickAll.Text = "Kick Everybody";
            this.itmKickAll.Click += new System.EventHandler(this.itmKickAll_Click);
            // 
            // itmStopPhysics
            // 
            this.itmStopPhysics.Index = 5;
            this.itmStopPhysics.Text = "Stop Physics";
            this.itmStopPhysics.Click += new System.EventHandler(this.itmStopPhysics_Click);
            // 
            // itmUnload
            // 
            this.itmUnload.Index = 6;
            this.itmUnload.Text = "Unload Empty Levels";
            this.itmUnload.Click += new System.EventHandler(this.itmUnload_Click);
            // 
            // itmSaveAll
            // 
            this.itmSaveAll.Index = 7;
            this.itmSaveAll.Text = "Save All";
            this.itmSaveAll.Click += new System.EventHandler(this.itmSaveAll_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.portMenuItem});
            this.menuItem3.Text = "Tools";
            // 
            // portMenuItem
            // 
            this.portMenuItem.Index = 0;
            this.portMenuItem.Text = "Port Tools";
            this.portMenuItem.Click += new System.EventHandler(this.portMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(847, 518);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(863, 556);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MCForge Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tbPlayers.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tbMain.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.mPlayerGroupBox.ResumeLayout(false);
            this.ctxPlayer.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TabPage tbLevels;
        private System.Windows.Forms.TabPage tbPlayers;
        private System.Windows.Forms.TabPage tbMain;
        private System.Windows.Forms.GroupBox groupBox2;
        private Components.ColoredLogReader txtLog;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstPlayersBig;
        private System.Windows.Forms.GroupBox grpInfo;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem itmShutDown;
        private System.Windows.Forms.MenuItem itmRestart;
        private System.Windows.Forms.MenuItem itmProperties;
        private System.Windows.Forms.MenuItem itmKickAll;
        private System.Windows.Forms.MenuItem itmStopPhysics;
        private System.Windows.Forms.MenuItem itmUnload;
        private System.Windows.Forms.MenuItem itmSaveAll;
        private System.Windows.Forms.GroupBox mPlayerGroupBox;
        private System.Windows.Forms.ListBox lstPlayers;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstLevels;
        private System.Windows.Forms.ComboBox cmbChatType;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.ContextMenuStrip ctxPlayer;
        private System.Windows.Forms.ToolStripMenuItem kickMenuItem;
        private System.Windows.Forms.ToolStripMenuItem banMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem promoteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem demoteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setrankMenuItem;
        private System.Windows.Forms.ToolStripComboBox setRankComboBoxItem;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem portMenuItem;
        private Components.ColoredLogReader coloredLogReader1;
    }
}
