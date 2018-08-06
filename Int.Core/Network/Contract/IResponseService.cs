//
// IResponseService.cs
//
// Author:
//       Sogurov Fiodor <f.songurov@software-dep.net>
//
// Copyright (c) 2016 Songurov
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
using System.Collections.Generic;
using Int.Core.Network.Contract;

namespace Int.Core.Network
{
    public interface IResponseService : IResponseFilter
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="T:Int.Core.Network.IResponseService" /> is online.
        /// </summary>
        /// <value><c>true</c> if is online; otherwise, <c>false</c>.</value>
        bool IsOnline { get; set; }

        /// <summary>
        ///     Gets or sets the code HTTPCLIENT.
        /// </summary>
        /// <value>The code.</value>
        int Code { get; set; }

        /// <summary>
        ///     Gets or sets the local error.
        /// </summary>
        /// <value>The local error.</value>
        Tuple<string, ErrorType> LocalError { get; set; }

        /// <summary>
        ///     1.Content Server
        ///     2.Errors Server
        /// </summary>
        /// <value>The content of the server.</value>
        Tuple<string, List<string>> ServerContent { get; set; }

        string ErrorHttpClient { get; set; }

        /// <summary>
        ///     Show Message Views
        /// </summary>
        /// <value>The UI Message.</value>
        string UIMessage { get; set; }

        string ETag { get; set; }
    }

    public enum ErrorType
    {
        None,
        Net,
        Server,
        Serialization
    }
}