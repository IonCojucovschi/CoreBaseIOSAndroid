//
// RequestFactory.cs
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
using Int.Core.Extensions;
using Int.Core.Network.Contract;
using Newtonsoft.Json;
using Plugin.Connectivity;

namespace Int.Core.Network.Response
{
    public static class RequestFactory
    {
        private static void Current_ConnectivityChanged(object sender,
            Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
        }

        public static T ExecuteRequest<T>(IRestCallbackClient response) where T : class, IResponseService, new()
        {
            var _errorData = new List<string>();


            try
            {
                CrossConnectivity.Current.ConnectivityChanged -= Current_ConnectivityChanged;
                CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            }
            catch
            {
            }

            var data = new T();

            data.Code = response.StatusCode;

            if (!string.IsNullOrEmpty(response.ErrorMessage) &&
                string.IsNullOrEmpty(response.Content))
            {
                if (!IsOnline())
                {
                    data.IsOnline = false;
                    data.ErrorHttpClient = response.ErrorMessage;
                    data.LocalError = Tuple.Create(ConfigResponse.Instance.ErrorNet, ErrorType.Net);
                    data.UIMessage = data.LocalError.Item1;
                    return data;
                }

                data.IsOnline = true;
                data.Code = response.StatusCode;
                data.ErrorHttpClient = response.ErrorMessage;

                ErrorParse(response, _errorData);
                data.ServerContent = Tuple.Create(response.Content, _errorData);
                data.LocalError = Tuple.Create(ConfigResponse.Instance.ErrorServer, ErrorType.Server);
                data.UIMessage = data.LocalError.Item1;

                return data;
            }

            try
            {
                if (response.StatusCode <= 304 && string.IsNullOrWhiteSpace(response.Content))
                {
                    const string content = "Not Content";

                    ErrorParse(response, _errorData);

                    var results = new T
                    {
                        IsOnline = true,
                        Code = response.StatusCode,
                        ErrorHttpClient = response.ErrorMessage,
                        LocalError = Tuple.Create(content, ErrorType.None),
                        ServerContent = Tuple.Create(content, _errorData)
                    };

                    return results;
                }

                var result = response.Content.FromJson<T>();

                if (result.IsNull()) throw new Exception("Not Content");

                ErrorParse(response, _errorData);


                result.IsOnline = true;
                result.Code = response.StatusCode;
                result.ETag = response.ETag;
                result.ErrorHttpClient = response.ErrorMessage;
                result.ServerContent = Tuple.Create(response.Content, _errorData);
                result.LocalError = Tuple.Create(response.ErrorMessage, ErrorType.None);
                result.UIMessage = result.LocalError.Item1;

                return result;
            }
            catch
            {
                try
                {
                    var result = response.Content.FromJson<BaseMResponse<List<string>>>();
                    ErrorParse(response, _errorData);
                    data.IsOnline = true;
                    data.ErrorHttpClient = response.ErrorMessage;
                    data.ServerContent = Tuple.Create(response.Content, _errorData);
                    data.LocalError = Tuple.Create(ConfigResponse.Instance.ErrorServer, ErrorType.Server);
                    data.UIMessage = data.LocalError.Item1;

                    return data;
                }
                catch
                {
                    data.IsOnline = true;
                    ErrorParse(response, _errorData);

                    data.ErrorHttpClient = response.ErrorMessage;
                    data.ServerContent = Tuple.Create(response.Content, _errorData);
                    data.LocalError = Tuple.Create(ConfigResponse.Instance.ErrorModel, ErrorType.Serialization);
                    data.UIMessage = data.LocalError.Item1;

                    return data;
                }
            }
        }

        private static void ErrorParse(IRestCallbackClient response, List<string> _errorData)
        {
            if (!response.Content.IsNullOrWhiteSpace())
            {
                var a = response.Content.FromJson<List<string>>();

                if (a.IsNull())
                {
                    var simpleString = response.Content.FromJson<ErrorWraper>();
                    if (!simpleString.IsNull() && !simpleString.Result)
                        _errorData.Add(simpleString.Error ?? simpleString.Data ?? response.Content);
                }
                else
                {
                    _errorData.AddRange(a);
                }
            }
        }

        public static bool IsOnline()
        {
            try
            {
                return CrossConnectivity.Current.IsConnected;
            }
            catch
            {
                return false;
            }
        }
    }

    public class ErrorWraper
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }
    }
}