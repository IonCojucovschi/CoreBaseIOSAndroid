//
// UIView.cs
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
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using AVFoundation;
using CoreAnimation;
using CoreGraphics;
using CoreText;
using Foundation;
using Int.Core.Application.Widget.Contract;
using Int.Core.Extensions;
using MediaPlayer;
using UIKit;

namespace Int.iOS.Extensions
{
    public class BorderGrandientConfig
    {
        public enum Orientation
        {
            Vertical,
            Horizontal
        }

        public CGColor StartColor { get; set; }

        public CGColor EndColor { get; set; }

        public CGPoint StartPoint { get; set; }

        public CGPoint EndPoint { get; set; }

        public nfloat BorderWidth { get; set; } = 1f;

        public static BorderGrandientConfig Create(UIColor[] color,
            Orientation orientation)
        {
            var border = new BorderGrandientConfig
            {
                StartColor = color.FirstOrDefault()?.CGColor,
                EndColor = color.LastOrDefault()?.CGColor
            };

            switch (orientation)
            {
                case Orientation.Vertical:
                    border.StartPoint = new CGPoint(0f, 0.0f);
                    border.EndPoint = new CGPoint(1.0f, 1.0f);
                    break;
                case Orientation.Horizontal:
                    border.StartPoint = new CGPoint(0f, 0.5f);
                    border.EndPoint = new CGPoint(1.0f, 0.5f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }
            return border;
        }
    }

    public static partial class Extensions
    {
        public enum ViewState
        {
            Gone,
            Visible,
            Invisible
        }

        public static CGRect RelativeRectToScreen(this UIView view)
        {
            var globalPoint = view.Superview?.ConvertPointToView(view.Frame.Location, null) ??
                              view.ConvertPointToView(view.Frame.Location, null);
            return new CGRect(globalPoint, view.Bounds.Size);
        }

        public static UIGestureRecognizer OnClick(this UIView view, Action action,
            uint tapsRequired = 1)
        {
            if (view.IsNull()) return null;

            UITapGestureRecognizer tap = null;

            AppTools.InvokeOnMainThread(() =>
            {
                view.UserInteractionEnabled = true;
                tap = new UITapGestureRecognizer(action)
                {
                    NumberOfTapsRequired = tapsRequired
                };
                view.AddGestureRecognizer(tap);
            });

            return tap;
        }

        public static UIGestureRecognizer OnLongClick(this UIView view, Action action)
        {
            if (view.IsNull()) return null;

            UILongPressGestureRecognizer longPress = null;

            AppTools.InvokeOnMainThread(() =>
            {
                view.UserInteractionEnabled = true;
                longPress = new UILongPressGestureRecognizer(action);
                view.AddGestureRecognizer(longPress);
            });

            return longPress;
        }

