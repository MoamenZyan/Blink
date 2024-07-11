
// User entity in db (has all information)
public class User
{
    public int Id {get; set;}
    public bool Verified {get; set;}
    public string? Photo {get; set;}
    public string? About {get; set;}
    public string? Headline {get; set;}
    public required string Username {get; set;}
    public required string FirstName {get; set;}
    public string Country {get; set;} = null!;
    public string Banner {get; set;} = null!;
    public string City {get; set;} = null!;
    public required string LastName {get; set;}
    public required string Password {get; set;}
    public required string Privacy {get; set;}
    public required string Email {get; set;}
    public DateTime CreatedAt {get; set;}

    public List<Friends> Friends {get; set;} = new List<Friends>();
    public List<Friends> FriendOf {get; set;} = new List<Friends>();
    public List<FriendRequestNotification> FriendRequestNotifications {get; set;} = new List<FriendRequestNotification>();
    public List<PostNotification> PostNotifications {get; set;} = new List<PostNotification>();
    public virtual List<Story> Stories {get; set;} = new List<Story>();
    public virtual List<Reply> Replies {get; set;} = new List<Reply>();
    public virtual List<Post> Posts {get; set;} = new List<Post>();
    public List<ReactionPost> ReactionPosts = new List<ReactionPost>();
    public List<ReactionComment> ReactionComments = new List<ReactionComment>();
    public List<ReactionReply> ReactionReplies = new List<ReactionReply>();
    public virtual List<Comment> Comments {get; set;} = new List<Comment>();
    public virtual List<Group> OwnedGroups {get; set;} = new List<Group>();
    public virtual List<GroupAdmin> AdminOfGroups {get; set;} = new List<GroupAdmin>();
    public virtual List<GroupUser> MemberOfGroups {get; set;} = new List<GroupUser>();
}

// User with full info e.g (posts, stories)
public class UserFullDto
{
    public int Id {get; set;}
    public bool verified {get; set;}
    public string? photo {get; set;}
    public string username {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Privacy {get; set;}
    public string Email {get; set;}
    public string Country {get; set;}
    public string Banner {get; set;}
    public string City {get; set;}
    public DateTime CreatedAt {get; set;}
    public string? About {get; set;}
    public string? Headline {get; set;}
    public string friendStatus {get; set;}

    public virtual List<PostDto> Posts {get; set;} = new List<PostDto>();
    public virtual List<StoryDto> Stories {get; set;} = new List<StoryDto>();
    public List<FriendDto> Friends {get; set;} = new List<FriendDto>();
    public List<FriendOfDto> FriendOf {get; set;} = new List<FriendOfDto>();
    public List<FriendDto> OutGoingfriendRequest {get; set;} = new List<FriendDto>(); 
    public List<FriendDto> IncomingFriendRequest {get; set;} = new List<FriendDto>();

    public UserFullDto(User user)
    {
        Id = user.Id;
        verified = user.Verified;
        photo = user.Photo;
        username = user.Username;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Country = user.Country;
        City = user.City;
        CreatedAt = user.CreatedAt;
        Banner = user.Banner;
        Privacy = user.Privacy;
        About = user.About;
        Headline = user.Headline;
        Posts = user.Posts.Select(p => new PostDto(p)).OrderByDescending(p => p.CreatedAt).ToList() ?? new List<PostDto>();
        Stories = user.Stories.Select(s => new StoryDto(s)).OrderByDescending(p => p.CreatedAt).ToList();
        Friends = user.Friends.Where(f => f.Type == "accepted" && f.UserId1 == Id).Select(f => new FriendDto(f)).ToList();
        FriendOf = user.FriendOf.Where(f => f.Type == "accepted" && f.UserId2 == Id).Select(f => new FriendOfDto(f)).ToList();
        OutGoingfriendRequest = user.Friends.Where(f => f.Type == "pending").Select(f => new FriendDto(f)).ToList();
        IncomingFriendRequest = user.FriendOf.Where(f => f.Type == "pending").Select(f => new FriendDto(f)).ToList();
        friendStatus = "";
    }
}

// Just user info
public class UserDto
{
    public int Id {get; set;}
    public bool verified {get; set;}
    public string? photo {get; set;}
    public string username {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Privacy {get; set;}
    public string Email {get; set;}
    public string Country {get; set;}
    public string City {get; set;}
    public string? About {get; set;}
    public string? Headline {get; set;}
    public DateTime CreatedAt {get; set;}

    public UserDto(User user)
    {
        Id = user.Id;
        verified = user.Verified;
        photo = user.Photo;
        username = user.Username;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Country = user.Country;
        City = user.City;
        CreatedAt = user.CreatedAt;
        Privacy = user.Privacy;
        About = user.About;
        Headline = user.Headline;
    }
}
