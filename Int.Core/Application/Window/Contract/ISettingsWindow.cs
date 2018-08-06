﻿//
//  ISettingsWindow.cs
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

namespace Int.Core.Application.Window.Contract
{
    public interface ISettingsWindow
    {
        bool BlockWindow { get; set; }

        bool Round { get; set; }
        bool Border { get; set; }

        string ColorWindowView { get; set; }
        string ColorContentView { get; set; }
        string ColorCentreView { get; set; }

        string ColorText { get; set; }
        string ColorCentreText { get; set; }

        float SizeFontTextTop { get; set; }
        float SizeFontTextCentre { get; set; }
        float SizeFontTextBottom { get; set; }
    }
}