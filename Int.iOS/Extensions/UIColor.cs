//
// UIColor.cs
//
// Author:
//       Songurov Fiodor <songurov@gmail.com>
//
// Copyright (c) 2016 Songurov Fiodor
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
using UIKit;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        public static UIColor FromHex(this UIColor color, int hexValue)
        {
            return UIColor.FromRGB(
                ((hexValue & 0xFF0000) >> 16) / 255.0f,
                ((hexValue & 0xFF00) >> 8) / 255.0f,
                (hexValue & 0xFF) / 255.0f
            );
        }

        public static UIColor FromHex(this UIColor color, string hexValue)
        {
            if (string.IsNullOrWhiteSpace(hexValue)) return UIColor.Clear;

            var colorString = hexValue.Replace("#", "");
            float red, green, blue;

            switch (colorString.Length)
            {
                case 3: // #RGB
                {
                    red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
                    green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
                    blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
                    return UIColor.FromRGB(red, green, blue);
                }
                case 6: // #RRGGBB
                {
                    red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                    green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                    blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                    return UIColor.FromRGB(red, green, blue);
                }
                case 8: // #AARRGGBB
                {
                    var alpha = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                    red = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                    green = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                    blue = Convert.ToInt32(colorString.Substring(6, 2), 16) / 255f;
                    return UIColor.FromRGBA(red, green, blue, alpha);
                }
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Invalid color value {hexValue} is invalid. It should be a hex value of the form #RBG, #RRGGBB, or #AARRGGBB");
            }
        }

        public static UIColor FromHex(this string hexValue)
        {
            if (string.IsNullOrWhiteSpace(hexValue)) return UIColor.Clear;

            var colorString = hexValue.Replace("#", "");
            float red, green, blue;

            switch (colorString.Length)
            {
                case 3: // #RGB
                {
                    red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
                    green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
                    blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
                    return UIColor.FromRGB(red, green, blue);
                }
                case 6: // #RRGGBB
                {
                    red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                    green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                    blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                    return UIColor.FromRGB(red, green, blue);
                }
                case 8: // #AARRGGBB
                {
                    var alpha = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                    red = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                    green = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                    blue = Convert.ToInt32(colorString.Substring(6, 2), 16) / 255f;
                    return UIColor.FromRGBA(red, green, blue, alpha);
                }
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Invalid color value {hexValue} is invalid. It should be a hex value of the form #RBG, #RRGGBB, or #AARRGGBB");
            }
        }
    }
}