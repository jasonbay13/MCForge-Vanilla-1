using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MCForge.Gui.Components {

    /// <summary>
    /// A richtextbox that colors incoming text using mincraft color codes.
    /// </summary>
    public partial class ColoredLogReader : StyledRichTextBox {

        private string Date {
            get {
                return "[" + DateTime.Now.ToString("T") + "] ";
            }
        }

        public ColoredLogReader() {
            InitializeComponent();
            ReadOnly = true;
            BackColor = Color.White;
        }

        public ColoredLogReader(IContainer container) {
            container.Add(this);
            InitializeComponent();
            ReadOnly = true;
            BackColor = Color.White;
        }

        /// <summary>
        /// Appends the log.
        /// </summary>
        /// <param name="text">The text to log.</param>
        public void AppendLog(string text) {
            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate { AppendLog(text); });
                return;
            }

            AppendLog(Date, Color.Gray, Color.White);

            if (!text.Contains('&') && !text.Contains('%')) {
                AppendLog(text, Color.Black, Color.White);
                return;
            }

            string[] messagesSplit = text.Split(new[] { '%', '&' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < messagesSplit.Length; i++) {
                string split = messagesSplit[i];
                if (String.IsNullOrWhiteSpace(split))
                    continue;
                Color? color = GetColorFromChar(split[0]);
                AppendLog(color != null ? split.Substring(1) : split, color ?? Color.Black, Color.White);
            }

        }

        /// <summary>
        /// Appends the log.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="foreColor">Color of the foreground.</param>
        /// <param name="bgColor">Color of the background.</param>
        public void AppendLog(string text, Color foreColor, Color bgColor) {
            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate { AppendLog(text, foreColor, bgColor); });
                return;
            }

            SelectionStart = TextLength;
            SelectionLength = 0;
            SelectionColor = foreColor;
            SelectionBackColor = bgColor;
            AppendText(text);
            SelectionBackColor = BackColor;
            SelectionColor = ForeColor;

        }

        
        private void ColoredReader_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e) {
            if(!e.LinkText.StartsWith("http://www.minecraft.net/classic/play/")){
                if(MessageBox.Show("Never open links from people that you don't trust!", "Warning!!", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;
            }

            Process.Start(e.LinkText);

        }

        /// <summary>
        /// Scrolls to the end of the log
        /// </summary>
        public void ScrollToEnd() {
            if (InvokeRequired) {
                Invoke((MethodInvoker)ScrollToEnd);
                return;
            }


            Select(Text.Length - 1, 0);
            ScrollToCaret();
        }

        /// <summary>
        /// Gets a color from a char.
        /// </summary>
        /// <param name="c">The char.</param>
        /// <returns>A color, that can be null</returns>
        public Color? GetColorFromChar(char c) {
            switch (c) {
                case '0': return Color.Black;
                case '1': return Color.DarkBlue;
                case '2': return Color.DarkGreen;
                case '3': return Color.DarkTurquoise;
                case '4': return Color.DarkRed;
                case '5': return Color.Purple;
                case '6': return Color.Gold;
                case '7': return Color.Gray;
                case '8': return Color.DarkGray;
                case '9': return Color.Blue;
                case 'a': return Color.LightGreen;
                case 'b': return Color.Teal;
                case 'c': return Color.Red;
                case 'd': return Color.Pink;
                case 'e': return Color.Yellow;
                case 'f': return Color.White;
                default: return null;
            }
        }


    }
}
