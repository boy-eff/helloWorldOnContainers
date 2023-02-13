using AuthService.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data
{
    public class AuthDbContext: IdentityDbContext<AppUser>
    {
        
    }
}