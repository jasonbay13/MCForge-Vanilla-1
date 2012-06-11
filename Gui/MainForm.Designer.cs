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
namespace MCForge.Gui
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mStatus = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mPlayerGroupBox = new System.Windows.Forms.GroupBox();
            this.mPlayersListBox = new System.Windows.Forms.ListBox();
            this.chatBox = new System.Windows.Forms.TextBox();
            this.chatButtonChange = new System.Windows.Forms.ComboBox();
            this.newsFeeder1 = new MCForge.Gui.Components.NewsFeeder(this.components);
            this.coloredReader1 = new MCForge.Gui.Components.ColoredReader(this.components);
            this.menuStrip1.SuspendLayout();
            this.mPlayerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mStatus
            // 
            this.mStatus.Location = new System.Drawing.Point(0, 475);
            this.mStatus.Name = "mStatus";
            this.mStatus.Size = new System.Drawing.Size(826, 22);
            this.mStatus.TabIndex = 1;
            this.mStatus.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pluginsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(826, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.pluginsToolStripMenuItem.Text = "Plugins";
            // 
            // mPlayerGroupBox
            // 
            this.mPlayerGroupBox.Controls.Add(this.mPlayersListBox);
            this.mPlayerGroupBox.Location = new System.Drawing.Point(594, 27);
            this.mPlayerGroupBox.Name = "mPlayerGroupBox";
            this.mPlayerGroupBox.Size = new System.Drawing.Size(220, 412);
            this.mPlayerGroupBox.TabIndex = 3;
            this.mPlayerGroupBox.TabStop = false;
            this.mPlayerGroupBox.Text = "Players";
            // 
            // mPlayersListBox
            // 
            this.mPlayersListBox.BackColor = System.Drawing.Color.White;
            this.mPlayersListBox.FormattingEnabled = true;
            this.mPlayersListBox.Location = new System.Drawing.Point(7, 20);
            this.mPlayersListBox.Name = "mPlayersListBox";
            this.mPlayersListBox.Size = new System.Drawing.Size(207, 381);
            this.mPlayersListBox.TabIndex = 0;
            // 
            // chatBox
            // 
            this.chatBox.Location = new System.Drawing.Point(12, 419);
            this.chatBox.Name = "chatBox";
            this.chatBox.Size = new System.Drawing.Size(487, 20);
            this.chatBox.TabIndex = 5;
            this.chatBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Chat);
            // 
            // chatButtonChange
            // 
            this.chatButtonChange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chatButtonChange.FormattingEnabled = true;
            this.chatButtonChange.Items.AddRange(new object[] {
            "Chat",
            "OpChat",
            "AdminChat"});
            this.chatButtonChange.Location = new System.Drawing.Point(505, 417);
            this.chatButtonChange.Name = "chatButtonChange";
            this.chatButtonChange.Size = new System.Drawing.Size(83, 21);
            this.chatButtonChange.TabIndex = 6;
            // 
            // newsFeeder1
            // 
            this.newsFeeder1.BackColor = System.Drawing.Color.White;
            this.newsFeeder1.Items.Add("1234");
            this.newsFeeder1.Items.Add("1234");
            this.newsFeeder1.Items.Add("12334");
            this.newsFeeder1.Items.Add("1234");
            this.newsFeeder1.Items.Add("12");
            this.newsFeeder1.Items.Add("34125123651346136");
            this.newsFeeder1.Items.Add("dsfdcv3e1r");
            this.newsFeeder1.Items.Add("123 r13424");
            this.newsFeeder1.Items.Add("c1c34");
            this.newsFeeder1.Location = new System.Drawing.Point(0, 452);
            this.newsFeeder1.Name = "newsFeeder1";
            this.newsFeeder1.ReadOnly = true;
            this.newsFeeder1.Size = new System.Drawing.Size(826, 20);
            this.newsFeeder1.TabIndex = 7;
            this.newsFeeder1.Text = "Connecting...";
            // 
            // coloredReader1
            // 
            this.coloredReader1.BackColor = System.Drawing.Color.White;
            this.coloredReader1.Location = new System.Drawing.Point(12, 28);
            this.coloredReader1.Name = "coloredReader1";
            this.coloredReader1.ReadOnly = true;
            this.coloredReader1.Size = new System.Drawing.Size(576, 383);
            this.coloredReader1.TabIndex = 8;
            this.coloredReader1.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 497);
            this.Controls.Add(this.coloredReader1);
            this.Controls.Add(this.newsFeeder1);
            this.Controls.Add(this.chatButtonChange);
            this.Controls.Add(this.chatBox);
            this.Controls.Add(this.mPlayerGroupBox);
            this.Controls.Add(this.mStatus);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MCForge Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mPlayerGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.StatusStrip mStatus;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private System.Windows.Forms.GroupBox mPlayerGroupBox;
        private System.Windows.Forms.ListBox mPlayersListBox;
        private System.Windows.Forms.TextBox chatBox;
        private System.Windows.Forms.ComboBox chatButtonChange;
        private Components.NewsFeeder newsFeeder1;
        private Components.ColoredReader coloredReader1;
	}
}
