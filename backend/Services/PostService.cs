using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using Ganss.Xss;

public class PostService
{
    private readonly IRepository<Post> _postRepository;
    private HtmlSanitizer sanitizer = new HtmlSanitizer();

    public PostService(IRepository<Post> postsRepository)
    {
        _postRepository = postsRepository;
    }

    // Add photo to database
    public async Task<Post?> AddPost(int id, JObject json)
    {
        Post post = new Post
        {
            user_id = id,
            photo = Convert.ToString(json["photo"]), // Upload The Photo ******
            privacy = Convert.ToString(json["privacy"])!,
            caption = sanitizer.Sanitize(Convert.ToString(json["caption"])!),
            created_at = DateTime.Now
        };
        var result = await _postRepository.AddAsync(post);
        return result;
    }

    public List<Post> Filter(Func<Post, bool> func)
    {
        List<Post> posts = _postRepository.Filter(func)!.ToList();
        return posts;
    }

    public async Task<List<Post>?> GetAllPosts()
    {
        List<Post>? posts = await _postRepository.GetAllAsync();
        return posts;
    }

    public async Task<Post?> GetPostById(int id)
    {
        Post? post = await _postRepository.GetByIdAsync(id);
        return post;
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
            if (post?.user_id == userId)
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
