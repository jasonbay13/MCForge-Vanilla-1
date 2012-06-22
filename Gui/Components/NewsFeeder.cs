/*
Copyright 2012 MCForge
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.Threading;

namespace MCForge.Gui.Components {
    public partial class NewsFeeder : DomainUpDown {

        public NewsFeeder() {
            InitializeComponent();
            
        }

        public NewsFeeder(IContainer container) {
            container.Add(this);
            InitializeComponent();
        }


        public void StartRead() {
            WebRequest request = WebRequest.Create("http://headdetect.com/news.txt");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null || response.StatusCode != HttpStatusCode.OK) {
                Items.Clear();
                Items.Add("Cannot connect to news server");
                return;
            }
            Items.Clear();
            Items.Add("Connecting...");
            try {
                using (var client = new WebClient()) {

                    client.DownloadStringAsync(new Uri("http://headdetect.com/news.txt"), null);
                    client.DownloadStringCompleted += (sender, args) => {
                        Items.Clear();
                        Items.AddRange(args.Result.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                        var Timer = new System.Timers.Timer(5000);
                        Timer.Elapsed += DisplayPosts;
                        Timer.Start();
                    };

                }
            }
            catch {
                Items.Clear();
                Items.Add("Cannot connect to news server");
            }

        }

        void DisplayPosts(object sender, System.Timers.ElapsedEventArgs args) {
            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate {
                    DisplayPosts(sender, args);
                });
                return;
            }

            int i = SelectedIndex + 1 < Items.Count ? SelectedIndex + 1 : 0;
            if (((string)Items[i])[0] == '!')
                ForeColor = Color.Red;
            else
                ForeColor = DefaultForeColor;
            SelectedIndex = i;
        }

        /*
         * 
         */
    }
}
