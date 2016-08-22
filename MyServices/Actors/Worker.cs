//-----------------------------------------------------------------------
// <copyright file="Worker.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System.Threading;
using Akka.Actor;
using Akka.Event;
using MyServices.Shared.Messages;

namespace MyServices.Shared.Actors
{
    public class Worker : ReceiveActor
    {
        private readonly ILoggingAdapter _logger;
        private readonly IActorRef _jobManagerProxyRef;

        public Worker(IActorRef jobManagerProxyRef)
        {
            _logger = Context.GetLogger();
            _jobManagerProxyRef = jobManagerProxyRef;

            Receive<MonkeyDoWork>(work =>
            {
                _logger.Info("Monkey Doing Work : {0}", work.WorkItem);

                _jobManagerProxyRef.Tell(new FoundWorker(work.WorkItem, Self));


            });
        }
    }
}