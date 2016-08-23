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

namespace MyServices.ServiceWorker
{
    internal class WorkerService : ServiceControl
    {
        public HostControl _hostControl;
        private static readonly ManualResetEvent asTerminatedEvent = new ManualResetEvent(false);

        public bool Start(HostControl hostControl)
        {
            _hostControl = hostControl;
            InitializeClusterSystem();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            //do your cleanup here
            Program.ClusterHelper.Tell(new ClusterHelper.RemoveMember());

            var cluster = Akka.Cluster.Cluster.Get(Program.ClusterSystem);
            cluster.RegisterOnMemberRemoved(() => MemberRemoved(Program.ClusterSystem));
            asTerminatedEvent.WaitOne();

            return true;
        }

        private async void MemberRemoved(ActorSystem actorSystem)
        {
            await actorSystem.Terminate();
            asTerminatedEvent.Set();
            Console.WriteLine("Member Removed");
        }

        public void InitializeClusterSystem()
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
