using Int.Core.Application.Widget.Contract;
using Int.Core.Wrappers.Widget.Exceptions;
using UIKit;

namespace Int.iOS.Wrappers.Widget.FactoryConcreteProductsConcrete
{
    public class CrossTextWrapper : CrossViewWrapper, IText
    {
        public CrossTextWrapper(UIView textView) : base(textView)
        {
            if (!(textView is UILabel || textView is UITextField || textView is UITextView))
                throw new CrossWidgetWrapperConstructorException(string.Format(NotCompatibleError, textView?.GetType()));
        }

        public string Text
        {
            get
            {
                var o = WrapedObject as UILabel;
                if (o != null)
                    return o.Text;

                var field = WrapedObject as UITextField;
                if (field != null)
                    return field.Text;

                var view = WrapedObject as UITextView;
                return view?.Text;
            }
            set
            {
                var o = WrapedObject as UILabel;
                if (o != null)
                    o.Text = value;

                var field = WrapedObject as UITextField;
                if (field != null)
                    field.Text = value;

                var view = WrapedObject as UITextView;
                if (view != null)
                    view.Text = value;
            }
        }
    }
}