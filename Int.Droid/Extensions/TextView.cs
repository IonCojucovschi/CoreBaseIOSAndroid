//
// TextView.cs
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
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using Int.Droid.Tools.Font;
using Java.Lang;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        public static void SetUnderline(this TextView textView)
        {
            var text = $"<u>{textView.Text}</u>";
            SetFormattedText(textView, text);
        }

        public static void SetTextBold(this TextView textView, string boldText)
        {
            SetTextBold(textView, textView.Text, boldText);
        }

        public static void SetFont(this TextView textView, string name)
        {
            if (textView != null)
                textView.Typeface = FontManager.Instance.GetFont(name);
        }

        public static void SetFont(this TextView textView, string name, float sizeFont)
        {
            AppTools.CurrentActivity.RunOnUiThread(() =>
            {
                if (textView != null)
                    textView.Typeface = FontManager.Instance.GetFont(name, sizeFont);

                textView?.SetTextSize(ComplexUnitType.Px, sizeFont.Iphone6PointsToDevicePixels());
            });
        }

        public static void SetCapitalization(this TextView textView)
        {
            textView?.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });
        }

        public static void ClearFocus(this TextView textView, Context context, string name)
        {
            if (textView == null) return;
            textView.Focusable = true;
            textView.FocusableInTouchMode = true;
            textView.ClearFocus();
        }

        public static void SetFormattedText(this TextView textView, string text)
        {
            var result = default(ISpanned);

#pragma warning disable CS0618 // Type or member is obsolete
            result = (int)Build.VERSION.SdkInt >= 24
                ? Html.FromHtml(text, FromHtmlOptions.ModeLegacy)
                : Html.FromHtml(text);
#pragma warning restore CS0618 // Type or member is obsolete

            textView.SetText(result, TextView.BufferType.Normal);
        }


        public static void SetLinkForSubstring(this TextView textView, string substringLink, string link)
        {
            var indexOf = textView.Text.IndexOf(substringLink, StringComparison.CurrentCulture);
            if (indexOf < 0) return;
            var sb = new StringBuilder(textView.Text);
            sb.Replace(indexOf, indexOf + substringLink.Length, $"<a href='{link}'/a><u>{substringLink}</u>");
            SetFormattedText(textView, sb.ToString());
            textView.LinksClickable = true;
            textView.MovementMethod = LinkMovementMethod.Instance;
        }

        public static void SetClickableSubstring(this TextView This, string substringLink, Action action, bool underline = true)
        {
            var indexOf = This.Text.IndexOf(substringLink, StringComparison.CurrentCulture);
            if (indexOf < 0)
                throw new InvalidOperationException($"{substringLink} can't be found in {This.Text}");
            var spannableString = new SpannableString(This.Text);
            spannableString.SetSpan(new ImplClickableSpane(action),
                                    indexOf, indexOf + substringLink.Length, SpanTypes.ExclusiveExclusive);
            if (underline)
                spannableString.SetSpan(new UnderlineSpan(), indexOf, indexOf + substringLink.Length, 0);

            This.TextFormatted = spannableString;
            This.MovementMethod = LinkMovementMethod.Instance;
        }


        public static void SetTextBold(this TextView textView, string text, string boldText)
        {
            var textToLower = text.ToLower();
            var boldTextToLower = boldText.ToLower();

            var indexOf = textToLower.IndexOf(boldTextToLower, StringComparison.CurrentCulture);
            if (indexOf < 0)
            {
                textView.Text = text;
                return;
            }
            var offset = indexOf + boldText.Length;
            var substringBold = text.Substring(indexOf, boldText.Length);
            var sb = new StringBuilder();
            sb.Append(text.Substring(0, indexOf));
            sb.Append("<b>").Append(substringBold).Append("</b>");
            sb.Append(text.Substring(offset, text.Length - offset));
            SetFormattedText(textView, sb.ToString());
        }

        private class ImplClickableSpane : ClickableSpan
        {
            private readonly Action _action;

            public ImplClickableSpane(Action action)
            {
                _action = action;
            }

            public override void OnClick(View widget)
            {
                _action?.Invoke();
            }
        }
    }
}