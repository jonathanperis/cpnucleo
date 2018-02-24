using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace dotnet_cpnucleo_pages.Authentication
{
    public static class ClaimsManager
    {
        public static ClaimsPrincipal CreateClaimsPrincipal(string type, string value)
        {
            IEnumerable<Claim> claims = CreateClaims(type, value);

            var identities = new ClaimsIdentity(claims, "Dummy");
            return new ClaimsPrincipal(new[] { identities });
        }  
        
        public static string ReadClaimsPrincipal(ClaimsPrincipal principal, string type)
        {          
            return principal.Claims.SingleOrDefault(x => x.Type == type)?.Value;
        }

        private static IEnumerable<Claim> CreateClaims(string type, string value)
        {
            return new []
            {
                new Claim(type, value)
            };
        }              
    }
}
