using System;
using System.Threading;
using Akka.Actor;
using Akka.Cluster.Tools.Singleton;
using log4net;
using MyServices.Shared;
using MyServices.Shared.Actors;
using MyServices.Shared.Helpers;
using Shared.Actors;
using Topshelf;

namespace MyServices.ServiceWorker2
{
    internal class WorkerService : ServiceControl
    {
        public HostControl _hostControl;

        public bool Start(HostControl hostControl)
        {
            _hostControl = hostControl;
            InitializeCommissionsSystem();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            //do your cleanup here
            Program.ClusterHelper.Tell(new ClusterHelper.RemoveMember());
            Thread.Sleep(5000); // Give the Remove time to actually remove...

            Program.ClusterSystem.Terminate();
            Thread.Sleep(2000); // Give time for actor system to terminate. 
            Console.WriteLine("Cleanup complete");
            return true;
        }

        public void InitializeCommissionsSystem()
        {
            Program.ClusterSystem = ActorSystemFactory.LaunchClusterManager();
            
            GlobalContext.Properties["ipaddress"] = AppSettings.GetIpAddressFromConfig();

            Program.ClusterHelper = Program.ClusterSystem.ActorOf(Props.Create(() => new ClusterHelper()), ActorPaths.ClusterHelperActor.Name);

            Program.ClusterSystem.ActorOf(ClusterSingletonManager.Props(
                singletonProps: Props.Create(() => new JobManager()),         // Props used to create actor singleton
                terminationMessage: PoisonPill.Instance,                  // message used to stop actor gracefully
                settings: ClusterSingletonManagerSettings.Create(Program.ClusterSystem).WithRole("worker")),// cluster singleton manager settings
                name: "jobmanager");
        }

    }
}
