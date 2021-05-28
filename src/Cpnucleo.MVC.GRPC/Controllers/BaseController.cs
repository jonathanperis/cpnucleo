using Cpnucleo.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cpnucleo.MVC.Controllers
{
    public class BaseController : Controller
    {
        public string Token => ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.Hash);
    }
}