//
// iOSObject.cs
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Foundation;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        public static NSObject ToNsObject(this object o)
        {
            return NSObject.FromObject(o);
        }

        public static void WriteToFilePcl(this object obj, string path)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));

            var bytes = Core.Extensions.Extensions.Serialize(obj);

            using (var streamFile = new FileStream(path,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.Write))
            {
                streamFile.Write(bytes, 0, bytes.Length);
            }
        }

        public static void WriteToFile(this object obj, string path)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));

            var bytes = SerializeBinary(obj);

            using (var streamFile = new FileStream(path,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.Write))
            {
                streamFile.Write(bytes, 0, bytes.Length);
            }
        }

        public static T ReadFromFilePcl<T>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));

            if (!File.Exists(path)) return default(T);
            var buffer = File.ReadAllBytes(path);
            if (buffer.Length == 0)
                return default(T);
            var obj = Core.Extensions.Extensions.Deserialize<T>(buffer);
            return obj;
        }

        public static T ReadFromFile<T>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));

            if (!File.Exists(path)) return default(T);
            var buffer = File.ReadAllBytes(path);
            if (buffer.Length == 0)
                return default(T);
            var obj = DeserializeBinary<T>(buffer);
            return obj;
        }

        public static byte[] SerializeBinary<T>(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            byte[] arrayData;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                arrayData = ms.ToArray();
            }
            return arrayData;
        }

        public static T DeserializeBinary<T>(byte[] arrayData)
        {
            if (arrayData == null) throw new ArgumentNullException(nameof(arrayData));

            T obj;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                ms.Write(arrayData, 0, arrayData.Length);
                ms.Seek(0, SeekOrigin.Begin);
                obj = (T) formatter.Deserialize(ms);
            }
            return obj;
        }

        public static IDisposable Subscribe<T>(this IObservable<T> @this, Action action)
        {
            return @this.Subscribe(e => action?.Invoke());
        }
    }
}