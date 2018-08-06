//
// BasePageViewModel.cs
//
// Author:
//       Songurov Fiodor <songurov@gmail.com>
//
// Copyright (c) 2016 Songurov Fiodor
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
using System.Windows.Input;
using Int.Core.Application.Contract;
using Int.Core.Application.Window.Contract;
using Int.Core.Data.MVVM.Contract;
using Int.Core.Data.Repository.Akavache.Contract;
using Int.Core.Wrappers.Window;
using ReactiveUI.Fody.Helpers;

namespace Int.Data.MVVM
{
    public abstract class BaseViewModel : ObservableObject, IBaseViewModel
    {
        public Type TypePage => GetType();

        public IUnitOfWork UnitOfWork { get; set; }

        public virtual ICommand Command { get; set; }

        [Reactive]
        public virtual string TitleApp { get; set; }

        public virtual void GoBack()
        {
        }

        public virtual void GoPage(Type type, object parm = null)
        {
        }

        public virtual IUser CurrentUser { get; set; }

        public IList<IWindow> Windows { get; set; }

        /// <summary>
        ///     Gets the window view.
        ///     Init view in collection WINDOWS and view impliment interface IWindow
        /// </summary>
        /// <returns>The window view.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected IWindow GetWindow<T>() where T : IWindow
        {
            return WindowFactory
                .GetWindow(this, typeof(T).Name);
        }

        #region IDialog

        public virtual void ShowError(string message = "", int timeSecond = 3)
        {
        }

        public virtual void ShowSuccess(string message = "", int timeSecond = 3)
        {
        }

        public IList<string> ButtonText { get; set; }

        public string TextIDialog { get; set; }


        #endregion IDialog

        #region temp

        public virtual void Hide()
        {
        }

        public virtual void Show()
        {
        }

        public virtual void Show(string text)
        {
        }

        #endregion

        protected abstract void RunOnMainThread(Action action);

        /// <summary>
        /// Called by hosting page/activity/view on resume to update changes in data/collections.
        /// </summary>
        public abstract void UpdateData();

        public abstract void OnPause();

        #region PopupWindow

        public IPopupWindow CurrentPopupWindow { get; set; }

        protected abstract bool ImplementsIPopupWindow(Type type);
        protected abstract object CreateObjectOfType(Type type);

        public virtual IPopupWindow CreatePopupWindow<T>()
        where T : IPopupWindow, new()
        {
            var popupWindow = new T() as IPopupWindow;
            AssignPopupWindowToVM(popupWindow);
            return popupWindow;
        }

        public virtual IPopupWindow CreatePopupWindow(Type windowType)
        {
            if (!ImplementsIPopupWindow(windowType))
                throw new Exception($"Type should implement {nameof(IPopupWindow)}");

            var popupWindow = CreateObjectOfType(windowType) as IPopupWindow;
            AssignPopupWindowToVM(popupWindow);
            return popupWindow;
        }

        /// <summary>
        /// Tries to save <see cref="IPopupWindow"/> refernce to ViewModel of this popup.
        /// Reference used to call <see cref="IPopupWindow.Close()"/> in advance.
        /// </summary>
        private void AssignPopupWindowToVM(IPopupWindow popupWindow)
        {
            var viewModel = (popupWindow as IViewModelContainer)?.ModelView;
            if (viewModel == null) return;

            viewModel.CurrentPopupWindow = popupWindow;
        }

        #endregion

        #region DeviceConnectivity

        public abstract void CallNumber(string number);

        public abstract void ComposeEmail(string address);

        public abstract void OpenLink(string url);

        #endregion
    }
}