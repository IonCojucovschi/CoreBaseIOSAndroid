using System;
using CoreAnimation;
using Foundation;
using Int.Core.Wrappers.Widget.Exceptions;
using Int.iOS.Extensions;
using Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory.Interfaces;
using UIKit;

namespace Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory.NativeWrappers.Text
{
    public class UITextFieldWrapper : INativeTextViewWrapper
    {
        private readonly UITextField _textField;
        private bool _hintColorSetted;
        private UIColor _hintColor;

        public UITextFieldWrapper(UITextField textField)
        {
            _textField = textField ?? throw new CrossWidgetWrapperNullReferenceException(nameof(textField));
            AssignClickEvents();
        }

        public Action TextChanged { get; set; }
        public Action TextFocus { get; set; }


        public string Text
        {
            get => TextField?.Text;
            set
            {
                if (TextField != null)
                    TextField.Text = value;
            }
        }

        public string Hint
        {
            get => TextField?.Placeholder;
            set
            {
                if (TextField == null) return;

                TextField.Placeholder = value;

                if (_hintColorSetted)
                    SetHintColor(_hintColor);
            }
        }

        public void SetFont(string fontType, float size = 17)
        {
            TextField?.SetFont(fontType, size);
        }

        public void SetHintColor(UIColor nativeTextColor)
        {
            _hintColorSetted = true;
            _hintColor = nativeTextColor;
            TextField.SetPlaceholderColor(nativeTextColor);
        }

        public void SetTextColor(UIColor nativeTextColor)
        {
            if (TextField != null)
                TextField.TextColor = nativeTextColor;
        }

        private void AssignClickEvents()
        {
            TextField.EditingChanged -= InnerTextChangeHandler;
            TextField.EditingChanged += InnerTextChangeHandler;

        }

        private void InnerTextChangeHandler(object sender, EventArgs e)
        {
            TextChanged?.Invoke();
        }

        public CALayer Layer { get => TextField.Layer; }

        public bool IsSecure { get => TextField.SecureTextEntry; set => TextField.SecureTextEntry = value; }

        public bool IsEmail
        {
            get => TextField.KeyboardType == UIKeyboardType.EmailAddress;

            set
            {
                if (value == true)
                    TextField.KeyboardType = UIKeyboardType.EmailAddress;
                else
                    TextField.KeyboardType = UIKeyboardType.Default;
            }
        }

        public UITextField TextField => _textField;

        public void SetCursorColor(UIColor nativeCursorColor)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                TextField.TintColor = nativeCursorColor;
            });
        }

        public void SetCursorNext(UITextFieldWrapper controllerNext, Action actionGo)
        {
            TextField.Delegate = new UITextViewCustom(this, controllerNext, actionGo);
        }

        public class UITextViewCustom : UITextFieldDelegate
        {
            protected readonly UITextFieldWrapper _wrapper;
            private Action _actionGo;
            private UITextFieldWrapper _actionNext;

            public UITextViewCustom(UITextFieldWrapper focus, UITextFieldWrapper next = null, Action go = null)
            {
                _wrapper = focus;

                _actionGo = go;
                _actionNext = next;
            }

            [Export("textFieldShouldReturn:")]
            public bool ShouldReturn(UITextField textField)
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
                        _actionGo?.Invoke();
                        break;
                    case UIReturnKeyType.Google:
                        break;
                    case UIReturnKeyType.Join:
                        break;
                    case UIReturnKeyType.Next:
                        _actionNext.TextField?.BecomeFirstResponder();
                        break;
                    case UIReturnKeyType.Search:
                        textField?.ResignFirstResponder();
                        _actionGo?.Invoke();
                        break;
                }
                return true;
            }

            public override bool ShouldBeginEditing(UITextField textField)
            {
                _wrapper.TextFocus?.Invoke();
                return true;
            }
        }
    }
}