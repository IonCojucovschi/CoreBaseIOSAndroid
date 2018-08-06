//
// ComponentViewSource.cs
//
// Author:
//       Sogurov Fiodor <f.songurov@software-dep.net>
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
using CoreGraphics;
using Foundation;
using Int.Core.Application.Widget.Contract.Table;
using Int.Core.Extensions;
using Int.iOS.Factories.Adapter.Abstraction;
using Int.iOS.Factories.Adapter.Contract;
using ObjCRuntime;
using UIKit;

namespace Int.iOS.Factories.Adapter
{
    [Serializable]
    public sealed class DisplayCellEventArgs : EventArgs
    {
        public DisplayCellEventArgs(UITableView tableView, UITableViewCell cell, NSIndexPath index)
        {
            TableView = tableView;
            Cell = cell;
            IndexPath = index;
        }

        public UITableView TableView { get; }
        public UITableViewCell Cell { get; }
        public NSIndexPath IndexPath { get; }
    }

    public delegate void DisplayCellEventHandler(object sender, DisplayCellEventArgs e);

    public abstract class ComponentViewSource<T> : UITableViewSource, IDataSource<T>
    {
        private readonly IDataSource<T> _dataSource;

        protected ComponentViewSource(IList<T> dataSource)
        {
            _dataSource = new InternalDataSource<T>(dataSource);
            _dataSource.DataSetChanged += (sender, e) => ReloadData();
        }

        protected ComponentViewSource(IList<T> dataSource, UITableView tableView) : this(dataSource)
        {
            TableView = tableView;

        }

        protected IList<T> DataSource => (_dataSource as InternalDataSource<T>)?.DataSource;

        protected UITableView TableView { get; }

        public bool AllowRowSelection { get; set; } = true;
        public bool AllowHighlighted { get; set; } = true;

        public int Count => _dataSource.Count
                            + (HasHeader ? 1 : 0)
                            + (HasFooter ? 1 : 0);

        public void AutoResize()
        {
            if (TableView == null)
                throw new Exception("TableView NULL");

            InvokeOnMainThread(() =>
            {
                TableView.EstimatedRowHeight = 44f;
                TableView.RowHeight = UITableView.AutomaticDimension;
                TableView.ReloadData();
            });
        }

        protected TK InitCell<TK>(string identifier, UITableView tableView) where TK : UITableViewCell
        {
            if (tableView.DequeueReusableCell(identifier) is TK cell) return cell;

            var views = NSBundle.MainBundle.LoadNib(identifier, tableView, null);
            cell = Runtime.GetNSObject(views.ValueAt(0)) as TK;

            cell.Bounds = tableView.Bounds;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (AllowRowSelection)
                EmitRowClick(tableView, indexPath.Row, GetItem(indexPath.Row));
        }

        public override void RowHighlighted(UITableView tableView, NSIndexPath rowIndexPath)
        {
            if (!AllowHighlighted) return;
            var cell = tableView.CellAt(rowIndexPath);
            cell.SetHighlighted(true, true);
        }


        public override void RowUnhighlighted(UITableView tableView, NSIndexPath rowIndexPath)
        {
            if (!AllowHighlighted) return;
            var cell = tableView.CellAt(rowIndexPath);
            cell.SetHighlighted(false, true);
        }

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            DisplayCell?.Invoke(this, new DisplayCellEventArgs(tableView, cell, indexPath));
        }

