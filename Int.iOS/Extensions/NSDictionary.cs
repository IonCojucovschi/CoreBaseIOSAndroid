//
// NSDictionary.cs
//
// Author:
//       Songurov Fiodor <songurov@gmail.com>
//
// Copyright (c) 2017 Songurov Fiodor
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
using System.Collections.Generic;
using System.Linq;
using Foundation;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        public static NSDictionary ToNsDictionary<TK, TV>(this IDictionary<TK, TV> dict)
            where TK : NSObject
            where TV : NSObject
        {
            return NSDictionary.FromObjectsAndKeys(dict.Values.ToArray(), dict.Keys.ToArray());
        }

        public static bool LocationInRange(this NSRange range, int location)
        {
            return location >= range.Location && location <= range.Location + range.Length;
        }

        public static bool LocationInRange(this NSRange range, uint location)
        {
            return location >= range.Location && location <= range.Location + range.Length;
        }

        public static nint NSMaxRange(this NSRange range)
        {
            return range.Location + range.Length;
        }

    }
}