using System;
using Android.Support.V4.View;
using Android.Views;

namespace Int.Droid.Transformations
{
    public class FadeTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        private const float MinAlpha = 0.3f;

        public void TransformPage(View view, float position)
        {
            if (position < -1 || position > 1)
            {
                view.Alpha = 0;
                return;
            }
            var scale = 1 - Math.Abs(position);
            var alpha = MinAlpha + (1 - MinAlpha) * scale;
            view.Alpha = alpha;
        }

    }
}

