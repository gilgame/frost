/* -----------------------------------------------------------------------------
 * AssemblyInfo.cs
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
using System.IO;
using System.Reflection;

namespace Frost.Lib
{
    /// <summary>
    /// A helper class for retrieving assembly info.
    /// </summary>
    public class AssemblyInfo
    {
        /// <summary>
        /// A generic function used to get the value of an assembly attribute.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="predicate">A function to get the value of the attribute.</param>
        /// <param name="defaultValue">A default value.</param>
        /// <returns>Attribute value or default if unassigned.</returns>
        public static string GetAttribute<T>(Func<T, string> predicate, string defaultValue = "")
        {
            Assembly assembly = Assembly.GetEntryAssembly();

            object[] attributes = assembly.GetCustomAttributes(typeof(T), false);
            if (attributes != null && attributes.Length > 0)
            {
                return predicate((T)attributes[0]);
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the Company assembly attribute value.
        /// </summary>
        public string Company =>
            AssemblyInfo.GetAttribute<AssemblyCompanyAttribute>(c => c.Company, "Company");

        /// <summary>
        /// Gets the Product assembly attribute value.
        /// </summary>
        public string Product =>
            AssemblyInfo.GetAttribute<AssemblyProductAttribute>(p => p.Product, "Product");

        /// <summary>
        /// Gets the Version assembly attribute value.
        /// </summary>
        public string Version =>
            AssemblyInfo.GetAttribute<AssemblyVersionAttribute>(v => v.Version, "Version");

        /// <summary>
        /// Gets the Copyright assembly attribute value.
        /// </summary>
        public string Copyright =>
            AssemblyInfo.GetAttribute<AssemblyCopyrightAttribute>(c => c.Copyright, "Copyright");

        /// <summary>
        /// Attempts to get the BuildDate of the assembly.
        /// </summary>
        public DateTime BuildDate =>
            File.GetCreationTime(Assembly.GetExecutingAssembly().Location);
    }
}
