using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract;
using Int.Core.Extensions;
using Int.Core.Wrappers;
using Int.Core.Wrappers.Widget.Exceptions;
using Int.iOS.Extensions;
using UIKit;

namespace Int.iOS.Wrappers.Widget.FactoryConcreteProducts
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

        protected readonly UIView WrapedObject;
        private UIGestureRecognizer _lastLongPressGestureRecognizer;
        private UIGestureRecognizer _lastTapGestureRecognizer;
        private readonly IList<UIGestureRecognizer> _lastSwipeRecognizerList = new List<UIGestureRecognizer>();

        public CrossViewWrapper(UIView view)
        {
            if (view.IsNull()) throw new CrossWidgetWrapperConstructorException(WrappedObjectIsNull);

            WrapedObject = view;
            AddTouch();
        }

        public object Controller { get; set; }

        protected string ColorSelected { get; private set; }
        protected string ColorOriginal { get; private set; }

        public bool Enabled
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        private bool ViewPress { get; set; }

        public object Tag { get; set; }

        public ViewState Visibility
        {
            get => (WrapedObject?.Hidden ?? true) ? ViewState.Invisible : ViewState.Visible;
            set
            {
                if (WrapedObject == null) return;

                switch (value)
                {
                    case (ViewState.Gone):
                        WrapedObject?.SetViewState(Extensions.Extensions.ViewState.Gone);
                        break;
                    case (ViewState.Invisible):
                        WrapedObject?.SetViewState(Extensions.Extensions.ViewState.Invisible);
                        break;
                    case (ViewState.Visible):
                        WrapedObject?.SetViewState(Extensions.Extensions.ViewState.Visible);
                        break;
                }
            }
        }


        public void SetBackgroundColor(string color, float? radiusAspect = null,
            string borderColor = null,
            float? borderWidth = null,
            RadiusType type = RadiusType.Static, string selectedColorView = "")
        {
            if (WrapedObject == null)
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

            AppTools.InvokeOnMainThread(() =>
            {
                ColorOriginal = color;
                WrapedObject.BackgroundColor = ColorOriginal.FromHex();

                if (borderWidth != null)
                {
                    if (string.IsNullOrEmpty(color))
                    {
                        ExceptionLogger.RaiseNonFatalException(
                            new ExceptionWithCustomStack(
                                EmptyBorderColorError,
                                Environment.StackTrace));

                        borderColor = TransparentColor;
                    }

                    WrapedObject?.RoundViewWithBorder(borderColor.FromHex().CGColor, borderWidth.Value, radiusAspect, type);
                }
                else if (radiusAspect != null)
                {
                    if (type == RadiusType.Static)
                        WrapedObject?.SetRoundView(radiusAspect ?? 0.5f);
                    else
                        WrapedObject?.RoundView(radiusAspect);
                }
                else
                {
                    if (type == RadiusType.Aspect)
                        WrapedObject?.RoundView(radiusAspect);
                }
            });
        }

        public void SetSelectedColor(string color)
        {
            ColorSelected = color;
        }

        public virtual void OnTouchView(State state)
        {
            switch (state)
            {
                case State.Began:
                case State.MoveIn:
                    SelectorBackroundView(true);
                    //selected
                    break;
                case State.MoveOut:
                case State.Cancel:
                    SelectorBackroundView(false);
                    break;
                //original => default
                default:
                    SelectorBackroundView(false);
                    //original
                    break;
            }
        }

        public virtual event EventHandler Click
        {
            add
            {
                _click += value;
                AssignGestureEventHandlers();
            }
            remove => _click -= value;
        }

        public virtual event EventHandler LongClick
        {
            add
            {
                _longClick += value;
                AssignGestureEventHandlers();
            }
            remove => _longClick -= value;
        }

        public virtual event SwipeEventHandler Swipe
        {
            add
            {
                _swipe += value;
                AssignGestureEventHandlers();
            }
            remove => _swipe -= value;
        }

        protected virtual void AssignGestureEventHandlers()
        {
            AppTools.InvokeOnMainThread(() =>
            {
                if (_click != null)
                {
                    if (_lastTapGestureRecognizer != null)
                        WrapedObject?.RemoveGestureRecognizer(_lastTapGestureRecognizer);

                    _lastTapGestureRecognizer = WrapedObject?.OnClick(() => _click?.Invoke(this, null));
                }

                if (_longClick != null)
                {
                    if (_lastLongPressGestureRecognizer != null)
                        WrapedObject?.RemoveGestureRecognizer(_lastLongPressGestureRecognizer);

                    _lastLongPressGestureRecognizer = WrapedObject?.OnLongClick(() =>
                    {
                        _longClick?.Invoke(this, null);

                        AppTools.InvokeOnMainThread(() => { OnTouchView(State.Cancel); });
                    });
                }

                if (_swipe == null) return;
                if (_lastSwipeRecognizerList != null)
                {
                    foreach (var recognizer in _lastSwipeRecognizerList)
                        WrapedObject?.RemoveGestureRecognizer(recognizer);
                    _lastSwipeRecognizerList.Clear();
                }
                else
                    throw new Exception("_lastSwipeRecognizerList should not be null");

                _lastSwipeRecognizerList?.Add(AssignSwipeGesture(GestureDirection.ToRight));
                _lastSwipeRecognizerList?.Add(AssignSwipeGesture(GestureDirection.ToLeft));
                _lastSwipeRecognizerList?.Add(AssignSwipeGesture(GestureDirection.ToUp));
                _lastSwipeRecognizerList?.Add(AssignSwipeGesture(GestureDirection.ToDown));

            });
        }

        private UISwipeGestureRecognizer AssignSwipeGesture(GestureDirection direction)
        {
            var swipeRecognizer = new UISwipeGestureRecognizer(() => _swipe(this, new SwipeEventArgs(direction)));

            switch (direction)
            {
                case GestureDirection.ToRight:
                    swipeRecognizer.Direction = UISwipeGestureRecognizerDirection.Right;
                    break;
                case GestureDirection.ToLeft:
                    swipeRecognizer.Direction = UISwipeGestureRecognizerDirection.Left;
                    break;
                case GestureDirection.ToUp:
                    swipeRecognizer.Direction = UISwipeGestureRecognizerDirection.Up;
                    break;
                case GestureDirection.ToDown:
                    swipeRecognizer.Direction = UISwipeGestureRecognizerDirection.Down;
                    break;
            }

            WrapedObject?.AddGestureRecognizer(swipeRecognizer);

            return swipeRecognizer;
        }

        private void AddTouch()
        {
            if (WrapedObject.IsNull()) return;

            var tapGesture = new ClickGestureRecognizer(WrapedObject);

            tapGesture.StateView -= StatViewTouch;
            tapGesture.StateView += StatViewTouch;

            AppTools.InvokeOnMainThread(() => { WrapedObject.AddGestureRecognizer(tapGesture); });
        }

        private void StatViewTouch(object obj, State state)
        {
            OnTouchView(state);

            if (ColorSelected.IsNullOrWhiteSpace()) return;
        }

        private void SelectorBackroundView(bool invetColor)
        {
            if (ColorSelected.IsNullOrWhiteSpace()) return;
            WrapedObject.BackgroundColor = invetColor ? ColorSelected?.FromHex() : ColorOriginal?.FromHex();
        }

        public void SetCornerRadius(float radius, DrawViewAngle viewCorner)
        {
            switch (viewCorner)
            {
                case DrawViewAngle.ToLeft:
                case DrawViewAngle.ToRight:
                    WrapedObject.SetCornerRadius(new CGSize(radius, radius),
                        UIRectCorner.TopLeft | UIRectCorner.TopRight);
                    break;
                case DrawViewAngle.ToBottomLeft:
                case DrawViewAngle.ToBottomRight:
                    WrapedObject.SetCornerRadius(new CGSize(radius, radius),
                        UIRectCorner.BottomLeft | UIRectCorner.BottomRight);
                    break;
            }
        }

        public void AddView(IView view)
        {
            WrapedObject.AddSubview(view as UIView);
        }

        private event EventHandler _click;

        private event EventHandler _longClick;

        private event SwipeEventHandler _swipe;
    }

    public class ClickGestureRecognizer : UIGestureRecognizer
    {
        public ClickGestureRecognizer(UIView view)
        {
            ViewConcrete = view;
        }

        public UIView ViewConcrete { get; set; }

        public event EventHandler<State> StateView;

        #region Private Variables

        private CGPoint _midpoint = CGPoint.Empty;
        private bool _strokeUp;

        #endregion

        #region Override Methods

        /// <summary>
        ///     Called when the touches end or the recognizer state fails
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            _strokeUp = false;
            _midpoint = CGPoint.Empty;
        }

        /// <summary>
        ///     Is called when the fingers touch the screen.
        /// </summary>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (touches.Count != 1)
            {
                State = UIGestureRecognizerState.Failed;
                StateView?.Invoke(this, Core.Wrappers.State.Ended);
            }

            StateView?.Invoke(this, Core.Wrappers.State.Began);
        }

        /// <summary>
        ///     Called when the touches are cancelled due to a phone call, etc.
        /// </summary>
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            // we fail the recognizer so that there isn't unexpected behavior
            // if the application comes back into view
            StateView?.Invoke(this, Core.Wrappers.State.Ended);
            State = UIGestureRecognizerState.Failed;
        }

        /// <summary>
        ///     Called when the fingers lift off the screen
        /// </summary>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            //
            if (State == UIGestureRecognizerState.Possible && _strokeUp)
            {
                State = UIGestureRecognizerState.Recognized;
                StateView?.Invoke(this, Core.Wrappers.State.Began);
            }

            StateView?.Invoke(this, Core.Wrappers.State.Ended);
        }

        /// <summary>
        ///     Called when the fingers move
        /// </summary>
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            var t = touches.AnyObject as UITouch;

            StateView?.Invoke(this,
                ViewConcrete.Bounds.Contains(t.LocationInView(ViewConcrete))
                    ? Core.Wrappers.State.MoveIn
                    : Core.Wrappers.State.MoveOut);
        }

        #endregion
    }
}