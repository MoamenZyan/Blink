using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


// Configure Reaction On Post Entity
public class ReactionPostConfiguration : IEntityTypeConfiguration<ReactionPost>
{
    public void Configure(EntityTypeBuilder<ReactionPost> builder)
    {
        // Table Name
        builder.ToTable("ReactionPosts");

        builder.HasKey(rp => new {rp.PostId, rp.UserId});

        // Relation With Post
        builder.HasOne(rp => rp.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(rp => rp.PostId);

        // Relation With User
        builder.HasOne(rp => rp.User)
                .WithMany(u => u.ReactionPosts)
                .HasForeignKey(rp => rp.UserId);
    }
}

// Configure Reaction On Comment Entity
public class ReactionCommentConfiguration : IEntityTypeConfiguration<ReactionComment>
{
    public void Configure(EntityTypeBuilder<ReactionComment> builder)
    {
        // Table Name
        builder.ToTable("ReactionComments");

        builder.HasKey(rp => new {rp.CommentId, rp.UserId});

        // Relation With Comment
        builder.HasOne(rp => rp.Comment)
                .WithMany(p => p.Reactions)
                .HasForeignKey(rp => rp.CommentId);

        // Relation With User
        builder.HasOne(rc => rc.User)
                .WithMany(u => u.ReactionComments)
                .HasForeignKey(rc => rc.UserId);
    }
}

// Configure Reaction On Reply Entity
public class ReactionReplyConfiguration : IEntityTypeConfiguration<ReactionReply>
{
    public void Configure(EntityTypeBuilder<ReactionReply> builder)
    {
        // Table Name
        builder.ToTable("ReactionReplies");

        builder.HasKey(rp => new {rp.ReplyId, rp.UserId});

        // Relation With Reply
        builder.HasOne(rp => rp.Reply)
                .WithMany(p => p.Reactions)
                .HasForeignKey(rp => rp.ReplyId);

        // Relation With User
        builder.HasOne(rr => rr.User)
                .WithMany(u => u.ReactionReplies)
                .HasForeignKey(rr => rr.UserId);
    }
}