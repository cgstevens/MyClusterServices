using Akka.Actor;

namespace MyServices.Shared.Messages
{
    public class FoundWorker
    {
        public FoundWorker(int workItem, IActorRef workerRef)
        {
            WorkItem = workItem;
            WorkerRef = workerRef;
        }
        public int WorkItem { get; private set; }
        public IActorRef WorkerRef { get; private set; }
    }
}
