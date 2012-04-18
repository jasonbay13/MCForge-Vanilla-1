namespace MCForge.Gui
{
    partial class FirstRunScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstRunScreen));
            this.DoNotDisplayAgain = new System.Windows.Forms.CheckBox();
            this.MCForge_Logo = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.MCForge_Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // DoNotDisplayAgain
            // 
            this.DoNotDisplayAgain.AutoSize = true;
            this.DoNotDisplayAgain.Location = new System.Drawing.Point(567, 711);
            this.DoNotDisplayAgain.Name = "DoNotDisplayAgain";
            this.DoNotDisplayAgain.Size = new System.Drawing.Size(171, 17);
            this.DoNotDisplayAgain.TabIndex = 0;
            this.DoNotDisplayAgain.Text = "Do not display this page again.";
            this.DoNotDisplayAgain.UseVisualStyleBackColor = true;
            this.DoNotDisplayAgain.CheckedChanged += new System.EventHandler(this.DoNotDisplayAgain_CheckedChanged);
            // 
            // MCForge_Logo
            // 
            this.MCForge_Logo.Image = ((System.Drawing.Image)(resources.GetObject("MCForge_Logo.Image")));
            this.MCForge_Logo.Location = new System.Drawing.Point(190, 26);
            this.MCForge_Logo.Name = "MCForge_Logo";
            this.MCForge_Logo.Size = new System.Drawing.Size(408, 85);
            this.MCForge_Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MCForge_Logo.TabIndex = 1;
            this.MCForge_Logo.TabStop = false;
            this.MCForge_Logo.Click += new System.EventHandler(this.MCForge_Logo_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Arial", 8.25F);
            this.richTextBox1.Location = new System.Drawing.Point(30, 138);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(418, 211);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // FirstRunScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 778);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.MCForge_Logo);
            this.Controls.Add(this.DoNotDisplayAgain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FirstRunScreen";
            this.Text = "First Run";
            ((System.ComponentModel.ISupportInitialize)(this.MCForge_Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox DoNotDisplayAgain;
        private System.Windows.Forms.PictureBox MCForge_Logo;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}