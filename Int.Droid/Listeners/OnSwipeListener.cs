using System;
using Android.Content;
using Android.Views;

namespace Int.Droid.Listeners
{
    public class OnSwipeListener : GestureDetector.SimpleOnGestureListener, View.IOnTouchListener
    {
        private const int SwipeThreshold = 100;
        private const int SwipeVelocityThreshold = 100;
        private readonly GestureDetector _gestureDetector;

        public Action OnSwipeBottom;

        public Action OnSwipeLeft;

        public Action OnSwipeRight;

        public Action OnSwipeTop;

        public OnSwipeListener(Context context)
        {
            _gestureDetector = new GestureDetector(context, this);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return _gestureDetector.OnTouchEvent(e);
        }

        public override bool OnDown(MotionEvent e)
        {
            return true;
        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            var result = false;

            try
            {
                var diffX = e2.GetX() - e1.GetX();
                var diffY = e2.GetY() - e1.GetY();
                if (Math.Abs(diffX) > Math.Abs(diffY))
                {
                    if (Math.Abs(diffX) > SwipeThreshold && Math.Abs(velocityX) > SwipeVelocityThreshold)
                    {
                        if (diffX > 0)
                            OnSwipeRight?.Invoke();
                        else
                            OnSwipeLeft?.Invoke();
                        result = true;
                    }
                }
                else if (Math.Abs(diffY) > SwipeThreshold && Math.Abs(velocityY) > SwipeVelocityThreshold)
                {
                    if (diffY > 0)
                        OnSwipeBottom?.Invoke();
                    else
                        OnSwipeTop?.Invoke();
                    result = true;
                }
            }
            catch
            {
                Console.WriteLine("Exception in SwipeListener");
            }

            return result;
        }
    }
}