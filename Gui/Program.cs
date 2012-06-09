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
            //Application.Run(new LogoForm());
            Application.Run(new MainForm());
        }
    }
}
