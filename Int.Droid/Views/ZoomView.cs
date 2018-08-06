//
// ZoomView.cs
//
// Author:
//       Songurov Fiodor <f.songurov@software-dep.net>
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
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Int.Droid.Views
{
    public class ScaleImageViewGestureDetector : GestureDetector.SimpleOnGestureListener
    {
        private readonly ScaleImageView m_ScaleImageView;

        public ScaleImageViewGestureDetector(ScaleImageView imageView)
        {
            m_ScaleImageView = imageView;
        }

        public override bool OnDown(MotionEvent e)
        {
            return true;
        }

        public override bool OnDoubleTap(MotionEvent e)
        {
            m_ScaleImageView.MaxZoomTo((int) e.GetX(), (int) e.GetY());
            m_ScaleImageView.Cutting();
            return true;
        }
    }

    public class ScaleImageView : ImageView, View.IOnTouchListener
    {
        private readonly Context m_Context;
        private GestureDetector m_GestureDetector;
        private int m_Height;
        private int m_IntrinsicHeight;
        private int m_IntrinsicWidth;
        private bool m_IsScaling;
        private Matrix m_Matrix;
        private readonly float[] m_MatrixValues = new float[9];
        private readonly float m_MaxScale = 2.0f;
        private float m_MinScale;
        private float m_PreviousDistance;
        private int m_PreviousMoveX;
        private int m_PreviousMoveY;
        private float m_Scale;
        private int m_Width;

        public ScaleImageView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            m_Context = context;
            Initialize();
        }

        public ScaleImageView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            m_Context = context;
            Initialize();
        }

        public float Scale => GetValue(m_Matrix, Matrix.MscaleX);

        public float TranslateX => GetValue(m_Matrix, Matrix.MtransX);

        public float TranslateY => GetValue(m_Matrix, Matrix.MtransY);

        public bool OnTouch(View v, MotionEvent e)
        {
            return OnTouchEvent(e);
        }

        public override void SetImageBitmap(Bitmap bm)
        {
            base.SetImageBitmap(bm);
            Initialize();
        }

        public override void SetImageResource(int resId)
        {
            base.SetImageResource(resId);
            Initialize();
        }

        private void Initialize()
        {
            SetScaleType(ScaleType.Matrix);
            m_Matrix = new Matrix();
            if (Drawable != null)
            {
                m_IntrinsicWidth = Drawable.IntrinsicWidth;
                m_IntrinsicHeight = Drawable.IntrinsicHeight;
                SetOnTouchListener(this);
            }
            m_GestureDetector = new GestureDetector(m_Context, new ScaleImageViewGestureDetector(this));
        }

        protected override bool SetFrame(int l, int t, int r, int b)
        {
            m_Width = r - l;
            m_Height = b - t;
            m_Matrix.Reset();
            var r_norm = r - l;
            m_Scale = r_norm / (float) m_IntrinsicWidth;
            var paddingHeight = 0;
            var paddingWidth = 0;
            if (m_Scale * m_IntrinsicHeight > m_Height)
            {
                m_Scale = m_Height / (float) m_IntrinsicHeight;
                m_Matrix.PostScale(m_Scale, m_Scale);
                paddingWidth = (r - m_Width) / 2;
            }
            else
            {
                m_Matrix.PostScale(m_Scale, m_Scale);
                paddingHeight = (b - m_Height) / 2;
            }
            m_Matrix.PostTranslate(paddingWidth, paddingHeight);
            ImageMatrix = m_Matrix;
            m_MinScale = m_Scale;
            ZoomTo(m_Scale, m_Width / 2, m_Height / 2);
            Cutting();
            return base.SetFrame(l, t, r, b);
        }

        private float GetValue(Matrix matrix, int whichValue)
        {
            matrix.GetValues(m_MatrixValues);
            return m_MatrixValues[whichValue];
        }

        public void MaxZoomTo(int x, int y)
        {
            if (m_MinScale != Scale && Scale - m_MinScale > 0.1f)
            {
                var scale = m_MinScale / Scale;
                ZoomTo(scale, x, y);
            }
            else
            {
                var scale = m_MaxScale / Scale;
                ZoomTo(scale, x, y);
            }
        }

        public void ZoomTo(float scale, int x, int y)
        {
            if (Scale * scale < m_MinScale)
            {
                scale = m_MinScale / Scale;
            }
            else
            {
                if (scale >= 1 && Scale * scale > m_MaxScale)
                    scale = m_MaxScale / Scale;
            }
            m_Matrix.PostScale(scale, scale);
            //move to center
            m_Matrix.PostTranslate(-(m_Width * scale - m_Width) / 2, -(m_Height * scale - m_Height) / 2);
            //move x and y distance
            m_Matrix.PostTranslate(-(x - m_Width / 2) * scale, 0);
            m_Matrix.PostTranslate(0, -(y - m_Height / 2) * scale);
            ImageMatrix = m_Matrix;
        }

        public void Cutting()
        {
            try
            {
                var width = (int) (m_IntrinsicWidth * Scale);
                var height = (int) (m_IntrinsicHeight * Scale);
                if (TranslateX < -(width - m_Width))
                    m_Matrix.PostTranslate(-(TranslateX + width - m_Width), 0);
                if (TranslateX > 0)
                    m_Matrix.PostTranslate(-TranslateX, 0);
                if (TranslateY < -(height - m_Height))
                    m_Matrix.PostTranslate(0, -(TranslateY + height - m_Height));
                if (TranslateY > 0)
                    m_Matrix.PostTranslate(0, -TranslateY);
                if (width < m_Width)
                    m_Matrix.PostTranslate((m_Width - width) / 2, 0);
                if (height < m_Height)
                    m_Matrix.PostTranslate(0, (m_Height - height) / 2);
                ImageMatrix = m_Matrix;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private float Distance(float x0, float x1, float y0, float y1)
        {
            var x = x0 - x1;
            var y = y0 - y1;
            return (float) Math.Sqrt(x * x + y * y);
        }

        private float DispDistance()
        {
            return (float) Math.Sqrt(m_Width * m_Width + m_Height * m_Height);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (m_GestureDetector.OnTouchEvent(e))
            {
                m_PreviousMoveX = (int) e.GetX();
                m_PreviousMoveY = (int) e.GetY();
                return true;
            }
            var touchCount = e.PointerCount;
            switch (e.Action)
            {
                case MotionEventActions.Down:
                case MotionEventActions.Pointer1Down:
                case MotionEventActions.Pointer2Down:
                {
                    if (touchCount >= 2)
                    {
                        var distance = Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                        m_PreviousDistance = distance;
                        m_IsScaling = true;
                    }
                }
                    break;
                case MotionEventActions.Move:
                {
                    if (touchCount >= 2 && m_IsScaling)
                    {
                        var distance = Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                        var scale = (distance - m_PreviousDistance) / DispDistance();
                        m_PreviousDistance = distance;
                        scale += 1;
                        scale = scale * scale;
                        ZoomTo(scale, m_Width / 2, m_Height / 2);
                        Cutting();
                    }
                    else if (!m_IsScaling)
                    {
                        var distanceX = m_PreviousMoveX - (int) e.GetX();
                        var distanceY = m_PreviousMoveY - (int) e.GetY();
                        m_PreviousMoveX = (int) e.GetX();
                        m_PreviousMoveY = (int) e.GetY();
                        m_Matrix.PostTranslate(-distanceX, -distanceY);
                        Cutting();
                    }
                }
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                case MotionEventActions.Pointer2Up:
                {
                    if (touchCount <= 1)
                        m_IsScaling = false;
                }
                    break;
            }
            return true;
        }
    }
}