using System;
using Akka.Actor;
using Akka.Event;
using Akka.Monitoring;
using MyServices.Shared.Messages;

namespace MyServices.Shared.Actors
{
    public class JobManager : ReceiveActor
    {
        private readonly ILoggingAdapter _logger;
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

            _monkeyTeller = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2), Self, new MonkeyDo(), Self);

            _counter = 0;
        }


        public void Ready()
        {
            Receive<MonkeyDo>(ic =>
            {
                _counter++;
                _logger.Info("Monkey Can Count : {0}", _counter);
            });
            
            ReceiveAny(task =>
            {
                _logger.Error(" [x] Oh Snap! JobTasker.Ready.ReceiveAny: \r\n{0}", task);
            });
        }
        
    }
}