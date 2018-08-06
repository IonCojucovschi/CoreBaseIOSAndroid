using Android.Support.V4.View;
using Android.Views;

namespace Int.Droid.Transformations
{
    public class ParallaxPageTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        private readonly int _id;

        public ParallaxPageTransformer(int id)
        {
            _id = id;
        }

        #region IPageTransformer implementation

        public void TransformPage(View page, float position)
        {
            var width = page.Width;
            var paraView = page.FindViewById<View>(_id);
            if (paraView == null)
                return;
            if (position > 1 && position < -1)
            {
                page.Alpha = 1;
                return;
            }
            paraView.TranslationX = -(position * (width / 2));
        }

        #endregion

    }
}

