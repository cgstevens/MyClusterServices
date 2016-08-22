using System;
using Akka.Actor;
using Akka.Event;
using MyServices.Shared.Messages;

namespace MyServices.Shared.Actors
{
    public class JobTasker : ReceiveActor
    {
        private readonly ILoggingAdapter _logger;
        private IActorRef _workerRouter;
        private ICancelable _monkeyTeller;
        private int _counter;

        public JobTasker()
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
            _logger.Info("JobTasker is becoming ready.");
            _monkeyTeller = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(1), Self, new MonkeyDo(), Self);

            _counter = 0;
        }


        public void Ready()
        {
            Receive<MonkeyDo>(ic =>
            {
                _counter++;
                _logger.Info("JobTasker tell JobManager something {0}", _counter);
            });
            
            ReceiveAny(task =>
            {
                _logger.Error(" [x] Oh Snap! JobTasker.Ready.ReceiveAny: \r\n{0}", task);
            });
        }
        
    }
}