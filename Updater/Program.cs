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
using System.Windows.Forms;

namespace Updater
{
    /// <summary>
    /// Class with program entry point.
    /// </summary>
    public sealed class Program
    {
        public static string updateserver = "http://update.mcforge.net";
        public static bool done = false;
        public static bool upgrade = false;
        public static List<string> dlactions = new List<string>();
        /// <summary>
        /// If the server is using mono
        /// </summary>
        public static bool OnMono { get { return (Type.GetType("Mono.Runtime") != null); } }
        /// <summary>
        /// Program entry point.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            CheckArgs(args);
            if (!OnMono) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else {
                Console.WriteLine("Linux system detected..");
                IAction.actions.Add(new Normal_Update());
                if (dlactions.Count > 0) {
                    foreach (string dl in dlactions) {
                        IAction.actions.Add(new Download_Action(dl));
                    }
                }
                IAction action = IAction.GetNext();
                Console.WriteLine(action.action);
                action.Completed += new IAction.ProgressChanged(action_Completed);
                action.Start();
                while (!done) {
                    System.Threading.Thread.Sleep(100);
                }
                Console.ReadKey();
            }
        }

        static void action_Completed()
        {
            if (IAction.NextAction()) {
                IAction action = IAction.GetNext();
                Console.WriteLine(action.action);
                action.Completed += action_Completed;
                action.Start();
            }
            else {
                Console.WriteLine("Update Complete!");
                done = true;
            }
        }
        public static void CheckArgs(string[] args) {
            foreach (string s in args) {
                switch (s.Split('=')[0].Trim()) {
                    case "update-server":
                        updateserver = s.Split('=')[1].Trim();
                        break;
                    case "upgrade":
                        upgrade = bool.Parse(s.Split('=')[1].Trim());
                        break;
                    case "action":
                        string value = s.Split('=')[1].Trim();
                        if (value.IndexOf("http://") == -1)
                            value = updateserver + "/Patch/" + value;
                        dlactions.Add(value);
                        break;
                }
            }
        }
        
    }
}
