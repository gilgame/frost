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
using System.Collections.Generic;

namespace Frost.Commands
{
    public class Config : IOptions
    {
        /// <summary>
        /// Represents a string that includes 7z flags to ignore Visual Studio files.
        /// </summary>
        public const string VS_FLAGS = "-xr!bin -xr!obj -xr!.vs -xr!.git -x!*.rsuser -x!*.suo -x!*.user -x!*.userosscache -x!*.sln.docstates -x!*.userprefs -x!*.log";

        private OptionCollection _Options;

        /// <inheritdoc/>
        public string Usage => _Options?.ToString();

        /// <inheritdoc/>
        public List<string> Arguments => _Options?.Arguments;

        /// <inheritdoc/>
        public bool ShowHelp { get; private set; }

        /// <summary>
        /// Gets the specified 7z command.
        /// </summary>
        public string Command { get; private set; } = "a";

        /// <summary>
        /// Gets custom flags for 7z specified by the user.
        /// </summary>
        public string CustomFlags { get; private set; }

        /// <summary>
        /// Indicates whether the program should include flags that ignore Visual Studio files.
        /// </summary>
        public bool VsFlags { get; private set; }

        /// <summary>
        /// Indicates whether the file name should include a time stamp.
        /// </summary>
        public bool Timestamp { get; private set; }

        /// <summary>
        /// Indicates whether 7z should recursively archive a directory.
        /// </summary>
        public bool Recursive { get; private set; }

        /// <summary>
        /// Indicates a custom output archive file name.
        /// </summary>
        public string OutputName { get; private set; }

        /// <summary>
        /// Indicates a custom path to the 7z executable.
        /// </summary>
        public string CustomPath { get; private set; }

        /// <inheritdoc/>
        public void Parse(string[] args)
        {
            _Options = new OptionCollection
            {
                {'c', "command", "specifcy a custom 7z command, default: add", v => Command = v},
                {'f', "flags", "specify custom flags, overrides all other flags", v => CustomFlags = v},
                {'v', "vsflags", "use visual studio file ext and folder name exceptions", v => VsFlags = true},
                {'t', "timestamp", "add timestamp to 7z file name", v => Timestamp = true },
                {'r', "recursive", "recurse subdirectories", v => Recursive = true},
                {'o', "output", "specify a custom file name for output", v => OutputName = v},
                {'p', "path", "specify a custom path to the 7z executable", v => CustomPath = v},
                {'h', "help", "show this message and exit", v => ShowHelp = true },
            }
            .SetUsage("Usage: backup [options] ([directory] | [file1 file2 ...])")
            .Parse(args);
        }
    }
}
