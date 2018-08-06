//
// CoordonateRef.cs
//
// Author:
//       Songurov <f.songurov@software-dep.net>
//
// Copyright (c) 2016 Songurov Fiodor
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
using System.Text;

namespace Int.Core.IO
{
    public struct CoordinateEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public static class CoordinateJob
    {
        public static string Encode(IEnumerable<CoordinateEntity> points)
        {
            var str = new StringBuilder();

            var encodeDiff = (Action<int>) (diff =>
            {
                var shifted = diff << 1;
                if (diff < 0)
                    shifted = ~shifted;

                var rem = shifted;

                while (rem >= 0x20)
                {
                    str.Append((char) ((0x20 | (rem & 0x1f)) + 63));

                    rem >>= 5;
                }

                str.Append((char) (rem + 63));
            });

            var lastLat = 0;
            var lastLng = 0;

            foreach (var point in points)
            {
                var lat = (int) Math.Round(point.Latitude * 1E5);
                var lng = (int) Math.Round(point.Longitude * 1E5);

                encodeDiff(lat - lastLat);
                encodeDiff(lng - lastLng);

                lastLat = lat;
                lastLng = lng;
            }

            return str.ToString();
        }

        public static IEnumerable<CoordinateEntity> Decode(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                throw new ArgumentNullException(nameof(encodedPoints));

            var polylineChars = encodedPoints.ToCharArray();
            var index = 0;

            var currentLat = 0;
            var currentLng = 0;

            while (index < polylineChars.Length)
            {
                var sum = 0;
                var shifter = 0;
                int next5Bits;
                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                } while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

                sum = 0;
                shifter = 0;
                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                } while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5Bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

                yield return new CoordinateEntity
                {
                    Latitude = Convert.ToDouble(currentLat) / 1E5,
                    Longitude = Convert.ToDouble(currentLng) / 1E5
                };
            }
        }
    }
}