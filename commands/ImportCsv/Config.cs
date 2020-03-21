/* -----------------------------------------------------------------------------
 * Config.cs
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
using System.Collections.Generic;

namespace Frost.ImportCsv
{
    /// <inheritdoc/>
    public class Config : IOptions
    {
        private OptionCollection _Options;

        /// <inheritdoc/>
        public string Usage => _Options?.ToString();

        /// <inheritdoc/>
        public List<string> Arguments => _Options?.Arguments;

        /// <inheritdoc/>
        public bool ShowHelp { get; private set; } = false;

        /// <summary>
        /// Gets the csv settings file path.
        /// </summary>
        public string ConfigFile { get; private set; } = String.Empty;

        /// <summary>
        /// Gets the
        /// </summary>
        public bool GenerateConfig { get; private set; } = false;

        /// <summary>
        /// Gets the string used to open the database connection.
        /// </summary>
        public string ConnectionString { get; private set; } = String.Empty;

        /// <summary>
        /// Gets the schema associated with the database connection, dbo by default.
        /// </summary>
        public string Schema { get; private set; } = String.Empty;

        /// <summary>
        /// Gets the table name is all files are meant to import to the same table.
        /// </summary>
        public string TableName { get; private set; } = String.Empty;

        /// <summary>
        /// Indicates whether target tables should be truncated.
        /// </summary>
        public bool Truncate { get; private set; } = false;

        /// <summary>
        /// Indicates whether the csv path is a directory.
        /// </summary>
        public bool IsDirectory { get; private set; } = false;

        /// <summary>
        /// Indicates whether the command should show
        /// additional information during the import.
        /// </summary>
        public bool Verbose { get; private set; } = false;

        /// <summary>
        /// Indicates if a directory import should be recursive.
        /// </summary>
        public bool Recursive { get; private set; } = false;

        /// <summary>
        /// Indicates whether the command should only test the database connection.
        /// </summary>
        public bool TestConnection { get; private set; } = false;

        /// <summary>
        /// Gets the csv settings file path.
        /// </summary>
        public CsvSettings CsvSettings { get; private set; } = new CsvSettings();

        /// <inheritdoc/>
        public void Parse(string[] args)
        {
            _Options = new OptionCollection
            {
                {'x', "xml", "xml file with config, overrides any command flags", v => ConfigFile = v },
                {'g', "generate", "generate config file from this command", (bool v) => GenerateConfig = v },
                {'c', "connection", "string to be used for sql server connection", v => ConnectionString = v },
                {'s', "schema", "path to table schema file, overrides other sql settings", v => Schema = v },
                {'q', "table", "destination table name, uses file name by default", v => TableName = v },
                {'v', "truncate", "truncate the destination table if it exists", (bool v) => Truncate = v },
                {'n', "hasheader", "first row has column names", (bool v) => CsvSettings.FirstRowHeader = v },
                {'m', "trimfields", "trim fields of whitespace", (bool v) => CsvSettings.TrimFields = v },
                {'e', "skipempty", "skip empty rows", (bool v) => CsvSettings.SkipEmptyRows = v },
                {'l', "delimiter", "field separator by integer value, default comma (44)", (int v) => CsvSettings.Delimiter = v },
                {'d', "directory", "import directory, .csv extension only", (bool v) => IsDirectory = v },
                {'r', "recursive", "import directory and its contents recursively", (bool v) => Recursive = v },
                {'v', "verbose", "display additional information during the import", (bool v) => Verbose = v },
                {'t', "test", "test the connection only", (bool v) => TestConnection = v },
                {'h', "help", "show this message and exit", (bool v) => ShowHelp = v },
            }
            .SetUsage("Usage: importcsv [options] ([directory] | [file1 file2 ...])")
            .Parse(args); ;
        }
    }
}
