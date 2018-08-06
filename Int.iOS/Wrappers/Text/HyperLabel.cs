//
//  HyperLabel.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2018 Songurov Fiodor
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
using System;
using System.Collections.Generic;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using Int.iOS.Extensions;
using UIKit;

namespace Int.iOS.Wrappers.Text
{
    [Register("HyperLabel"), DesignTimeVisible(true)]
    public class HyperLabel : UILabel
    {
        private const float HighLightAnimationTime = 0.15f;
        private IDictionary<NSRange, LinkHandler> _handlerDictionary;
        private NSAttributedString _backupAttributedText;
        private bool _wasTextCheck;

        private static readonly UIColor HyperLinkColorDefault = UIColor.FromRGB(28, 135, 199);
        private static readonly UIColor HyperLinkColorHighlight = UIColor.FromRGB(242, 183, 73);


        public HyperLabel()
        {
            Initialize();
        }

        public HyperLabel(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public HyperLabel(CGRect frame) : base(frame)
        {
            Initialize();
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                _wasTextCheck = true;
                if (Lines != 0)
                {
                    base.Text = value;
                    return;
                }
                var newAttr = new NSMutableAttributedString(value);
                newAttr.AddAttributes(new UIStringAttributes
                {
                    Font = Font
                }, new NSRange(0, value.Length));
                AttributedText = newAttr;
            }
        }


        public UIStringAttributes LinkAttributeDefault { get; set; } = new UIStringAttributes
        {
            ForegroundColor = HyperLinkColorDefault,
            UnderlineStyle = NSUnderlineStyle.Single
        };

        public UIStringAttributes LinkAttributeHighlight { get; set; } = new UIStringAttributes
        {
            ForegroundColor = HyperLinkColorHighlight,
            UnderlineStyle = NSUnderlineStyle.Single
        };

        public delegate void LinkHandler(HyperLabel label, NSRange range);

        #region  APIs
        public void SetLinkForRange(NSRange range, UIStringAttributes attributes, LinkHandler handler)
        {
            CheckIfShouldDoAttribute();
            var newAttribute = new NSMutableAttributedString(AttributedText);
            newAttribute.AddAttributes(attributes, range);
            if (handler != null)
                _handlerDictionary.TryAdd(range, handler);
            AttributedText = newAttribute;
        }

        public void SetLinkForRange(NSRange range, LinkHandler handler)
        {
            SetLinkForRange(range, LinkAttributeDefault, handler);
        }

        public void SetLinkForSubstring(string substring, UIStringAttributes attributes, LinkHandler handler)
        {
            var range = Text.IndexOf(substring, StringComparison.CurrentCulture);
            if (range < 0) return;
            SetLinkForRange(new NSRange(range, substring.Length), attributes, handler);
        }

        public void SetLinkForSubstring(string substring, LinkHandler handler)
        {
            SetLinkForSubstring(substring, LinkAttributeDefault, handler);
        }

        public void SetLinkForSubstrings(string[] substrings, LinkHandler handler)
        {
            foreach (var item in substrings)
                SetLinkForSubstring(item, handler);
        }

        public void ClearActionDictionary()
        {
            _handlerDictionary.Clear();
        }

        #endregion

        #region Event Handler

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            _backupAttributedText = AttributedText;
            var arr = touches.ToArray<UITouch>();
            foreach (var touch in arr)
            {
                var touchPoint = touch.LocationInView(this);
                var rangeValue = AttributedTextRangeForPoint(touchPoint);
                if (rangeValue == null) continue;

                var range = rangeValue.Value;
                var attributedString = new NSMutableAttributedString(AttributedText);
                attributedString.AddAttributes(LinkAttributeHighlight, range);
                TransitionNotify(this, HighLightAnimationTime, UIViewAnimationOptions.TransitionCrossDissolve, () =>
                {
                    AttributedText = attributedString;
                }, null);
            }

            this.BackgroundColor = UIColor.Blue;
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
        }

        #endregion

        #region Substring Locator

        private NSRange? AttributedTextRangeForPoint(CGPoint point)
        {
            var layoutManager = new NSLayoutManager();
            var textContainer = new NSTextContainer(CGSize.Empty)
            {
                LineFragmentPadding = 0.0f,
                LineBreakMode = LineBreakMode,
                MaximumNumberOfLines = (nuint)Lines,
                Size = Bounds.Size
            };

            layoutManager.AddTextContainer(textContainer);
            var textStorage = new NSTextStorage();
            textStorage.SetString(AttributedText);
            textStorage.AddLayoutManager(layoutManager);

            var textBoundingBox = layoutManager.GetUsedRectForTextContainer(textContainer);
            var textContainerOffset = new CGPoint(
                (Bounds.Width - textBoundingBox.Width) * 0.5f - textBoundingBox.GetMinX(),
                (Bounds.Height - textBoundingBox.Height) * 0.5f - textBoundingBox.GetMinY());

            var locationOfTouchInTextContainer = new CGPoint(point.X - textContainerOffset.X,
                                                             point.Y - textContainerOffset.Y);
            var frac = new nfloat(0.0f);
            var indexOfCharacter = layoutManager.CharacterIndexForPoint(locationOfTouchInTextContainer,
                                                                        textContainer, ref frac);
            foreach (var pair in _handlerDictionary)
            {
                var range = pair.Key;
                if (range.LocationInRange((int)indexOfCharacter))
                    return range;
            }
            return null;
        }

        #endregion

        private void ReverseAnimation()
        {
            TransitionNotify(this, HighLightAnimationTime, UIViewAnimationOptions.TransitionCrossDissolve, () =>
            {
                AttributedText = _backupAttributedText;
            }, null);
        }

        private void CheckIfShouldDoAttribute()
        {
            if (!_wasTextCheck && Text.Length > 0)
                Text = Text;
        }

        private void Initialize()
        {
            _handlerDictionary = new Dictionary<NSRange, LinkHandler>();
            UserInteractionEnabled = true;
        }
    }

}
