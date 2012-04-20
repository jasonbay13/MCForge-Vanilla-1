using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MCForge.Gui.API {
    public interface MCForgeGui {

        /// <summary>
        /// Get the form.
        /// </summary>
        Form Form { get;}

        /// <summary>
        /// Title of the menu item 
        /// </summary>
        string MenuTitle { get; }

        
        /// <summary>
        /// Image of the menu item
        /// </summary>
        /// <remarks>This can be null</remarks>
        Image MenuImage { get; }

    }
}
