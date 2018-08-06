//
//  Network.cs
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
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Int.Core.Device.Contract;

namespace Int.iOS.Device
{
    public class Network : DeviceBase<Network>, IInfoDevice
    {
        public string IpAdressString { get; set; }
        public string MacAdress { get; set; }
        public long IpAdress { get; set; }

        public IInfoDevice GetInfo()
        {
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    MacAdress = BitConverter.ToString(netInterface.GetPhysicalAddress().GetAddressBytes());

            try
            {
                IpAdress = Convert.ToInt64(NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(x => x.Name.Equals("en0"))
                    .Select(n => n.GetIPProperties().UnicastAddresses.First().Address)
                    .Where(x => x.AddressFamily == AddressFamily.InterNetwork));
            }
            catch
            {
            }

            return this;
        }
    }
}