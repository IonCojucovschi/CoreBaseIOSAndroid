//
// Network.cs
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

using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.Telephony;
using Java.Util;
using Java.Lang;

namespace Int.Droid.Device
{
    public class Network : DeviceBase
    {
        private static WifiManager _telephonyManagerWifi;

        private static ConnectivityManager _connectivityManager;

        public static WifiManager WifiManager => _telephonyManagerWifi ??
                                                 (_telephonyManagerWifi = GetManager<WifiManager>(Context.WifiService));

        public int CheckSpeedFromWifi => WifiManager.ConnectionInfo.LinkSpeed;

        public static ConnectivityManager ConnectivityManagers => _connectivityManager ??
                                                                  (_connectivityManager =
                                                                      GetManager<ConnectivityManager>(
                                                                          Context.ConnectivityService));



        public static bool IsConnected
        {
            get
            {
                var info = ConnectivityManagers.ActiveNetworkInfo;
                return info != null && info.IsConnected;
            }
        }

        public static bool IsConnectedWifi
        {
            get
            {
                var info = ConnectivityManagers.ActiveNetworkInfo;
                return info != null && info.IsConnected && info.Type == ConnectivityType.Wifi;
            }
        }

        public static bool IsConnectedMobile
        {
            get
            {
                var info = ConnectivityManagers.ActiveNetworkInfo;
                return info != null && info.IsConnected && info.Type == ConnectivityType.Mobile;
            }
        }

        public static string GprsStatus
        {
            get
            {
                if (!IsConnected) return "NotConnected";
                if (IsConnectedWifi)
                    return "Wifi";

                if (!IsConnectedMobile) return "NotConnected";
                var info = ConnectivityManagers.ActiveNetworkInfo;
                return info.SubtypeName;
            }
        }
    }
}