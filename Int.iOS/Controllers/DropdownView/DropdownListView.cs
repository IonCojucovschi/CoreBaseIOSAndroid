//
// DropdownListView.cs
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

using System;
using CoreGraphics;
using Foundation;
using Int.iOS.Extensions;
using UIKit;

namespace Int.iOS.Controllers.DropdownView
{
    public sealed class ItemSelectedEventArgs : EventArgs
    {
        public ItemSelectedEventArgs(int position, object item)
        {
            Position = position;
        }

        public int Position { get; }
        public object Item { get; private set; }
    }

    public enum Anchor
    {
        Top,
        Bottom
    }

    public interface IDropdownDelegate
    {
        bool ShouldClose { get; }
        void PrepareForShow(UIView contentView, UITableView tableView, CGRect frameForTable);
        void PrepareForHide(UIView contentView, UITableView tableView);
    }

    public class DropdownListView : UIView
    {
        private readonly UILabel _label;
        private readonly Options _options = new Options();
        private readonly UITextField _textField;
        private readonly CType _type;
        private IAdapter _adapter;
        private UIView _mockView;

        private UITableView _tableView;

        public DropdownListView()
            : base(UIScreen.MainScreen.Bounds)
        {
            Initialiaze();
            _type = CType.Custom;
        }

        public DropdownListView(UITextField textField) : this()
        {
            _textField = textField;
            _type = CType.Text;
            AnchorView = textField;
        }

        public DropdownListView(UILabel label) : this()
        {
            _label = label;
            _type = CType.Label;
            AnchorView = label;
        }

        public DropdownAppearance AppearanceDropdown { get; set; } = SharedAppearance;

        public static DropdownAppearance SharedAppearance { get; } = new DropdownAppearance
        {
            Background = UIColor.Clear,
            Radius = 0f
        };

        public UIView ContentView { get; private set; }

        public UIView AnchorView { get; set; }

        public int MinHeight { get; set; }
        public int MinWidth { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }

        public bool IsVisible { get; private set; }

        public int ItemToShow { get; set; } = -1;

        public IDropdownDelegate Delegate { get; set; }

        public int SelectedItemPosition { get; private set; }

        public IAdapter Adapter
        {
            get => _adapter;
            set
            {
                if (value != null)
                    value.NotifyDataSetChanged -= DataSetChangeEvent;
                _adapter = value;
                InitAdapter();
            }
        }

        public event EventHandler<ItemSelectedEventArgs> ItemSelected;

        public event EventHandler OnDismiss;

        /// <summary>
        ///     Show this instance.
        ///     Will use AnchorView to show the dropdown
        /// </summary>
        public void Show()
        {
            if (AnchorView == null)
                throw new Exception("You need to set AnchorView before calling this function");
            Show(AnchorView, Anchor.Bottom);
        }

        /// <summary>
        ///     Show this instance.
        ///     Will use AnchorView to show the dropdown
        /// </summary>
        public void Show(Anchor anchor)
        {
            if (AnchorView == null)
                throw new Exception("You need to set AnchorView before calling this function");
            Show(AnchorView, anchor);
        }


        /// <summary>
        ///     Show the specified view.
        /// </summary>
        /// <param name="view">View is used to show dropdown.</param>
        public void Show(UIView view)
        {
            Show(view, Anchor.Bottom);
        }

        public void Show(UIView view, Anchor anchor)
        {
            var viewFrame = view.RelativeRectToScreen();
            if (view == null)
                throw new ArgumentNullException(nameof(view), "view == null");
            if (_adapter == null)
                throw new NullReferenceException("adapter");
            if (_adapter.Count == 0)
                return;
            _options.CellHeight = (int) _adapter.GetView(0, _tableView).Bounds.Height;
            _options.AnchorFrame = viewFrame;
            _options.Anchor = anchor;
            Show(GetFrameFromOptions(_options));
        }

        public void Show(CGRect frame)
        {
            if (IsVisible) return;
            if (Delegate != null)
                Delegate.PrepareForShow(ContentView, _tableView, frame);
            else
                SetFrameForTable(frame);
            _mockView.BackgroundColor = AppearanceDropdown.Background;
            ContentView.Layer.CornerRadius = AppearanceDropdown.Radius;
            ContentView.Layer.MasksToBounds = true;
            IsVisible = true;
            Alpha = 0f;
            UIApplication.SharedApplication.Delegate.GetWindow().Add(this);
            AnimateNotify(0.3f, () => Alpha = 1f, null);
        }

        public void Hide()
        {
            if (!IsVisible) return;
            OnDismiss?.Invoke(this, EventArgs.Empty);
            if (Delegate != null)
            {
                if (!Delegate.ShouldClose) return;
                Delegate.PrepareForHide(this, _tableView);
            }

            AnimateNotify(0.3f, () => Alpha = 0f,
                _ =>
                {
                    RemoveFromSuperview();
                    IsVisible = false;
                });
        }

