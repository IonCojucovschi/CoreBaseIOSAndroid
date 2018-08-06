//
// InternalDataSource.cs
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
using System.Linq;
using Foundation;
using Int.iOS.Factories.Adapter.Contract;

namespace Int.iOS.Factories.Adapter.Abstraction
{
    public class InternalDataSource<T> : NSObject, IDataSource<T>
    {
        private IList<T> _originalList;

        public InternalDataSource(IList<T> dataSource)
        {
            DataSource = _originalList = dataSource;
        }

        public IList<T> DataSource { get; set; }

        public float HeightCell { get; set; }

        public event EventHandler DataSetChanged;

        public int Count => DataSource.Count;

        public void Add(T item)
        {
            DataSource.Add(item);
            ReloadData();
        }

        public void AddRange(IEnumerable<T> col)
        {
            if (col == null)
                throw new ArgumentNullException(nameof(col));
            var haveElements = false;
            foreach (var item in col)
            {
                haveElements = true;
                DataSource.Add(item);
            }
            if (haveElements)
                ReloadData();
        }

        public void Remove(T item)
        {
            DataSource.Remove(item);
            ReloadData();
        }

        public int IndexOf(T item)
        {
            return DataSource.IndexOf(item);
        }

        public T GetItem(int pos)
        {
            return DataSource[pos];
        }


        /// <summary>
        ///     Filters the by.
        ///     Will not work if you dont initialiaze with tableView
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <param name="autoReset">If set to <c>true</c> auto reset.</param>
        public void FilterBy(Func<T, bool> predicate, bool autoReset = true)
        {
            var flag = DataSource.Count == _originalList.Count;
            if (autoReset)
                DataSource = _originalList;
            var temp = DataSource.Where(predicate).ToList();
            if (temp.Count == _originalList.Count && flag)
                return;
            DataSource = temp;
            ReloadData();
        }

        public void ClearFilter()
        {
            if (_originalList == null)
                return;
            if (DataSource.Count == _originalList.Count)
                return;
            DataSource = _originalList;
            ReloadData();
        }


        public virtual void UpdateDataSource(IList<T> newDataSource)
        {
            _originalList = DataSource = newDataSource;
            ReloadData();
        }

        public void ReloadData()
        {
            AppTools.InvokeOnMainThread(() => DataSetChanged?.Invoke(this, EventArgs.Empty));
        }
    }
}