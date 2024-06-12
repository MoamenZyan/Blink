
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Configure Comment Entity
public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        // Table Name
        builder.ToTable("Comments");

        // Configure Primary Key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        // Configure The Rest Non-Prime Fields
        builder.Property(p => p.Photo)
            .HasColumnType("VARCHAR")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(p => p.Content)
            .HasColumnType("VARCHAR")
            .HasMaxLength(4096)
            .IsRequired(false);

        // Relation With User
        builder.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);
        
        // Relation With Post
        builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);
    }
}
