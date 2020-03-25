/* -----------------------------------------------------------------------------
 * XmlSerializer.cs
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
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Frost.Lib.Parsers
{
    /// <summary>
    /// A class that provides methods for serializing objects in xml.
    /// </summary>
    public static class XmlSerializer
    {
        /// <summary>
        /// Serializes an object and returns it in an xml representation.
        /// </summary>
        /// <param name="o">The object to be serialized.</param>
        public static string Serialize(object o)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(o.GetType());

            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true
            };

            var result = new StringBuilder();
            using (var writer = XmlWriter.Create(result, settings))
            {
                var namespaces = new XmlSerializerNamespaces
                (
                    new[] { XmlQualifiedName.Empty }
                );

                serializer.Serialize(writer, o, namespaces);
            }
            return result.ToString();
        }

        /// <summary>
        /// Deserializes an xml string and returns a new instance of an object.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="serialized">The xml body.</param>
        public static T Deserialize<T>(string serialized)
        {
            if (String.IsNullOrEmpty(serialized))
            {
                return default(T);
            }

            //remove any headers
            var preamble = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (serialized.StartsWith(preamble))
            {
                serialized = serialized.Remove(0, preamble.Length - 1);
            }

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var reader = new StringReader(serialized))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
