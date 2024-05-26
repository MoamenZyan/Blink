using System;
using Microsoft.EntityFrameworkCore;


public class DatabaseContext : DbContext
{

    public DatabaseContext (DbContextOptions<DatabaseContext> options) : base (options){}

    DbSet<User> Users;
    DbSet<Post> Posts;
    DbSet<Comment> Comments;
    DbSet<Reply> Replies;
    DbSet<ReactionPost> ReactionPosts;
    DbSet<ReactionComment> ReactionComments;
    DbSet<ReactionReply> ReactionReplies;
    DbSet<Group> Groups_;
    DbSet<GroupAdmin> GroupAdmin;
    DbSet<GroupUser> GroupUser;
    DbSet<Story> Stories;

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
