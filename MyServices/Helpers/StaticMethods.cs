using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace MyServices.Shared.Helpers
{
    public static class StaticMethods
    {
        public static string UpdateSubscriberPath(string subscriberKeyPath)
        {
            var subscriber = subscriberKeyPath;
            if (!subscriber.Contains("@"))
            {
                subscriber = subscriber.Replace("akka://" + ActorPaths.ActorSystem, "akka.tcp://" + ActorPaths.ActorSystem + "@" + AppSettings.GetIpAddressFromConfig());
            }
            return subscriber;
        }

        public static string GetHostIpAddress()
        {
            var validIpAddresses = new List<string>(AppSettings.ServerIpList.Split(new char[] { ';' }));
            string serverIp;

            if (validIpAddresses.Contains("127.0.0.1"))
            {
                serverIp = "127.0.0.1";
            }
            else
            {
                serverIp =
                    Dns.GetHostEntry(Dns.GetHostName())
                        .AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork && validIpAddresses.Contains(x.ToString())).ToString();
            }
            return serverIp;
        }
        
    }
}
