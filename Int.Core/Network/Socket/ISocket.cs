//
// ISocket.cs
//
// Author:
//       Songurov <songurov@gmail.com>
//
// Copyright (c) 2017 Songurov
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

namespace Int.Core.Network.Socket
{
    public interface ISocket
    {
        bool Connect { get; set; }

        Exception ErrorMessage { get; set; }
        bool OnConnect();
        bool OnDisconnect();
        void OnResume();
    }

    public interface ISocketCallBack
    {
        void EventDisconnect(object objResponse);
        void EventError(object objResponse);
        void EventMessage(object objResponse);
        void EventReconnect(object objResponse);
        void EventReconnecting(object objResponse);
        void EventConnectError(object objResponse);
        void EventReconnectError(object objResponse);
        void EventConnectTimeout(object objResponse);
        void EventReconnectFailed(object objResponse);
        void EventReconnectAttempt(object objResponse);
    }
}