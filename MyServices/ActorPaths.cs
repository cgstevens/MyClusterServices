using Akka.Actor;

namespace MyServices.Shared
{
    /// <summary>
    /// Static helper class used to define paths to fixed-name actors
    /// (helps eliminate errors when using <see cref="ActorSelection"/>)
    /// </summary>
    public static class ActorPaths
    {
        public static readonly string ActorSystem = "myservice";
        public static readonly ActorMetaData JobManagerActor = new ActorMetaData("jobmanager", "/user/jobmanager");
        public static readonly ActorMetaData ClusterHelperActor = new ActorMetaData("clusterhelper", "/user/clusterhelper");
        public static readonly ActorMetaData ActorMonitorActor = new ActorMetaData("actormonitor", "/user/actormonitor");
    }

    /// <summary>
    /// Meta-data class
    /// </summary>
    public class ActorMetaData
    {
        public ActorMetaData(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; private set; }
        public string Path { get; private set; }
    }
}
