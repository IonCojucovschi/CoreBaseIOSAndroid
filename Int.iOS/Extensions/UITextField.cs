//
// UITextField.cs
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
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Int.Core.Extensions;
using Int.iOS.Controllers.ModalPicker;
using UIKit;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        public static async void SetTime(this UITextField txt, UIViewController controller,
            UIDatePickerMode mode = UIDatePickerMode.Date, string format = "dd.MM.yyyy")
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "", controller)
            {
                HeaderBackgroundColor = UIColor.Gray,
                HeaderTextColor = UIColor.White,
                TransitioningDelegate = new ModalPickerTransitionDelegate(),
                ModalPresentationStyle = UIModalPresentationStyle.Custom,
                DatePicker = {Mode = mode}
            };


            modalPicker.OnModalPickerDismissed += (s, ea) =>
            {
                var dateFormatter = new NSDateFormatter
                {
                    DateFormat = format
                };

                txt.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
            };

            await controller.PresentViewControllerAsync(modalPicker, true);
        }

        public static async Task SetTime(this UITextField lbl, UIViewController controller,
            UIDatePickerMode mode = UIDatePickerMode.Date, string format = "dd.MM.yyyy",
            Action<UITextField, string> actionNew = default(Action<UITextField, string>), bool setIntern = false)
        {
            var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "", controller)
            {
                HeaderBackgroundColor = UIColor.Gray,
                HeaderTextColor = UIColor.White,
                TransitioningDelegate = new ModalPickerTransitionDelegate(),
                ModalPresentationStyle = UIModalPresentationStyle.Custom,
                DatePicker = {Mode = mode}
            };


            if (setIntern)
                modalPicker.DatePicker.Date = NSDate.Now.AddSeconds(3600);

            modalPicker.OnModalPickerDismissed += (s, ea) =>
            {
                var dateFormatter = new NSDateFormatter
                {
                    DateFormat = format
                };

                if (!setIntern)
                    lbl.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
                actionNew?.Invoke(lbl, dateFormatter.ToString(modalPicker.DatePicker.Date));
            };

            await controller.PresentViewControllerAsync(modalPicker, true);
        }

        public static void SetPlaceholderColor(this UITextField textField, UIColor color)
        {
            SetPlaceholderColor(textField, textField.Placeholder, color);
        }

        public static void SetPlaceholderColor(this UITextField textField, string text, UIColor color)
        {
            textField.AttributedPlaceholder = new NSAttributedString(text.IsNull() ? "" : text, null, color);
        }

        public static void SetCapitalization(this UITextField textField,
            UITextAutocapitalizationType typeCharacters = UITextAutocapitalizationType.AllCharacters)
        {
            textField.AutocapitalizationType = typeCharacters;
        }

        public static void SetPadding(this UITextField textField, nfloat paddingLeft)
        {
            var view = new UIView
            {
                Frame = new CGRect(paddingLeft, paddingLeft, paddingLeft, paddingLeft)
            };

            textField.LeftView = view;
            textField.LeftViewMode = UITextFieldViewMode.Always;
        }

        public static void ColorBackround(this UITextField txt, UIView viewColor, bool animation = false)
        {
            txt.ShouldBeginEditing += textField =>
            {
                if (animation)
                    viewColor.AnimationShake();

                if (textField.Tag == 0)
                    viewColor.BackgroundColor = viewColor.BackgroundColor.ColorWithAlpha(1f);

                return true;
            };

            txt.ShouldEndEditing += textField =>
            {
                if (textField.Tag == 0)
                    viewColor.BackgroundColor = viewColor.BackgroundColor.ColorWithAlpha(0.4f);

                return true;
            };
        }

        public static void ResponderNext(this UITextField txt, UITextField next, Action go)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                txt.ShouldReturn += textField =>
                {
                    switch (textField.ReturnKeyType)
                    {
                        case UIReturnKeyType.Done:
                            textField?.ResignFirstResponder();
                            break;
                        case UIReturnKeyType.Default:
                            break;
                        case UIReturnKeyType.Go:
                            textField?.ResignFirstResponder();
                            go?.Invoke();
                            break;
                        case UIReturnKeyType.Google:
                            break;
                        case UIReturnKeyType.Join:
                            break;
                        case UIReturnKeyType.Next:
                            next?.BecomeFirstResponder();
                            break;
                        case UIReturnKeyType.Search:
                            textField?.ResignFirstResponder();
                            go?.Invoke();
                            break;
                    }
                    return true;
                };
            });
        }

        public static void SetReturn(this UITextField textView)
        {
            textView.ShouldReturn += TextFieldShouldReturn;
        }

        private static bool TextFieldShouldReturn(IUITextInputTraits textfield)
        {
            switch (textfield.ReturnKeyType)
            {
                case UIReturnKeyType.Done:
                    (textfield as UITextField)?.ResignFirstResponder();
                    break;
                case UIReturnKeyType.Default:
                    break;
                case UIReturnKeyType.Go:
                    (textfield as UITextField)?.ResignFirstResponder();
                    break;
                case UIReturnKeyType.Google:
                    break;
                case UIReturnKeyType.Join:
                    break;
                case UIReturnKeyType.Next:
                    (textfield as UITextField)?.BecomeFirstResponder();
                    break;
                case UIReturnKeyType.Route:
                    break;
                case UIReturnKeyType.Search:
                    (textfield as UITextField)?.ResignFirstResponder();
                    break;
                case UIReturnKeyType.Send:
                    break;
                case UIReturnKeyType.Yahoo:
                    break;
                case UIReturnKeyType.EmergencyCall:
                    break;
                case UIReturnKeyType.Continue:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UIView.BeginAnimations(string.Empty, IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);
            UIView.CommitAnimations();

            return false;
        }

        public static void LimitCharacterLength(this UITextField textView, int maxLength)
        {
            textView.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var currentCharacterCount = textField.Text?.Length ?? 0;
                if (range.Length + range.Location > currentCharacterCount)
                    return false;

                var newLength = currentCharacterCount + replacementString.Length - range.Length;
                return newLength <= maxLength;
            };
        }
    }
}