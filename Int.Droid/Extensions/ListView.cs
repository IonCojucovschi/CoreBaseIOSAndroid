using System;
using System.Reactive.Linq;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Int.Droid.Factories.Adapter;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        private static void ListView_ChildViewAdded(int height, ViewGroup.ChildViewAddedEventArgs e)
        {
            e.Child.LayoutParameters.Height = height;
        }

        public static void SetChildHeight(this ListView listView, int height)
        {
            listView.ChildViewAdded += (sender, e) => ListView_ChildViewAdded(height, e);
        }

        public static IObservable<ItemEventArgs<T>> WhenItemClick<T, TVh>(
            this ComponentViewHolderAdapterFactory<T, TVh> This)
            where TVh : ViewHolder
        {
            return Observable.FromEventPattern<ItemEventArgs<T>>(
                    e => This.ItemClick += e, e => This.ItemClick -= e)
                .Select(args => args.EventArgs);
        }

        public static void SetAdapterRv(this RecyclerView @this, RecyclerView.Adapter adapter)
        {
            @this.SetLayoutManager(new LinearLayoutManager(AppTools.CurrentActivity));
            @this.SetAdapter(adapter);
        }

        public static void DisableScroll(this RecyclerView recyclerView)
        {
            recyclerView.SetLayoutManager(
                new ScrollConfigurableLayoutManager(recyclerView.Context)
                { IsScrollEnabled = false });
        }

        private class ScrollConfigurableLayoutManager : LinearLayoutManager
        {
            public ScrollConfigurableLayoutManager(Context context) : base(context) { }

            public bool IsScrollEnabled { get; set; } = true;

            public override bool CanScrollVertically()
            {
                return IsScrollEnabled && base.CanScrollVertically();
            }
        }
    }
}