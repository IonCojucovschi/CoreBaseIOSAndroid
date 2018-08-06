using Android.Support.V4.View;
using Android.Views;

namespace Int.Droid.Transformations
{
    public class WheelPageTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        private const float MaxAngle = 30f;

        public void TransformPage(View view, float position)
        {
            if (position < -1 || position > 1)
            {
                view.Alpha = 0;
                return;
            }
            view.Alpha = 1;
            view.PivotY = view.Height / 2;
            view.PivotX = position < 0 ? view.Width : 0;
            view.RotationX = MaxAngle * position;
        }
    }
}

