//
// MainThreadDispatcher.cs
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
using System.Threading.Tasks;
using Android.OS;

namespace Int.Droid.Helpers
{
    public class MainThreadDispatcher
    {
        private static readonly Handler Handler = new Handler(Looper.MainLooper);

        private static volatile MainThreadDispatcher _instance;
        private static readonly object SyncRoot = new object();

        private MainThreadDispatcher()
        {
        }

        public static MainThreadDispatcher Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new MainThreadDispatcher();
                }

                return _instance;
            }
        }

        public static void Post(Action action)
        {
            var currentLooper = Looper.MyLooper();

            if (currentLooper != null && currentLooper.Thread == Looper.MainLooper.Thread)
                action();
            else
                Handler.Post(action);
        }

        public static void PostAsyc(Action action, long delay)
        {
            var currentLooper = Looper.MyLooper();

            if (currentLooper != null && currentLooper.Thread == Looper.MainLooper.Thread)
                action();
            else
                Handler.PostDelayed(action, delay);
        }

        public Task PostAsync(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            Post(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(string.Empty);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }
    }
}