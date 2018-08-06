//
//  IText.cs
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

namespace Int.Core.Application.Widget.Contract
{
    public interface IText : IView
    {
        bool IsSecure { get; }

        string Text { get; set; }
        string Hint { get; set; }

        Action<IText> TextChanged { get; set; }

        Action Focus { get; set; }

        void SetTextColor(string textColor);
        void SetHintColor(string textColor);
        void SetFont(string fontType, float size = 16);
        void SetSelectedTextColor(string colorText);
        void SetShadowLayer(float radius, float dx, float dy, string color);
        void SetSecure(InputType transformation = InputType.Text, object nextController = null, Action executeGo = null);
        void SetLinkAndStyle(string substring, string link, string style);
        void SetLinkAndStyle(string[] substring, string[] link, string style);

        void SetCursorColor(string cursorColor);
        void SetNextCursor(IText textNext = null, Action actionGo = null);
    }

    public enum InputType
    {
        Text,
        Password,
        Email
    }
}