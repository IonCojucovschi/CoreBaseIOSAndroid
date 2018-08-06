//
// NetworkClient.cs
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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Int.Core.Network
{
    public enum TypeWebClient
    {
        HttpClient,
        WebRequest
    }

    public class NetworkClient : NetworkClientBase, INetworkClient
    {
        private async Task CreatorRequestAsync(TypeRequest typeRequest)
        {
            _requestType = typeRequest;

            try
            {
                switch (typeRequest)
                {
                    case TypeRequest.Post:

                        using (
                            _response =
                                await
                                    ClientWeb.PostAsync(new Uri(ClientWeb.BaseAddress, _urlController),
                                        new StringContent(_contentPost, Encoding.UTF8, _contentType.MediaType)))
                        {
                            if (_response.IsSuccessStatusCode ||
                                _response.StatusCode == HttpStatusCode.Forbidden ||
                                _response.StatusCode == HttpStatusCode.BadRequest ||
                                _response.StatusCode == HttpStatusCode.InternalServerError)
                                using (var content = _response.Content)
                                {
                                    _responseStream = await content.ReadAsStreamAsync();

                                    using (var srs = new StreamReader(_responseStream))
                                    {
                                        _responseString = srs.ReadToEnd();
                                    }
                                }
                        }

                        break;

                    case TypeRequest.Get:

                        using (_response = await ClientWeb.GetAsync(new Uri(ClientWeb.BaseAddress, _urlController)))
                        {
                            if (_response.IsSuccessStatusCode ||
                                _response.StatusCode == HttpStatusCode.Forbidden ||
                                _response.StatusCode == HttpStatusCode.BadRequest)
                                using (var content = _response.Content)
                                {
                                    _responseStream = await content.ReadAsStreamAsync();

                                    using (var srs = new StreamReader(_responseStream))
                                    {
                                        _responseString = srs.ReadToEnd();
                                    }
                                }

                            ClientWeb.Dispose();
                        }

                        break;
                    case TypeRequest.Delete:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(typeRequest), typeRequest, null);
                }
            }
            catch
            {
                throw;
            }
        }

        #region INetworkClient implementation

        public NetworkClient SetUrl(string url)
        {
            _urlController = url;
            return this;
        }

        public NetworkClient SetBaseUrl(string url)
        {
            _urlBase = url;
            ClientWeb.BaseAddress = new Uri(_urlBase);
            return this;
        }

        public NetworkClient SetContent(string content)
        {
            _contentPost = content;
            return this;
        }

        public NetworkClient SetHeader(Dictionary<string, string> value)
        {
            foreach (var item in value)
                ClientWeb.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
            return this;
        }

        public NetworkClient Settings(INetworkSettings settings)
        {
            _urlBase = settings.UrlBase;
            _urlController = settings.UrlController;
            ClientWeb.DefaultRequestHeaders.Accept.Add(settings.ContentType);
            _contentType = settings.ContentType;
            ClientWeb.Timeout = TimeSpan.FromSeconds(3000);
            ClientWeb.BaseAddress = new Uri(_urlBase);

            if (settings.HeaderColletion != null && settings.HeaderColletion.Count != 0)
                SetHeader(settings.HeaderColletion);

            return this;
        }

        public async Task<NetworkClient> ExecuteAsync()
        {
            if (!string.IsNullOrEmpty(_contentPost))
                await CreatorRequestAsync(TypeRequest.Post);
            else
                await CreatorRequestAsync(TypeRequest.Get);

            return this;
        }

        #endregion

        #region implemented abstract members of NetworkClientBase

        public override async Task RequestUrlAsync(TypeRequest typeRequest)
        {
            await CreatorRequestAsync(typeRequest);
        }

        private MediaTypeWithQualityHeaderValue _contentType =
            new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded");

        public override MediaTypeWithQualityHeaderValue ContentType => _contentType;

        private HttpResponseMessage _response;

        public override HttpResponseMessage ResponseHttp => _response;

        private string _contentPost;

        public override string ContentPost => _contentPost;

        private Stream _responseStream;
        public override Stream ResponseSteam => _responseStream;

        private string _responseString;
        public override string ResponseString => _responseString;

        private string _urlBase;

        public override string UrlBase => _urlBase;

        private TypeRequest _requestType;

        public override TypeRequest RequestType => _requestType;

        private string _urlController;

        public override string UrlController => _urlController;

        #endregion
    }
}