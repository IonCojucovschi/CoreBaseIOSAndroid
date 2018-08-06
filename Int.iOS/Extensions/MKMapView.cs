//
// MKMapView.cs
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
using System.Collections.Generic;
using System.Linq;
using MapKit;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        public static void CenterMapToAnnotations(this MKMapView map, double latlongPadding)
        {
            if (map?.Annotations == null)
                return;

            IList<MKAnnotation> annotations = map.Annotations.Select(a => a as MKAnnotation).ToList();

            if (annotations.Count <= 1)
                return;

            var firstOrDefault = annotations.OrderByDescending(e => e.Coordinate.Latitude).FirstOrDefault();
            if (firstOrDefault == null) return;
            {
                var maxLat = firstOrDefault.Coordinate.Latitude;
                var mkAnnotation = annotations.OrderByDescending(e => e.Coordinate.Longitude).FirstOrDefault();
                if (mkAnnotation == null) return;
                {
                    var maxLong = mkAnnotation.Coordinate.Longitude;

                    var orDefault = annotations.OrderBy(e => e.Coordinate.Latitude).FirstOrDefault();
                    if (orDefault == null) return;
                    {
                        var minLat = orDefault.Coordinate.Latitude;
                        var annotation = annotations.OrderBy(e => e.Coordinate.Longitude).FirstOrDefault();
                        if (annotation == null) return;
                        var minLong = annotation.Coordinate.Longitude;
                        var region = new MKCoordinateRegion
                        {
                            Center =
                            {
                                Latitude = (maxLat + minLat) / 2,
                                Longitude = (maxLong + minLong) / 2
                            },
                            Span =
                            {
                                LatitudeDelta = maxLat - minLat + latlongPadding,
                                LongitudeDelta = maxLong - minLong + latlongPadding
                            }
                        };


                        map.Region = region;
                    }
                }
            }
        }

        public static int ZoomLevel(this MKMapView mapView)
        {
            var zoomScale = mapView.VisibleMapRect.Size.Width / mapView.Bounds.Size.Width;
            var zoomExponent = Math.Log(zoomScale);
            var zoomLevel = (int) (20 - Math.Ceiling(zoomExponent));

            return zoomLevel;
        }
    }
}