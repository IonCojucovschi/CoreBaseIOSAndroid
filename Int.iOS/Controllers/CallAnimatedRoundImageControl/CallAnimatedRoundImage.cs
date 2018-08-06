//
// CallAnimatedRoundImage.cs
//
// Author:
//       arslan <chameleon256@gmail.com>
//
// Copyright (c) 2017 (c) ARSLAN ATAEV
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
using CoreGraphics;
using Int.iOS.Views;
using UIKit;

namespace Int.iOS.Controllers.CallAnimatedRoundImageControl
{
    public class CallAnimatedRoundImage : UIView
    {
        private readonly UIView[] _animatableCircles;
        private bool _requestedToStopAnimation;
        private readonly Settings _settings;

        public CallAnimatedRoundImage(Settings settings)
        {
            _settings = settings;
            _animatableCircles = new UIView[_settings.CirclesCount];
            InitView();
        }

        /// <summary>
        ///     Starts the animation.
        /// </summary>
        /// <param name="times">Number of repetitions. -1 is infinite. </param>
        public void PlayAnimation(double fullCycleDuration, int times = -1)
        {
            if (!_requestedToStopAnimation)
                if (times == -1)
                    AnimateOneCycle(fullCycleDuration, () => PlayAnimation(fullCycleDuration, times));
                else if (times > 0)
                    AnimateOneCycle(fullCycleDuration, () => PlayAnimation(fullCycleDuration, --times));

            _requestedToStopAnimation = false;
        }

        public void StopAnimation()
        {
            _requestedToStopAnimation = true;
        }

        private void InitView()
        {
            for (var i = 0; i < _settings.CirclesCount; i++)
                AddCircle(i);
            AddImageBackground();
            AddImage();
        }

        private void AddCircle(int indexOutsideIn)
        {
            var view = new UIRoundView();
            var size = GetCircleSize(indexOutsideIn);
            var location = GetCircleLocation(size);
            view.Frame = new CGRect(location, size);
            var circleColor = _settings.ImageBackgroundColor.ColorWithAlpha(_settings.OuterCircleAlpha);
            view.BackgroundColor = circleColor;
            view.Alpha = 0.0f;
            _animatableCircles[indexOutsideIn] = view;
            AddSubview(view);
        }

        private void AddImageBackground()
        {
            var view = new UIRoundView();
            InitViewFrame(view, _settings.ImageBackgroundDiamterAspect);
            view.BackgroundColor = _settings.ImageBackgroundColor;
            AddSubview(view);
        }

        private void AddImage()
        {
            var view = new UIRoundImageView();
            InitViewFrame(view, _settings.ImageDiameterAspect);
            if (_settings.Image != null)
                view.Image = _settings.Image;
            _settings.ImageAssignedEventHandler = assignedImage =>
            {
                if (assignedImage != null)
                    view.Image = assignedImage;
            };
            AddSubview(view);
        }

        private void InitViewFrame(UIView view, nfloat aspect)
        {
            var diameter = ConvertAspectToDiameter(aspect);
            var size = new CGSize(diameter, diameter);
            var location = GetCircleLocation(size);
            view.Frame = new CGRect(location, size);
        }

        private CGSize GetCircleSize(int indexOutsideIn)
        {
            var diameter = _settings.ViewDiameter -
                           indexOutsideIn * (GetFreeSpaceDeltaDiameter() / _settings.CirclesCount);
            return new CGSize(diameter, diameter);
        }

        private CGPoint GetCircleLocation(CGSize size)
        {
            var axisPosition = (_settings.ViewDiameter - size.Width) / 2;
            return new CGPoint(axisPosition, axisPosition);
        }

        private nfloat GetFreeSpaceDeltaDiameter()
        {
            nfloat contentDiamter = 0;

            if (_settings.ImageBackgroundDiamterAspect >= _settings.ImageDiameterAspect)
                contentDiamter = ConvertAspectToDiameter(_settings.ImageBackgroundDiamterAspect);
            else
                contentDiamter = ConvertAspectToDiameter(_settings.ImageDiameterAspect);
            ;

            return _settings.ViewDiameter - contentDiamter;
        }

        private nfloat ConvertAspectToDiameter(nfloat aspect)
        {
            return _settings.ViewDiameter * aspect;
        }

        private void AnimateOneCycle(double fullCycleDuration, Action onAnimationEnd)
        {
            var subCycleDuration = fullCycleDuration / _settings.CirclesCount;
            AnimateSubCycle(_settings.CirclesCount - 1, subCycleDuration, onAnimationEnd);
        }

        private void AnimateSubCycle(int cycleIndex, double duration, Action onAnimationEnd)
        {
            AnimateNotify(duration / 2,
                () => _animatableCircles[cycleIndex].Alpha = 1.0f,
                finished =>
                {
                    if (cycleIndex - 1 >= 0)
                        AnimateSubCycle(cycleIndex - 1, duration, () =>
                        {
                            AnimateNotify(duration / 2,
                                () => _animatableCircles[cycleIndex].Alpha = 0.0f,
                                outFinished => onAnimationEnd?.Invoke());
                        });
                    else
                        AnimateNotify(duration / 2,
                            () => _animatableCircles[cycleIndex].Alpha = 0.0f,
                            outFinished => onAnimationEnd?.Invoke());
                });
        }

        public class Settings
        {
            private UIImage _image;

            /// <summary>
            ///     No need to assign. Event used by <see cref="CallAnimatedRoundImage" />.
            /// </summary>
            public Action<UIImage> ImageAssignedEventHandler;

            public nfloat ViewDiameter { get; set; }

            /// <summary>
            ///     Gets or sets the image diameter aspect.
            /// </summary>
            /// <value>
            ///     The image diameter aspect from 1.0 to 0.0 where 1.0 completely fills controller view and 0.0 makes image
            ///     invisible.
            /// </value>
            public nfloat ImageDiameterAspect { get; set; }

            public UIImage Image
            {
                get => _image;
                set
                {
                    _image = value;
                    ImageAssignedEventHandler?.Invoke(_image);
                }
            }

            /// <summary>
            ///     Gets or sets the image background diamter aspect.
            /// </summary>
            /// <value>
            ///     The image background diamter aspect from 1.0 to 0.0 where 1.0 completely fills controller view and 0.0 makes
            ///     background invisible.
            /// </value>
            public nfloat ImageBackgroundDiamterAspect { get; set; }

            /// <summary>
            ///     Gets or sets the color of the image background. Also sets color for animated circles.
            /// </summary>
            /// <value>The color of the image background.</value>
            public UIColor ImageBackgroundColor { get; set; }

            /// <summary>
            ///     Gets or sets the animated circles count. Background is not in count.
            /// </summary>
            /// <value>The circles count.</value>
            public int CirclesCount { get; set; }

            public nfloat OuterCircleAlpha { get; set; }
        }
    }
}