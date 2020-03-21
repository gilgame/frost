/* -----------------------------------------------------------------------------
 * ActionOption.cs
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

namespace Frost.Lib.CommandLine
{
    /// <summary>
    /// Repsents an <see cref="Option"/> that calls an Action with a specified string parameter.
    /// </summary>
    public class ActionOption : Option
    {
        /// <summary>
        /// Gets the option action.
        /// </summary>
        public Action<string> Action { get; }

        /// <summary>
        /// Initializes a new instance of ActionOption and sets its properties.
        /// </summary>
        /// <param name="key">The option key.</param>
        /// <param name="names">String of additional names separated by pipes (|).</param>
        /// <param name="description">The description of the option.</param>
        /// <param name="action">The option action.</param>
        public ActionOption(char key, string names, string description, Action<string> action)
            : base(key, names, description)
        {
            Action = action;
        }

        /// <summary>
        /// Invokes the Action associated with the option.
        /// </summary>
        /// <param name="value">The option value.</param>
        protected override void OnInvoke(string value)
        {
            Action(value);
        }
    }

    /// <summary>
    /// Repsents an <see cref="Option"/> that calls an Action with a specified generic parameter.
    /// </summary>
    public class ActionOption<T> : Option
    {
        /// <summary>
        /// Gets the option action.
        /// </summary>
        public Action<T> Action { get; }

        /// <summary>
        /// Initializes a new instance of ActionOption and sets its properties.
        /// </summary>
        /// <param name="key">The option key.</param>
        /// <param name="names">String of additional names separated by pipes (|).</param>
        /// <param name="description">The description of the option.</param>
        /// <param name="action">The option action.</param>
        public ActionOption(char key, string names, string description, Action<T> action)
            : base(key, names, description)
        {
            Action = action;
        }

        /// <summary>
        /// Invokes the Action associated with the option.
        /// </summary>
        /// <param name="value">The option value.</param>
        protected override void OnInvoke(string value)
        {
            Action(Option.Parse<T>(this, value));
        }
    }
}
