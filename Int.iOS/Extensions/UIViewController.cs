//
// UIViewController.cs
//
// Author:
//       Sogurov Fiodor <f.songurov@software-dep.net>
//
// Copyright (c) 2017 Songurov
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
using UIKit;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        public static void ClearStack(this UINavigationController navigation, int positionClear)
        {
            var a = navigation.ViewControllers.ToList();

            var counter = 0;

            for (var i = a.Count - 1; i >= 0; --i)
            {
                counter++;
                if (positionClear >= counter)
                    a.RemoveAt(i);
                else
                    break;
            }

            navigation.ViewControllers = a.ToArray();
        }

        public static void GoToScreen(this UIViewController controller, Type type, bool animation = true,
            int clearStackCount = 0)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                if (clearStackCount > 0)
                    AppTools.RootNavigationController.ClearStack(clearStackCount);

                if (AppTools.RootNavigationController != null && AppTools.Storyboard != null)
                    AppTools.RootNavigationController.PushViewController(
                        AppTools.Storyboard.InstantiateViewController(type.Name), animation);
            });
        }

        public static void MoveTo(this UIViewController current, UIViewController parent, UIView containerView = null)
        {
            parent.AddChildViewController(current);
            containerView = containerView ?? parent.View;
            containerView.Layer.MasksToBounds = true;
            current.View.Frame = containerView.Bounds;
            current.View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            current.View.TranslatesAutoresizingMaskIntoConstraints = true;
            containerView.AddSubview(current.View);

            current.DidMoveToParentViewController(parent);
        }

        public static void RemoveFromParent(this UIViewController current)
        {
            current.WillMoveToParentViewController(null);
            current.View.RemoveFromSuperview();
            current.RemoveFromParentViewController();
        }
    }
}