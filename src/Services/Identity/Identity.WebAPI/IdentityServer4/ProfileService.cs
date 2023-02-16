using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.WebAPI.IdentityServer4;

public class ProfileService : IProfileService
{
    private readonly UserManager<AppUser> _userManager;

    public ProfileService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);

        var roles = await _userManager.GetRolesAsync(user);

        var claims = roles.Select(role => new Claim("roles", role));
        
        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        
        context.IsActive = (user != null);
    }
}