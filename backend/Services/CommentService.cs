
using Ganss.Xss;
using Microsoft.Extensions.Primitives;
public class CommentService
{
    private readonly IRepository<Comment> _commentsRepository;
    private readonly IRepository<Post> _postsRepository;
    private readonly IRepository<User> _usersRepository;
    private readonly IRedisCache _redis;
    private readonly HtmlSanitizer sanitizer = new HtmlSanitizer();

    public CommentService(
        IRepository<Comment> commentsRepository,
        IRepository<Post> postsRepository,
        IRedisCache redis,
        IRepository<User> usersRepository
    )
    {
        _commentsRepository = commentsRepository;
        _postsRepository = postsRepository;
        _redis = redis;
        _usersRepository = usersRepository;
    }

    // Creating Comment
    public async Task<CommentDto?> CreateComment(string token, Dictionary<string, StringValues> form)
    {
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return null;

        try
        {
            Console.WriteLine($"User Id" + userId);
            Comment comment = new Comment
            {
                UserId = userId,
                PostId = Convert.ToInt16(form["PostId"]!),
                Photo = "null",
                Content = sanitizer.Sanitize(form["Content"]!),
                CreatedAt = DateTime.Now,
            };
            var post = await _postsRepository.GetByIdAsync(Convert.ToInt16(form["PostId"]!));
            await _commentsRepository.AddAsync(comment);
            _redis.Del($"{userId}:posts");
            _redis.Del($"{userId}:friends");
            _redis.Del($"user?username={post?.User.Username}");
            return new CommentDto(comment);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<bool> DeleteComment(string token, int id)
    {
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return false;
        var result = _commentsRepository.Filter(c => c.UserId == userId && c.Id == id)?.FirstOrDefault();
        if (result is null)
            return false;
        var post = await _postsRepository.GetByIdAsync(result.PostId);

        if (post is null)
            return false;
        _redis.Del($"{userId}:friends");
        _redis.Del($"{userId}:posts");
        _redis.Del($"user?username={post?.User.Username}");
        _redis.Del("posts");
        return await _commentsRepository.Delete(result);
    }
}