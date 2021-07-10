using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GameEngine.Utils {

public class IPManager {

    public static string ipv4 => GetIP(ADDRESSFAM.IPv4);
    public static string ipv6 => GetIP(ADDRESSFAM.IPv6);

    public static string GetIP(ADDRESSFAM Addfam)
    {
        //Return null if ADDRESSFAM is Ipv6 but Os does not support it
        if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6) {
            return null;
        }

        var output = "";

        foreach (var item in NetworkInterface.GetAllNetworkInterfaces()) {
        #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            var _type1 = NetworkInterfaceType.Wireless80211;
            var _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) &&
                item.OperationalStatus == OperationalStatus.Up)
        #endif
            {
                foreach (var ip in item.GetIPProperties().UnicastAddresses) {
                    //IPv4
                    if (Addfam == ADDRESSFAM.IPv4) {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork) {
                            output = ip.Address.ToString();
                        }
                    }

                    //IPv6
                    else if (Addfam == ADDRESSFAM.IPv6) {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6) {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }

        return output;
    }

}

public enum ADDRESSFAM { IPv4, IPv6 }

}