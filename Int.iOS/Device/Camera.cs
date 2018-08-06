//
// Camera.cs
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
using UIKit;

namespace Int.iOS.Device
{
    public class Camera : DeviceBase<Camera>
    {
        public enum CImagePicker : long
        {
            PhotoLibrary = UIImagePickerControllerSourceType.PhotoLibrary,
            Camera = UIImagePickerControllerSourceType.Camera,
            Custom = Camera + 1
        }

        public static string Title { get; set; } = "Pick Source";
        public static string SCamera { get; set; } = "Camera";
        public static string PhotoLibrary { get; set; } = "Photo Library";

        public static bool AllowEditing { get; set; } = false;
        public static bool AllowImageEditing { get; set; } = false;

        public static void TakePicture(UIViewController parent, Action<UIImage> callback)
        {
            PickImageInternal(parent, UIImagePickerControllerSourceType.Camera, callback);
        }

        public static void SelectPicture(UIViewController parent, Action<UIImage> callback)
        {
            PickImageInternal(parent, UIImagePickerControllerSourceType.PhotoLibrary, callback);
        }

        public static void ShowOptions(UIViewController parent, Action<UIImage> callback,
            IList<ImagePicker> actions = null)
        {
            ShowImageActions(parent, action =>
            {
                if (action.SourceType == CImagePicker.Custom)
                {
                    action.Action?.Invoke(_ => callback?.Invoke(_));
                    return;
                }
                PickImageInternal(parent, (UIImagePickerControllerSourceType) action.SourceType, callback);
            }, actions);
        }

        private static void PickImageInternal(UIViewController parent, UIImagePickerControllerSourceType source,
            Action<UIImage> callback)
        {
            var imagePicker = new UIImagePickerController
            {
                AllowsEditing = AllowEditing,
                AllowsImageEditing = AllowImageEditing
            };


            imagePicker.FinishedPickingMedia +=
                (sender, e) => { imagePicker.DismissViewController(true, () => callback?.Invoke(e.OriginalImage)); };
            imagePicker.SourceType = source;
            imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(source);
            imagePicker.Canceled += (sender, e) => imagePicker.DismissViewController(true, null);
            parent.PresentViewController(imagePicker, true, null);
        }

        private static void ShowImageActions(UIViewController parent, Action<ImagePicker> callback,
            IList<ImagePicker> actions = null)
        {
            var actionSheetAlert = UIAlertController.Create(Title, "", UIAlertControllerStyle.ActionSheet);
            if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
                actionSheetAlert.AddAction(UIAlertAction.Create(SCamera, UIAlertActionStyle.Default, obj =>
                    callback(new ImagePicker
                    {
                        SourceType = CImagePicker.Camera
                    })));
            actionSheetAlert.AddAction(UIAlertAction.Create(PhotoLibrary, UIAlertActionStyle.Default, obj =>
                callback(new ImagePicker
                {
                    SourceType = CImagePicker.PhotoLibrary
                })));

            if (actions != null)
                foreach (var action in actions)
                    actionSheetAlert.AddAction(UIAlertAction.Create(action.Name, UIAlertActionStyle.Default, obj =>
                        callback(action)));

            actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
            parent.PresentViewController(actionSheetAlert, true, null);
        }

        public class ImagePicker
        {
            public string Name { get; set; }
            public CImagePicker SourceType { get; set; }
            public Action<Action<UIImage>> Action { get; set; }
        }
    }
}