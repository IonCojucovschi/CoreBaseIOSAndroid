//
//  ExceptionApp.cs
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

using Int.Core.Extensions;
using Int.Core.Network;

namespace Int.Core.Application.Exception
{
    public sealed class ExceptionApp : System.Exception
    {
        private readonly string _message;

        public ExceptionApp(string message, IResponseService response = null) : base(message.IsNullOrWhiteSpace()
            ? response?.ServerContent?.Item1
            : message)
        {
            _message = message;
            Response = response;
            Source = response?.ServerContent?.Item1;
        }

        public IResponseService Response { get; }

        public override string Message => _message.IsNullOrWhiteSpace() ? Response?.ServerContent?.Item1 : _message;
    }
}