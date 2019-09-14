using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Cpnucleo.Infra.CrossCutting.Identity
{
    public class ClaimsManager : IClaimsManager
    {
        public ClaimsPrincipal CreateClaimsPrincipal(string type, string value)
        {
            IEnumerable<Claim> claims = CreateClaims(type, value);

            ClaimsIdentity identities = new ClaimsIdentity(claims, "Dummy");
            return new ClaimsPrincipal(new[] { identities });
        }

        public string ReadClaimsPrincipal(ClaimsPrincipal principal, string type)
        {
            return principal.Claims.SingleOrDefault(x => x.Type == type)?.Value;
        }

        private static IEnumerable<Claim> CreateClaims(string type, string value)
        {
            return new[]
            {
                new Claim(type, value)
            };
        }
    }
}
