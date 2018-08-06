//
// AppTools.cs
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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Int.iOS.Factories.Activity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SafariServices;
using UIKit;

namespace Int.iOS
{
    public static class AppTools
    {
        public static string OpenTelprompt = "telprompt://";
        public static string OpenTel = "tel://";
        public static string MailTo = "mailto:";
        public static double SPix => UIScreen.MainScreen.Bounds.Size.Width * UIScreen.MainScreen.Scale;

        public static UIViewController TopViewController
        {
            get
            {
                var navController =
                    UIApplication.SharedApplication.Delegate.GetWindow().RootViewController as UINavigationController;
                return navController != null ? navController.ViewControllers.LastOrDefault() : RootNavigationController;
            }
        }

        public static UIStoryboard Storyboard =>
            UIApplication.SharedApplication.Delegate.GetWindow().RootViewController.Storyboard;

        public static UINavigationController RootNavigationController
            => UIApplication.SharedApplication.Delegate.GetWindow().RootViewController as UINavigationController;


        public static bool CanCall => UIApplication.SharedApplication.CanOpenUrl(new NSUrl(OpenTelprompt));

        public static string ApplicationName => NSBundle.MainBundle.InfoDictionary["CFBundleName"].ToString();

        public static UIViewController GetViewController<T>()
        {
            return Storyboard.InstantiateViewController(typeof(T).Name);
        }

        public static void HideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }

        public static void InvokeOnMainThread(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            UIApplication.SharedApplication.InvokeOnMainThread(action);
        }

        public static void OpenUrl(NSUrl url, Action<bool> completion)
        {
            UIApplication.SharedApplication.OpenUrl(url, new UIApplicationOpenUrlOptions(), completion);
        }

        public static IphoneType IphoneSizeAfter()
        {
            if (UIScreen.MainScreen.Bounds.Height == 667)
                return IphoneType.IPhnoneNormal;

            if (UIScreen.MainScreen.Bounds.Height > 667)
                return IphoneType.IPhonePlus;

            return IphoneType.IPhoneSe;
        }

        /// <summary>
        /// <key>UIStatusBarStyle</key>
        //  <string>UIStatusBarStyleLightContent</string>
        //  <key>UIViewControllerBasedStatusBarAppearance</key>
        //  <false/>
        /// </summary>
        /// <param name="uIColor">U IC olor.</param>
        public static void SetBackroundColorBarStyle(UIColor uIColor)
        {
            UINavigationBar.Appearance.BackgroundColor = uIColor;
        }

        /// <summary>
        /// <key>UIStatusBarStyle</key>
        //  <string>UIStatusBarStyleLightContent</string>
        //  <key>UIViewControllerBasedStatusBarAppearance</key>
        //  <false/>
        /// </summary>
        /// <param name="uIColor">U IC olor.</param>
        public static void SetTextColorBarStyle(UIColor uIColor)
        {
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
            {
                TextColor = uIColor
            });
        }

        public static void OpenLink(string link, UIViewController fromViewController = null)
        {
            var version = new Version(UIDevice.CurrentDevice.SystemVersion);
            if (version.Major >= 9)
            {
                var dest = new SFSafariViewController(new NSUrl(link), true)
                {
                    Delegate = new SafariDelegate()
                };

                if (fromViewController == null)
                    TopViewController.PresentViewController(dest, true, null);
                else
                    fromViewController.PresentViewController(dest, true, null);
            }
            else
            {
                OpenUrl(link);
            }
        }

        public static void OpenUrl(string url, Action<bool> completion)
        {
            OpenUrl(new NSUrl(url), completion);
        }

        public static bool OpenUrl(string url)
        {
            OpenUrl(url, null);
            return true;
        }

        public static async Task<bool> CheckGalleryPermission()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
            var hasPermissionToSaveToGallery = permissionStatus == PermissionStatus.Granted;
            if (!hasPermissionToSaveToGallery)
            {
                var permissionList = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Photos);
                if (permissionList.TryGetValue(Permission.Photos, out permissionStatus))
                    hasPermissionToSaveToGallery = permissionStatus == PermissionStatus.Granted;
            }

            return hasPermissionToSaveToGallery;
        }

        public static void GoToScreen<T>(bool animation = true) where T : ComponentViewController
        {
            InvokeOnMainThread(() =>
            {
                var navigationController = RootNavigationController;
                if (navigationController == null || Storyboard == null) return;
                navigationController.PushViewController(Storyboard.InstantiateViewController(typeof(T).Name),
                    animation);
            });
        }

        public static void OpenPhone(string number)
        {
            if (!CanCall) return;
            try
            {
                UIApplication.SharedApplication.OpenUrl(
                    new NSUrl(OpenTelprompt + number.Replace("+", "00").Replace(" ", "").Trim()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        public static void OpenMail(string mail)
        {
            try
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(MailTo + mail));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private class SafariDelegate : SFSafariViewControllerDelegate
        {
            public override void DidFinish(SFSafariViewController controller)
            {
                controller.DismissViewController(true, null);
            }
        }
    }

    public enum IphoneType
    {
        IPhoneSe,
        IPhnoneNormal,
        IPhonePlus
    }
}