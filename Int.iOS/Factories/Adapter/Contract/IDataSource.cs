//
// IDataSource.cs
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
using UIKit;

namespace Int.iOS.Factories.Adapter.Contract
{
    public interface IDataSource<T> : IUIScrollViewDelegate
    {
        int Count { get; }

        /// <summary>
        ///     Occurs when data set changed.
        /// </summary>
        event EventHandler DataSetChanged;

        /// <summary>
        ///     Add the specified item.
        ///     Will reloadData
        /// </summary>
        /// <param name="item">Item.</param>
        void Add(T item);

        /// <summary>
        ///     Adds the range.
        ///     Will reloadData
        /// </summary>
        /// <param name="col">Col.</param>
        void AddRange(IEnumerable<T> col);

        /// <summary>
        ///     Remove the specified item.
        ///     Will reloadData
        /// </summary>
        /// <param name="item">Item.</param>
        void Remove(T item);

        /// <summary>
        ///     Updates the data source.
        ///     Will reloadData
        /// </summary>
        /// <param name="newDataSource">New data source.</param>
        void UpdateDataSource(IList<T> newDataSource);

        T GetItem(int pos);

        int IndexOf(T item);

        /// <summary>
        ///     Filters the by.
        ///     Will reloadData
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <param name="autoReset">If set to <c>true</c> auto reset.</param>
        void FilterBy(Func<T, bool> predicate, bool autoReset = true);

        /// <summary>
        ///     Clears the filter.
        ///     Will reloadData
        /// </summary>
        void ClearFilter();

        void ReloadData();
    }
}