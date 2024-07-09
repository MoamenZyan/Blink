
using Newtonsoft.Json;

public class NotificationService
{
    private readonly NotificationsRepository _notificationRepository;
    private readonly IRedisCache _redis;
    public NotificationService(NotificationsRepository notificationsRepository, IRedisCache redis)
    {
        _notificationRepository = notificationsRepository;
        _redis = redis;
    }

    public async Task<bool> FriendRequestNotification(int senderId, int receiverId)
    {
        FriendRequestNotification notification = new FriendRequestNotification
        {
            OwnerId = receiverId,
            UserId = senderId,
            CreatedAt = DateTime.Now
        };

        try
        {
            var result = await _notificationRepository.AddFriendRequestNotification(notification);
            _redis.Del($"user:{receiverId}:friendRequestNotifications");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> DeleteFriendRequestNotification(int sender, int receiver)
    {
        try
        {
            await _notificationRepository.DeleteFriendRequestNotification(sender, receiver);
            _redis.Del($"user:{receiver}:friendRequestNotifications");
            _redis.Del($"user:{receiver}:postNotifications");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> DeleteAllNotifications(string token)
    {
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return false;
        try
        {
            await _notificationRepository.DeleteAllNotifications(userId);
            _redis.Del($"user:{userId}:friendRequestNotifications");
            _redis.Del($"user:{userId}:postNotifications");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<(List<PostNotificationDto>?, List<FriendRequestNotificationDto>?)> GetAllNotifications(string token)
    {
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return (null, null);

        var fromRedisPostNotification = _redis.Get($"user:{userId}:postNotifications");
        var fromRedisFriendRequestNotification = _redis.Get($"user:{userId}:friendRequestNotifications");
        List<PostNotificationDto>? PostNoti = null;
        List<FriendRequestNotificationDto>? FriendRequestNoti = null;

        if (fromRedisPostNotification is null)
        {
            var PostNotifications = await _notificationRepository.GetAllPostNotification(userId);
            _redis.Set($"user:{userId}:postNotifications", JsonConvert.SerializeObject(PostNotifications, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
            PostNoti = PostNotifications.Select(n => new PostNotificationDto(n)).ToList();
        }
        else
        {
            PostNoti = JsonConvert.DeserializeObject<List<PostNotification>>(fromRedisPostNotification)?.Select(n => new PostNotificationDto(n)).ToList();
        }

        if (fromRedisFriendRequestNotification is null)
        {
            var FriendRequestNotifications = await _notificationRepository.GetAllFriendRequestNotification(userId);
            _redis.Set($"user:{userId}:friendRequestNotifications", JsonConvert.SerializeObject(FriendRequestNotifications, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
            FriendRequestNoti = FriendRequestNotifications.Select(n => new FriendRequestNotificationDto(n)).ToList();
        }
        else
        {
            FriendRequestNoti = JsonConvert.DeserializeObject<List<FriendRequestNotification>>(fromRedisFriendRequestNotification)?.Select(n => new FriendRequestNotificationDto(n)).ToList();
        }

        return (PostNoti, FriendRequestNoti);
    }
}