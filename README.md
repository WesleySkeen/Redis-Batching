# Redis-Batching

While doing a POC to re order some redis calls, I wanted to test out if batching might yield some promising results. I am looking to move 4 redis calls up stream and call them together. Then passing the results down stream. 

Benchmarking retrieving keys with
  - Batch get
  - Parallel get
  - Serial get

Results for 1,000 keys
```
Batch took 11ms
Parallel took 7ms
Serial took 829ms
```

So looking at the above results. Parallel look up seems to be the faster operation. Atleast in this simple scenario. I will try both in a production envirorment with real data and see what results I get. That said, batching could generally be better for optimising connection usage to the redis instance.

I got curious and decided to try look up many more keys

Results for 900,000 keys
```
Batch took 2081ms
Parallel took 7372ms
Serial took 829200ms
Calling batch
```

This time batching came out on top. So depending on the server specs and the use case, both batch and parallel could be options
