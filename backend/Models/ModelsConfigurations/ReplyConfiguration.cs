
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Configure Reply Entity
public class ReplyConfiguration : IEntityTypeConfiguration<Reply>
{
    public void Configure(EntityTypeBuilder<Reply> builder)
    {
        // Table Name
        builder.ToTable("Replies");
        
        // Configure Primary Key
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        // Configure The Rest Non-Prime Fields
        builder.Property(r => r.Photo)
            .HasColumnType("VARCHAR")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(r => r.Content)
            .HasColumnType("VARCHAR")
            .HasMaxLength(4096)
            .IsRequired(false);



        // Relation With User
        builder.HasOne(r => r.User)
                .WithMany(u => u.Replies)
                .HasForeignKey(r => r.UserId);

        // Relation With Comment
        builder.HasOne(r => r.Comment)
                .WithMany(c => c.Replies)
                .HasForeignKey(r => r.CommentId);

        // Relation With Post
        builder.HasOne(r => r.Post)
                .WithMany(p => p.Replies)
                .HasForeignKey(r => r.PostId);
    }
}
