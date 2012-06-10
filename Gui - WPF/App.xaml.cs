using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MCForge.Utils;
using MCForge;
using MCForge.Utils.Settings;

namespace MCForge.Gui.Wpf {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private void Application_Startup(object sender, StartupEventArgs e) {
            ServerSettings.Init();
        }

    }
}
