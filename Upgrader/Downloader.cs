/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 7/10/2012
 * Time: 1:27 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using System.Net;

namespace MCForge
{
    /// <summary>
    /// Description of Downloader.
    /// </summary>
    public class Downloader
    {
        bool done;
        int old;
        public Downloader()
        {
        }
        public void Download(string file, string path) {
            Console.WriteLine(" ");
            Console.Write("[");
            using (WebClient wc = new WebClient()) {
                wc.DownloadFileAsync(new Uri(file), path);
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                wc.DownloadFileCompleted += delegate { Console.Write("]"); done = true; };
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage * 78;
            if (progress > old) {
                Console.Write("=");
                old++;
            }
        }
        public void Wait() {
            while (!done) {
                Thread.Sleep(100);
            }
        }
        public void Reset() {
            done = false;
            old = 0;
        }
    }
}
