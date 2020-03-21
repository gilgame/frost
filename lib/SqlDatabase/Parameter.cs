/* -----------------------------------------------------------------------------
 * Parameter.cs
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
using System.Data;

namespace Frost.Lib.SqlDatabase
{
    /// <summary>
    /// Represents a SqlParameter to be used with <see cref="Client"/>.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the SqlDbType of the parameter.
        /// </summary>
        public SqlDbType SqlDbType { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the data within the column.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the type name for a table valued parameter.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the direction of the parameter.
        /// </summary>
        public ParameterDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Creates a new instance of Parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The SqlDbType of the parameter.</param>
        /// <param name="size">The maximum size of the data within the column.</param>
        /// <param name="typename">The type name for a table valued parameter.</param>
        /// <param name="direction">The direction of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public Parameter(string name, SqlDbType type, int size, string typename, ParameterDirection direction, object value)
        {
            ParameterName = name ?? throw new ArgumentException("name cannot be empty", "name");
            Value = value ?? throw new ArgumentException("value cannot be null", "value");

            SqlDbType = type;
            Size = size;
            TypeName = String.IsNullOrEmpty(typename) ? null : typename;
            Direction = direction;
        }
    }
}
