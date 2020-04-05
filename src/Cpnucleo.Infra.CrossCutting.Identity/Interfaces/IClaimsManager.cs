using System.Collections.Generic;
using System.Security.Claims;

namespace Cpnucleo.Infra.CrossCutting.Identity.Interfaces
{
    public interface IClaimsManager
    {
        ClaimsPrincipal CreateClaimsPrincipal(string id, string token);

        string ReadClaimsPrincipal(ClaimsPrincipal principal, string type);
    }
}
