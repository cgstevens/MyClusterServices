using System;
using Akka.Actor;
using Akka.Event;
using Akka.Monitoring;

namespace MyServices.Shared.Actors
{
    public class ActorMonitor : ReceiveActor
    {
        private readonly EventStream _eventStream = Context.System.EventStream;
        private readonly ILoggingAdapter _logger;

        public ActorMonitor()
        {
            _logger = Context.GetLogger();
            BecomeReady();
        }

        protected override void PostRestart(Exception reason)
        {
        }

        protected override void PreStart()
        {
            _eventStream.Subscribe(Self, typeof(DeadLetter));
        }

        protected override void PostStop()
        {
            _eventStream.Unsubscribe(Self);
        }

        private void BecomeReady()
        {
            Become(Ready);
        }


        public void Ready()
        {
            Receive<DeadLetter>(ic =>
            {
                var message = ic.Message;

                if (message.GetType().Name == "DeathWatchNotification")
                {
                    Context.IncrementCounter("DeathWatchNotification");
                }
                else if(message.GetType().Name == "Terminate")
                {
                    Context.IncrementCounter("Terminate");
                }
                else
                {
                    Context.IncrementCounter("DeadLetter");
                    _logger.Warning("DeadLetter:{0}:{1}:{2}", ic.Message, ic.Sender?.Path.ToSerializationFormat(), ic.Recipient.Path.ToSerializationFormat());
                }
                
            });
            
            ReceiveAny(task =>
            {
                _logger.Error(" [x] Oh Snap! JobTasker.Ready.ReceiveAny: \r\n{0}", task);
            });
        }
        
    }
}