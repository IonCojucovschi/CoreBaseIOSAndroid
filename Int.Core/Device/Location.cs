//
// Location.cs
//
// Author:
//       Songurov Fiodor <songurov@gmail.com>
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
using System.Diagnostics;
using System.Threading.Tasks;
using Int.Core.Network;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace Int.Core.Device
{
    public class Location : ApiBase<Location>
    {
        public double[] MyCoodonate { get; set; }

        public async Task<double[]> GetCurrentPositionAsync()
        {
            Position position = null;

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    MyCoodonate = new[]
                    {
                        position.Latitude, position.Longitude
                    };

                    return MyCoodonate;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                    return new[] { 0.0, 0.0 };

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

                MyCoodonate = new[]
                {
                    position.Latitude, position.Longitude
                };

                return MyCoodonate;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);

                return new[] { 0.0, 0.0 };
            }
        }
    }
}