//
// IConstraint.cs
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

using System.Collections.Generic;
using UIKit;

namespace Int.iOS.Wrappers.View
{
    public enum CType
    {
        Ratio,
        MarginLeft,
        MarginRight,
        MarginTop,
        MarginBottom,
        VerticalSpace,
        Width,
        Height,
        WidthOf,
        HeightOf,
        CenterX,
        CenterY
    }


    /// <summary>
    ///     A interface that help to chain constraints
    ///     TranslatesAutoresizingMaskIntoConstraints will be set to false for views
    /// </summary>
    public interface IConstraint
    {
        UIView View { get; }

        UIView View2 { get; }

        /// <summary>
        ///     Gets the last constraint.
        /// </summary>
        /// <value>The last constraint.</value>
        NSLayoutConstraint LastConstraint { get; }

        /// <summary>
        ///     Gets all constraints.
        /// </summary>
        /// <value>All constraints.</value>
        IList<NSLayoutConstraint> AllConstraints { get; }

        IConstraint PinToParrent(float constant = 0f);

        IConstraint CenterToParrent(float multiplier = 1.0f);

        /// <summary>
        ///     Sets the ratio constraint for the view
        /// </summary>
        /// <returns>The ratio constraint.</returns>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint SetRatioConstraint(float multiplier = 1.0f, float constant = 0f);

        /// <summary>
        ///     Set this view to left of second view
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint LeftOf(UIView view2, float multiplier = 1.0f, float constant = 0f);

        /// <summary>
        ///     Set this view to top of second view
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint TopOf(UIView view2, float multiplier = 1.0f, float constant = 0f);

        /// <summary>
        ///     Set this view to right of second view
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint RightOf(UIView view2, float multiplier = 1.0f, float constant = 0f);

        /// <summary>
        ///     Set this view to bottom of second view
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint BottomOf(UIView view2, float multiplier = 1.0f, float constant = 0f);

        /// <summary>
        ///     Set this view under second view
        /// </summary>
        /// <returns>Chain invokable interface.</returns>
        /// <param name="view">View.</param>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint VerticalOf(UIView view2, float multiplier = 1.0f, float constant = 0f);

        /// <summary>
        ///     Set this view center x to  second view center x
        /// </summary>
        /// <returns>The X of.</returns>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint CenterXOf(UIView view2, float multiplier = 1.0f, float constant = 0f);

        /// <summary>
        ///     Set this view centery to second view center y
        /// </summary>
        /// <returns>The Y of.</returns>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint CenterYOf(UIView view2, float multiplier = 1.0f, float constant = 0f);

        /// <summary>
        ///     Set this view width to widht of second view
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint WidthOf(UIView view2, float multiplier = 1.0f, float constant = 0f);

        /// <summary>
        ///     Set this view width to height of second view
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="view2">View2.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint HeightOf(UIView view2, float multiplier = 1.0f, float constant = 0f);

        /// <summary>
        ///     Width the specified multiplier and constant.
        /// </summary>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint Width(float multiplier, float constant);

        /// <summary>
        ///     Height the specified multiplier and constant.
        /// </summary>
        /// <param name="multiplier">Multiplier.</param>
        /// <param name="constant">Constant.</param>
        IConstraint Height(float multiplier, float constant);
    }
}