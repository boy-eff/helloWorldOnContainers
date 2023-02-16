using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data;

public class AuthDbContext: IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public AuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedRoles(builder);
        SeedUsers(builder);
        SeedUserRoles(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole<int>>().HasData(new List<IdentityRole<int>>()
        {
            new IdentityRole<int>()
            {
                Id = 1,
                Name = "User",
                NormalizedName = "USER"
            },
            new IdentityRole<int>()
            {
                Id = 2,
                Name = "Moderator",
                NormalizedName = "MODERATOR"
            },
            new IdentityRole<int>()
            {
                Id = 3,
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            }
        });
    }

    private static void SeedUsers(ModelBuilder builder)
    {
        var hasher = new PasswordHasher<AppUser>();
        var admin = new AppUser()
        {
            Id = 1,
            UserName = "superadmin",
            NormalizedUserName = "SUPERADMIN",
            SecurityStamp = Guid.NewGuid().ToString(),
            PasswordHash = hasher.HashPassword(null, "superadmin")
        };
        builder.Entity<AppUser>().HasData(admin);
    }

    private static void SeedUserRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int>()
            {
                RoleId = 3,
                UserId = 1
            });
    }
}