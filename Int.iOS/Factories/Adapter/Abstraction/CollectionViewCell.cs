//
// CollectionViewCell.cs
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
using System.Collections.Generic;
using Int.Core.Application.Widget.Contract.Table.Adapter;
using Int.iOS.Factories.Adapter.Contract;
using Int.iOS.Wrappers.Widget.CrossViewInjection;
using UIKit;

namespace Int.iOS.Factories.Adapter.Abstraction
{
    public abstract class CollectionViewCell<T> : UICollectionViewCell, IReusableView<T>, ICrossCell
    {
        private T _model;
        private bool _wasInit;

        protected CollectionViewCell(IntPtr handle) : base(handle)
        {
        }

        public IList<T> SourceData { get; set; }

        public event EventHandler<ReusableViewNotifyEventHandler<T>> Notify;

        public virtual T Model
        {
            get => _model;
            set
            {
                BeforeBind();
                _model = value;
                if (!_wasInit)
                {
                    OnCreate();
                    _wasInit = true;
                }
                OnBind(Position);
                (CrossCellModel as ICrossCellViewHolder<T>)?.Bind(_model);
            }
        }

        public int Position { get; set; }
        public UINavigationController Navigate { get; set; }
        public UIStoryboard Storyboard { get; set; }
        public int Count { get; set; }

        public ICrossCellViewHolder CrossCellModel { get; set; }
        public UITableView TableView { get; set; }

        public virtual void OnCreate()
        {
            (CrossCellModel as ICrossCellViewHolder<T>)?.OnCreate();
        }

        public virtual void BeforeBind()
        {
            var unset = new CrossViewInjector(this);
        }

        public abstract void OnBind(int position);

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            if (_wasInit) return;
            OnCreate();
            _wasInit = true;
        }

        protected void InvokeNotify()
        {
            InvokeNotify(null);
        }

        protected void InvokeNotify(object tag)
        {
            InvokeOnMainThread(() => Notify?.Invoke(
                this, new ReusableViewNotifyEventHandler<T>(Model, tag)));
        }
    }
}