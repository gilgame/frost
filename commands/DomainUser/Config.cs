/* -----------------------------------------------------------------------------
 * Options.cs
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
using Frost.Lib.CommandLine;
using Frost.Lib.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Frost.Commands.DomainUser
{
    public class Config : IOptions
    {
        private OptionCollection _Options;

        /// <inheritdoc/>
        public string Usage => _Options?.ToString();

        /// <inheritdoc/>
        public List<string> Arguments => _Options?.Arguments;

        /// <summary>
        /// Gets or sets a list of exe shortcut commands.
        /// </summary>
        public Dictionary<string, string> Shortcuts { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public bool ShowHelp { get; set; }

        /// <summary>
        /// Credentials supplied are for remote access only.
        /// </summary>
        public bool NetOnly { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the exe or command path.
        /// </summary>
        public string CommandPath { get; set; }

        /// <summary>
        /// Gets or sets customs flags to be passed with the
        /// command. Overrides all other flags.
        /// </summary>
        public string CustomFlags { get; set; }

        /// <summary>
        /// Gets or sets whether the program should create a shortcuts file.
        /// </summary>
        public bool CreateShortcuts { get; set; }

        /// <inheritdoc/>
        public void Parse(string[] args)
        {
            _Options = new OptionCollection
            {
                {'n', "netonly", "credeintial are for remote access only", v => NetOnly = true },
                {'u', "user", "user name, including optional domain: domain\\user", v => Username = v },
                {'p', "path", "custom command path, overrides shortcuts", v => CommandPath = v },
                {'f', "flags", "any custom flags to be passed to the command", v => CustomFlags = v },
                {'s', "shortcuts", "create customizable shortcuts file", v => CreateShortcuts = true },
                {'h', "help", "show this message and exit", v => ShowHelp = true },
            }
            .SetUsage("Usage: duser [options] (shortcut)")
            .Parse(args);
        }

        /// <summary>
        /// Attempts to load shortcuts from disk or loads them from resources.
        /// </summary>
        /// <param name="name">The resource file name.</param>
        public void LoadShortcuts(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            var path = Path.Combine(GetRoot(), name);

            if (File.Exists(path))
            {
                LoadFromFile(path);
            }
            else
            {
                LoadFromMemory(name);
            }
        }

        /// <summary>
        /// Attempts to create a shortcuts file.
        /// </summary>
        /// <param name="name">The name of the resource file.</param>
        public void MakeShortcuts(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (File.Exists(name))
                throw new InitException("shortcuts file already exists");

            var contents = GetResourceText(name);

            var path = Path.Combine(GetRoot(), name);

            File.WriteAllText(path, contents);
        }

        private void LoadFromFile(string name)
        {
            var contents = File.ReadAllText(name);

            Shortcuts = Configs.JSonDeserialize<Dictionary<string, string>>(contents);
        }

        private void LoadFromMemory(string name)
        {
            var contents = GetResourceText(name);

            Shortcuts = Configs.JSonDeserialize<Dictionary<string, string>>(contents);
        }

        private string GetResourceText(string name)
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceNames().Single(r => r.EndsWith(name));

            return Configs.GetTextResource(assembly, resource);
        }

        private string GetRoot()
        {
            var path = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase);

            return new FileInfo(path.AbsolutePath).Directory.FullName;
        }
    }
}
