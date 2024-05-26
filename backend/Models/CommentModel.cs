using System.ComponentModel.DataAnnotations;

public class Comment
{
    [Key]
    public int id {get; set;}
    public int user_id {get; set;}
    public int post_id {get; set;}
    public string? photo {get; set;}
    public string? content {get; set;}
    public DateTime created_at {get; set;}
}
