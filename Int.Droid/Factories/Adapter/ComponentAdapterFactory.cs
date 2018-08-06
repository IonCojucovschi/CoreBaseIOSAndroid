//
// ComponentAdapterFactory.cs
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
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;

namespace Int.Droid.Factories.Adapter
{
    [Serializable]
    public sealed class ItemEventArgs<T> : EventArgs
    {
        public ItemEventArgs(T model)
        {
            Model = model;
        }

        public ItemEventArgs(T model, object tag) : this(model)
        {
            Tag = tag;
        }

        public ItemEventArgs(T model, object tag, int position) : this(model, tag)
        {
            Position = position;
        }

        public object Tag { get; }
        public T Model { get; }
        public int Position { get; }
    }

    public abstract class ComponentAdapterFactory<T> : BaseAdapter
    {
        private IList<T> _currentList;
        private int? _footerId;
        private int? _headerId;
        private IList<T> _originalList;


        protected ComponentAdapterFactory(IList<T> dataSource, Context context = default(Context))
        {
            if (context == null)
                context = AppTools.CurrentActivity;

            if (dataSource == null)
                throw new ArgumentNullException(nameof(dataSource), "dataSource == null");

            if (context != null)
                Context = context;
            else
                throw new ArgumentNullException(nameof(context), "context == null");

            LayoutInflater = LayoutInflater.FromContext(context);
            _originalList = dataSource;
            _currentList = new List<T>(_originalList);
        }

        protected ComponentAdapterFactory(Context context, IList<T> dataSource) : this(dataSource, context)
        {
        }

        protected Context Context { get; }

        public override int Count
        {
            get
            {
                var count = _currentList?.Count ?? 0;
                if (HaveHeader())
                    count++;
                if (HaveFooter())
                    count++;
                return count;
            }
        }

        protected LayoutInflater LayoutInflater { get; }

        protected virtual IList<T> DataSource => _currentList;

        public View Header { get; private set; }

        public View Footer { get; private set; }

        public int Position { get; set; }

        public virtual event EventHandler<ItemEventArgs<T>> ItemClick;
        public virtual event EventHandler DataSetChanged;

        public void SetHeader(View view)
        {
            Header = view;
            if (view == null)
                _headerId = null;
        }

        public void SetHeader(int resource)
        {
            _headerId = resource;
            if (Header != null || resource == 0)
                Header = null;
        }

        public void SetFooter(View view)
        {
            Footer = view;
            if (view == null)
                _footerId = null;
        }

        public void SetFooter(int resource)
        {
            _footerId = resource;
            if (_footerId != null || resource == 0)
                _footerId = null;
        }

        public void Add(T item)
        {
            DataSource.Add(item);
            NotifyDataSetThreadSafe();
        }

        public void Remove(T item)
        {
            DataSource.Remove(item);
            NotifyDataSetThreadSafe();
        }

        public void UpdateDataSource(IEnumerable<T> newDataSource)
        {
            _originalList = _currentList = newDataSource.ToList();
            NotifyDataSetThreadSafe();
        }

        public void FilterBy(Func<T, bool> predicate, bool autoReset = true)
        {
            var flag = _currentList.Count == _originalList.Count;
            if (autoReset)
                _currentList = _originalList;
            var temp = _currentList.Where(predicate).ToList();
            if (temp.Count == _originalList.Count && flag)
                return;
            _currentList = temp;
            NotifyDataSetThreadSafe();
        }

        public void ClearFilter()
        {
            if (_originalList == null)
                return;
            if (_currentList.Count == _originalList.Count)
                return;
            _currentList = _originalList;
            NotifyDataSetThreadSafe();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Object GetItem(int position)
        {
            return position;
        }

        protected virtual void EmitItemClick(T model)
        {
            ItemClick?.Invoke(this, new ItemEventArgs<T>(model));
        }

        protected virtual void EmitItemClick(object sender, T model)
        {
            ItemClick?.Invoke(sender, new ItemEventArgs<T>(model, sender));
        }

        protected virtual void EmitItemClick(object sender, T model, int position)
        {
            ItemClick?.Invoke(sender, new ItemEventArgs<T>(model, sender, position));
        }

        protected abstract View GetItemView(T model, View convertView, ViewGroup parent);

        private bool HaveHeader()
        {
            return Header != null || _headerId != null && _headerId.Value > 0;
        }

        private bool HaveFooter()
        {
            return Footer != null || _footerId != null && _footerId.Value > 0;
        }

        public sealed override View GetView(int position, View convertView, ViewGroup parent)
        {
            Position = position;

            if (Position == 0 && HaveHeader())
            {
                if (Header == null && _headerId != null)
                    Header = LayoutInflater.Inflate(_headerId.Value, parent, false);
                return Header;
            }
            if (Position == Count - 1 && HaveFooter())
            {
                if (Footer == null && _footerId != null)
                    Footer = LayoutInflater.Inflate(_footerId.Value, parent, false);
                return Footer;
            }
            convertView = CheckConvertView(convertView);

            var model = _currentList[HaveHeader() ? Position - 1 : Position];

            return GetItemView(model, convertView, parent);
        }

        private View CheckConvertView(View convertView)
        {
            if (HaveHeader() && Header == convertView)
                return null;
            if (HaveFooter() && Footer == convertView)
                return null;
            return convertView;
        }

        private void NotifyDataSetThreadSafe()
        {
            var handler = new Handler(Context.MainLooper);
            handler.Post(() =>
            {
                NotifyDataSetChanged();
                DataSetChanged?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}