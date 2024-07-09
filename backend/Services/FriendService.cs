public class FriendService
{
    private readonly IRepository<Friends> _friendsRepository;
    private readonly NotificationService _notificationService;
    private readonly IRedisCache _redis;
    public FriendService (IRepository<Friends> friendsRepository, IRedisCache redis, NotificationService notificationService)
    {
        _friendsRepository = friendsRepository;
        _notificationService = notificationService;
        _redis = redis;
    }

    public async Task<bool> SendRequest(string token, int id)
    {
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return false;

        try
        {
            Friends friendRequest = new Friends
            {
                UserId1 = userId,
                UserId2 = id,
                Type = "pending",
                CreatedAt = DateTime.Now
            };
            await _friendsRepository.AddAsync(friendRequest);
            await _notificationService.FriendRequestNotification(userId, id);
            _redis.Del($"friendStatus:{userId}:{id}");
            _redis.Del("users");
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> AcceptRequest(string token, int id)
    {
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return false;
        
        try
        {
            var request = _friendsRepository.Filter(f => f.UserId1 == id && f.UserId2 == userId)?.FirstOrDefault();
            if (request is null)
                return false;
            
            if (request.Type != "accepted")
                request.Type = "accepted";
            
            await _friendsRepository.Update(request);
            await _notificationService.DeleteFriendRequestNotification(id, userId);
            _redis.Del($"friendStatus:{userId}:{id}");
            _redis.Del($"friendStatus:{id}:{userId}");
            _redis.Del($"{userId}:friends");
            _redis.Del($"{userId}:non-friends");
            _redis.Del($"{id}:friends");
            _redis.Del($"{id}:non-friends");
            _redis.Del($"{id}:posts");
            _redis.Del($"{userId}:posts");
            _redis.Del("users");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> DeleteFriend(string token, int id)
    {
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return false;
        
        try
        {
            var request = _friendsRepository.Filter(f => f.UserId1 == userId && f.UserId2 == id ||
                             f.UserId1 == id && f.UserId2 == userId)?.FirstOrDefault();

            if (request is null)
                return false;
            
            await _friendsRepository.Delete(request);
            await _notificationService.DeleteFriendRequestNotification(userId, id);
            _redis.Del($"friendStatus:{userId}:{id}");
            _redis.Del($"friendStatus:{id}:{userId}");
            _redis.Del($"{userId}:friends");
            _redis.Del($"{userId}:non-friends");
            _redis.Del($"{id}:friends");
            _redis.Del($"{id}:non-friends");
            _redis.Del("users");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> RejectFriendRequest(string token, int id)
    {
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return false;

        try
        {
            var request = _friendsRepository.Filter(f => f.UserId1 == id && f.UserId2 == userId)?.FirstOrDefault();

            if (request is null)
                return false;
            
            await _friendsRepository.Delete(request);
            await _notificationService.DeleteFriendRequestNotification(id, userId);
            _redis.Del($"friendStatus:{userId}:{id}");
            _redis.Del($"friendStatus:{id}:{userId}");
            _redis.Del("users");
            _redis.Del($"{userId}:friends");
            _redis.Del($"{userId}:non-friends");
            _redis.Del($"{id}:friends");
            _redis.Del($"{id}:non-friends");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }
}
