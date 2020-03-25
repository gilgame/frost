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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Frost.Commands.Backup
{
    /// <summary>
    /// Represents a program that simplifies calls to 7z.exe with custom parameters.
    /// </summary>
    public class Program : ConsoleHelper
    {
        private static string _ExePath;
        private static readonly Config _Config = new Config();

#if DEBUG
        private static string[] GetArgs() => new string[]
        {
            //@"-v",
            //@"C:\Users\Tim\source\repos\frost",
            @"-h",
        };
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

            if (_Config.ShowHelp)
            {
                QuitUsage(_Config);
            }

            var filename = _ExePath;
            var command = BuildCommand();

            Console.WriteLine($"Attempting to run: {filename} {command}");

            var result = 1;
            try
            {
                var process = Process.Start(filename, command);
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

            _Config.Parse(args);

            if (!_Config.ShowHelp && _Config.Arguments.Count < 1)
            {
                throw new InitException("not enough files were supplied");
            }

            if (String.IsNullOrEmpty(_Config.CustomPath))
            {
                _ExePath = GetExePath();
                if (String.IsNullOrEmpty(_ExePath))
                {
                    throw new InitException("7-zip is not correctly installed on this system");
                }
                else
                {
                    _ExePath = Path.Combine(_ExePath, "7z.exe");
                }
            }
            else
            {
                _ExePath = _Config.CustomPath;
            }
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(_Config, e.ExceptionObject as Exception);
        }

        private static string GetExePath()
        {
            var exe = String.Empty;
            var pathVariable = Environment.GetEnvironmentVariable("Path");

            var paths = pathVariable.Split(';');
            foreach (var path in paths)
            {
                if (path.Contains("7-zip", StringComparison.OrdinalIgnoreCase))
                {
                    exe = path;
                    break;
                }
            }
            return exe;
        }

        private static string BuildCommand()
        {
            List<string> args = new List<string>
            {
                String.Format("{0} {1}", _Config.Command, GetOutput().AddQutoes())
            };

            foreach (var file in _Config.Arguments)
            {
                args.Add(file.AddQutoes());
            }

            if (!String.IsNullOrEmpty(_Config.CustomFlags))
            {
                args.Add(_Config.CustomFlags);

                return String.Join("", args);
            }

            if (_Config.Recursive)
            {
                args.Add("-r");
            }

            if (_Config.VsFlags)
            {
                args.Add(Config.VS_FLAGS);
            }

            if (_Config.GitFlags)
            {
                args.Add(Config.GIT_FLAGS);
            }

            var result = String.Join(" ", args);

            return result;
        }

        private static string GetOutput()
        {
            if (!String.IsNullOrEmpty(_Config.OutputName))
                return _Config.OutputName;

            var path = Path.GetDirectoryName(_Config.Arguments[0]);
            var filename = Path.GetFileNameWithoutExtension(_Config.Arguments[0]);
            var file = Path.Combine(path, filename).TrimEnd('\\');

            if (_Config.Timestamp)
                return String.Format("{0}-{1}.7z", file, DateTime.Now.ToString("yyyyMMddHHmmss"));
            else
                return String.Format("{0}.7z", file);
        }
    }
}
