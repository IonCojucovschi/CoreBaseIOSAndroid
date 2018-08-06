using System;
using Android.Support.V4.View;
using Android.Views;

namespace Int.Droid.Transformations
{
    public class SinkAndSlideTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        public void TransformPage(View view, float position)
        {
            if (position < -1 || position > 1)
            {
                view.Alpha = 0;
                return;
            }

            view.Alpha = 1;
            if (position < 0)
            {
                view.ScaleX = 1 - Math.Abs(position);
                view.ScaleY = 1 - Math.Abs(position);
            }
        }
    }
}

