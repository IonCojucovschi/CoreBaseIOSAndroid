//
//  BaseNativeViewModel.cs
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
using System.Diagnostics;
using Int.Core.Application.Window.Contract;
using Int.Core.IO.Config;
using Int.Data.MVVM;
using Int.Droid.Extensions;
using Java.Lang;
using Int.Droid.Window;

namespace Int.Droid.Data.MVVM
{
    public abstract class BaseNativeViewModel : BaseViewModel
    {
        public override void GoBack()
        {
            AppTools.CurrentActivity?.GoToScreen(null,
                true,
                animationLeft: Resource.Animation.LeftSlide,
                animationRight: Resource.Animation.RightSlide);
        }

        public override void GoPage(Type type, object parm = null)
        {
            AppTools.CurrentActivity?.GoToScreen(type,
                animationLeft: Resource.Animation.MoveRight,
                animationRight: Resource.Animation.MoveLeft);
        }

        protected override void RunOnMainThread(Action action)
        {
            if (action != null)
                AppTools.RunOnUiThread(action);
        }

        #region PopupWindow

        protected override bool ImplementsIPopupWindow(Type type)
        {
            return type?.GetInterface(nameof(IPopupWindow)) != null;
        }

        protected override object CreateObjectOfType(Type type)
        {
            var ctor = type.GetConstructor(new Type[] { });
            return ctor?.Invoke(new Type[] { });
        }

        #endregion

        #region IDialog

        public override void Hide()
        {
            try
            {
                WindowShare.Instance.Hide();
            }
            catch (IllegalArgumentException e)
            {
                if (e.Message != "View not attached to window manager")
                    throw e;

                Debug.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
        }

        public override void Show()
        {
            WindowShare.Instance.Show(DialogConfig.Instance.Wait,
                                      TimeIWindow.Normal);
        }

        public override void Show(string text)
        {
            WindowShare.Instance.Show(text,
                                      TimeIWindow.Normal);
        }

        public override void ShowSuccess(string message = "", int timeSecond = 3)
        {
            WindowShare.Instance.Show(message ?? DialogConfig.Instance.Success,
                                      TimeIWindow.Normal);
        }

        public override void ShowError(string message = "", int timeSecond = 3)
        {
            WindowShare.Instance.Show(message ?? DialogConfig.Instance.Error,
                                      TimeIWindow.Normal);
        }

        #endregion

        #region DeviceConnectivity

        public override void CallNumber(string number)
        {
            AppTools.OpenDialPhone(number, AppTools.CurrentActivity);
        }

        public override void ComposeEmail(string address)
        {
            AppTools.SendEmail(address, AppTools.CurrentActivity);
        }

        public override void OpenLink(string url)
        {
            AppTools.OpenLink(url, AppTools.CurrentActivity);
        }

        #endregion
    }
}