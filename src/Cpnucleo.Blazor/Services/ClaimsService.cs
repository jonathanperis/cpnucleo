using System.Security.Claims;

namespace Cpnucleo.Blazor.Services;

internal static class ClaimsService
{
    public static ClaimsPrincipal CreateClaimsPrincipal(IEnumerable<Claim> claims)
    {
        ClaimsIdentity identities = new(claims, "Cookie");

        return new ClaimsPrincipal(new[] { identities });
    }

    public static string ReadClaimsPrincipal(ClaimsPrincipal principal, string type)
    {
        return principal.Claims.SingleOrDefault(x => x.Type == type)?.Value;
    }
}