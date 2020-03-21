using System.Collections.Generic;
using System.Security.Claims;

namespace Cpnucleo.Infra.CrossCutting.Identity.Interfaces
{
    public interface IClaimsManager
    {
        ClaimsPrincipal CreateClaimsPrincipal(IEnumerable<Claim> claims);

        string ReadClaimsPrincipal(ClaimsPrincipal principal, string type);
    }
}
