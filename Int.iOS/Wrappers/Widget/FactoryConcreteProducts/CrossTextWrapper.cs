using System;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract;
using Int.Core.Extensions;
using Int.Core.Wrappers;
using Int.Core.Wrappers.Widget.Exceptions;
using Int.iOS.Extensions;
using Int.iOS.Wrappers.Text;
using Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory;
using Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory.Interfaces;
using Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory.NativeWrappers.Text;
using UIKit;

namespace Int.iOS.Wrappers.Widget.FactoryConcreteProducts
{
    public class CrossTextWrapper : CrossViewWrapper, IText
    {
        private const int Iphone6HeightDp = 667;
        private const string NullReferenceExceptionMessage = "{0} is null";
        private const string EmptyTextColorError = "Text color is null or empty";

        private readonly INativeTextViewWrapper _nativeTextViewWrapper;

        public CrossTextWrapper(UIView textView) : base(textView)
        {
            _nativeTextViewWrapper = NativeWrapperFactory.GetTextViewWrapper(textView);

            if (_nativeTextViewWrapper == null)
                throw new CrossWidgetWrapperConstructorException(string.Format(NotCompatibleError,
                    textView?.GetType()));

            Controller = textView;

            AssignTextChangeHandlers();
        }

        protected string ColorSelectedText { get; private set; }
        protected string ColorOriginalText { get; private set; }

        public Action<IText> TextChanged { get; set; }
        public Action Focus { get; set; }


        public string Text
        {
            get => _nativeTextViewWrapper?.Text;
            set
            {
                if (_nativeTextViewWrapper == null)
                    throw new CrossWidgetWrapperNullReferenceException(string.Format(NullReferenceExceptionMessage,
                        nameof(_nativeTextViewWrapper)));
                _nativeTextViewWrapper.Text = value;
            }
        }

        public string Hint
        {
            get => _nativeTextViewWrapper?.Hint;
            set
            {
                if (_nativeTextViewWrapper == null)
                    throw new CrossWidgetWrapperNullReferenceException(string.Format(NullReferenceExceptionMessage,
                        nameof(_nativeTextViewWrapper)));
                _nativeTextViewWrapper.Hint = value;
            }
        }


        public void SetFont(string fontType, float size = 16)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                var sclaedSize = size * UIScreen.MainScreen.Bounds.Height / Iphone6HeightDp;
                _nativeTextViewWrapper?.SetFont(fontType, (float)sclaedSize);
            });
        }

        public void SetHintColor(string textColor)
        {
            if (string.IsNullOrWhiteSpace(textColor))
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        EmptyTextColorError,
                        Environment.StackTrace));

                return;
            }

            var nativeTextColor = textColor.FromHex();

            _nativeTextViewWrapper.SetHintColor(nativeTextColor);
        }

        public void SetTextColor(string textColor)
        {
            if (string.IsNullOrWhiteSpace(textColor))
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        EmptyTextColorError,
                        Environment.StackTrace));

                return;
            }

            ColorOriginalText = textColor;
            var nativeTextColor = ColorOriginalText.FromHex();

            _nativeTextViewWrapper?.SetTextColor(nativeTextColor);
        }

        public void SetSelectedTextColor(string colorText)
        {
            ColorSelectedText = colorText;
        }

        public override void OnTouchView(State state)
        {
            base.OnTouchView(state);

            switch (state)
            {
                case State.Began:
                case State.MoveIn:

                    SelectorBackroundText(true);

                    //selected
                    break;
                case State.MoveOut:
                case State.Cancel:
                    SelectorBackroundText(false);
                    break;
                //original => default
                default:
                    SelectorBackroundText(false);
                    //original
                    break;
            }
        }

        private void AssignTextChangeHandlers()
        {
            if (_nativeTextViewWrapper is UILabelWrapper) return;

            _nativeTextViewWrapper.TextFocus -= InnerTextFocusHandler;
            _nativeTextViewWrapper.TextFocus += InnerTextFocusHandler;

            _nativeTextViewWrapper.TextChanged -= InnerTextChangeHandler;
            _nativeTextViewWrapper.TextChanged += InnerTextChangeHandler;
        }

        private void InnerTextChangeHandler()
        {
            TextChanged?.Invoke(this);
        }

        private void InnerTextFocusHandler()
        {
            Focus?.Invoke();
        }

        private void SelectorBackroundText(bool invetColor)
        {
            if (ColorSelectedText.IsNullOrWhiteSpace()) return;

            _nativeTextViewWrapper?.SetTextColor(invetColor
                ? ColorSelectedText?.FromHex()
                : ColorOriginalText?.FromHex());
        }

        public void SetShadowLayer(float radius, float dx, float dy, string color)
        {
            _nativeTextViewWrapper.Layer.ShadowRadius = radius;
            _nativeTextViewWrapper.Layer.ShadowOffset = new CoreGraphics.CGSize(dx, dy);
            _nativeTextViewWrapper.Layer.ShadowColor = color.FromHex().CGColor;

            _nativeTextViewWrapper.Layer.MasksToBounds = false;
        }

        public void SetSecure(InputType transformation = InputType.Text, object nextController = null, Action executeGo = null)
        {
            if (transformation == InputType.Password)
                _nativeTextViewWrapper.IsSecure = !_nativeTextViewWrapper.IsSecure;
            else _nativeTextViewWrapper.IsEmail |= transformation == InputType.Email;
        }

        public void SetCursorColor(string cursorColor)
        {
            _nativeTextViewWrapper.SetCursorColor(cursorColor.FromHex());
        }

        public void SetLinkAndStyle(string substring, string link, string style) { }

        public void SetLinkAndStyle(string[] substring, string[] link, string style)
        {
            var hiper = Controller as HyperLabel;
            hiper.SetLinkForSubstrings(substring, HandleLinkHandler);
        }

        private void HandleLinkHandler(HyperLabel label, Foundation.NSRange range) { }

        public void SetNextCursor(IText textNext = null, Action actionGo = null)
        {
            _nativeTextViewWrapper.SetCursorNext(new UITextFieldWrapper(textNext.Controller as UITextField), actionGo);
        }

        public bool IsSecure => _nativeTextViewWrapper.IsSecure;

    }
}