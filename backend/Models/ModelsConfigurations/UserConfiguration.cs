using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Configure User Entity
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table Name
        builder.ToTable("Users");

        // Configure Primary Key
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        // Configure The Rest Of Non-Prime Fields
        builder.Property(u => u.Photo)
            .HasColumnType("VARCHAR")
            .HasMaxLength(100);

        builder.Property(u => u.Username)
            .HasColumnType("VARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Password)
            .HasColumnType("VARCHAR")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasColumnType("VARCHAR")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasColumnType("VARCHAR")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnType("VARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Privacy)
            .HasColumnType("VARCHAR")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.Headline)
                .HasColumnType("VARCHAR")
                .HasMaxLength(144)
                .IsRequired(false);

        builder.Property(u => u.About)
                .HasColumnType("VARCHAR")
                .HasMaxLength(1024)
                .IsRequired(false);
        
        builder.Property(u => u.Country)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired(false);

        builder.Property(u => u.City)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired(false);

        builder.HasIndex(u => u.Username).IsUnique();
    }
}