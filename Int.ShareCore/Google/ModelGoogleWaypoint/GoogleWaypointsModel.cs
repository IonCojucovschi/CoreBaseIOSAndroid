// GoogleWaypointsModel.cs
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

namespace Int.Core.Share.Google.ModelGoogleWaypoint
{
    public class GoogleWaypointsModel
    {
        public IList<GeocodedWaypoint> GeocodedWaypoints { get; set; } = new List<GeocodedWaypoint>();
        public IList<Route> Routes { get; set; } = new List<Route>();
        public string Status { get; set; }
    }

    public class GeocodedWaypoint
    {
        public string GeocoderStatus { get; set; }
        public bool PartialMatch { get; set; }
        public string PlaceId { get; set; }
        public IList<string> Types { get; set; } = new List<string>();
    }

    public class Northeast
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Southwest
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Bounds
    {
        public Northeast Northeast { get; set; }
        public Southwest Southwest { get; set; }
    }

    public class Distance
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class Duration
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class EndLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class StartLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Polyline
    {
        public string Points { get; set; }
    }

    public class Leg
    {
        public Distance Distance { get; set; }
        public Duration Duration { get; set; }
        public string EndAddress { get; set; }
        public EndLocation EndLocation { get; set; }
        public string StartAddress { get; set; }
        public StartLocation StartLocation { get; set; }
        public IList<Step> Steps { get; set; } = new List<Step>();
        public IList<object> TrafficSpeedEntry { get; set; } = new List<object>();
        public IList<object> ViaWaypoint { get; set; } = new List<object>();
    }

    public class Step
    {
        public Distance Distance { get; set; }
        public Duration Duration { get; set; }
        public EndLocation EndLocation { get; set; }
        public string HtmlInstructions { get; set; }
        public Polyline Polyline { get; set; }
        public StartLocation StartLocation { get; set; }
        public string TravelMode { get; set; }
        public string Maneuver { get; set; }
    }

    public class OverviewPolyline
    {
        public string Points { get; set; }
    }

    public class Route
    {
        public Bounds Bounds { get; set; }
        public string Copyrights { get; set; }
        public IList<Leg> Legs { get; set; } = new List<Leg>();

        public OverviewPolyline OverviewPolyline { get; set; }

        public string Summary { get; set; }
        public IList<object> Warnings { get; set; } = new List<object>();

        public IList<int> WaypointOrder { get; set; } = new List<int>();
    }
}