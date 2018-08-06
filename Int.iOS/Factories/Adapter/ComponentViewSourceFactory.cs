//
// ComponentViewSourceFactory.cs
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
using System.Linq;
using Foundation;
using Int.Core.Application.Widget.Contract.Table.Adapter;
using Int.iOS.Factories.Adapter.Contract;
using UIKit;

namespace Int.iOS.Factories.Adapter
{
    public static partial class ComponentViewSourceFactory
    {
        public static ComponentViewSource<T> CreateForTable<T>(string reuseIdentifier, IList<T> dataSource,
            ITableViewSourceDelegate<T> del = null,
            ICrossCellViewHolder<T> crossCellModel = null, bool register = false)
        {
            return CreateForTable(reuseIdentifier, dataSource, null, del, crossCellModel, register);
        }

        public static ComponentViewSource<T> CreateForTable<T>(string reuseIdentifier, IList<T> dataSource,
            UITableView tableView, ITableViewSourceDelegate<T> del = null,
          ICrossCellViewHolder<T> crossCellModel = null, bool register = false, bool vertical = false)
        {
            return new ViewSourceInternal<T>(reuseIdentifier, dataSource, tableView, del, crossCellModel, register, vertical);
        }

        private class ViewSourceInternal<T> : ComponentViewSource<T>, IAdapter<T>
        {
            private readonly ICrossCellViewHolder<T> _crossCellModel;
            private readonly ITableViewSourceDelegate<T> _del;
            private readonly string _reuseIdentifier;
            private IList<T> _dataSource;
            private IReusableView<T> rCell;
            private readonly bool _vertical;

            public ViewSourceInternal(string reuseIdentifier, IList<T> dataSource, UITableView tableView,
           ITableViewSourceDelegate<T> del, ICrossCellViewHolder<T> crossCellModel = null, bool register = false, bool vertical = false)
                : base(dataSource, tableView)
            {
                _reuseIdentifier = reuseIdentifier;
                _del = del;
                _crossCellModel = crossCellModel;
                _dataSource = dataSource;
                _vertical = vertical;

                if (register)
                    tableView?.RegisterNibForCellReuse(UINib.FromName(reuseIdentifier, NSBundle.MainBundle), reuseIdentifier);
            }

            public void UpdateDataSource(IEnumerable<T> data)
            {
                // named parameter setted for explicit call UpdateDataSource(IList<T> newDataSource)
                UpdateDataSource(newDataSource: data.ToList());
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(_reuseIdentifier, indexPath);

                if (HasHeader && indexPath.Row == 0)
                    cell = GetHeaderCell();

                if (HasFooter && indexPath.Row == Count - 1)
                    cell = GetFooterCell();

                if (cell == null)
                    throw new Exception("Invalid reuseIdentifier");

                rCell = cell as IReusableView<T>;
                if (rCell == null)
                    throw new Exception("Your cell most implement IReusableView<T> or " +
                                        "derive from TableViewCell");

                rCell.CrossCellModel = _crossCellModel;

                _del?.OnCreate(cell, tableView, _crossCellModel);
                _crossCellModel?.OnCreate();

                rCell.Count = Count;
                rCell.Navigate = AppTools.RootNavigationController;
                rCell.Storyboard = AppTools.Storyboard;
                rCell.Position = indexPath.Row;
                rCell.TableView = tableView;
                rCell.Model = GetItem(indexPath.Row);

                if (_vertical)
                {
                }

                return cell;
            }
        }
    }
}