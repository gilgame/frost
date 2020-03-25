/* -----------------------------------------------------------------------------
 * ParameterCollection.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Frost.Lib.SqlDatabase
{
    /// <summary>
    /// Represents a collection of <see cref="Parameter"/> elements.
    /// </summary>
    public sealed class ParameterCollection : IEnumerable
    {
        private readonly List<SqlParameter> _Parameters = new List<SqlParameter>();

        /// <summary>
        /// Gets the <see cref="Parameter" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero based index of the parameter.</param>
        public SqlParameter this[int index]
        {
            get
            {
                return GetParameter(index);
            }
            set
            {
                var p = GetParameter(index);
                p.Value = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Parameter" /> with the specified name.
        /// </summary>
        /// <param name="index">The name of the parameter.</param>
        public SqlParameter this[string name]
        {
            get
            {
                return GetParameter(name);
            }
            set
            {
                var p = GetParameter(name);
                p.Value = value;
            }
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _Parameters.Count;
            }
        }

        /// <summary>
        /// Initializes a new instance of ParameterCollection.
        /// </summary>
        internal ParameterCollection()
        {
        }

        private SqlParameter GetParameter(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException("The specified index was out of range.");

            return this[index];
        }

        private SqlParameter GetParameter(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            var param = _Parameters.Find(i => i.ParameterName == name);
            if (param == null)
                throw new IndexOutOfRangeException("The specified name is not valid.");

            return param;
        }

        /// <summary>
        /// Copies the <see cref="Parameter" /> elements to an array.
        /// </summary>
        /// <returns></returns>
        public SqlParameter[] ToArray()
        {
            return _Parameters.ToArray();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return _Parameters.GetEnumerator();
        }

        public SqlParameter Add(SqlParameter item)
        {
            Validate(item);
            _Parameters.Add(item);
            return item;
        }

        /// <summary>
        /// Adds a <see cref="Parameter"/> to the collection.
        /// </summary>
        /// <param name="parameter">The parameter object.</param>
        public ParameterCollection Add(Parameter parameter)
        {
            return Add(parameter);
        }

        /// <summary>
        /// Adds a <see cref="Parameter"/> to the collection.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The parameter System.Data.SqlDbType.</param>
        /// <param name="value"></param>
        public ParameterCollection Add(string name, System.Data.SqlDbType type, object value)
        {
            return Add(name, type, System.Data.ParameterDirection.Input, value);
        }

        /// <summary>
        /// Adds a <see cref="Parameter"/> to the collection.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The parameter System.Data.SqlDbType.</param>
        /// <param name="size">The maximum size of the data within the column.</param>
        /// <param name="value">The value of the parameter.</param>
        public ParameterCollection Add(string name, System.Data.SqlDbType type, int size, object value)
        {
            return Add(name, type, size, System.Data.ParameterDirection.Input, value);
        }

        /// <summary>
        /// Adds a <see cref="Parameter"/> to the collection.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The parameter System.Data.SqlDbType.</param>
        /// <param name="direction">The direction of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public ParameterCollection Add(string name, System.Data.SqlDbType type, System.Data.ParameterDirection direction, object value)
        {
            return Add(name, type, -1, direction, value);
        }

        /// <summary>
        /// Adds a <see cref="Parameter"/> to the collection.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The parameter System.Data.SqlDbType.</param>
        /// <param name="typename">The type name for a table valued parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public ParameterCollection Add(string name, System.Data.SqlDbType type, string typename, object value)
        {
            return Add(name, type, -1, typename, System.Data.ParameterDirection.Input, value);
        }

        /// <summary>
        /// Adds a <see cref="Parameter"/> to the collection.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The parameter System.Data.SqlDbType.</param>
        /// <param name="size">The maximum size of the data within the column.</param>
        /// <param name="direction">The direction of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public ParameterCollection Add(string name, System.Data.SqlDbType type, int size, System.Data.ParameterDirection direction, object value)
        {
            return Add(name, type, size, null, direction, value);
        }

        /// <summary>
        /// Adds a <see cref="Parameter"/> to the collection.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The parameter System.Data.SqlDbType.</param>
        /// <param name="size">The maximum size of the data within the column.</param>
        /// <param name="typename">The type name for a table valued parameter.</param>
        /// <param name="direction">The direction of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public ParameterCollection Add(string name, System.Data.SqlDbType type, int size, string typename, System.Data.ParameterDirection direction, object value)
        {
            return Add(new Parameter(name, type, size, typename, direction, value));
        }

        /// <summary>
        /// Adda an array of <see cref="Parameter"/> to the end of the collection.
        /// </summary>
        /// <param name="items">The parameter elements.</param>
        public void AddRange(SqlParameter[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
            {
                Validate(item);

                _Parameters.Add(item);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="Parameter"/> is in the collection
        /// </summary>
        /// <param name="item">The parameter element.</param>
        public bool Contains(SqlParameter item)
        {
            return _Parameters.Contains(item);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Parameter"/> name is in the collection.
        /// </summary>
        /// <param name="item">The parameter name.</param>
        public bool Contains(string name)
        {
            if (name == null)
                return false;

            return _Parameters.Find(i => i.ParameterName == name) != null;
        }

        /// <summary>
        /// Removes the specified <see cref="Parameter"/> from the collection.
        /// </summary>
        /// <param name="item">The parameter element.</param>
        public void Remove(SqlParameter item)
        {
            _Parameters.Remove(item);
        }

        /// <summary>
        /// Removes the specified <see cref="Parameter"/> from the collection.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public void Remove(string name)
        {
            if (name != null)
            {
                var param = GetParameter(name);
                _Parameters.Remove(param);
            }
        }

        /// <summary>
        /// Removes all the <see cref="Parameter"/> elements from the collection.
        /// </summary>
        public void Clear()
        {
            _Parameters.Clear();
        }

        /// <summary>
        /// Gets the location of the specified <see cref="Parameter"/>.
        /// </summary>
        /// <param name="item">The parameter element.</param>
        public int IndexOf(SqlParameter item)
        {
            if (item != null)
            {
                for (int i = 0; i < _Parameters.Count; i++)
                {
                    if (item == _Parameters[i])
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the location of the specified <see cref="Parameter"/>
        /// element with the specified name.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public int IndexOf(string name)
        {
            if (name != null)
            {
                for (int i = 0; i < _Parameters.Count; i++)
                {
                    if (name == _Parameters[i].ParameterName)
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Sets this to the specified <see cref="Client"/> parameter collection.
        /// </summary>
        /// <param name="client">The client object.</param>
        public Client Set(Client client)
        {
            client.Parameters = this;

            return client;
        }

        private void Validate(SqlParameter item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (IndexOf(item) >= 0)
                throw new ArgumentException("This SqlParameter has already been added to the collection.");

            if (item.ParameterName == String.Empty)
                item.ParameterName = String.Format("Parameter{0}", _Parameters.Count);
        }
    }
}
