//
// ImageLoader.cs
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

using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Int.Droid.Controllers;
using Int.Droid.Helpers;
using Int.Droid.Views;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        public static void ImageLoaderAsync(this ImageLoader view, string path)
        {
            if (!File.Exists(path))
            {
                view.SetImageBitmap(null);
                return;
            }
            if (view.Height > 0 && view.Width > 0)
            {
                view.SetImageBitmap(DecodeFromFile(path, view.Width, view.Height));
                return;
            }
            var lisener = new GlobalLayoutListener(view, true);
            view.ShowLoading(true);
            lisener.GlobalLayout += async (sender, e) =>
                view.SetImageBitmap(
                    await DecodeFromFileAsync(path, view.Width, view.Height));
        }

        private static Bitmap DecodeFromFile(string path, int width, int height)
        {
            var options = new BitmapFactory.Options {InJustDecodeBounds = true};

            try
            {
                var bytes = File.ReadAllBytes(path);
                using (var ms = new MemoryStream(bytes))
                {
                    BitmapFactory.DecodeStream(ms, null, options);
                    options.InSampleSize = BitmapHelper.CalculateInSampleSize(options, width, height);
                    options.InJustDecodeBounds = false;
                    ms.Position = 0;
                    return BitmapFactory.DecodeStream(ms, null, options);
                }
            }
            catch
            {
                return null;
            }
        }

        private static async Task<Bitmap> DecodeFromFileAsync(string path, int width, int height)
        {
            var options = new BitmapFactory.Options();

            try
            {
                options.InJustDecodeBounds = true;
                var bytes = File.ReadAllBytes(path);
                using (var ms = new MemoryStream(bytes))
                {
                    await BitmapFactory.DecodeStreamAsync(ms, null, options);
                    options.InSampleSize = BitmapHelper.CalculateInSampleSize(options, width, height);
                    options.InJustDecodeBounds = false;
                    ms.Position = 0;
                    return await BitmapFactory.DecodeStreamAsync(ms, null, options);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}