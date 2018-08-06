//
// ComponentActivity.cs
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
using System.Collections.Generic;
using System.Threading;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Int.Core.IO.Config;
using Int.Droid.Extensions;
using Int.Droid.Helpers;
using Int.Droid.Wrappers;
using Int.Droid.Window;
using Int.Core.Application.Window.Contract;

namespace Int.Droid.Factories.Activity
{
    public abstract class ComponentActivity : FragmentActivity
    {
        public const int PERMISSIONS_REQUEST_WRITE_EXTERNAL_STORAGE = 1;
        public const int PERMISSIONS_REQUEST_READ_PHONE_STATE = 2;
        public const int PERMISSIONS_REQUEST_CAMERA = 3;
        public const int PERMISSIONS_REQUEST_MICROPHONE = 4;
        public const int PERMISSIONS_REQUEST_LOCATION = 5;

        protected Dictionary<PermissionType, bool> ActivityPermissions = new Dictionary<PermissionType, bool>
        {
            {PermissionType.Camera, false},
            {PermissionType.ReadPhoneState, false},
            {PermissionType.WriteExternalStorage, false},
            {PermissionType.Microphone, false},
            {PermissionType.Location, false}
        };

        public string TitlePage { get; set; } = string.Empty;
        public virtual bool GlobalFont { get; } = false;
        public virtual View RootViewApp { get; set; }
        protected abstract int LayoutResource { get; }

        protected virtual void OnPrimeToStartApp()
        {
        }

        protected virtual void OnAfter()
        {
        }

        protected abstract void FindViews();
        protected abstract void InitViews();

        protected abstract void HandlerViews();
        protected abstract void RemoveHandlerViews();
        protected abstract void TranslateViews();

        protected virtual void SetFontsViews()
        {
        }

        public virtual void CloseApp()
        {
            Utility.CloseApp(this);
        }

        protected virtual void TemplateMethod()
        {
            AppTools.AppContext = this;

            FindViews();
            InjectAndCallUISetup();
            InitViews();
            HandlerViews();
            TranslateViews();
            ConfigurationPage();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AppTools.CurrentActivity = this;

            try
            {
                SetContentView(LayoutResource);
            }
            catch
            {
            }

            RootViewApp = FindViewById<View>(Android.Resource.Id.Content);
            TemplateMethod();
        }

        private void InjectAndCallUISetup()
        {
            Cheeseknife.Inject(this);
        }

        protected override void OnResume()
        {
            base.OnResume();

            AppTools.AppContext = this;
            AppTools.CurrentActivity = this;

            OnAfter();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            OnPrimeToStartApp();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected virtual void GoToActivity<T>() where T : Android.App.Activity
        {
            this.GoToScreen<T>();
        }

        public virtual void ConfigurationPage()
        {
            SetFontsViews();
        }

        protected virtual void GoToBackActivity()
        {
            Finish();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            GoToBackActivity();
        }

        public virtual void ReleaseView()
        {
            RemoveHandlerViews();
            if (!(RootViewApp is ViewGroup))
                return;
            RootViewApp.Background?.SetCallback(null);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            for (var i = 0; i < ((ViewGroup)RootViewApp).ChildCount; i++)
                ((ViewGroup)RootViewApp).GetChildAt(i);
            ((ViewGroup)RootViewApp).RemoveAllViews();
            Cheeseknife.Reset(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ThreadPool.QueueUserWorkItem(_ => MainThreadDispatcher.Post(ReleaseView));
        }

        public override void OnLowMemory()
        {
            GC.Collect();
            base.OnLowMemory();
        }

        public override void OnTrimMemory(TrimMemory level)
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            base.OnTrimMemory(level);
        }

        protected bool GetPermission(PermissionType permission, Action onPermissionDenied = null)
        {
            var permissionType = "";
            var permissionId = 0;
            switch (permission)
            {
                case PermissionType.Camera:
                    {
                        permissionType = Manifest.Permission.Camera;
                        permissionId = PERMISSIONS_REQUEST_CAMERA;
                    }
                    break;
                case PermissionType.ReadPhoneState:
                    {
                        permissionType = Manifest.Permission.ReadPhoneState;
                        permissionId = PERMISSIONS_REQUEST_READ_PHONE_STATE;
                    }
                    break;
                case PermissionType.WriteExternalStorage:
                    {
                        permissionType = Manifest.Permission.WriteExternalStorage;
                        permissionId = PERMISSIONS_REQUEST_WRITE_EXTERNAL_STORAGE;
                    }
                    break;
                case PermissionType.Microphone:
                    {
                        permissionType = Manifest.Permission.RecordAudio;
                        permissionId = PERMISSIONS_REQUEST_MICROPHONE;
                    }
                    break;
                case PermissionType.Location:
                    {
                        permissionType = Manifest.Permission.AccessFineLocation;
                        permissionId = PERMISSIONS_REQUEST_LOCATION;
                    }
                    break;
            }
            if (string.IsNullOrEmpty(permissionType))
                return false;

            var permissionCheck = ContextCompat.CheckSelfPermission(this, permissionType);
            if (permissionCheck == Permission.Denied)
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.CallPhone))
                    onPermissionDenied?.Invoke();
                else
                    ActivityCompat.RequestPermissions(this, new[] { permissionType }, permissionId);
                ActivityPermissions[permission] = false;
                return ActivityPermissions[permission];
            }

            ActivityPermissions[permission] = true;
            return ActivityPermissions[permission];
        }

        protected enum PermissionType
        {
            WriteExternalStorage,
            ReadPhoneState,
            Camera,
            Microphone,
            Location
        }

        #region AndHUD

        public virtual void Hide()
        {
            AppTools.RunOnUiThread(WindowShare.Instance.Hide);
        }

        public virtual void Show()
        {
            AppTools.RunOnUiThread(() =>
            {
                WindowShare.Instance.Show(DialogConfig.Instance.Wait,
                                          TimeIWindow.Normal);
            });
        }

        public virtual void Show(string text)
        {
            AppTools.RunOnUiThread(() =>
            {
                WindowShare.Instance.Show(text,
                                          TimeIWindow.Normal);
            });
        }

        public virtual void ShowSuccess(string success = null, int second = 3)
        {
            AppTools.RunOnUiThread(() =>
            {
                WindowShare.Instance.ShowSuccess(success ?? DialogConfig.Instance.Success,
                                          TimeIWindow.Normal);
            });
        }

        public virtual void ShowError(string error = null, int second = 3)
        {
            AppTools.RunOnUiThread(() =>
            {
                WindowShare.Instance.ShowError(error ?? DialogConfig.Instance.Error,
                                          TimeIWindow.Normal);
            });
        }

        #endregion
    }
}