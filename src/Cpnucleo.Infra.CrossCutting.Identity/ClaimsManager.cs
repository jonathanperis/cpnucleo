using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using System.Linq;
using System.Security.Claims;

namespace Cpnucleo.Infra.CrossCutting.Identity
{
    public class ClaimsManager : IClaimsManager
    {
        public ClaimsPrincipal CreateClaimsPrincipal(string id, string token)
        {
            ClaimsIdentity identities = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.PrimarySid, id),
                new Claim(ClaimTypes.Hash, token),
            });

            return new ClaimsPrincipal(new[] { identities });
        }

        public string ReadClaimsPrincipal(ClaimsPrincipal principal, string type)
        {
            return principal.Claims.SingleOrDefault(x => x.Type == type)?.Value;
        }
    }
}
