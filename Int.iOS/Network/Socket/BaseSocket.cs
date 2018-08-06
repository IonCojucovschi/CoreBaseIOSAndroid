//
// BaseSocket.cs
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
using System.Diagnostics;
using Int.Core.Network.Socket;
using Quobject.SocketIoClientDotNet.Client;

namespace Int.iOS.Network.Socket
{
    public class ConfigSocket : IConfigSocket
    {
        public ConfigSocket(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
    }

    public interface IConfigSocket
    {
        string Url { get; set; }
    }

    public abstract class BaseSocket : ISocket, ISocketCallBack
    {
        private readonly IConfigSocket _config;

        protected BaseSocket(IConfigSocket config)
        {
            _config = config;
            ConnectIntern();
        }

        protected Quobject.SocketIoClientDotNet.Client.Socket Socket { get; set; }
        protected virtual IO.Options ConnectOptions { get; } = new IO.Options {ForceNew = true, Reconnection = false};

        public bool Connect { get; set; }
        public Exception ErrorMessage { get; set; }

        public virtual bool OnConnect()
        {
            CloseSocket();

            Socket = ConnectOptions != null ? IO.Socket(_config.Url, ConnectOptions) : IO.Socket(_config.Url);

            Subscribe();
            EventConnect();

            return true;
        }

        public virtual bool OnDisconnect()
        {
            ConnectionLost?.Invoke();
            CloseSocket();

            return true;
        }

        public virtual void OnResume()
        {
            OnDisconnect();
            OnConnect();
        }

        public event Action ConnectionLost;
        public event Action ConnectionOnRaise;

        private void ConnectIntern()
        {
            OnConnect();
        }

        protected virtual void Subscribe()
        {
            UnSubscribe();

            Socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_ERROR, EventError);
            Socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_MESSAGE, EventMessage);

            Socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_RECONNECT, EventReconnect);
            Socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_RECONNECTING, EventReconnecting);

            Socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_RECONNECT_ERROR, EventReconnectError);
            Socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_CONNECT_TIMEOUT, EventConnectTimeout);
            Socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_RECONNECT_FAILED, EventReconnectFailed);
            Socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_RECONNECT_ATTEMPT, EventReconnectAttempt);
        }

        protected virtual void UnSubscribe()
        {
        }

        private void CloseSocket()
        {
            if (Socket == null) return;
            Socket?.Close();
            Socket = null;
            GC.Collect();
        }

        #region ISocketCallBack

        public virtual void EventConnect()
        {
            ConnectionOnRaise?.Invoke();
        }

        public virtual void EventDisconnect(object objResponse)
        {
            ConnectionLost?.Invoke();
            ErrorMessage = new Exception(objResponse.ToString());

            Debug.WriteLine("EventDisconnect == " + objResponse);
        }

        public virtual void EventError(object objResponse)
        {
            ErrorMessage = new Exception(objResponse.ToString());

            Debug.WriteLine("EventError == " + objResponse);
        }

        public virtual void EventMessage(object objResponse)
        {
            ErrorMessage = new Exception(objResponse.ToString());

            Debug.WriteLine("EventMessage == " + objResponse);
        }

        public virtual void EventReconnect(object objResponse)
        {
            ErrorMessage = new Exception(objResponse.ToString());

            Debug.WriteLine("EventReconnect == " + objResponse);
        }

        public virtual void EventReconnecting(object objResponse)
        {
            ErrorMessage = new Exception(objResponse.ToString());

            Debug.WriteLine("EventReconnecting == " + objResponse);
        }

        public virtual void EventConnectError(object objResponse)
        {
            ErrorMessage = new Exception(objResponse.ToString());

            Debug.WriteLine("EventConnectError == " + objResponse);
        }

        public virtual void EventReconnectError(object objResponse)
        {
            ErrorMessage = new Exception(objResponse.ToString());

            Debug.WriteLine("EventReconnectError == " + objResponse);
        }

        public virtual void EventConnectTimeout(object objResponse)
        {
            ErrorMessage = new Exception(objResponse.ToString());

            Debug.WriteLine("EventConnectTimeout == " + objResponse);
        }

        public virtual void EventReconnectFailed(object objResponse)
        {
            ErrorMessage = new Exception(objResponse.ToString());

            Debug.WriteLine("EventReconnectFailed == " + objResponse);
        }

        public virtual void EventReconnectAttempt(object objResponse)
        {
            ErrorMessage = new Exception(objResponse.ToString());

            Debug.WriteLine("EventReconnectAttempt == " + objResponse);
        }

        #endregion
    }
}