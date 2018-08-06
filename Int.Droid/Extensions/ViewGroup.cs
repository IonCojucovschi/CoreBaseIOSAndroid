//
// ViewGroup.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        public static void SetFonts(this ViewGroup grp, Typeface type, ICollection<View> ignoreList = null)
        {
            for (var i = 0; i < grp.ChildCount; i++)
            {
                var view = grp.GetChildAt(i);
                if (ignoreList != null &&
                    ignoreList.Select(_ => ReferenceEquals(view, _)).ToList().Count == 0)
                    continue;
                var viewGroup = view as ViewGroup;
                if (viewGroup != null)
                {
                    SetFonts(viewGroup, type, ignoreList);
                    continue;
                }
                var textView = view as TextView;
                if (textView == null) continue;
                if (ignoreList == null)
                    textView.SetTypeface(type, TypefaceStyle.Normal);
            }
        }

        public static IObservable<object> WhenClick(this View @this)
        {
            @this.Clickable = true;
            return Observable.FromEventPattern(e => @this.Click += e, e => @this.Click -= e);
        }

        public static IDisposable WhenClick(this View @this, Action action)
        {
            return @this.WhenClick().Subscribe(e => action?.Invoke());
        }
    }
}