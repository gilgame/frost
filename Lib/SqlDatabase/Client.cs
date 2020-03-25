/* -----------------------------------------------------------------------------
 * Client.cs
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
using System.Data.SqlClient;
using System.Text;

namespace Frost.Lib.SqlDatabase
{
    /// <summary>
    /// A SqlConnection wrapper class for assisting in sql server queries.
    /// </summary>
    public class Client : IDisposable
    {
        private readonly SqlConnection _Connection;

        /// <summary>
        /// Gets or sets the string used to open the database connection.
        /// </summary>
        public string ConnectionString => _Connection.ConnectionString;

        /// <summary>
        /// Gets or sets the SqlCredential object for this connection.
        /// </summary>
        public SqlCredential Credential => _Connection.Credential;

        /// <summary>
        /// Indicates the mode recent SqlConnection state.
        /// </summary>
        public ConnectionState State => _Connection.State;

        /// <summary>
        /// A collection of parameters to be used with this SqlConnection.
        /// </summary>
        public ParameterCollection Parameters { get; set; } = new ParameterCollection();

        /// <summary>
        /// Occurs when the component is disposed.
        /// </summary>
        public event EventHandler Disposed
        {
            add
            {
                _Connection.Disposed += value;
            }
            remove
            {
                _Connection.Disposed -= value;
            }
        }

        /// <summary>
        /// Occurs when the sql server returns a message.
        /// </summary>
        public event SqlInfoMessageEventHandler InfoMessage
        {
            add
            {
                _Connection.InfoMessage += value;
            }
            remove
            {
                _Connection.InfoMessage -= value;
            }
        }

        /// <summary>
        /// Occurs when the state of the connection changes.
        /// </summary>
        public event StateChangeEventHandler StateChange
        {
            add
            {
                _Connection.StateChange += value;
            }
            remove
            {
                _Connection.StateChange -= value;
            }
        }

        public Client()
        {
            _Connection = new SqlConnection();
        }

        public Client(string connectionString)
        {
            _Connection = new SqlConnection(connectionString);
        }

        public Client(string connectionString, SqlCredential credential)
        {
            _Connection = new SqlConnection(connectionString, credential);
        }

        /// <summary>
        /// Released all resources used by the connection.
        /// </summary>
        public void Dispose()
        {
            _Connection.Dispose();
        }

        /// <summary>
        /// Starts a database transaction.
        /// </summary>
        public SqlTransaction BeginTransaction()
        {
            return _Connection.BeginTransaction();
        }

        /// <summary>
        /// Starts a database transaction with a specified name.
        /// </summary>
        public SqlTransaction BeginTransaction(string name)
        {
            return _Connection.BeginTransaction(name);
        }

        /// <summary>
        /// Adds a parameter to the <see cref="ParameterCollection"/> associated with the connection.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="type">The SqlDbType type.</param>
        public SqlParameter AddParameter(string name, SqlDbType type)
        {
            var param = new SqlParameter(name, type);

            Parameters.Add(param).Direction = ParameterDirection.Input;

            return param;
        }

        /// <summary>
        /// Adds a parameter to the <see cref="ParameterCollection"/> associated with the connection.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="type">The SqlDbType type.</param>
        /// <param name="value">The parameter value.</param>
        public SqlParameter AddParameter(string name, SqlDbType type, object value)
        {
            var param = new SqlParameter(name, type)
            {
                Value = value
            };

            Parameters.Add(param).Direction = ParameterDirection.Input;

            return param;
        }

        /// <summary>
        /// Adds a parameter to the <see cref="ParameterCollection"/> associated with the connection.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="type">The SqlDbType type.</param>
        /// <param name="size">The parameter size.</param>
        public SqlParameter AddParameter(string name, SqlDbType type, int size)
        {
            var param = new SqlParameter(name, type, size);

            Parameters.Add(param).Direction = ParameterDirection.Input;

            return param;
        }

        /// <summary>
        /// Adds a parameter to the <see cref="ParameterCollection"/> associated with the connection.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="type">The SqlDbType type.</param>
        /// <param name="size">The parameter size.</param>
        /// <param name="value">The parameter value.</param>
        public SqlParameter AddParameter(string name, SqlDbType type, int size, object value)
        {
            var param = new SqlParameter(name, type, size)
            {
                Value = value
            };

            Parameters.Add(param).Direction = ParameterDirection.Input;

            return param;
        }

        /// <summary>
        /// Executes the query and returns the number of rows affected.
        /// </summary>
        /// <param name="command">The command string.</param>
        /// <param name="commandType">The command type.</param>
        public int ExecuteNonQuery(string command, CommandType commandType)
        {
            var rows = 0;

            Post(() =>
            {
                _Connection.Open();

                using (var sqlCommand = new SqlCommand(command, _Connection))
                {
                    sqlCommand.CommandType = commandType;
                    sqlCommand.Parameters.AddRange(Parameters.ToArray());
                    rows = sqlCommand.ExecuteNonQuery();
                }
            });

            return rows;
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row as a result.
        /// </summary>
        /// <param name="command">The command string.</param>
        /// <param name="commandType">The command type.</param>
        public object ExecuteScalar(string command, CommandType commandType)
        {
            object result = null;

            Post(() =>
            {
                _Connection.Open();

                using (var sqlCommand = new SqlCommand(command, _Connection))
                {
                    sqlCommand.CommandType = commandType;
                    sqlCommand.Parameters.AddRange(Parameters.ToArray());
                    result = sqlCommand.ExecuteScalar();
                }
            });

            return result;
        }

        /// <summary>
        /// Executes the query and returns the result as a DataTable.
        /// </summary>
        /// <param name="command">The command string.</param>
        /// <param name="commandType">The command type.</param>
        public DataTable ExecuteQuery(string command, CommandType commandType)
        {
            var result = new DataTable("Results");

            Post(() =>
            {
                _Connection.Open();

                using (var sqlCommand = new SqlCommand(command, _Connection))
                using (var adapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlCommand.CommandType = commandType;
                    sqlCommand.Parameters.AddRange(Parameters.ToArray());
                    adapter.Fill(result);
                }
            });

            return result;
        }

        /// <summary>
        /// Copies all rows of the supplied DataTable to a destination table.
        /// </summary>
        /// <param name="name">The name of the destination table.</param>
        /// <param name="table">The DataTable to be copied.</param>
        public void BulkCopy(string name, DataTable table)
        {
            Post(() =>
            {
                _Connection.Open();
                using (var bulk = new SqlBulkCopy(_Connection))
                {
                    bulk.DestinationTableName = name;
                    foreach (var column in table.Columns)
                    {
                        bulk.ColumnMappings.Add(column.ToString(), column.ToString());
                    }
                    bulk.WriteToServer(table);
                }
            });
        }

        /// <summary>
        /// Attempts to create a database table based on the supplied
        /// DataColumnCollection if it doesn't already exist.
        /// </summary>
        /// <param name="name">The name of the table.</param>
        /// <param name="columns">The table columns.</param>
        public void CreateTable(string name, DataColumnCollection columns)
        {
            Post(() =>
            {
                var query = MakeTableQuery(name, columns);

                ExecuteNonQuery(query, CommandType.Text);
            });
        }

        /// <summary>
        /// Truncates a destination table if it exists.
        /// </summary>
        /// <param name="name">The name of the table.</param>
        public void TruncateTabe(string name)
        {
            Post(() =>
            {
                var query = MakeTruncateQuery(name);

                ExecuteNonQuery(query, CommandType.Text);
            });
        }

        private void Post(Action action)
        {
            try
            {
                action();
            }
            catch(Exception e)
            {
                throw new SqlException(e.Message);
            }
            finally
            {
                if (_Connection.State != ConnectionState.Closed)
                {
                    _Connection.Close();
                }
            }
        }

        private static string MakeTableQuery(string name, DataColumnCollection columns)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("table name is required", "name");

            if (columns == null || columns.Count < 1)
                throw new ArgumentException("there are no columns to add", "columns");

            var query = new StringBuilder();

            query.AppendLine($"IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE name = '{name}' AND xtype = 'U')");
            query.AppendLine($"CREATE TABLE {name} (");
            foreach (DataColumn c in columns)
            {
                query.AppendLine($"[{c.ColumnName}] VARCHAR(255) NULL,");
            }
            query.AppendLine(")");

            return query.ToString();
        }

        private static string MakeTruncateQuery(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("table name is required", "name");

            var query = new StringBuilder();

            query.AppendLine($"IF EXISTS (SELECT 1 FROM sysobjects WHERE name = '{name}' AND xtype = 'U')");
            query.AppendLine($"TRUNCATE TABLE {name}");

            return query.ToString();
        }
    }
}
