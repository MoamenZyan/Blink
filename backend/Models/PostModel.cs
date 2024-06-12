
public class Post
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public string? Photo {get; set;}
    public string? Caption {get; set;}
    public required string Privacy {get; set;}
    public DateTime CreatedAt {get; set;}

    public virtual User User {get; set;} = null!;
    public virtual List<Reply> Replies {get; set;} = new List<Reply>();
    public virtual List<Comment> Comments {get; set;} = new List<Comment>();
    public virtual List<ReactionPost> Reactions {get; set;} = new List<ReactionPost>();
}


// Full post info e.g (User)
public class PostDto
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public string? Photo {get; set;}
    public string? Caption {get; set;}
    public string? UserPhoto {get; set;}
    public string? Username {get; set;}
    public string Privacy {get; set;}
    public DateTime CreatedAt {get; set;}

    public List<ReplyDto> Replies {get; set;} = new List<ReplyDto>();
    public List<CommentDto> Comments {get; set;} = new List<CommentDto>();
    public List<ReactionPostDto> Reactions {get; set;} = new List<ReactionPostDto>();

    public PostDto(Post post)
    {
        Id = post.Id;
        UserId = post.User.Id;
        Photo = post.Photo;
        Caption = post.Caption;
        Privacy = post.Privacy;
        CreatedAt = post.CreatedAt;
        UserPhoto = post.User.Photo;
        Username = post.User.Username;
        Replies = post.Replies.Select(r => new ReplyDto(r)).ToList();
        Comments = post.Comments.Select(c => new CommentDto(c)).ToList();
        Reactions = post.Reactions.Select(rp => new ReactionPostDto(rp)).ToList();
    }
}