        public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            DisplayCellEnded?.Invoke(this, new DisplayCellEventArgs(tableView, cell, indexPath));
        }

        public override bool ShouldHighlightRow(UITableView tableView, NSIndexPath rowIndexPath)
        {
            return AllowHighlighted;
        }

        private void CheckIfHaveTable()
        {
            if (TableView == null)
                throw new Exception("You need to call constructor with " +
                                    "const(list,tableView)");
        }

        #region EventsScroll

        public event EventHandler TopScroller;
        public event EventHandler BottomScroller;

        public event EventHandler StartScroller;

        public override void DecelerationStarted(UIScrollView scrollView)
        {
            if (scrollView.Bounds.Y == 0)
                BottomScroller?.Invoke(this, EventArgs.Empty);
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            if (scrollView.Bounds.Y > 0)
                TopScroller?.Invoke(this, EventArgs.Empty);
        }

        public override void DraggingStarted(UIScrollView scrollView)
        {
            if (scrollView.Bounds.Y < scrollView.Bounds.Height)
                StartScroller?.Invoke(this, EventArgs.Empty);
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            ViewDidScroll?.Invoke(scrollView, EventArgs.Empty);
        }

        #endregion

        #region Events

        public event EventHandler DataSetChanged;

        public event EventHandler ViewDidScroll;

        public event RowClickedEventHandler<T> RowClicked;

        public event DisplayCellEventHandler DisplayCell;
        public event DisplayCellEventHandler DisplayCellEnded;

        #endregion

        #region IDataSource

        /// <summary>
        ///     Add the specified item.
        ///     Will not work if you dont initialiaze with tableView
        /// </summary>
        /// <param name="item">Item.</param>
        public virtual void Add(T item)
        {
            _dataSource.Add(item);
        }

        public virtual void AddRange(IEnumerable<T> col)
        {
            _dataSource.AddRange(col);
        }

        /// <summary>
        ///     Remove the specified item.
        ///     Will not work if you dont initialiaze with tableView
        /// </summary>
        /// <param name="item">Item.</param>
        public virtual void Remove(T item)
        {
            _dataSource.Remove(item);
        }

        public T GetItem(int pos)
        {
            var dataSourceItem = default(T);

            var realDataSourcePosition = pos;
            var realDataSourceCount = Count;
            if (HasHeader) realDataSourcePosition--;
            if (HasHeader) realDataSourceCount--;
            if (HasFooter) realDataSourceCount--;

            if (realDataSourcePosition >= 0 && realDataSourcePosition < realDataSourceCount)
                dataSourceItem = _dataSource.GetItem(realDataSourcePosition);

            return dataSourceItem;
        }

        public virtual int IndexOf(T item)
        {
            var index = _dataSource.IndexOf(item);
            if (index != -1 && HasHeader)
                index++;
            return index;
        }

        /// <summary>
        ///     Filters the by.
        ///     Will not work if you dont initialiaze with tableView
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <param name="autoReset">If set to <c>true</c> auto reset.</param>
        public virtual void FilterBy(Func<T, bool> predicate, bool autoReset = true)
        {
            _dataSource.FilterBy(predicate, autoReset);
        }

        /// <summary>
        ///     Clears the filter.
        ///     Will not work if you dont initialiaze with tableView
        /// </summary>
        public virtual void ClearFilter()
        {
            _dataSource.ClearFilter();
        }

        public virtual void UpdateDataSource(IList<T> newDataSource)
        {
            if (newDataSource.IsNull()) return;
            _dataSource.UpdateDataSource(newDataSource);
        }

        public void ReloadData()
        {
            DataSetChanged?.Invoke(this, EventArgs.Empty);
            CheckIfHaveTable();
            TableView.InvokeOnMainThread(TableView.ReloadData);
        }

        public void RefrashView()
        {
            TableView.ReloadRows(new NSIndexPath[] { }, UITableViewRowAnimation.None);
        }

        #endregion

        #region virtual funct

        protected virtual void EmitRowClick(object sender, T model)
        {
            RowClicked?.Invoke(sender, new RowClickedEventArgs<T>(model));
        }

        protected virtual void EmitRowClick(object sender, int position, T model)
        {
            RowClicked?.Invoke(sender, new RowClickedEventArgs<T>(
                position, model));
        }

        protected virtual void EmitRowClick(object sender, int position, T model,
            object tag)
        {
            RowClicked?.Invoke(sender, new RowClickedEventArgs<T>(
                position, model, tag));
        }

        #endregion

        #region Header and Footer

        private string _headerCellResuseIdentifier;
        private string _footerCellResuseIdentifier;
        private UITableViewCell _headerCell;
        private UITableViewCell _footerCell;

        protected bool HasHeader => !string.IsNullOrWhiteSpace(_headerCellResuseIdentifier);
        protected bool HasFooter => !string.IsNullOrWhiteSpace(_footerCellResuseIdentifier);


        /// <summary>
        ///     Adds the header. Count value increased by one.
        ///     RowClick event will return:
        ///     * zero based row index starting from header
        ///     * null dafult(T) model.
        /// </summary>
        public TCell AddHeader<TCell>() where TCell : UITableViewCell
        {
            _headerCellResuseIdentifier = typeof(TCell).Name;
            _headerCell = null;
            GetHeaderCell();
            return (TCell)_headerCell;
        }

        /// <summary>
        ///     Adds the footer. Count value increased by one.
        ///     RowClick event will return:
        ///     * zero based row index taking in account this footer
        ///     * null default(T) model.
        /// </summary>
        public TCell AddFooter<TCell>() where TCell : UITableViewCell
        {
            _footerCellResuseIdentifier = typeof(TCell).Name;
            _footerCell = null;
            GetFooterCell();
            return (TCell)_footerCell;
        }

        public void AddFooterView(UIView viewFooter, float heightView = 44)
        {
            if (!TableView.IsNull())
            {
                viewFooter.Frame = new CoreGraphics.CGRect(0, 0, TableView.Frame.Width, heightView);
                TableView.TableFooterView = viewFooter;
            }
        }

        public void AddHeaderView(UIView viewHeader, float heightView = 44)
        {
            if (!TableView.IsNull())
            {
                viewHeader.Frame = new CoreGraphics.CGRect(0, 0, TableView.Frame.Width, heightView);
                TableView.TableHeaderView = viewHeader;
            }
        }

        protected UITableViewCell GetHeaderCell()
        {
            CheckIfHaveTable();
            return _headerCell ?? (_headerCell = InitCell<UITableViewCell>(_headerCellResuseIdentifier, TableView));
        }

        protected UITableViewCell GetFooterCell()
        {
            CheckIfHaveTable();
            return _footerCell ?? (_footerCell = InitCell<UITableViewCell>(_footerCellResuseIdentifier, TableView));
        }

        #endregion
    }
}