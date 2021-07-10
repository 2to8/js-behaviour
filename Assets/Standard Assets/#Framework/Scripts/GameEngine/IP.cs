using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;

namespace GameEngine
{
    public class IP
    {
        public enum ADDRESSFAM { IPv4, IPv6 }

        public static string GetIP() => GetIP(ADDRESSFAM.IPv4);

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <param name="Addfam">要获取的IP类型</param>
        /// <returns></returns>
        public static string GetIP(ADDRESSFAM Addfam)
        {
            if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6) {
                return null;
            }
            var output = "";

            var ipv4 = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(t => t.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                    && t.OperationalStatus == OperationalStatus.Up
                    && t.GetIPProperties()
                        .UnicastAddresses.FirstOrDefault(i => i.Address.AddressFamily == AddressFamily.InterNetwork)
                    != null);

            if (ipv4 != null) {
                return ipv4.GetIPProperties()
                    .UnicastAddresses.First(t => t.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Address.ToString();
            }

            foreach (var item in NetworkInterface.GetAllNetworkInterfaces()) {
            #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                var _type1 = NetworkInterfaceType.Wireless80211;
                var _type2 = NetworkInterfaceType.Ethernet;

                if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2)
                    && item.OperationalStatus == OperationalStatus.Up)
            #endif
                {
                    foreach (var ip in item.GetIPProperties().UnicastAddresses) {
                        //IPv4
                        if (Addfam == ADDRESSFAM.IPv4) {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork) {
                                output = ip.Address.ToString();
                                Debug.Log("IP:" + output);

                                return output;
                            }
                        }

                        //IPv6
                        else if (Addfam == ADDRESSFAM.IPv6) {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6) {
                                output = ip.Address.ToString();
                                Debug.Log("IP:" + output);

                                return output;
                            }
                        }
                    }
                }
            }

            return output;
        }
    }
}