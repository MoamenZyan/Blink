using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


// User entity in db (has all information)
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

// Data transfer object for sending the entity without any sensitive data
public class UserDto
{
    public int id {get; set;}
    public bool verified {get; set;}
    public string? photo {get; set;}
    public string username {get; set;}
    public string first_name {get; set;}
    public string last_name {get; set;}
    public string email {get; set;}
    public DateTime created_at {get; set;}

    public UserDto(User user)
    {
        id = user.id;
        verified = user.verified;
        photo = user.photo;
        username = user.username;
        first_name = user.first_name;
        last_name = user.last_name;
        email = user.email;
        created_at = user.created_at;
    }
}
