//
// BaseAdapter.cs
//
// Author:
//       Songurov <songurov@gmail.com>
//
// Copyright (c) 2017 Songurov
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
using Android.Views;
using Int.Core.Extensions;
using Int.Core.Wrappers;

namespace Int.Droid.Factories.Adapter.RecyclerView
{
    public abstract class BaseAdapter<T, TVh> : Android.Support.V7.Widget.RecyclerView.Adapter
        where TVh : Android.Support.V7.Widget.RecyclerView.ViewHolder
    {
        private const int MainViewHolderType = 0;
        private const int HeaderViewHolderType = 1;
        private const int FooterViewHolderType = 2;
        private Func<ViewGroup, View> _footerCreator;

        private Func<ViewGroup, View> _headerCreator;

        private IList<T> _originalList;

        protected BaseAdapter() : this(new List<T>()) { }

        protected BaseAdapter(IList<T> dataSource)
        {
            _originalList = dataSource;
            DataSource = new List<T>(dataSource);
        }

        protected IList<T> DataSource { get; private set; }

        public T this[int index] => DataSource[index];

        public override int ItemCount
        {
            get
            {
                var count = DataSource?.Count ?? 0;
                if (HasHeader)
                    count++;
                if (HasFooter)
                    count++;
                return count;
            }
        }

        private bool HasHeader => _headerCreator != null;
        private bool HasFooter => _footerCreator != null;

        public IClickableView Delegate { get; set; } = new DefaultEffect();

        /// <summary>
        ///     Sets the header.
        /// </summary>
        /// <param name="viewCreator">View creator. Should return View inflated to ViewGroup in parameters. (but not attached)</param>
        public void SetHeader(Func<ViewGroup, View> viewCreator)
        {
            _headerCreator = viewCreator;
        }

        /// <summary>
        ///     Sets the footer.
        /// </summary>
        /// <param name="viewCreator">View creator. Should return View inflated to ViewGroup in parameters. (but not attached)</param>
        public void SetFooter(Func<ViewGroup, View> viewCreator)
        {
            _footerCreator = viewCreator;
        }

        public void Add(T item)
        {
            DataSource.Add(item);
            NotifyItemInserted(DataSource.Count - 1);
        }

        public void Add(T item, int position)
        {
            DataSource.Insert(position, item);
            NotifyItemInserted(position);
        }

        public void AddRange(IEnumerable<T> items)
        {
            var count = DataSource.Count;
            DataSource.AddRange(items);
            NotifyItemRangeInserted(count == 0 ? 0 : count - 1, DataSource.Count - count);
        }

        public void ClearAndReplace(IEnumerable<T> items)
        {
            if (DataSource.Equals(items)) return;
            DataSource.Clear();
            var enumerable = items as T[] ?? items.ToArray();
            DataSource.AddRange(enumerable);

            DataSource = new List<T>(enumerable);
            _originalList = DataSource;

            NotifyDataSetChanged();
        }

        public void Remove(T item)
        {
            var index = DataSource.IndexOf(item);
            if (index < 0) return;
            DataSource.RemoveAt(index);
            NotifyItemRemoved(index);
        }

        public void Reload(T item)
        {
            var index = DataSource.IndexOf(item);
            if (index < 0) return;
            NotifyItemChanged(index);
        }

        public void Reload(IList<T> items)
        {
            var list = items.Select(item => DataSource.IndexOf(item)).Where(index => index >= 0).ToList();
            if (list.Count == 0) return;

            foreach (var item in list)
                NotifyItemChanged(item);
        }

        public void ClearFilter()
        {
            if (_originalList == null)
                return;
            if (DataSource.Count == _originalList.Count)
                return;
            DataSource = _originalList;
            NotifyDataSetChanged();
        }

        public void FilterBy(Func<T, bool> predicate, bool autoReset = true)
        {
            var flag = DataSource.Count == _originalList.Count;
            if (autoReset)
                DataSource = _originalList;
            var temp = DataSource.Where(predicate).ToList();
            if (temp.Count == _originalList.Count && flag)
                return;
            DataSource = temp;
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            DataSource.Clear();
            NotifyDataSetChanged();
        }

        public override int GetItemViewType(int position)
        {
            if (HasHeader && position == 0)
                return HeaderViewHolderType;

            if (HasFooter && position == ItemCount - 1)
                return FooterViewHolderType;

            return MainViewHolderType;
        }

        public override void OnBindViewHolder(Android.Support.V7.Widget.RecyclerView.ViewHolder holder, int position)
        {
            if (GetItemViewType(position) != MainViewHolderType) return;

            SetHolder(holder as TVh, position - (HasHeader ? 1 : 0));
        }

        public event EventHandler<ItemClickEventArgs> ItemClick;

        public override Android.Support.V7.Widget.RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent,
            int viewType)
        {
            Android.Support.V7.Widget.RecyclerView.ViewHolder vh = null;

            switch (viewType)
            {
                case MainViewHolderType:
                    vh = CreateHolder(parent, viewType);
                    break;
                case HeaderViewHolderType:
                    var headerView = _headerCreator?.Invoke(parent);
                    if (headerView == null) return null;
                    vh = new SimpleConcreteViewHolder(headerView);
                    break;
                case FooterViewHolderType:
                    var footerView = _footerCreator?.Invoke(parent);
                    if (footerView == null) return null;
                    vh = new SimpleConcreteViewHolder(footerView);
                    break;
            }

            if (viewType == MainViewHolderType)
                vh.ItemView.Touch += (sender, e) =>
                {
                    var view = sender as View;
                    var actionMask = e.Event.ActionMasked;

                    Action clickAction = () => ItemClick?.Invoke(
                        this, new ItemClickEventArgs(vh.AdapterPosition, DataSource[vh.AdapterPosition], vh as TVh));

                    switch (actionMask)
                    {
                        case MotionEventActions.Down:
                            Delegate.OnTouch(view, State.Began, clickAction);
                            break;
                        case MotionEventActions.Up:
                            Delegate.OnTouch(view, State.Ended, clickAction);
                            break;
                        case MotionEventActions.Cancel:
                            Delegate.OnTouch(view, State.Cancel, clickAction);
                            break;
                    }
                };

            return vh;
        }

        public abstract void SetHolder(TVh holder, int position);

        public abstract TVh CreateHolder(ViewGroup parent, int viewType);

        public class ItemClickEventArgs : EventArgs
        {
            public ItemClickEventArgs(int pos, T item, TVh viewHolder)
            {
                Position = pos;
                Item = item;
                ViewHolder = viewHolder;
            }

            public TVh ViewHolder { get; }
            public T Item { get; }
            public int Position { get; }
        }

        private class SimpleConcreteViewHolder :
            Android.Support.V7.Widget.RecyclerView.ViewHolder
        {
            public SimpleConcreteViewHolder(View view) : base(view)
            {
            }
        }
    }

    public class DefaultEffect : IClickableView
    {
        public bool Enabled { get; set; }

        public event EventHandler Click;

        public void SendActionForControlEvent()
        {
            throw new NotImplementedException();
        }

        public void OnTouch(object view, State state, Action clickAction)
        {
            var unused = view as View;

            OnClick(new EventArgs());

            switch (state)
            {
                case State.Ended:
                    clickAction?.Invoke();
                    break;
            }
        }

        protected virtual void OnClick(EventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}