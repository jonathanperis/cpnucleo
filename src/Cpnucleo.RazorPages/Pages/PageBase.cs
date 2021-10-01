using Cpnucleo.RazorPages.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Pages;

public class PageBase : PageModel
{
    public string Token => ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.Hash);
}
