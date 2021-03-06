﻿/* -----------------------------------------------------------------------------
 * Extensions.cs
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

namespace Frost.Lib
{
    /// <summary>
    /// A class containing string extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Compares the start of a string with a list of chars.
        /// </summary>
        /// <param name="self">This string self.</param>
        /// <param name="chars">List of chars to compare.</param>
        public static bool StartsWith(this string self, params char[] chars)
        {
            if (chars == null)
                return false;

            foreach (var c in chars)
            {
                if (self.StartsWith(c))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Surrounds a string with quotation marks.
        /// </summary>
        /// <param name="self">This string self.</param>
        public static string AddQutoes(this string self)
        {
            return $"\"{self}\"";
        }
    }
}
