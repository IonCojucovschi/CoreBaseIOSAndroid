//
// Exif.cs
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
using Android.Graphics;
using Android.Media;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        public static int GetExifRotationDegrees(this string filePath)
        {
            int rotation;
            var exifInt = new ExifInterface(filePath);
            var exifRotation = exifInt.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Normal);

            switch (exifRotation)
            {
                case (int)Orientation.Rotate270:
                    rotation = 270;
                    break;
                case (int)Orientation.Rotate180:
                    rotation = 180;
                    break;
                case (int)Orientation.Rotate90:
                    rotation = 90;
                    break;
                default:
                    return 0;
            }

            return rotation;
        }

        public static Bitmap ToRotatedBitmap(this Bitmap sourceBitmap, int rotationDegrees)
        {
            if (rotationDegrees == 0)
                return sourceBitmap;

            var width = sourceBitmap.Width;
            var height = sourceBitmap.Height;

            if (rotationDegrees == 90 || rotationDegrees == 270)
            {
                width = sourceBitmap.Height;
                height = sourceBitmap.Width;
            }

            var bitmap = Bitmap.CreateBitmap(width, height, sourceBitmap.GetConfig());
            using (var canvas = new Canvas(bitmap))
            {
                using (var paint = new Paint())
                {
                    using (var shader = new BitmapShader(sourceBitmap, Shader.TileMode.Clamp, Shader.TileMode.Clamp))
                    {
                        using (var matrix = new Matrix())
                        {
                            canvas.Save();

                            switch (rotationDegrees)
                            {
                                case 90:
                                    canvas.Rotate(rotationDegrees, width / 2, width / 2);
                                    break;
                                case 270:
                                    canvas.Rotate(rotationDegrees, height / 2, height / 2);
                                    break;
                                default:
                                    canvas.Rotate(rotationDegrees, width / 2, height / 2);
                                    break;
                            }

                            canvas.DrawBitmap(sourceBitmap, matrix, paint);
                            canvas.Restore();
                        }
                    }
                }
            }

            if (sourceBitmap.Handle == IntPtr.Zero || sourceBitmap.IsRecycled) return bitmap;
            sourceBitmap.Recycle();
            sourceBitmap.Dispose();

            return bitmap;
        }
    }
}