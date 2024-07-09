
using Microsoft.EntityFrameworkCore;

public class FriendsRepository : IRepository<Friends>
{
    private readonly DatabaseContext _context;

    public FriendsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Friends?> AddAsync(Friends friend)
    {
        try
        {
            await _context.Friends.AddAsync(friend);
            await _context.SaveChangesAsync();
            return friend;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<bool> Delete(Friends friend)
    {
        try
        {
            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public List<Friends>? Filter(Func<Friends, bool> func)
    {
        return _context.Friends.Where(func).ToList();
    }

    public async Task<List<Friends>?> GetAllAsync()
    {
        return await _context.Friends.ToListAsync();
    }

    public async Task<Friends?> GetByIdAsync(int id)
    {
        return await _context.Friends.FirstOrDefaultAsync(f => f.UserId2 == id);
    }

    public async Task<bool> Update(Friends friend)
    {
        try
        {
            _context.Friends.Update(friend);
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