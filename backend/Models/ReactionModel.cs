
public class ReactionPost
{
    public int user_id {get; set;}
    public int post_id {get; set;}
    public bool type {get; set;}
}

public class ReactionComment
{
    public int user_id {get; set;}
    public int comment_id {get; set;}
    public bool type {get; set;}
}

public class ReactionReply
{
    public int user_id {get; set;}
    public int reply_id {get; set;}
    public bool type {get; set;}
}
