//
//  SettingWindow.cs
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

using Int.Core.Application.Window.Contract;

namespace Int.Core.Application.Window
{
    public class SettingWindow : ISettingsWindow
    {
        public bool BlockWindow { get; set; } = true;

        public string ColorWindowView { get; set; } = "#00000000";
        public string ColorContentView { get; set; } = "#eef3f0";
        public string ColorText { get; set; } = "#000000";

        public float SizeFontTextTop { get; set; } = 16;
        public float SizeFontTextCentre { get; set; } = 16;
        public float SizeFontTextBottom { get; set; } = 16;

        public string ColorCentreView { get; set; } = "#00000000";
        public string ColorCentreText { get; set; } = "#000000";

        public bool Round { get; set; } = true;
        public bool Border { get; set; } = false;
    }
}