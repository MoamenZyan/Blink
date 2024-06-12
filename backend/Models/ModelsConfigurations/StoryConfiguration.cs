using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Configure Story Entity
public class StoryConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        // Table Name
        builder.ToTable("Stories");

        // Configure Primary Key
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        // Configure The Rest Non-Prime Fields
        builder.Property(s => s.Photo)
            .HasColumnType("VARCHAR")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(s => s.Content)
        .HasColumnType("VARCHAR")
        .HasMaxLength(1024)
        .IsRequired(false);

        builder.Property(s => s.Privacy).HasColumnType("VARCHAR").HasMaxLength(20);

        // Relation With User
        builder.HasOne(s => s.User)
                .WithMany(u => u.Stories)
                .HasForeignKey(s => s.UserId);
    }
}