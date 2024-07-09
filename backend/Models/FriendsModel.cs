
public class Friends
{
    public int UserId1 {get; set;}
    public int UserId2 {get; set;}
    public required string Type {get; set;}
    public DateTime CreatedAt {get; set;}

    public User User1 {get; set;} = null!;
    public User User2 {get; set;} = null!;
}

public class FriendDto
{
    public int FriendId {get; set;}
    public string Type {get; set;}
    public DateTime CreatedAt {get; set;}

    public UserDto Friend {get; set;}

    public FriendDto(Friends friendRequest)
    {
        FriendId = friendRequest.UserId2;
        Type = friendRequest.Type;
        CreatedAt = friendRequest.CreatedAt;
        Friend = new UserDto(friendRequest.User2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is FriendDto other)
        {
            return FriendId == other.FriendId;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return FriendId.GetHashCode();
    }
}
