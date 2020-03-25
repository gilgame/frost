/* -----------------------------------------------------------------------------
 * Logger.cs
 * 
 * Copyright (c) 2020 TD Fellows, TD Frost. All rights reserved.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subjectto the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 * -----------------------------------------------------------------------------
 */

using System;
using System.IO;
using System.Text;

namespace Frost.Lib.Logging
{
    /// <summary>
    /// A class that provides error and message logging to a file.
    /// </summary>
    public static class Logger
    {
        private const string D_SOURCE = "Frost";
        private const string D_FILE = "error.log";

        /// <summary>
        /// Gets the current logger state.
        /// </summary>
        public static LoggerState State { get; private set; } = LoggerState.Ready;

        /// <summary>
        /// Gest or sets the message source.
        /// </summary>
        public static string Source { get; set; }

        /// <summary>
        /// Gets or sets the log file name.
        /// </summary>
        public static string Filename { get; set; }

        /// <summary>
        /// Occurs when the logger has forcibly stopped.
        /// </summary>
        public static event LoggerEventHandler Stopped;

        /// <summary>
        /// Logs a message to a file destination.
        /// </summary>
        /// <param name="message">The message body.</param>
        public static void Log(string message)
        {
            if (State == LoggerState.Stopped)
                return;

            try
            {
                //attempt to log message to file
                var file = CreateFile();
                var text = Format(message);

                File.AppendAllText(file, text);
            }
            catch (Exception ex)
            {
                //set logger state to stopped if unable to log message
                State = LoggerState.Stopped;
                RaiseStopped(ex.Message);
            }
        }

        private static string CreateFile()
        {
            //use default values if properties aren't set
            string source = String.IsNullOrEmpty(Source) ? D_SOURCE : Source;
            string name = String.IsNullOrEmpty(Filename) ? D_FILE : Filename;

            //use local appdata as directory root
            string root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            //put together root/source//filename
            string path = Path.Combine(root, source);
            string file = Path.Combine(path, name);

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!File.Exists(file))
                {
                    File.Create(file);
                }
            }
            catch (Exception ex)
            {
                throw new LoggerException(ex.Message);
            }

            return file;
        }

        private static string Format(string message)
        {
            StringBuilder formatted = new StringBuilder(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));

            formatted.AppendLine("-------------------");
            formatted.AppendLine(message);
            formatted.AppendLine();

            return formatted.ToString();
        }

        private static void RaiseStopped(string message)
        {
            Stopped?.Invoke(null, new LoggerEventArgs(message));
        }
    }

    /// <summary>
    /// Specifies the current Logger state.
    /// </summary>
    public enum LoggerState
    {
        Ready,
        Stopped,
    }
}
