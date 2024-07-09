
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

// Repository for Users

public class UsersRepository : IRepository<User>
{
    private readonly DatabaseContext _context;

    public UsersRepository(DatabaseContext databaseContext)
    {
        _context = databaseContext;
    }

    // Method to add user to database
    public async Task<User?> AddAsync(User user)
    {
        var result = await _context.Users.AddAsync(user);
        if (result.State == EntityState.Added)
        {
            await _context.SaveChangesAsync();
            return user;
        }
        else
            return null;
    }

    // Method to get specific user from database
    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await _context.Users
                    .Include(u => u.Posts)
                    .Include(u => u.Stories)
                    .FirstOrDefaultAsync(u => u.Id == id);

        if (user is not null)
            return user;
        else
            return null;
    }

    // Method to get all users from database
    public async Task<List<User>?> GetAllAsync()
    {
        var users = await _context.Users.Include(u => u.Posts)
                                            .ThenInclude(p => p.Reactions)
                                        .Include(u => u.Posts)
                                            .ThenInclude(p => p.Comments)
                                        .Include(u => u.Posts)
                                            .ThenInclude(u => u.Replies)
                                        .Include(u => u.Friends)
                                            .ThenInclude(u => u.User2)
                                        .Include(u => u.FriendOf)
                                            .ThenInclude(u => u.User1)
                                        .Include(u => u.Stories).ToListAsync();
        return users;
    }

    // Method that filter users by delegate function
    public List<User> Filter(Func<User, bool> func)
    {
        List<User>? users = _context.Users
                            .Include(u => u.Posts)
                                .ThenInclude(p => p.Reactions)
                            .Include(u => u.Posts)
                                .ThenInclude(p => p.Comments)
                            .Include(u => u.Posts)
                                .ThenInclude(p => p.Replies)
                            .Include(u => u.Stories)
                            .Where(func)
                            .ToList();
        return users;
    }

    // Update The Passed User
    public async Task<bool> Update(User user)
    {
        EntityEntry result = _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return result.State == EntityState.Modified;
    }

    // Method to delete one or more user
    public async Task<bool> Delete(User user)
    {
        try
        {
            _context.Users.Remove(user);
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
