//
// IGoogleAuthenticatorDelegate.cs
//
// Author:
//       Songurov <songurov@gmail.com>
//
// Copyright (c) 2018 Songurov
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
using Int.Core.Helpers.Google.Authentication.Model;

namespace Int.Core.Helpers.Google.Authentication.Contract
{
    public interface IGoogleAuthenticatorDelegate
    {
        void OnAuthenticationCompleted(GoogleOAuthToken token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
    }

    public interface IGoogleAuthenticationService
    {
        void Init(Dictionary<string, object> config);
        void SetAuthenticationCallbacks(IGoogleAuthenticatorDelegate callbacks);
        void SignIn();
        void SignOut();
        string GetIdToken();
        string GetAccountName();
        bool IsConnected();
        void Disconnect();

        bool HandleURL(object urlObject, string sourceApplication, object annotation);
    }
}
