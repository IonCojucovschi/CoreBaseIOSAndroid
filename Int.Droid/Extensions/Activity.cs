//
// Activity.cs
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
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Android.Views;
using Android.Views.InputMethods;
using Int.Core.Extensions;
using Uri = Android.Net.Uri;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        public static void HideKeyboard(this Activity activity)
        {
            ((InputMethodManager)activity.GetSystemService(Context.InputMethodService)).HideSoftInputFromWindow(
                (activity.CurrentFocus ?? activity.FindViewById(Android.Resource.Id.Content) ?? new View(activity))
                .WindowToken, 0);
        }

        public static void ShowKeyboard(this Activity activity)
        {
            ((InputMethodManager)activity.GetSystemService(Context.InputMethodService)).ShowSoftInput(
                activity.CurrentFocus ?? activity.FindViewById(Android.Resource.Id.Content) ?? new View(activity), 0);
        }

        private static Activity ConfigScreen(Activity activity, Type nextScreen, ActivityFlags flag)
        {
            if (nextScreen.IsNull()) return null;
            activity.RunOnUiThread(() =>
            {
                var intent = new Intent(activity, nextScreen);
                intent.AddFlags(flag);
                activity.StartActivity(intent);
            });

            return activity;
        }

        public static void ShareApp(this Activity activity, string imageUrl, Bitmap image)
        {
            var path = MediaStore.Images.Media.InsertImage(AppTools.CurrentActivity.ContentResolver, image,
                "Image Description", null);
            var uri = Uri.Parse(path);

            var i = new Intent(Intent.ActionSend);
            i.SetType("image/*");
            i.PutExtra(Intent.ExtraText, "Share Image");
            i.AddFlags(ActivityFlags.GrantReadUriPermission);

            i.PutExtra(Intent.ExtraText, imageUrl);
            i.PutExtra(Intent.ExtraStream, uri);
            activity.StartActivity(Intent.CreateChooser(i, "Share via"));
        }

        public static void ShareApp(this Activity activity, Uri imageUrl)
        {
            var i = new Intent(Intent.ActionSend);
            i.SetType("image/*");
            i.AddFlags(ActivityFlags.GrantReadUriPermission);
            i.PutExtra(Intent.ExtraStream, imageUrl);
            activity.StartActivity(Intent.CreateChooser(i, "Share via"));
        }

        public static void GoToScreen<T>(this Activity activity, bool finishActivity = false,
            ActivityFlags flag = ActivityFlags.NewTask | ActivityFlags.ClearTop, int animationRight = -1,
            int animationLeft = -1)
        {
            activity.RunOnUiThread(() =>
            {
                if (finishActivity)
                {
                    activity.Finish();
                    activity.OverridePendingTransition(animationRight, animationLeft);
                    return;
                }

                var resultActivity = ConfigScreen(activity, typeof(T), flag);

                if (resultActivity != null && (animationLeft > -1 || animationRight > -1))
                    resultActivity.OverridePendingTransition(animationRight, animationLeft);
            });
        }


        public static void GoToScreen(this Activity activity, Type type, bool finishActivity = false,
            ActivityFlags flag = ActivityFlags.NewTask | ActivityFlags.ClearTop, int animationRight = -1,
            int animationLeft = -1)
        {
            activity.RunOnUiThread(() =>
            {
                if (finishActivity)
                {
                    activity.Finish();
                    activity.OverridePendingTransition(animationRight, animationLeft);
                    return;
                }

                var resultActivity = ConfigScreen(activity, type, flag);

                if (resultActivity != null && (animationLeft > -1 || animationRight > -1))
                    resultActivity.OverridePendingTransition(animationRight, animationLeft);
            });
        }

        public static void ApplicationClose(this Activity activity, Type nextScreen)
        {
            var exiterActivityIntent = new Intent(activity, nextScreen);
            exiterActivityIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask |
                                          ActivityFlags.ExcludeFromRecents);
            activity.StartActivity(exiterActivityIntent);
        }

        public static int GetStatusBarHeight(this Activity activity)
        {
            var resourceId = activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            return resourceId > 0 ? activity.Resources.GetDimensionPixelSize(resourceId) : 0;
        }

        public static void CloseApp(this Activity activity)
        {
            Int.Droid.Helpers.Utility.CloseApp(activity);
        }
    }
}