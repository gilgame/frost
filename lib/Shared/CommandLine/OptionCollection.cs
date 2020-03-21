/* -----------------------------------------------------------------------------
 * OptionCollection.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Frost.Lib.CommandLine
{
    /// <summary>
    /// An <see cref="Option"/> collection that parses command line arguments.
    /// </summary>
    public class OptionCollection : KeyedCollection<string, Option>
    {
        private string _Usage = String.Empty;

        /// <summary>
        /// Gets unnamed parameters that were passed to the option set.
        /// </summary>
        public List<string> Arguments { get; } = new List<string>();

        /// <summary>
        /// Adds a new string instance of <see cref="ActionOption"/> with properties.
        /// </summary>
        /// <param name="key">The option key.</param>
        /// <param name="names">String of additional names separated by pipes (|).</param>
        /// <param name="description">The description of the option.</param>
        /// <param name="action">The option action.</param>
        public OptionCollection Add(char key, string names, string description, Action<string> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            base.Add(new ActionOption(key, names, description, action));

            return this;
        }

        /// <summary>
        /// Adds a new generic instance of <see cref="ActionOption"/> with properties.
        /// </summary>
        /// <param name="key">The option key.</param>
        /// <param name="names">String of additional names separated by pipes (|).</param>
        /// <param name="description">The description of the option.</param>
        /// <param name="action">The option action.</param>
        public OptionCollection Add<T>(char key, string names, string description, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            base.Add(new ActionOption<T>(key, names, description, action));

            return this;
        }

        /// <summary>
        /// Extracts the key for the specified <see cref="Option"/>.
        /// </summary>
        /// <param name="option">The command line option.</param>
        protected override string GetKeyForItem(Option option)
        {
            if (option == null)
                throw new ArgumentNullException("item");

            return option.Key.ToString();
        }

        /// <summary>
        /// Sets a string representation of the command line usage.
        /// </summary>
        /// <param name="usage">The usage string body.</param>
        public OptionCollection SetUsage(string usage)
        {
            _Usage = usage;

            return this;
        }

        /// <summary>
        /// Gets a string representation of the command line usage.
        /// </summary>
        public string GetUsage()
        {
            return _Usage;
        }

        /// <summary>
        /// Parses command line arguments against the option set configuration.
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <returns>Returns a list of unnamed arguments that weren't parsed, if any.</returns>
        public OptionCollection Parse(string[] args)
        {
            if (args == null || args.Length < 1)
                return this;

            foreach (var arg in args)
            {
                //if not starting with a delimiter, unnamed parameter
                if (!arg.StartsWith(new char[] { '-', '/' }))
                {
                    Arguments.Add(arg);
                    continue;
                }

                //flag = - or --
                //name = anything except : or = indicating a value
                //value = the rest of the string
                var regex = new Regex(@"^(?<flag>-|--)(?<name>[^:=\r\n]+)[:=]?(?<value>.*)$");

                Match match = regex.Match(arg);

                if (!match.Success)
                    throw new OptionException(arg, "invalid format");

                var flag = match.Groups["flag"].Value;
                var name = match.Groups["name"].Value;
                var value = match.Groups["value"].Value;

                if (GetOption(flag, name, out Option o))
                {
                    o.Invoke(value);
                    continue;
                }

                throw new OptionException(arg, "unknown option");
            }

            return this;
        }

        private bool GetOption(string flag, string name, out Option option)
        {
            option = null;

            //if short name (key)
            if (flag == "-")
            {
                if (Contains(name))
                {
                    option = this[name];
                    return true;
                }
            }

            //if long name
            if (flag == "--")
            {
                var o = this.FirstOrDefault(o => o.Names.Any(n => n == name));
                if (o != null)
                {
                    option = o;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts the option set to a readable help format, including usage.
        /// </summary>
        public override string ToString()
        {
            StringBuilder help = new StringBuilder();

            help.AppendLine(_Usage);
            help.AppendLine();

            foreach (var option in this)
            {
                help.AppendLine($"\t{option}");
            }

            return help.ToString();
        }
    }
}
