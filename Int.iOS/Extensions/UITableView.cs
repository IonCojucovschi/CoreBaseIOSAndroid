//
// UITableView.cs
//
// Author:
//       arslan <chameleon256@gmail.com>
//
// Copyright (c) 2017 (c) ARSLAN ATAEV
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

using CoreGraphics;
using Foundation;
using UIKit;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        public static void ScrollToEnd(this UITableView tableView, bool animated = false, int section = 0)
        {
            var lastRowIndex = tableView.NumberOfRowsInSection(section) - 1;

            if (lastRowIndex < 0) return;

            tableView.InvokeOnMainThread(() =>
            {
                tableView.ScrollToRow(NSIndexPath.FromRowSection(lastRowIndex, section),
                    UITableViewScrollPosition.Top, animated);
            });
        }

        public static void ResizeTableView(this UITableView table, int count)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                table.SetNeedsLayout();
                table.LayoutIfNeeded();

                table.RowHeight = table.Frame.Height / count;
            });
        }

        public static void ResizeTableView(this UITableView table, int count, float height)
        {
            table.SetNeedsLayout();
            table.LayoutIfNeeded();

            table.RowHeight = height;

            table.Frame = new CGRect(table.Frame.X, table.Frame.Y, table.Frame.Width, count * height);
        }
    }
}