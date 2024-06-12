
public class Reply
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public int PostId {get; set;}
    public int CommentId {get; set;}
    public string? Photo {get; set;}
    public string? Content {get; set;}
    public DateTime CreatedAt {get; set;}

    public virtual User User {get; set;} = null!;
    public virtual Post Post {get; set;} = null!;
    public virtual Comment Comment {get; set;} = null!;

    public virtual List<ReactionReply> Reactions {get; set;} = new List<ReactionReply>();
}

public class ReplyDto
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public int PostId {get; set;}
    public int CommentId {get; set;}
    public string? Photo {get; set;}
    public string? Content {get; set;}
    public string? UserPhoto {get; set;}
    public string? Username {get; set;}
    public DateTime CreatedAt {get; set;}

    public virtual List<ReactionReplyDto> Reactions {get; set;} = new List<ReactionReplyDto>();

    public ReplyDto(Reply reply)
    {
        Id = reply.Id;
        UserId = reply.User.Id;
        PostId = reply.Post.Id;
        CommentId = reply.Comment.Id;
        Photo = reply.Photo;
        Content = reply.Content;
        UserPhoto = reply.User.Photo;
        Username = reply.User.Username;
        CreatedAt = reply.CreatedAt;
        Reactions = reply.Reactions.Select(r => new ReactionReplyDto(r)).ToList();
    }
}
