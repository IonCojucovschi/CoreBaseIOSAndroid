using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Int.iOS.Extensions;
using UIKit;

namespace Int.iOS.Views
{
    public class UIMaterialTextInput : UITextField
    {
        private UIColor _backgroundColor;
        private UILabel _hintLabel;

        private string _hintText;
        private bool _isOpened;
        private CALayer _underline;
        private bool _wasResized;

        protected virtual UIColor HintActiveColor => UIColor.Blue;
        protected virtual UIColor HintInactiveColor => UIColor.LightGray;
        protected virtual UIColor UnderlineColor => UIColor.Orange;

        protected virtual double AnimationDuration => 0.2;
        protected virtual nfloat HintLabelOffsetMultiplier => 1.0f;
        protected virtual nfloat HintLabelScale => 0.9f;
        protected virtual float UnderlineWidth => 1.0f;
        protected virtual int HintLines => 1;
        protected virtual bool AdjustsHintFontSize => true;

        public override void MovedToSuperview()
        {
            base.MovedToSuperview();

            SetInitialState();
        }

        private void SetInitialState()
        {
            BorderStyle = UITextBorderStyle.None;
            Placeholder = string.Empty;
            SetUnderline();
            CreatePlaceHolder();
        }

        private void SetUnderline(UIColor color = null)
        {
            _underline?.RemoveFromSuperLayer();

            this.AddBottomBorder(color ?? HintInactiveColor, UnderlineWidth);
            if (Layer.Sublayers.Length > 0)
                _underline = Layer.Sublayers[Layer.Sublayers.Length - 1];
        }

        private void SetPlaceholderTextStyle()
        {
            if (_hintLabel == null) return;

            _hintLabel.Font = Font;
            _hintLabel.TextColor = HintInactiveColor;
            _hintLabel.TextAlignment = TextAlignment;
        }

        private void CreatePlaceHolder()
        {
            _hintLabel = new UILabel();
            _backgroundColor = BackgroundColor ?? UIColor.Clear;

            _hintLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            _hintLabel.Lines = HintLines;
            _hintLabel.Text = _hintText;
            _hintLabel.BackgroundColor = _backgroundColor;
            _hintLabel.AdjustsFontSizeToFitWidth = AdjustsHintFontSize;
            SetPlaceholderTextStyle();

            base.Placeholder = string.Empty;
            BackgroundColor = UIColor.Clear;

            InsertSubview(_hintLabel);

            EditingDidBegin += (sender, e) =>
            {
                AnimateColorChange(AnimationDuration, true);
                if (string.IsNullOrWhiteSpace(Text))
                    AnimateOpening(AnimationDuration);
            };

            EditingDidEnd += (sender, e) =>
            {
                AnimateColorChange(AnimationDuration);
                if (string.IsNullOrWhiteSpace(Text))
                    AnimateClosing(AnimationDuration);
            };
        }

        private void InsertSubview(UIView view)
        {
            Superview.InsertSubviewBelow(view, this);

            var verticalConstraint = NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal,
                view, NSLayoutAttribute.CenterY, 1, 0);
            var leadingConstraint = NSLayoutConstraint.Create(this, NSLayoutAttribute.Leading, NSLayoutRelation.Equal,
                view, NSLayoutAttribute.Leading, 1, 0);
            var trailingConstraint = NSLayoutConstraint.Create(this, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal,
                view, NSLayoutAttribute.Trailing, 1, 0);

            Superview.AddConstraints(new[] {verticalConstraint, leadingConstraint, trailingConstraint});
        }

        private void AnimateOpening(double duration = 0)
        {
            if (_isOpened) return;
            _isOpened = true;

            if (_wasResized || _hintLabel.Frame.Height == 0)
            {
                _hintLabel.Superview.SetNeedsLayout();
                _hintLabel.Superview.LayoutIfNeeded();
                _wasResized = false;
            }

            AnimateNotify(
                duration,
                () =>
                {
                    var initialTransform = _hintLabel.Transform;
                    var scaledTransform = CGAffineTransform.Scale(initialTransform, HintLabelScale, HintLabelScale);
                    var translatedTransform = CGAffineTransform.Translate(
                        scaledTransform,
                        -((Frame.Width - _hintLabel.Frame.Width * HintLabelScale) / 2 / HintLabelScale),
                        -(((Frame.Height * HintLabelOffsetMultiplier - _hintLabel.Frame.Height * HintLabelScale) / 2 +
                           _hintLabel.Frame.Height * HintLabelScale) / HintLabelScale));
                    _hintLabel.Transform = translatedTransform;
                },
                completed =>
                {
                    if (!completed) return;
                    BackgroundColor = _backgroundColor;
                });
        }

        private void AnimateClosing(double duration = 0)
        {
            if (!_isOpened) return;
            _isOpened = false;

            AnimateNotify(
                duration,
                () => { _hintLabel.Transform = CGAffineTransform.MakeIdentity(); },
                completed =>
                {
                    if (!completed) return;
                    BackgroundColor = UIColor.Clear;
                });
        }

        private void AnimateColorChange(double duration, bool active = false)
        {
            AnimateNotify(
                duration,
                () =>
                {
                    _hintLabel.TextColor = active ? HintActiveColor : HintInactiveColor;
                    if (_underline == null) return;
                    _underline.BorderColor = active ? UnderlineColor.CGColor : HintInactiveColor.CGColor;
                },
                completed => { });
        }

        #region ctor

        public UIMaterialTextInput()
        {
            Initialize();
        }

        public UIMaterialTextInput(CGRect frame) : base(frame)
        {
            Initialize();
        }

        protected internal UIMaterialTextInput(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        protected UIMaterialTextInput(NSObjectFlag t) : base(t)
        {
            Initialize();
        }

        public UIMaterialTextInput(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        private static void Initialize()
        {
        }

        #endregion

        #region overriden properties

        public override CGRect Bounds
        {
            get => base.Bounds;
            set
            {
                base.Bounds = value;
                _wasResized = true;
                SetUnderline();

                if (!_isOpened) return;
                AnimateClosing();
                AnimateOpening();
            }
        }

        public override UIFont Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                SetPlaceholderTextStyle();
            }
        }

        public override UITextBorderStyle BorderStyle
        {
            get => UITextBorderStyle.None;
            set => base.BorderStyle = value;
        }

        public override string Placeholder
        {
            get => _hintLabel?.Text;
            set
            {
                _hintText = value;
                if (_hintLabel != null)
                    _hintLabel.Text = _hintText;
            }
        }

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                if (!string.IsNullOrWhiteSpace(Text))
                    AnimateOpening();
            }
        }

        #endregion
    }
}