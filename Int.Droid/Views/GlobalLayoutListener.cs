//
// GlobalLayoutListener.cs
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
using Android.Util;
using Android.Views;
using Object = Java.Lang.Object;

namespace Int.Droid.Views
{
    public class GlobalLayoutListener : Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private const string Tag = "GlobalLayoutLisener";
        private EventHandler _handler;
        private View _view;
        private ViewTreeObserver _viewTreeObserver;
        private bool _wasRaise;
        private bool _wasRemoved;

        public GlobalLayoutListener()
        {
            OneTime = false;
        }

        public GlobalLayoutListener(View view, bool oneTime = false)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view), "view == null");
            _view = view;
            OneTime = oneTime;
            Init();
        }

        public View View
        {
            get => _view;
            set
            {
                _view = value;
                if (value != null)
                    Init();
            }
        }

        public bool OneTime { get; set; }

        void ViewTreeObserver.IOnGlobalLayoutListener.OnGlobalLayout()
        {
            EmitGlobalLayout();
            if (OneTime)
            {
                if (_wasRaise)
                    return;
                _wasRaise = true;
                RemoveLisener();
                return;
            }
            _viewTreeObserver = _view.ViewTreeObserver;
        }

        ~GlobalLayoutListener()
        {
            RemoveLisener();
        }


        public event EventHandler GlobalLayout
        {
            add
            {
                if (_view == null)
                    throw new NullReferenceException("View was not set yet");
                if (_wasRaise)
                {
                    value(this, null);
                    return;
                }
                _handler += value;
            }

            remove
            {
                // Analysis disable once DelegateSubtraction
#pragma warning disable RECS0020 // Delegate subtraction has unpredictable result
                _handler -= value;
#pragma warning restore RECS0020 // Delegate subtraction has unpredictable result
            }
        }

        private void RemoveLisener()
        {
            if (_wasRemoved)
                return;
            if (_view == null)
                return;
            try
            {
                _viewTreeObserver = _view.ViewTreeObserver;
                _viewTreeObserver.RemoveOnGlobalLayoutListener(this);
                _wasRemoved = true;
                _view = null;
                _viewTreeObserver = null;
            }
            catch (Exception ex)
            {
                Log.Warn(Tag, $"Failed to remove lisenere:{ex.Message}");
            }
        }

        private void EmitGlobalLayout()
        {
            _handler?.Invoke(_view, null);
        }

        private void Init()
        {
            _viewTreeObserver = _view.ViewTreeObserver;
            _viewTreeObserver.AddOnGlobalLayoutListener(this);
        }
    }
}