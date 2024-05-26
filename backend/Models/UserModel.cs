using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class User
{
    [Key]
    public int id {get; set;}
    public bool verified {get; set;}
    public string? photo {get; set;}
    public required string username {get; set;}
    public required string first_name {get; set;}
    public required string last_name {get; set;}
    public required string password {get; set;}
    public required string email {get; set;}
    public DateTime created_at {get; set;}
}
