// MapsBase.cs
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


using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Int.Core.IO;
using Int.Core.Network;

namespace Int.Core.Wrappers.Google
{
    public abstract class MapsBase : INetworkSettings
    {
        protected MapsBase()
        {
            ConfigService();
        }

        protected CoordinateEntity Coordonate { get; set; }

        protected virtual Uri BaseUrl { get; set; } = new Uri("https://maps.googleapis.com/maps/api/");

        protected string TypeMap { get; set; }

        protected string TypeFormatResponse { get; set; }

        protected string TypeMapView { get; set; }

        protected double[] MyCoordonate { get; set; }

        protected double?[] DestinationCoordonate { get; set; }

        protected IList<double[]> Waypoints { get; set; }

        protected bool Alternatives { get; set; }

        protected bool Optimization { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string UrlBase
        {
            get { return BaseUrl.ToString(); }
            set { }
        }

        public string UrlController
        {
            get { return CreateStringUrl(); }
            set { }
        }

        public MediaTypeWithQualityHeaderValue ContentType
        {
            get { return MediaTypeWithQualityHeaderValue.Parse("application/x-www-form-urlencoded"); }
            set { }
        }

        public Dictionary<string, string> HeaderColletion { get; set; }

        protected abstract void ConfigService();

        protected abstract string CreateStringUrl();
    }
}