//
// UILabelExtension.cs
//
// Author:
//       Songurov Fiodor <f.songurov@software-dep.net>
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
using System.Threading.Tasks;
using CoreGraphics;
using CoreText;
using Foundation;
using Int.iOS.Controllers.ModalPicker;
using UIKit;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        public enum Direction
        {
            /// <summary>
            ///     Will Expand Width keeping Height
            /// </summary>
            Width,

            /// <summary>
            ///     Will Expand Height keeping Width
            /// </summary>
            Height
        }

        /// <summary>
        ///     Sizes to fit.
        /// </summary>
        /// <param name="lbl">Lbl.</param>
        /// <param name="direction">Direction.</param>
        public static void SizeToFit(this UILabel lbl, Direction direction)
        {
            if (lbl == null)
                return;
            var width = direction == Direction.Width
                ? nfloat.MaxValue
                : lbl.Bounds.Width;
            var height = direction == Direction.Height
                ? nfloat.MaxValue
                : lbl.Bounds.Height;

            var size = lbl.SizeThatFits(new CGSize(width, height));
            var frame = lbl.Frame;
            frame.Size = size;
            lbl.Frame = frame;
        }

        public static void SetBold(this UILabel lbl, NSRange range)
        {
            var attrs = new NSMutableAttributedString(lbl.Text);
            attrs.AddAttributes(new CTStringAttributes
            {
                Font = UIFontDescriptorSymbolicTraits.Bold.GetFontWithTraits(lbl.Font.PointSize).GetCtFont()
            }, range);
            lbl.AttributedText = attrs;
        }

        public static void SetBold(this UILabel lbl)
        {
            if (lbl.Text == null) return;
            SetBold(lbl, new NSRange(0, lbl.Text.Length));
        }

        public static nfloat SizeHeight(this UILabel lable, nfloat viewWidth, string text, nfloat sizeFont)
        {
            var lables = new UILabel
            {
                Font = lable.Font.WithSize(sizeFont),
                Text = text,
                Frame = new CGRect(0, 0, viewWidth, nfloat.MaxValue),
                Lines = 0
            };

            lables.SizeToFit();

            return lables.Bounds.Height;
        }

        public static void BoldPartText(this UILabel current, string strFull, int startChar, int endChar,
            UIFont sizeFont)
        {
            var firstAttributes = new UIStringAttributes
            {
                Font = sizeFont
            };

            var prettyString = new NSMutableAttributedString(strFull);

            prettyString.SetAttributes(firstAttributes.Dictionary, new NSRange(startChar, endChar));
            current.AttributedText = prettyString;
        }

        public static bool ReadMoreShow(this UILabel lable, nfloat sizeFont, float spaceLine = 1)
        {
            lable.SetNeedsLayout();
            lable.LayoutIfNeeded();

            var heightText = lable.SizeHeight(lable.Frame.Width, lable.Text, sizeFont);

            return lable.Frame.Height <= heightText * spaceLine;
        }

        public static nint GetNumberOfLines(this UILabel lbl)
        {
            if (lbl.Lines > 0) return lbl.Lines;
            var paragraphStyle = new NSMutableParagraphStyle {LineBreakMode = lbl.LineBreakMode};


            var maxSize = new CGSize(lbl.Bounds.Width, nfloat.MaxValue);
            var attributes = new UIStringAttributes
            {
                Font = lbl.Font,
                ParagraphStyle = (NSParagraphStyle) paragraphStyle.Copy()
            };
            var nsstring = new NSString(lbl.Text);
            var rect = nsstring.GetBoundingRect(maxSize, NSStringDrawingOptions.UsesLineFragmentOrigin, attributes,
                null);
            return (nint) Math.Floor(rect.Height / lbl.Font.LineHeight);
        }

        public static async Task SetTime(this UILabel lbl, UIViewController controller,
            UIDatePickerMode mode = UIDatePickerMode.Date, string format = "dd.MM.yyyy",
            Action<UILabel, string> actionNew = default(Action<UILabel, string>), bool setIntern = false)
        {
            var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "", controller)
            {
                HeaderBackgroundColor = UIColor.Gray,
                HeaderTextColor = UIColor.White,
                TransitioningDelegate = new ModalPickerTransitionDelegate(),
                ModalPresentationStyle = UIModalPresentationStyle.Custom,
                DatePicker = {Mode = mode}
            };


            if (setIntern)
                modalPicker.DatePicker.Date = NSDate.Now.AddSeconds(3600);

            modalPicker.OnModalPickerDismissed += (s, ea) =>
            {
                var dateFormatter = new NSDateFormatter
                {
                    DateFormat = format
                };

                if (!setIntern)
                    lbl.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
                actionNew?.Invoke(lbl, dateFormatter.ToString(modalPicker.DatePicker.Date));
            };

            await controller.PresentViewControllerAsync(modalPicker, true);
        }

        public static bool SetHtml(this UILabel lbl, string html, bool failBacktoText = true)
        {
            if (string.IsNullOrEmpty(html)) return true;
            var attr = new NSAttributedStringDocumentAttributes();
            var nsError = new NSError();
            attr.DocumentType = NSDocumentType.HTML;
            var unicode = NSData.FromString(html, NSStringEncoding.Unicode);
            lbl.AttributedText = new NSAttributedString(unicode, attr, ref nsError);
            var flag = lbl.AttributedText.Length > 0;
            if (!flag && failBacktoText)
                lbl.Text = html;
            return flag;
        }
    }
}