
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Configure Group Entity
public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        // Table Name
        builder.ToTable("Groups_");

        // Configure Primary Key
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).ValueGeneratedOnAdd();

        // Configure The Rest Non-Prime Fields
        builder.Property(p => p.Photo)
            .HasColumnType("VARCHAR")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(p => p.Description)
            .HasColumnType("VARCHAR")
            .HasMaxLength(4096)
            .IsRequired(false);

        builder.Property(p => p.Privacy)
            .HasColumnType("VARCHAR")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Name)
            .HasColumnType("VARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        // Relation With User (Owner)
        builder.HasOne(u => u.Owner)
                .WithMany(u => u.OwnedGroups)
                .HasForeignKey(u => u.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}


// Configure Group Admin Entity
public class GroupAdminConfiguration : IEntityTypeConfiguration<GroupAdmin>
{
    public void Configure(EntityTypeBuilder<GroupAdmin> builder)
    {
        // Table Name
        builder.ToTable("GroupAdmins");

        // Configure Primary Key
        builder.HasKey(ga => new {ga.GroupId, ga.UserId});

        // Relation With Group
        builder.HasOne(ga => ga.Group)
                .WithMany(g => g.Admins)
                .HasForeignKey(ga => ga.GroupId);

        // Relation With Admin
        builder.HasOne(ga => ga.User)
                .WithMany(u => u.AdminOfGroups)
                .HasForeignKey(ga => ga.UserId);
    }
}


// Configure Group User Entity
public class GroupUserConfiguration : IEntityTypeConfiguration<GroupUser>
{
    public void Configure(EntityTypeBuilder<GroupUser> builder)
    {
        // Table Name
        builder.ToTable("GroupUsers");

        // Configure Primary Key
        builder.HasKey(gu => new {gu.GroupId, gu.UserId});

        // Relation With Group
        builder.HasOne(gu => gu.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(gu => gu.GroupId);

        // Relation With User
        builder.HasOne(gu => gu.User)
                .WithMany(u => u.MemberOfGroups)
                .HasForeignKey(gu => gu.UserId);
    }
}