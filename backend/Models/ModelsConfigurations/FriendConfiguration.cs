
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FriendConfiguration : IEntityTypeConfiguration<Friends>
{
    public void Configure(EntityTypeBuilder<Friends> builder)
    {
        // Table Name
        builder.ToTable("Friends");

        // Configure Primary Key
        builder.HasKey(f => new {f.UserId1, f.UserId2});
        
        // Configuration With User
        builder.HasOne(f => f.User1)
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.UserId1)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.User2)
                .WithMany(u => u.FriendOf)
                .HasForeignKey(f => f.UserId2)
                .OnDelete(DeleteBehavior.Restrict);
    }
}