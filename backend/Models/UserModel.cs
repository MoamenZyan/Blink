
// User entity in db (has all information)
public class User
{
    public int Id {get; set;}
    public bool Verified {get; set;}
    public string? Photo {get; set;}
    public required string Username {get; set;}
    public required string FirstName {get; set;}
    public required string LastName {get; set;}
    public required string Password {get; set;}
    public required string Privacy {get; set;}
    public required string Email {get; set;}
    public DateTime CreatedAt {get; set;}

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
    public DateTime CreatedAt {get; set;}

    public virtual List<PostDto> Posts {get; set;} = new List<PostDto>();
    public virtual List<StoryDto> Stories {get; set;} = new List<StoryDto>();

    public UserFullDto(User user)
    {
        Id = user.Id;
        verified = user.Verified;
        photo = user.Photo;
        username = user.Username;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        CreatedAt = user.CreatedAt;
        Privacy = user.Privacy;
        Posts = user.Posts.Select(p => new PostDto(p)).ToList();
        Stories = user.Stories.Select(s => new StoryDto(s)).ToList();
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
        CreatedAt = user.CreatedAt;
        Privacy = user.Privacy;
    }
}
