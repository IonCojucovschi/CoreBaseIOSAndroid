//
//  ComponentViewSourceFactory.Collection.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2018 Songurov Fiodor
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Int.Core.Application.Widget.Contract.Table.Adapter;
using Int.iOS.Factories.Adapter.Abstraction;
using Int.iOS.Factories.Adapter.Contract;
using UIKit;

namespace Int.iOS.Factories.Adapter
{
    public static partial class ComponentViewSourceFactory
    {
        public static CollectionViewSource<T> CreateForCollection<T>(string reuseIdentifier, IList<T> dataSource,
                                                                   ICollectionViewSourceDelegate<T> del = null)
        {
            return CreateForCollection(reuseIdentifier, dataSource, null, del);
        }

        public static CollectionViewSource<T> CreateForCollection<T>(string reuseIdentifier, IList<T> dataSource,
                                                                     UICollectionView collectionView,
                                                                     ICollectionViewSourceDelegate<T> del = null,
                                                                     ICrossCellViewHolder<T> crossCellModel = null, bool register = false,
                                                                     Tuple<int, int> widthCountCell = default(Tuple<int, int>))
        {
            return new CollectionViewSourceInternal<T>(reuseIdentifier, dataSource, collectionView, del, crossCellModel, register, widthCountCell);
        }

        private class CollectionViewSourceInternal<T> : CollectionViewSource<T>, IAdapter<T>
        {
            private readonly string _reuseIdentifier;
            private readonly ICollectionViewSourceDelegate<T> _del;
            private readonly ICrossCellViewHolder<T> _crossCellModel;

            private readonly UICollectionView _collectionView;
            private readonly Tuple<int, int> _centreCell;

            public CollectionViewSourceInternal(string reuseIdentifier, IList<T> dataSource,
                                                UICollectionView collectionView, ICollectionViewSourceDelegate<T> del,
                                                ICrossCellViewHolder<T> crossCellModel = null,
                                                bool register = false, Tuple<int, int> centreCell = default(Tuple<int, int>))
                : base(dataSource, collectionView)
            {

                _reuseIdentifier = reuseIdentifier;
                _del = del;
                _crossCellModel = crossCellModel;
                _collectionView = collectionView;
                _centreCell = centreCell;

                if (register)
                    collectionView?.RegisterNibForCell(UINib.FromName(reuseIdentifier, NSBundle.MainBundle), reuseIdentifier);
            }

            private void CentreCell()
            {
                AppTools.InvokeOnMainThread(() =>
                {
                    _collectionView.Superview.LayoutIfNeeded();
                    _collectionView.Superview.SetNeedsDisplay();

                    var insets = _collectionView.ContentInset;
                    var collectioViewWidth = _collectionView.Superview.Bounds.Width;

                    nfloat left = 0;

                    if ((_centreCell.Item1 * _centreCell.Item2) < collectioViewWidth)
                        left = (collectioViewWidth - (_centreCell.Item1 * _centreCell.Item2)) / 2;

                    insets.Left = left;
                    _collectionView.ContentInset = insets;
                });
            }

            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
            {
                var cell = collectionView.DequeueReusableCell(_reuseIdentifier, indexPath);
                if (cell == null)
                    throw new Exception("Invalid reuseIdentifier");
                var rCell = cell as IReusableView<T>;
                if (rCell == null)
                    throw new Exception("Your cell most implement IReusableView<T> or " +
                                        "derive from CollectionViewCell");


                rCell.CrossCellModel = _crossCellModel;
                _del?.OnCreate(cell, collectionView);
                _crossCellModel?.OnCreate();

                rCell.Model = GetItem((int)indexPath.Item);

                if (_centreCell != null)
                    CentreCell();

                return (UICollectionViewCell)cell;
            }

            public void UpdateDataSource(IEnumerable<T> data)
            {
                // named parameter setted for explicit call UpdateDataSource(IList<T> newDataSource)
                UpdateDataSource(newDataSource: data.ToList());
            }
        }
    }
}
