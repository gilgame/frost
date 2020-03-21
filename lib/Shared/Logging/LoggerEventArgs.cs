/* -----------------------------------------------------------------------------
 * LoggerEventArgs.cs
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

namespace Frost.Lib.Logging
{
    /// <summary>
    /// Delegate for logger events.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event args object.</param>
    public delegate void LoggerEventHandler(object sender, LoggerEventArgs e);

    /// <summary>
    /// The event arguments that are passed when a logger event occurs.
    /// </summary>
    public class LoggerEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of LoggerEventArgs with a message.
        /// </summary>
        /// <param name="message">The message body.</param>
        public LoggerEventArgs(string message)
        {
            Message = message;
        }
    }
}
