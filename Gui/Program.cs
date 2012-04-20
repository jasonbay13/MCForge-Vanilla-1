using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using MCForge.Utilities.Settings;
using MCForge.Utilities;
using MCForge.Utils;
using MCForge;

namespace MCForge.Gui {
    class Program {
        static void Main(string[] args) {
            Logger.Init();
            ServerSettings.Init();
            ColorUtils.Init();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //new Thread(new ThreadStart(Server.Init)).Start(); Will be called when the main form is created
            Application.Run(new frmMain());
        }
    }
}
