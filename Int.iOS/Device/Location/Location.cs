//
// Location.cs
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

using System;
using CoreLocation;
using UIKit;

namespace Int.iOS.Device.Location
{
    public class LocationUpdatedEventArgs : EventArgs
    {
        public LocationUpdatedEventArgs(CLLocation location)
        {
            Location = location;
        }

        public CLLocation Location { get; }
    }

    public enum LisingType
    {
        Foreground,
        Background
    }

    public class Location
    {
        private bool _debugInfo;
        private bool _isDebugMode;
        private bool _isUpdating;
        protected CLLocationManager _locMgr;

        public Location(LisingType lisingType = LisingType.Foreground)
        {
            _locMgr = new CLLocationManager {PausesLocationUpdatesAutomatically = false};


            if (!UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) return;
            switch (lisingType)
            {
                case LisingType.Foreground:
                    _locMgr.RequestWhenInUseAuthorization(); // only in foreground
                    break;
                case LisingType.Background:
                    _locMgr.RequestAlwaysAuthorization();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lisingType), lisingType, null);
            }
        }

        public static double[] MyCoordonate { get; set; }

        public bool DebugInfo
        {
            get => _debugInfo;
            set
            {
                _debugInfo = value;
                switch (_debugInfo)
                {
                    case true:
                        if (_isDebugMode)
                            return;
                        _isDebugMode = true;
                        LocationUpdated += PrintLocation;
                        break;
                    case false:
                        _isDebugMode = false;
                        LocationUpdated -= PrintLocation;
                        break;
                }
            }
        }

        public bool PausesLocationUpdatesAutomatically
        {
            get => _locMgr.PausesLocationUpdatesAutomatically;
            set => _locMgr.PausesLocationUpdatesAutomatically = value;
        }

        public bool AllowsBackgroundLocationUpdates
        {
            get => UIDevice.CurrentDevice.CheckSystemVersion(9, 0) && _locMgr.AllowsBackgroundLocationUpdates;
            set
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                    _locMgr.AllowsBackgroundLocationUpdates = value;
            }
        }

        public CLLocationManager LocMgr => _locMgr;

        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

        public void StartLocationUpdates()
        {
            if (_isUpdating)
                return;

            if (!CLLocationManager.LocationServicesEnabled) return;
            _isUpdating = true;

            LocMgr.DesiredAccuracy = 1;
            LocMgr.LocationsUpdated +=
                (sender, e) => LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
            LocMgr.StartUpdatingLocation();
        }

        public void StopLocationUpdates()
        {
            if (!_isUpdating)
                return;
            _isUpdating = false;
            LocMgr.StopUpdatingLocation();
        }


        private static void PrintLocation(object sender, LocationUpdatedEventArgs e)
        {
            var location = e.Location;
            Console.WriteLine("Altitude: " + location.Altitude + " meters");
            Console.WriteLine("Longitude: " + location.Coordinate.Longitude);
            Console.WriteLine("Latitude: " + location.Coordinate.Latitude);
            Console.WriteLine("Course: " + location.Course);
            Console.WriteLine("Speed: " + location.Speed);
        }
    }
}