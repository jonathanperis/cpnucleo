using System.Security.Claims;

namespace Cpnucleo.Pages.Authentication
{
    public interface IClaimsManager
    {
        ClaimsPrincipal CreateClaimsPrincipal(string type, string value);

        string ReadClaimsPrincipal(ClaimsPrincipal principal, string type);
    }
}
