//
// Utility.cs
//
// Author:
//       Songurov Fiodor <f.songurov@software-dep.net>
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
using System.Linq;
using Foundation;

namespace Int.iOS.Helpers
{
    public static class Utility
    {
        public static string PersonalFolder => Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public static string CurrentVersionBuild =>
            NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();

        public static string CurrentVersion =>
            NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleShortVersionString")].ToString();

        public static bool ItOrNot =>
            NSLocale.PreferredLanguages.FirstOrDefault()?.ToLowerInvariant().Substring(0, 2) == "it";
    }
}