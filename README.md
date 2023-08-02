# Redis-Batching

Test adding 1000 keys to redis and benchmarking retrieving those keys in 
  - Batch get
  - Parallel get
  - Serial get

Results 
```
Batch took 11ms
Parallel took 7ms
Serial took 829ms
```
