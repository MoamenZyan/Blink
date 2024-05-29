using Microsoft.EntityFrameworkCore;


public class DatabaseContext : DbContext
{

    public DatabaseContext (DbContextOptions<DatabaseContext> options) : base (options){}

    public DbSet<User> Users {get; set;}
    public DbSet<Post> Posts {get; set;}
    public DbSet<Comment> Comments {get; set;}
    public DbSet<Reply> Replies{get; set;}
    public DbSet<ReactionPost> ReactionPosts {get; set;}
    public DbSet<ReactionComment> ReactionComments {get; set;}
    public DbSet<ReactionReply> ReactionReplies {get; set;}
    public DbSet<Group> Groups_ {get; set;}
    public DbSet<GroupAdmin> GroupAdmin {get; set;}
    public DbSet<GroupUser> GroupUser {get; set;}
    public DbSet<Story> Stories {get; set;}

    // Create PK out of non-prime attributes
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReactionPost>().HasKey(r => new {r.user_id, r.post_id}).HasName("PK_ReactionPost");
        modelBuilder.Entity<ReactionComment>().HasKey(r => new {r.user_id, r.comment_id}).HasName("PK_ReactionComment");
        modelBuilder.Entity<ReactionReply>().HasKey(r => new {r.user_id, r.reply_id}).HasName("PK_ReactionReply");
        modelBuilder.Entity<GroupAdmin>().HasKey(r => new {r.group_id, r.admin_id}).HasName("PK_GroupAdmin");
        modelBuilder.Entity<GroupUser>().HasKey(r => new {r.group_id, r.user_id}).HasName("PK_GroupUser");
        base.OnModelCreating(modelBuilder);
    }
}
