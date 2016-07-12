using System;
using System.Configuration;
using Akka.Actor;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using MyServices.Shared.Helpers;

namespace MyServices.Shared.Actors
{
    public static class ActorSystemFactory
    {
        public static ActorSystem LaunchClusterManager()
        {
            var serverPort = AppSettings.ServerPort;
            string serverIp = StaticMethods.GetHostIpAddress();

            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
            var akkaConfig = section.AkkaConfig;

            var injectTcpInfo = string.Format(@"akka.remote.helios.tcp.public-hostname = {0}{1} akka.remote.helios.tcp.hostname = {2}{3} akka.remote.helios.tcp.port = {4}{5}", serverIp, Environment.NewLine, serverIp, Environment.NewLine, serverPort, Environment.NewLine);
            
            var finalConfig = ConfigurationFactory.ParseString(
                injectTcpInfo)
                .WithFallback(akkaConfig);

            return ActorSystem.Create(ActorPaths.ActorSystem, finalConfig);
        }

    }
}
