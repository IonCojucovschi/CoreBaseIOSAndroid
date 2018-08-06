using System;
using Android.Support.V4.View;
using Android.Views;

namespace Int.Droid.Transformations
{
    public class ScaleTranformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        private const float MinScale = 0.5f;

        public void TransformPage(View view, float position)
        {
            if (position < -1 || position > 1)
            {
                view.Alpha = 0;
                return;
            }
            view.Alpha = 1;

            var scale = 1 - Math.Abs(position) * (1 - MinScale);
            view.ScaleX = scale;
            view.ScaleY = scale;
            var xMargin = view.Width * (1 - scale) / 2;
            if (position < 0)
                view.TranslationX = xMargin / 2;
            else
                view.TranslationX = -xMargin / 2;

        }
    }
}

