/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 6/26/2012
 * Time: 2:34 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace Updater
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        public static MainForm form;
        IAction current;
        public MainForm(string[] args) {
            InitializeComponent();
        }
        public MainForm() {
            InitializeComponent();
        }
        
        void MainFormLoad(object sender, EventArgs e) {
            ChangeLable("Starting..");
            newsFeeder1.StartRead();
            form = this;
            Thread t = new Thread(new ThreadStart(StartActions));
            t.Start();
        }
        
        public void StartActions() {
            IAction.actions.Add(new Normal_Update());
            if (Program.dlactions.Count > 0) {
                foreach (string dl in Program.dlactions) {
                    IAction.actions.Add(new Download_Action(dl));
                }
            }
            Program.dlactions.Clear();
            current = IAction.GetNext();
            ChangeLable(current.action);
            current.OnProgress += new IAction.ProgressChanged(action_progress);
            current.Completed += new IAction.ProgressChanged(action_Completed);
            current.Start();
        }
        public void action_Completed() {
            if (this.InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate { action_Completed(); });
                return;
            }
            Thread.Sleep(500);
            current.Completed -= action_Completed;
            current.OnProgress -= action_progress;
            ChangeProgress(0);
            if (IAction.NextAction()) {
                NextAction();
            }
            else {
                ChangeProgress(100);
                ChangeLable("Done!");
            }
        }
        public void NextAction() {
            current = null;
            current = IAction.GetNext();
            ChangeLable(current.action);
            current.Completed += new IAction.ProgressChanged(action_Completed);
            current.OnProgress += new IAction.ProgressChanged(action_progress);
            current.Start();
        }
        public void action_progress() {
            progressBar1.Value = current.progress;
        }
        public void ChangeProgress(int i) {
            progressBar1.Value = i;
        }
        
        public void ChangeLable(string newtext) {
            this.label1.Text = newtext;
        }
    }
}
