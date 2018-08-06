//
// Media.cs
//
// Author:
//       Songurov Fiodor <songurov@gmail.com>
//
// Copyright (c) 2016 Songurov Fiodor
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

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Int.Core.Network;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace Int.Core.Device
{
    public class Media : ApiBase<Media>
    {
        public async Task<Stream> TakePhotoResize(int resizeProcente = 80)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Debug.WriteLine("Not Acces Camera.");
                return default(Stream);
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Custom,
                CompressionQuality = resizeProcente,
                CustomPhotoSize = resizeProcente
            });

            if (file == null)
                return default(Stream);

            var stream = file.GetStream();
            file.Dispose();
            return stream;
        }

        public async Task<Stream> TakePhotoResizeSave(string directory, string name, int resizeProcente = 80)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Debug.WriteLine("Not Acces Camera.");
                return default(Stream);
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Custom,
                CompressionQuality = resizeProcente,
                CustomPhotoSize = resizeProcente,
                Directory = directory,
                Name = name
            });

            if (file == null)
                return default(Stream);

            var stream = file.GetStream();
            file.Dispose();
            return stream;
        }

        public async Task<Stream> TakeVideoResize(int resizeProcente = 80)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                Debug.WriteLine("Not Acces Camera.");
                return default(Stream);
            }

            var file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
            {
                PhotoSize = PhotoSize.Custom,
                CompressionQuality = resizeProcente,
                CustomPhotoSize = resizeProcente
            });

            if (file == null)
                return default(Stream);

            var stream = file.GetStream();
            file.Dispose();
            return stream;
        }
    }
}