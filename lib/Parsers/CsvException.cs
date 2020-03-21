/* -----------------------------------------------------------------------------
 * CsvException.cs
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

namespace Frost.Lib.Parsers
{
    /// <summary>
    /// Thrown when the csv parser class encounters an error.
    /// </summary>
    public class CsvException : Exception
    {
        /// <summary>
        /// Gets the last row successfully parsed.
        /// </summary>
        public string Row { get; private set; }

        /// <summary>
        /// Initializes a new instance of CsvException with the last
        /// row successfully parsed and a message.
        /// </summary>
        /// <param name="row">The last row successfully parsed.</param>
        /// <param name="message">The message body.</param>
        public CsvException(string row, string message) : base(message)
        {
            Row = row;
        }
    }
}
