

using StackExchange.Redis;

public class RedisCache : IRedisCache
{
    private readonly IConnectionMultiplexer _redis;

    public RedisCache(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    // To Get Cache
    public string? Get(string key)
    {
        var db = _redis.GetDatabase();
        return db.StringGet(key);
    }

    // To Set Cache
    public void Set(string key, string value, TimeSpan expiry)
    {
        IDatabase db = _redis.GetDatabase();
        db.StringSet(key, value, expiry);
    }
}