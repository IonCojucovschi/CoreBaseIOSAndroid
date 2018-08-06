﻿//
// ClickableView.cs
//
// Author:
//       Valentin Grigorean <v.grigorean@software-dep.net>
//
// Copyright (c) 2016 
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
using Int.Core.Wrappers;
using Int.iOS.Extensions;
using UIKit;

namespace Int.iOS.Wrappers
{
    [Register("ClickableView")]
    public class ClickableView : UIView, IClickableView
    {
        public ClickableView()
        {
            Initialize();
        }

        public ClickableView(CGRect frame) : base(frame)
        {
            Initialize();
        }

        public ClickableView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public ClickableView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public ClickableView(NSObjectFlag t) : base(t)
        {
            Initialize();
        }

        public bool Enabled { get; set; } = true;


        public event EventHandler Click = delegate { };

        public void OnTouch(object view, State began, Action clickAction)
        {
            throw new NotImplementedException();
        }

        public virtual void SendActionForControlEvent()
        {
            if (Enabled)
                Click?.Invoke(this, EventArgs.Empty);
        }


        private void Initialize()
        {
            this.OnClick(SendActionForControlEvent);
        }
    }
}