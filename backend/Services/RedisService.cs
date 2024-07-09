

using System.Data.Common;
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

    // To Delete Cache
    public void Del(string key)
    {
        IDatabase db = _redis.GetDatabase();
        db.KeyDelete(key);
    }

    // Add Like To A Post In Cache
    public void AddLike(int postId, int userId)
    {
        IDatabase db = _redis.GetDatabase();
        db.SetAdd($"post:{postId}:likes", userId);
    }

    // Delete Like From A Post In Cache
    public void DelLike(int postId, int userId)
    {
        IDatabase db = _redis.GetDatabase();
        db.SetRemove($"post:{postId}:likes", userId);
    }

    // Check Weather User Has Like On A Post Or Not
    public bool IsPostLikedByUser(int postId, int userId)
    {
        IDatabase db = _redis.GetDatabase();
        return db.SetContains($"post:{postId}:likes", userId);
    }

    public bool Exists(string key)
    {
        IDatabase db = _redis.GetDatabase();
        return db.KeyExists(key);
    }

    public void CreateLikesSet(int postId)
    {
        IDatabase db = _redis.GetDatabase();
        db.SetAdd($"post:{postId}:likes", "");
    }
}