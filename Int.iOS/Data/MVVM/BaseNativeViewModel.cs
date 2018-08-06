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
using Int.Core.Application.Window.Contract;
using Int.Core.Extensions;
using Int.Core.IO.Config;
using Int.Data.MVVM;
using Int.iOS.Window;

namespace Int.iOS.Data.MVVM
{
    public abstract class BaseNativeViewModel : BaseViewModel
    {
        private const int MilisecondsInSecond = 1000;

        public override void GoBack()
        {
            AppTools.InvokeOnMainThread(() =>
            {
                if (AppTools.RootNavigationController?.ViewControllers?.Length <= 1) return;
                AppTools.RootNavigationController?.PopViewController(true);
            });
        }

        public override void GoPage(Type type, object parm = null)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                if (AppTools.RootNavigationController == null || AppTools.Storyboard == null) return;
                AppTools.RootNavigationController?.PushViewController(
                    AppTools.Storyboard?.InstantiateViewController(type?.Name), true);
            });
        }

        protected override void RunOnMainThread(Action action)
        {
            if (action != null)
                AppTools.InvokeOnMainThread(action);
        }

        #region IDialog

        public override void Hide()
        {
            base.Hide();

            AppTools.InvokeOnMainThread(WindowShare.Instance.Hide);
        }

        public override void Show()
        {
            base.Show();

            AppTools.InvokeOnMainThread(WindowShare.Instance.Show);
        }

        public override void Show(string text)
        {
            base.Show();

            AppTools.InvokeOnMainThread(() =>
            {
                WindowShare.Instance.Show(text, TimeIWindow.Normal);
            });
        }

        public override void ShowSuccess(string message = "", int timeSecond = 3)
        {
            base.ShowSuccess(message, timeSecond);

            AppTools.InvokeOnMainThread(() =>
            {
                WindowShare.Instance.ShowSuccess(message.IsNullOrWhiteSpace()
                    ? DialogConfig.Instance.Success
                                                 : message, TimeIWindow.Normal);
            });
        }

        public override void ShowError(string message = "", int timeSecond = 3)
        {
            base.ShowError(message, timeSecond);

            AppTools.InvokeOnMainThread(() =>
            {
                WindowShare.Instance.ShowError(message.IsNullOrWhiteSpace()
                                                 ? DialogConfig.Instance.Error
                                                 : message, TimeIWindow.Normal);
            });
        }

        #endregion

        #region PopupWindow

        protected override bool ImplementsIPopupWindow(Type type)
        {
            return type.GetInterface(nameof(IPopupWindow)) != null;
        }

        protected override object CreateObjectOfType(Type type)
        {
            var ctor = type.GetConstructor(new Type[] { });
            return ctor?.Invoke(new Type[] { });
        }

        #endregion

        #region DeviceConnectivity

        public override void CallNumber(string number)
        {
            AppTools.OpenPhone(number);
        }

        public override void ComposeEmail(string address)
        {
            AppTools.OpenMail(address);
        }

        public override void OpenLink(string url)
        {
            AppTools.OpenLink(url);
        }

        #endregion
    }
}