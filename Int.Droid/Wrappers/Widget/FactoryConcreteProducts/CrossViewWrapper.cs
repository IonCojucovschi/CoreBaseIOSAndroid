using System;
using System.Collections.Generic;
using System.Timers;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract;
using Int.Core.Extensions;
using Int.Core.Wrappers;
using Int.Core.Wrappers.Widget.Exceptions;
using Int.Droid.Extensions;
using Int.Droid.Views;

namespace Int.Droid.Wrappers.Widget.FactoryConcreteProducts
{
    public class CrossViewWrapper : IView
    {
        private const string EmptyColorError = "Passed color is null or empty. Setted tranparent by default";

        private const string EmptyBorderColorError =
            "Passed border color is null or empty. Setted tranparent by default";

        private const string TransparentColor = "#00000000";
        protected const string NotCompatibleError = "{0} is not compatible";
        protected const string WrappedObjectIsNull = "Wrapped object is null";
        protected const string AdapterIsNull = "Could not retrieve adapter";
        protected string Url { get; set; }
        protected int DrawableId { get; set; }

        private const int LongClickTimeout = 1000;
        private Timer _longClickTimer;
        private float? _measuredHeight;

        protected View WrappedObject;

        public object Controller { get; set; }


        public CrossViewWrapper(View view)
        {
            if (view.IsNull())
                throw new CrossWidgetWrapperConstructorException(string.Format(NotCompatibleError, "null"));
            WrappedObject = view;

            Controller = view;
        }

        protected string ColorOriginalView { get; set; }

        protected string ColorOriginalV2View { get; set; }

        protected string ColorSelectedView { get; set; }
        protected string ColorSelectedChild { get; set; }

        private float? RadiusAspect { get; set; }
        private RadiusType RadiusValueType { get; set; }

        private string BorderColor { get; set; }
        private float? BorderWidth { get; set; }

        protected Color ColorSelectedViewAndroid => ColorSelectedChild.ToColor();

        public object Tag { get; set; }

        public View.IOnTouchListener AssignedTouchListener { get; set; }

        public ViewState Visibility
        {
            get
            {
                switch (WrappedObject?.Visibility)
                {
                    case (ViewStates.Gone):
                        return ViewState.Gone;
                    case (ViewStates.Invisible):
                        return ViewState.Invisible;
                    case (ViewStates.Visible):
                        return ViewState.Visible;
                    default:
                        return ViewState.Gone;
                }
            }
            set
            {
                if (WrappedObject == null) return;

                AppTools.CurrentActivity.RunOnUiThread(() =>
                {
                    switch (value)
                    {
                        case (ViewState.Gone):
                            WrappedObject.Visibility = ViewStates.Gone;
                            break;
                        case (ViewState.Invisible):
                            WrappedObject.Visibility = ViewStates.Invisible;
                            break;
                        case (ViewState.Visible):
                            WrappedObject.Visibility = ViewStates.Visible;
                            break;
                    }
                });
            }
        }


