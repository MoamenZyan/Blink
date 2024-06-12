
public class Group
{
    public int Id {get; set;}
    public int OwnerId {get; set;}
    public string? Photo {get; set;}
    public string? Description {get; set;}
    public required string Name {get; set;}
    public required string Privacy {get; set;}
    public DateTime CreatedAt {get; set;}

    public virtual User Owner {get; set;} = null!;
    public List<GroupUser> Members = new List<GroupUser>();
    public List<GroupAdmin> Admins = new List<GroupAdmin>();
}

public class GroupDto
{
    public int Id {get; set;}
    public string? Photo {get; set;}
    public string? Description {get; set;}
    public string Name {get; set;}
    public string Privacy {get; set;}
    public DateTime CreatedAt {get; set;}

    public virtual UserDto Owner {get; set;} = null!;
    public List<GroupUser> Members = new List<GroupUser>();
    public List<GroupAdmin> Admins = new List<GroupAdmin>();

    public GroupDto(Group group)
    {
        Id = group.Id;
        Photo = group.Photo;
        Description = group.Description;
        Name = group.Name;
        Privacy = group.Privacy;
        CreatedAt = group.CreatedAt;

        Owner = new UserDto(group.Owner);
        Members = group.Members;
        Admins = group.Admins;
    }
}

public class GroupAdmin
{
    public int GroupId {get; set;}
    public int UserId {get; set;}

    public virtual Group Group {get; set;} = null!;
    public virtual User User {get; set;} = null!;
}

public class GroupAdminDto
{
    public virtual GroupDto Group {get; set;} = null!;
    public virtual UserDto User {get; set;} = null!;

    public GroupAdminDto(GroupAdmin groupAdmin)
    {
        Group = new GroupDto(groupAdmin.Group);
        User = new UserDto(groupAdmin.User);
    }
}

public class GroupUser
{
    public int GroupId {get; set;}
    public int UserId {get; set;}

    public virtual Group Group {get; set;} = null!;
    public virtual User User {get; set;} = null!;
}

public class GroupUserDto
{
    public virtual GroupDto Group {get; set;} = null!;
    public virtual UserDto User {get; set;} = null!;

    public GroupUserDto(GroupUser groupUser)
    {
        Group = new GroupDto(groupUser.Group);
        User = new UserDto(groupUser.User);
    }
}