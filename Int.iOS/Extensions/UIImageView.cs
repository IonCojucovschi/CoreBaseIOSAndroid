//
// UIImageView.cs
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
using System.Drawing;
using System.Linq;
using CoreGraphics;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using FFImageLoading.Work;
using Foundation;
using Int.Core.Extensions;
using UIKit;

namespace Int.iOS.Extensions
{
    public enum ImageLoadType
    {
        Xamarin,
        FFImage
    }

    public static partial class Extensions
    {
        private const string ActivityIndicatorTag = "ActivityIndicator";

        public static void ChangeImageColor(this UIImageView imgView, UIColor color)
        {
            if (imgView.Image != null)
                imgView.Image = imgView.Image.ChangeColor(color);
        }

        public static void ChangeHighlightedImageColor(this UIImageView imgView, UIColor color)
        {
            var img = imgView.HighlightedImage ?? imgView.Image;
            if (img == null) return;
            img = img.ChangeColor(color);
            imgView.HighlightedImage = img;
        }

        public static void SetImage(this UIImageView current, string url, ImageLoadType type = ImageLoadType.Xamarin,
            Action success = null, Action<Exception> error = null, Action startLoading = null,
            string imagePlaceholder = "error_placeholder.png", bool showLoadIndicator = true,
            UIActivityIndicatorViewStyle loadIndicatorType = UIActivityIndicatorViewStyle.Gray)
        {
            if (url.IsNullOrWhiteSpace())
            {
                AppTools.InvokeOnMainThread(() =>
                {
                    current.Image = null;
                });

                return;
            }

            var activityIndicator = PrepareImageView(current, showLoadIndicator, loadIndicatorType);

            TaskParameter loader = null;

            if (!url.Contains("http"))
                loader = ImageService.Instance.LoadCompiledResource(url);
            else
                loader = ImageService.Instance.LoadUrl(url);

            if (url.Contains(".svg"))
                loader
                    .WithCustomDataResolver(new SvgDataResolver(200, 0, true))
                    .WithCustomLoadingPlaceholderDataResolver(new SvgDataResolver(200, 0, true));

            RunImageLoader(current, success, error, startLoading, imagePlaceholder, activityIndicator, loader);
        }

        public static void SetImage(this UIImageView current, System.IO.Stream imageStream,
            Action success = null, Action<Exception> error = null, Action startLoading = null,
            string imagePlaceholder = "error_placeholder.png", bool showLoadIndicator = true,
            UIActivityIndicatorViewStyle loadIndicatorType = UIActivityIndicatorViewStyle.Gray)
        {
            if (!imageStream?.CanRead ?? false) return;

            var activityIndicator = PrepareImageView(current, showLoadIndicator, loadIndicatorType);
            var loader = ImageService.Instance.LoadStream(
                cancelationToken =>
                    System.Threading.Tasks.Task.Factory.StartNew(
                    () =>
                    {
                        cancelationToken.ThrowIfCancellationRequested();

                        var streamClone = new System.IO.MemoryStream();
                        if (imageStream?.CanSeek ?? false)
                            imageStream?.Seek(0, System.IO.SeekOrigin.Begin);

                        imageStream?.CopyTo(streamClone);

                        if (streamClone?.CanSeek ?? false)
                            streamClone?.Seek(0, System.IO.SeekOrigin.Begin);

                        return streamClone as System.IO.Stream;
                    }, cancelationToken));
            RunImageLoader(current, success, error, startLoading, imagePlaceholder, activityIndicator, loader);
        }

        private static UIActivityIndicatorView PrepareImageView(
            UIImageView current, bool showLoadIndicator, UIActivityIndicatorViewStyle loadIndicatorType)
        {
            UIActivityIndicatorView activityIndicator = null;
            if (showLoadIndicator)
                current.InvokeOnMainThread(() =>
                {
                    activityIndicator = current.Subviews
                            .FirstOrDefault(subview => subview.Tag == ActivityIndicatorTag.GetHashCode())
                        as UIActivityIndicatorView;

                    if (activityIndicator == null)
                    {
                        activityIndicator = new UIActivityIndicatorView
                        {
                            Tag = ActivityIndicatorTag.GetHashCode(),
                            HidesWhenStopped = true,
                            TranslatesAutoresizingMaskIntoConstraints = false,
                            ActivityIndicatorViewStyle = loadIndicatorType
                        };

                        current.AddSubview(activityIndicator);
                        activityIndicator.TopOf(current).LeftOf(current).RightOf(current).BottomOf(current);
                    }

                    activityIndicator?.StartAnimating();
                });

            current.InvokeOnMainThread(() => current.Image = null);
            return activityIndicator;
        }

