//
// UIButtonView.cs
//
// Author:
//       Valentin <valentin.grigorean1@gmail.com>
//
// Copyright (c) 2016 Valentin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Foundation;
using Int.Core.Wrappers;
using UIKit;

namespace Int.iOS.Wrappers.Button
{
    public interface IButtonDelegate
    {
        void HandleState(UIView view, State state);
    }

    public enum ButtonState
    {
        Normal,
        Highlighted
    }

    /// <summary>
    ///     A class that highlight when is pressed using TintColor
    ///     Good to use when ur using view as a button
    /// </summary>
    [Register("UIButtonView")]
    [DesignTimeVisible(true)]
    public class UiButtonView : ClickableView, IButtonDelegate
    {
        public delegate void StateChangedCallback(UIView view, State state);

        private readonly Dictionary<ButtonState, TextState> _stateMap = new Dictionary<ButtonState, TextState>
        {
            {ButtonState.Normal, new TextState()},
            {ButtonState.Highlighted, new TextState()}
        };

        private float _cornerRadius;
        private UIColor _currentColor;

        private State _currentState;
        private readonly string _nameFont = "Helvetica";
        private bool _useHalfCorner;
        private bool _wasSetTint;

        public UiButtonView()
        {
            Initialize();
        }

        public UiButtonView(IntPtr ptr)
            : base(ptr)
        {
            Initialize();
        }

        public UiButtonView(CGRect frame)
            : base(frame)
        {
            Initialize();
        }

        public UiButtonView(NSCoder coder)
            : base(coder)
        {
            Initialize();
        }

        public UiButtonView(NSObjectFlag t)
            : base(t)
        {
            Initialize();
        }

        public bool SetToggle { get; set; }
        public bool ToggleOn { get; set; }

        public UILabel TitleLabel { get; private set; }

        public ButtonState StateButton { get; private set; }

        public bool ShouldHighlightSubviewsOnTouch { get; set; } = true;

        public IButtonDelegate Delegate { get; set; }

        public StateChangedCallback StateChanged { get; set; }

        [Export("CornerRadius")]
        [Browsable(true)]
        public virtual float CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                if (_useHalfCorner)
                    return;

