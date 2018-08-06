//
// ImageView.cs
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
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Provider;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using Int.Core.Application.Exception;
using Int.Core.Extensions;
using Uri = Android.Net.Uri;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        private const string ImageAsyncErrorMessage = "Svg image can be set only to ImageViewAsync";

        private static ImageViewAsync CastImageViewToAsync(ImageView image)
        {
            if (image is ImageViewAsync imageAsync) return imageAsync;
            ExceptionLogger.RaiseNonFatalException(
                new ExceptionWithCustomStack(
                    ImageAsyncErrorMessage, Environment.StackTrace));
            return null;
        }

        public static Bitmap GetBitmapFromImageView(this ImageView source)
        {
            return (source?.Drawable as BitmapDrawable)?.Bitmap;
        }

        public static string GetPathToImage(this Uri uri, Context context)
        {
            try
            {
                var cursor = context.ContentResolver.Query(uri, null, null, null, null);
                if (cursor == null)
                    return uri.Path;
                cursor.MoveToFirst();
                var idx = cursor.GetColumnIndex(MediaStore.Images.ImageColumns.Data);
                var rez = cursor.GetString(idx);
                cursor.Close();
                return rez;
            }
            catch
            {
                return uri.Path;
            }
        }

        public static void SetImage(this ImageView image, string url = "", Stream stream = default(Stream), int resourceDrawable = 0)
        {
            ImageViewAsync imageAsync = null;
            if ((imageAsync = CastImageViewToAsync(image)) == null)
                return;

            if (!stream.IsNull())
                ImageService.Instance
                    .LoadStream(cancelationToken =>
                        System.Threading.Tasks.Task.Factory.StartNew(
                            () =>
                            {
                                cancelationToken.ThrowIfCancellationRequested();

                                var streamClone = new MemoryStream();
                                if (stream?.CanSeek ?? false)
                                    stream?.Seek(0, SeekOrigin.Begin);

                                stream?.CopyTo(streamClone);

                                if (streamClone?.CanSeek ?? false)
                                    streamClone.Seek(0, SeekOrigin.Begin);

                                return streamClone as Stream;
                            }, cancelationToken))
                    .Into(imageAsync);
            else if (!url.IsNullOrWhiteSpace())
            {
                ImageService.Instance
                    .LoadUrl(url)
                    .Into(imageAsync);
            }
            else if (resourceDrawable > 0)
                ImageService.Instance
                    .LoadCompiledResource(resourceDrawable.ToString())
                    .Into(imageAsync);
        }

        public static void SetImageTransform(ImageView image, string url = "", Stream stream = default(Stream), int resourceDrawable = 0)
        {
            ImageViewAsync imageAsync = null;
            if ((imageAsync = CastImageViewToAsync(image)) == null)
                return;

            if (!stream.IsNull())
                ImageService.Instance
                            .LoadStream(cancelationToken =>
                               System.Threading.Tasks.Task.Factory.StartNew(
                               () =>
                               {
                                   cancelationToken.ThrowIfCancellationRequested();

                                   var streamClone = new MemoryStream();
                                   if (stream?.CanSeek ?? false)
                                       stream?.Seek(0, SeekOrigin.Begin);

                                   stream?.CopyTo(streamClone);

                                   if (streamClone?.CanSeek ?? false)
                                       streamClone.Seek(0, SeekOrigin.Begin);

                                   return streamClone as Stream;
                               }, cancelationToken))
                            .Transform(new CircleTransformation())
                            .Into(imageAsync);
            else if (!url.IsNullOrWhiteSpace())
            {
                ImageService.Instance
                            .LoadUrl(url)
                            .Transform(new CircleTransformation())
                            .Into(imageAsync);
            }
            else if (resourceDrawable > 0)
                ImageService.Instance
                            .LoadCompiledResource(resourceDrawable.ToString())
                            .Transform(new CircleTransformation())
                            .Into(imageAsync);
        }
    }
}