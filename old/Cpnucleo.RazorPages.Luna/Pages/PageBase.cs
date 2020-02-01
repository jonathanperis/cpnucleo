using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Luna.Pages
{
    public class PageBase : PageModel
    {
        private readonly IClaimsManager _claimsManager;

        public PageBase(IClaimsManager claimsManager)
        {
            _claimsManager = claimsManager;
        }

        public string Token => _claimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.Hash);
    }
}
