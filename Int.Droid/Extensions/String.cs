//
// String.cs
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

using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                return new MailAddress(email).Address.Equals(email);
            }
            catch
            {
                return false;
            }
        }

        public static string CreateMd5(this string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();

                foreach (var t in hashBytes)
                    sb.Append(t.ToString("X2"));
                return sb.ToString();
            }
        }

        public static async Task<Bitmap> LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            var options = new BitmapFactory.Options { InJustDecodeBounds = true, InSampleSize = 2 };
            BitmapFactory.DecodeFile(fileName, options);

            var outHeight = options.OutHeight;
            var outWidth = options.OutWidth;
            var inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                    ? outHeight / height
                    : outWidth / width;
            }

            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            var resizedBitmap = await BitmapFactory.DecodeFileAsync(fileName, options);

            return resizedBitmap;
        }
    }
}