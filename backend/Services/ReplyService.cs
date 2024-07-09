using Ganss.Xss;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;

public class ReplyService
{
    private readonly IRepository<Reply> _replyRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Post> _postRepository;
    private readonly IRedisCache _redis;
    private HtmlSanitizer sanitizer = new HtmlSanitizer();
    
    public ReplyService(IRepository<Reply> replyRepository,
        IRedisCache redis,
        IRepository<User> userRepository,
        IRepository<Post> postRepository)
    {
        _replyRepository = replyRepository;
        _userRepository = userRepository;
        _postRepository = postRepository;
        _redis = redis;
    }


    public async Task<ReplyDto?> AddReply(string token, Dictionary<string, StringValues> body)
    {
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return null;
        
        try
        {
            Console.WriteLine(body["PostId"]);
            Console.WriteLine(body["CommentId"]);
            Reply reply = new Reply
            {
                UserId = userId,
                PostId = Convert.ToInt16(body["PostId"])!,
                CommentId = Convert.ToInt16(body["CommentId"]),
                Content = sanitizer.Sanitize(body["Content"]!),
                CreatedAt = DateTime.Now
            };
            await _replyRepository.AddAsync(reply);
            var post = await _postRepository.GetByIdAsync(Convert.ToInt16(body["PostId"]));
            _redis.Del($"{userId}:posts");
            _redis.Del($"{userId}:friends");
            _redis.Del($"user?username={post?.User.Username}");
            _redis.Del("posts");
            return new ReplyDto(reply);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public List<ReplyDto>? GetAllCommentReplies(int id)
    {
        var replies = _replyRepository.Filter(r => r.CommentId == id)?
            .Select(r => new ReplyDto(r))?
            .OrderBy(r => r.CreatedAt).ToList();
        return replies;
    }

    public async Task<bool> DeleteReply(string token, int id)
    {
        var userId = JwtService.VerifyToken(token);
        if (userId is default(int))
            return false;
        var result = _replyRepository.Filter(r => r.UserId == userId && r.Id == id)?.FirstOrDefault();
        if (result is null)
            return false;
        var post = await _postRepository.GetByIdAsync(result.PostId);
        if (post is null)
            return false;
        _redis.Del($"{userId}:posts");
        _redis.Del($"{userId}:friends");
        _redis.Del($"user?username={post?.User.Username}");
        _redis.Del("posts");
        return await _replyRepository.Delete(result);
    }
}