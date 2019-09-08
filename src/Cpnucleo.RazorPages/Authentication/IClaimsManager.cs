using System.Security.Claims;

namespace Cpnucleo.RazorPages.Authentication
{
    public interface IClaimsManager
    {
        ClaimsPrincipal CreateClaimsPrincipal(string type, string value);

        string ReadClaimsPrincipal(ClaimsPrincipal principal, string type);
    }
}
