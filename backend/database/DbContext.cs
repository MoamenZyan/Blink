using Microsoft.EntityFrameworkCore;


public class DatabaseContext : DbContext
{

    public DatabaseContext (DbContextOptions<DatabaseContext> options) : base (options)
    {
        this.ChangeTracker.LazyLoadingEnabled = false;
    }

    public DbSet<User> Users {get; set;}
    public DbSet<Post> Posts {get; set;}
    public DbSet<Comment> Comments {get; set;}
    public DbSet<Reply> Replies{get; set;}
    public DbSet<ReactionPost> ReactionPosts {get; set;}
    public DbSet<ReactionComment> ReactionComments {get; set;}
    public DbSet<ReactionReply> ReactionReplies {get; set;}
    public DbSet<Group> Groups_ {get; set;}
    public DbSet<GroupAdmin> GroupAdmins {get; set;}
    public DbSet<GroupUser> GroupUsers {get; set;}
    public DbSet<Story> Stories {get; set;}

    // Load Configuration From Assembly
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
