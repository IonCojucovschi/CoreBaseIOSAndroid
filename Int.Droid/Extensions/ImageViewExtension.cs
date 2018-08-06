using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Int.Droid.Views;

namespace Int.Droid.Extensions
{
    public static class ImageViewExtension
    {

        private enum Type
        {
            File,
            Res
        }

        private class Options
        {
            public int Width { get; set; }

            public int Height { get; set; }

            public Type Type { get; set; }

            public object Path { get; set; }
        }

        private class NResource
        {
            public int Id { get; set; }

            public Resources Resources { get; set; }

        }

        public static void Recycle(this ImageView imageView)
        {
            if (imageView.Drawable == null)
                return;
            imageView.SetImageDrawable(null);
        }

        public static async Task LoadFromFileAsync(this ImageView imageView, string path)
        {
            if (!File.Exists(path))
                return;
            var options = new Options
            {
                Path = path,
                Type = Type.File
            };
            await LoadAsync(imageView, options);
        }

        public static async Task LoadFromResAsync(this ImageView imageView, Resources resource, int id)
        {
            var options = new Options
            {
                Type = Type.Res,
                Path = new NResource
                {
                    Id = id,
                    Resources = resource
                }
            };
            await LoadAsync(imageView, options);
        }

        private static async Task LoadAsync(this ImageView imageView, Options options)
        {
            if (imageView.Width == 0 || imageView.Height == 0)
            {
                try
                {
                    var globalLisener = new GlobalLayoutListener(imageView, true);
                    EventHandler handler = null;
                    handler = async (s, e) =>
                    {
                        globalLisener.GlobalLayout -= handler;
                        handler = null;
                        globalLisener = null;
                        SetSize(options, imageView);
                        imageView.SetImageBitmap(await DecodeBitmap(options));
                    };
                    globalLisener.GlobalLayout += handler;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return;
            }
            SetSize(options, imageView);
            imageView.SetImageBitmap(await DecodeBitmap(options));
        }

        private static void SetSize(Options options, View view)
        {
            options.Height = view.Height;
            options.Width = view.Width;
        }

        private static async Task<Bitmap> DecodeBitmap(Options options)
        {
            var option = new BitmapFactory.Options();
            option.InJustDecodeBounds = true;
            option.InPreferredConfig = Bitmap.Config.Rgb565;
            Bitmap bp = null;
            switch (options.Type)
            {
                case Type.File:
                    var path = (string)options.Path;
                    await BitmapFactory.DecodeFileAsync(path, option);

                    option.InSampleSize = CalculateInSampleSize(option,
                        options.Width, options.Height);

                    option.InJustDecodeBounds = false;

                    bp = await BitmapFactory.DecodeFileAsync(path, option);
                    break;
                case Type.Res:
                    var res = (NResource)options.Path;

                    await BitmapFactory.
                        DecodeResourceAsync(res.Resources, res.Id, option);

                    option.InSampleSize = CalculateInSampleSize(option,
                        options.Width, options.Height);

                    option.InJustDecodeBounds = false;

                    bp = await BitmapFactory.
                        DecodeResourceAsync(res.Resources, res.Id, option);
                    break;
            }
            return bp;
        }

        private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            var height = options.OutHeight;
            var width = options.OutWidth;
            var inSampleSize = 1;
            if (height > reqHeight || width > reqWidth)
            {
                var heightRatio = Math.Round((float)height / reqHeight);
                var widthRatio = Math.Round((float)width / reqWidth);

                inSampleSize = (int)(heightRatio < widthRatio ? heightRatio : widthRatio);

            }
            return inSampleSize;
        }


    }
}