        private void SetFrameForTable(CGRect frame)
        {
            ContentView.Frame = frame;
            _tableView.Frame = new CGRect(CGPoint.Empty, frame.Size);
        }

        private CGRect GetFrameFromOptions(Options options)
        {
            var frame = CGRect.Empty;
            var screenBounds = UIScreen.MainScreen.Bounds;
            var yOffset = options.AnchorFrame.Y;
            nfloat height = 0;
            int count;
            switch (options.Anchor)
            {
                case Anchor.Bottom:
                    yOffset += options.AnchorFrame.Height;
                    while (height + options.CellHeight + yOffset < screenBounds.Height)
                    {
                        count = (int) height / (int) options.CellHeight;
                        if (ItemToShow > 0 && count == ItemToShow)
                            break;
                        if (count >= Adapter.Count)
                            break;
                        height += options.CellHeight;
                    }
                    height = (nfloat) Math.Max(height, MinHeight);
                    frame.X = options.AnchorFrame.X + XOffset;
                    frame.Y = options.AnchorFrame.Height + options.AnchorFrame.Y + YOffset;
                    frame.Width = (nfloat) Math.Max(options.AnchorFrame.Width, MinWidth);
                    frame.Height = height;
                    break;
                case Anchor.Top:
                    while (yOffset - (height + options.CellHeight) > 0)
                    {
                        count = (int) height / (int) options.CellHeight;
                        if (ItemToShow > 0 && count == ItemToShow)
                            break;
                        if (count >= Adapter.Count)
                            break;
                        height += options.CellHeight;
                    }
                    height = (nfloat) Math.Max(height, MinHeight);
                    frame.X = options.AnchorFrame.X + XOffset;
                    frame.Y = options.AnchorFrame.Y - height - YOffset;
                    frame.Width = (nfloat) Math.Max(options.AnchorFrame.Width, MinWidth);
                    frame.Height = height;
                    break;
            }
            return frame;
        }

        private void DataSetChangeEvent(object sender, EventArgs e)
        {
            if (_tableView == null) return;
            _tableView.ReloadData();
            if (Adapter.Count == 0)
                Hide();
            else
                SetFrameForTable(GetFrameFromOptions(_options));
        }

        private void InitAdapter()
        {
            if (_adapter.Count == 0)
                return;
            _tableView?.RemoveFromSuperview();
            _adapter.NotifyDataSetChanged += DataSetChangeEvent;
            _tableView = new UITableView(UIScreen.MainScreen.Bounds);
            var source = new ViewSourceInternal(Adapter, this);
            _tableView.Source = source;
            ContentView.AddSubview(_tableView);
        }


        private void OnDismissF(UITableViewCell cell, int position)
        {
            SelectedItemPosition = position;
            ItemSelected?.Invoke(this, new ItemSelectedEventArgs(position, Adapter.GetItem(position)));
            if (_type == CType.Custom) return;
            switch (_type)
            {
                case CType.Text:
                    _textField.Text = Adapter.GetItem(position).ToString();
                    break;
                case CType.Label:
                    _label.Text = Adapter.GetItem(position).ToString();
                    break;
                case CType.Custom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Hide();
        }

        private void Initialiaze()
        {
            _mockView = new UIView(UIScreen.MainScreen.Bounds);
            _mockView.OnClick(Hide);
            AddSubview(_mockView);

            ContentView = new UIView(_mockView.Bounds) {BackgroundColor = UIColor.Clear};
            ContentView.Layer.MasksToBounds = true;
            AddSubview(ContentView);

            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowRadius = 1f;
            Layer.ShadowOpacity = 0.5f;

            ContentView.Layer.MasksToBounds = true;
        }

        private enum CType
        {
            Custom,
            Text,
            Label
        }

        private delegate void OnDismissCallback(UITableViewCell cell, int position);

        private class Options
        {
            public nfloat CellHeight { get; set; }
            public CGRect AnchorFrame { get; set; }
            public Anchor Anchor { get; set; }
        }

        private class ViewSourceInternal : UITableViewSource
        {
            private readonly IAdapter _adapter;
            private readonly DropdownListView _listView;

            public ViewSourceInternal(IAdapter adapter, DropdownListView listView)
            {
                _adapter = adapter;
                _listView = listView;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.CellAt(indexPath);
                cell.Selected = true;
                _listView.OnDismissF(cell, indexPath.Row);
            }

            public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.CellAt(indexPath);
                cell.Selected = false;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return _adapter.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = _adapter.GetView(indexPath.Row, tableView);
                return cell;
            }
        }

        public class DropdownAppearance
        {
            public UIColor Background { get; set; }
            public float Radius { get; set; }
        }
    }
}