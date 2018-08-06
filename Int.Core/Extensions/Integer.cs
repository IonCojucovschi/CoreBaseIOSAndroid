//
// Integer.cs
//
// Author:
//       Songurov <f.songurov@software-dep.net>
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

namespace Int.Core.Extensions
{
    /// <summary>
    ///     Extensions.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        ///     Tos the string.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="nr">Nr.</param>
        /// <param name="paddingZeroes">Padding zeroes.</param>
        public static string ToString(this int nr, int paddingZeroes)
        {
            return nr.ToString("D" + paddingZeroes);
        }

        public static DateTime UnixTimeStampToDateTime(this int unixTimeStamp)
        {
            if (unixTimeStamp <= 0) return DateTime.MinValue;

            var unixStart = new DateTime(1970, 1, 1);
            var unixTimeStampInTicks = unixTimeStamp * TimeSpan.TicksPerSecond;
            var date = new DateTime(unixStart.Ticks + unixTimeStampInTicks).ToLocalTime();

            return date;
        }

        public static DateTime UnixTimeStampToDateTime(this double unixTimeStamp)
        {
            if (unixTimeStamp <= 0) return DateTime.MinValue;

            var unixStart = new DateTime(1970, 1, 1);
            var unixTimeStampInTicks = (long) (unixTimeStamp * TimeSpan.TicksPerSecond);
            var date = new DateTime(unixStart.Ticks + unixTimeStampInTicks).ToLocalTime();

            return date;
        }


        public static DateTime UnixTimeStampToDateTimeStatic(this int unixTimeStamp)
        {
            if (unixTimeStamp <= 0) return DateTime.MinValue;

            var unixStart = new DateTime(1970, 1, 1);
            var unixTimeStampInTicks = unixTimeStamp * TimeSpan.TicksPerSecond;
            var date = new DateTime(unixStart.Ticks + unixTimeStampInTicks);

            return date;
        }

        public static DateTime UnixTimeStampToDateTimeStatic(this double unixTimeStamp)
        {
            if (unixTimeStamp <= 0) return DateTime.MinValue;

            var unixStart = new DateTime(1970, 1, 1);
            var unixTimeStampInTicks = (long) (unixTimeStamp * TimeSpan.TicksPerSecond);
            var date = new DateTime(unixStart.Ticks + unixTimeStampInTicks);

            return date;
        }
    }
}