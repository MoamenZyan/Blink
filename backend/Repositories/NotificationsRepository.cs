
using Microsoft.EntityFrameworkCore;

public class NotificationsRepository
{
    private readonly DatabaseContext _context;
    public NotificationsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> AddPostNotification(PostNotification notification)
    {
        try
        {
            await _context.PostNotifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> AddFriendRequestNotification(FriendRequestNotification notification)
    {
        try
        {
            await _context.FriendRequestNotifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> DeleteAllNotifications(int id)
    {
        try
        {
            await _context.FriendRequestNotifications.Where(n => n.OwnerId == id).ExecuteDeleteAsync();
            await _context.PostNotifications.Where(n => n.OwnerId == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> DeleteFriendRequestNotification(int sender, int receiver)
    {
        try
        {
            await _context.FriendRequestNotifications.Where(n => n.OwnerId == receiver && n.UserId == sender).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<List<PostNotification>> GetAllPostNotification(int id) =>
        await _context.PostNotifications.Where(n => n.OwnerId == id).Include(n => n.User).Include(n => n.Post).ToListAsync();

    public async Task<List<FriendRequestNotification>> GetAllFriendRequestNotification(int id) =>
        await _context.FriendRequestNotifications.Where(n => n.OwnerId == id).Include(n => n.User).ToListAsync();

}