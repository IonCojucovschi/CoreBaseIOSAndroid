//
// PaymentCardTextView.cs
//
// Author:
//       arslan_ataev <chameleon256@gmail.com>
//
// Copyright (c) 2017 (c) ARSLAN ATAEV
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

using System.Linq;
using System.Text.RegularExpressions;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Widget;

namespace Int.Droid.Controllers
{
    [Register("Int.Droid.Controllers.PaymentCardEditText")]
    public class PaymentCardEditText : EditText
    {
        private const int MaxCharLengthWithSpaces = 19;

        public PaymentCardEditText(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(
            context, attrs, defStyleAttr, defStyleRes)
        {
            SetDefaultAttributes();
        }

        public PaymentCardEditText(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs,
            defStyleAttr)
        {
            SetDefaultAttributes();
        }

        public PaymentCardEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            SetDefaultAttributes();
        }

        public PaymentCardEditText(Context context) : base(context)
        {
            SetDefaultAttributes();
        }

        private void SetDefaultAttributes()
        {
            SetSingleLine(true);
            InputType = InputTypes.ClassNumber;
            var filters = GetFilters();
            var newFilters = new IInputFilter[filters.Length + 1];
            filters.CopyTo(newFilters, 0);
            var lengthFilter = new InputFilterLengthFilter(MaxCharLengthWithSpaces);
            newFilters[newFilters.Length - 1] = lengthFilter;
            SetFilters(newFilters);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            TextChanged += FormatTextAsCardNumber;
            Text = Text;
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            TextChanged -= FormatTextAsCardNumber;
        }

        private static void FormatTextAsCardNumber(object sender, TextChangedEventArgs e)
        {
            var cardNumber = e.Text.ToString().Trim().Replace(" ", string.Empty);
            var formatedText = Regex.Replace(cardNumber, "(?!^).{4}", " $0", RegexOptions.RightToLeft);
            if (string.CompareOrdinal(e.Text.ToString(), formatedText) == 0) return;
            var spaceSymbolsCorrection = formatedText.Length - e.Text.Count();
            var cursorPosition = ((EditText) sender).SelectionEnd;
            ((EditText) sender).Text = string.Copy(formatedText);
            ((EditText) sender)?.SetSelection(cursorPosition + spaceSymbolsCorrection);
        }
    }
}