        public void SetBackgroundColor(string color, float? radiusAspect = null,
            string borderColor = null,
            float? borderWidth = null,
            RadiusType type = RadiusType.Static, string selectedColorView = "")
        {
            if (WrappedObject == null)
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        WrappedObjectIsNull, Environment.StackTrace));
                return;
            }

            if (string.IsNullOrEmpty(color))
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        EmptyColorError,
                        Environment.StackTrace));

                color = TransparentColor;
            }

            if (selectedColorView.IsNullOrWhiteSpace())
            {
                ColorOriginalView = color;
                ColorOriginalV2View = ColorOriginalView;
            }
            else
            {
                ColorOriginalV2View = selectedColorView;
            }

            BorderColor = borderColor;
            BorderWidth = borderWidth;
            RadiusValueType = type;
            RadiusAspect = radiusAspect;

            if (RadiusAspect != null && WrappedObject is ImageView)
            {
                WrappedObject = (FFImageLoading.Views.ImageViewAsync)WrappedObject;

                if (radiusAspect == null) return;
                var b = new FFImageLoading.Transformations.CornersTransformation((double)radiusAspect, FFImageLoading.Transformations.CornerTransformType.AllRounded, 0, 0);

                if (!Url.IsNullOrWhiteSpace())
                {

                    var a = FFImageLoading.ImageService.Instance
                        .LoadUrl(Url)
                        .Transform(new List<FFImageLoading.Work.ITransformation>() { b });
                    FFImageLoading.TaskParameterPlatformExtensions.Into(a, WrappedObject as FFImageLoading.Views.ImageViewAsync);

                }
                else if (DrawableId > 0)
                {
                    var a = FFImageLoading.ImageService.Instance
                        .LoadCompiledResource(DrawableId.ToString())
                        .Transform(b);

                    FFImageLoading.TaskParameterPlatformExtensions.Into(a, WrappedObject as FFImageLoading.Views.ImageViewAsync);
                }
            }
            else if (RadiusAspect != null && borderWidth != null)
            {
                if (string.IsNullOrEmpty(color))
                {
                    ExceptionLogger.RaiseNonFatalException(
                        new ExceptionWithCustomStack(
                            EmptyBorderColorError,
                            Environment.StackTrace));

                    borderColor = TransparentColor;
                }

                switch (type)
                {
                    case RadiusType.Aspect:
                        InvokeWithMeasuredHeight(height =>
                            SetRoundView(WrappedObject, ColorOriginalV2View.ToColor(), height * RadiusAspect.Value,
                                borderColor.ToColor(), borderWidth.Value));
                        break;
                    case RadiusType.Static:
                        SetRoundView(WrappedObject, ColorOriginalV2View.ToColor(),
                            RadiusAspect.Value.Iphone6PointsToDevicePixels(), borderColor.ToColor(), borderWidth.Value);
                        break;
                }
            }
            else if (RadiusAspect != null)
            {
                switch (type)
                {
                    case RadiusType.Aspect:
                        InvokeWithMeasuredHeight(height =>
                            SetRoundView(WrappedObject, ColorOriginalV2View.ToColor(), height * RadiusAspect.Value));
                        break;
                    case RadiusType.Static:
                        SetRoundView(WrappedObject, ColorOriginalV2View.ToColor(),
                            RadiusAspect.Value.Iphone6PointsToDevicePixels());
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case RadiusType.Aspect:

                        if (!borderColor.IsNull())
                            InvokeWithMeasuredHeight(height =>
                            {
                                if (borderWidth != null)
                                    SetRoundView(WrappedObject, ColorOriginalV2View.ToColor(), height * 0.5f,
                                        borderColor?.ToColor(), borderWidth.Value);
                            });
                        else
                            InvokeWithMeasuredHeight(height =>
                                SetRoundView(WrappedObject, ColorOriginalV2View.ToColor(), height * 0.5f));
                        break;
                    default:

                        AppTools.CurrentActivity?.RunOnUiThread(() =>
                        {
                            WrappedObject.SetBackgroundColor(ColorOriginalV2View.ToColor());
                        });
                        break;
                }
            }
        }

        public virtual event EventHandler Click
        {
            add
            {
                _click += value;
                AssignTouchListeners();
            }
            remove => _click -= value;
        }

        public virtual event EventHandler LongClick
        {
            add
            {
                _longClick += value;
                AssignTouchListeners();
            }
            remove => _longClick -= value;
        }

        public virtual event SwipeEventHandler Swipe
        {
            add
            {
                _swipe += value;
                AssignTouchListeners();
            }
            remove => _swipe -= value;
        }

        public virtual void OnTouchView(State state)
        {
            switch (state)
            {
                case State.Began:
                case State.MoveIn:
                    SelectorBackroundView(true);
                    break;
                case State.Ended:
                    SelectorBackroundView(false);
                    InnerClickHandler();
                    break;
                default:
                    SelectorBackroundView(false);
                    break;
            }
        }

        public void SetSelectedColor(string color)
        {
            ColorSelectedView = color;
        }

        protected virtual void AssignTouchListeners()
        {
            if (WrappedObject == null)
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        WrappedObjectIsNull, Environment.StackTrace));

                return;
            }

            AssignedTouchListener = new TouchListener(Touch);
            WrappedObject.SetOnTouchListener(AssignedTouchListener);
        }

        private bool Touch(View sender, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _longClickTimer = _longClickTimer ?? new Timer(LongClickTimeout)
                    {
                        AutoReset = false
                    };

                    _longClickTimer.Elapsed -= ViewLongClickHandler;
                    _longClickTimer.Elapsed += ViewLongClickHandler;

                    _longClickTimer.Start();

                    OnTouchView(State.Began);
                    break;
                case MotionEventActions.Up:
                    if (IsTouchInsideView(sender, e) && (_longClickTimer?.Enabled ?? true))
                    {
                        _longClickTimer?.Stop();
                        OnTouchView(State.Ended);
                    }
                    else
                    {
                        OnTouchView(State.Cancel);
                    }
                    break;
                case MotionEventActions.Move:
                    if (IsTouchInsideView(sender, e))
                    {
                        OnTouchView(State.MoveIn);
                    }
                    else
                    {
                        _longClickTimer.Stop();
                        OnTouchView(State.MoveOut);
                    }
                    break;
                case MotionEventActions.Cancel:
                    OnTouchView(State.Cancel);
                    break;
            }

            gestureDetector = gestureDetector ?? new GestureDetector(new GestureListener(this) { SwipeHandler = _swipe });
            return (bool)gestureDetector?.OnTouchEvent(e);
        }

        private bool IsTouchInsideView(View view, MotionEvent motionEvent)
        {
            if (view == null || motionEvent == null) return false;

            var x = motionEvent.GetX();
            var y = motionEvent.GetY();
            var viewMaxX = view.MeasuredWidth;
            var viewMaxY = view.MeasuredHeight;

            return x >= 0 && x <= viewMaxX && y >= 0 && y <= viewMaxY;
        }

        protected void InnerClickHandler()
        {
            _click?.Invoke(this, new EventArgs());
        }

        protected void ViewLongClickHandler(object sender, ElapsedEventArgs e)
        {
            WrappedObject.RunOnUiThread(() =>
                OnTouchView(State.Cancel));
            _longClick?.Invoke(this, new EventArgs());
        }

        private void InvokeWithMeasuredHeight(Action<float> action)
        {
            if (_measuredHeight.HasValue)
            {
                action?.Invoke(_measuredHeight.Value);
                return;
            }

            if (WrappedObject?.LayoutParameters?.Height > 0)
            {
                action?.Invoke(WrappedObject.LayoutParameters.Height);
                return;
            }

            WrappedObject?.Post(() =>
            {
                _measuredHeight = WrappedObject.MeasuredHeight;

                action?.Invoke(_measuredHeight.Value);
            });
        }


        private void CopyMeasuredSizeToLayoutParams(View view)
        {
            ViewGroup.LayoutParams newLayoutParams = null;

            switch (view.LayoutParameters)
            {
                case GridLayout.LayoutParams _:
                    newLayoutParams = new GridLayout.LayoutParams(view.LayoutParameters);
                    break;
                case LinearLayout.LayoutParams _:
                    newLayoutParams = new LinearLayout.LayoutParams(view.LayoutParameters);
                    break;
                case RelativeLayout.LayoutParams _:
                    newLayoutParams = new RelativeLayout.LayoutParams(view.LayoutParameters);
                    break;
                case FrameLayout.LayoutParams _:
                    newLayoutParams = new FrameLayout.LayoutParams(view.LayoutParameters);
                    break;
                case ViewGroup.MarginLayoutParams _:
                    newLayoutParams = new ViewGroup.MarginLayoutParams(view.LayoutParameters);
                    break;
                case ViewGroup.LayoutParams _:
                    newLayoutParams = new ViewGroup.LayoutParams(view.LayoutParameters);
                    break;
            }

            if (newLayoutParams == null) return;

            newLayoutParams.Height = view.MeasuredHeight;
            newLayoutParams.Width = view.MeasuredWidth;

            view.LayoutParameters = newLayoutParams;
        }

        private event EventHandler _click;

        private event EventHandler _longClick;

        private event SwipeEventHandler _swipe;

        private void SetRoundView(View view, Color color, float cornerRadius,
            Color? borderColor = null,
            float? borderWidth = null)
        {
            var round = new RoundView
            {
                Radius = cornerRadius,
                Color = color
            };

            if (borderColor.HasValue)
                round.BorderColor = borderColor.Value;

            if (borderWidth.HasValue)
                round.LineWidth = borderWidth.Value;

            view.SetBackgroundCompat(round);
        }

        private void SelectorBackroundView(bool invetColor)
        {
            if (invetColor)
                SetBackgroundColor(ColorOriginalView, RadiusAspect, BorderColor, BorderWidth, RadiusValueType,
                    ColorSelectedView);
            else
                SetBackgroundColor(ColorOriginalView, RadiusAspect, BorderColor, BorderWidth, RadiusValueType);
        }

        public void AddView(IView view)
        {
            (WrappedObject as ViewGroup)?.AddView(view as View);
        }

        private class TouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            private readonly Func<View, MotionEvent, bool> _action;

            public TouchListener(Func<View, MotionEvent, bool> action)
            {
                _action = action;
            }

            public bool OnTouch(View v, MotionEvent e)
            {
                return _action?.Invoke(v, e) ?? false;
            }
        }

        private GestureDetector gestureDetector;
        private class GestureListener : GestureDetector.SimpleOnGestureListener
        {
            private const int SWIPE_DISTANCE_THRESHOLD = 100;
            private const int SWIPE_VELOCITY_THRESHOLD = 100;
            private readonly IView _viewWrapper;

            public SwipeEventHandler SwipeHandler { get; set; }

            public GestureListener(IView viewWrapper)
            {
                _viewWrapper = viewWrapper;
            }

            public override bool OnDown(MotionEvent e)
            {
                return true;
            }

            private void CallSwipe(GestureDirection direction)
            {
                SwipeHandler?.Invoke(_viewWrapper, new SwipeEventArgs(direction));
            }

            public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
                var result = false;
                try
                {
                    var diffY = e2.GetY() - e1.GetY();
                    var diffX = e2.GetX() - e1.GetX();
                    if (Math.Abs(diffX) > Math.Abs(diffY))
                    {
                        if (Math.Abs(diffX) > SWIPE_DISTANCE_THRESHOLD && Math.Abs(velocityX) > SWIPE_VELOCITY_THRESHOLD)
                        {
                            CallSwipe(diffX > 0 ? GestureDirection.ToRight : GestureDirection.ToLeft);
                            result = true;
                        }
                    }
                    else if (Math.Abs(diffY) > SWIPE_DISTANCE_THRESHOLD && Math.Abs(velocityY) > SWIPE_VELOCITY_THRESHOLD)
                    {
                        CallSwipe(diffY > 0 ? GestureDirection.ToDown : GestureDirection.ToUp);
                        result = true;
                    }
                }
                catch (Exception exception)
                {
                    ExceptionLogger.RaiseNonFatalException(exception);
                }

                return result;
            }
        }
    }
}