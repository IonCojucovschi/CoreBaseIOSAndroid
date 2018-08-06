//
// MultiDexApplication.cs
//
// Author:
//       Songurov Fiodor <songurov@gmail.com>
//
// Copyright (c) 2017 Songurov Fiodor
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
using Android.Runtime;
using Java.Interop;

namespace Int.Droid
{
    [Register("android/support/multidex/MultiDexApplication", DoNotGenerateAcw = true)]
    public class MultiDexApplication : Application
    {
        internal static readonly JniPeerMembers Members =
            new XAPeerMembers("android/support/multidex/MultiDexApplication", typeof(MultiDexApplication));

        internal static IntPtr JavaClassHandle;

        private static IntPtr _idCtor;

        [Register(".ctor", "()V", "", DoNotGenerateAcw = true)]
        public MultiDexApplication()
            : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            if (Handle != IntPtr.Zero)
                return;

            if (GetType() != typeof(MultiDexApplication))
            {
                SetHandle(
                    JNIEnv.StartCreateInstance(GetType(), "()V"),
                    JniHandleOwnership.TransferLocalRef);
                JNIEnv.FinishCreateInstance(Handle, "()V");
                return;
            }

            if (_idCtor == IntPtr.Zero)
                _idCtor = JNIEnv.GetMethodID(ClassRef, "<init>", "()V");
            SetHandle(
                JNIEnv.StartCreateInstance(ClassRef, _idCtor),
                JniHandleOwnership.TransferLocalRef);
            JNIEnv.FinishCreateInstance(Handle, ClassRef, _idCtor);
        }

        protected MultiDexApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        internal static IntPtr ClassRef =>
            JNIEnv.FindClass("android/support/multidex/MultiDexApplication", ref JavaClassHandle);

        protected override IntPtr ThresholdClass => ClassRef;

        protected override Type ThresholdType => typeof(MultiDexApplication);
    }
}