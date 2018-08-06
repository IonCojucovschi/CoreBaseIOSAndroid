//
// Utility.cs
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
using Android.Content;
using Android.OS;
using Int.Droid.Views;
using Java.Text;
using Java.Util;
using Environment = System.Environment;

namespace Int.Droid.Helpers
{
    public static class Utility
    {
        public static string PersonalFolder => Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public static DateTime CurrentDateTimePhone
            => Convert.ToDateTime(DateFormat.DateTimeInstance.Format(Calendar.Instance.Time));

        public static bool HasFroyo()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.Froyo;
        }

        public static bool HasGingerbread()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.Gingerbread;
        }

        public static bool HasHoneycomb()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb;
        }

        public static bool HasHoneycombMr1()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.HoneycombMr1;
        }

        public static bool HasJellyBean()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean;
        }

        public static bool HasKitKat()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;
        }

        public static bool HasLollipop()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop;
        }

        public static void CloseApp(Context context)
        {
            var exiterActivityIntent = new Intent(context, typeof(ActivityClose));
            exiterActivityIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask |
                                          ActivityFlags.ExcludeFromRecents);
            context.StartActivity(exiterActivityIntent);
        }

        public static string VersionName(Context context = null)
        {
            return AppTools.CurrentActivity.PackageManager.GetPackageInfo(AppTools.CurrentActivity.PackageName, 0).VersionName;
        }

        public static int VersionCode(Context context = null)
        {
            return AppTools.CurrentActivity.PackageManager.GetPackageInfo(AppTools.CurrentActivity.PackageName, 0).VersionCode;
        }
    }
}