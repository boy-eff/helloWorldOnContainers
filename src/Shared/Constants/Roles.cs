using Microsoft.AspNetCore.Identity;

namespace Shared.Constants;

public static class Roles
{
    public static IdentityRole<int> UserRole = new IdentityRole<int>()
    {
        Id = 1,
        Name = "User",
        NormalizedName = "USER"
    };

    public static IdentityRole<int> ModeratorRole = new IdentityRole<int>()
    {
        Id = 2,
        Name = "Moderator",
        NormalizedName = "MODERATOR"
    };

    public static IdentityRole<int> AdminRole = new IdentityRole<int>()
    {
        Id = 3,
        Name = "SuperAdmin",
        NormalizedName = "SUPERADMIN"
    };
}