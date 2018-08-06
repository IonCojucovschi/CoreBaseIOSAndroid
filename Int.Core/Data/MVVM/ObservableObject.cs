//
// ObservableObject.cs
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
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace Int.Data.MVVM
{
    public class ObservableObject : ReactiveObject, IObservableObject, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;
        public virtual bool ThrowOnInvalidPropertyName { get; set; }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void VerifyPropertyName(string propertyName)
        {
            if (GetType().GetRuntimeProperty(propertyName) != null) return;
            var msg = "Invalid property name: " + propertyName;

            if (ThrowOnInvalidPropertyName)
                throw new Exception(msg);
        }

        protected void PropertyChangedExt([CallerMemberName] string propertyName = null)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            this.RaisePropertyChanged(propertyName);
        }

        protected void PropertyChangingExt([CallerMemberName] string propertyName = null)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            this.RaisePropertyChanging(propertyName);
        }
    }
}