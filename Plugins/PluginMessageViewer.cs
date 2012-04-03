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
*/﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge;
using System.Timers;
using MCForge.Interface.Plugin;
using MCForge.Entity;

namespace PluginsDLL
{
    public class PluginMessageViewer : IPluginMessageViewer
    {
        List<Viewer> viewing = new List<Viewer>();
        public void ShowMessage(Player p, string message)
        {
            viewing.Add(new Viewer(p, message, 0));
            viewing[viewing.Count - 1].NeedsUpdate += new Viewer.update(SendMessage);
            viewing[viewing.Count - 1].ForceUpdate();
        }
        private void SendMessage(Viewer v)
        {
            for (int i = totalHeight - messageHeight; i > 0; i--)
                v.p.SendMessage("");
            for (int i = v.pos; i < v.pos + messageHeight; i++)
            {
                if (i < v.message.Length)
                    v.p.SendMessage(v.message[i]);
                else
                    v.p.SendMessage("");
            }
        }
        private static int width = 100;
        private static int messageHeight = 10;
        private static int totalHeight = 20;
        private static char col = '%';
        private static double refreshTime = 9500;
        private static int calcLength(string part)
        {
            return part.Length - 2 * part.Count(c => { return c == col; });
        }
        private static string getMaxString(string line, int max)
        {
            int o = line.Length - line.TrimStart().Length;
            int vlen = 0;
            int i = o;
            for (; i + max < line.Length && vlen < max; i++)
            {
                vlen = calcLength(line.Substring(o, max + i));
            }
            if (o + max + i >= line.Length) return line;
            int t = line.Substring(o, max + i).LastIndexOf(' ');
            if (t >= 0) i -= i + max - t;
            return line.Substring(0, (o + max + i < line.Length) ? o + max + i : line.Length).TrimEnd();
        }
        private static string[] prepare(string text)
        {
            List<string> ret = new List<string>();
            string[] lines = text.Split('\n');
            foreach (string l in lines)
            {
                int i = 0;
                string t = getMaxString(l.Substring(i), width);
                while (i + t.Length < l.Length)
                {
                    ret.Add(t.TrimStart());
                    i += t.Length;
                    t = getMaxString(l.Substring(i), width);
                }
                ret.Add(t.TrimStart());
            }
            return ret.ToArray();
        }
        private int indexOfPlayer(Player p)
        {
            return viewing.FindIndex(v => { return v.p.USERNAME == p.USERNAME; });
        }
        public string Name
        {
            get { return "MessageViewer"; }
        }

        public string Author
        {
            get { return "ninedrafted"; }
        }

        public int Version
        {
            get { return 1; }
        }

        public string CUD
        {
            get { throw new NotImplementedException(); }
        }

        public void Initialize()
        {
        }
        public class Viewer
        {
            public Viewer(Player p, string message, int pos) : this(p, prepare(message), pos) { }
            private Viewer(Player p, string[] message, int pos)
            {
                this.p = p;
                this.message = message;
                this.pos = pos;
                t = new Timer(refreshTime);
                t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            }
            void t_Elapsed(object sender, ElapsedEventArgs e)
            {
                if (NeedsUpdate != null)
                {
                    NeedsUpdate(this);
                }
            }
            public Player p;
            public string[] message;
            public int pos;
            public void ForceUpdate()
            {
                if (NeedsUpdate != null)
                {
                    t.Stop();
                    NeedsUpdate(this);
                    t.Start();
                }
            }
            public event update NeedsUpdate;
            public delegate void update(Viewer v);
            Timer t;
        }
        private void setMessagePos(Player p, int pos)
        {
            int index = indexOfPlayer(p);
            if (index > 0)
            {
                if (pos > 0 && pos < viewing[index].message.Length)
                {
                    viewing[index].pos = pos;
                }
            }
        }
        private void moveMessagePos(Player p, int move)
        {
            int index = indexOfPlayer(p);
            if (index > 0)
            {
                if (viewing[index].pos + move > 0 && viewing[index].pos + move + messageHeight < viewing[index].message.Length)
                {
                    viewing[index].pos = viewing[index].pos + move;
                }
            }
        }
        public void ShowNextPage(Player p)
        {
            moveMessagePos(p, messageHeight);
        }

        public void ShowPage(Player p, int page)
        {
            setMessagePos(p, (page - 1) * messageHeight);
        }

        public void ShowNextLine(Player p)
        {
            moveMessagePos(p, 1);
        }

        public void ShowPreviousPage(Player p)
        {
            moveMessagePos(p, -messageHeight);
        }

        public void ShowPreviousLine(Player p)
        {
            moveMessagePos(p, -1);
        }
        private void SendEnd(Player p)
        {
            int index = indexOfPlayer(p);
            int i = viewing[index].message.Length - totalHeight;
            for (i = (i < 0) ? 0 : i; i < viewing[index].message.Length; i++)
            {
                p.SendMessage(viewing[index].message[i]);
            }
        }
        /// <summary>
        /// Appends text.
        /// </summary>
        /// <param name="p">The player</param>
        /// <param name="text">The Text</param>
        public void AppendText(Player p, string text)
        {
            int index = indexOfPlayer(p);
            bool update = viewing[index].pos + messageHeight > viewing[index].message.Length;
            string[] append = prepare(text);
            string[] message = new string[append.Length + viewing[index].message.Length];
            viewing[index].message.CopyTo(message, 0);
            append.CopyTo(message, viewing[index].message.Length);
            viewing[index].message = message;
            if (update) viewing[index].ForceUpdate();
        }


        public void Stop(Player p)
        {
            int i = indexOfPlayer(p);
            SendEnd(p);
            if (i > 0) viewing.RemoveAt(i);
        }
    }
}