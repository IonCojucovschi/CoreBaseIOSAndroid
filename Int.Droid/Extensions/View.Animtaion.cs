using Android.Views;
using Android.Views.Animations;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        public static void AnimateShake(this View view, int shakeDeltaPx = 5, int timesToRepeat = 2)
        {
            var animationStepDuration = 50;

            var shakeAnimationStart = new TranslateAnimation(0, -shakeDeltaPx, 0, 0)
            {
                Duration = animationStepDuration / 2
            };
            var shakeAnimationMiddleToRight = new TranslateAnimation(-shakeDeltaPx, 2 * shakeDeltaPx, 0, 0)
            {
                Duration = animationStepDuration
            };
            var shakeAnimationMiddleToLeft = new TranslateAnimation(shakeDeltaPx, -2 * shakeDeltaPx, 0, 0)
            {
                Duration = animationStepDuration
            };
            var shakeAnimationEnd = new TranslateAnimation(-shakeDeltaPx, shakeDeltaPx, 0, 0)
            {
                Duration = animationStepDuration / 2
            };

            shakeAnimationStart.AnimationEnd += (sender, e) =>
                view.StartAnimation(shakeAnimationMiddleToRight);
            shakeAnimationMiddleToRight.AnimationEnd += (sender, e) =>
                view.StartAnimation(shakeAnimationMiddleToLeft);
            shakeAnimationMiddleToLeft.AnimationEnd += (sender, e) =>
            {
                if (--timesToRepeat > 0)
                    view.StartAnimation(shakeAnimationMiddleToLeft);
                else
                    view.StartAnimation(shakeAnimationEnd);
            };
            shakeAnimationEnd.AnimationEnd += (sender, e) =>
                view.ClearAnimation();

            view.StartAnimation(shakeAnimationStart);
        }
    }
}