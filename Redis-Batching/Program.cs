// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using StackExchange.Redis;

class Program
{
    public static async Task Main(string[] args)
    {
        ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost");

        IDatabase db = redis.GetDatabase();

        var redisKeys = new List<RedisKey>();

        for (int i = 0; i < 1000; i++)
        {
            db.StringSet(i.ToString(), $"value {i}");  
            redisKeys.Add(i.ToString());
        }
        
        await CallGetBatchAsync(db, redisKeys);
        await CallGetAsync(db, redisKeys);
        await CallParallelGetAsync(db, redisKeys);
        
        Console.ReadLine();
    }

    private static async Task CallGetBatchAsync(IDatabase db, List<RedisKey> redisKeys)
    {
        var swBatch1 = Stopwatch.StartNew();

        await db.StringGetAsync(redisKeys.ToArray());

        swBatch1.Stop();

        Console.WriteLine($"batch took {swBatch1.ElapsedMilliseconds}ms");
    }
    
    private static async Task CallGetAsync(IDatabase db, List<RedisKey> redisKeys)
    {
        var swGet1 = Stopwatch.StartNew();

        foreach (var redisKey in redisKeys)
        {
            await db.StringGetAsync(redisKey);
        }

        swGet1.Stop();
        Console.WriteLine($"get took {swGet1.ElapsedMilliseconds}ms");
    }
    
    private static async Task CallParallelGetAsync(IDatabase db, List<RedisKey> redisKeys)
    {
        var swParallelGet1 = Stopwatch.StartNew();
        
        var tasks = new List<Task>();
        
        foreach (var redisKey in redisKeys)
        {
            tasks.Add(
                Task.Run(async () =>
                {
                    await db.StringGetAsync(redisKey);
                }));
        }

        await Task.WhenAll(tasks.ToArray());

        swParallelGet1.Stop();
        Console.WriteLine($"parallel get took {swParallelGet1.ElapsedMilliseconds}ms");
    }
}
