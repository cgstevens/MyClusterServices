# Akka.Net Cluster Singleton example.

The solution contains 4 services; 2 Workers and 2 Lighthouses. 
You should be able to pull down the solution and run it.
The worker will demonstrate how the single creates a worker on the other member and report its status back to the manager using the singleton proxy. 

In a highly-available application, it is occasionally necessary to have some processing performed in isolation within a cluster, while still allowing for processing to continue should the designated worker node fail. Cluster Singleton pattern makes this very easy.
For example, consider a system that processes file from a clustered file system or aggregating and calculating invoices or inventory. 
In my case I need to calculate thhe invoices for a single market and only want that market running once within the cluster hence only one invoice being processed. 

****It is important that any given invoice is processed exactly once.

Atomic management is difficult due to the lag time of the system whether it is a file system or database.
Any system that relies solely on renaming or locking to designate which node is processing that item will most likely run into contention, leading to unreliable results. An alternative approach is to ensure only one node is actively seeking work. In this scenario no contention can possibly occur. 
I proven that when having 2 services running trying to find next available item to process will eventually deadlock. 

If you are wondering why I have two lighthouses?  In every project I just have it standard to make thing seem more realisitic.
  You should in fact always run 2+ in Production.  If you only run one and the lighthouse goes down and comes back up then the worker node will now become split brain and in this example having 2 singleton breaks the whole design.
  
Get the facts from the source...
  http://getakka.net/docs/clustering/cluster-singleton
  
##Example in depth
In this example you will see that I am creating a Singleton Actor to manage a WorkItem.
This WorkItem is to be completed on one of the ServiceWorkers.

First we need to configure the Singleton Actor and its Proxy Actor.  The proxy actor will automatically keep track of the actor's current location, update it if necessary and buffer all messages during the handover process. 
Never use the Singleton IActorRef outside of the Singleton.  You will notice that it will work until your Singleton fails and switches to another node.  When this occurs the Actor with the Singleton IActorRef will now be an invalid address.

###Setup the Singleton 
  You will want to specify the role when you want to limit actor's singleton context to a nodes having particular role.
  In this case if it is not specified your singleton could end up on the lighthouse member.
  
```
  singleton {
    singleton-name = "jobmanager"
    role = ""
    hand-over-retry-interval = 1s
  }
```

###Setup the Singleton Proxy
```
singleton-proxy {
    singleton-name = "jobmanager"
    role = ""
    singleton-identification-interval = 2s
    buffer-size = 100
  }
```

###Creating the Singleton Actor
Here you can see I am creating the Singleton on only members with a role of "worker".
Remove the .WithRole to see the jobmanager assign work to the the Lighthouse members as well.
```
Program.ClusterSystem.ActorOf(ClusterSingletonManager.Props(
                singletonProps: Props.Create(() => new JobManager()),         // Props used to create actor singleton
                terminationMessage: PoisonPill.Instance,                  // message used to stop actor gracefully
                settings: ClusterSingletonManagerSettings.Create(Program.ClusterSystem).WithRole("worker")),// cluster singleton manager settings
                name: "jobmanager");
```

###Creating the Proxy
  Here I am creating my proxy which will track the Singleton on only member with the role of "worker".
  You could have just the Singleton live on the "worker" member and have the proxy on all members of your cluster.
  
  **NOTE: I need to verify that this is even correct. Perhaps you shouldn't pass the actor at all.  The proxy should be in the JobWorker.**

```
var proxy = Context.ActorOf(ClusterSingletonProxy.Props(
    singletonManagerPath: "/user/jobmanager",
    settings: ClusterSingletonProxySettings.Create(Context.System).WithRole("worker")),
    name: "managerProxy");

  _workerRouter = Context.ActorOf(new ClusterRouterPool(
    local: new RandomPool(1), 
    settings: new ClusterRouterPoolSettings(30, 1, true, "worker")
    ).Props(Props.Create(() => new Worker(proxy))));
```
