/* -----------------------------------------------------------------------------
 * IOptions.cs
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

using System.Collections.Generic;

namespace Frost.Lib.CommandLine
{
    /// <summary>
    /// Defines methods and properties for an <see cref="OptionCollection"/> wrapper class.
    /// </summary>
    public interface IOptions
    {
        /// <summary>
        /// Gets a list of unnamed arguments passed in the command line.
        /// </summary>
        public List<string> Arguments { get; }

        /// <summary>
        /// Gets a string definition of the <see cref="OptionCollection"/> usage.
        /// </summary>
        public string Usage { get; }

        /// <summary>
        /// Indicates whether the application should display the <see cref="OptionCollection"/> usage.
        /// </summary>
        public bool ShowHelp { get; }

        /// <summary>
        /// Creates a new instance of <see cref="OptionCollection"/> and parses command line arguments.
        /// </summary>
        /// <param name="args">Collection of command line arguments.</param>
        public void Parse(string[] args);
    }
}
