//
// Byte.cs
//
// Author:
//      Songurov <f.songurov@software-dep.net>
//
// Copyright (c) 2016 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Text;
using System.Linq;

namespace Int.Core.Extensions
{
    /// <summary>
    ///     Extensions.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        ///     Hexs the string.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="arr">Arr.</param>
        public static string HexString(this byte[] arr)
        {
            return BitConverter.ToString(arr).Replace("-", "");
        }

        public static string ToStr(this byte[] bytes)
        {
            var str = new StringBuilder();

            if (bytes != null)
                bytes.Select(x => str.AppendFormat("{0:x2} ", x));
            return str.ToString();
        }
    }
}