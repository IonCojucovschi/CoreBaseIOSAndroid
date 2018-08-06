//
// UITextView.cs
//
// Author:
//       Sogurov Fiodor <f.songurov@software-dep.net>
//
// Copyright (c) 2016 Songurov
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
using Foundation;
using Int.Core.Extensions;
using Int.iOS.Tools.Font;
using UIKit;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        private static readonly HashSet<Type> Types = new HashSet<Type>
        {
            typeof(UIButton),
            typeof(UILabel),
            typeof(UITextField),
            typeof(UITextView)
        };

        private static bool Close(UITextView textView, NSRange range, string value)
        {
            if (value.Equals("\n"))
                textView.ResignFirstResponder();
            return true;
        }

        public static void SetReturn(this UITextView textView)
        {
            textView.ShouldChangeText = Close;
            textView.ReturnKeyType = UIReturnKeyType.Done;
            textView.EnablesReturnKeyAutomatically = true;
        }

        public static void SetCapitalization(this UITextView textField,
            UITextAutocapitalizationType typeCharacters = UITextAutocapitalizationType.AllCharacters)
        {
            AppTools.InvokeOnMainThread(() => { textField.AutocapitalizationType = typeCharacters; });
        }

        /// <summary>
        ///     Sets the placeholder.
        ///     Placeholder takes font from target <see cref="textView" />.
        /// </summary>
        /// <param name="textView">Text view.</param>
        /// <param name="text"></param>
        /// <param name="textColor">Placeholder text color.</param>
        public static void SetPlaceholder(this UITextView textView, string text, UIColor textColor)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                var backgroundLabel = new UILabel();
                var backgroundColor = textView.BackgroundColor ?? UIColor.Clear;

                backgroundLabel.TranslatesAutoresizingMaskIntoConstraints = false;
                backgroundLabel.Lines = 0;
                backgroundLabel.Text = text;
                backgroundLabel.Font = textView.Font;
                backgroundLabel.TextColor = textColor;
                backgroundLabel.TextAlignment = textView.TextAlignment;
                backgroundLabel.BackgroundColor = backgroundColor;

                textView.Text = string.Empty;
                textView.BackgroundColor = UIColor.Clear;

                textView.Superview.InsertSubviewBelow(backgroundLabel, textView);

                var topConstraint = NSLayoutConstraint.Create(textView, NSLayoutAttribute.TopMargin,
                    NSLayoutRelation.Equal, backgroundLabel, NSLayoutAttribute.Top, 1, 0);
                var leadingConstraint = NSLayoutConstraint.Create(textView, NSLayoutAttribute.LeadingMargin,
                    NSLayoutRelation.Equal, backgroundLabel, NSLayoutAttribute.Leading, 1, 0);
                var trailingConstraint = NSLayoutConstraint.Create(textView, NSLayoutAttribute.TrailingMargin,
                    NSLayoutRelation.Equal, backgroundLabel, NSLayoutAttribute.Trailing, 1, 0);

                textView.Superview.AddConstraints(new[] { topConstraint, leadingConstraint, trailingConstraint });

                textView.Changed += (sender, e) =>
                {
                    if (string.IsNullOrWhiteSpace((sender as UITextView)?.Text))
                    {
                        backgroundLabel.Hidden = false;
                        textView.BackgroundColor = UIColor.Clear;
                    }
                    else
                    {
                        backgroundLabel.Hidden = true;
                        textView.BackgroundColor = backgroundColor;
                    }
                };
            });
        }

        public static void SetLineSpacing(this UILabel label, nfloat spacing)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                var paragraphStyle = new NSMutableParagraphStyle
                {
                    LineSpacing = spacing
                };
                var attrString = new NSMutableAttributedString(label.Text);
                var style = UIStringAttributeKey.ParagraphStyle;
                var range = new NSRange(0, attrString.Length);

                attrString.AddAttribute(style, paragraphStyle, range);


                label.AttributedText = attrString;
            });
        }

        public static void SetFont(this UIButton label, string fontType, float size = 17)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                if (label.IsNull()) return;
                var font = FontManager.Instance.GetFont(fontType);
                if (font.IsNull()) return;
                SetSize(label, size, font);
            });
        }

        public static void SetFont(this UILabel label, string fontType)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                if (label.IsNull()) return;
                var font = FontManager.Instance.GetFont(fontType);
                if (font.IsNull()) return;
                label.Font = font?.WithSize(label.Font.PointSize);
            });
        }

        public static void SetFont(this UILabel label, string fontType,
            float size)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                var font = FontManager.Instance.GetFont(fontType);
                if (font.IsNull()) return;

                SetSize(label, size, font);
            });
        }

        public static void SetFont(this UITextField textField, string fontType,
            float size)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                var font = FontManager.Instance.GetFont(fontType);
                if (font.IsNull()) return;
                SetSize(textField, size, font);
            });
        }

        public static void SetFont(this UITextView textView, string fontType,
            float size)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                var font = FontManager.Instance.GetFont(fontType);
                if (font.IsNull()) return;
                SetSize(textView, size, font);
            });
        }

        #region size

        private static void SetSize(UITextField label, float size, UIFont font)
        {
            label.Font = font.WithSize(Calculate(size));
        }

        private static void SetSize(UITextView label, float size, UIFont font)
        {
            label.Font = font.WithSize(Calculate(size));
        }

        private static void SetSize(UILabel label, float size, UIFont font)
        {
            label.Font = font.WithSize(Calculate(size));
        }

        private static void SetSize(UIButton label, float size, UIFont font)
        {
            label.Font = font.WithSize(Calculate(size));
        }

        private static float Calculate(float sizeIncriment)
        {
            return sizeIncriment;
        }

        #endregion
    }
}