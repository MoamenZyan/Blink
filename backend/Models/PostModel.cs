using System.ComponentModel.DataAnnotations;

public class Post
{
    [Key]
    public int id {get; set;}
    public int user_id {get; set;}
    public string? photo {get; set;}
    public string? caption {get; set;}
    public required string privacy {get; set;}
    public DateTime created_at {get; set;}
}
