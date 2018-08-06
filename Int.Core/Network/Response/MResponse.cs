//
//  MResponse.cs
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
using System.Collections.Generic;
using Int.Core.Application.Exception;
using Int.Core.Extensions;
using Newtonsoft.Json;

namespace Int.Core.Network.Response
{
    public class BaseMResponse<TModel> : IResponseService
    {
        [JsonProperty("message")]
        public virtual string Message { get; set; }

        [JsonProperty("data")]
        public virtual TModel Data { get; set; }

        [JsonProperty("status_code")]
        public int Code { get; set; }

        public string ErrorHttpClient { get; set; }

        public bool IsOnline { get; set; }

        public Tuple<string, ErrorType> LocalError { get; set; }

        public Tuple<string, List<string>> ServerContent { get; set; }

        public string UIMessage { get; set; }

        public string ETag { get; set; }

        public virtual void OnResponse(Action success, Action<ExceptionApp> responseString = null)
        {
            if (!IsOnline)
            {
                responseString?.Invoke(new ExceptionApp(LocalError?.Item1));
            }
            else if (Code >= 400 && (LocalError?.Item2 != ErrorType.None || !Message.IsNullOrWhiteSpace()))
            {
                if (!Message.IsNullOrWhiteSpace())
                {
                    UIMessage = Message;
                }
                else
                {
                    if (Code == 0 || Data.IsNull())
                        UIMessage = "Crash SERVER (PHP) - ConvertModel";
                    else if (Code >= 500)
                        UIMessage = "Crash SERVER (PHP) - 500";
                    else
                        UIMessage = "Error(s) SERVER (PHP)";
                }

                LocalError = Tuple.Create(UIMessage, ErrorType.Server);
                responseString?.Invoke(new ExceptionApp(UIMessage));
            }
            else if (IsOnline)
            {
                if (Data.IsNull())
                    UIMessage = "Crash SERVER (PHP) - ConvertModel";

                success?.Invoke();
            }
        }
    }
}