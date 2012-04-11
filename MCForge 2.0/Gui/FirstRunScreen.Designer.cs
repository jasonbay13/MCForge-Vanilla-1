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
            // FirstRunScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 778);
            this.Controls.Add(this.DoNotDisplayAgain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FirstRunScreen";
            this.Text = "First Run";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox DoNotDisplayAgain;
    }
}