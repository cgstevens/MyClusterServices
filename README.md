# MyClusterServices
Akka.Net Cluster Singleton example.

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
  

In this example you will see that I am creating a Singleton Actor to manage a WorkItem.
This WorkItem is to be completed on one of the ServiceWorkers.


  
  
  
