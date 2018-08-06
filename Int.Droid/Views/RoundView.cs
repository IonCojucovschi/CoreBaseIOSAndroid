//
// RoundView.cs
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
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Int.Droid.Views
{
    public class RoundView : Drawable
    {
        private readonly BitmapShader _bmpShader;
        private readonly RectF _oval = new RectF();
        private readonly Paint _paint = new Paint(PaintFlags.AntiAlias);
        private readonly Paint _paintStroke = new Paint(PaintFlags.AntiAlias);
        private readonly Rect _rect = new Rect();
        private readonly RectF _rectF = new RectF();

        public RoundView()
        {
            _paint.SetStyle(Paint.Style.Fill);
            _paintStroke.SetStyle(Paint.Style.Stroke);
        }

        public RoundView(Bitmap bmp) : this()
        {
            _bmpShader = new BitmapShader(bmp, Shader.TileMode.Clamp, Shader.TileMode.Clamp);
            _paint.SetShader(_bmpShader);
            _oval = new RectF();
        }

        public override int Opacity => 1;
        public float Radius { get; set; } = 0;

        public float LineWidth
        {
            get => _paintStroke.StrokeWidth;
            set => _paintStroke.StrokeWidth = value;
        }

        public Paint.Style Style
        {
            get => _paint.GetStyle();
            set => _paint.SetStyle(value);
        }

        public Color Color
        {
            get => _paint.Color;
            set => _paint.Color = value;
        }

        public Color BorderColor
        {
            get => _paintStroke.Color;
            set => _paintStroke.Color = value;
        }

        public override void Draw(Canvas canvas)
        {
            var halfLineWidth = (int)Math.Ceiling(LineWidth / 2);
            _rect.Set(halfLineWidth, halfLineWidth, Bounds.Width() - halfLineWidth, Bounds.Height() - halfLineWidth);
            _rectF.Set(_rect);
            canvas.DrawRoundRect(_rectF, Radius, Radius, _paint);
            if (LineWidth > 0)
                canvas.DrawRoundRect(_rectF, Radius, Radius, _paintStroke);
        }

        protected override void OnBoundsChange(Rect bounds)
        {
            base.OnBoundsChange(bounds);
            _oval.Set(0, 0, bounds.Width(), bounds.Height());
        }

        public override void SetAlpha(int alpha)
        {
        }

        public override void SetColorFilter(ColorFilter colorFilter)
        {
        }
    }
}