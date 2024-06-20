
using backend.Migrations;
using Microsoft.AspNetCore.Mvc;
using Sprache;

public class ReactionService
{
    private readonly IRepository<ReactionPost> _reactionPostRepository;
    private readonly IRepository<ReactionComment> _reactionCommentRepository;
    private readonly IRepository<ReactionReply> _reactionReplyRepository;
    private readonly IRepository<Post> _postRepository;
    private readonly IRedisCache _redis;


    public ReactionService(IRepository<ReactionPost> reactionPostRepository,
                            IRepository<ReactionComment> reactionCommentRepository,
                            IRepository<ReactionReply> reactionReplyRepository,
                            IRedisCache redis,
                            IRepository<Post> postRepository)
    {
        _reactionCommentRepository = reactionCommentRepository;
        _reactionPostRepository = reactionPostRepository;
        _reactionReplyRepository = reactionReplyRepository;
        _postRepository = postRepository;
        _redis = redis;
    }

    public async Task<bool> AddReactionOnPost(int postId, int userId)
    {
        ReactionPost reaction = new ReactionPost
        {
            PostId = postId,
            UserId = userId
        };
        try
        {
            await _reactionPostRepository.AddAsync(reaction);
            var post = await _postRepository.GetByIdAsync(postId);
            if (post is null)
                return false;
            _redis.Del($"user?username={post.User.Username}");
            _redis.AddLike(postId, userId);
            _redis.Del("posts");
            return true;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return false;
        }
    }

    public async Task<bool> DeleteReactionFromPost(int postId, int userId)
    {
        ReactionPost? reaction = _reactionPostRepository.Filter(r => r.PostId == postId && r.UserId == userId)!.FirstOrDefault();
        if (reaction is null)
            return true;

        try
        {
            await _reactionPostRepository.Delete(reaction);
            var post = await _postRepository.GetByIdAsync(postId);
            if (post is null)
                return false;
            _redis.Del($"user?username={post.User.Username}");
            _redis.DelLike(postId, userId);
            _redis.Del("posts");
            return true;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return false;
        }
    }

    public bool HasReactionOnAPost(int postId, int userId)
    {
        return _redis.IsPostLikedByUser(postId, userId);
    }
}