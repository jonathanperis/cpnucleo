using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Cpnucleo.Infra.CrossCutting.Identity
{
    public class ClaimsManager : IClaimsManager
    {
        public ClaimsPrincipal CreateClaimsPrincipal(IEnumerable<Claim> claims)
        {
            ClaimsIdentity identities = new ClaimsIdentity(claims, "Dummy");
            return new ClaimsPrincipal(new[] { identities });
        }

        public string ReadClaimsPrincipal(ClaimsPrincipal principal, string type)
        {
            return principal.Claims.SingleOrDefault(x => x.Type == type)?.Value;
        }
    }
}
