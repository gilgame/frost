/* -----------------------------------------------------------------------------
 * ConsoleHelper.cs
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

using Frost.Lib.CommandLine;
using System;

namespace Frost.Lib
{
    /// <summary>
    /// A wrapper class for console applications that helps handle
    /// printing, exit, and exception handling tasks.
    /// </summary>
    public class ConsoleHelper
    {
        /// <summary>
        /// Prints an entry header using assembly information.
        /// </summary>
        public static void PrintHeader()
        {
            var info = new AssemblyInfo();

            Console.WriteLine("{0} {1} ({2}) : {3} : {4}",
                info.Product,
                info.Version,
                Environment.Is64BitProcess ? "x64" : "x86",
                info.Copyright,
                info.BuildDate.ToString("yyyy/MM/dd")
            );
            Console.WriteLine();
        }

        /// <summary>
        /// Prints <see cref="OptionCollection"/> command line usage.
        /// </summary>
        /// <param name="options">The command line options.</param>
        public static void PrintUsage(IOptions options)
        {
            if (options == null)
                return;

            Console.WriteLine(options.Usage);
        }

        /// <summary>
        /// Handles printing exceptions and application exit with
        /// a specified <see cref="IOptions"/> object.
        /// </summary>
        /// <param name="options">The command line options.</param>
        /// <param name="e">The Exception object.</param>
        public static void HandleException(IOptions options, Exception e)
        {
            if (e is OptionException)
            {
                OptionException oe = e as OptionException;
                QuitUsage(options, $"{oe.Message}: {oe.Option}");
            }

            if (e is InitException)
            {
                InitException ie = e as InitException;
                QuitUsage(options, ie.Message);
            }

            HandleException(e);
        }

        /// <summary>
        /// Handles printing exceptions and application exit.
        /// </summary>
        /// <param name="e">The Exception object.</param>
        public static void HandleException(Exception e)
        {
            QuitError(e);
        }

        /// <summary>
        /// Prints command line options usage and exits the application.
        /// </summary>
        /// <param name="options">The command line options.</param>
        public static void QuitUsage(IOptions options)
        {
            PrintUsage(options);
            Quit(0);
        }

        /// <summary>
        /// Prints command line options usage with a message and exits the application.
        /// </summary>
        /// <param name="options">The command line options.</param>
        /// <param name="message">The message body.</param>
        public static void QuitUsage(IOptions options, string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();

            QuitUsage(options);
        }

        /// <summary>
        /// Print error message and quit the application.
        /// </summary>
        /// <param name="message">The message body.</param>
        public static void QuitError(string message)
        {
            Console.WriteLine("An unhandled exception has occurred:");
            Console.WriteLine(message);
            Console.WriteLine();

            Quit(1);
        }

        /// <summary>
        /// Print exception information and quit the application.
        /// </summary>
        /// <param name="e">The exception object.</param>
        public static void QuitError(Exception ex)
        {
            QuitError(ex.ToString());
        }

        /// <summary>
        /// Quit the application.
        /// </summary>
        /// <param name="code">The exit code.</param>
        /// <returns>The exit code.</returns>
        public static void Quit(int code)
        {
            Quit(code, code > 0);
        }

        /// <summary>
        /// Quit the application.
        /// </summary>
        /// <param name="code">The exit code.</param>
        /// <param name="wait">Wait for user response.</param>
        /// <returns>The exit code.</returns>
        public static void Quit(int code, bool wait)
        {
            if (wait)
            {
                Console.WriteLine();
                Console.Write("Press any key to exit . . .");
                Console.ReadKey();
            }

            Environment.Exit(code);
        }
    }
}
