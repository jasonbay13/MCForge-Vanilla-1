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
*/
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge;

namespace MCForge.Interface.Plugin
{
    public interface IPluginMessageViewer : IPlugin
    {
        /// <summary>
        /// Starts showing a message.
        /// </summary>
        /// <param name="p">The player to show the message to.</param>
        /// <param name="message">The message to show.</param>
        void ShowMessage(Player p, string message);
        /// <summary>
        /// Appends text at the end of the message.
        /// </summary>
        /// <param name="p">The player to append the message to</param>
        /// <param name="text">The message to append.</param>
        void AppendText(Player p, string text);
        /// <summary>
        /// Shows the next page if possible.
        /// </summary>
        /// <param name="p">The player to show the next page to. </param>
        void ShowNextPage(Player p);

        /// <summary>
        /// Shows a selected page if possible.
        /// </summary>
        /// <param name="p">The player to show the page to.</param>
        /// <param name="page">The page to show.</param>
        void ShowPage(Player p, int page);

        /// <summary>
        /// Shows next line if possible.
        /// </summary>
        /// <param name="p">The player to show the line to.</param>
        void ShowNextLine(Player p);

        /// <summary>
        /// Shows previous page if possible.
        /// </summary>
        /// <param name="p">The player to show the page to.</param>
        void ShowPreviousPage(Player p);

        /// <summary>
        /// Shows previous line if possible.
        /// </summary>
        /// <param name="p">The player to show the line to.</param>
        void ShowPreviousLine(Player p);

        /// <summary>
        /// Stops showing the message.
        /// </summary>
        /// <param name="p">The player to stop showing the message to.</param>
        void Stop(Player p);
    }
}