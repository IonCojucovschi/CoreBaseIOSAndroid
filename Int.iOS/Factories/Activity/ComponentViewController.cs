//
// ComponentViewController.cs
//
// Author:
//       Sogurov Fiodor <f.songurov@software-dep.net>
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
using System.Threading;
using Foundation;
using Int.Core.IO.Config;
using UIKit;
using Int.iOS.Window;

namespace Int.iOS.Factories.Activity
{
    public abstract class ComponentViewController : UIViewController
    {
        private const int MilisecondsInSecond = 1000;

        private UIView _activeTextView;

        protected ComponentViewController(IntPtr handle) : base(handle)
        {
        }

        protected IDictionary<string, UIStoryboardSegue> SegueCollection { get; set; } =
            new Dictionary<string, UIStoryboardSegue>();

        public string TitlePage { get; set; } = string.Empty;

        /// <summary>
        ///     Return the UIView to which hiding keyboard gestures will be added.
        ///     Supposed to return root view.
        ///     If is null then gestures will not be created. Null by default.
        /// </summary>
        protected virtual UIView KeyboardHideGestureHostingView => null;

        protected virtual UIView ActiveTextView => _activeTextView;
        protected bool IsKeyboardShown { get; private set; }

        protected abstract void BindViews();
        protected abstract void HandlerViews();
        protected abstract void RemoveHandlerViews();
        protected abstract void TranslateViews();

        protected virtual void UnBindViews()
        {
            Hide();
        }

        protected virtual void ReloadViews()
        {
        }

        protected virtual void SetFontsViews()
        {
        }

        protected virtual void GoBack(bool animated = true)
        {
            InvokeOnMainThread(() =>
            {
                if (NavigationController?.ViewControllers?.Length <= 1) return;
                NavigationController?.PopViewController(animated);
            });
        }

        protected virtual void ConfigurationPage()
        {
        }

        protected void GoToScreen(Type type, bool animation = true)
        {
            InvokeOnMainThread(() =>
            {
                if (NavigationController == null || Storyboard == null) return;
                NavigationController.PushViewController(Storyboard.InstantiateViewController(type.Name), animation);
            });
        }

        protected virtual void TemplateMethod()
        {
            BindViews();
            InitHandler();
            TranslateViews();
            ConfigurationPage();
            SetFontsViews();
        }

        private void InitHandler()
        {
            RemoveHandlerViews();
            HandlerViews();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            RemoveHandlerViews();
        }

        public virtual void CloseApp()
        {
            Thread.CurrentThread.Abort();
        }

        public static void OpenUrl(NSUrl url, Action<bool> completion)
        {
            AppTools.OpenUrl(url, completion);
        }

        protected void ShowWindow<T>(T type) where T : UIViewController
        {
            PresentViewController(type, false, null);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SubscribeToUiNotifications();
            TemplateMethod();
            AddKeyboardHidingGestures();
        }

        private void SubscribeToUiNotifications()
        {
            if (KeyboardHideGestureHostingView == null) return;

            UIKeyboard.Notifications.ObserveWillShow(KeyboardWillShowInternalEventHandler);
            UIKeyboard.Notifications.ObserveWillHide(KeyboardWillHideInternalEventHandler);

            UITextField.Notifications.ObserveTextDidBeginEditing(TextViewDidBeginEditingEventHandler);
            UITextField.Notifications.ObserveTextDidEndEditing(TextViewDidEndEditingEventHandler);

            UITextView.Notifications.ObserveTextDidBeginEditing(TextViewDidBeginEditingEventHandler);
            UITextView.Notifications.ObserveTextDidEndEditing(TextViewDidEndEditingEventHandler);
        }

        private void AddKeyboardHidingGestures()
        {
            if (KeyboardHideGestureHostingView == null) return;

            var tapGestureRecognizer = new UITapGestureRecognizer(HideKeyboard)
            {
                ShouldReceiveTouch = (recognizer, touch) => IsKeyboardShown
            };

            KeyboardHideGestureHostingView.AddGestureRecognizer(tapGestureRecognizer);

            var swipeGestureRecognizer = new TopPrioritySwipeGesture(HideKeyboard)
            {
                Direction = UISwipeGestureRecognizerDirection.Down,
                ShouldReceiveTouch = (recognizer, touch) => IsKeyboardShown
            };

            KeyboardHideGestureHostingView.AddGestureRecognizer(swipeGestureRecognizer);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            SegueCollection.Add(segue.Identifier ?? segue.ToString(), segue);
        }

        protected T GetContainerView<T>(string identifier = null) where T : UIViewController
        {
            UIStoryboardSegue segue = null;

            if (!string.IsNullOrWhiteSpace(identifier))
                SegueCollection?.TryGetValue(identifier, out segue);
            else
                segue = SegueCollection?.Values?.FirstOrDefault(x =>
                    x.DestinationViewController.GetType().Name == typeof(T).Name);

            return segue?.DestinationViewController as T;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ReloadViews();
            InitHandler();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            UnBindViews();
        }

        protected void HideKeyboard()
        {
            ActiveTextView?.ResignFirstResponder();
        }

        private void KeyboardWillShowInternalEventHandler(object sender, UIKeyboardEventArgs e)
        {
            OnKeyboardWillAppear(e.FrameEnd.Width, e.FrameEnd.Height, e.AnimationDuration, e.AnimationCurve);
        }

        private void KeyboardWillHideInternalEventHandler(object sender, UIKeyboardEventArgs e)
        {
            OnKeyboardWillDisappear(e.FrameBegin.Width, e.FrameBegin.Height, e.AnimationDuration, e.AnimationCurve);
        }

        protected virtual void OnKeyboardWillAppear(nfloat keyboardWidth, nfloat keyboardHeight,
            double animationDuration, UIViewAnimationCurve animationCurve)
        {
            IsKeyboardShown = true;
        }

        protected virtual void OnKeyboardWillDisappear(nfloat keyboardWidth, nfloat keyboardHeight,
            double animationDuration, UIViewAnimationCurve animationCurve)
        {
            IsKeyboardShown = false;
        }


        private void TextViewDidBeginEditingEventHandler(object sender, NSNotificationEventArgs e)
        {
            _activeTextView = e.Notification.Object as UIView;
        }

        private void TextViewDidEndEditingEventHandler(object sender, NSNotificationEventArgs e)
        {
            if (ActiveTextView == e.Notification.Object)
                _activeTextView = null;
        }

        #region iDialog

        public virtual void Hide()
        {
            AppTools.InvokeOnMainThread(WindowShare.Instance.Hide);
        }

        public virtual void Show()
        {
            AppTools.InvokeOnMainThread(() =>
            {
                WindowShare.Instance.Show(DialogConfig.Instance.Wait,
                                          Core.Application.Window.Contract.TimeIWindow.Normal);
            });
        }

        public virtual void ShowSuccess(string message = null, int timeSecond = 3)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                WindowShare.Instance.ShowSuccess(DialogConfig.Instance.Success,
                                                 Core.Application.Window.Contract.TimeIWindow.Normal);
            });
        }

        public virtual void ShowError(string message = null, int timeSecond = 3)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                WindowShare.Instance.ShowError(DialogConfig.Instance.Error,
                                                 Core.Application.Window.Contract.TimeIWindow.Normal);
            });
        }

        #endregion

        private class TopPrioritySwipeGesture : UISwipeGestureRecognizer
        {
            public TopPrioritySwipeGesture(Action action) : base(action) { }

            public override bool ShouldBeRequiredToFailByGestureRecognizer(UIGestureRecognizer otherGestureRecognizer)
            {
                return true;
            }
        }
    }
}