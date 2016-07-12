namespace MyServices.Shared.Messages
{
    public class MonkeyDo
    {
    }

    public class MonkeyDoWork
    {
        public MonkeyDoWork(int workItem)
        {
            WorkItem = workItem;
        }

        public int WorkItem { get; private set; }
    }
}
