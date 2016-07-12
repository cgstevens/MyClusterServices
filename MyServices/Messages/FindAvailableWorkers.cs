namespace MyServices.Shared.Messages
{
    public class FindAvailableWorkers
    {
        public FindAvailableWorkers(int nodeCount)
        {
            NodeCount = nodeCount;
        }

        public int NodeCount { get; private set; }
    }
}
