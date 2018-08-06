using Android.Support.V4.View;
using Android.Views;
using System.Collections.Generic;
using Android.Content;
using System;

namespace Int.Droid.Views
{
    public abstract class PagerAdapter<T> : PagerAdapter
    {
        private readonly ViewHolderContainer _viewHolder = new ViewHolderContainer();
        private readonly IDictionary<int, Android.Views.View> _inUse =
            new Dictionary<int, View>();

        protected PagerAdapter(Context context, IList<T> collection)
        {
            DataSource = collection;
            Context = context;
            LayoutInflater = LayoutInflater.FromContext(context);
        }

        protected Context Context { get; private set; }

        protected IList<T> DataSource { get; private set; }

        protected LayoutInflater LayoutInflater { get; private set; }

        public override int Count
        {
            get
            {
                return DataSource.Count;
            }
        }

        public abstract View GetView(int position, View convertView,
                                                   ViewGroup parent);


        public override bool IsViewFromObject(Android.Views.View view,
                                              Java.Lang.Object objectValue)
        {
            return view == ((ViewHolder)objectValue).View;
        }

        public override Java.Lang.Object InstantiateItem(
            ViewGroup container, int position)
        {
            var viewHolder = _viewHolder.GetView();
            var view = GetView(position, viewHolder.View, container);
            if (view == null)
                throw new NullReferenceException("view can't be null");
            _inUse.Add(position, view);
            if (viewHolder.View == null)
                viewHolder.View = view;
            viewHolder.IsAttaced = true;
            container.AddView(viewHolder.View);
            return viewHolder;
        }

        public override void DestroyItem(ViewGroup container, int position,
                                         Java.Lang.Object objectValue)
        {
            var view = (ViewHolder)objectValue;
            view.IsAttaced = false;
            _viewHolder.Add(view);
            _inUse.Remove(position);
            container.RemoveView(view.View);
        }

        public View this[int index]
        {
            get
            {
                return _inUse.ContainsKey(index) ? _inUse[index] : null;
            }
        }

        private class ViewHolder : Java.Lang.Object
        {
            public bool IsAttaced { get; set; }

            public Android.Views.View View { get; set; }
        }

        private class ViewHolderContainer
        {
            private readonly List<WeakReference> _container = new List<WeakReference>();

            public void Add(ViewHolder viewHolder)
            {
                _container.Add(new WeakReference(viewHolder));
                Clear();
            }

            public ViewHolder GetView()
            {
                Clear();
                foreach (var item in _container)
                {
                    var viewHolder = (ViewHolder)item.Target;
                    if (item.IsAlive && !viewHolder.IsAttaced)
                        return viewHolder;
                }
                return new ViewHolder();
            }

            private void Clear()
            {
                var list = new List<WeakReference>();
                foreach (var wr in _container)
                {
                    if (wr.IsAlive && !((ViewHolder)wr.Target).IsAttaced)
                        list.Add(wr);
                }
                _container.Clear();
                _container.AddRange(list);
            }
        }
    }
}

