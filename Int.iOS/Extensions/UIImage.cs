//
// UIImage.cs
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
using System.IO;
using System.Threading.Tasks;
using CoreGraphics;
using CoreImage;
using Foundation;
using Int.Core.Extensions;
using UIKit;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        public static UIImage ChangeColor(this UIImage img, UIColor color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));
            UIGraphics.BeginImageContextWithOptions(img.Size, false, img.CurrentScale);
            var ctx = UIGraphics.GetCurrentContext();
            color.SetFill();
            ctx.TranslateCTM(0, img.Size.Height);
            ctx.ScaleCTM(1f, -1f);
            var rect = new CGRect(CGPoint.Empty, img.Size);
            ctx.ClipToMask(rect, img.CGImage);
            ctx.FillRect(rect);
            var coloredImg = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return coloredImg;
        }

        public static void ChangeColorTint(this UIImageView img, UIColor color)
        {
            img.Image = img?.Image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            img.TintColor = color;
        }

        public static UIImage ScaleAndCropImage(this UIImage sourceImage, SizeF targetSize)
        {
            var imageSize = sourceImage.Size;
            UIImage newImage = null;
            var width = imageSize.Width;
            var height = imageSize.Height;
            var targetWidth = targetSize.Width;
            var targetHeight = targetSize.Height;
            nfloat scaledWidth = targetWidth;
            nfloat scaledHeight = targetHeight;
            var thumbnailPoint = PointF.Empty;
            if (imageSize != targetSize)
            {
                var widthFactor = targetWidth / width;
                var heightFactor = targetHeight / height;
                nfloat scaleFactor = 0.0f;
                scaleFactor = widthFactor > heightFactor ? widthFactor : heightFactor;
                scaledWidth = width * scaleFactor;
                scaledHeight = height * scaleFactor;
                // center the image
                if (widthFactor > heightFactor)
                {
                    thumbnailPoint.Y = (targetHeight - (float)scaledHeight) * 0.5f;
                }
                else
                {
                    if (widthFactor < heightFactor)
                        thumbnailPoint.X = (targetWidth - (float)scaledWidth) * 0.5f;
                }
            }

            UIGraphics.BeginImageContextWithOptions(targetSize, false, 0.0f);
            var thumbnailRect = new RectangleF(thumbnailPoint, new SizeF((float)scaledWidth, (float)scaledHeight));
            sourceImage.Draw(thumbnailRect);
            newImage = UIGraphics.GetImageFromCurrentImageContext();
            if (newImage == null)
                Console.WriteLine("could not scale image");
            //pop the context to get back to the default
            UIGraphics.EndImageContext();

            return newImage;
        }

        public static UIImage Resize(this UIImage img, CGSize newSize)
        {
            var rect = new CGRect(0, 0, newSize.Width, newSize.Height);
            UIGraphics.BeginImageContextWithOptions(newSize, false, 1.0f);
            img.Draw(rect);
            var newImg = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return newImg;
        }

        public static UIImage ResizeImage(this UIImage sourceImage, float width, float height)
        {
            UIGraphics.BeginImageContext(new SizeF(width, height));
            sourceImage.Draw(new RectangleF(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        public static UIImage ScaleImage(this UIImage image, int maxSize)
        {
            UIImage res;

            using (var imageRef = image.CGImage)
            {
                var alphaInfo = imageRef.AlphaInfo;
                var colorSpaceInfo = CGColorSpace.CreateDeviceRGB();
                if (alphaInfo == CGImageAlphaInfo.None)
                    alphaInfo = CGImageAlphaInfo.NoneSkipLast;

                var width = imageRef.Width;
                var height = imageRef.Height;


                if (height >= width)
                {
                    width = (int)Math.Floor(width * (maxSize / (double)height));
                    height = maxSize;
                }
                else
                {
                    height = (int)Math.Floor(height * (maxSize / (double)width));
                    width = maxSize;
                }


                CGBitmapContext bitmap;

                if (image.Orientation == UIImageOrientation.Up || image.Orientation == UIImageOrientation.Down)
                {
                    var bytesPerRow = GetBytesPerRow(width);
                    bitmap = new CGBitmapContext(IntPtr.Zero, width, height, imageRef.BitsPerComponent, bytesPerRow,
                        colorSpaceInfo, alphaInfo);
                }
                else
                {
                    var bytesPerRow = GetBytesPerRow(height);
                    bitmap = new CGBitmapContext(IntPtr.Zero, height, width, imageRef.BitsPerComponent, bytesPerRow,
                        colorSpaceInfo, alphaInfo);
                }

                switch (image.Orientation)
                {
                    case UIImageOrientation.Left:
                        bitmap.RotateCTM((float)Math.PI / 2);
                        bitmap.TranslateCTM(0, -height);
                        break;
                    case UIImageOrientation.Right:
                        bitmap.RotateCTM(-((float)Math.PI / 2));
                        bitmap.TranslateCTM(-width, 0);
                        break;
                    case UIImageOrientation.Up:
                        break;
                    case UIImageOrientation.Down:
                        bitmap.TranslateCTM(width, height);
                        bitmap.RotateCTM(-(float)Math.PI);
                        break;
                }

                bitmap.DrawImage(new Rectangle(0, 0, (int)width, (int)height), imageRef);


                res = UIImage.FromImage(bitmap.ToImage());
                bitmap = null;
            }


            return res;
        }

        private static nint GetBytesPerRow(nint width)
        {
            const int bytesPerPixel = 4;
            const int cacheLineSize = 64;

            var result =
                new nint((int)(Math.Ceiling((double)(width * bytesPerPixel) / cacheLineSize) * cacheLineSize));

            return result;
        }

        public static UIImage MaxResizeImage(this UIImage sourceImage, float maxWidth, float maxHeight)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new SizeF((float)width, (float)height));
            sourceImage.Draw(new RectangleF(0, 0, (float)width, (float)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        public static UIImage CropImage(this UIImage sourceImage, int cropX, int cropY, int width, int height)
        {
            var imgSize = sourceImage.Size;
            UIGraphics.BeginImageContext(new SizeF(width, height));
            var context = UIGraphics.GetCurrentContext();
            var clippedRect = new RectangleF(0, 0, width, height);
            context.ClipToRect(clippedRect);
            var drawRect = new RectangleF(-cropX, -cropY, (float)imgSize.Width, (float)imgSize.Height);
            sourceImage.Draw(drawRect);
            var modifiedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return modifiedImage;
        }

        public static UIImage ResizeImageIOS(this UIImage sourceImage, float width, float height)
        {
            var originalImage = sourceImage;

            using (var context = new CGBitmapContext(IntPtr.Zero,
                (int)width, (int)height, 8,
                (int)(4 * width), CGColorSpace.CreateDeviceRGB(),
                CGImageAlphaInfo.PremultipliedFirst))
            {
                var imageRect = new RectangleF(0, 0, width, height);

                context.DrawImage(imageRect, originalImage.CGImage);


                var resizedImage = UIImage.FromImage(context.ToImage(), 0, UIImageOrientation.Up);
                return resizedImage;
            }
        }

        public static UIImage MaskImage(this UIImage img, UIImage maskImage)
        {
            var maskRef = maskImage.CGImage;
            var mask = CGImage.CreateMask((int)maskRef.Width, (int)maskRef.Height,
                (int)maskRef.BitsPerComponent, (int)maskRef.BitsPerPixel,
                (int)maskRef.BytesPerRow, maskRef.DataProvider, null, false);
            var masked = img.CGImage.WithMask(mask);
            return new UIImage(masked);
        }

        public static async Task<UIImage> FromUrlAsync(string url)
        {
            return UIImage.LoadFromData(NSData.FromArray(await url.GetUrl()));
        }

        public static UIImage MaskImageOnAlpha(this UIImage img, UIImage maskImg, bool inverse = false)
        {
            byte[] originalData, maskData;
            var originalCtx = CreateContext(img.Size, out originalData);
            var maskCtx = CreateContext(maskImg.Size, out maskData);
            originalCtx.DrawImage(new CGRect(0, 0, img.Size.Width, img.Size.Height), img.CGImage);
            maskCtx.DrawImage(new CGRect(0, 0, maskImg.Size.Width, maskImg.Size.Height), maskImg.CGImage);
            for (var i = 0; i < originalData.Length; i += 4)
            {
                var a = maskData[i + 3];
                if (inverse)
                {
                    if (a == 0) continue;
                    originalData[i] = 0;
                    originalData[i + 1] = 0;
                    originalData[i + 2] = 0;
                    originalData[i + 3] = 0;
                }
                else
                {
                    if (a != 0) continue;
                    originalData[i] = 0;
                    originalData[i + 1] = 0;
                    originalData[i + 2] = 0;
                    originalData[i + 3] = 0;
                }
            }
            var newImg = UIImage.FromImage(originalCtx.ToImage());
            originalCtx.Dispose();
            maskCtx.Dispose();
            return newImg;
        }

        public static string ToBase64(this UIImage img)
        {
            var nsdata = img.AsPNG().ToArray();
            return Convert.ToBase64String(nsdata);
        }

        public static UIImage ToGrayScale(this UIImage img)
        {
            var imgRect = new CGRect(0, 0, img.Size.Width, img.Size.Height);
            using (var colorSpace = CGColorSpace.CreateDeviceGray())
            {
                using (var ctx = new CGBitmapContext(
                    IntPtr.Zero, (nint)img.Size.Width, (nint)img.Size.Height, 8, 0,
                    colorSpace, CGImageAlphaInfo.None))
                {
                    ctx.DrawImage(imgRect, img.CGImage);
                    return new UIImage(ctx.ToImage());
                }
            }
        }

        public static UIImage ToImage(this byte[] data)
        {
            if (data == null)
                return null;
            UIImage image;
            try
            {
                image = new UIImage(NSData.FromArray(data));
            }
            catch (Exception)
            {
                return null;
            }
            return image;
        }

        public static UIImage InvertColors(this UIImage img)
        {
            using (var coreImg = new CIImage(img.CGImage))
            {
                var filter = new CIColorInvert
                {
                    Image = coreImg
                };
                var output = filter.OutputImage;
                var ctx = CIContext.FromOptions(null);
                var cgimage = ctx.CreateCGImage(output, output.Extent);
                return UIImage.FromImage(cgimage);
            }
        }

        public static void ToImage(this UIImageView img, string imageBase64)
        {
            img.Image = UIImage.LoadFromData(NSData.FromArray(Convert.FromBase64String(imageBase64)));
        }

        public static UIImage GetMaskFromAlpha(this UIImage img)
        {
            using (var ctx = CreateContext(img.Size, out var data))
            {
                ctx.DrawImage(new CGRect(0, 0, img.Size.Width, img.Size.Height), img.CGImage);
                var white = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                var black = new byte[] { 0xFF, 0x00, 0x00, 0x00 };

                for (var i = (nint)0; i < data.Length; i += 4)
                {
                    var a = data[i + 3];
                    if (a == 0)
                    {
                        white.CopyTo(data, (int)i);
                        continue;
                    }
                    var r = data[i];
                    var g = data[i + 1];
                    var b = data[i + 2];
                    if (r != 0 || g != 0 || b != 0)
                        black.CopyTo(data, i);
                }
                return UIImage.FromImage(ctx.ToImage());
            }
        }


        public static void SaveToFile(this UIImage img, string path)
        {
            var data = img.AsPNG();
            File.WriteAllBytes(path, data.ToArray());
        }

        public static async void SaveToAlbum(this UIImage img, Action success, Action<string> error)
        {
            if (await AppTools.CheckGalleryPermission())
                img.SaveToPhotosAlbum((image, err) =>
                {
                    if (!err.IsNull() && !err.LocalizedDescription.IsNullOrWhiteSpace())
                        error?.Invoke(err.LocalizedDescription);
                    else
                        success?.Invoke();
                });
        }

        public static CGRect CropRectForImage(this UIImage img)
        {
            var cgImage = img.CGImage;
            var width = cgImage.Width;
            var height = cgImage.Height;
            var bytesPerRow = width * 4;
            var byteCount = bytesPerRow * height;

            var colorSpace = CGColorSpace.CreateDeviceRGB();


            var data = new byte[byteCount];
            using (var ctx = new CGBitmapContext(data, width, height, 8,
                bytesPerRow, colorSpace, CGImageAlphaInfo.PremultipliedLast))
            {
                var rect = new CGRect(0, 0, width, height);

                ctx.DrawImage(rect, cgImage);


                var lowX = width;
                var lowY = height;
                var highX = (nint)0;
                var highY = (nint)0;


                for (var y = 0; y < height; y++)
                    for (var x = 0; x < width; x++)
                    {
                        var pixelIndex = (width * y + x) * 4;
                        if (data[pixelIndex] == 0) continue;
                        if (x < lowX) lowX = x;
                        if (x > highX) highX = x;
                        if (y < lowY) lowY = y;
                        if (y > highY) highY = y;
                    }
                return new CGRect(lowX, lowY, highX - lowX, highY - lowY);
            }
        }

        public static UIImage GetSubImage(this UIImage img, CGRect rect)
        {
            UIGraphics.BeginImageContext(rect.Size);
            var ctx = UIGraphics.GetCurrentContext();

            var drawRect = new CGRect(-rect.X, -rect.Y, img.Size.Width, img.Size.Height);

            ctx.ClipToRect(drawRect);

            img.Draw(drawRect);

            var subImg = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return subImg;
        }


        public static CGBitmapContext CreateContext(CGSize size, out byte[] data)
        {
            return CreateContext(size, out data, CGImageAlphaInfo.PremultipliedFirst);
        }

        public static CGBitmapContext CreateContext(CGSize size, out byte[] data, CGImageAlphaInfo bitmapInfo)
        {
            var width = (int)size.Width;
            var height = (int)size.Height;
            var bytesPerRow = width * 4;
            var byteCount = bytesPerRow * height;

            var colorSpace = CGColorSpace.CreateDeviceRGB();

            data = new byte[byteCount];
            return new CGBitmapContext(data, width, height, 8,
                bytesPerRow, colorSpace, bitmapInfo);
        }
    }
}