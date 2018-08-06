//
//  BaseWindow.cs
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
using Int.Core.Application.Window.Contract;
using Int.Core.Data.MVVM.Contract;
using Int.Core.Extensions;
using Int.iOS.Extensions;
using Int.iOS.Views;
using Int.iOS.Wrappers.Widget.CrossViewInjection;
using UIKit;

namespace Int.iOS.Window
{
    public class BaseWindow : UIViewXib, IPopupWindow, IViewModelContainer
    {
        public BaseWindow(Type nameView) : base(nameView.Name)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                AppTools.RootNavigationController.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
                AppTools.RootNavigationController.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
            });
        }

        public virtual void Close()
        {
            this.AnimationFade(AnimationType.Out, onCompletion: obj =>
            {
                AppTools.InvokeOnMainThread(Hide);
                this.Alpha = 1.0f;
            });
        }

        public virtual void Show()
        {
            AppTools.InvokeOnMainThread(() =>
            {
                if (!ModelView.IsNull())
                    new CrossViewInjector(this);

                Frame = AppTools.TopViewController.View.Frame;
                AppTools.TopViewController.View.AddSubview(this);
            });
        }

        public virtual IBaseViewModel ModelView { get; }
    }
}