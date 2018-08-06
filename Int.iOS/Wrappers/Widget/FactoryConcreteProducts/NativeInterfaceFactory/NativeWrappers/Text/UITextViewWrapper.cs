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
    public class UITextViewWrapper : INativeTextViewWrapper
    {
        private const string NotImplementedExceptionMessage = "{0}.{1} not implemented";
        private readonly UITextView _textView;
        private UIColor _hintColor;

        public UITextViewWrapper(UITextView textView)
        {
            _textView = textView ?? throw new CrossWidgetWrapperNullReferenceException(nameof(textView));
            AssignClickEvents();
        }

        public Action TextChanged { get; set; }
        public Action TextFocus { get; set; }


        public string Text
        {
            get => TextView?.Text;
            set
            {
                if (TextView != null)
                    TextView.Text = value;
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
            set => TextView.SetPlaceholder(value, _hintColor ?? TextView.TextColor);
        }

        public void SetFont(string fontType, float size = 17)
        {
            TextView?.SetFont(fontType, size);
        }

        public bool IsEmail
        {
            get => TextView.KeyboardType == UIKeyboardType.EmailAddress;

            set
            {
                if (value == true)
                    TextView.KeyboardType = UIKeyboardType.EmailAddress;
                else
                    TextView.KeyboardType = UIKeyboardType.Default;
            }
        }

        public void SetHintColor(UIColor nativeTextColor)
        {
            _hintColor = nativeTextColor;

            ExceptionLogger.RaiseNonFatalException(
                new ExceptionWithCustomStack(
                    string.Format(NotImplementedExceptionMessage, GetType().Name, nameof(SetHintColor))));
        }

        public void SetTextColor(UIColor nativeTextColor)
        {
            if (TextView != null)
                TextView.TextColor = nativeTextColor;
        }

        private void AssignClickEvents()
        {
            TextView.Changed -= _textView_Changed;
            TextView.Changed += _textView_Changed;

        }

        private void _textView_Changed(object sender, EventArgs e)
        {
            InnerTextChangeHandler();
        }

        private bool InnerTextChangeHandler()
        {
            TextChanged?.Invoke();

            return true;
        }

        public CALayer Layer { get => TextView.Layer; }

        public bool IsSecure { get => TextView.SecureTextEntry; set => TextView.SecureTextEntry = value; }

        public UITextView TextView => _textView;

        public void SetCursorColor(UIColor nativeCursorColor)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                TextView.TintColor = nativeCursorColor;
            });
        }

        public void SetCursorNext(UITextFieldWrapper controllerNext, Action actionGo)
        {
            TextView.Delegate = new UITextViewCustom(this);
        }

        public class UITextViewCustom : UITextViewDelegate
        {
            private readonly UITextViewWrapper _wapper;

            public UITextViewCustom(UITextViewWrapper focus)
            {
                _wapper = focus;
            }

            public override bool ShouldBeginEditing(UITextView textView)
            {
                _wapper.TextFocus?.Invoke();
                return true;
            }
        }
    }
}