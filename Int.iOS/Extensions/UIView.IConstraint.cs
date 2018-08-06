//
// UIView.IConstraint.cs
//
// Author:
//       Songurov Fiodor <songurov@gmail.com>
//
// Copyright (c) 2016 Songurov Fiodor
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

using Int.iOS.Wrappers.View;
using UIKit;

namespace Int.iOS.Extensions
{
    public static partial class Extensions
    {
        public static IConstraint PinToParent(this UIView view, float constant = 0.0f)
        {
            var helper = new Constraint(view);
            return helper.PinToParrent(constant);
        }

        public static IConstraint CenterToParrent(this UIView view, float multiplier = 1)
        {
            var helper = new Constraint(view);
            return helper.CenterToParrent(multiplier);
        }

        /// <summary>
        ///     Replaces the constraint with.
        ///     Will not call layout if needed
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="oldConstraint">Old constraint.</param>
        /// <param name="newConstraint">New constraint.</param>
        public static void ReplaceConstraintWith(this UIView view,
            NSLayoutConstraint oldConstraint,
            NSLayoutConstraint newConstraint)
        {
            view.RemoveConstraint(oldConstraint);
            view.AddConstraint(newConstraint);
        }

        /// <summary>
        ///     Sets the ratio constraint.
        /// </summary>
        /// <returns>The ratio constraint.</returns>
        /// <param name="view">View.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint SetRatioConstraint(this UIView view,
            float multiplier = 1.0f,
            float constant = 0f)
        {
            var helper = new Constraint(view);
            return helper.SetRatioConstraint(multiplier, constant);
        }

        /// <summary>
        ///     Set this view to left of second  view
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view">View.</param>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint LeftOf(this UIView view, UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            var helper = new Constraint(view);
            return helper.LeftOf(view2, multiplier, constant);
        }

        /// <summary>
        ///     Set this view to top of second view
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view">View.</param>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint TopOf(this UIView view, UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            var helper = new Constraint(view);
            return helper.TopOf(view2, multiplier, constant);
        }

        /// <summary>
        ///     Set this view to right of second view
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view">View.</param>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint RightOf(this UIView view, UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            var helper = new Constraint(view);
            return helper.RightOf(view2, multiplier, constant);
        }

        /// <summary>
        ///     Set this view to bottom of second view
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view">View.</param>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint BottomOf(this UIView view, UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            var helper = new Constraint(view);
            return helper.BottomOf(view2, multiplier, constant);
        }

        /// <summary>
        ///     Set this view under second view
        /// </summary>
        /// <returns>Chain invokable interface.</returns>
        /// <param name="view">View.</param>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint VerticalOf(this UIView view, UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            var helper = new Constraint(view);
            return helper.VerticalOf(view2, multiplier, constant);
        }

        /// <summary>
        ///     Set this view to centerX of second view
        /// </summary>
        /// <returns>The X of.</returns>
        /// <param name="view">View.</param>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint CenterXOf(this UIView view, UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            var helper = new Constraint(view);
            return helper.CenterXOf(view2, multiplier, constant);
        }

        /// <summary>
        ///     Set this view to centerY of second view
        /// </summary>
        /// <returns>The Y of.</returns>
        /// <param name="view">View.</param>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint CenterYOf(this UIView view, UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            var helper = new Constraint(view);
            return helper.CenterYOf(view2, multiplier, constant);
        }

        /// <summary>
        ///     Set this view width with the second view width
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view">View.</param>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint WidthOf(this UIView view, UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            var helper = new Constraint(view);
            return helper.WidthOf(view2, multiplier, constant);
        }

        /// <summary>
        ///     Set this view height with the second view height
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view">View.</param>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint HeightOf(this UIView view, UIView view2,
            float multiplier = 1f,
            float constant = 0f)
        {
            var helper = new Constraint(view);
            return helper.HeightOf(view2, multiplier, constant);
        }

        /// <summary>
        ///     Width the specified view, multiplier and constant.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint Width(this UIView view,
            float multiplier,
            float constant)
        {
            var helper = new Constraint(view);
            return helper.Width(multiplier, constant);
        }

        /// <summary>
        ///     Height the specified view, multiplier and constant.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        public static IConstraint Height(this UIView view,
            float multiplier,
            float constant)
        {
            var helper = new Constraint(view);
            return helper.Height(multiplier, constant);
        }
    }
}