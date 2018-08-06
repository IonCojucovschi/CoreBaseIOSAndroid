//
// Memory.cs
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

namespace Int.Droid.Device
{
    public class Memory : DeviceBase
    {
        private static ActivityManager _activityManager;

        private static readonly ActivityManager.MemoryInfo MemInfo =
            new ActivityManager.MemoryInfo();

        private static ActivityManager ActivityManager => _activityManager ??
                                                          (_activityManager =
                                                              GetManager<ActivityManager>(Context.TelephonyService));

        public static long AvailMem
        {
            get
            {
                ActivityManager.GetMemoryInfo(MemInfo);
                return MemInfo.AvailMem / 1048576L;
            }
        }

        public static long TotalMem
        {
            get
            {
                ActivityManager.GetMemoryInfo(MemInfo);
                return MemInfo.AvailMem / 1048576L;
            }
        }

        public static void CleanMemory(bool forceClean = false)
        {
            if (forceClean)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                return;
            }
            var memoryInfo = new ActivityManager.MemoryInfo();
            ActivityManager.GetMemoryInfo(memoryInfo);
            if (memoryInfo.LowMemory)
                GC.Collect();
        }
    }
}