//
// Location.cs
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
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Locations;

namespace Int.Droid.Device
{
    public class Location
    {
        public static async Task<double[]> GetCoordonateFromName(string address)
        {
            var location = new Geocoder(Application.Context);

            IList<Address> resultServer = new List<Address>();

            try
            {
                resultServer = await location.GetFromLocationNameAsync(address, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var firstOrDefault = resultServer.FirstOrDefault();
            if (firstOrDefault != null)
                return resultServer.Count > 0
                    ? new[]
                    {
                        firstOrDefault.Latitude,
                        firstOrDefault.Longitude
                    }
                    : default(double[]);

            location.Dispose();
            return default(double[]);
        }

        public static async Task<IList<double[]>> GetCoordonateFromName(IList<string> address)
        {
            var location = new Geocoder(Application.Context);

            IList<double[]> result = new List<double[]>();

            foreach (var item in address)
            {
                IList<Address> resultServer = new List<Address>();

                try
                {
                    resultServer = await location.GetFromLocationNameAsync(item, 1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (resultServer.Count <= 0) continue;
                var firstOrDefault = resultServer.FirstOrDefault();
                if (firstOrDefault != null)
                    result.Add(new[] {firstOrDefault.Latitude, firstOrDefault.Longitude});
            }

            location.Dispose();
            return result;
        }
    }
}