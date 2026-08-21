using System.Runtime.CompilerServices;
using HospitalSaoJose.Domain.Entities;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("WebApi.Tests")]

namespace HospitalSaoJose.Infrastructure.DataAccess;

internal class HospitalSaoJoseDbContext : DbContext
{
    public HospitalSaoJoseDbContext(DbContextOptions<HospitalSaoJoseDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<ProfileRole> ProfileRoles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.Property(role => role.Id).ValueGeneratedNever();
            entity.Property(role => role.Key).HasMaxLength(100).IsRequired();
            entity.Property(role => role.Name).HasMaxLength(150).IsRequired();
            entity.Property(role => role.Description).HasMaxLength(500);
            entity.HasIndex(role => role.Key).IsUnique().HasFilter("\"Active\" = true");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.ToTable("Profiles");
            entity.Property(profile => profile.Id).ValueGeneratedNever();
            entity.Property(profile => profile.Name).HasMaxLength(100).IsRequired();
            entity.Property(profile => profile.Description).HasMaxLength(500);
            entity.HasIndex(profile => profile.Name).IsUnique().HasFilter("\"Active\" = true");
        });

        modelBuilder.Entity<ProfileRole>(entity =>
        {
            entity.ToTable("ProfileRoles");
            entity.HasKey(profileRole => new { profileRole.ProfileId, profileRole.RoleId });
            entity.HasOne(profileRole => profileRole.Profile)
                .WithMany(profile => profile.ProfileRoles)
                .HasForeignKey(profileRole => profileRole.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(profileRole => profileRole.Role)
                .WithMany(role => role.ProfileRoles)
                .HasForeignKey(profileRole => profileRole.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(user => user.Id).ValueGeneratedNever();
            entity.Property(user => user.Name).HasMaxLength(255).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(255).IsRequired();
            entity.Property(user => user.Password).HasMaxLength(2000).IsRequired();
            entity.HasIndex(user => user.Email).IsUnique().HasFilter("\"Active\" = true");
            entity.HasOne(user => user.Profile)
                .WithMany(profile => profile.Users)
                .HasForeignKey(user => user.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.Property(category => category.Id).ValueGeneratedNever();
            entity.Property(category => category.Name).HasMaxLength(150).IsRequired();
            entity.Property(category => category.Slug).HasMaxLength(60).IsRequired();
            entity.HasIndex(category => category.Slug).IsUnique().HasFilter("\"Active\" = true");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Documents");
            entity.Property(document => document.Id).ValueGeneratedNever();
            entity.Property(document => document.Title).HasMaxLength(255).IsRequired();
            entity.Property(document => document.Description).HasMaxLength(8000).IsRequired();
            entity.Property(document => document.ExternalLink).HasMaxLength(2000);
            entity.Property(document => document.OriginalFileName).HasMaxLength(255).IsRequired();
            entity.Property(document => document.StoredFileName).HasMaxLength(100).IsRequired();
            entity.Property(document => document.ContentType).HasMaxLength(100).IsRequired();
            entity.HasIndex(document => document.PublicationDate);
            entity.HasOne(document => document.Category)
                .WithMany(category => category.Documents)
                .HasForeignKey(document => document.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
