using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Cpnucleo.Infra.CrossCutting.Identity
{
    internal class ClaimsManager : IClaimsManager
    {
        public ClaimsPrincipal CreateClaimsPrincipal(string id, string token)
        {
            IEnumerable<Claim> claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, id),
                new Claim(ClaimTypes.Hash, token)
            };

            ClaimsIdentity identities = new ClaimsIdentity(claims, "Cookie");

            return new ClaimsPrincipal(new[] { identities });
        }

        public string ReadClaimsPrincipal(ClaimsPrincipal principal, string type)
        {
            return principal.Claims.SingleOrDefault(x => x.Type == type)?.Value;
        }
    }
}
