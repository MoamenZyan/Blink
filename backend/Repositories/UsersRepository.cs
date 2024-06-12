
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
        var users = await _context.Users.ToListAsync();
        List<User> Users = new List<User>();
        if (users is not null)
        {
            foreach(User user in users)
                Users.Add(user);
            return Users;
        }
        else
        {
            return null;
        }
    }

    // Method that filter users by delegate function
    public IEnumerable<User> Filter(Func<User, bool> func)
    {
        List<User>? users = _context.Users.Where(func).ToList();
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
