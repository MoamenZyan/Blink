
public class Comment
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public int PostId {get; set;}
    public string? Photo {get; set;}
    public string? Content {get; set;}
    public DateTime CreatedAt {get; set;}

    public virtual User User {get; set;} = null!;
    public virtual Post Post {get; set;} = null!;
    public virtual List<Reply> Replies {get; set;} = new List<Reply>();
    public List<ReactionComment> Reactions = new List<ReactionComment>();
}

public class CommentDto
{
    public int Id {get; set;}
    public string? Photo {get; set;}
    public string? Content {get; set;}
    public DateTime CreatedAt {get; set;}

    public virtual UserDto User {get; set;} = null!;
    public virtual PostDto Post {get; set;} = null!;
    public virtual List<ReplyDto> Replies {get; set;} = new List<ReplyDto>();
    public List<ReactionCommentDto> Reactions = new List<ReactionCommentDto>();

    public CommentDto(Comment comment)
    {
        Id = comment.Id;
        Photo = comment.Photo;
        Content = comment.Content;
        CreatedAt = comment.CreatedAt;

        User = new UserDto(comment.User);
        Post = new PostDto(comment.Post);
        Replies = comment.Replies.Select(r => new ReplyDto(r)).ToList();
        Reactions = comment.Reactions.Select(r => new ReactionCommentDto(r)).ToList();
    }
}
