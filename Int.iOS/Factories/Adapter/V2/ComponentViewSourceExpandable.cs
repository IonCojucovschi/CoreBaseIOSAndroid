using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Int.Core.Application.Widget.Contract.Table;
using Int.Core.Extensions;
using Int.Core.Factories.Adapter.V2;
using Int.iOS.Extensions;
using Int.iOS.Factories.Adapter.V2.ExpandableSource;
using UIKit;

namespace Int.iOS.Factories.Adapter.V2
{
    public abstract class ComponentViewSourceExpandable<TItem, TSubItem, TVHeader, TVCell> : ComponentViewSource<TItem>
        where TItem : IExpandableCellData<TSubItem>
        where TVHeader : UIExpandableTableViewHeaderCell<TItem>
        where TVCell : UITableViewCell
    {
        public delegate void RowChildClickEventHandler<TParent, TChild>(object sender,
            RowChildClickEventArgs<TParent, TChild> e);

        public delegate void RowParentClickEventHandler<T>(object sender, RowParentClickEventArgs<T> e);

        private readonly Dictionary<int, IExpandable> _listHeaderCells = new Dictionary<int, IExpandable>();

        private readonly bool _singleExpandable;

        protected ComponentViewSourceExpandable(UITableView tableView, IList<TItem> items,
            bool singleExpandable = true, bool ignorTableStyle = false) : base(items, tableView)
        {
            if (!ignorTableStyle && tableView.Style != UITableViewStyle.Grouped)
                throw new ComponentViewSourceExpandableException(
                    $"Set TableView style to Grpouped in Storyboard via XCode otherwise override {nameof(ignorTableStyle)}.");

            DataSetChanged += (sender, e) => InitHeaderStates();
            InitHeaderStates();
            _singleExpandable = singleExpandable;
        }

        private static string CellIdentifierHeader => typeof(TVHeader).Name;
        private static string CellIdentifierChildren => typeof(TVCell).Name;

        private IList<bool> ItemExpandedState { get; set; }
        protected IList<TItem> Items => DataSource;

        protected virtual nfloat HeightHeader { get; set; } = 40.0f;
        protected virtual nfloat HeightRow { get; set; } = 40.0f;

        private void InitHeaderStates()
        {
            ItemExpandedState = new List<bool>(Items.Select(item => false));
        }

        [Obsolete("RowClicked is deprecated, please use ParentRowClicked and ChildRowClicked instead.")]
        public new event RowClickedEventHandler<TItem> RowClicked;

        public event RowParentClickEventHandler<TItem> ParentRowClicked;
        public event RowChildClickEventHandler<TItem, TSubItem> ChildRowClicked;

        protected abstract void OnBindCell(TVCell viewCell, TVHeader viewHeader, TSubItem model, int position,
            int positionParent);

        protected abstract void OnBindHeader(TVHeader viewHeader, TItem model, bool expanded, int position);

        protected abstract void OnCollapseCell(int collapsePosition);

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (Items == null)
                throw new ComponentViewSourceExpandableException("Item list wasn't initialized.");

            var view = InitCell<TVCell>(CellIdentifierChildren, tableView);
            var headerCell = InitCell<TVHeader>(CellIdentifierHeader, tableView);

            var subItems = Items.ElementAtOrDefault(indexPath.Section)?.SubExpandItems ?? new List<TSubItem>();
            var modelCell = subItems.ElementAtOrDefault(indexPath.Row);

            OnBindCell(view, headerCell, modelCell, indexPath.Row, indexPath.Section);

            if (Items.Count - 1 == indexPath.Row)
                LastItem();

            return view;
        }

        protected virtual void LastItem()
        {
        }

        public override void HeaderViewDisplayingEnded(UITableView tableView, UIView headerView, nint section)
        {
            _listHeaderCells.Remove((int)section);
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return HeightHeader;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var headerCell = InitCell<TVHeader>(CellIdentifierHeader, tableView);

            var sec = (int)section;
            var modelHeader = Items.ElementAtOrDefault(sec);

            OnBindHeader(headerCell, modelHeader, ItemExpandedState.ElementAtOrDefault(sec), sec);

            headerCell.ClickableViewForExpansion.OnClick(() =>
            {
                ParentRowClicked?.Invoke(tableView, new RowParentClickEventArgs<TItem>((int)section, modelHeader));

                if (ItemExpandedState.ElementAtOrDefault(sec))
                    CollapseItem(sec);
                else
                    ExpandItem(sec);
            });

            if (_listHeaderCells.ContainsKey(sec))
                _listHeaderCells[sec] = headerCell as IExpandable;
            else
                _listHeaderCells.Add(sec, headerCell as IExpandable);

            return headerCell.ContentView;
        }

        private void ExpandItem(int section)
        {
            // Return if current expanded item and item to expand are same.
            if (_singleExpandable && ItemExpandedState.Select((state, pos) => new Tuple<int, bool>(pos, state))
                    .FirstOrDefault(arg => arg.Item2)?.Item1 == section)
                return;

            if (_singleExpandable)
                for (var i = 0; i < ItemExpandedState.Count(); i++)
                    if (ItemExpandedState[i])
                        CollapseItem(i);

            ItemExpandedState[section] = true;

            if (_listHeaderCells.TryGetValue(section, out var headerCell))
                headerCell?.OnExpand();

            var itemsInSection = (int)RowsInSection(TableView, section);
            if (itemsInSection <= 0) return;

            var rowsToAdd = new NSIndexPath[itemsInSection];
            for (var i = 0; i < itemsInSection; i++)
                rowsToAdd[i] = NSIndexPath.FromRowSection(i, section);

            TableView.InsertRows(rowsToAdd, UITableViewRowAnimation.None);
        }

        private void CollapseItem(int section)
        {
            if (_listHeaderCells.TryGetValue(section, out var headerCell))
                headerCell?.OnCollapse();

            var itemsInSection = (int)RowsInSection(TableView, section);
            if (itemsInSection <= 0) return;

            ItemExpandedState[(int)section] = false;
            var rowsToRemove = new NSIndexPath[itemsInSection];
            for (var i = 0; i < itemsInSection; i++)
                rowsToRemove[i] = NSIndexPath.FromRowSection(i, section);

            TableView.DeleteRows(rowsToRemove, UITableViewRowAnimation.None);

            OnCollapseCell(section);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return Items.IsNull() ? 0 : Items.Count();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (!ItemExpandedState.ElementAtOrDefault((int)section)) return 0;
            return Items.ElementAtOrDefault((int)section)?.SubExpandItems?.Count ?? 0;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (!AllowRowSelection) return;
            if ((Items?.Count ?? 0) <= 0) return;

            if (Items == null) return;
            var item = Items.ElementAtOrDefault(indexPath.Section);
            var subItems = item?.SubExpandItems;

            if ((subItems?.Count ?? 0) <= 0) return;

            if (subItems != null)
                ChildRowClicked?.Invoke(tableView, new RowChildClickEventArgs<TItem, TSubItem>(
                    indexPath.Section, indexPath.Row, item, subItems[indexPath.Row]));
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return HeightRow;
        }

        protected virtual void OnRowClicked(RowClickedEventArgs<TItem> e)
        {
            RowClicked?.Invoke(this, e);
        }
    }
}