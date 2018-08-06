//
// NetworkClientBase.cs
//
// Author:
//       Songurov <f.songurov@software-dep.net>
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

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Int.Core.Network
{
    public class NetWorkError : EventArgs
    {
        public NetWorkError(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public string Message { get; }
        public bool Success { get; }
    }

    public enum TypeRequest
    {
        Post,
        Get,
        Delete
    }

    public abstract class NetworkClientBase
    {
        protected NetworkClientBase()
        {
            ClientWeb = new HttpClient();
        }

        public abstract HttpResponseMessage ResponseHttp { get; }
        public abstract string ContentPost { get; }
        public abstract Stream ResponseSteam { get; }
        public abstract string ResponseString { get; }
        public abstract MediaTypeWithQualityHeaderValue ContentType { get; }
        public abstract string UrlBase { get; }
        public abstract TypeRequest RequestType { get; }
        public abstract string UrlController { get; }
        protected HttpClient ClientWeb { get; }

        protected static HttpWebRequest GetWebRequest(string formattedUri)
        {
            var serviceUri = new Uri(formattedUri, UriKind.Absolute);
            return (HttpWebRequest) WebRequest.Create(serviceUri);
        }

        public abstract Task RequestUrlAsync(TypeRequest typeRequest);
    }
}