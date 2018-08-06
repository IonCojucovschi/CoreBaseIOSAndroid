using System;
using Int.iOS.Factories.Adapter.CellView;
using UIKit;

namespace Int.iOS.Factories.Adapter.V2
{
    public abstract class UIExpandableTableViewHeaderCell<T> : ComponentTableViewCell<T>
    {
        protected UIExpandableTableViewHeaderCell(IntPtr handle) : base(handle) { }

        /// <summary>
        ///     Gets the clickable view which expand action will be assigned to.
        /// </summary>
        public virtual UIView ClickableViewForExpansion => ContentView;

        /// <summary>
        ///     Called when cell is starting to expand.
        /// </summary>
        public virtual void OnExpand() { }

        /// <summary>
        ///     Called when cell is starting to collapse.
        /// </summary>
        public virtual void OnCollapse() { }
    }
}