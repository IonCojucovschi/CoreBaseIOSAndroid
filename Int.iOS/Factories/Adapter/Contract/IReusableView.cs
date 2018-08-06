//
// IReusableView.cs
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
using Int.Core.Application.Widget.Contract.Table.Adapter;
using UIKit;

namespace Int.iOS.Factories.Adapter.Contract
{
    public class ReusableViewNotifyEventHandler<T> : EventArgs
    {
        public ReusableViewNotifyEventHandler() : this(default(T), null)
        {
        }

        public ReusableViewNotifyEventHandler(T model) : this(model, null)
        {
        }

        public ReusableViewNotifyEventHandler(object tag) : this(default(T), tag)
        {
        }

        public ReusableViewNotifyEventHandler(T model, object tag)
        {
            Model = model;
            Tag = tag;
        }

        public object Tag { get; }
        public T Model { get; }
    }

    public interface IReusableView<T>
    {
        UINavigationController Navigate { get; set; }
        UIStoryboard Storyboard { get; set; }

        T Model { get; set; }
        int Position { get; set; }
        int Count { get; set; }

        UITableView TableView { get; set; }

        ICrossCellViewHolder CrossCellModel { get; set; }
        void BeforeBind();
        void OnBind(int position);
        void OnCreate();
        event EventHandler<ReusableViewNotifyEventHandler<T>> Notify;
    }
}