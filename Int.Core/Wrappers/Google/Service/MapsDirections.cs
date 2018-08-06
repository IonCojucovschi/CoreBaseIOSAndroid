// MapsDirections.cs
// Author:
//       Songurov <f.songurov@software-dep.net>
// Copyright (c) 2016 Songurov Fiodor
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Int.Core.IO;
using Int.Core.Network;

namespace Int.Core.Wrappers.Google.Service
{
    public class CoordinateMap
    {
        public IList<string> Distance { get; set; }
        public IList<string> Time { get; set; }
        public IList<IEnumerable<CoordinateEntity>> Waypoints { get; set; }
        public IEnumerable<CoordinateEntity> Data { get; set; }
        public IList<int> WaypointOrder { get; set; }
    }

    public class MapsDirections : MapsBase
    {
        protected override void ConfigService()
        {
            TypeMap = "directions";
            TypeFormatResponse = "json";
            TypeMapView = "origin";
        }

        public MapsDirections SetMyCoordonate(double lat, double lng)
        {
            MyCoordonate = new[] {lat, lng};
            return this;
        }

        public MapsDirections SetMyCoordonate(double[] myCoordonte)
        {
            MyCoordonate = myCoordonte;
            return this;
        }

        public MapsDirections SetDestination(double? lat, double? lng)
        {
            DestinationCoordonate = new[] {lat, lng};
            return this;
        }

        public MapsDirections SetDestination(double?[] destinationCoordonate)
        {
            DestinationCoordonate = destinationCoordonate;
            return this;
        }

        public MapsDirections SetAlternativeRoute(bool routeAlternative)
        {
            Alternatives = routeAlternative;
            return this;
        }

        public MapsDirections SetOptimizationRoute(bool routeOptimization)
        {
            Optimization = routeOptimization;
            return this;
        }

        public MapsDirections SetWaypoints(IList<double[]> additionalDestination)
        {
            Waypoints = additionalDestination;
            return this;
        }

        public MapsDirections SetWaypoints(IList<CoordinateEntity> additionalDestination)
        {
            Waypoints = additionalDestination.Select(item => new[] {item.Latitude, item.Longitude}).ToArray();
            return this;
        }

        protected override string CreateStringUrl()
        {
            var baseUrl = string.Empty;

            var myCoordonate = MyCoordonate;

            if (myCoordonate != null && myCoordonate[0] != 0 && myCoordonate[1] != 0)
            {
                baseUrl = TypeMap + "/" + TypeFormatResponse + "?" + TypeMapView + "=" +
                          MyCoordonate[0].ToString(CultureInfo.InvariantCulture).Replace(',', '.') +
                          "," + MyCoordonate[1].ToString(CultureInfo.InvariantCulture).Replace(',', '.') +
                          "&destination=" + DestinationCoordonate.ElementAtOrDefault(0).ToString().Replace(',', '.') +
                          "," +
                          DestinationCoordonate.ElementAtOrDefault(1).ToString().Replace(',', '.');
            }
            else
            {
                if (Waypoints.Count != 0)
                    baseUrl = TypeMap + "/" + TypeFormatResponse + "?" + TypeMapView + "=" +
                              Waypoints.ElementAtOrDefault(0)?[0].ToString(CultureInfo.InvariantCulture)
                                  .Replace(',', '.') +
                              "," +
                              Waypoints.ElementAtOrDefault(0)?[1].ToString(CultureInfo.InvariantCulture)
                                  .Replace(',', '.') +
                              "&destination=" +
                              DestinationCoordonate.ElementAtOrDefault(0).ToString().Replace(',', '.') +
                              "," +
                              DestinationCoordonate.ElementAtOrDefault(1).ToString().Replace(',', '.');
            }

            var builder = new StringBuilder();

            builder.Append(baseUrl);

            if (Waypoints == null || Waypoints.Count <= 0)
                return builder.ToString();
            builder.Append("&waypoints=");

            if (Optimization)
                builder.Append("optimize:true");

            foreach (var item in Waypoints)
                builder.Append("|" + item[0].ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "," +
                               item[1].ToString(CultureInfo.InvariantCulture).Replace(',', '.'));

            return builder.ToString();
        }

        public async Task<string> GetCoordonateGoogleAsync()
        {
            var result = await new NetworkClient().Settings(this).ExecuteAsync();

            if (result.ResponseHttp == null || result.ResponseString == null)
                return null;

            return result.ResponseString;
        }
    }
}