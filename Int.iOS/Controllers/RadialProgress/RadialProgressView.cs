//
// RadialProgressView.cs
//
// Author:
//       Songurov Fiodor <songurov@gmail.com>
//
// Copyright (c) 2017 Songurov Fiodor
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
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Int.iOS.Controllers.RadialProgress
{
    [Register("RadialProgressView")]
    public class RadialProgressView : UIView
    {
        private const float FULL_CIRCLE = 2 * (float)Math.PI;
        private readonly UIColor _backColor = UIColor.LightGray;

        private readonly nfloat _degrees = 0.0f;
        private readonly UIColor _frontColor = UIColor.Green;
        private readonly int _lineWidth = 10;
        private int _radius = 10;

        public RadialProgressView(CGRect frame, int lineWidth, nfloat degrees)
        {
            _lineWidth = lineWidth;
            _degrees = degrees;
            Frame = new CGRect(frame.X, frame.Y, frame.Width, frame.Height);
            BackgroundColor = UIColor.Clear;

            Initialize();
        }

        public RadialProgressView(CGRect frame, int lineWidth, UIColor backColor, UIColor frontColor)
        {
            _lineWidth = lineWidth;
            Frame = new CGRect(frame.X, frame.Y, frame.Width, frame.Height);
            BackgroundColor = UIColor.Clear;

            Initialize();
        }

        public UIColor DigitColor { get; set; }

        public UIColor TextColor { get; set; }

        public int TextSize { get; set; }

        public string Text { get; set; }

        public int Progress { get; set; }


        private void Initialize()
        {
            var circlePath = new UIBezierPath();

            // Circle layer
            var circleLayer = new CAShapeLayer
            {
                Path = circlePath.CGPath,
                FillColor = UIColor.Clear.CGColor,
                StrokeColor = UIColor.Green.CGColor,
                StrokeEnd = 94 / 100,
                LineWidth = _lineWidth,
                ZPosition = 1
            };
            var circleBackgroundLayer = new CAShapeLayer
            {
                Path = circlePath.CGPath,
                FillColor = UIColor.Clear.CGColor,
                StrokeColor = UIColor.LightGray.CGColor,
                StrokeEnd = 1.0f,
                LineWidth = _lineWidth,
                ZPosition = -1
            };
            Layer.AddSublayer(circleLayer);
            Layer.AddSublayer(circleBackgroundLayer);
        }

        public void AnimateView()
        {
        }

        public void DrawGraph(CGContext g, nfloat x0, nfloat y0)
        {
            g.SetLineWidth(_lineWidth);

            // Draw background circle
            var path = new CGPath();
            _backColor.SetStroke();
            path.AddArc(x0, y0, _radius, 0, 2.0f * (float)Math.PI, true);
            g.AddPath(path);
            g.DrawPath(CGPathDrawingMode.Stroke);

            // Draw overlay circle
            var pathStatus = new CGPath();
            _frontColor.SetStroke();
            pathStatus.AddArc(x0, y0, _radius, 0, _degrees * (float)Math.PI, false);
            g.AddPath(pathStatus);
            g.DrawPath(CGPathDrawingMode.Stroke);
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            using (var g = UIGraphics.GetCurrentContext())
            {
                _radius = (int)(Bounds.Width / 2) - 8;
                DrawGraph(g, Bounds.GetMidX(), Bounds.GetMidY());
            }
            ;

            BackgroundColor = UIColor.Clear;
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            return base.SizeThatFits(size);
        }
    }
}