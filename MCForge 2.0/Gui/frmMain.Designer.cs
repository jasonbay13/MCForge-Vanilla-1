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
namespace MCForge
{
	partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mStatus = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mPlayerGroupBox = new System.Windows.Forms.GroupBox();
            this.mPlayersListBox = new System.Windows.Forms.ListBox();
            this.chatBox = new System.Windows.Forms.TextBox();
            this.chatButtonChange = new System.Windows.Forms.ComboBox();
            this.coloredTextBox1 = new MCForge.Gui.Components.ColoredTextBox(this.components);
            this.menuStrip1.SuspendLayout();
            this.mPlayerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mStatus
            // 
            this.mStatus.Location = new System.Drawing.Point(0, 442);
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
            this.chatBox.Size = new System.Drawing.Size(499, 20);
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
            this.chatButtonChange.Location = new System.Drawing.Point(517, 417);
            this.chatButtonChange.Name = "chatButtonChange";
            this.chatButtonChange.Size = new System.Drawing.Size(71, 21);
            this.chatButtonChange.TabIndex = 6;
            // 
            // coloredTextBox1
            // 
            this.coloredTextBox1.AutoWordSelection = true;
            this.coloredTextBox1.BackColor = System.Drawing.Color.White;
            this.coloredTextBox1.Location = new System.Drawing.Point(12, 27);
            this.coloredTextBox1.Name = "coloredTextBox1";
            this.coloredTextBox1.ReadOnly = true;
            this.coloredTextBox1.Size = new System.Drawing.Size(576, 386);
            this.coloredTextBox1.TabIndex = 4;
            this.coloredTextBox1.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 464);
            this.Controls.Add(this.chatButtonChange);
            this.Controls.Add(this.chatBox);
            this.Controls.Add(this.coloredTextBox1);
            this.Controls.Add(this.mPlayerGroupBox);
            this.Controls.Add(this.mStatus);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MCForge Server";
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
        private Gui.Components.ColoredTextBox coloredTextBox1;
        private System.Windows.Forms.TextBox chatBox;
        private System.Windows.Forms.ComboBox chatButtonChange;
	}
}

