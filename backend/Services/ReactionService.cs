
using backend.Migrations;
using Microsoft.AspNetCore.Mvc;
using Sprache;

public class ReactionService
{
    private readonly IRepository<ReactionPost> _reactionPostRepository;
    private readonly IRepository<ReactionComment> _reactionCommentRepository;
    private readonly IRepository<ReactionReply> _reactionReplyRepository;
    private readonly NotificationsRepository _notificationsRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Post> _postRepository;
    private readonly IRedisCache _redis;


    public ReactionService(IRepository<ReactionPost> reactionPostRepository,
                            IRepository<ReactionComment> reactionCommentRepository,
                            IRepository<ReactionReply> reactionReplyRepository,
                            IRedisCache redis,
                            IRepository<Post> postRepository,
                            NotificationsRepository notificationsRepository,
                            IRepository<User> userRepository)
    {
        _reactionCommentRepository = reactionCommentRepository;
        _reactionPostRepository = reactionPostRepository;
        _reactionReplyRepository = reactionReplyRepository;
        _postRepository = postRepository;
        _notificationsRepository = notificationsRepository;
        _userRepository = userRepository;
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
            var user = await _userRepository.GetByIdAsync(userId);
            PostNotification notification = new PostNotification
            {
                PostId = post.Id,
                UserId = userId,
                OwnerId = post.User.Id,
                Message = $"and {post.Reactions.Count - 1} others liked your post",
                CreatedAt = DateTime.Now,
            };
            await _notificationsRepository.AddPostNotification(notification);
            _redis.Del($"user:{post.User.Id}:postNotifications");
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
        var postLikes = _redis.Exists($"post:{postId}:likes");
        if (postLikes is false)
        {
            ReactionPost? reaction = _reactionPostRepository.Filter(r => r.PostId == postId && r.UserId == userId)!.FirstOrDefault();
            if (reaction is null)
            {
                _redis.CreateLikesSet(postId);
                return false;
            }
            _redis.AddLike(postId, userId);
            return true;
        }
        else
        {
            return _redis.IsPostLikedByUser(postId, userId);
        }
    }
}