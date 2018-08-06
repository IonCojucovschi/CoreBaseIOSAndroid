//
// StringAdapter.cs
//
// Author:
//       valentingrigorean <v.grigorean@software-dep.net>
//
// Copyright (c) 2016 valentingrigorean
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

using System.Collections.Generic;
using System.Diagnostics;
using UIKit;

namespace Int.iOS.Controllers.DropdownView
{
    public class StringAdapter : BaseAdapter<string>
    {
        private bool _wasInit;


        public StringAdapter(IList<string> dataSource) : base(dataSource)
        {
        }

        public SimpleCellView.SimpleCellViewAppearance Apperance { get; set; }

        #region IAdapter implementation

        public override UITableViewCell GetView(int position, UITableView tableView)
        {
            if (!_wasInit)
            {
                _wasInit = true;
                tableView.RegisterNibForCellReuse(SimpleCellView.Nib,
                    SimpleCellView.CellId);
            }
            var cell = tableView.DequeueReusableCell(SimpleCellView.CellId) as SimpleCellView;
            if (Apperance != null)
                if (cell != null) cell.AppearanceCell = Apperance;
            Debug.Assert(cell != null, "cell != null");
            cell.Model = DataSource[position];
            return cell;
        }

        #endregion
    }
}