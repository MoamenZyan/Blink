

using Microsoft.EntityFrameworkCore;

public class ReactionsPostRepository : IRepository<ReactionPost>
{
    private readonly DatabaseContext _context;
    public ReactionsPostRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ReactionPost?> AddAsync(ReactionPost entity)
    {
        try
        {
            await _context.ReactionPosts.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return null;
        }
    }

    public async Task<bool> Delete(ReactionPost reaction)
    {
        try
        {
            _context.ReactionPosts.Remove(reaction);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return false;
        }
    }

    public List<ReactionPost>? Filter(Func<ReactionPost, bool> func)
    {
        return _context.ReactionPosts.Where(func).ToList();
    }

    public async Task<List<ReactionPost>?> GetAllAsync()
    {
        return await _context.ReactionPosts.ToListAsync();
    }

    public Task<ReactionPost?> GetByIdAsync(int id)
    {
        return null!;
    }

    public Task<bool> Update(ReactionPost Entity)
    {
        return null!;
    }
}

public class ReactionsCommentRepository : IRepository<ReactionComment>
{
    private readonly DatabaseContext _context;
    public ReactionsCommentRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ReactionComment?> AddAsync(ReactionComment entity)
    {
        try
        {
            await _context.ReactionComments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return null;
        }
    }

    public async Task<bool> Delete(ReactionComment reaction)
    {
        try
        {
            _context.ReactionComments.Remove(reaction);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return false;
        }
    }

    public List<ReactionComment>? Filter(Func<ReactionComment, bool> func)
    {
        return _context.ReactionComments.Where(func).ToList();
    }

    public async Task<List<ReactionComment>?> GetAllAsync()
    {
        return await _context.ReactionComments.ToListAsync();
    }

    public Task<ReactionComment?> GetByIdAsync(int id)
    {
        return null!;
    }

    public Task<bool> Update(ReactionComment Entity)
    {
        return null!;
    }
}

public class ReactionsReplyRepository : IRepository<ReactionReply>
{
    private readonly DatabaseContext _context;
    public ReactionsReplyRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ReactionReply?> AddAsync(ReactionReply entity)
    {
        try
        {
            await _context.ReactionReplies.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return null;
        }
    }

    public async Task<bool> Delete(ReactionReply reaction)
    {
        try
        {
            _context.ReactionReplies.Remove(reaction);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return false;
        }
    }

    public List<ReactionReply>? Filter(Func<ReactionReply, bool> func)
    {
        return _context.ReactionReplies.Where(func).ToList();
    }

    public async Task<List<ReactionReply>?> GetAllAsync()
    {
        return await _context.ReactionReplies.ToListAsync();
    }

    public Task<ReactionReply?> GetByIdAsync(int id)
    {
        return null!;
    }

    public Task<bool> Update(ReactionReply Entity)
    {
        return null!;
    }
}
