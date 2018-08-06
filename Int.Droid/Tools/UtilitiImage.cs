//
//  UtilitiImage.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2017 Songurov
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

//using System;
//using Android.Graphics;
//using Android.Graphics.Drawables;
//using Android.Views;
//using Android.Widget;
//using Com.Bumptech.Glide;
//using Com.Bumptech.Glide.Load.Resource.Bitmap;
//using Com.Bumptech.Glide.Request;
//using Com.Bumptech.Glide.Request.Target;
//using ImageViews.Photo;
//using Int.Core.Extensions;
//using Int.Droid.Extensions;
//using Exception = Java.Lang.Exception;
//using Object = Java.Lang.Object;

namespace Int.Droid.Tools
{
    public class UtilityImage  //: Object, IRequestListener
    {
        //private static readonly Lazy<UtilityImage> Service = new Lazy<UtilityImage>(() => new UtilityImage());

        //private static Bitmap _bitmapSave;

        //private ImageView _close;

        //private string _imageUrl;

        //private PopupWindow _popupWindow;

        //private ProgressBar _progressBar;
        //private View _rootView;
        //private ImageView _share;

        //private ImageView _zoomImage;

        //private PhotoViewAttacher attacher;

        //protected UtilityImage()
        //{
        //}

        //public static UtilityImage Instance => Service.Value;

        //public bool OnException(Exception p0, Object p1, ITarget p2, bool p3)
        //{
        //    return true;
        //}

        //public bool OnResourceReady(Object p0, Object p1, ITarget p2, bool p3, bool p4)
        //{
        //    _bitmapSave = (p0 as GlideBitmapDrawable)?.Bitmap;
        //    _progressBar.Visibility = ViewStates.Gone;

        //    return false;
        //}

        //public new void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //private void ConfigPopup()
        //{
        //    _popupWindow = new PopupWindow(_rootView, ViewGroup.LayoutParams.MatchParent,
        //        ViewGroup.LayoutParams.MatchParent, true)
        //    {
        //        Touchable = true,
        //        Focusable = true,
        //        OutsideTouchable = true
        //    };
        //    _popupWindow.SetBackgroundDrawable(new BitmapDrawable());
        //}

        //private void InitViews(int resLayout = 0)
        //{
        //    try
        //    {
        //        _rootView = AppTools.CurrentActivity.LayoutInflater.Inflate(
        //            resLayout == 0 ? Resource.Layout.ZoomView : resLayout, null);

        //        _zoomImage = _rootView.FindViewById<ImageView>(Resource.Id.imageViewZoom);
        //        _close = _rootView.FindViewById<ImageView>(Resource.Id.imageViewClose);
        //        _share = _rootView.FindViewById<ImageView>(Resource.Id.imageViewShare);
        //        _progressBar = _rootView.FindViewById<ProgressBar>(Resource.Id.progressBar);

        //        attacher = new PhotoViewAttacher(_zoomImage);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Toast.MakeText(AppTools.CurrentActivity, ex.Message, ToastLength.Long)
        //            .Show();
        //    }
        //}

        //public UtilityImage Open(int resImageClose, int resImageShare, string imageUrl, int resLayout = 0)
        //{
        //    InitViews(resLayout);
        //    ConfigPopup();

        //    try
        //    {
        //        _close.SetImage(resImageClose);
        //        _share.SetImage(resImageShare);

        //        _close.Click -= _close_Click;
        //        _close.Click += _close_Click;

        //        _imageUrl = imageUrl;

        //        if (!_imageUrl.IsNullOrWhiteSpace() && !_imageUrl.ToHttps().IsNullOrWhiteSpace())
        //            Glide.With(AppTools.CurrentActivity)
        //                 .Load(_imageUrl?.ToHttps())
        //                 .Listener(this).Into(_zoomImage);

        //        _share.Click += _share_Click;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Toast.MakeText(AppTools.CurrentActivity, ex.Message, ToastLength.Long)
        //            .Show();
        //    }

        //    return this;
        //}

        //public void Show()
        //{
        //    _popupWindow.ShowAtLocation(AppTools.CurrentActivity.Window.DecorView, GravityFlags.NoGravity, 0, 0);
        //}

        //public void Dismiss()
        //{
        //    _popupWindow.Dismiss();
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //        _popupWindow.Dispose();
        //}

        //private void _close_Click(object sender, EventArgs e)
        //{
        //    Dismiss();
        //}

        //private void _share_Click(object sender, EventArgs e)
        //{
        //    AppTools.CurrentActivity.ShareApp(_imageUrl, _bitmapSave);
        //}
    }
}