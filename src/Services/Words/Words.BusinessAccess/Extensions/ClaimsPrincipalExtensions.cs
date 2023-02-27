using System.Security.Claims;

namespace Words.BusinessAccess.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }
        var claim = principal.FindFirst(x => x.Type.Equals("sub"));
        return claim != null ? int.Parse(claim.Value) : 0;
    }
}