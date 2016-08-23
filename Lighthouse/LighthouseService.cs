// Copyright 2014-2015 Aaron Stannard, Petabridge LLC
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

using System;
using System.Threading;
using Akka.Actor;
using MyServices.Shared;
using MyServices.Shared.Actors;
using Shared.Actors;
using Topshelf;

namespace Lighthouse
{
    public class LighthouseService : ServiceControl
    {
        private ActorSystem _lighthouseSystem;
        private HostControl _hostControl;
        private static readonly ManualResetEvent asTerminatedEvent = new ManualResetEvent(false);
        
        public bool Start(HostControl hostControl)
        {
            _hostControl = hostControl;
            InitializeCluster();
            return true;
        }
        
        public bool Stop(HostControl hostControl)
        {
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
        public void InitializeCluster()
        {
            _lighthouseSystem = ActorSystemFactory.LaunchClusterManager();
            Program.ClusterSystem = _lighthouseSystem;
            Program.ClusterHelper = Program.ClusterSystem.ActorOf(Props.Create(() => new ClusterHelper()), ActorPaths.ClusterHelperActor.Name);
        }
    }
}
