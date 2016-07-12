using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Akka.Actor;
using log4net;
using log4net.Appender;
using log4net.Config;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using MyServices.Shared.Helpers;
using Ninject.Modules;
using Topshelf;
using Topshelf.Ninject;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using Shared.Actors;

namespace MyServices.ServiceWorker
{
    class Program
    {
        static bool exitSystem = false;
        public static ActorSystem CLusterSystem { get; set; }
        public static IActorRef ClusterHelper;
        private static PerformanceCollectorModule perfCollectorModule;
        public static string AkkaHostIpAddress { get; set; }

        #region Trap application termination
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");

            //do your cleanup here
            ClusterHelper.Tell(new ClusterHelper.RemoveMember());
            Thread.Sleep(5000); // Give the Remove time to actually remove...

            CLusterSystem.Terminate();
            Thread.Sleep(2000); // Give time for actor system to terminate. 
            Console.WriteLine("Cleanup complete");

            //allow main to run off
            exitSystem = true;

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }
        #endregion

        static int Main(string[] args)
        {
            // Some biolerplate to react to close window event, CTRL-C, kill, etc
            if (Environment.UserInteractive)
            {
                _handler += new EventHandler(Handler);
                SetConsoleCtrlHandler(_handler, true);
            }

            var serviceName = AppSettings.ServiceWorkerName;

            XmlConfigurator.Configure();
            LogManager.GetRepository().GetAppenders().Where(x => x is EventLogAppender).ToList().ForEach(z => ((EventLogAppender)z).ApplicationName = typeof(Program).Namespace);
            GlobalContext.Properties["application"] = typeof(Program).Namespace;
            GlobalContext.Properties["host"] = Dns.GetHostName();
            GlobalContext.Properties["ipaddress"] = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault().ToString();
            
            var topShelfExitCode = (int)HostFactory.Run(hostConfiguratior =>
            {
                hostConfiguratior.UseAssemblyInfoForServiceInfo();
                hostConfiguratior.SetServiceName(serviceName);
                hostConfiguratior.SetDisplayName(serviceName);
                hostConfiguratior.SetDescription("Commissions Messaging - " + serviceName);
                hostConfiguratior.DependsOnEventLog();
                hostConfiguratior.UseLog4Net();
                hostConfiguratior.UseNinject(new ServiceModule());
                hostConfiguratior.RunAsLocalSystem();
                //hostConfiguratior.StartAutomatically();
                hostConfiguratior.Service<WorkerService>((serviceController) =>
                {
                    serviceController.ConstructUsingNinject();
                    serviceController.WhenStarted((service, hostControl) => service.Start(hostControl));
                    serviceController.WhenStopped((service, hostControl) => service.Stop(hostControl));
                });
                hostConfiguratior.EnableServiceRecovery(r =>
                {
                    r.RestartService(1);
                });

            });
            return topShelfExitCode;
        }
    }

    internal class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            
        }
    }
}
