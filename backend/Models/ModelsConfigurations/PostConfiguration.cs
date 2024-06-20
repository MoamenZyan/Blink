
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Configure Post Entity
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        // Table Name
        builder.ToTable("Posts");

        // Configure Primary Key
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        // Configure The Rest Non-Prime Fields
        builder.Property(p => p.Photo)
            .HasColumnType("VARCHAR")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(p => p.Caption)
            .HasColumnType("VARCHAR")
            .HasMaxLength(4069)
            .IsRequired(false);

        builder.Property(p => p.Privacy)
            .HasColumnType("VARCHAR")
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(p => p.Type)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50)
                .IsRequired();

        // Relation With User
        builder.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);
    }
}
