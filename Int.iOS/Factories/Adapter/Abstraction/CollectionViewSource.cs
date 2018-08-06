//
// CollectionViewSource.cs
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
using CoreGraphics;
using Int.Core.Application.Widget.Contract.Table;
using Int.iOS.Factories.Adapter.Contract;
using UIKit;

namespace Int.iOS.Factories.Adapter.Abstraction
{
    public abstract class CollectionViewSource<T> : UICollectionViewSource, IDataSource<T>
    {


        private readonly IDataSource<T> _dataSource;

        protected CollectionViewSource(IList<T> dataSource)
        {
            _dataSource = new InternalDataSource<T>(dataSource);
            _dataSource.DataSetChanged += (sender, e) => ReloadData();
        }

        protected CollectionViewSource(IList<T> dataSource, UICollectionView collectionView) : this(dataSource)
        {
            CollectionView = collectionView;
        }

        protected UICollectionView CollectionView { get; }

        public event RowClickedEventHandler<T> RowClicked;

        public int Count => _dataSource.Count;

        public event EventHandler DataSetChanged;

        public void Add(T item)
        {
            _dataSource.Add(item);
        }

        public void AddRange(IEnumerable<T> col)
        {
            _dataSource.AddRange(col);
        }

        public void ClearFilter()
        {
            _dataSource.ClearFilter();
        }

        public void FilterBy(Func<T, bool> predicate, bool autoReset = true)
        {
            _dataSource.FilterBy(predicate, autoReset);
        }

        public T GetItem(int pos)
        {
            return _dataSource.GetItem(pos);
        }

        public int IndexOf(T item)
        {
            return _dataSource.IndexOf(item);
        }

        public void ReloadData()
        {
            DataSetChanged?.Invoke(this, EventArgs.Empty);
            CheckIfHaveTable();
            CollectionView.ReloadData();
        }

        public void Remove(T item)
        {
            _dataSource.Remove(item);
        }

        public void UpdateDataSource(IList<T> newDataSource)
        {
            _dataSource.UpdateDataSource(newDataSource);
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return Count;
        }

        private void CheckIfHaveTable()
        {
            if (CollectionView == null)
                throw new Exception("You need to call constructor with " +
                                    "const(list,collectionView)");
        }
    }
}