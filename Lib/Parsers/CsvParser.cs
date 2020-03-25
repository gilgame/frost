/* -----------------------------------------------------------------------------
 * CsvParser.cs
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
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Frost.Lib.Parsers
{
    /// <summary>
    /// A parser class for csv files.
    /// </summary>
    public class CsvParser
    {
        private ICsvSettings _Settings;

        /// <summary>
        /// Initializes a new instance of CsvParser with settings.
        /// </summary>
        /// <param name="settings">The csv parser settings.</param>
        public CsvParser(ICsvSettings settings)
        {
            _Settings = settings ?? throw new ArgumentNullException("settings");
        }

        /// <summary>
        /// Parses a csv file located on disk and converts it to a DataTable.
        /// </summary>
        /// <param name="file">The path to the file.</param>
        public DataTable Parse(string file)
        {
            var result = new DataTable("Results");
            var lines = File.ReadAllLines(file);
            var regex = GetRegex();

            for (int i = 0; i < lines.Length; i++)
            {
                if (String.IsNullOrEmpty(lines[i].Trim()))
                {
                    if (_Settings.SkipEmptyRows)
                        continue;
                }

                var matches = regex.Matches(lines[i]);
                if (matches.Count < 1)
                {
                    throw new CsvException(lines[i], "invalid or unsupported csv format");
                }
                var fields = GetFields(matches);

                //create columns if it hasn't been done yet
                if (result.Columns.Count < 1)
                {
                    if (_Settings.FirstRowHeader)
                    {
                        CreateTable(fields, out result);
                        continue;
                    }
                    else
                    {
                        CreateTable(fields.Count, out result);
                    }
                }

                result.Rows.Add(fields.ToArray());
            }

            return result;
        }

        private List<string> GetFields(MatchCollection matches)
        {
            var fields = new List<string>();

            //TODO: quotes don't stop the split
            foreach (Match match in matches)
            {
                fields.Add(Trim(match.Value));
            }

            return fields;
        }

        private void CreateTable(List<string> names, out DataTable table)
        {
            if (names == null || names.Count < 1)
                throw new ArgumentException("column count cannot be zero", "names");

            table = new DataTable("Results");
            table.Clear();

            foreach (var name in names)
            {
                table.Columns.Add(Trim(name));
            }
        }

        private void CreateTable(int numColumns, out DataTable table)
        {
            if (numColumns < 1)
                throw new ArgumentException("column count cannot be zero", "numColumns");

            table = new DataTable("Results");
            table.Clear();

            for (int i = 0; i < numColumns; i++)
            {
                table.Columns.Add();
            }
        }

        private string Trim(string field)
        {
            //remove any quotes
            field = field.Trim('"');

            if (_Settings.TrimFields)
                return field.Trim();
            else
                return field;
        }

        private Regex GetRegex()
        {
            var d = (char)_Settings.Delimiter;

            //Author: Iulian Fecioru
            //Source: http://regexlib.com/REDetails.aspx?regexp_id=2308
            //Original: \A[^,"]*(?=,)|(?:[^",]*"[^"]*"[^",]*)+|[^",]*"[^"]*\Z|(?<=,)[^,]*(?=,)|(?<=,)[^,]*\Z|\A[^,]*\Z

            var pattern = $@"\A[^{d}""]*(?={d})|(?:[^""{d}]*""[^""]*""[^""{d}]*)+|[^""{d}]*""[^""]*\Z|(?<={d})[^{d}]*(?={d})|(?<={d})[^{d}]*\Z|\A[^{d}]*\Z";
            
            return new Regex(pattern);
        }
    }
}
