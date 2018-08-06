//
// MapHelper.cs
//
// Author:
//       Songurov <songurov@gmail.com>
//
// Copyright (c) 2018 Songurov
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
namespace Int.Core.Helpers
{
    public class GeoPosition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class MapHelper
    {
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            var theta = lon1 - lon2;
            var dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2Deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }

            if (double.IsNaN(dist) == true)
            {
                dist = 0;
            }

            return (dist);
        }

        private static double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        private static readonly Random _Random = new Random();

        public static GeoPosition GenerateRandomLocation(double y0, double x0, int radius)
        {
            // Convert radius from meters to degrees
            double radiusInDegrees = radius / 111000f;

            var u = _Random.NextDouble();
            var v = _Random.NextDouble();
            var w = radiusInDegrees * Math.Sqrt(u);
            var t = 2 * Math.PI * v;
            var x = w * Math.Cos(t);
            var y = w * Math.Sin(t);

            // Adjust the x-coordinate for the shrinking of the east-west distances
            var new_x = x / Math.Cos(y0);

            var foundLongitude = new_x + x0;
            var foundLatitude = y + y0;

            return new GeoPosition
            {
                Latitude = foundLatitude,
                Longitude = foundLongitude,
            };
        }
    }
}
