
public class ReactionPost
{
    public int UserId {get; set;}
    public int PostId {get; set;}
    public bool Type {get; set;}

    public virtual User User {get; set;} = null!;
    public virtual Post Post {get; set;} = null!;
}

public class ReactionPostDto
{
    public int UserId {get; set;}
    public int PostId {get; set;}
    public bool Type {get; set;}
    public string Username {get; set;} = null!;

    public ReactionPostDto (ReactionPost reactionPost)
    {
        UserId = reactionPost.UserId;
        PostId = reactionPost.PostId;
        Type = reactionPost.Type;

        Username = reactionPost.User.Username;
    }
}

public class ReactionComment
{
    public int UserId {get; set;}
    public int CommentId {get; set;}
    public bool Type {get; set;}

    public virtual User User {get; set;} = null!;
    public virtual Comment Comment {get; set;} = null!;
}

public class ReactionCommentDto
{
    public int UserId {get; set;}
    public int CommentId {get; set;}
    public bool Type {get; set;}
    public string Username {get; set;} = null!;

    public ReactionCommentDto(ReactionComment reactionComment)
    {
        UserId = reactionComment.UserId;
        CommentId = reactionComment.CommentId;
        Type = reactionComment.Type;

        Username = reactionComment.User.Username;
    }
}

public class ReactionReply
{
    public int UserId {get; set;}
    public int ReplyId {get; set;}
    public bool Type {get; set;}

    public virtual User User {get; set;} = null!;
    public virtual Reply Reply {get; set;} = null!;
}


public class ReactionReplyDto
{
    public int UserId {get; set;}
    public int ReplyId {get; set;}
    public bool Type {get; set;}
    public string Username {get; set;} = null!;

    public ReactionReplyDto(ReactionReply reactionReply)
    {
        UserId = reactionReply.UserId;
        ReplyId = reactionReply.ReplyId;
        Type = reactionReply.Type;

        Username = reactionReply.User.Username;
    }
}