using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using MCForge.Utils;
using MCForge;
using MCForge.Utils.Settings;

namespace MCForge.Gui {
    class Program {
        static void Main(string[] args) {
            ServerSettings.Init();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //new Thread(new ThreadStart(Server.Init)).Start(); Will be called when the main form is created
            //Application.Run(new LogoForm());
            Application.Run(new MainForm());
        }
    }
}
