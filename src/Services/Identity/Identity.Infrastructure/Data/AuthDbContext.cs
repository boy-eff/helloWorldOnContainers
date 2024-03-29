using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;

namespace Identity.Infrastructure.Data;

public class AuthDbContext: IdentityDbContext<AppUser, IdentityRole<int>, int>, IAuthDbContext
{
    public AuthDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.AddInboxStateEntity();
        builder.AddOutboxMessageEntity();
        builder.AddOutboxStateEntity();
        
        SeedRoles(builder);
        SeedUsers(builder);
        SeedUserRoles(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole<int>>().HasData(new List<IdentityRole<int>>()
        {
            Shared.Constants.Roles.UserRole,
            Shared.Constants.Roles.ModeratorRole,
            Shared.Constants.Roles.AdminRole
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

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}