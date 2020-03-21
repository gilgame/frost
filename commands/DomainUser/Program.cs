/* -----------------------------------------------------------------------------
 * Program.cs
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

using Frost.Lib;
using Frost.Lib.Parsers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Frost.Commands.DomainUser
{
    /// <summary>
    /// A program that calls runas with prompts and shortcuts.
    /// </summary>
    public class Program : ConsoleHelper
    {
        private class Csv : ICsvSettings
        {
            public int Delimiter => 44;
            public bool FirstRowHeader => false;
            public bool SkipEmptyRows => true;
            public bool TrimFields => true;
        }

        private const string P_SHORTCUTS = "RunasShortcuts.json";

        private static Options _Options = new Options();

#if DEBUG

        private static string[] GetArgs()
        {
            return new string[]
            {
                "-h",
            };
        }

#endif

        /// <summary>
        /// The main entry point.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            PrintHeader();

#if DEBUG
            args = GetArgs();
#endif

            Init(args);

            //create shortcuts file and exit
            if (_Options.CreateShortcuts)
            {
                Console.WriteLine("Creating shortcuts file");
                _Options.MakeShortcuts(P_SHORTCUTS);

                Quit(0);
            }

            var command = BuildCommand();

            Console.WriteLine($"Attempting to run: runas {command}");

            var result = 1;
            try
            {
                var process = Process.Start("runas", command);
                process.WaitForExit();
                result = process.ExitCode;
            }
            catch (Exception ex)
            {
                QuitError(ex);
            }

            Quit(result);
        }

        private static void Init(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            _Options.Parse(args);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is ShortcutException)
            {
                var shortcut = e.ExceptionObject as ShortcutException;

                QuitUsage(_Options, shortcut.Message);
            }

            HandleException(_Options, e.ExceptionObject as Exception);
        }

        private static string BuildCommand()
        {
            var command = new StringBuilder();

            if (_Options.NetOnly)
            {
                command.Append("/netonly ");
            }

            var user = String.IsNullOrEmpty(_Options.Username)
                ? PromptUser("Enter a user name:")
                : _Options.Username;

            command.Append($"/user:{user} ");

            var path = String.IsNullOrEmpty(_Options.CommandPath)
                ? GetShortcut()
                : _Options.CommandPath;

            command.Append($"{path.AddQutoes()}");

            if (!String.IsNullOrEmpty(_Options.CustomFlags))
            {
                command.Append($"{_Options.CustomFlags} ");
            }

            return command.ToString().Trim();
        }

        private static string GetShortcut()
        {
            var key = _Options.Arguments.Count < 1
                ? PromptUser("Enter a shortcut:")
                : _Options.Arguments[0];

            _Options.LoadShortcuts(P_SHORTCUTS);

            var pair = _Options.Shortcuts.FirstOrDefault(k => k.Key.ToLower() == key.ToLower());
            if (pair.Value == null)
            {
                throw new ShortcutException("shortcut not found");
            }

            return pair.Value;
        }

        private static string PromptUser(string message)
        {
            Console.WriteLine(message);

            var shortcut = new StringBuilder();

            ConsoleKeyInfo info;
            while (((info = Console.ReadKey()).Key) != ConsoleKey.Enter)
            {
                if (info.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    throw new InitException("user cancelled");
                }

                shortcut.Append(info.KeyChar);
            }
            if (String.IsNullOrEmpty(shortcut.ToString()))
            {
                return PromptUser(message);
            }

            Console.WriteLine();
            Console.WriteLine();

            return shortcut.ToString();
        }
    }
}
