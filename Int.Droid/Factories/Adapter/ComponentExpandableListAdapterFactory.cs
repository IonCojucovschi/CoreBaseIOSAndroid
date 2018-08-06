//
// ComponentExpandableListAdapterFactory.cs
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
using Android.Widget;
using Object = Java.Lang.Object;

namespace Int.Droid.Factories.Adapter
{
    public abstract class ComponentExpandableListAdapterFactory<TGroups, TItems> : BaseExpandableListAdapter
    {
        protected ComponentExpandableListAdapterFactory(Context context)
        {
            ContextBaseAdapter = context;
        }

        protected ComponentExpandableListAdapterFactory(Context context, IList<TGroups> itemGroup,
            IList<TItems> itemChild)
            : this(context)
        {
            ItemChilders = itemChild;
            ItemGroups = itemGroup;
        }


        protected ComponentExpandableListAdapterFactory(Context context, IList<TGroups> itemGroup,
            Dictionary<TGroups, IList<TItems>> itemChild)
            : this(context)
        {
            ChildCollectionDictionary = itemChild;
            ItemGroups = itemGroup;
        }

        public IList<TItems> ItemChilders { get; }
        public IList<TGroups> ItemGroups { get; }

        public Dictionary<TGroups, IList<TItems>> ChildCollectionDictionary { get; }

        public Context ContextBaseAdapter { get; }

        #region implemented abstract members of BaseExpandableListAdapter

        public override Object GetChild(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return ItemChilders.Count;
        }

        public override Object GetGroup(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }


        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView,
            ViewGroup parent)
        {
            return null;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            return null;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public override int GroupCount => ItemGroups.Count;
        public override bool HasStableIds => true;
        public override int GroupTypeCount => ItemGroups.Count;

        #endregion
    }
}