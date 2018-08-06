//
//  Singleton.cs
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
using System.Diagnostics;
using Com.OneSignal;
using Com.OneSignal.Abstractions;

namespace Int.Core.Network.Singleton
{
    public abstract class Singleton<T> : object, ISingleton<T> where T : new()
    {
        private static readonly Lazy<T> Service = new Lazy<T>(() => new T());

        public static T Instance => Service.Value;
    }

    public class UtilityNotification : Singleton<UtilityNotification>
    {
        public UtilityNotification Configuration(Action page, string key)
        {
            OneSignal.Current.RegisterForPushNotifications();

#if DEBUG
            OneSignal.Current.SetLogLevel(LOG_LEVEL.VERBOSE, LOG_LEVEL.WARN);
#endif

            OneSignal.Current.StartInit(key)
                .Settings(new Dictionary<string, bool>
                {
                    {IOSSettings.kOSSettingsKeyInAppLaunchURL, true}
                })
                .InFocusDisplaying(OSInFocusDisplayOption.Notification)
                .HandleNotificationOpened(result => { page?.Invoke(); })
                .HandleNotificationReceived(notification => { page?.Invoke(); })
                .EndInit();

            OneSignal.Current.IdsAvailable((playerId, pushToken) =>
            {
                Debug.WriteLine("OneSignal.Current.IdsAvailable:D playerID: {0}, pushToken: {1}", playerId,
                    pushToken);
            });


            return this;
        }
    }
}