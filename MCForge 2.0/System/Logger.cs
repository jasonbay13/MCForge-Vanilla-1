/*
Copyright 2012 MCForge
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
using System;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MCForge.Core;
using System.Web.Configuration;
using System.Reflection;
using System.Globalization;

namespace MCForge.Utils {
    /// <summary>
    /// Logger Utility
    /// </summary>
    public class Logger {

        private static bool _flushMessages;
        private static bool _flushErrorMessages;

        /// <summary>
        /// This event is called when Logger.Log() is invoked
        /// </summary>
        public static event EventHandler<LogEventArgs> OnRecieveLog;

        /// <summary>
        /// This event is called when Logger.LogError() is invoked
        /// </summary>
        public static event EventHandler<LogEventArgs> OnRecieveErrorLog;

        internal static Thread _workerThread;
        internal static Queue<LogEventArgs> _flushQueue;

        internal static Thread _errorWorkerThread;
        internal static Queue<LogEventArgs> _flushErrorQueue;


        private static DateTime _lastTime;


        public static string CurrentLogFile {
            get { return _lastTime.ToString("MM-dd-yyyy") + "-MCForge.log"; }
        }


        /// <summary>
        /// Initializes Logger object staticly
        /// </summary>
        static Logger() {

            _flushMessages = true;
            _flushErrorMessages = true;

            _flushQueue = new Queue<LogEventArgs>();
            _flushErrorQueue = new Queue<LogEventArgs>();

            _workerThread = new Thread(Flush);
            _workerThread.Start();
            _errorWorkerThread = new Thread(FlushErrors);
            _errorWorkerThread.Start();


            _lastTime = DateTime.Now;
            FileUtils.CreateDirIfNotExist(FileUtils.LogsPath);
            FileUtils.CreateFileIfNotExist(FileUtils.LogsPath + CurrentLogFile, "--MCForge: Version: " + Assembly.GetExecutingAssembly().GetName().Version + ", OS:" + Environment.OSVersion + ", ARCH:" + (Environment.Is64BitOperatingSystem ? "x64" :  "x86") + ", CULTURE: " + CultureInfo.CurrentCulture +  Environment.NewLine);

        }

        /// <summary>
        /// De-Initializes the logger class
        /// </summary>
        public static void DeInit() {
            _flushMessages = false;
            _flushErrorMessages = false;
        }

        /// <summary>
        /// Logs a message, to be grabbed by a log event handler
        /// </summary>
        /// <param name="message">The message to be logged</param>
        /// <param name="logType">The log type</param>
        public static void Log(string message, LogType logType = LogType.Normal) {
            Color one = Color.White;
            switch (logType) {
                case LogType.Critical:
                    one = Color.DarkRed;
                    break;
                case LogType.Debug:
                    one = Color.DarkMagenta;
                    break;
                case LogType.Error:
                    one = Color.Red;
                    break;
                case LogType.Warning:
                    one = Color.Yellow;
                    break;
            }
                Log(message, one, Color.Black, logType);
        }

        /// <summary>
        /// Logs a message, to be grabbed by a log event handler
        /// </summary>
        /// <param name="message">The message to be logged</param>
        /// <param name="textColor">Color of the text</param>
        /// <param name="bgColor">Color of the background</param> 
        /// <param name="logType">The log type</param>
        public static void Log(string message, Color textColor, Color bgColor, LogType logType = LogType.Normal) {
                _flushQueue.Enqueue(new LogEventArgs(message, logType, textColor, bgColor));
        }

        /// <summary>
        /// Logs an exception, to be grabbed by a log event handler
        /// </summary>
        /// <param name="e">Exception to be logged</param>
        public static void LogError(Exception e) {
            _flushErrorQueue.Enqueue(new LogEventArgs(e.Message + "\n" + e.StackTrace, LogType.Error));
        }

        private static int count;
        /// <summary>
        /// Writes the log message to the log file
        /// </summary>
        /// <param name="log">Message to log</param>
        public static void WriteLog(string log) {
            if (count != 0) {
                int c = count;
                while (count >= c) Thread.Sleep(200);
            }
            count++;
            if (_lastTime.Day != DateTime.Now.Day) {
                _lastTime = DateTime.Now;
                FileUtils.CreateFileIfNotExist(FileUtils.LogsPath + CurrentLogFile, "--MCForge: Version: " + Assembly.GetExecutingAssembly().GetName().Version + ", OS:" + Environment.OSVersion + ", ARCH:" + (Environment.Is64BitOperatingSystem ? "x64" :  "x86") + ", CULTURE: " + CultureInfo.CurrentCulture +  Environment.NewLine);
            }

            try {
                using (var writer = new StreamWriter(FileUtils.LogsPath + CurrentLogFile, true)) {
                    writer.WriteLine(log);
                }
            }
            catch { }
            count--;
        }

        internal static void Flush() {
            while (_flushMessages) {
                Thread.Sleep(20);
                if (_flushQueue.Count > 0) {
                    var arg = _flushQueue.Dequeue();
                    if (OnRecieveLog != null)
                    {
                        try
                        {
                            if (arg.LogType == LogType.Debug && !Server.DebugMode)
                                continue;
                            OnRecieveLog(null, arg);
                            WriteLog(arg.Message);
                        }
                        catch { }
                    }
                }

            }
        }

        internal static void FlushErrors() {
            while (_flushErrorMessages) {
                Thread.Sleep(20);
                if (_flushErrorQueue.Count > 0) {
                    var arg = _flushErrorQueue.Dequeue();
                    if (OnRecieveErrorLog != null)
                        OnRecieveErrorLog(null,arg );
                     WriteLog("-------[Error]-------\n\r " + arg.Message + "\n\r---------------------");
                }

            }
        }

    }

    /// <summary>
    /// Log type for the specified message
    /// </summary>
    public enum LogType {
        /// <summary>
        /// The normal messages
        /// </summary>
        Normal,
        /// <summary>
        /// Error messages
        /// </summary>
        Error,
        /// <summary>
        /// Debug messages (only appears if the server is in debugging mode)
        /// </summary>
        Debug,
        /// <summary>
        /// Warning messages
        /// </summary>
        Warning,
        /// <summary>
        /// Critical messages
        /// </summary>
        Critical,

    }

    /// <summary>
    ///Log event where object holding the event
    ///would get a string (the message)
    /// </summary>
    public class LogEventArgs : EventArgs {

        /// <summary>
        /// Get or set the message of the log event
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Get or set the type of log
        /// </summary>
        public LogType LogType { get; set; }


        /// <summary>
        /// Get or set the color of the text
        /// </summary>
        public Color TextColor { get; set; }


        /// <summary>
        /// Get or set the color of the background
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Default constructor for creating a log event
        /// </summary>
        /// <param name="log">Message of the log event</param>
        /// <param name="logType">Type of log event</param>
        public LogEventArgs(string log, LogType logType) {
            Message = log;
            LogType = logType;
            TextColor = Color.White;
            BackgroundColor = Color.Black;
        }

        /// <summary>
        /// Default constructor for creating a log event
        /// </summary>
        /// <param name="log">Message of the log event</param>
        /// <param name="logType">Type of log event</param>
        /// <param name="textColor">Color of the text</param>
        /// <param name="bgColor">Color of the background</param>
        public LogEventArgs(string log, LogType logType, Color textColor, Color bgColor) {
            Message = log;
            LogType = logType;
            TextColor = textColor;
            BackgroundColor = bgColor;
        }

    }
}