
using Microsoft.EntityFrameworkCore;

public class RepliesRepository : IRepository<Reply>
{
    private readonly DatabaseContext _context;

    public RepliesRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Reply?> AddAsync(Reply reply)
    {
        try
        {
            await _context.Replies.AddAsync(reply);
            await _context.SaveChangesAsync();
            return reply;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<bool> Delete(Reply reply)
    {
        try
        {
            _context.Replies.Remove(reply);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public List<Reply>? Filter(Func<Reply, bool> func)
    {
        return _context.Replies.Where(func).ToList();
    }

    public async Task<List<Reply>?> GetAllAsync()
    {
        return await _context.Replies.ToListAsync();
    }

    public async Task<Reply?> GetByIdAsync(int id)
    {
        return await _context.Replies.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> Update(Reply reply)
    {
        try
        {
            _context.Replies.Update(reply);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }
}