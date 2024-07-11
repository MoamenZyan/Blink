using Newtonsoft.Json.Linq;
using Ganss.Xss;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public class StoryService
{
    private readonly IRedisCache _redis;
    private readonly IRepository<Story> _storyRepository;
    private readonly IRepository<User> _userRepository;
    private readonly HtmlSanitizer sanitizer = new HtmlSanitizer();
    private readonly UploadPhotoService _uploadPhotoService;
    private readonly Guid uuid = Guid.NewGuid();
    public StoryService(IRedisCache redis,
                        IRepository<Story> storyRepository,
                        UploadPhotoService uploadPhotoService,
                        IRepository<User> userRepository)
    {
        _redis = redis;
        _storyRepository = storyRepository;
        _uploadPhotoService = uploadPhotoService;
        _userRepository = userRepository;
    }

    public async Task<Story?> AddStoryAsync(int userId, Dictionary<string, StringValues> body, IFormFile photo)
    {
        var photoName = uuid.ToString();
        if (photo is not null)
        {
            var res = await _uploadPhotoService.StoryPhotoUpload(photo, photoName);
            if (res is false)
                throw new Exception("story photo didn't upload");
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return null;

        Story story = new Story
        {
            Content = sanitizer.Sanitize(body["Content"]!),
            UserId = userId,
            Photo = photo == null ? "null" : $"https://blink-blog.s3.eu-north-1.amazonaws.com/storyPhoto/{photoName}",
            Privacy = body["Privacy"]!,
            BackgroundColor = body["backgroundColor"]!,
            TextColor = body["textColor"],
            CreatedAt = DateTime.Now,
            User = user!
        };
        Story? result = await _storyRepository.AddAsync(story);

        if (result is null)
            return null;
        _redis.Del($"user?username={user.Username}");
        _redis.Del("users");
        return result;
    }

    public async Task<List<StoryFullDto>?> GetAllStories()
    {
        string storiesFromRedis = _redis.Get("stories")!;
        if (storiesFromRedis is null)
        {
            List<Story>? storiesFromDB = await _storyRepository.GetAllAsync();
            if (storiesFromDB is null)
                return null;
            _redis.Set("stories", JsonConvert.SerializeObject(storiesFromDB, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
            return storiesFromDB.Select(story => new StoryFullDto(story)).ToList();
        }
        else
        {
            List<Story> stories = JsonConvert.DeserializeObject<List<Story>>(storiesFromRedis)!;
            return stories.Select(story => new StoryFullDto(story)).ToList();
        }
    }

    public List<StoryFullDto>? GetAllUserStories(int userId)
    {
        string storiesFromRedis = _redis.Get($"stories:user:{userId}")!;
        if (storiesFromRedis is null)
        {
            var storiesFromDB = _storyRepository.Filter(s => s.UserId == userId);
            _redis.Set($"stories:user:{userId}", JsonConvert.SerializeObject(storiesFromDB, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
            return storiesFromDB!.Select(story => new StoryFullDto(story)).ToList();
        }
        else
        {
            List<Story> stories = JsonConvert.DeserializeObject<List<Story>>(storiesFromRedis)!;
            return stories.Select(story => new StoryFullDto(story)).ToList();
        }
    }

    // To get story with it's id
    public async Task<StoryFullDto?> GetStoryById(int id)
    {
        var storyFromRedis = _redis.Get($"story:{id}");
        if (storyFromRedis is null)
        {
            var storyFromDb = await _storyRepository.GetByIdAsync(id);
            if (storyFromDb is null)
                return null;
            _redis.Set($"story:{id}", JsonConvert.SerializeObject(storyFromDb, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
            return new StoryFullDto(storyFromDb);
        }
        else
        {
            var story = JsonConvert.DeserializeObject<Story>(storyFromRedis, JsonSettings.DefaultSettings);
            if (story is null)
                return null;
            return new StoryFullDto(story);
        }
    }
}