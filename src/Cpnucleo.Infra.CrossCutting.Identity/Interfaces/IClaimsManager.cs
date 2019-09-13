using System.Security.Claims;

namespace Cpnucleo.Infra.CrossCutting.Identity.Interfaces
{
    public interface IClaimsManager
    {
        ClaimsPrincipal CreateClaimsPrincipal(string type, string value);

        string ReadClaimsPrincipal(ClaimsPrincipal principal, string type);
    }
}
