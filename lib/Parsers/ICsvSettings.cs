/* -----------------------------------------------------------------------------
 * ICsvSettings.cs
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

namespace Frost.Lib.Parsers
{
    /// <summary>
    /// Defines a configuration for an instance of <see cref="CsvParser"/>.
    /// </summary>
    public interface ICsvSettings
    {
        /// <summary>
        /// Gets or sets an integer that represents a single character delimiter.
        /// </summary>
        public int Delimiter { get; }

        /// <summary>
        /// Gets or sets whether the parser should assume the first row contains headers.
        /// </summary>
        public bool FirstRowHeader { get; }

        /// <summary>
        /// Gets or sets whether the parser should ignore empty rows.
        /// </summary>
        public bool SkipEmptyRows { get; }

        /// <summary>
        /// Gets or sets whether the parser should trim fields as it goes.
        /// </summary>
        public bool TrimFields { get; }
    }
}
