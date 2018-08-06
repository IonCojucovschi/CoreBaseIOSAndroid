using System;
using System.Collections.Generic;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract.Table;
using Int.Core.Application.Widget.Contract.Table.Adapter;
using Int.Core.Wrappers.Widget.Exceptions;
using Int.iOS.Extensions;
using UIKit;

namespace Int.iOS.Wrappers.Widget.FactoryConcreteProducts
{
    public class CrossListViewWrapper : CrossViewWrapper, IListView
    {
        private const string ClickEventNotImplementedMessage = "You should not assign click events to list itself";

        public event RowClickedEventHandler<object> RowClicked;

        public CrossListViewWrapper(UIView view) : base(view)
        {
            if (!(view is UITableView) && !(view is UICollectionView))
                throw new CrossWidgetWrapperConstructorException(
                    string.Format(NotCompatibleError, view?.GetType()));
        }

        public void UpdateDataSource<T>(IList<T> data)
        {
            AppTools.InvokeOnMainThread(() =>
                GetAdapter<T>()?.UpdateDataSource(data));
        }

        public void ClearFilter<T>()
        {
            AppTools.InvokeOnMainThread(() =>
                GetAdapter<T>()?.ClearFilter());
        }

        public void FilterBy<T>(Func<T, bool> predicate, bool autoReset = true)
        {
            AppTools.InvokeOnMainThread(() =>
                GetAdapter<T>()?.FilterBy(predicate, autoReset));
        }

        protected override void AssignGestureEventHandlers()
        {
            ExceptionLogger.RaiseNonFatalException(
                new ExceptionWithCustomStack(
                    ClickEventNotImplementedMessage, Environment.StackTrace));
        }

        private IAdapter<T> GetAdapter<T>()
        {
            var wrapedListView = WrapedObject as UITableView;

            var wrapedListViewCollection = WrapedObject as UICollectionView;

            if (wrapedListView == null)
            {
                if (wrapedListViewCollection == null)
                {
                    ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        WrappedObjectIsNull,
                        Environment.StackTrace));

                    return null;
                }
            }

            var adapter = wrapedListView?.Source as IAdapter<T> ?? wrapedListViewCollection?.Source as IAdapter<T>;

            if (adapter == null)
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        AdapterIsNull,
                        Environment.StackTrace));

            if (adapter == null) return null;
            adapter.RowClicked -= Adapter_RowClicked;
            adapter.RowClicked += Adapter_RowClicked;

            return adapter;
        }

        private void Adapter_RowClicked<T>(object sender, RowClickedEventArgs<T> e)
        {
            RowClicked?.Invoke(sender, new RowClickedEventArgs<object>(e));
        }

        public void RowCountLimit(int count)
        {
            var wrapedListView = WrapedObject as UITableView;

            if (wrapedListView == null)
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        WrappedObjectIsNull,
                        Environment.StackTrace));
            }

            wrapedListView?.ResizeTableView(count);
        }

        public void Orientation(OrientationListView view)
        {
        }
    }
}