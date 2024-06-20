// Redis Interface
public interface IRedisCache
{
    void Set(string key, string value, TimeSpan expiry);
    string? Get(string key);
    void Del(string key);

    void AddLike(int postId, int userId);
    void DelLike(int postId, int userId);
    bool IsPostLikedByUser(int postId, int userId);
}