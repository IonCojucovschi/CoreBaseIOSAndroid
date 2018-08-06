//
// PlatformPerformance.cs
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

using Android.App;
using Java.Lang;
using Process = Android.OS.Process;
using Thread = System.Threading.Thread;

namespace Int.Droid.Helpers
{
    public class Performance
    {
        private static volatile Performance _instance;
        private static readonly object SyncRoot = new object();
        private readonly ActivityManager _activityManager;
        private readonly ActivityManager.MemoryInfo _memoryInfo;
        private readonly Runtime _runtime;

        private Performance()
        {
            _runtime = Runtime.GetRuntime();
            _activityManager = (ActivityManager) Application.Context.GetSystemService("activity");
            _memoryInfo = new ActivityManager.MemoryInfo();
        }

        public static Performance Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new Performance();
                }

                return _instance;
            }
        }

        public int GetCurrentManagedThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        public int GetCurrentSystemThreadId()
        {
            return Process.MyTid();
        }

        public string GetMemoryInfo()
        {
            _activityManager.GetMemoryInfo(_memoryInfo);
            var availableMegs = _memoryInfo.AvailMem / 1048576d;
            var totalMegs = _memoryInfo.TotalMem / 1048576d;
            var percentAvail = (double) _memoryInfo.AvailMem / _memoryInfo.TotalMem * 100d;

            var availableMegsHeap = (_runtime.TotalMemory() - _runtime.FreeMemory()) / 1048576d;
            var totalMegsHeap = _runtime.MaxMemory() / 1048576d;
            var percentAvailHeap =
                (double) (_runtime.TotalMemory() - _runtime.FreeMemory()) / _runtime.MaxMemory() * 100d;

            return
                $"[PERFORMANCE] Memory - Free: {availableMegs:0}MB ({percentAvail:0}%), Total: {totalMegs:0}MB, Heap - Free: {availableMegsHeap:0}MB ({percentAvailHeap:0}%), Total: {totalMegsHeap:0}MB";
        }
    }
}