using Microsoft.EntityFrameworkCore;

public static class DB
{
    // Creating Tables On Application Startup
    public static void Init(IServiceProvider serviceProvider)
    {
        var DbContext = serviceProvider.GetRequiredService<DatabaseContext>();
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.UsersTable);
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.PostsTable);
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.CommentsTable);
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.RepliesTable);
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.StoriesTable);
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.GroupsTable);
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.GroupAdminTable);
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.GroupUserTable);
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.ReactionsPostsTable);
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.ReactionsCommentsTable);
        DbContext.Database.ExecuteSqlRaw(DatabaseSchema.ReactionsRepliesTable);
    }
}
