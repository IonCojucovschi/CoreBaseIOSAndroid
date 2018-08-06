//
// FitToParentAnimation.cs
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
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Views.Animations;

namespace Int.Droid.Animations
{
#pragma warning disable XA0001 // Find issues with Android API usage
    public class FitToParentAnimation : Animation
#pragma warning restore XA0001 // Find issues with Android API usage
    {
        private int _childInitialWidth;
        private int _deltaWidth;
        private bool _hasToExpand;
        private int _parentInitialWidth;
        private View _resizableView;
        private View _resizableViewParentView;
        private bool _viewsInitialStatesFixed;

        public FitToParentAnimation(View resizableView)
        {
            InitViews(resizableView);
        }

        public FitToParentAnimation(View resizableView, Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitViews(resizableView);
        }

        private int ChildMeasuredWidth => _resizableView?.MeasuredWidth ?? 0;
        private int ParentMeasuredWidth => _resizableViewParentView?.MeasuredWidth ?? 0;

        public event EventHandler OnPostAnimationExecuted;

        private void InitViews(View childView)
        {
            _resizableView = childView;
            _resizableViewParentView = childView.Parent as View;
        }

        private void FixViewsInitialState()
        {
            _childInitialWidth = ChildMeasuredWidth;
            _parentInitialWidth = ParentMeasuredWidth;
            if (_parentInitialWidth == 0)
                throw new Exception("ParentView was not found.");
            _deltaWidth = Math.Abs(_parentInitialWidth - _childInitialWidth);
            _hasToExpand = _parentInitialWidth > _childInitialWidth;
            _viewsInitialStatesFixed = true;
        }


#pragma warning disable XA0001 // Find issues with Android API usage
        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
#pragma warning restore XA0001 // Find issues with Android API usage
        {
            if (!_viewsInitialStatesFixed)
                FixViewsInitialState();
            t.TransformationType = TransformationTypes.Matrix;
            if (_hasToExpand)
                ExpandView(interpolatedTime);
            else
                CollapseView(interpolatedTime);
            _resizableView.RequestLayout();
            if (!HasEnded) return;
            _hasToExpand = !_hasToExpand;
            OnPostAnimationExecuted?.Invoke(_resizableView, null);
        }

        private void ExpandView(float interpolatedTime)
        {
            var viewGroup = _resizableView as ViewGroup;
            if (viewGroup != null)
                viewGroup.LayoutParameters.Width = _childInitialWidth + (int) (_deltaWidth * interpolatedTime);
        }

        private void CollapseView(float interpolatedTime)
        {
            var viewGroup = _resizableView as ViewGroup;
            if (viewGroup != null)
                viewGroup.LayoutParameters.Width = _parentInitialWidth - (int) (_deltaWidth * interpolatedTime);
        }

        public override bool WillChangeBounds()
        {
            return true;
        }
    }
}