        private static void RunImageLoader(UIImageView current, Action success, Action<Exception> error, Action startLoading, string imagePlaceholder, UIActivityIndicatorView activityIndicator, TaskParameter loader)
        {
            loader.DownloadStarted(obj => startLoading?.Invoke())
                .ErrorPlaceholder(imagePlaceholder)
                .Error(exception =>
                {
                    error?.Invoke(exception);
                    current.InvokeOnMainThread(() =>
                    {
                        activityIndicator?.StopAnimating();
                        activityIndicator?.RemoveFromSuperview();
                    });
                })
                .Finish(objf =>
                {
                    current.InvokeOnMainThread(() =>
                    {
                        activityIndicator?.StopAnimating();
                        activityIndicator?.RemoveFromSuperview();
                    });

                    success?.Invoke();
                })
                .Into(current);
        }

        public static UIImage ScaleAndRotateImage(this UIImage imageIn, UIImageOrientation orIn)
        {
            const int kMaxResolution = 2048;

            var imgRef = imageIn.CGImage;
            float width = imgRef.Width;
            float height = imgRef.Height;
            CGAffineTransform transform;
            var bounds = new RectangleF(0, 0, width, height);

            if (width > kMaxResolution || height > kMaxResolution)
            {
                var ratio = width / height;

                if (ratio > 1)
                {
                    bounds.Width = kMaxResolution;
                    bounds.Height = bounds.Width / ratio;
                }
                else
                {
                    bounds.Height = kMaxResolution;
                    bounds.Width = bounds.Height * ratio;
                }
            }

            var scaleRatio = bounds.Width / width;
            var imageSize = new SizeF(width, height);
            var orient = orIn;
            float boundHeight;

            switch (orient)
            {
                case UIImageOrientation.Up: //EXIF = 1
                    transform = CGAffineTransform.MakeIdentity();
                    break;

                case UIImageOrientation.UpMirrored: //EXIF = 2
                    transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    break;

                case UIImageOrientation.Down: //EXIF = 3
                    transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
                    break;

                case UIImageOrientation.DownMirrored: //EXIF = 4
                    transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
                    transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
                    break;

                case UIImageOrientation.LeftMirrored: //EXIF = 5
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.Left: //EXIF = 6
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
                    transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.RightMirrored: //EXIF = 7
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.Right: //EXIF = 8
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                    break;

                default:
                    throw new Exception("Invalid image orientation");
            }

            UIGraphics.BeginImageContext(bounds.Size);

            var context = UIGraphics.GetCurrentContext();

            if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
            {
                context.ScaleCTM(-scaleRatio, scaleRatio);
                context.TranslateCTM(-height, 0);
            }
            else
            {
                context.ScaleCTM(scaleRatio, -scaleRatio);
                context.TranslateCTM(0, -height);
            }

            context.ConcatCTM(transform);
            context.DrawImage(new RectangleF(0, 0, width, height), imgRef);

            var imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy;
        }

        /// <summary>
        ///     Loads the image view from base64.
        ///     Nothing happen if string is null or empty
        /// </summary>
        /// <param name="imgView">Image view.</param>
        /// <param name="base64">Base64.</param>
        public static void LoadFromBase64(this UIImageView imgView, string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return;
            var data = new NSData(base64, NSDataBase64DecodingOptions.None);
            var img = UIImage.LoadFromData(data);
            imgView.Image = img;
        }

        public static void CropImage(this UIImageView imgView)
        {
            var rect = imgView.Image.CropRectForImage();
            using (var cg = imgView.Image.CGImage.WithImageInRect(rect))
            {
                imgView.Image = UIImage.FromImage(cg);
            }
        }

        public static void AnimateHighlight(this UIImageView imgView, bool highlighted, float duration = 0.35f)
        {
            var expandTransform = CGAffineTransform.MakeScale(1.15f, 1.15f);
            var initDur = duration * 0.25f;
            var lastDur = duration * 0.75f;

            UIView.TransitionNotify(imgView, initDur, UIViewAnimationOptions.TransitionCrossDissolve,
                () =>
                {
                    imgView.Highlighted = highlighted;
                    imgView.Transform = expandTransform;
                }, finished =>
                {
                    UIView.AnimateNotify(lastDur, 0.0f, 0.4f, 0.2f, UIViewAnimationOptions.CurveEaseOut, () =>
                        imgView.Transform = CGAffineTransform.CGAffineTransformInvert(expandTransform), null);
                });
        }
    }
}