                Layer.CornerRadius = _cornerRadius;
                Layer.MasksToBounds = true;
                SetNeedsDisplay();
            }
        }

        [Export("HalfHeighCorner")]
        [Browsable(true)]
        public bool HalfHeighCorner
        {
            get => _useHalfCorner;
            set
            {
                _useHalfCorner = value;
                Layer.CornerRadius = value ? Bounds.Height / 2f : _cornerRadius;
                Layer.MasksToBounds = true;
                SetNeedsDisplay();
            }
        }

        void IButtonDelegate.HandleState(UIView view, State state)
        {
            SetCurrentState();
            switch (state)
            {
                case State.Began:
                case State.MoveIn:
                    if (!_wasSetTint)
                    {
                        _wasSetTint = true;
                        _currentColor = BackgroundColor;
                        BackgroundColor = TintColor;
                        SelectedController();
                    }
                    break;
                case State.Ended:
                case State.MoveOut:
                    if (_wasSetTint)
                    {
                        _wasSetTint = false;
                        BackgroundColor = _currentColor;
                        UnSelectedController();

                        if (SetToggle)
                            ToggleState(obj => { });
                    }
                    break;
            }
            if (ShouldHighlightSubviewsOnTouch)
                NotifiyAllSubViews(_wasSetTint, this);
            HighlightChanged?.Invoke(this, new HighlightChangedEventArgs(_wasSetTint));
        }

        protected event EventHandler<HighlightChangedEventArgs> HighlightChanged = delegate { };

        public override void SendActionForControlEvent()
        {
            _currentState = State.Ended;
            HandleState();
            base.SendActionForControlEvent();
        }

        protected virtual void SelectedController()
        {
        }

        protected virtual void UnSelectedController()
        {
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            if (!Enabled) return;
            base.TouchesBegan(touches, evt);
            _currentState = State.Began;
            HandleState();
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            if (!Enabled) return;
            base.TouchesMoved(touches, evt);
            if (_currentState == State.Ended)
                return;
            var arr = touches.ToArray<UITouch>();
            var touch = arr[0];
            var state = PointInside(touch.LocationInView(touch.View), null)
                ? State.MoveIn
                : State.MoveOut;
            if (state == _currentState)
                return;
            _currentState = state;
            HandleState();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            if (!Enabled) return;
            base.TouchesEnded(touches, evt);
            if (_currentState == State.Ended)
                return;
            _currentState = State.Ended;
            HandleState();
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            if (!Enabled) return;
            base.TouchesCancelled(touches, evt);
            if (_currentState == State.Ended)
                return;
            _currentState = State.Ended;
            HandleState();
        }

        /// <summary>
        ///     Sets the title.
        ///     Will get only first label in view
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="state">State.</param>
        public void SetTitle(string text, ButtonState state = ButtonState.Normal)
        {
            CheckIfHaveLbl();
            foreach (var item in _stateMap)
            {
                if (item.Key == state)
                {
                    item.Value.Text = text;
                    item.Value.IsTextSet = true;
                }
                if (item.Value.IsTextSet) continue;
                item.Value.Text = text;
            }
            SetCurrentState();
        }

        /// <summary>
        ///     Sets the title.
        ///     Will get only first label in view
        /// </summary>
        /// <param name="color">color.</param>
        /// <param name="state">State.</param>
        public void SetTitleColor(UIColor color, ButtonState state)
        {
            CheckIfHaveLbl();
            foreach (var item in _stateMap)
            {
                if (item.Key == state)
                {
                    item.Value.Color = color;
                    item.Value.IsColorSet = true;
                }
                if (item.Value.IsColorSet) continue;
                item.Value.Color = color;
            }
            SetCurrentState();
        }

        private void SetCurrentState()
        {
            if (TitleLabel == null) return;
            var currentState = _stateMap[StateButton];
            TitleLabel.Text = currentState.Text;
            TitleLabel.TextColor = currentState.Color;
        }

        private void CheckIfHaveLbl()
        {
            if (TitleLabel != null) return;
            TitleLabel = (UILabel) Subviews.FirstOrDefault(_ => _ is UILabel);
            if (TitleLabel == null)
                throw new Exception("Could not find any labels in subviews please have atleast 1 label");
            GenerateDefaultState(ButtonState.Normal);
            GenerateDefaultState(ButtonState.Highlighted);
        }

        private void GenerateDefaultState(ButtonState state)
        {
            var item = _stateMap[state];
            item.Text = TitleLabel.Text;
            item.Color = TitleLabel.TextColor;
        }

        private void HandleState()
        {
            switch (_currentState)
            {
                case State.Began:
                case State.MoveIn:
                    StateButton = ButtonState.Highlighted;
                    break;
                case State.Ended:
                case State.MoveOut:
                    StateButton = ButtonState.Normal;
                    break;
            }

            if (Delegate != this && StateChanged != null)
                throw new Exception("You can't use both delegate and lambda function");
            if (StateChanged != null)
                StateChanged?.Invoke(this, _currentState);
            else
                Delegate?.HandleState(this, _currentState);
        }

        private void NotifiyAllSubViews(bool hightlighted, UIView view)
        {
            if (view.Subviews == null || view.Subviews.Length == 0) return;
            for (var i = 0; i < view.Subviews.Length; i++)
            {
                var child = view.Subviews[i];
                var img = child as UIImageView;
                if (img?.HighlightedImage != null)
                    img.Highlighted = hightlighted;
                var lbl = child as UILabel;
                if (lbl != null)
                    lbl.Highlighted = hightlighted;

                NotifiyAllSubViews(hightlighted, child);
            }
        }

        public void ToggleState(Action<UiButtonView> action)
        {
            if (!ToggleOn)
            {
                _currentColor = BackgroundColor;
                BackgroundColor = TintColor;
                ToggleOn = true;
            }
            else
            {
                BackgroundColor = UIColor.Clear;
                ToggleOn = false;
            }

            action?.Invoke(this);
        }

        private void Initialize()
        {
            Layer.MasksToBounds = true;
            Delegate = this;
        }

        protected virtual float SizeFont()
        {
            return 12f;
        }

        protected virtual UIColor ColorFont()
        {
            return UIColor.FromRGB(255, 255, 255);
        }

        protected virtual string NameFont()
        {
            return _nameFont;
        }

        protected sealed class HighlightChangedEventArgs : EventArgs
        {
            public HighlightChangedEventArgs(bool highlighted)
            {
                Highlighted = highlighted;
            }

            public bool Highlighted { get; }
        }

        private class TextState
        {
            public string Text { get; set; }
            public UIColor Color { get; set; }

            public bool IsTextSet { get; set; }
            public bool IsColorSet { get; set; }
        }
    }
}