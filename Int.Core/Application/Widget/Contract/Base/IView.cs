//
//  IView.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2017 Songurov
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
using Int.Core.Wrappers;

namespace Int.Core.Application.Widget.Contract
{
    public enum RadiusType
    {
        Static,
        Aspect
    }

    public interface IView
    {
        object Controller { get; set; }

        /// <summary>
        ///     Sets the color of the background.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="radiusAspect">
        ///     Aspect relatively to view height. Value from 0.0f to 0.5f. Where 0.0f - square angled
        ///     corner, 0.05f - makes start and end of view completely round.
        /// </param>
        /// <param name="borderColor">Border color.</param>
        /// <param name="borderWidth">Border width.</param>
        /// <param name="type"></param>
        /// <param name="selectedColorView"></param>
        void SetBackgroundColor(string color, float? radiusAspect = null,
            string borderColor = null, float? borderWidth = null,
            RadiusType type = RadiusType.Static, string selectedColorView = "");

        ViewState Visibility { get; set; }

        event EventHandler Click;
        event EventHandler LongClick;
        event SwipeEventHandler Swipe;
        object Tag { get; set; }

        void SetSelectedColor(string colorView);
        void OnTouchView(State state);

        void AddView(IView view);
    }

    public enum ViewState : sbyte
    {
        Gone = -1,
        Invisible = 0,
        Visible = 1
    }

    public enum GestureDirection
    {
        ToLeft,
        ToRight,
        ToUp,
        ToDown
    }

    public delegate void SwipeEventHandler(IView sender, SwipeEventArgs e);

    public class SwipeEventArgs : EventArgs
    {
        public GestureDirection Direction { get; }

        public SwipeEventArgs(GestureDirection direction)
        {
            Direction = direction;
        }
    }
}