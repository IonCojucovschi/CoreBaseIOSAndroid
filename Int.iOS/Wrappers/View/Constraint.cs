//
// Constraint.cs
//
// Author:
//       Valentin <valentin.grigorean1@gmail.com>
//
// Copyright (c) 2016 Valentin
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
using Int.iOS.Extensions;
using UIKit;

namespace Int.iOS.Wrappers.View
{
    internal class Constraint : IConstraint
    {
        private readonly IDictionary<CType, UIView> _exists =
            new Dictionary<CType, UIView>();

        private float _constant;
        private float _multiplier;

        private UIView _parent;

        public Constraint(UIView view)
        {
            View = view;
            View.TranslatesAutoresizingMaskIntoConstraints = false;
        }

        public IConstraint PinToParrent(float constant = 0)
        {
            var parent = View.Superview;
            return View.LeftOf(parent, 1f, constant).RightOf(parent, 1f, constant).TopOf(parent, 1f, constant)
                .BottomOf(parent, 1f, constant);
        }

        public IConstraint CenterToParrent(float multiplier = 1)
        {
            var parent = View.Superview;
            return View.CenterXOf(parent, multiplier).CenterYOf(parent, multiplier);
        }


        private void SetConstraint(CType type)
        {
            InitAndValidate(type);

            var rel1 = NSLayoutAttribute.Width;
            var rel2 = NSLayoutAttribute.Height;

            switch (type)
            {
                case CType.MarginLeft:
                    rel1 = rel2 = NSLayoutAttribute.Left;
                    break;
                case CType.MarginTop:
                    rel1 = rel2 = NSLayoutAttribute.Top;
                    break;
                case CType.MarginRight:
                    rel1 = rel2 = NSLayoutAttribute.Right;
                    break;
                case CType.MarginBottom:
                    rel1 = rel2 = NSLayoutAttribute.Bottom;
                    break;
                case CType.VerticalSpace:
                    rel1 = NSLayoutAttribute.Top;
                    rel2 = NSLayoutAttribute.Bottom;
                    break;
                case CType.CenterX:
                    rel1 = rel2 = NSLayoutAttribute.CenterX;
                    break;
                case CType.CenterY:
                    rel1 = rel2 = NSLayoutAttribute.CenterY;
                    break;
                case CType.WidthOf:
                    rel1 = rel2 = NSLayoutAttribute.Width;
                    break;
                case CType.HeightOf:
                    rel1 = rel2 = NSLayoutAttribute.Height;
                    break;
                case CType.Width:
                    rel1 = NSLayoutAttribute.Width;
                    rel2 = NSLayoutAttribute.NoAttribute;
                    break;
                case CType.Height:
                    rel1 = NSLayoutAttribute.Height;
                    rel2 = NSLayoutAttribute.NoAttribute;
                    break;
            }

            var constraint = NSLayoutConstraint.Create(
                View,
                rel1,
                NSLayoutRelation.Equal,
                View2,
                rel2,
                _multiplier,
                _constant);

            AllConstraints.Add(constraint);
            _parent.AddConstraint(constraint);
        }

        private void InitAndValidate(CType type)
        {
            if (_exists.ContainsKey(type))
                throw new Exception($"{type} allready exists");
            switch (type)
            {
                case CType.Ratio:
                    View2 = _parent = View;
                    _exists.Add(type, View);
                    return;
                case CType.Width:
                case CType.Height:
                    _parent = View;
                    View2 = null;
                    _exists.Add(type, View);
                    return;
            }

            if (View2 == null)
                throw new Exception("Second view == null");
            if (View.Superview == null && View2.Superview == null)
            {
                var window = UIApplication.SharedApplication.Delegate.GetWindow();
                _parent = TryFindRecursiveParrent(window) ?? View2;
            }
            else if (Equals(View.Superview, View2))
            {
                _parent = View2;
            }
            else if (Equals(View.Superview, View2.Superview))
            {
                _parent = View.Superview;
            }
            else if (Equals(View2.Superview, View))
            {
                _parent = View;
            }
            else
            {
                throw new Exception("Invalid constraint views don't " +
                                    "have same parrent");
            }

            View2.TranslatesAutoresizingMaskIntoConstraints = false;

            _exists.Add(type, View2);
        }

        private UIView CheckParrent(IDisposable view)
        {
            if (Equals(View, view))
                return View;
            return Equals(view, View2) ? View2 : null;
        }

        private UIView TryFindRecursiveParrent(UIView startView)
        {
            var parrent = CheckParrent(startView);
            if (parrent != null)
                return parrent;
            foreach (var t in startView.Subviews)
            {
                parrent = TryFindRecursiveParrent(t);
                if (parrent != null)
                    return parrent;
            }
            return null;
        }

        #region IConstraint implementation

        public UIView View { get; }

        public UIView View2 { get; private set; }

        public IConstraint SetRatioConstraint(float multiplier = 1f, float constant = 0f)
        {
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.Ratio);
            return this;
        }

        public IConstraint LeftOf(UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            View2 = view2;
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.MarginLeft);
            return this;
        }

        public IConstraint TopOf(UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            View2 = view2;
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.MarginTop);
            return this;
        }

        public IConstraint RightOf(UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            View2 = view2;
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.MarginRight);
            return this;
        }

        public IConstraint BottomOf(UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            View2 = view2;
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.MarginBottom);
            return this;
        }

        public IConstraint VerticalOf(UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            View2 = view2;
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.VerticalSpace);
            return this;
        }

        public IConstraint CenterXOf(UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            View2 = view2;
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.CenterX);
            return this;
        }

        public IConstraint CenterYOf(UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            View2 = view2;
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.CenterY);
            return this;
        }

        public IConstraint WidthOf(UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            View2 = view2;
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.WidthOf);
            return this;
        }

        public IConstraint HeightOf(UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            View2 = view2;
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.HeightOf);
            return this;
        }

        public IConstraint Width(float multiplier, float constant)
        {
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.Width);
            return this;
        }

        public IConstraint Height(float multiplier, float constant)
        {
            _multiplier = multiplier;
            _constant = constant;
            SetConstraint(CType.Height);
            return this;
        }


        public NSLayoutConstraint LastConstraint => AllConstraints[AllConstraints.Count - 1];

        public IList<NSLayoutConstraint> AllConstraints { get; } = new List<NSLayoutConstraint>();

        #endregion
    }
}