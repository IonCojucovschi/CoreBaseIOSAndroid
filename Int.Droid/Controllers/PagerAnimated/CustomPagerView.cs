using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Int.Droid.Controllers.PagerAnimated
{
    public class CustomPagerView : FrameLayout
    {
        public View BackgroundLayer { get; private set; }
        public View ForegroundTint { get; private set; }

        public void SetBackgroundLayer(View view)
        {
            BackgroundLayer = view;
            AddView(view, 0);
        }

        public void SetForegroundTint(View view)
        {
            ForegroundTint = view;
            AddView(view);
        }

        #region Constructors

        public CustomPagerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public CustomPagerView(Context context) : base(context)
        {
        }

        public CustomPagerView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomPagerView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public CustomPagerView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        #endregion
    }
}