//
// Bitmap.cs
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
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        private const int Quality = 85;

        public static string ConvertBitmapToBase64(this Bitmap value)
        {
            using (var stream = new MemoryStream())
            {
                value.Compress(Bitmap.CompressFormat.Png, Quality, stream);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static async Task<string> ConvertBitmapToBase64Async(this Bitmap value)
        {
            using (var stream = new MemoryStream())
            {
                await value.CompressAsync(Bitmap.CompressFormat.Png, Quality, stream);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static Bitmap ConverBase64ToBitmap(this string value)
        {
            var options = new BitmapFactory.Options();
            if (value == null) return null;
            using (var stream = new MemoryStream(Convert.FromBase64String(value)))
            {
                options.InJustDecodeBounds = false;
                options.InSampleSize = 2;
                return BitmapFactory.DecodeStream(stream, null, options);
            }
        }

        public static async Task<Bitmap> ConverBase64ToBitmapAsync(this string value)
        {
            var options = new BitmapFactory.Options();

            using (var stream = new MemoryStream(Convert.FromBase64String(value)))
            {
                options.InJustDecodeBounds = false;
                options.InSampleSize = 2;
                return await BitmapFactory.DecodeStreamAsync(stream, null, options);
            }
        }

        public static string ConvertBase64ToImagePath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("string empty (Path)");

            if (!File.Exists(path))
                throw new FileNotFoundException();

            var data = File.ReadAllBytes(path);

            return BitmapFactory.DecodeByteArray(data, 0, data.Length)?.ConvertBitmapToBase64();
        }

        public static async Task<string> ConvertBase64ToImagePathAsync(this string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("string empty (Path)");

            if (!File.Exists(path))
                throw new FileNotFoundException();

            var data = File.ReadAllBytes(path);

            var img = await BitmapFactory.DecodeByteArrayAsync(data, 0, data.Length);

            return img != null ? await img.ConvertBitmapToBase64Async() : default(string);
        }

        public static async Task<Bitmap> ResizeBitmapAsync(this string fileName, int width, int height)
        {
            var inSampleSize = 2;

            var options = new BitmapFactory.Options {InJustDecodeBounds = true, InSampleSize = inSampleSize};
            await BitmapFactory.DecodeFileAsync(fileName, options);

            var outHeight = options.OutHeight;
            var outWidth = options.OutWidth;

            if (outHeight > height || outWidth > width)
                inSampleSize = outWidth > outHeight
                    ? outHeight / height
                    : outWidth / width;

            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            var resizedBitmap = await BitmapFactory.DecodeFileAsync(fileName, options);

            return resizedBitmap;
        }

        public static Bitmap CreateScaledBitmap(this Bitmap bitmap, float scaleFactor)
        {
            using (var m = new Matrix())
            {
                m.SetRectToRect(new RectF(0, 0, bitmap.Width, bitmap.Height),
                    new RectF(0, 0, bitmap.Width * scaleFactor, bitmap.Height * scaleFactor), Matrix.ScaleToFit.Center);
                return Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, m, true);
            }
        }

        public static Bitmap ResizeBitmap(this string fileName, int width, int height)
        {
            var inSampleSize = 2;

            var options = new BitmapFactory.Options {InJustDecodeBounds = true, InSampleSize = inSampleSize};
            BitmapFactory.DecodeFile(fileName, options);

            var outHeight = options.OutHeight;
            var outWidth = options.OutWidth;

            if (outHeight > height || outWidth > width)
                inSampleSize = outWidth > outHeight
                    ? outHeight / height
                    : outWidth / width;

            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            var resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }

        public static Bitmap RotateBitmap(this Bitmap source, float angle)
        {
            var matrix = new Matrix();
            matrix.PostRotate(angle);
            return Bitmap.CreateBitmap(source, 0, 0, source.Width, source.Height, matrix, true);
        }
    }
}