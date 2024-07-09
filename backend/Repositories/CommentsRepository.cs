

using Microsoft.EntityFrameworkCore;

public class CommentsRepository : IRepository<Comment>
{
    private readonly DatabaseContext _context;

    public CommentsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Comment?> AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> Delete(Comment comment)
    {
        try
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception err)
        {
            Console.WriteLine(err);
            return false;
        }
    }

    public List<Comment>? Filter(Func<Comment, bool> func)
    {
        return _context.Comments.Where(func).ToList();
    }

    public async Task<List<Comment>?> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> Update(Comment comment)
    {
        try
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception err)
        {
            Console.WriteLine(err);
            return false;
        }
    }
}