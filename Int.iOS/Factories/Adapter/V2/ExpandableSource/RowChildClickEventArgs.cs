using System;

namespace Int.iOS.Factories.Adapter.V2.ExpandableSource
{
    [Serializable]
    public sealed class RowChildClickEventArgs<TParent, TChild> : EventArgs
    {
        public RowChildClickEventArgs(int groupPosition, int inGroupPosition,
            TParent parentModel, TChild childModel)
        {
            ParentModel = parentModel;
            ChildModel = childModel;
            GroupPosition = groupPosition;
            InGroupPosition = inGroupPosition;
        }

        public RowChildClickEventArgs(int groupPosition, int inGroupPosition,
            TParent parentModel, TChild childModel, object tag)
            : this(groupPosition, inGroupPosition, parentModel, childModel)
        {
            Tag = tag;
        }

        public object Tag { get; }

        public TParent ParentModel { get; }
        public TChild ChildModel { get; }

        public int GroupPosition { get; }
        public int InGroupPosition { get; }
    }
}