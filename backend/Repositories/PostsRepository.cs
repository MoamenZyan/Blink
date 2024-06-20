using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class PostsRepository : IRepository<Post>
{
    private readonly DatabaseContext _context;
    public PostsRepository(DatabaseContext databaseContext)
    {
        _context = databaseContext;
    }

    // Add Post To Database
    public async Task<Post?> AddAsync(Post post)
    {
        try
        {
            var result = await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            if (result.State == EntityState.Added || result.State == EntityState.Unchanged)
                return post;
            else
                return null;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return null;
        }
    }

    // Filter Posts On Specific Criteria
    public List<Post> Filter(Func<Post, bool> func)
    {
        var results =  _context.Posts.Where(func).OrderByDescending(p => p.CreatedAt).ToList();
        return results;
    }

    // Get All Posts From Database
    public async Task<List<Post>?> GetAllAsync()
    {
        List<Post> posts = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Reactions)
                .ThenInclude(p => p.User)
            .Include(p => p.Comments)
            .Include(p => p.Replies)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return posts;
    }

    // Get Post By Specific Id
    public async Task<Post?> GetByIdAsync(int id)
    {
        Post? post = await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Reactions)
                    .FirstOrDefaultAsync(post => post.Id == id);

        return post;
    }

    // Update The Passed Post
    public async Task<bool> Update(Post post)
    {
        EntityEntry result = _context.Posts.Update(post);
        await _context.SaveChangesAsync();
        return result.State == EntityState.Modified;
    }

    // Delete Post From Database
    public async Task<bool> Delete(Post post)
    {
        try
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
        catch(Exception exp)
        {
            Console.WriteLine(exp);
            return false;
        }
    }
}