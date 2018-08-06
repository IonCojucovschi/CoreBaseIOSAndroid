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
using Android.App;
using Android.Content;
using Android.Provider;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

namespace Int.Droid.Device
{
    public class Camera
    {
        public static Uri TakePhotoInDirectory(Activity context, string directory, int requestCode,
            Action<string> error)
        {
            var dt = DateTime.Now;
            var datadir = Environment.ExternalStorageDirectory.AbsolutePath + directory;

            var fileDir = new File(datadir);
            if (!fileDir.Exists())
                fileDir.Mkdir();

            var dateTimeStamp = dt.Year.ToString("D4") + "-" + dt.Month.ToString("D2") + "-" + dt.Day.ToString("D2") +
                                "-" + dt.Hour.ToString("D2") + dt.Minute.ToString("D2") + dt.Second.ToString("D2");
            var pathToFile = datadir + "/photo-" + dateTimeStamp + ".jpg";

            var file = new File(pathToFile);
            try
            {
                file.CreateNewFile();
            }
            catch (Exception e)
            {
                error?.Invoke(e.Message);
                return null;
            }

            var uri = Uri.FromFile(file);

            var intent = new Intent(MediaStore.ActionImageCapture);
            intent.PutExtra(MediaStore.ExtraOutput, uri);
            context.StartActivityForResult(intent, requestCode);

            return uri;
        }

        public static Uri TakeVideoInDirectory(Activity context, string directory, int requestCode,
            Action<string> error)
        {
            var dt = DateTime.Now;
            var datadir = Environment.ExternalStorageDirectory.AbsolutePath + directory;

            var fileDir = new File(datadir);
            if (!fileDir.Exists())
                fileDir.Mkdir();

            var dateTimeStamp = dt.Year.ToString("D4") + "-" + dt.Month.ToString("D2") + "-" + dt.Day.ToString("D2") +
                                "-" + dt.Hour.ToString("D2") + dt.Minute.ToString("D2") + dt.Second.ToString("D2");
            var pathToFile = datadir + "/video-" + dateTimeStamp + ".mp4";

            var file = new File(pathToFile);
            try
            {
                file.CreateNewFile();
            }
            catch (Exception e)
            {
                error?.Invoke(e.Message);
                return null;
            }

            var uri = Uri.FromFile(file);

            var intent = new Intent(MediaStore.ActionVideoCapture);
            intent.PutExtra(MediaStore.ExtraDurationLimit, 60);
            intent.PutExtra(MediaStore.ExtraOutput, uri);
            context.StartActivityForResult(intent, requestCode);

            return uri;
        }
    }
}