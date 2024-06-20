
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PostNotificationConfiguration : IEntityTypeConfiguration<PostNotification>
{
    public void Configure(EntityTypeBuilder<PostNotification> builder)
    {
        // Table Name
        builder.ToTable("PostNotifications");

        // Configure Primary Key
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).ValueGeneratedOnAdd();

        // Configure Non-Prime Fields
        builder.Property(n => n.Message)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();
        
        // Relation With User
        builder.HasOne(np => np.User)
                .WithMany(u => u.PostNotifications)
                .HasForeignKey(np => np.UserId);

        // Relation With Post
        builder.HasOne(np => np.Post)
                .WithMany(p => p.PostNotifications)
                .HasForeignKey(np => np.PostId);
    }
}


public class FriendRequestNotificationConfiguration : IEntityTypeConfiguration<FriendRequestNotification>
{
    public void Configure(EntityTypeBuilder<FriendRequestNotification> builder)
    {
        // Table Name
        builder.ToTable("FriendRequestNotifications");

        // Configure Primary Key
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).ValueGeneratedOnAdd();

        // Relation With User
        builder.HasOne(np => np.User)
                .WithMany(u => u.FriendRequestNotifications)
                .HasForeignKey(np => np.UserId);
    }
}
