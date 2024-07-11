using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ganss.Xss;

public class PostService
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<User> _userRepository;
    private readonly UserService _userService;
    private readonly UploadPhotoService _uploadPhotoService;
    private readonly IRedisCache _redis;
    private Guid uuid = Guid.NewGuid();
    private HtmlSanitizer sanitizer = new HtmlSanitizer();

    public PostService(IRepository<Post> postsRepository,
                        IRepository<User> userRepository,
                        IRedisCache redis,
                        UploadPhotoService uploadPhotoService,
                        UserService userService
                        )
    {
        _postRepository = postsRepository;
        _userRepository = userRepository;
        _redis = redis;
        _uploadPhotoService = uploadPhotoService;
        _userService = userService;
    }

    // Add post to database
    public async Task<PostDto?> AddPost(int id, Dictionary<string, StringValues> body, IFormFile photo)
    {
        var photoName = uuid.ToString();
        if (photo is not null)
        {
           var res = await _uploadPhotoService.PostPhotoUpload(photo, photoName);
           if (res is false)
                throw new Exception("post photo didn't upload");
        }

        Post post = new Post
        {
            UserId = id,
            Photo = photo == null ? "null" : $"https://blink-blog.s3.eu-north-1.amazonaws.com/postPhoto/{photoName}",
            Banner = "null",
            Privacy = Convert.ToString(body["Privacy"])!,
            Caption = sanitizer.Sanitize(Convert.ToString(body["Caption"])!),
            CreatedAt = DateTime.Now,
            Type = "normal"
        };
        var result = await _postRepository.AddAsync(post);
        if (result is null)
            throw new Exception("error in adding post to database");

        var postsFromDb = await _postRepository.GetAllAsync();
        _redis.Set("posts", JsonConvert.SerializeObject(postsFromDb, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
        User? user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            throw new Exception("There is no user associated with this post");
        result.User = user;
        return new PostDto(result);
    }

    public List<Post> Filter(Func<Post, bool> func)
    {
        List<Post> posts = _postRepository.Filter(func)!.OrderByDescending(p => p.CreatedAt).ToList();
        return posts;
    }

    public async Task<List<PostDto>?> GetAllPosts(string token)
    {
        // Getting posts from redis
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return new List<PostDto>();

        string? json = _redis.Get($"{userId}:posts");
        if (json is null)
        {
            List<User>? users = await _userService.GetAllFriendsUsersForPosts(userId);
            List<Post>? posts = users?.SelectMany(u => u.Posts).OrderByDescending(p => p.CreatedAt).ToList();
            if (posts is null)
                return null;
            // If not in redis then set it
            _redis.Set($"{userId}:posts", JsonConvert.SerializeObject(posts, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
            return posts.Select(p => new PostDto(p)).ToList();
        }
        else
        {
            var postsFromRedis = JsonConvert.DeserializeObject<List<Post>?>(json)!;
            return postsFromRedis.Select(p => new PostDto(p)).ToList();
        }
    }

    public async Task<PostDto?> GetPostById(int id)
    {
        // getting post from redis
        string? json = _redis.Get($"post?id={id}");
        if (json is null)
        {
            Post? result = await _postRepository.GetByIdAsync(id);
            if (result is null)
                return null;
            // If not in redis then set it
            _redis.Set($"post?id={id}", JsonConvert.SerializeObject(result, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
            return new PostDto(result);
        }
        return new PostDto(JsonConvert.DeserializeObject<Post>(json)!);
    }

    public async Task<bool> UpdatePost(JObject json, string token, int postId)
    {
        try
        {
            var userId = JwtService.VerifyToken(token);
            if (postId != userId)
                return false;
            var post = await _postRepository.GetByIdAsync(postId);
            foreach(var property in json.Properties())
            {
                var postProperty = post?.GetType().GetProperty(property.Name);
                var convertedTypeProp = property.Value.ToObject(postProperty?.PropertyType!);
            }
            return await _postRepository.Update(post!);
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return false;
        }
    }

    public async Task<bool> DeletePost(string token, int postId)
    {
        try
        {
            var userId = JwtService.VerifyToken(token);
            Post? post = await _postRepository.GetByIdAsync(postId);
            if (post?.UserId == userId)
                return await _postRepository.Delete(post);
            else
                return false;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return false;
        }
    }
}
