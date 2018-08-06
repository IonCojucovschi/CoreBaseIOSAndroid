//
// ComponentViewHolderAdapterFactory.cs
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
using Android.Content;
using Android.Views;
using Object = Java.Lang.Object;

namespace Int.Droid.Factories.Adapter
{
    public abstract class ComponentViewHolderAdapterFactory<T, TVh> : ComponentAdapterFactory<T>
        where TVh : ViewHolder
    {
        protected ComponentViewHolderAdapterFactory(IList<T> dataSource, int resource = 0, View view = null)
            : base(dataSource)
        {
            Resources = resource;
            ViewItem = view;
        }

        protected ComponentViewHolderAdapterFactory(Context context, IList<T> dataSource, int resource = 0) : base(
            context, dataSource)
        {
            Resources = resource;
        }

        public int Resources { get; set; }

        public View ViewItem { get; set; }

        public abstract TVh CreateHolder(View parent);
        public abstract void SetHolder(TVh viewHolder, T item);

        public virtual void CreateResource(int position)
        {
        }

        protected override View GetItemView(T model, View convertView, ViewGroup parent)
        {
            TVh viewHolder;

            CreateResource(Position);

            if (convertView == null)
            {
                if (Resources > 0 || Resources == -1)
                {
                    if (Resources != -1)
                        ViewItem = LayoutInflater.Inflate(Resources, parent, false);
                    else if (model != null)
                        ViewItem = model as View;
                    viewHolder = CreateHolder(ViewItem);
                }
                else
                {
                    viewHolder = CreateHolder(parent);
                }

                if (viewHolder.View == null)
                    throw new NullReferenceException("ViewHolder must return a view!");

                viewHolder.View.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as TVh;
            }

            SetHolder(viewHolder, model);

            return viewHolder?.View;
        }
    }

    public abstract class ViewHolder : Object
    {
        protected ViewHolder(View view)
        {
            View = view;
        }

        public View View { get; }
    }
}