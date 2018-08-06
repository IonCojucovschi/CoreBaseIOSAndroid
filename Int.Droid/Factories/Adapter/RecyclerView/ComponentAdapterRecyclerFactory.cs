//
// ComponentAdapterRecyclerFactory.cs
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

using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Int.Core.Application.Widget.Contract.Table;
using Int.Core.Application.Widget.Contract.Table.Adapter;
using Int.Data.MVVM;
using Int.Droid.Wrappers;
using Int.Droid.Wrappers.Widget.CrossViewInjection;

namespace Int.Droid.Factories.Adapter.RecyclerView
{
    public delegate ComponentViewHolder<T> CreateViewHolderDelegate<T>(LayoutInflater inflater, ViewGroup parent);

    public static class ComponentAdapterRecyclerFactory
    {
        public static BaseAdapter<T, ComponentViewHolder<T>>
            CreateAdapter<T, TVModel>(IList<T> items, CreateViewHolderDelegate<T> createFunc)
            where TVModel : BaseViewModel
        {
            return new InternalRvAdapter<T>(items, createFunc);
        }

        public static BaseAdapter<T, ComponentViewHolder<T>>
            CreateAdapter<T>(CreateViewHolderDelegate<T> createFunc)
        {
            return new InternalRvAdapter<T>(new List<T>(), createFunc);
        }

        private class InternalRvAdapter<T> : BaseAdapter<T, ComponentViewHolder<T>>, IAdapter<T>, IAdapterConfigurable
        {
            private readonly CreateViewHolderDelegate<T> _createFunc;

            private int? _rowHeight;
            public int? RowHeight
            {
                get
                {
                    return _rowHeight;
                }
                set
                {
                    _rowHeight = value;
                    NotifyDataSetChanged();
                }
            }

            public InternalRvAdapter(IList<T> items, CreateViewHolderDelegate<T> createFunc)
                : base(items)
            {
                _createFunc = createFunc;
            }

            public void UpdateDataSource(IEnumerable<T> data)
            {
                ClearAndReplace(data);
            }

            public event RowClickedEventHandler<T> RowClicked;

            public override ComponentViewHolder<T> CreateHolder(ViewGroup parent, int viewType)
            {
                return _createFunc(LayoutInflater.FromContext(parent.Context), parent);
            }

            public override void SetHolder(ComponentViewHolder<T> holder, int position)
            {
                holder.Bind(DataSource.ElementAtOrDefault(position));

                if (RowHeight != null)
                    holder.SetHeight(RowHeight.Value);

                ItemClick -= InternalRvAdapter_ItemClick;
                ItemClick += InternalRvAdapter_ItemClick;
            }

            private void InternalRvAdapter_ItemClick(object sender, ItemClickEventArgs e)
            {
                RowClicked?.Invoke(sender, new RowClickedEventArgs<T>(e.Item));
            }
        }
    }

    public abstract class ComponentViewHolder<T> :
        Android.Support.V7.Widget.RecyclerView.ViewHolder, ICrossCell
    {
        protected ComponentViewHolder(View view, ICrossCellViewHolder<T> vmodel) : base(view)
        {
            CrossCellModel = vmodel;
            Initialize();
        }

        public void SetHeight(int height)
        {
            Android.Widget.LinearLayout.LayoutParams newLayout;
            if (ItemView.LayoutParameters != null)
                newLayout = new Android.Widget.LinearLayout.LayoutParams(ItemView.LayoutParameters);
            else
                newLayout = new Android.Widget.LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);

            newLayout.Width = ItemView.LayoutParameters?.Width ?? ViewGroup.LayoutParams.MatchParent;
            newLayout.Height = height;

            ItemView.LayoutParameters = newLayout;
        }

        public ICrossCellViewHolder CrossCellModel { get; set; }

        public virtual void Bind(T model)
        {
            SetRefInVm();

            (CrossCellModel as ICrossCellViewHolder<T>)?.Bind(model);
        }

        public virtual void OnCreate()
        {
            (CrossCellModel as ICrossCellViewHolder<T>)?.OnCreate();
        }

        private void Initialize()
        {
            SetRefInVm();

            OnCreate();
        }

        private void SetRefInVm()
        {
            Cheeseknife.Inject(this, ItemView);
            var unset = new CrossViewInjector(this);
        }
    }

    public interface IAdapterConfigurable
    {
        /// <summary>
        /// Gets or sets the height of the row. Height remains same as in design if value == null.
        /// </summary>
        /// <value>The height of the row.</value>
        int? RowHeight { get; set; }
    }
}