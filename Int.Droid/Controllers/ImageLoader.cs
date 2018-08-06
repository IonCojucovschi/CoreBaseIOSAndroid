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

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Int.Droid.Controllers
{
    [Register("Int.Droid.Controllers.ImageLoader")]
    public class ImageLoader : LinearLayout
    {
        private ImageView _imageView;
        private ProgressBar _progressBar;

        public ImageLoader(Context context)
            : base(context)
        {
            Init();
        }

        public ImageLoader(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Init();
        }

        public ImageLoader(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Init();
        }

        public void ShowLoading(bool show)
        {
            _progressBar.Visibility = show ? ViewStates.Visible : ViewStates.Invisible;
            _imageView.Visibility = show ? ViewStates.Invisible : ViewStates.Visible;
        }

        public void SetImageBitmap(Bitmap bitmap)
        {
            _imageView.SetImageBitmap(bitmap);
            ShowLoading(false);
        }

        private void Init()
        {
            var view = LayoutInflater.FromContext(Context)
                .Inflate(Resource.Layout.ImageLoader, this, false);
            _progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar1);
            _imageView = view.FindViewById<ImageView>(Resource.Id.imageView1);
            _imageView.Visibility = ViewStates.Invisible;
            _imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
            AddView(view);
        }

        public sealed override void AddView(View child)
        {
            base.AddView(child);
        }
    }
}