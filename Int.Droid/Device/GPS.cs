//
// GPS.cs
//
// Author:
//       Songurov Fiodor <f.songurov@software-dep.net>
//
// Copyright (c) 2016 Songurov
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

namespace Int.Droid.Device
{
    public class Gps : DeviceBase
    {
        public static float CheckDistance(double startLatitude, double startLongitude, double endLatitude,
            double endLongitude)
        {
            var distanceResults = new float[1];
            Android.Locations.Location.DistanceBetween(startLatitude, startLongitude, endLatitude,
                endLongitude, distanceResults);

            return distanceResults[0];
        }

        public static double CheckSpeed(double startLatitude, double startLongitude, double endLatitude,
            double endLongitude, IReadOnlyList<long> times)
        {
            var distanceResults = new float[1];
            Android.Locations.Location.DistanceBetween(startLatitude, startLongitude, endLatitude, endLongitude,
                distanceResults);

            var distance = distanceResults[0];
            var time = Math.Abs(times[0] - times[1]) / 1000;

            return distance / time;
        }
    }
}