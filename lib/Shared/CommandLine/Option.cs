/* -----------------------------------------------------------------------------
 * Option.cs
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
using System.ComponentModel;

namespace Frost.Lib.CommandLine
{
    /// <summary>
    /// A base class that represents a command line option.
    /// </summary>
    public abstract class Option
    {
        /// <summary>
        /// Gets or sets the option key.
        /// </summary>
        public char Key { get; private set; }

        /// <summary>
        /// Gets a collection of additional option names.
        /// </summary>
        public List<string> Names { get; private set; } = new List<string>();

        /// <summary>
        /// Gets the option description.
        /// </summary>
        public string Description { get; private set; } = String.Empty;

        /// <summary>
        /// Initializes a new instance of Option and sets its properties.
        /// </summary>
        /// <param name="key">The option key.</param>
        /// <param name="names">String of additional names separated by pipes (|).</param>
        /// <param name="description">The description of the option.</param>
        public Option(char key, string names, string description)
        {
            if (key == '\0')
                throw new ArgumentException("Options require a key.");

            Key = key;
            Description = description;

            ParseNames(names);
        }

        private void ParseNames(string names)
        {
            var parts = names.Split('|', StringSplitOptions.RemoveEmptyEntries);

            foreach (var name in parts)
            {
                Names.Add(name);
            }
        }

        /// <summary>
        /// Checks to see if a specified name is associated with the option.
        /// </summary>
        /// <param name="name">The option name.</param>
        public bool HasName(string name)
        {
            foreach (var n in Names)
            {
                if (n == name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Invokes a method associated with the option.
        /// </summary>
        /// <param name="value">The option value.</param>
        public void Invoke(string value)
        {
            OnInvoke(value);
        }

        /// <summary>
        /// Invokes a method assigned by the derived class.
        /// </summary>
        /// <param name="value">The option value.</param>
        protected abstract void OnInvoke(string value);

        /// <summary>
        /// Converts option properties to a readable string.
        /// </summary>
        public override string ToString()
        {
            var flags = GetFlags();

            return $"{flags} : {Description}";
        }

        private string GetFlags()
        {
            if (Names.Count > 0)
            {
                var names = String.Join('|', Names);

                return $"-{Key} --{names}";
            }
            else
            {
                return $"-{Key}";
            }
        }

        /// <summary>
        /// Attempts to convert the option value string to the specified type.
        /// </summary>
        /// <typeparam name="T">The option value type.</typeparam>
        /// <param name="option">The option object.</param>
        /// <param name="value">The option value.</param>
        protected static T Parse<T>(Option option, string value)
        {
            Type type = typeof(T);

            bool nullable = type.IsValueType
                && type.IsGenericType
                && !type.IsGenericTypeDefinition
                && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            Type target = nullable ? type.GetGenericArguments()[0] : type;

            T result = default;

            try
            {
                if (value != null)
                {
                    return (T)TypeDescriptor
                        .GetConverter(target)
                        .ConvertFromString(value);
                }
                else
                {
                    return result;
                }
            }
            catch
            {
                throw new OptionException(
                    option.GetFlags(),
                    $"could not convert string '{value}' to type {target.Name}"
                );
            }
        }
    }
}
