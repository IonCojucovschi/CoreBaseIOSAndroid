using Android.Support.V4.View;
using Android.Views;
using Java.Lang;
using Math = System.Math;

namespace Int.Droid.Controllers.PagerAnimated.PageTransformers
{
    public class RevealingPageTransformer : Object, ViewPager.IPageTransformer
    {
        private const float UsualScale = 1.0f;

        private const int HsvValueMin = 0;
        private const int HsvValueIndex = 2;
        private const int HsvVarCount = 3;

        private readonly float _darkenMultiplier;
        private readonly float _positionOffset;
        private readonly float _viewShrinkScale;

        public RevealingPageTransformer(float darkenMultiplier, float shrinkedViewScale, float postionOffset)
        {
            _darkenMultiplier = darkenMultiplier;
            _viewShrinkScale = shrinkedViewScale;
            _positionOffset = postionOffset;
        }

        public void TransformPage(View page, float position)
        {
            if (page == null) return;
            var scaleFactor = Math.Abs(position - _positionOffset);
            page.ScaleY = UsualScale - (UsualScale - _viewShrinkScale) * scaleFactor;
            if (page.GetType() != typeof(CustomPagerView)) return;
            var customPagerView = (CustomPagerView) page;
            customPagerView.ForegroundTint.Alpha = _darkenMultiplier * scaleFactor;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}