        public static void ClearGestures(this UIView view)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                var gestures = view.GestureRecognizers;
                if (gestures == null) return;
                foreach (var gesture in gestures)
                    view.RemoveGestureRecognizer(gesture);
            });
        }

        public static UIView SetViewState(this UIView view, ViewState viewState)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                switch (viewState)
                {
                    case ViewState.Visible:
                        view.Hidden = false;
                        break;
                    case ViewState.Gone:
                    case ViewState.Invisible:
                        view.Hidden = true;
                        break;
                }
            });

            return view;
        }

        public static T GetViewWithType<T>(this UIView view) where T : UIView
        {
            return view as T ?? view.Subviews.Select(GetViewWithType<T>).FirstOrDefault(res => res != null);
        }

        public static void SetCornerRadius(this UIView view, CGSize radii, UIRectCorner corners)
        {
            var rounded = UIBezierPath.FromRoundedRect(view.Bounds, corners, radii);
            var shape = new CAShapeLayer { Path = rounded.CGPath };
            view.Layer.Mask = shape;
            view.Layer.MasksToBounds = true;
        }

        public static void SetHeightScroll(this UIScrollView scroller)
        {
            nfloat contentRect = 0.0f;

            foreach (var view in scroller.Subviews)
            {
                var contentRects = view.Frame.Size.Height + view.Frame.Y;

                if (contentRects > contentRect)
                    contentRect = contentRects;
            }

            scroller.ContentSize = new CGSize(320, contentRect + contentRect / 20);
        }

        public static void OnSwipe(this UIView view, Action<UISwipeGestureRecognizer> action)
        {
            view.UserInteractionEnabled = true;
            var swipeDirectionR = new UISwipeGestureRecognizer(action)
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            var swipeDirectionL = new UISwipeGestureRecognizer(action)
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };
            view.AddGestureRecognizer(swipeDirectionR);
            view.AddGestureRecognizer(swipeDirectionL);
        }

        public static void RoundViewWithBorder(this UIView view, CGColor borderColor, float borderWidth,
            float? radiusAspect = null, RadiusType typeRadius = RadiusType.Static)
        {
            if (typeRadius == RadiusType.Static)
                view.SetRoundView(radiusAspect ?? 0.5f);
            else
                view.RoundView(radiusAspect);

            view.Layer.BorderColor = borderColor;
            view.Layer.BorderWidth = borderWidth;
        }

        public static void RoundView(this UIView view, float? radiusAspect = null)
        {
            view?.Superview?.SetNeedsLayout();
            view?.Superview?.LayoutIfNeeded();

            view.Layer.CornerRadius = view.Bounds.Height * (radiusAspect ?? 0.5f);
            view.Layer.MasksToBounds = true;
        }

        public static void SetRoundView(this UIView view, float radius)
        {
            if (view.IsNull()) return;

            view?.Superview?.SetNeedsLayout();
            view?.Superview?.LayoutIfNeeded();

            view.Layer.CornerRadius = radius;
            view.Layer.MasksToBounds = true;
        }


        public static UIFont GetFontWithTraits(this UIFontDescriptorSymbolicTraits traits, nfloat size)
        {
            var desc = new UIFontDescriptor();
            desc = desc.CreateWithTraits(traits);
            return UIFont.FromDescriptor(desc, size);
        }

        public static CTFont GetCtFont(this UIFont font)
        {
            var fda = new CTFontDescriptorAttributes
            {
                FamilyName = font.FamilyName,
                Size = (float)font.PointSize,
                StyleName = font.FontDescriptor.Face
            };
            var fd = new CTFontDescriptor(fda);
            return new CTFont(fd, 0);
        }

        public static void RestoreLayer(this UIView view)
        {
            view.Layer.CornerRadius = 0;
            view.Layer.MasksToBounds = false;
            view.Layer.BorderColor = UIColor.Clear.CGColor;
            view.Layer.BorderWidth = 0;
        }

        public static void AddGradientBorder(this UIView view, BorderGrandientConfig config)
        {
            view.Layer.MasksToBounds = true;
            view.Layer.BorderWidth = 0f;

            var grandientLayer = new CAGradientLayer();
            var bounds = new CGRect(CGPoint.Empty, view.Frame.Size);
            grandientLayer.Frame = bounds;
            grandientLayer.StartPoint = config.StartPoint;
            grandientLayer.EndPoint = config.EndPoint;
            grandientLayer.Colors = new[] { config.StartColor, config.EndColor };

            var shapeLayer = new CAShapeLayer
            {
                LineWidth = config.BorderWidth,
                FillColor = null,
                StrokeColor = UIColor.Black.CGColor,
                Path = UIBezierPath.FromRoundedRect(bounds,
                    view.Layer.CornerRadius).CGPath
            };

            grandientLayer.Mask = shapeLayer;
            view.Layer.AddSublayer(grandientLayer);
        }

        public static void AddRightBorder(this UIView view, UIColor color, float borderWidth = 1f)
        {
            var bottomBorder = new CALayer
            {
                BorderColor = color.CGColor,
                BorderWidth = borderWidth,
                Frame = new RectangleF((float)view.Frame.Size.Width - borderWidth, 0, borderWidth,
                (float)view.Frame.Size.Height)
            };
            view.Layer.AddSublayer(bottomBorder);
        }

        public static void AddLeftBorder(this UIView view, UIColor color, float borderWidth = 1f)
        {
            var bottomBorder = new CALayer
            {
                BorderColor = color.CGColor,
                BorderWidth = borderWidth,
                Frame = new RectangleF(0, 0, borderWidth, (float)view.Frame.Size.Height)
            };

            view.Layer.AddSublayer(bottomBorder);
        }

        public static void AddBottomBorder(this UIView view, UIColor color, float borderWidth = 1f)
        {
            var bottomBorder = new CALayer
            {
                BorderColor = color.CGColor,
                BorderWidth = borderWidth,
                Frame = new RectangleF(0, (float)view.Frame.Height - borderWidth,
                (float)view.Frame.Size.Width, borderWidth)
            };
            view.Layer.AddSublayer(bottomBorder);
        }



        public static void SetBorder(this UIView btn, UIColor color, float borderWidth = 1f)
        {
            btn.Layer.MasksToBounds = true;
            btn.Layer.BorderColor = color.CGColor;
            btn.Layer.BorderWidth = borderWidth;

            btn.Layer.Frame = new CGRect(-1, -1, btn.Frame.Width, btn.Frame.Height);
        }

        public static AVPlayer ShowVideo(this UIView view, string pathVideo)
        {
            var asset = AVAsset.FromUrl(NSUrl.FromFilename(pathVideo));
            var playerItem = new AVPlayerItem(asset);

            var player = new AVPlayer(playerItem);

            var playerLayer = AVPlayerLayer.FromPlayer(player);
            playerLayer.Frame = view.Frame;

            view.Layer.AddSublayer(playerLayer);

            player.Play();

            return player;
        }

        public static void ShowVideoUrlMp4(this UIView view, string pathVideo)
        {
            var moviePlayer = new MPMoviePlayerController
            {
                ContentUrl = new NSUrl(pathVideo),
                ShouldAutoplay = false,
                ControlStyle = MPMovieControlStyle.Embedded
            };


            moviePlayer.View.Frame = view.Frame;
            moviePlayer.PrepareToPlay();

            moviePlayer.Play();
            moviePlayer.SetFullscreen(true, true);


            MPMoviePlayerController.Notifications.ObserveDidExitFullscreen((sender, e) =>
            {
                moviePlayer.Stop();
                moviePlayer.View.RemoveFromSuperview();
                moviePlayer.Dispose();
                moviePlayer = null;
                GC.Collect();
            });

            view.AddSubview(moviePlayer.View);
        }

        public static UIViewController GetViewController(this UIView view)
        {
            UIResponder responder = view;
            while (!(responder is UIViewController))
            {
                responder = responder.NextResponder;
                if (responder == null)
                    break;
            }

            return (UIViewController)responder;
        }

        public static UIStoryboard GetStoryboard(this UIView view)
        {
            var viewController = view?.GetViewController();
            return viewController?.Storyboard;
        }

        public static IObservable<object> WhenClick(this UIView @this)
        {
            UIGestureRecognizer gesture = null;
            return Observable.FromEventPattern(e => { gesture = @this.OnClick(() => e?.Invoke(@this, null)); },
                e => @this.RemoveGestureRecognizer(gesture));
        }

        public static IDisposable WhenClick(this UIView @this, Action action)
        {
            return @this.WhenClick().Subscribe(e => action?.Invoke());
        }
    }
}