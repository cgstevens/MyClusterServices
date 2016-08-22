using System;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Routing;
using Akka.Event;
using Akka.Monitoring;
using Akka.Routing;
using MyServices.Shared.Messages;
using Akka.Cluster.Tools.Singleton;

namespace MyServices.Shared.Actors
{
    public class JobManager : ReceiveActor, IWithUnboundedStash
    {
        private readonly ILoggingAdapter _logger;
        private IActorRef _workerRouter;
        private ICancelable _monkeyTeller;
        private int _counter;

        public JobManager()
        {
            _logger = Context.GetLogger();
            BecomeReady();
        }

        protected override void PostRestart(Exception reason)
        {
            base.PostRestart(reason);
        }

        protected override void PreStart()
        {
            base.PreStart();
        }

        protected override void PostStop()
        {
            base.PostStop();
        }

        private void BecomeReady()
        {
            Become(Ready);
            _logger.Info("JobMaster is becoming ready.");

            var proxy = Context.ActorOf(ClusterSingletonProxy.Props(
                singletonManagerPath: "/user/jobmanager",
                settings: ClusterSingletonProxySettings.Create(Context.System).WithRole("worker")),
                name: "managerProxy");

            _workerRouter = Context.ActorOf(new ClusterRouterPool(
                local: new RandomPool(1), 
                settings: new ClusterRouterPoolSettings(30, 1, true, "worker")
                ).Props(Props.Create(() => new Worker(proxy))));

            _monkeyTeller = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(1), Self, new MonkeyDo(), Self);

            Context.ActorOf(Props.Create(() => new JobTasker()), "jobtasker");

            _counter = 0;
        }


        public void Ready()
        {
            Receive<MonkeyDo>(ic =>
            {
                _workerRouter.Ask<Routees>(new GetRoutees()).ContinueWith(tr =>
                {
                    if (tr.IsFaulted)
                    {
                        _logger.Error(tr.Exception, "WorkerRouter was Faulted ");
                        return new FindAvailableWorkers(0);
                    }
                    if (tr.IsCanceled)
                    {
                        _logger.Error(tr.Exception, "WorkerRouter was Canceled ");
                        return new FindAvailableWorkers(0);
                    }

                    return new FindAvailableWorkers(tr.Result.Members.Count());
                }, TaskContinuationOptions.AttachedToParent & TaskContinuationOptions.ExecuteSynchronously).PipeTo(Self);
                Become(SearchingForJob);
            });
            
            ReceiveAny(task =>
            {
                _logger.Error(" [x] Oh Snap! JobTasker.Ready.ReceiveAny: \r\n{0}", task);
            });
        }

        public void SearchingForJob()
        {

            Receive<MonkeyDo>(ic =>
            {
                Stash.Stash();
            });

            Receive<FindAvailableWorkers>(ic =>
            {
                if (ic.NodeCount == 0)
                {
                    // We dind't find any routees
                    Stash.UnstashAll();
                    Become(Ready);
                    _logger.Warning("Did not find any available workers");
                    return;
                }

                _counter++;
                _logger.Info("Monkey Create Work Item : {0}", _counter);
                _workerRouter.Tell(new MonkeyDoWork(_counter));
            });

            Receive<FoundWorker>(ic =>
            {
                _logger.Info("Monkey Manager Found Worker to complete work item: {0}, {1}", ic.WorkItem, ic.WorkerRef.ToString());
                Stash.UnstashAll();
                Become(Ready);
            });



            ReceiveAny(task =>
            {
                _logger.Error(" [x] Oh Snap! JobTasker.Ready.ReceiveAny: \r\n{0}", task);
            });
        }

        public IStash Stash { get; set; }
    }
}