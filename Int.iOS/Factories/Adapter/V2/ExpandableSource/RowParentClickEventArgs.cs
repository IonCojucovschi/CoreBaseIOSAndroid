using System;

namespace Int.iOS.Factories.Adapter.V2.ExpandableSource
{
    [Serializable]
    public sealed class RowParentClickEventArgs<T> : EventArgs
    {
        public RowParentClickEventArgs(int position, T model)
        {
            Model = model;
            Position = position;
        }

        public RowParentClickEventArgs(int position, T model, object tag)
            : this(position, model)
        {
            Tag = tag;
        }

        public object Tag { get; }

        public T Model { get; }

        public int Position { get; }
    }
}