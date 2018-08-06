using System;
using System.Collections.Generic;

namespace Int.Core.Application.Widget.Contract.Table
{
    public enum OrientationListView
    {
        Vertical,
        Horizontal
    }

    public interface IListView : IView
    {
        void UpdateDataSource<T>(IList<T> data);
        void FilterBy<T>(Func<T, bool> predicate, bool autoReset = true);
        void ClearFilter<T>();
        event RowClickedEventHandler<object> RowClicked;
        void RowCountLimit(int count);
        void Orientation(OrientationListView view);
    }

    public delegate void RowClickedEventHandler<T>(object sender, RowClickedEventArgs<T> e);

    public sealed class RowClickedEventArgs<T> : EventArgs
    {
        public RowClickedEventArgs(T model)
        {
            Model = model;
        }

        public RowClickedEventArgs(int position, T model)
        {
            Model = model;
            Position = position;
        }

        public RowClickedEventArgs(int position, T model, object tag)
            : this(position, model)
        {
            Tag = tag;
        }

        public object Tag { get; }
        public T Model { get; }
        public int Position { get; }
    }
}