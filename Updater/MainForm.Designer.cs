/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 6/26/2012
 * Time: 2:34 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Updater
{
    partial class MainForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.newsFeeder1 = new MCForge.Gui.Components.NewsFeeder(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // newsFeeder1
            // 
            this.newsFeeder1.Location = new System.Drawing.Point(2, 91);
            this.newsFeeder1.Name = "newsFeeder1";
            this.newsFeeder1.Size = new System.Drawing.Size(347, 20);
            this.newsFeeder1.TabIndex = 0;
            this.newsFeeder1.Text = "newsFeeder1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(2, 33);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(347, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(193, 62);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(156, 28);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Start MCForge after update";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 112);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.newsFeeder1);
            this.Name = "MainForm";
            this.Text = "Updater";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private MCForge.Gui.Components.NewsFeeder newsFeeder1;
    }
}
