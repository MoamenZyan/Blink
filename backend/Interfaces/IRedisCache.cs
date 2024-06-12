// Redis Interface
public interface IRedisCache
{
    void Set(string key, string value, TimeSpan expiry);
    string? Get(string key);
}