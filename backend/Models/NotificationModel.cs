
public class PostNotification
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public int PostId {get; set;}
    public int OwnerId {get; set;}
    public required string Message {get; set;}
    public DateTime CreatedAt {get; set;}

    public User User {get; set;} = null!;
    public Post Post {get; set;} = null!;
}


public class FriendRequestNotification
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public int OwnerId {get; set;}
    public DateTime CreatedAt {get; set;}

    public User User {get; set;} = null!;
}



public class PostNotificationDto
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public int OwnerId {get; set;}
    public string? Username {get; set;}
    public string? UserPhoto {get; set;}
    public string Message {get; set;}
    public DateTime CreatedAt {get; set;}
    public string? PostPhoto {get; set;}
    public int Reactions {get; set;}
    public int PostId {get; set;}

    public PostNotificationDto(PostNotification notification)
    {
        Id = notification.Id;
        UserId = notification.UserId;
        OwnerId = notification.OwnerId;
        Username = notification.User.Username;
        UserPhoto = notification.User.Photo;
        Message = notification.Message;
        CreatedAt = notification.CreatedAt;
        PostPhoto = notification.Post.Photo;
        PostId = notification.Post.Id;
        Reactions = notification.Post.Reactions.Count;
    }
}

public class FriendRequestNotificationDto
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public int OwnerId {get; set;}
    public DateTime CreatedAt {get; set;}
    public string Username {get; set;}
    public string? UserPhoto {get; set;}
    public string? City {get; set;}
    public string? Country {get; set;}
    public string Email {get; set;}

    public FriendRequestNotificationDto(FriendRequestNotification notification)
    {
        Id = notification.Id;
        UserId = notification.User.Id;
        OwnerId = notification.OwnerId;
        CreatedAt = notification.CreatedAt;
        Username = notification.User.Username;
        UserPhoto = notification.User.Photo;
        City = notification.User.City;
        Country = notification.User.Country;
        Email = notification.User.Email;
    }
}