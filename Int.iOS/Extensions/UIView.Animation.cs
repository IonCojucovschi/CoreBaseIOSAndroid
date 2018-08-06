//
// UIView.Animation.cs
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
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Int.iOS.Extensions
{
    public enum AnimationType
    {
        In,
        Out
    }

    public enum FlipType
    {
        Horizontal,
        Vertical
    }

    public enum ShakeType
    {
        Horizontal,
        Vertical
    }

    public enum SlideDirection
    {
        FromRight,
        FromLeft,
        FromTop,
        FromBottom,
        ToRight,
        ToLeft,
        ToTop,
        ToBottom
    }

    public enum SpinDirection
    {
        Clockwise,
        CounterClockwise
    }

    public static partial class Extensions
    {
        private const float MinAlpha = 0.0f;
        private const float MaxAlpha = 1.0f;

        public static void AnimateScale(this UIView view, AnimationType type, double duration = 0.3,
            Action<bool> onCompletion = null)
        {
            var minTransform = CGAffineTransform.MakeScale(0.1f, 0.1f);
            var maxTransform = CGAffineTransform.MakeRotation(0f);

            switch (type)
            {
                case AnimationType.In:
                    view.Alpha = MinAlpha;
                    view.Transform = minTransform;
                    break;
                case AnimationType.Out:
                    view.Alpha = MaxAlpha;
                    view.Transform = maxTransform;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            AnimateNotifyInternal(duration, () =>
            {
                switch (type)
                {
                    case AnimationType.In:
                        view.Alpha = MaxAlpha;
                        view.Transform = maxTransform;
                        break;
                    case AnimationType.Out:
                        view.Alpha = MinAlpha;
                        view.Transform = minTransform;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }, onCompletion);
        }

        public static void AnimationShake(this UIView view, ShakeType type = ShakeType.Horizontal, int count = 2,
            double duration = 0.2f, float withTranslation = -5f)
        {
            var animation = new CABasicAnimation
            {
                KeyPath = $"transform.translation.{(type == ShakeType.Horizontal ? "x" : "y")}",
                TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.Linear),
                RepeatCount = count,
                Duration = duration / count,
                AutoReverses = true,
                By = new NSNumber(withTranslation)
            };

            view.Layer.AddAnimation(animation, "shake");
        }

        public static void AnimationFade(this UIView view, AnimationType type,
            double duration = 0.3,
            Action<bool> onCompletion = null)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                view.Alpha = type == AnimationType.In ? MinAlpha : MaxAlpha;
                view.Transform = CGAffineTransform.MakeIdentity();

                AnimateNotifyInternal(duration,
                    () => { view.Alpha = type == AnimationType.In ? MaxAlpha : MinAlpha; }, onCompletion);
            });
        }

        public static void AnimationFlip(this UIView view,
            AnimationType type, FlipType flip,
            double duration = 0.3,
            Action<bool> onCompletion = null)
        {
            var m34 = (nfloat) (-1 * 0.001);

            view.Alpha = 1.0f;

            var minTransform = CATransform3D.Identity;

            minTransform.m34 = m34;


            var maxTransform = CATransform3D.Identity;
            maxTransform.m34 = m34;

            nfloat x = flip == FlipType.Horizontal ? 1.0f : 0f;
            nfloat y = flip == FlipType.Vertical ? 1.0f : 0f;

            switch (type)
            {
                case AnimationType.In:
                    minTransform = minTransform.Rotate((float) (1 * Math.PI * 0.5),
                        x, y, 0.0f);
                    view.Alpha = MinAlpha;
                    view.Layer.Transform = minTransform;
                    break;
                case AnimationType.Out:
                    minTransform = minTransform.Rotate((float) (-1 * Math.PI * 0.5),
                        x, y, 0.0f);
                    view.Alpha = MaxAlpha;
                    view.Layer.Transform = maxTransform;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            AnimateNotifyInternal(duration, () =>
            {
                view.Layer.AnchorPoint = new CGPoint(0.5f, 0.5f);

                switch (type)
                {
                    case AnimationType.In:
                        view.Alpha = MaxAlpha;
                        view.Layer.Transform = maxTransform;
                        break;
                    case AnimationType.Out:
                        view.Alpha = MinAlpha;
                        view.Layer.Transform = minTransform;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }, onCompletion);
        }

        public static void AnimationLeft(this UIView view,
            AnimationType type,
            float xStrat = 25f,
            double duration = 0.5,
            Action<bool> onCompletion = null)
        {
            UIView.SetAnimationDelegate(view);
            UIView.SetAnimationDuration(duration);

            AnimateNotifyInternal(duration, () =>
            {
                switch (type)
                {
                    case AnimationType.In:
                        view.Alpha = MaxAlpha;
                        UIView.BeginAnimations("moveLeft");
                        view.Frame = new CGRect(xStrat, view.Frame.Y, view.Bounds.Width, view.Bounds.Height);
                        break;
                    case AnimationType.Out:
                        view.Alpha = MinAlpha;
                        UIView.BeginAnimations("moveRight");
                        view.Frame = new CGRect(view.Bounds.Width, view.Frame.Y, view.Bounds.Width, view.Bounds.Height);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                UIView.CommitAnimations();
            }, onCompletion);
        }

        public static void AnimationRotate(this UIView view, AnimationType type, bool fromLeft = true,
            double duration = 0.3, Action<bool> onCompletion = null)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                var minTransform = CGAffineTransform.MakeRotation((fromLeft ? -1f : 1) * 720);
                var maxTransform = CGAffineTransform.MakeRotation(0f);

                switch (type)
                {
                    case AnimationType.In:
                        view.Alpha = MinAlpha;
                        view.Transform = minTransform;
                        break;
                    case AnimationType.Out:
                        view.Alpha = MaxAlpha;
                        view.Transform = maxTransform;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                AnimateNotifyInternal(duration, () =>
                {
                    switch (type)
                    {
                        case AnimationType.In:
                            view.Alpha = MaxAlpha;
                            view.Transform = maxTransform;
                            break;
                        case AnimationType.Out:
                            view.Alpha = MinAlpha;
                            view.Transform = minTransform;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(type), type, null);
                    }
                }, onCompletion);
            });
        }

        public static void AnimationSpin(this UIView view, SpinDirection type,
            double duration = 0.3, Action<bool> onCompletion = null)
        {
            const int iterationCount = 3;
            var iterationDuration = duration / iterationCount;
            var directionSign = type == SpinDirection.Clockwise ? -1.0f : 1.0f;
            var angle = NMath.PI * (2.0f / iterationCount) * directionSign;
            RotateIterationaly(view, iterationCount, angle, iterationDuration, onCompletion);
        }

        private static void RotateIterationaly(this UIView view, int iterationCount, nfloat angle,
            double duration, Action<bool> onCompletion)
        {
            var transform = CGAffineTransform.Rotate(view.Transform, angle);

            UIView.AnimateNotify(duration, 0, UIViewAnimationOptions.CurveLinear,
                () => { view.Transform = transform; },
                finished =>
                {
                    if (!finished) return;
                    if (--iterationCount > 0)
                        RotateIterationaly(view, iterationCount, angle, duration, onCompletion);
                    else
                        onCompletion?.Invoke(finished);
                });
        }

        public static void AnimationSlide(this UIView view, SlideDirection slideDirection, double duration = 0.3,
            Action<bool> onCompletion = null)
        {
            var minTransform = CGAffineTransform.MakeRotation(0f);
            var maxTransform = CGAffineTransform.MakeRotation(0f);

            view.Transform = minTransform;

            AnimateNotifyInternal(duration, () =>
            {
                switch (slideDirection)
                {
                    case SlideDirection.FromBottom:
                        minTransform = CGAffineTransform.MakeTranslation(0f, view.Bounds.Height);
                        maxTransform = CGAffineTransform.MakeIdentity();
                        break;
                    case SlideDirection.FromLeft:
                        minTransform = CGAffineTransform.MakeTranslation(-view.Bounds.Width, 0f);
                        maxTransform = CGAffineTransform.MakeIdentity();
                        break;
                    case SlideDirection.FromRight:
                        minTransform = CGAffineTransform.MakeTranslation(view.Bounds.Width, 0f);
                        maxTransform = CGAffineTransform.MakeIdentity();
                        break;
                    case SlideDirection.FromTop:
                        minTransform = CGAffineTransform.MakeTranslation(0f, -view.Bounds.Height);
                        maxTransform = CGAffineTransform.MakeIdentity();
                        break;
                    case SlideDirection.ToBottom:
                        minTransform = CGAffineTransform.MakeIdentity();
                        maxTransform = CGAffineTransform.MakeTranslation(0f, view.Bounds.Height);
                        break;
                    case SlideDirection.ToLeft:
                        minTransform = CGAffineTransform.MakeIdentity();
                        maxTransform = CGAffineTransform.MakeTranslation(-view.Bounds.Width, 0f);
                        break;
                    case SlideDirection.ToRight:
                        minTransform = CGAffineTransform.MakeIdentity();
                        maxTransform = CGAffineTransform.MakeTranslation(view.Bounds.Width, 0f);
                        break;
                    case SlideDirection.ToTop:
                        minTransform = CGAffineTransform.MakeIdentity();
                        maxTransform = CGAffineTransform.MakeTranslation(0f, -view.Bounds.Height);
                        break;
                    default:
                        minTransform = CGAffineTransform.MakeIdentity();
                        maxTransform = CGAffineTransform.MakeIdentity();
                        break;
                }

                view.Transform = maxTransform;
            }, onCompletion);
        }

        private static void AnimateNotifyInternal(double duration, Action animation, Action<bool> onCompletion)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                UIView.AnimateNotify(duration, 0, UIViewAnimationOptions.CurveEaseInOut, animation,
                    finished => { onCompletion?.Invoke(finished); });
            });
        }
    }
}