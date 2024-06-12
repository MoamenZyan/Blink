
public class Story
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public string? Photo {get; set;}
    public string? Content {get; set;}
    public required string Privacy {get; set;}
    public DateTime CreatedAt {get; set;}

    public virtual User User {get; set;} = null!;
}

// Full story info e.g (User)
public class StoryFullDto
{
    public int Id {get; set;}
    public string? Photo {get; set;}
    public string? Content {get; set;}
    public string Privacy {get; set;}
    public DateTime CreatedAt {get; set;}

    public virtual UserDto User {get; set;} = null!;

    public StoryFullDto(Story story)
    {
        Id = story.Id;
        Photo = story.Photo;
        Content = story.Content;
        Privacy = story.Privacy;
        CreatedAt = story.CreatedAt;
        User = new UserDto(story.User);
    }
}

// Just Story Info
public class StoryDto
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public string? Photo {get; set;}
    public string? Content {get; set;}
    public string Privacy {get; set;}
    public DateTime CreatedAt {get; set;}

    public StoryDto(Story story)
    {
        Id = story.Id;
        UserId = story.User.Id;
        Photo = story.Photo;
        Content = story.Content;
        Privacy = story.Privacy;
        CreatedAt = story.CreatedAt;
    }
}