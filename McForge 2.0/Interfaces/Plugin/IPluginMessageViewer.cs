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