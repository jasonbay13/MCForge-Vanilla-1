/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 6/26/2012
 * Time: 2:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel;

namespace Updater
{
    /// <summary>
    /// Description of Action.
    /// </summary>
    public abstract class IAction
    {
        #region Abstract Methods
        /// <summary>
        /// The file to downoad
        /// </summary>
        public abstract string download { get; }
        /// <summary>
        /// The name to save it as
        /// </summary>
        public abstract string saveas { get; }
        /// <summary>
        /// This will appear on the text above the progress bar
        /// </summary>
        public abstract string action { get; }
        /// <summary>
        /// If this update requires any action
        /// </summary>
        public abstract void Action();
        #endregion
        public WebClient wc;
        public int progress;
        public delegate void ProgressChanged();
        public event ProgressChanged OnProgress;
        public event ProgressChanged Completed;
        public static List<IAction> actions = new List<IAction>();
        static int current = 0;
        public IAction()
        {
        }
        /// <summary>
        /// Start the action
        /// </summary>
        public virtual void Start() {
            if (!String.IsNullOrEmpty(download)) {
                wc = new WebClient();
                wc.DownloadFileAsync(new Uri(download), saveas);
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            }
            else {
                this.Action();
                current++;
                if (Completed != null)
                    Completed();
            }
        }
        
        public static bool NextAction() {
            return current < actions.Count;
        }
        public static IAction GetNext() {
            return actions[current];
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            wc.Dispose();
            this.Action();
            current++;
            if (Completed != null)
                Completed();
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (MainForm.form.InvokeRequired) {
                MainForm.form.BeginInvoke((MethodInvoker)delegate { wc_DownloadProgressChanged(sender, e); });
                return;
            }
            progress = e.ProgressPercentage;
            if (OnProgress != null)
                OnProgress();
        }
    }
}
