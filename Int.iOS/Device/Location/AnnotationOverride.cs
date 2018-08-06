//
// AnnotationOverride.cs
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

using CoreLocation;
using MapKit;
using UIKit;

namespace Int.iOS.Device.Location
{
    public class AnnotationOverride : MKAnnotation
    {
        public AnnotationOverride(CLLocationCoordinate2D coordonate, string title, string subtitle, string description)
        {
            Title = title;
            Subtitle = subtitle;
            Description = description;
            Coordinate = coordonate;
        }

        public AnnotationOverride(UIImage image, CLLocationCoordinate2D coordonate, string title, string subtitle,
            string description) : this(coordonate, title, subtitle, description)
        {
            Title = title;
            ImageAnnotation = image;
            Subtitle = subtitle;
            Description = description;
            Coordinate = coordonate;
        }

        #region implemented abstract members of MKAnnotation

        public override CLLocationCoordinate2D Coordinate { get; }

        #endregion

        public override string Title { get; }

        public override string Subtitle { get; }

        public override string Description { get; }

        public UIImage ImageAnnotation { get; set; }
    }
}