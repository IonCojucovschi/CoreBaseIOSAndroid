//
// LocationGoogleParse.cs
//
// Author:
//       Sogurov Fiodor <f.songurov@software-dep.net>
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

using System.Collections.Generic;
using System.Linq;
using Int.Core.IO;
using Int.Core.Wrappers.Google.Service;

namespace Int.Core.Share.Google.ModelGoogleWaypoint
{
    public static class LocationGoogleParse
    {
        public static CoordinateMap GetCoordonation(GoogleWaypointsModel datas)
        {
            var results = new CoordinateMap {Waypoints = new List<IEnumerable<CoordinateEntity>>()};

            var parseString = datas;

            if (parseString == null) return results;

            foreach (var item in parseString.Routes)
            {
                var data = CoordinateJob.Decode(item.OverviewPolyline?.Points);
                results.Data = data;

                foreach (var waypoint in item.Legs)
                {
                    var polylines = new List<CoordinateEntity>();
                    foreach (var step in waypoint.Steps)
                        polylines.AddRange(CoordinateJob.Decode(step.Polyline.Points));
                    results.Waypoints.Add(polylines);
                }

                var resultDistance = item.Legs.Select(x => x.Distance.Text).ToList();
                var resultDuration = item.Legs.Select(x => x.Duration.Text).ToList();

                results.Time = resultDuration;
                results.Distance = resultDistance;
                results.WaypointOrder = item.WaypointOrder; //new List<int> { 0 , 1 };
            }

            return results;
        }
    }
}