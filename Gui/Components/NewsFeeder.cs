using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Drawing;

namespace MCForge.Gui.Components {
    public partial class NewsFeeder : DomainUpDown {

        public bool ShowPosts { get; set; }

        public NewsFeeder() {
            InitializeComponent();
        }

        public NewsFeeder(IContainer container) {
            container.Add(this);
            InitializeComponent();
        }

        private List<string> newsPosts;

        public void StartRead() {
            newsPosts = new List<string>();
            
            using (var client = new WebClient()) {
                string[] lines = client.DownloadString("http://headdetect.com/news.txt").Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
                newsPosts.AddRange(lines);
                Items.Clear();
                Items.AddRange(lines);
            }
        }

        public void DisplayPosts() {
            Timer mTime = new Timer();
            mTime.Interval = 5000;
            mTime.Tick += (sender, args) => {
                int i = SelectedIndex + 1 < newsPosts.Count ? SelectedIndex + 1 : 0;
                if (newsPosts[i][0] == '!')
                    ForeColor = Color.Red;
                else
                    ForeColor = DefaultForeColor;
                SelectedIndex = i;
            };
            mTime.Start();
        }

    }
}
