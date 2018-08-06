//
// BaseAdapter.cs
//
// Author:
//       Valentin Grigorean <v.grigorean@software-dep.net>
//
// Copyright (c) 2016 
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
using UIKit;

namespace Int.iOS.Controllers.DropdownView
{
    public abstract class BaseAdapter<T> : IAdapter
    {
        private readonly IList<T> _originalList;

        protected BaseAdapter() : this(new List<T>())
        {
        }

        protected BaseAdapter(IList<T> dataSource)
        {
            DataSource = _originalList = dataSource;
        }

        protected IList<T> DataSource { get; private set; }

        public int Count => DataSource.Count;
        public event EventHandler NotifyDataSetChanged;

        public virtual object GetItem(int position)
        {
            return DataSource[position];
        }

        public abstract UITableViewCell GetView(int position, UITableView tableView);

        public void FilterBy(Func<T, bool> filter, bool autoReset = true)
        {
            var flag = DataSource.Count == _originalList.Count;
            if (autoReset)
                DataSource = _originalList;
            var temp = DataSource.Where(filter).ToList();
            if (temp.Count == _originalList.Count && flag)
                return;
            DataSource = temp;
            NotifyDataChange();
        }

        public void Add(T item)
        {
            DataSource.Add(item);
            NotifyDataChange();
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                DataSource.Add(item);
            NotifyDataChange();
        }

        public void Remove(T item)
        {
            DataSource.Remove(item);
            NotifyDataChange();
        }

        public void Clear()
        {
            DataSource.Clear();
            NotifyDataChange();
        }

        protected void NotifyDataChange()
        {
            InvokeOnMainThread(() => NotifyDataSetChanged?.Invoke(this, EventArgs.Empty));
        }

        private static void InvokeOnMainThread(Action action)
        {
            AppTools.InvokeOnMainThread(action);
        }
    }
}