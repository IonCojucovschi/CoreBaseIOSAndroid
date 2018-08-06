using Android.Views;
using Android.Widget;
using Int.Core.Application.Widget.Contract;
using Int.Core.Wrappers.Widget.Exceptions;

namespace Int.Droid.Wrappers.Widget.FactoryConcreteProductsConcrete
{
    public class CrossTextWrapper : CrossViewWrapper, IText
    {
        public CrossTextWrapper(View textView) : base(textView)
        {
            if (!(textView is TextView))
                throw new CrossWidgetWrapperConstructorException(string.Format(NotCompatibleError, textView?.GetType()));
        }

        public string Text
        {
            get => (WrapedObject as TextView)?.Text;
            set
            {
                var o = WrapedObject as TextView;
                if (o != null)
                    o.Text = value;
            }
        }
    }
}