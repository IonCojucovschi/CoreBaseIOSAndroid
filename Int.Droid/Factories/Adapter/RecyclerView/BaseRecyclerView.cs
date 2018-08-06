//
//  BaseRecyclerView.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2017 Songurov
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;

namespace Int.Droid.Factories.Adapter.RecyclerView
{
    public class BaseRecyclerView : Android.Support.V7.Widget.RecyclerView
    {
        private readonly CustomAdapterDataObserver DataObserver;

        private View _emptyView;

        public BaseRecyclerView(Context context) :
            base(context)
        {
            DataObserver = new CustomAdapterDataObserver(UpdateEmptyView);
        }

        public BaseRecyclerView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            DataObserver = new CustomAdapterDataObserver(UpdateEmptyView);
        }

        public BaseRecyclerView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            DataObserver = new CustomAdapterDataObserver(UpdateEmptyView);
        }

        /// <summary>
        ///     Gets or sets the empty view. When the backing adapter has no
        ///     data this view will be made visible and the recycler view hidden.
        /// </summary>
        /// <value>The empty view.</value>
        public View EmptyView
        {
            get => _emptyView;
            set
            {
                _emptyView = value;
                UpdateEmptyView();
            }
        }

        public bool IsNullOrEmpty => GetAdapter() == null || GetAdapter().ItemCount == 0;

        public override void SetAdapter(Adapter adapter)
        {
            SetLayoutManager(new LinearLayoutManager(AppTools.CurrentActivity));
            var _adapter = GetAdapter();
            if (_adapter != null)
                adapter.UnregisterAdapterDataObserver(DataObserver);
            if (adapter != null)
                adapter.RegisterAdapterDataObserver(DataObserver);
            base.SetAdapter(adapter);
            UpdateEmptyView();
        }

        private void UpdateEmptyView()
        {
            if (_emptyView == null || GetAdapter() == null) return;
            var showEmptyView = GetAdapter().ItemCount == 0;
            _emptyView.Visibility = showEmptyView ? ViewStates.Visible : ViewStates.Gone;
            Visibility = showEmptyView ? ViewStates.Gone : ViewStates.Visible;
        }

        private class CustomAdapterDataObserver : AdapterDataObserver
        {
            private readonly Action Callback;

            public CustomAdapterDataObserver(Action callback)
            {
                Callback = callback;
            }

            public override void OnChanged()
            {
                base.OnChanged();
                Callback();
            }

            public override void OnItemRangeInserted(int positionStart, int itemCount)
            {
                base.OnItemRangeInserted(positionStart, itemCount);
                Callback();
            }

            public override void OnItemRangeRemoved(int positionStart, int itemCount)
            {
                base.OnItemRangeRemoved(positionStart, itemCount);
                Callback();
            }
        }
    }
}