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
