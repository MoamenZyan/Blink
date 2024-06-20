
using Microsoft.EntityFrameworkCore;

public class StoriesRepository : IRepository<Story>
{
    private readonly DatabaseContext _context;

    public StoriesRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Story?> AddAsync(Story story)
    {
        await _context.Stories.AddAsync(story);
        await _context.SaveChangesAsync();
        return story;
    }

    public async Task<bool> Delete(Story story)
    {
        try
        {
            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return false;
        }
    }

    public List<Story>? Filter(Func<Story, bool> func)
    {
        return _context.Stories.Include(s => s.User).Where(func).ToList();
    }

    public async Task<List<Story>?> GetAllAsync()
    {
        return await _context.Stories.Include(s => s.User).ToListAsync();
    }

    public async Task<Story?> GetByIdAsync(int id)
    {
        return await _context.Stories.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> Update(Story story)
    {
        try
        {
            _context.Stories.Update(story);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return false;
        }
    }
}
