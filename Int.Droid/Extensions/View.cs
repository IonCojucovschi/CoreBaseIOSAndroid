//
// View.cs
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
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Int.Droid.Views;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        private const int IPhone6HeightDp = 667;

        public static int GetWidthScaleDown(this View view)
        {
            return (int)(view.Width / Resources.System.DisplayMetrics.Density);
        }

        public static int GetHeightScaleDown(this View view)
        {
            return (int)(view.Height / Resources.System.DisplayMetrics.Density);
        }

        public static int Iphone6PointsToDevicePixels(this float heightPoints)
        {
            var proportionalHeight = heightPoints / IPhone6HeightDp;
            var metrics = Resources.System.DisplayMetrics;
            return (int)Math.Ceiling(metrics.HeightPixels * proportionalHeight);
        }

        public static ViewGroup.LayoutParams SetHeightWeight(this Context context, out int heightView,
            int addingHeight = 60)
        {
            heightView = context.Resources.DisplayMetrics.HeightPixels / 3 - addingHeight;
            return new AbsListView.LayoutParams(context.Resources.DisplayMetrics.WidthPixels, heightView);
        }

        public static void OnLayout(this View view, Action action)
        {
            var layoutLisener = new GlobalLayoutListener(view, true);

            layoutLisener.GlobalLayout += (sender, e) =>
            {
                action?.Invoke();
                layoutLisener = null;
            };
        }

        public static void SetBackgroundRound(this View view, float radius, Color color)
        {
            view?.OnLayout(() =>
            {
                var round = new RoundView();
                round.Color = color;
                round.Radius = radius;

                view.SetBackgroundCompat(round);
            });
        }

        public static void SetBackgroundRoundWithBorder(this View view, float radius, Color color,
            Color borderColor, float borderWidth)
        {
            view?.OnLayout(() =>
            {
                var round = new RoundView();
                round.Color = color;
                round.BorderColor = borderColor;
                round.LineWidth = borderWidth;
                round.Radius = radius;

                view.SetBackgroundCompat(round);
            });
        }

        public static void OnTouch(this View view, Action actionUp, Action actionDown = default(Action),
            float[] alpha = default(float[]))
        {
            if (view != null)
                view.Touch += (sender, e) =>
                {
                    if (e.Event == null) return;
                    if (alpha == null)
                        alpha = new[] { 0.5f, 1, 1 };

                    switch (e.Event.Action)
                    {
                        case MotionEventActions.Down:
                        case MotionEventActions.Move:
                            {
                                ((View)sender).Alpha = alpha.ElementAtOrDefault(0);
                                actionDown?.Invoke();
                            }
                            break;
                        case MotionEventActions.Cancel:
                            ((View)sender).Alpha = alpha.ElementAtOrDefault(1);
                            break;
                        case MotionEventActions.Up:
                            {
                                ((View)sender).Alpha = alpha.ElementAtOrDefault(2);
                                actionUp?.Invoke();
                            }
                            break;
                    }
                };
        }

        public static void SetTintOnTouch(this View imgBtn, ImageView[] image, TextView text, Action action,
            Color color)
        {
            if (imgBtn != null)
                imgBtn.Touch += (sender, e) =>
                {
                    Console.WriteLine(e.Event.Action);

                    switch (e.Event.Action)
                    {
                        case MotionEventActions.Down:
                        case MotionEventActions.Move:
                            image[0].SetColorFilter(color);
                            image[1].SetColorFilter(color);
                            text.SetTextColor(color);

                            break;
                        case MotionEventActions.Cancel:
                        case MotionEventActions.Up:
                            image[0].ClearColorFilter();
                            image[1].ClearColorFilter();

                            text.SetTextColor(Color.White);
                            action();

                            break;
                        default:
                            image[0].ClearColorFilter();
                            image[1].ClearColorFilter();

                            text.SetTextColor(Color.White);
                            break;
                    }
                };
        }

        public static void SetBackgroundCompat(this View view, Drawable drawable)
        {
            AppTools.CurrentActivity.RunOnUiThread(() =>
            {
                if (Build.VERSION.SdkInt > BuildVersionCodes.JellyBean)
                    view.Background = drawable;
                else
#pragma warning disable CS0618 // Type or member is obsolete
                    view.SetBackgroundDrawable(drawable);
#pragma warning restore CS0618 // Type or member is obsolete
            });
        }

        public static Tuple<View, PopupWindow> GetViewPopupWindow(this int resource)
        {
            var view = AppTools.CurrentActivity.LayoutInflater.Inflate(resource, null);
            var popUp = new PopupWindow(view, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent,
                true);

            popUp.ShowAtLocation(view, GravityFlags.Center, 0, 0);

            return Tuple.Create(view, popUp);
        }
    }
}