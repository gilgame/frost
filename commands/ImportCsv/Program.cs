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
using Frost.Lib.Logging;
using Frost.Lib.Parsers;
using Frost.Lib.SqlDatabase;
using System;
using System.IO;

namespace Frost.ImportCsv
{
    public class Program : ConsoleHelper
    {
        private static readonly Config _Config = new Config();

#if DEBUG
        private static string[] GetArgs()
        {
            var args = new string[]
            {
                @"-c=""Server=FME-DEV-SQL-01.FMYN.COM\\SQL2014; Initial Catalog=StrattonWarren; Integrated Security=true""",
                @"-n",
                @"-m",
                @"-e",
                @"-l=44",
                @"-v",
                @"C:\Test\importcsv\test_import.csv",
            };
            return args;
        }
#endif

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

            ImportCsv();

            Quit(0);
        }

        private static void Init(string[] args)
        {
            var info = new AssemblyInfo();

            Logger.Source = info.Company;
            Logger.Filename = Path.ChangeExtension(info.Product, "log");
            Logger.Stopped += OnLoggerStopped;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            _Config.Parse(args);
        }

        private static void OnLoggerStopped(object sender, LoggerEventArgs e)
        {
            Console.WriteLine("Logger object has encountered an exception and stopped.");
            Console.WriteLine(e.Message);
            Console.WriteLine();
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(_Config, e.ExceptionObject as Exception);
        }

        private static void ImportCsv()
        {
            var parser = new CsvParser(_Config.CsvSettings);

            using (var client = new Client(_Config.ConnectionString))
            {
                var transaction = client.BeginTransaction("bulk");

                try
                {
                    foreach (string file in _Config.Arguments)
                    {
                        var data = parser.Parse(file);
                        var schema = String.IsNullOrEmpty(_Config.Schema) ? "dbo" : _Config.Schema;
                        var name = $"{schema}.[{Path.GetFileNameWithoutExtension(file)}]";

                        client.CreateTable(name, data.Columns);
                        client.BulkCopy(name, data);
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (InvalidOperationException ix)
                    {
                        Console.WriteLine(ix.Message);
                    }

                    HandleException(ex);
                }
            }
        }
    }
}
