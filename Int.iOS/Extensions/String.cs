//
// String.cs
//
// Author:
//       Songurov Fiodor <songurov@gmail.com>
//
// Copyright (c) 2017 Songurov Fiodor
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
using Foundation;
using UIKit;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        public enum ReturnType
        {
            None,

            /// <summary>
            ///     Will return back to app after call ends
            /// </summary>
            Back
        }

        public static void CallNumber(this string number, ReturnType returnType = ReturnType.Back)
        {
            var havePrefix = number.StartsWith("tel://", StringComparison.CurrentCulture) ||
                             number.StartsWith("telprompt://", StringComparison.CurrentCulture);
            if (!havePrefix)
                switch (returnType)
                {
                    case ReturnType.Back:
                        if (!number.Contains("telprompt://"))
                            number = "telprompt://" + number;
                        break;
                    case ReturnType.None:
                        if (!number.Contains("tel://"))
                            number = "tel://" + number;
                        break;
                }
            UIApplication.SharedApplication.OpenUrl(new NSUrl(number));
        }

        public static string GetPath(this string name, string extenstionFiole = "png")
        {
            return NSBundle.MainBundle.PathForResource(name, extenstionFiole);
        }

        public static nfloat GetWidthFromText(this string str, float fontSize = 17f)
        {
            var lbl = new UILabel { Text = str };
            lbl.Font = lbl.Font.WithSize(fontSize);
            lbl.SizeToFit();
            return lbl.Bounds.Width;
        }
    }
}