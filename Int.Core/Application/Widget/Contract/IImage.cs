//
//  IImage.cs
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

using System.IO;

namespace Int.Core.Application.Widget.Contract
{
    public interface IImage : IView
    {
        void SetImageFromUrl(string url, float quality = 1.0f);
        void SetImageFromResource(string imageName);
        void SetImageFromStream(Stream imageStream);
        void SetSelected(string selectedColorImage);
        void SetImageColorFilter(string color);
        void SetRoundImage();
    }
}