using System.Security.Claims;

namespace Cpnucleo.RazorPages.Services;

public static class ClaimsService
{
    public static ClaimsPrincipal CreateClaimsPrincipal(IEnumerable<Claim> claims)
    {
        ClaimsIdentity identities = new ClaimsIdentity(claims, "Cookie");

        return new ClaimsPrincipal(new[] { identities });
    }

    public static string ReadClaimsPrincipal(ClaimsPrincipal principal, string type)
    {
        return principal.Claims.SingleOrDefault(x => x.Type == type)?.Value;
    }
}
