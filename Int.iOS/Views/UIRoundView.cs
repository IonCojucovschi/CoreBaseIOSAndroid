//
// RoundView.cs
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
using Foundation;
using Int.iOS.Extensions;
using UIKit;

namespace Int.iOS.Views
{
    public class UIRoundView : UIView
    {
        public UIRoundView()
        {
        }

        public UIRoundView(CGRect frame) : base(frame)
        {
        }

        protected internal UIRoundView(IntPtr handle) : base(handle)
        {
        }

        protected UIRoundView(NSObjectFlag t) : base(t)
        {
        }

        public UIRoundView(NSCoder coder) : base(coder)
        {
        }

        public override CGRect Bounds
        {
            get => base.Bounds;
            set
            {
                base.Bounds = value;
                this.RoundView();
            }
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            this.RoundView();
        }
    }
}