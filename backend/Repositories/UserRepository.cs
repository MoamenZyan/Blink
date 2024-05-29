
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Repository for Users

public class UserRepository : IRepository<User>
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext databaseContext)
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
        var user = await _context.Users.FirstOrDefaultAsync(u => u.id == id);
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
    public async Task<IEnumerable<User>?> Filter(Func<User, bool> func)
    {
        List<User>? users = await GetAllAsync();
        if (users is not null)
            return users.Where(func);
        else
            return null;
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
