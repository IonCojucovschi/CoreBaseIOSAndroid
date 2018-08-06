//
// AutoCompleteTextField.cs
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
using CoreGraphics;
using Foundation;
using UIKit;

namespace Int.iOS.Controllers
{
    public static class AutoCompleteTextFieldManager
    {
        private static List<AutoCompleteTextField> _autoCompleteTextFields;

        public static void Add(UIViewController viewController, UITextField textView, List<string> elements,
            Action callBackFinish)
        {
            if (_autoCompleteTextFields == null)
                _autoCompleteTextFields = new List<AutoCompleteTextField>();
            _autoCompleteTextFields.Add(new AutoCompleteTextField(viewController, textView, elements, callBackFinish));
        }

        public static void RemoveAll()
        {
            _autoCompleteTextFields?.Clear();
        }

        private class AutoCompleteTextField
        {
            private readonly UITableView _autoCompleteTableView;
            private readonly List<string> _elements;
            private readonly Action _selected;
            private readonly UITextField _textField;
            private readonly UIViewController _viewController;
            private UITableViewSource _autoCompleteTableViewSource;
            private List<string> _matchedElements;
            private int _selectedIndex = -1;
            private EventHandler _textFieldEditingChangedHandler;

            public AutoCompleteTextField(UIViewController viewController, UITextField textView, List<string> elements,
                Action selected)
            {
                _selected = selected;
                _viewController = viewController;
                _textField = textView;
                _selectedIndex = -1;
                _elements = elements;
                _matchedElements = _elements
                    .Where(e => e.ToLowerInvariant().Contains(_textField.Text.ToLowerInvariant())).ToList();
                _autoCompleteTableView = new UITableView(new CGRect(0, _textField.Frame.Y,
                    _viewController.View.Frame.Width,
                    _viewController.View.Frame.Height / 2 - _textField.Frame.Y - _textField.Frame.Height));
                _autoCompleteTableViewSource = new AutoCompleteTableViewSource(_matchedElements, SelectedElement);
                _autoCompleteTableView.ReloadData();
                _autoCompleteTableView.ScrollEnabled = true;
                _autoCompleteTableView.Hidden = true;
                _viewController.View.AddSubview(_autoCompleteTableView);

                _textField.EditingChanged -= GetTextFieldEditingChangedHandler();
                _textField.EditingChanged += GetTextFieldEditingChangedHandler();
            }

            private void SelectedElement(nint index)
            {
                _selectedIndex = (int) index;
                var selectedElement = _matchedElements[_selectedIndex];
                _textField.Text = selectedElement;
                _autoCompleteTableView.Hidden = true;
                _selected();
            }

            private EventHandler GetTextFieldEditingChangedHandler()
            {
                if (_textFieldEditingChangedHandler == null)
                    _textFieldEditingChangedHandler = delegate
                    {
                        _selectedIndex = -1;
                        _autoCompleteTableView.Hidden = false;
                        _matchedElements = _elements.Where(elem =>
                            elem.ToLowerInvariant().Contains(_textField.Text.ToLowerInvariant())).ToList();

                        if (string.IsNullOrWhiteSpace(_textField.Text))
                            _autoCompleteTableView.Hidden = true;
                        else _autoCompleteTableView.Hidden |= _matchedElements.Count == 0;

                        _autoCompleteTableViewSource =
                            new AutoCompleteTableViewSource(_matchedElements, SelectedElement);
                        _autoCompleteTableView.Source = _autoCompleteTableViewSource;
                        _autoCompleteTableView.ReloadData();
                    };
                return _textFieldEditingChangedHandler;
            }
        }

        private class AutoCompleteTableViewSource : UITableViewSource
        {
            private readonly List<string> _elements;
            private readonly Action<nint> _selectedElementCallback;

            public AutoCompleteTableViewSource(List<string> elements, Action<nint> selectedElementCallback)
            {
                _elements = elements;
                _selectedElementCallback = selectedElementCallback;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return _elements.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var element = _elements[indexPath.Row];

                var cell = new UITableViewCell();
                cell.TextLabel.Text = element;
                cell.TextLabel.TextColor = UIColor.DarkGray;

                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                _selectedElementCallback?.Invoke(indexPath.Row);
            }
        }
    }
}