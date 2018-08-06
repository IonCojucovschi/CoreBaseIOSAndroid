//
// LocationDelegate.cs
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
using Int.iOS.Helpers;
using MapKit;
using UIKit;

namespace Int.iOS.Device.Location
{
    public class LocationDelegate : MKMapViewDelegate
    {
        private readonly UIColor _colorStroke;
        private readonly IList<IMKAnnotation> _items = new List<IMKAnnotation>();
        private readonly float _lineWidth;

        public LocationDelegate(UIColor colorStorke, float line)
        {
            _colorStroke = colorStorke;
            _lineWidth = line;
        }


        public override MKOverlayView GetViewForOverlay(MKMapView mapView, IMKOverlay overlay)
        {
            if (overlay.GetType() != typeof(MKPolyline))
                return null;

            var pLineView = new MKPolylineView(overlay as MKPolyline)
            {
                LineWidth = _lineWidth,
                StrokeColor = _colorStroke
            };

            return pLineView;
        }

        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            if (annotation == null)
                return null;


            var detailButton = UIButton.FromType(UIButtonType.DetailDisclosure);

            detailButton.TouchUpInside += (s, e) =>
            {
                var a = ((AnnotationOverride) annotation).Coordinate;

                if (a.Latitude.Equals(Location.MyCoordonate[0]) && a.Longitude.Equals(Location.MyCoordonate[1]))
                    return;

                LocationHelper.GoogleMap(Location.MyCoordonate, new[] {a.Latitude, a.Longitude});
            };

            var pnn = new MKAnnotationView(annotation, "icon")
            {
                RightCalloutAccessoryView = detailButton,
                CanShowCallout = true,
                Image = (annotation as AnnotationOverride)?.ImageAnnotation
            };

            _items.Add(annotation);
            return pnn;
        }
    }
}