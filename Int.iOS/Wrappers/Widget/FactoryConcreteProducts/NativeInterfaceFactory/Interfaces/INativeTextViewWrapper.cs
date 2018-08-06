using System;
using Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory.NativeWrappers.Text;
using UIKit;

namespace Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory.Interfaces
{
    public interface INativeTextViewWrapper : INativeViewWrapper
    {
        bool IsSecure { get; set; }

        bool IsEmail { get; set; }

        string Text { get; set; }
        string Hint { get; set; }

        Action TextChanged { get; set; }
        Action TextFocus { get; set; }

        void SetTextColor(UIColor nativeTextColor);
        void SetHintColor(UIColor nativeTextColor);
        void SetFont(string fontType, float sizeFont = 17);

        void SetCursorColor(UIColor nativeCursorColor);

        void SetCursorNext(UITextFieldWrapper controllerNext, Action actionGo);
    }
}