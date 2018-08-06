using System;
using CoreAnimation;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract;
using Int.Core.Wrappers.Widget.Exceptions;
using Int.iOS.Extensions;
using Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory.Interfaces;
using UIKit;

namespace Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory.NativeWrappers.Text
{
    public class UILabelWrapper : INativeTextViewWrapper
    {
        private const string NotImplementedExceptionMessage = "{0}.{1} not implemented";
        private readonly UILabel _labelView;

        public UILabelWrapper(UILabel labelView)
        {
            _labelView = labelView ?? throw new CrossWidgetWrapperNullReferenceException(nameof(labelView));
        }

        public string Text
        {
            get
            {
                var _text = "";
                if (_labelView != null)
                    AppTools.InvokeOnMainThread(() => { _text = _labelView?.Text; });
                return _text;
            }
            set
            {
                if (_labelView != null)
                    AppTools.InvokeOnMainThread(() => { _labelView.Text = value; });
            }
        }

        public string Hint
        {
            get
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        string.Format(NotImplementedExceptionMessage, GetType().Name, nameof(Hint))));
                return null;
            }
            set
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        string.Format(NotImplementedExceptionMessage, GetType().Name, nameof(Hint))));

                AppTools.InvokeOnMainThread(() =>
                {
                    var temp = value;
                });
            }
        }

        public Action TextChanged
        {
            get
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        string.Format(NotImplementedExceptionMessage, GetType().Name, nameof(TextChanged))));
                return null;
            }
            set
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        string.Format(NotImplementedExceptionMessage, GetType().Name, nameof(TextChanged))));

                var temp = value;
            }
        }

        public Action TextFocus
        {
            get
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        string.Format(NotImplementedExceptionMessage, GetType().Name, nameof(TextChanged))));
                return null;
            }
            set
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        string.Format(NotImplementedExceptionMessage, GetType().Name, nameof(TextChanged))));

                var temp = value;
            }
        }

        public CALayer Layer { get => _labelView.Layer; }

        public bool IsSecure { get => false; set => throw new NotImplementedException(); }

        public bool IsEmail { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void SetCursorColor(UIColor nativeCursorColor)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                _labelView.TintColor = nativeCursorColor;
            });
        }

        public void SetCursorNext(UITextFieldWrapper controllerNext, Action actionGo)
        {
            throw new NotImplementedException();
        }

        public void SetFont(string fontType, float size = 17)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                _labelView?.SetFont(fontType, size);
            });
        }

        public void SetHintColor(UIColor nativeTextColor)
        {
            ExceptionLogger.RaiseNonFatalException(
                new ExceptionWithCustomStack(
                    string.Format(NotImplementedExceptionMessage, GetType().Name, nameof(SetHintColor))));
        }

        public void SetTextColor(UIColor nativeTextColor)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                if (_labelView != null)
                    _labelView.TextColor = nativeTextColor;
            });
        }
    }
}