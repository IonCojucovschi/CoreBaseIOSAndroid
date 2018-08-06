//
//  UIViewXib.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2017 Songurov
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using CoreGraphics;
using Foundation;
using Int.Core.Extensions;
using Int.iOS.Extensions;
using UIKit;

namespace Int.iOS.Views
{
    [Register("UIViewXib")]
    public abstract class UIViewXib : UIView
    {
        private bool _wasInit;

        protected UIViewXib()
        {
            Initialize();
        }

        protected UIViewXib(string nameView = "")
        {
            ViewName = nameView;
            Initialize();
        }

        protected UIViewXib(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        protected UIViewXib(CGRect frame) : base(frame)
        {
            Initialize();
        }

        public UIView ContentView { get; private set; }

        public string Text { get; set; }

        public string ViewName { get; }


        public override CGRect Bounds
        {
            get => base.Bounds;
            set
            {
                base.Bounds = value;
                if (ContentView != null)
                    ContentView.Bounds = value;
            }
        }

        public virtual void ViewDidLoad()
        {
        }

        public virtual void Hide()
        {
            InvokeOnMainThread(RemoveFromSuperview);
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            if (_wasInit) return;
            ViewDidLoad();
            _wasInit = true;
        }

        private void LoadNib()
        {
            try
            {
                ContentView = GetNib().GetItem<UIView>(0);
                ContentView.Frame = Bounds;
                ContentView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                ContentView.TranslatesAutoresizingMaskIntoConstraints = true;
                AddSubview(ContentView);
            }
            catch
            {
                throw new Exception("After that name I can not find xib , please write Identify");
            }
        }

        /// <summary>
        ///     Create Reference from CustomView
        /// </summary>
        /// <returns>The object.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected T CreateObject<T>() where T : UIView
        {
            return this.GetViewWithType<T>();
        }

        private NSArray GetNib()
        {
            var views = NSBundle.MainBundle.LoadNib(ViewName.IsNullOrWhiteSpace() ? GetType().Name : ViewName, this,
                null);

            if (views.Count == 0)
                throw new Exception("Not Views");

            return views;
        }

        private void Initialize()
        {
            BackgroundColor = UIColor.Clear;
            LoadNib();
        }
    }
}