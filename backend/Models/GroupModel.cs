using System.ComponentModel.DataAnnotations;

public class Group
{
    [Key]
    public int id {get; set;}
    public string? photo {get; set;}
    public string? description {get; set;}
    public required string name {get; set;}
    public required string privacy {get; set;}
    public DateTime created_at {get; set;}
}

public class GroupAdmin
{
    public int group_id {get; set;}
    public int admin_id {get; set;}
}

public class GroupUser
{
    public int group_id {get; set;}
    public int user_id {get; set;}
}
