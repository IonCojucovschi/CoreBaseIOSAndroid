using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract.Table;
using Int.Core.Application.Widget.Contract.Table.Adapter;
using Int.Core.Extensions;
using Int.Core.Wrappers.Widget.Exceptions;
using Int.Droid.Factories.Adapter.RecyclerView;

namespace Int.Droid.Wrappers.Widget.FactoryConcreteProducts
{
    public class CrossListViewWrapper : CrossViewWrapper, IListView
    {
        private const string ClickEventNotImplementedMessage = "You should not assign click events to list itself";

        public CrossListViewWrapper(View view) : base(view)
        {
            if (!(view is RecyclerView))
                throw new CrossWidgetWrapperConstructorException(
                    string.Format(NotCompatibleError, view?.GetType()));
        }

        public void UpdateDataSource<T>(IList<T> data)
        {
            AppTools.CurrentActivity?.RunOnUiThread(() =>
                GetAdapter<T>()?.UpdateDataSource(data));
        }

        public void FilterBy<T>(Func<T, bool> predicate, bool autoReset = true)
        {
            AppTools.CurrentActivity?.RunOnUiThread(() =>
                GetAdapter<T>()?.FilterBy(predicate, autoReset));
        }

        public void ClearFilter<T>()
        {
            AppTools.CurrentActivity?.RunOnUiThread(() =>
                GetAdapter<T>()?.ClearFilter());
        }

        public event RowClickedEventHandler<object> RowClicked;

        protected override void AssignTouchListeners()
        {
            ExceptionLogger.RaiseNonFatalException(
                new ExceptionWithCustomStack(
                    WrappedObjectIsNull, Environment.StackTrace));
        }

        private IAdapter<T> GetAdapter<T>()
        {
            if (!(WrappedObject is RecyclerView wrapedListView))
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        WrappedObjectIsNull,
                        Environment.StackTrace));

                return null;
            }


            var adapter = wrapedListView.GetAdapter() as IAdapter<T>;
            if (adapter == null)
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        AdapterIsNull,
                        Environment.StackTrace));


            if (!adapter.IsNull())
            {
                adapter.RowClicked -= this.Adapter_RowClicked;
                adapter.RowClicked += this.Adapter_RowClicked;
            }

            return adapter;
        }

        private void Adapter_RowClicked<T>(object sender, RowClickedEventArgs<T> e)
        {
            RowClicked?.Invoke(sender, new RowClickedEventArgs<object>(e));
        }

        public void RowCountLimit(int count)
        {
            var wrapedListView = WrappedObject as RecyclerView;

            _limitCount = count;

            if (wrapedListView == null)
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        WrappedObjectIsNull,
                        Environment.StackTrace));
            }

            wrapedListView?.Post(() =>
            {
                var calculatedHeight = wrapedListView.MeasuredHeight / count;

                if (wrapedListView?.GetAdapter() is IAdapterConfigurable rowHeightCustomizableAdapter)
                    rowHeightCustomizableAdapter.RowHeight = calculatedHeight;
            });
        }

        private int _limitCount = 1;

        public void Orientation(OrientationListView view)
        {
            var wrapedListView = WrappedObject as RecyclerView;

            if (wrapedListView == null)
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        WrappedObjectIsNull,
                        Environment.StackTrace));
            }

            if (view == OrientationListView.Horizontal)
                wrapedListView?.SetLayoutManager(new GridLayoutManager(AppTools.CurrentActivity, _limitCount, LinearLayoutManager.Horizontal, false));
        }
    }
}