using Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory.Interfaces;
using Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory.NativeWrappers.Text;
using UIKit;

namespace Int.iOS.Wrappers.Widget.FactoryConcreteProducts.NativeInterfaceFactory
{
    public static class NativeWrapperFactory
    {
        public static INativeTextViewWrapper GetTextViewWrapper(UIView view)
        {
            switch (view)
            {
                case UILabel _:
                    return new UILabelWrapper(view as UILabel);
                case UITextField _:
                    return new UITextFieldWrapper(view as UITextField);
                case UITextView _:
                    return new UITextViewWrapper(view as UITextView);
            }

            return null;
        }
    }
}